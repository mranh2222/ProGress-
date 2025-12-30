Imports System.Threading.Tasks
Imports System.Web.Mvc
Imports System.Linq

Public Class DashboardController
    Inherits Controller
    
    Private ReadOnly _firebaseService As FirebaseService
    
    Public Sub New()
        _firebaseService = New FirebaseService()
    End Sub
    
    ' GET: Dashboard
    Async Function Index() As Task(Of ActionResult)
        Dim tasks = Await _firebaseService.GetAllTasksAsync()
        Dim technicians = Await _firebaseService.GetAllTechniciansAsync()
        
        ' Thống kê
        ViewBag.TotalTasks = tasks.Count
        ViewBag.PendingTasks = tasks.Where(Function(t) t.Status = TaskStatus.Pending).Count()
        ViewBag.InProgressTasks = tasks.Where(Function(t) t.Status = TaskStatus.InProgress).Count()
        ViewBag.WaitingTasks = tasks.Where(Function(t) t.Status = TaskStatus.Waiting).Count()
        ViewBag.CompletedTasks = tasks.Where(Function(t) t.Status = TaskStatus.Completed).Count()
        ViewBag.PausedTasks = tasks.Where(Function(t) t.Status = TaskStatus.Paused).Count()
        
        ' Nhóm theo technician
        ViewBag.TasksByTechnician = tasks.GroupBy(Function(t) t.AssignedTo).ToDictionary(Function(g) g.Key, Function(g) g.ToList())
        ViewBag.Technicians = technicians
        
        ' Thống kê theo technician (tên và số lượng)
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
        
        ' Thống kê theo nền tảng hỗ trợ
        Dim zaloCount = tasks.Where(Function(t) t.SupportPlatform = SupportPlatform.Zalo).Count()
        Dim memberSupportCount = tasks.Where(Function(t) t.SupportPlatform = SupportPlatform.MemberSupport).Count()
        Dim customerContactCount = tasks.Where(Function(t) t.SupportPlatform = SupportPlatform.CustomerContactSale).Count()
        
        ViewBag.ZaloCount = zaloCount
        ViewBag.MemberSupportCount = memberSupportCount
        ViewBag.CustomerContactCount = customerContactCount
        
        ' Nhóm theo status cho Kanban
        ViewBag.TasksByStatus = New Dictionary(Of TaskStatus, List(Of Task)) From {
            {TaskStatus.Pending, tasks.Where(Function(t) t.Status = TaskStatus.Pending).ToList()},
            {TaskStatus.InProgress, tasks.Where(Function(t) t.Status = TaskStatus.InProgress).ToList()},
            {TaskStatus.Waiting, tasks.Where(Function(t) t.Status = TaskStatus.Waiting).ToList()},
            {TaskStatus.Completed, tasks.Where(Function(t) t.Status = TaskStatus.Completed).ToList()},
            {TaskStatus.Paused, tasks.Where(Function(t) t.Status = TaskStatus.Paused).ToList()}
        }
        
        Return View(tasks)
    End Function
    
    ' GET: Dashboard/GetTasksByTechnician
    Async Function GetTasksByTechnician(technicianId As String) As Task(Of JsonResult)
        Try
            If String.IsNullOrEmpty(technicianId) Then
                Return Json(New With {.success = False, .message = "ID kỹ thuật viên không hợp lệ"}, JsonRequestBehavior.AllowGet)
            End If
            
            Dim allTasks = Await _firebaseService.GetAllTasksAsync()
            Dim technicianTasks = allTasks.Where(Function(t) t.AssignedTo = technicianId).OrderByDescending(Function(t) t.CreatedDate).ToList()
            
            ' Chuyển đổi sang format JSON
            Dim tasksData = technicianTasks.Select(Function(t) New With {
                .id = t.Id,
                .description = t.Description,
                .customerName = t.CustomerName,
                .softwareName = t.SoftwareName,
                .status = CInt(t.Status),
                .createdDate = t.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                .responseToCustomer = t.ResponseToCustomer
            }).ToList()
            
            Return Json(New With {.success = True, .tasks = tasksData}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Return Json(New With {.success = False, .message = $"Lỗi: {ex.Message}"}, JsonRequestBehavior.AllowGet)
        End Try
    End Function
End Class
