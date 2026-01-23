Imports System.Web.Mvc
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports System.Security.Cryptography
Imports System.Web
Imports System.Linq

Public Class AccountController
    Inherits Controller

    Private ReadOnly _connectionString As String
    Private ReadOnly _apiUrl As String = ConfigurationManager.AppSettings("ExternalServerApiUrl")
    Private ReadOnly _httpClient As New HttpClient()
    
    ' SSO OAuth Configuration
    Private ReadOnly _oauthAuthorizeUrl As String = ConfigurationManager.AppSettings("KCS.OAuth.AuthorizeUrl")
    Private ReadOnly _oauthTokenUrl As String = ConfigurationManager.AppSettings("KCS.OAuth.TokenUrl")
    Private ReadOnly _oauthUserInfoUrl As String = ConfigurationManager.AppSettings("KCS.OAuth.UserInfoUrl")
    Private ReadOnly _oauthClientId As String = ConfigurationManager.AppSettings("KCS.OAuth.ClientId")
    Private ReadOnly _oauthClientSecret As String = ConfigurationManager.AppSettings("KCS.OAuth.ClientSecret")
    Private ReadOnly _oauthRedirectUri As String = ConfigurationManager.AppSettings("KCS.OAuth.RedirectUri")
    Private ReadOnly _oauthScope As String = ConfigurationManager.AppSettings("KCS.OAuth.Scope")
    
    Public Sub New()
        Dim configConnectionString = ConfigurationManager.ConnectionStrings("DefaultConnection")
        If configConnectionString IsNot Nothing AndAlso Not String.IsNullOrEmpty(configConnectionString.ConnectionString) Then
            _connectionString = configConnectionString.ConnectionString
        Else
            ' Fallback cho local development
            _connectionString = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProGressDB;Integrated Security=True"
        End If
    End Sub

    ' GET: Account/Login - SSO Flow: Redirect sang KCS
    Function Login() As ActionResult
        ' Kiểm tra nếu đã đăng nhập thì redirect về Dashboard
        If Session("UserId") IsNot Nothing Then
            Return RedirectToAction("Index", "Dashboard")
        End If
        
        ' Kiểm tra xem có config SSO không (nếu chưa có thì fallback về form login cũ)
        If String.IsNullOrEmpty(_oauthAuthorizeUrl) OrElse String.IsNullOrEmpty(_oauthClientId) OrElse _oauthClientId = "YOUR_CLIENT_ID_HERE" Then
            ' Chưa config SSO, hiển thị form login cũ
            Return View()
        End If
        
        ' SSO Flow: Generate state (CSRF protection) và redirect sang KCS
        Dim state = GenerateRandomString(32)
        Session("OAuthState") = state
        
        Dim authorizeUrl = String.Format("{0}?client_id={1}&redirect_uri={2}&response_type=code&scope={3}&state={4}",
            _oauthAuthorizeUrl,
            HttpUtility.UrlEncode(_oauthClientId),
            HttpUtility.UrlEncode(_oauthRedirectUri),
            HttpUtility.UrlEncode(_oauthScope),
            HttpUtility.UrlEncode(state))
        
        Return Redirect(authorizeUrl)
    End Function
    
    ' GET: Account/Callback - Nhận code từ KCS sau khi user đăng nhập
    Async Function Callback(code As String, state As String, errorParam As String) As System.Threading.Tasks.Task(Of ActionResult)
        ' Kiểm tra lỗi từ KCS
        If Not String.IsNullOrEmpty(errorParam) Then
            TempData("ErrorMessage") = "Đăng nhập thất bại: " & errorParam
            Return RedirectToAction("Login")
        End If
        
        ' Validate state (CSRF protection)
        Dim savedState = Session("OAuthState")
        If String.IsNullOrEmpty(savedState) OrElse savedState <> state Then
            TempData("ErrorMessage") = "Lỗi bảo mật: State không khớp."
            Return RedirectToAction("Login")
        End If
        Session.Remove("OAuthState")
        
        ' Kiểm tra có code không
        If String.IsNullOrEmpty(code) Then
            TempData("ErrorMessage") = "Không nhận được authorization code từ KCS."
            Return RedirectToAction("Login")
        End If
        
        Try
            ' Bước 1: Đổi code lấy access_token (server-to-server)
            Dim tokenResponse = Await ExchangeCodeForToken(code)
            If tokenResponse Is Nothing OrElse Not tokenResponse.ContainsKey("access_token") Then
                TempData("ErrorMessage") = "Không thể lấy access token từ KCS."
                Return RedirectToAction("Login")
            End If
            
            Dim accessToken = tokenResponse("access_token").ToString()
            
            ' Bước 2: Dùng access_token để lấy thông tin user
            Dim userInfo = Await GetUserInfo(accessToken)
            If userInfo Is Nothing Then
                TempData("ErrorMessage") = "Không thể lấy thông tin người dùng từ KCS."
                Return RedirectToAction("Login")
            End If
            
            ' Bước 3: Clone/Sync user về DB local và tạo session
            Dim username = If(userInfo.ContainsKey("username"), userInfo("username").ToString(),
                             If(userInfo.ContainsKey("email"), userInfo("email").ToString(), ""))
            
            If String.IsNullOrEmpty(username) Then
                TempData("ErrorMessage") = "KCS không trả về thông tin username/email."
                Return RedirectToAction("Login")
            End If
            
            Using conn As New SqlConnection(_connectionString)
                Await conn.OpenAsync()
                
                ' Kiểm tra user đã tồn tại chưa
                Dim checkQuery = "SELECT Id FROM Users WHERE Username = @Username"
                Dim cmdCheck As New SqlCommand(checkQuery, conn)
                cmdCheck.Parameters.AddWithValue("@Username", username)
                Dim existingUserId = Await cmdCheck.ExecuteScalarAsync()
                
                If existingUserId Is Nothing Then
                    ' Thêm user mới
                    Dim insertQuery = "INSERT INTO Users (Username, Email, FullName, CreatedDate) VALUES (@Username, @Email, @FullName, GETDATE()); SELECT SCOPE_IDENTITY();"
                    Dim cmdInsert As New SqlCommand(insertQuery, conn)
                    cmdInsert.Parameters.AddWithValue("@Username", username)
                    cmdInsert.Parameters.AddWithValue("@Email", If(userInfo.ContainsKey("email"), userInfo("email").ToString(), ""))
                    cmdInsert.Parameters.AddWithValue("@FullName", If(userInfo.ContainsKey("fullName"), userInfo("fullName").ToString(),
                                                                     If(userInfo.ContainsKey("name"), userInfo("name").ToString(), username)))
                    existingUserId = Await cmdInsert.ExecuteScalarAsync()
                Else
                    ' Cập nhật thông tin user
                    Dim updateQuery = "UPDATE Users SET Email = @Email, FullName = @FullName WHERE Username = @Username"
                    Dim cmdUpdate As New SqlCommand(updateQuery, conn)
                    cmdUpdate.Parameters.AddWithValue("@Username", username)
                    cmdUpdate.Parameters.AddWithValue("@Email", If(userInfo.ContainsKey("email"), userInfo("email").ToString(), ""))
                    cmdUpdate.Parameters.AddWithValue("@FullName", If(userInfo.ContainsKey("fullName"), userInfo("fullName").ToString(),
                                                                     If(userInfo.ContainsKey("name"), userInfo("name").ToString(), username)))
                    Await cmdUpdate.ExecuteNonQueryAsync()
                End If
                
                ' Lưu session
                Session("UserId") = existingUserId
                Session("Username") = username
                Dim fullName = If(userInfo.ContainsKey("fullName"), userInfo("fullName").ToString(),
                                 If(userInfo.ContainsKey("name"), userInfo("name").ToString(), username))
                Session("FullName") = fullName
                
                TempData("WelcomeMessage") = "Chào mừng " & fullName & "!"
            End Using
            
            Return RedirectToAction("Index", "Dashboard")
            
        Catch ex As Exception
            TempData("ErrorMessage") = "Lỗi xử lý đăng nhập SSO: " & ex.Message
            System.Diagnostics.Debug.WriteLine("SSO Callback Error: " & ex.Message)
            Return RedirectToAction("Login")
        End Try
    End Function
    
    ' Helper: Đổi authorization code lấy access token
    Private Async Function ExchangeCodeForToken(code As String) As System.Threading.Tasks.Task(Of Dictionary(Of String, Object))
        Try
            Dim tokenData = New List(Of KeyValuePair(Of String, String)) From {
                New KeyValuePair(Of String, String)("grant_type", "authorization_code"),
                New KeyValuePair(Of String, String)("code", code),
                New KeyValuePair(Of String, String)("redirect_uri", _oauthRedirectUri),
                New KeyValuePair(Of String, String)("client_id", _oauthClientId),
                New KeyValuePair(Of String, String)("client_secret", _oauthClientSecret)
            }
            
            Dim content = New FormUrlEncodedContent(tokenData)
            Dim response = Await _httpClient.PostAsync(_oauthTokenUrl, content)
            
            If response.IsSuccessStatusCode Then
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                Return JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(responseContent)
            Else
                Dim errorContent = Await response.Content.ReadAsStringAsync()
                System.Diagnostics.Debug.WriteLine("Token Exchange Error: " & errorContent)
                Return Nothing
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("ExchangeCodeForToken Error: " & ex.Message)
            Return Nothing
        End Try
    End Function
    
    ' Helper: Lấy thông tin user từ access token
    Private Async Function GetUserInfo(accessToken As String) As System.Threading.Tasks.Task(Of Dictionary(Of String, Object))
        Try
            Dim request = New HttpRequestMessage(HttpMethod.Get, _oauthUserInfoUrl)
            request.Headers.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken)
            
            Dim response = Await _httpClient.SendAsync(request)
            
            If response.IsSuccessStatusCode Then
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                Return JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(responseContent)
            Else
                Dim errorContent = Await response.Content.ReadAsStringAsync()
                System.Diagnostics.Debug.WriteLine("GetUserInfo Error: " & errorContent)
                Return Nothing
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("GetUserInfo Error: " & ex.Message)
            Return Nothing
        End Try
    End Function
    
    ' Helper: Generate random string cho state (CSRF protection)
    Private Function GenerateRandomString(length As Integer) As String
        Dim chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim random = New Random()
        Return New String(Enumerable.Range(0, length).Select(Function(i) chars(random.Next(chars.Length))).ToArray())
    End Function

    ' POST: Account/Login
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Login(ByVal username As String, ByVal password As String) As System.Threading.Tasks.Task(Of ActionResult)
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            ModelState.AddModelError("", "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.")
            Return View()
        End If

        Try
            ' 1. Gửi API đến máy chủ bên ngoài để xác thực
            Dim loginData = New With {
                .username = username,
                .password = password
            }
            
            Dim jsonContent = JsonConvert.SerializeObject(loginData)
            Dim content = New StringContent(jsonContent, Encoding.UTF8, "application/json")
            
            ' Thiết lập timeout cho HttpClient
            _httpClient.Timeout = TimeSpan.FromSeconds(30)
            
            Dim response = Await _httpClient.PostAsync(_apiUrl, content)
            
            If response.IsSuccessStatusCode Then
                ' 2. Đọc response từ máy chủ
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                Dim responseData = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(responseContent)
                
                ' 3. Lấy cookie từ response headers và chuyển tiếp về browser của người dùng
                Dim setCookieHeaders As IEnumerable(Of String) = Nothing
                If response.Headers.TryGetValues("Set-Cookie", setCookieHeaders) Then
                    For Each cookieHeader In setCookieHeaders
                        ' Thêm cookie vào response hiện tại để browser lưu lại
                        Response.Headers.Add("Set-Cookie", cookieHeader)
                    Next
                End If
                
                ' 4. Clone thông tin user từ máy chủ và lưu vào SQL Server local
                ' Giả sử API trả về object user chứa thông tin cơ bản
                Dim userInfo As Dictionary(Of String, Object) = Nothing
                
                If responseData.ContainsKey("user") Then
                    userInfo = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(responseData("user").ToString())
                End If

                If userInfo IsNot Nothing Then
                    Using conn As New SqlConnection(_connectionString)
                        Await conn.OpenAsync()
                        
                        ' Kiểm tra user đã tồn tại chưa bằng Username
                        Dim checkQuery = "SELECT Id FROM Users WHERE Username = @Username"
                        Dim cmdCheck As New SqlCommand(checkQuery, conn)
                        cmdCheck.Parameters.AddWithValue("@Username", username)
                        Dim existingUserId = Await cmdCheck.ExecuteScalarAsync()
                        
                        If existingUserId Is Nothing Then
                            ' Thêm user mới nếu chưa có (Clone từ server)
                            Dim insertQuery = "INSERT INTO Users (Username, Email, FullName, CreatedDate) VALUES (@Username, @Email, @FullName, GETDATE()); SELECT SCOPE_IDENTITY();"
                            Dim cmdInsert As New SqlCommand(insertQuery, conn)
                            cmdInsert.Parameters.AddWithValue("@Username", username)
                            cmdInsert.Parameters.AddWithValue("@Email", If(userInfo.ContainsKey("email"), userInfo("email").ToString(), ""))
                            cmdInsert.Parameters.AddWithValue("@FullName", If(userInfo.ContainsKey("fullName"), userInfo("fullName").ToString(), If(userInfo.ContainsKey("name"), userInfo("name").ToString(), username)))
                            existingUserId = Await cmdInsert.ExecuteScalarAsync()
                        Else
                            ' Cập nhật thông tin nếu đã tồn tại để đồng bộ với server
                            Dim updateQuery = "UPDATE Users SET Email = @Email, FullName = @FullName WHERE Username = @Username"
                            Dim cmdUpdate As New SqlCommand(updateQuery, conn)
                            cmdUpdate.Parameters.AddWithValue("@Username", username)
                            cmdUpdate.Parameters.AddWithValue("@Email", If(userInfo.ContainsKey("email"), userInfo("email").ToString(), ""))
                            cmdUpdate.Parameters.AddWithValue("@FullName", If(userInfo.ContainsKey("fullName"), userInfo("fullName").ToString(), If(userInfo.ContainsKey("name"), userInfo("name").ToString(), username)))
                            Await cmdUpdate.ExecuteNonQueryAsync()
                        End If
                        
                        ' 5. Lưu thông tin vào Session để quản lý đăng nhập local
                        Session("UserId") = existingUserId
                        Session("Username") = username
                        Dim fullName = If(userInfo.ContainsKey("fullName"), userInfo("fullName").ToString(), If(userInfo.ContainsKey("name"), userInfo("name").ToString(), username))
                        Session("FullName") = fullName

                        ' Thông báo chào mừng (hiển thị 1 lần sau redirect)
                        TempData("WelcomeMessage") = "Chào mừng " & fullName & "!"
                    End Using
                    
                    Return RedirectToAction("Index", "Dashboard")
                Else
                    ModelState.AddModelError("", "Đăng nhập thành công nhưng máy chủ không trả về thông tin người dùng.")
                End If
            Else
                Dim errorContent = Await response.Content.ReadAsStringAsync()
                ModelState.AddModelError("", "Đăng nhập thất bại: Thông tin tài khoản không chính xác hoặc lỗi từ máy chủ.")
            End If
        Catch ex As Exception
            ModelState.AddModelError("", "Lỗi kết nối máy chủ API: " & ex.Message)
            System.Diagnostics.Debug.WriteLine("Login Error: " & ex.Message)
        End Try
        
        Return View()
    End Function

    ' Logout
    Function Logout() As ActionResult
        Session.Clear()
        Response.Cookies.Clear()
        Return RedirectToAction("Login")
    End Function

    ' GET: Account/UserProfile
    Function UserProfile() As ActionResult
        If Session("UserId") Is Nothing Then
            Return RedirectToAction("Login")
        End If

        Dim userId As Integer = Convert.ToInt32(Session("UserId"))
        Dim model As New User()

        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Dim query = "SELECT Id, Username, Email, FullName, CreatedDate FROM Users WHERE Id = @Id"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@Id", userId)
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        model.Id = Convert.ToInt32(reader("Id"))
                        model.Username = reader("Username").ToString()
                        model.Email = reader("Email").ToString()
                        model.FullName = reader("FullName").ToString()
                        If reader("CreatedDate") IsNot DBNull.Value Then
                            model.CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                        End If
                    Else
                        Return RedirectToAction("Logout")
                    End If
                End Using
            End Using
        End Using

        Return View("Profile", model)
    End Function

    ' POST: Account/UserProfile
    <HttpPost>
    <ValidateAntiForgeryToken>
    Function UserProfile(model As User) As ActionResult
        If Session("UserId") Is Nothing Then
            Return RedirectToAction("Login")
        End If

        ' Bỏ validation các field không dùng ở màn hồ sơ
        ModelState.Remove("Username")
        ModelState.Remove("Password")

        If Not ModelState.IsValid Then
            Return View("Profile", model)
        End If

        Dim userId As Integer = Convert.ToInt32(Session("UserId"))
        Using conn As New SqlConnection(_connectionString)
            conn.Open()
            Dim updateQuery = "UPDATE Users SET Email = @Email, FullName = @FullName WHERE Id = @Id"
            Using cmd As New SqlCommand(updateQuery, conn)
                cmd.Parameters.AddWithValue("@Email", If(model.Email, ""))
                cmd.Parameters.AddWithValue("@FullName", If(model.FullName, ""))
                cmd.Parameters.AddWithValue("@Id", userId)
                cmd.ExecuteNonQuery()
            End Using
        End Using

        Session("FullName") = If(String.IsNullOrWhiteSpace(model.FullName), Session("Username"), model.FullName)
        TempData("WelcomeMessage") = "Đã cập nhật hồ sơ cá nhân."
        Return RedirectToAction("UserProfile")
    End Function
End Class




