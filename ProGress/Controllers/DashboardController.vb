Imports System.Web.Mvc
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Configuration
Imports ThreadingTask = System.Threading.Tasks

Public Class DashboardController
    Inherits Controller
    
    Private ReadOnly _dataService As SqlDataService
    
    Public Sub New()
        _dataService = New SqlDataService()
    End Sub
    
    ' GET: Dashboard
    Async Function Index() As ThreadingTask.Task(Of ActionResult)
        Dim tasks = Await _dataService.GetAllTasksAsync()
        Dim technicians = Await _dataService.GetAllTechniciansAsync()
        
        ViewBag.TotalTasks = tasks.Count
        ViewBag.PendingTasks = tasks.Where(Function(t) t.Status = TaskStatus.Pending).Count()
        ViewBag.InProgressTasks = tasks.Where(Function(t) t.Status = TaskStatus.InProgress).Count()
        ViewBag.WaitingTasks = tasks.Where(Function(t) t.Status = TaskStatus.Waiting).Count()
        ViewBag.CompletedTasks = tasks.Where(Function(t) t.Status = TaskStatus.Completed).Count()
        ViewBag.PausedTasks = tasks.Where(Function(t) t.Status = TaskStatus.Paused).Count()
        
        ViewBag.TasksByTechnician = tasks.GroupBy(Function(t) t.AssignedTo).ToDictionary(Function(g) g.Key, Function(g) g.ToList())
        ViewBag.Technicians = technicians
        
        Dim technicianStats = New List(Of Object)
        For Each tech In technicians
            Dim techTasks = tasks.Where(Function(t) t.AssignedTo = tech.Id).ToList()
            technicianStats.Add(New With {
                .Id = tech.Id,
                .Name = tech.Name,
                .Total = techTasks.Count,
                .Pending = techTasks.Where(Function(t) t.Status = TaskStatus.Pending).Count(),
                .InProgress = techTasks.Where(Function(t) t.Status = TaskStatus.InProgress).Count(),
                .Waiting = techTasks.Where(Function(t) t.Status = TaskStatus.Waiting).Count(),
                .Completed = techTasks.Where(Function(t) t.Status = TaskStatus.Completed).Count(),
                .Paused = techTasks.Where(Function(t) t.Status = TaskStatus.Paused).Count()
            })
        Next
        ViewBag.TechnicianStats = technicianStats.OrderByDescending(Function(t) t.Total).ToList()
        
        Dim zaloCount = tasks.Where(Function(t) t.SupportPlatform = SupportPlatform.Zalo).Count()
        Dim memberSupportCount = tasks.Where(Function(t) t.SupportPlatform = SupportPlatform.MemberSupport).Count()
        Dim customerContactCount = tasks.Where(Function(t) t.SupportPlatform = SupportPlatform.CustomerContactSale).Count()
        
        ViewBag.ZaloCount = zaloCount
        ViewBag.MemberSupportCount = memberSupportCount
        ViewBag.CustomerContactCount = customerContactCount
        
        ViewBag.TasksByStatus = New Dictionary(Of TaskStatus, List(Of Task)) From {
            {TaskStatus.Pending, tasks.Where(Function(t) t.Status = TaskStatus.Pending).ToList()},
            {TaskStatus.InProgress, tasks.Where(Function(t) t.Status = TaskStatus.InProgress).ToList()},
            {TaskStatus.Waiting, tasks.Where(Function(t) t.Status = TaskStatus.Waiting).ToList()},
            {TaskStatus.Completed, tasks.Where(Function(t) t.Status = TaskStatus.Completed).ToList()},
            {TaskStatus.Paused, tasks.Where(Function(t) t.Status = TaskStatus.Paused).ToList()}
        }
        
        Return View(tasks)
    End Function

    ' Đường dẫn bí mật để khởi tạo Database trên Server: /Dashboard/SetupDatabase
    Async Function SetupDatabase() As ThreadingTask.Task(Of ActionResult)
        Try
            Dim sqlPath = Server.MapPath("~/DATABASE_SETUP.sql")
            Dim script = System.IO.File.ReadAllText(sqlPath)
            
            ' Tách các lệnh SQL theo từ khóa GO
            Dim commands = System.Text.RegularExpressions.Regex.Split(script, "^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline Or System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            
            Dim connectionString As String
            Dim configConnectionString = ConfigurationManager.ConnectionStrings("DefaultConnection")
            If configConnectionString IsNot Nothing AndAlso Not String.IsNullOrEmpty(configConnectionString.ConnectionString) Then
                connectionString = configConnectionString.ConnectionString
            Else
                connectionString = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProGressDB;Integrated Security=True"
            End If
            
            Using conn As New SqlConnection(connectionString)
                Await conn.OpenAsync()
                For Each cmdText In commands
                    If Not String.IsNullOrWhiteSpace(cmdText) Then
                        Using cmd As New SqlCommand(cmdText, conn)
                            Await cmd.ExecuteNonQueryAsync()
                        End Using
                    End If
                Next
            End Using
            
            Return Content("Chúc mừng! Database đã được khởi tạo thành công trên Server. Bây giờ bạn có thể sử dụng web.")
        Catch ex As Exception
            Return Content("Lỗi khởi tạo Database: " & ex.Message)
        End Try
    End Function
    
    ' GET: Dashboard/GetTasksByTechnician
    Async Function GetTasksByTechnician(technicianId As String) As ThreadingTask.Task(Of JsonResult)
        Try
            If String.IsNullOrEmpty(technicianId) Then
                Return Json(New With {.success = False, .message = "ID kỹ thuật viên không hợp lệ"}, JsonRequestBehavior.AllowGet)
            End If
            
            Dim allTasks = Await _dataService.GetAllTasksAsync()
            Dim technicianTasks = allTasks.Where(Function(t) t.AssignedTo = technicianId).OrderByDescending(Function(t) t.CreatedDate).ToList()
            
            Dim tasksData = technicianTasks.Select(Function(t) New With {
                .id = t.Id,
                .description = t.Description,
                .customerName = t.CustomerName,
                .softwareName = t.SoftwareName,
                .status = CInt(t.Status),
                .createdDate = t.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                .responseToCustomer = t.ResponseToCustomer,
                .isSaved = t.IsSaved
            }).ToList()
            
            Return Json(New With {.success = True, .tasks = tasksData}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.success = False, .message = $"Lỗi: {ex.Message}"}, JsonRequestBehavior.AllowGet)
        End Try
    End Function
End Class
