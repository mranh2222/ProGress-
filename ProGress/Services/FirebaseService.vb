Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters
Imports System.Threading.Tasks
Imports System.Linq
Imports System.Collections.Generic
Imports System.Diagnostics

Public Class FirebaseService
    Private ReadOnly _baseUrl As String
    Private ReadOnly _httpClient As HttpClient
    Private ReadOnly _jsonSettings As JsonSerializerSettings

    Public Sub New()
        ' Cấu hình Firebase URL
        _baseUrl = System.Configuration.ConfigurationManager.AppSettings("FirebaseUrl")
        If String.IsNullOrEmpty(_baseUrl) Then
            _baseUrl = "https://your-project.firebaseio.com"
        End If

        ' Cấu hình HttpClient
        _httpClient = New HttpClient()
        _httpClient.BaseAddress = New Uri(_baseUrl)
        _httpClient.Timeout = TimeSpan.FromSeconds(30)

        ' Cấu hình JSON Serializer Settings
        _jsonSettings = New JsonSerializerSettings() With {
            .DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ",
            .NullValueHandling = NullValueHandling.Ignore,
            .DefaultValueHandling = DefaultValueHandling.Ignore,
            .Converters = New List(Of JsonConverter) From {
                New StringEnumConverter()
            }
        }
    End Sub

    ' Log error helper
    Private Sub LogError(methodName As String, ex As Exception, Optional additionalInfo As String = "")
        Try
            Dim errorMsg = $"[FirebaseService.{methodName}] Error: {ex.Message}"
            If Not String.IsNullOrEmpty(additionalInfo) Then
                errorMsg &= $" | Info: {additionalInfo}"
            End If
            If ex.InnerException IsNot Nothing Then
                errorMsg &= $" | Inner: {ex.InnerException.Message}"
            End If
            Debug.WriteLine(errorMsg)
            ' Có thể thêm logging vào file hoặc database nếu cần
        Catch
            ' Ignore logging errors
        End Try
    End Sub

    ' Lấy tất cả tasks
    Public Async Function GetAllTasksAsync() As Task(Of List(Of Task))
        Try
            Dim response = Await _httpClient.GetAsync("/tasks.json")
            response.EnsureSuccessStatusCode()
            Dim json = Await response.Content.ReadAsStringAsync()

            If String.IsNullOrEmpty(json) OrElse json = "null" Then
                Return New List(Of Task)
            End If

            Dim tasksDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Task))(json, _jsonSettings)
            If tasksDict Is Nothing Then
                Return New List(Of Task)
            End If

            Dim tasks = New List(Of Task)
            ' Lấy danh sách reference data để populate names
            Dim technicians = Await GetAllTechniciansAsync()
            Dim saleManagers = Await GetAllSaleManagersAsync()
            Dim software = Await GetAllSoftwareAsync()

            ' Gán Id từ key và đảm bảo các list được khởi tạo
            For Each kvp In tasksDict
                Dim task = kvp.Value
                If task IsNot Nothing Then
                    task.Id = kvp.Key
                    ' Đảm bảo các list được khởi tạo
                    If task.Attachments Is Nothing Then
                        task.Attachments = New List(Of String)
                    End If
                    If task.Images Is Nothing Then
                        task.Images = New List(Of String)
                    End If
                    If task.History Is Nothing Then
                        task.History = New List(Of TaskHistory)
                    End If

                    ' Populate AssignedToName nếu null
                    If String.IsNullOrEmpty(task.AssignedToName) AndAlso Not String.IsNullOrEmpty(task.AssignedTo) Then
                        Dim tech = technicians.FirstOrDefault(Function(t) t.Id = task.AssignedTo)
                        If tech IsNot Nothing Then
                            task.AssignedToName = tech.Name
                        End If
                    End If

                    ' Populate SaleManagerName nếu null
                    If String.IsNullOrEmpty(task.SaleManagerName) AndAlso Not String.IsNullOrEmpty(task.SaleManagerId) Then
                        Dim sale = saleManagers.FirstOrDefault(Function(s) s.Id = task.SaleManagerId)
                        If sale IsNot Nothing Then
                            task.SaleManagerName = sale.Name
                        End If
                    End If

                    ' Populate SoftwareName nếu null
                    If String.IsNullOrEmpty(task.SoftwareName) AndAlso Not String.IsNullOrEmpty(task.SoftwareId) Then
                        Dim soft = software.FirstOrDefault(Function(s) s.Id = task.SoftwareId)
                        If soft IsNot Nothing Then
                            task.SoftwareName = soft.Name
                        End If
                    End If

                    tasks.Add(task)
                End If
            Next

            Return tasks.OrderByDescending(Function(t) t.CreatedDate).ToList()
        Catch ex As Exception
            LogError("GetAllTasksAsync", ex)
            Return New List(Of Task)
        End Try
    End Function

    ' Lấy task theo ID
    Public Async Function GetTaskByIdAsync(taskId As String) As Task(Of Task)
        Try
            If String.IsNullOrEmpty(taskId) Then
                Return Nothing
            End If

            Dim response = Await _httpClient.GetAsync($"/tasks/{taskId}.json")
            response.EnsureSuccessStatusCode()
            Dim json = Await response.Content.ReadAsStringAsync()

            If String.IsNullOrEmpty(json) OrElse json = "null" Then
                Return Nothing
            End If

            Dim task = JsonConvert.DeserializeObject(Of Task)(json, _jsonSettings)
            If task IsNot Nothing Then
                task.Id = taskId
                ' Đảm bảo các list được khởi tạo
                If task.Attachments Is Nothing Then
                    task.Attachments = New List(Of String)
                End If
                If task.Images Is Nothing Then
                    task.Images = New List(Of String)
                End If
                If task.History Is Nothing Then
                    task.History = New List(Of TaskHistory)
                End If

                ' Populate names nếu null
                Dim technicians = Await GetAllTechniciansAsync()
                Dim saleManagers = Await GetAllSaleManagersAsync()
                Dim software = Await GetAllSoftwareAsync()

                ' Populate AssignedToName nếu null
                If String.IsNullOrEmpty(task.AssignedToName) AndAlso Not String.IsNullOrEmpty(task.AssignedTo) Then
                    Dim tech = technicians.FirstOrDefault(Function(t) t.Id = task.AssignedTo)
                    If tech IsNot Nothing Then
                        task.AssignedToName = tech.Name
                    End If
                End If

                ' Populate SaleManagerName nếu null
                If String.IsNullOrEmpty(task.SaleManagerName) AndAlso Not String.IsNullOrEmpty(task.SaleManagerId) Then
                    Dim sale = saleManagers.FirstOrDefault(Function(s) s.Id = task.SaleManagerId)
                    If sale IsNot Nothing Then
                        task.SaleManagerName = sale.Name
                    End If
                End If

                ' Populate SoftwareName nếu null
                If String.IsNullOrEmpty(task.SoftwareName) AndAlso Not String.IsNullOrEmpty(task.SoftwareId) Then
                    Dim soft = software.FirstOrDefault(Function(s) s.Id = task.SoftwareId)
                    If soft IsNot Nothing Then
                        task.SoftwareName = soft.Name
                    End If
                End If
            End If
            Return task
        Catch ex As Exception
            LogError("GetTaskByIdAsync", ex, $"taskId: {taskId}")
            Return Nothing
        End Try
    End Function

    ' Tạo task mới
    Public Async Function CreateTaskAsync(task As Task) As Task(Of Boolean)
        Try
            If task Is Nothing Then
                LogError("CreateTaskAsync", New ArgumentNullException("task"), "Task is null")
                Return False
            End If

            ' Đảm bảo các list được khởi tạo
            If task.Attachments Is Nothing Then
                task.Attachments = New List(Of String)
            End If
            If task.Images Is Nothing Then
                task.Images = New List(Of String)
            End If
            If task.History Is Nothing Then
                task.History = New List(Of TaskHistory)
            End If

            ' Đảm bảo ID được set
            If String.IsNullOrEmpty(task.Id) Then
                task.Id = Guid.NewGuid().ToString()
            End If

            ' Đảm bảo CreatedDate được set
            If task.CreatedDate = Nothing OrElse task.CreatedDate = DateTime.MinValue Then
                task.CreatedDate = DateTime.Now
            End If

            Dim json = JsonConvert.SerializeObject(task, _jsonSettings)
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")
            Dim response = Await _httpClient.PutAsync($"/tasks/{task.Id}.json", content)

            If response.IsSuccessStatusCode Then
                Return True
            Else
                Dim errorContent = Await response.Content.ReadAsStringAsync()
                LogError("CreateTaskAsync", New Exception($"HTTP {response.StatusCode}"), $"Response: {errorContent}")
                Return False
            End If
        Catch ex As Exception
            LogError("CreateTaskAsync", ex, $"TaskId: {If(task?.Id, "null")}")
            Return False
        End Try
    End Function

    ' Cập nhật task
    Public Async Function UpdateTaskAsync(task As Task) As Task(Of Boolean)
        Try
            If task Is Nothing Then
                LogError("UpdateTaskAsync", New ArgumentNullException("task"), "Task is null")
                Return False
            End If

            If String.IsNullOrEmpty(task.Id) Then
                LogError("UpdateTaskAsync", New ArgumentException("Task ID is required"), "")
                Return False
            End If

            ' Đảm bảo các list được khởi tạo
            If task.Attachments Is Nothing Then
                task.Attachments = New List(Of String)
            End If
            If task.Images Is Nothing Then
                task.Images = New List(Of String)
            End If
            If task.History Is Nothing Then
                task.History = New List(Of TaskHistory)
            End If

            ' Cập nhật UpdatedDate
            task.UpdatedDate = DateTime.Now

            Dim json = JsonConvert.SerializeObject(task, _jsonSettings)
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")
            Dim response = Await _httpClient.PutAsync($"/tasks/{task.Id}.json", content)

            If response.IsSuccessStatusCode Then
                Return True
            Else
                Dim errorContent = Await response.Content.ReadAsStringAsync()
                LogError("UpdateTaskAsync", New Exception($"HTTP {response.StatusCode}"), $"Response: {errorContent}")
                Return False
            End If
        Catch ex As Exception
            LogError("UpdateTaskAsync", ex, $"TaskId: {If(task?.Id, "null")}")
            Return False
        End Try
    End Function

    ' Xóa task
    Public Async Function DeleteTaskAsync(taskId As String) As Task(Of Boolean)
        Try
            If String.IsNullOrEmpty(taskId) Then
                Return False
            End If

            Dim response = Await _httpClient.DeleteAsync($"/tasks/{taskId}.json")
            response.EnsureSuccessStatusCode()
            Return True
        Catch ex As Exception
            LogError("DeleteTaskAsync", ex, $"taskId: {taskId}")
            Return False
        End Try
    End Function

    ' Lấy tất cả technicians
    Public Async Function GetAllTechniciansAsync() As Task(Of List(Of Technician))
        Try
            Dim response = Await _httpClient.GetAsync("/technicians.json")
            response.EnsureSuccessStatusCode()
            Dim json = Await response.Content.ReadAsStringAsync()

            If String.IsNullOrEmpty(json) OrElse json = "null" Then
                Return GetDefaultTechnicians()
            End If

            Dim techsDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Technician))(json, _jsonSettings)
            If techsDict Is Nothing Then
                Return GetDefaultTechnicians()
            End If

            Dim techs = New List(Of Technician)
            For Each kvp In techsDict
                If kvp.Value IsNot Nothing Then
                    kvp.Value.Id = kvp.Key
                    techs.Add(kvp.Value)
                End If
            Next

            Return If(techs.Count > 0, techs, GetDefaultTechnicians())
        Catch ex As Exception
            LogError("GetAllTechniciansAsync", ex)
            Return GetDefaultTechnicians()
        End Try
    End Function

    ' Tạo technician mới
    Public Async Function CreateTechnicianAsync(technician As Technician) As Task(Of Boolean)
        Try
            If technician Is Nothing Then
                LogError("CreateTechnicianAsync", New Exception("Technician is Nothing"), "Technician object is null")
                Return False
            End If

            If String.IsNullOrEmpty(technician.Id) Then
                LogError("CreateTechnicianAsync", New Exception("Technician.Id is empty"), "ID is null or empty")
                Return False
            End If

            If String.IsNullOrEmpty(technician.Name) Then
                LogError("CreateTechnicianAsync", New Exception("Technician.Name is empty"), "Name is null or empty")
                Return False
            End If

            Dim json = JsonConvert.SerializeObject(technician, _jsonSettings)
            Debug.WriteLine($"CreateTechnicianAsync - JSON: {json}")
            Debug.WriteLine($"CreateTechnicianAsync - URL: /technicians/{technician.Id}.json")
            
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")
            Dim url = $"/technicians/{technician.Id}.json"
            Dim response = Await _httpClient.PutAsync(url, content)
            
            Debug.WriteLine($"CreateTechnicianAsync - Response Status: {response.StatusCode}")
            
            If Not response.IsSuccessStatusCode Then
                Dim errorContent = Await response.Content.ReadAsStringAsync()
                LogError("CreateTechnicianAsync", New Exception($"HTTP {response.StatusCode}: {errorContent}"), $"URL: {url}")
                Return False
            End If
            
            response.EnsureSuccessStatusCode()
            Debug.WriteLine("CreateTechnicianAsync - Success")
            Return True
        Catch ex As Exception
            LogError("CreateTechnicianAsync", ex, $"Technician ID: {If(technician?.Id, "null")}, Name: {If(technician?.Name, "null")}")
            Return False
        End Try
    End Function

    Private Function GetDefaultTechnicians() As List(Of Technician)
        Return New List(Of Technician) From {
            New Technician With {.Id = "tech1", .Name = "Đinh Trọng Đạt", .Email = "dinh.trong.dat@example.com", .IsActive = True},
            New Technician With {.Id = "tech2", .Name = "Nguyễn Đình Việt", .Email = "nguyen.dinh.viet@example.com", .IsActive = True},
            New Technician With {.Id = "tech3", .Name = "Ngô Mạnh Hà", .Email = "ngo.manh.ha@example.com", .IsActive = True},
            New Technician With {.Id = "tech4", .Name = "Trịnh Tiến Anh", .Email = "trinh.tien.anh@example.com", .IsActive = True}
        }
    End Function

    ' Lấy tất cả Sale Managers
    Public Async Function GetAllSaleManagersAsync() As Task(Of List(Of SaleManager))
        Try
            Dim response = Await _httpClient.GetAsync("/saleManagers.json")
            response.EnsureSuccessStatusCode()
            Dim json = Await response.Content.ReadAsStringAsync()

            If String.IsNullOrEmpty(json) OrElse json = "null" Then
                Return GetDefaultSaleManagers()
            End If

            Dim salesDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, SaleManager))(json, _jsonSettings)
            If salesDict Is Nothing Then
                Return GetDefaultSaleManagers()
            End If

            Dim sales = New List(Of SaleManager)
            For Each kvp In salesDict
                If kvp.Value IsNot Nothing Then
                    kvp.Value.Id = kvp.Key
                    sales.Add(kvp.Value)
                End If
            Next

            Return If(sales.Count > 0, sales, GetDefaultSaleManagers())
        Catch ex As Exception
            LogError("GetAllSaleManagersAsync", ex)
            Return GetDefaultSaleManagers()
        End Try
    End Function

    Private Function GetDefaultSaleManagers() As List(Of SaleManager)
        Return New List(Of SaleManager) From {
            New SaleManager With {.Id = "sale1", .Name = "Ms. Nguyệt", .IsActive = True},
            New SaleManager With {.Id = "sale2", .Name = "Ms. Trang", .IsActive = True},
            New SaleManager With {.Id = "sale3", .Name = "Ms. Như", .IsActive = True},
            New SaleManager With {.Id = "sale4", .Name = "Ms. Huyền", .IsActive = True},
            New SaleManager With {.Id = "sale5", .Name = "Ms. Hiền", .IsActive = True}
        }
    End Function

    ' Tạo Sale Manager mới
    Public Async Function CreateSaleManagerAsync(saleManager As SaleManager) As Task(Of Boolean)
        Try
            If saleManager Is Nothing Then
                LogError("CreateSaleManagerAsync", New Exception("SaleManager is Nothing"), "SaleManager object is null")
                Return False
            End If

            If String.IsNullOrEmpty(saleManager.Id) Then
                LogError("CreateSaleManagerAsync", New Exception("SaleManager.Id is empty"), "ID is null or empty")
                Return False
            End If

            If String.IsNullOrEmpty(saleManager.Name) Then
                LogError("CreateSaleManagerAsync", New Exception("SaleManager.Name is empty"), "Name is null or empty")
                Return False
            End If

            Dim json = JsonConvert.SerializeObject(saleManager, _jsonSettings)
            Debug.WriteLine($"CreateSaleManagerAsync - JSON: {json}")
            Debug.WriteLine($"CreateSaleManagerAsync - URL: /saleManagers/{saleManager.Id}.json")
            
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")
            Dim url = $"/saleManagers/{saleManager.Id}.json"
            Dim response = Await _httpClient.PutAsync(url, content)
            
            Debug.WriteLine($"CreateSaleManagerAsync - Response Status: {response.StatusCode}")
            
            If Not response.IsSuccessStatusCode Then
                Dim errorContent = Await response.Content.ReadAsStringAsync()
                LogError("CreateSaleManagerAsync", New Exception($"HTTP {response.StatusCode}: {errorContent}"), $"URL: {url}")
                Return False
            End If
            
            response.EnsureSuccessStatusCode()
            Debug.WriteLine("CreateSaleManagerAsync - Success")
            Return True
        Catch ex As Exception
            LogError("CreateSaleManagerAsync", ex, $"SaleManager ID: {If(saleManager?.Id, "null")}, Name: {If(saleManager?.Name, "null")}")
            Return False
        End Try
    End Function

    ' Lấy tất cả Software
    Public Async Function GetAllSoftwareAsync() As Task(Of List(Of Software))
        Try
            Dim response = Await _httpClient.GetAsync("/software.json")
            response.EnsureSuccessStatusCode()
            Dim json = Await response.Content.ReadAsStringAsync()

            If String.IsNullOrEmpty(json) OrElse json = "null" Then
                Return GetDefaultSoftware()
            End If

            Dim softDict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Software))(json, _jsonSettings)
            If softDict Is Nothing Then
                Return GetDefaultSoftware()
            End If

            Dim software = New List(Of Software)
            For Each kvp In softDict
                If kvp.Value IsNot Nothing Then
                    kvp.Value.Id = kvp.Key
                    software.Add(kvp.Value)
                End If
            Next

            Return If(software.Count > 0, software, GetDefaultSoftware())
        Catch ex As Exception
            LogError("GetAllSoftwareAsync", ex)
            Return GetDefaultSoftware()
        End Try
    End Function

    Private Function GetDefaultSoftware() As List(Of Software)
        Return New List(Of Software) From {
            New Software With {.Id = "soft1", .Name = "WDL", .IsActive = True},
            New Software With {.Id = "soft2", .Name = "PBC", .IsActive = True},
            New Software With {.Id = "soft3", .Name = "PFD 2025", .IsActive = True},
            New Software With {.Id = "soft4", .Name = "IFD", .IsActive = True},
            New Software With {.Id = "soft5", .Name = "RCBC", .IsActive = True},
            New Software With {.Id = "soft6", .Name = "EtabsGen", .IsActive = True},
            New Software With {.Id = "soft7", .Name = "RCSC", .IsActive = True},
            New Software With {.Id = "soft8", .Name = "WLA", .IsActive = True},
            New Software With {.Id = "soft9", .Name = "PFD2015", .IsActive = True},
            New Software With {.Id = "soft10", .Name = "RCC", .IsActive = True},
            New Software With {.Id = "soft11", .Name = "QS Smart", .IsActive = True},
            New Software With {.Id = "soft12", .Name = "PFD", .IsActive = True},
            New Software With {.Id = "soft13", .Name = "RCB", .IsActive = True},
            New Software With {.Id = "soft14", .Name = "DG", .IsActive = True},
            New Software With {.Id = "soft15", .Name = "KSD", .IsActive = True},
            New Software With {.Id = "soft16", .Name = "PBC 2025", .IsActive = True}
        }
    End Function
End Class