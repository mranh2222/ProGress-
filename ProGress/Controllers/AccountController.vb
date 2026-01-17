Imports System.Web.Mvc
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class AccountController
    Inherits Controller

    Private ReadOnly _connectionString As String = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProGressDB;Integrated Security=True"
    Private ReadOnly _apiUrl As String = ConfigurationManager.AppSettings("ExternalServerApiUrl")
    Private ReadOnly _httpClient As New HttpClient()

    ' GET: Account/Login
    Function Login() As ActionResult
        Return View()
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
                        Session("FullName") = If(userInfo.ContainsKey("fullName"), userInfo("fullName").ToString(), If(userInfo.ContainsKey("name"), userInfo("name").ToString(), username))
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
End Class




