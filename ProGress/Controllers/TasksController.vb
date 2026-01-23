Imports System.Web.Mvc
Imports System.Linq
Imports System.Collections.Generic
Imports ThreadingTask = System.Threading.Tasks

Public Class TasksController
    Inherits Controller
    
    Private ReadOnly _dataService As SqlDataService
    Private ReadOnly _storageService As FileStorageService
    
    Public Sub New()
        _dataService = New SqlDataService()
        _storageService = New FileStorageService()
    End Sub
    
    ' GET: Tasks
    Async Function Index() As ThreadingTask.Task(Of ActionResult)
        Dim tasks = Await _dataService.GetAllTasksAsync()
        Return View(tasks)
    End Function
    
    ' GET: Tasks/Details/5
    Async Function Details(id As String) As ThreadingTask.Task(Of ActionResult)
        If String.IsNullOrEmpty(id) Then
            Return RedirectToAction("Index")
        End If
        
        Dim task = Await _dataService.GetTaskByIdAsync(id)
        If task Is Nothing Then
            Return HttpNotFound()
        End If
        
        Return View(task)
    End Function
    
    ' GET: Tasks/Create
    Async Function Create() As ThreadingTask.Task(Of ActionResult)
        ViewBag.Technicians = Await _dataService.GetAllTechniciansAsync()
        ViewBag.SaleManagers = Await _dataService.GetAllSaleManagersAsync()
        ViewBag.Software = Await _dataService.GetAllSoftwareAsync()
        Return View(New Task())
    End Function
    
    ' POST: Tasks/Create
    <HttpPost>
    <ValidateAntiForgeryToken>
    <ValidateInput(False)>
    Async Function Create(task As Task, uploadedFiles As HttpPostedFileBase(), uploadedImages As HttpPostedFileBase()) As ThreadingTask.Task(Of ActionResult)
        If task Is Nothing Then task = New Task()
        
        task.Description = Request.Unvalidated.Form("Description")
        task.CustomerName = Request.Unvalidated.Form("CustomerName")
        task.Tag = Request.Unvalidated.Form("Tag")
        task.SaleManagerId = Request.Unvalidated.Form("SaleManagerId")
        task.SoftwareId = Request.Unvalidated.Form("SoftwareId")
        task.AssignedTo = Request.Unvalidated.Form("AssignedTo")
        
        Dim fileReceivedDateValue = Request.Unvalidated.Form("FileReceivedDate")
        If Not String.IsNullOrEmpty(fileReceivedDateValue) Then
            Dim d As DateTime
            If DateTime.TryParse(fileReceivedDateValue, d) Then task.FileReceivedDate = d
        End If
        
        Dim statusValue = Request.Unvalidated.Form("Status")
        If Not String.IsNullOrEmpty(statusValue) Then
            Dim s As Integer
            If Integer.TryParse(statusValue, s) Then task.Status = CType(s, TaskStatus)
        End If
        
        If String.IsNullOrEmpty(task.Description) OrElse String.IsNullOrEmpty(task.CustomerName) OrElse 
           String.IsNullOrEmpty(task.SaleManagerId) OrElse String.IsNullOrEmpty(task.SoftwareId) OrElse 
           String.IsNullOrEmpty(task.AssignedTo) Then
            ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin bắt buộc.")
            ViewBag.Technicians = Await _dataService.GetAllTechniciansAsync()
            ViewBag.SaleManagers = Await _dataService.GetAllSaleManagersAsync()
            ViewBag.Software = Await _dataService.GetAllSoftwareAsync()
            Return View(task)
        End If

        If uploadedFiles IsNot Nothing Then
            For Each postedFile As HttpPostedFileBase In uploadedFiles
                If postedFile IsNot Nothing AndAlso postedFile.ContentLength > 0 Then
                    Dim url = Await _storageService.UploadFileAsync(postedFile, "uploads")
                    If Not String.IsNullOrEmpty(url) Then task.Attachments.Add(url)
                End If
            Next
        End If
        
        If uploadedImages IsNot Nothing Then
            For Each postedImage As HttpPostedFileBase In uploadedImages
                If postedImage IsNot Nothing AndAlso postedImage.ContentLength > 0 Then
                    Dim url = Await _storageService.UploadFileAsync(postedImage, "images")
                    If Not String.IsNullOrEmpty(url) Then task.Images.Add(url)
                End If
            Next
        End If

        Await _dataService.CreateTaskAsync(task)
        Return RedirectToAction("Index")
    End Function
    
    ' GET: Tasks/Edit/5
    Async Function Edit(id As String) As ThreadingTask.Task(Of ActionResult)
        If String.IsNullOrEmpty(id) Then Return RedirectToAction("Index")
        Dim task = Await _dataService.GetTaskByIdAsync(id)
        If task Is Nothing Then Return HttpNotFound()
        
        ViewBag.Technicians = Await _dataService.GetAllTechniciansAsync()
        ViewBag.SaleManagers = Await _dataService.GetAllSaleManagersAsync()
        ViewBag.Software = Await _dataService.GetAllSoftwareAsync()
        Return View(task)
    End Function
    
    ' POST: Tasks/Edit/5
    <HttpPost>
    <ValidateAntiForgeryToken>
    <ValidateInput(False)>
    Async Function Edit(id As String, task As Task, uploadedFiles As HttpPostedFileBase(), uploadedImages As HttpPostedFileBase()) As ThreadingTask.Task(Of ActionResult)
        If task Is Nothing Then task = New Task()
        task.Id = id
        
        task.Description = Request.Unvalidated.Form("Description")
        task.CustomerName = Request.Unvalidated.Form("CustomerName")
        task.Solution = Request.Unvalidated.Form("Solution")
        task.ResponseToCustomer = Request.Unvalidated.Form("ResponseToCustomer")
        task.Tag = Request.Unvalidated.Form("Tag")
        task.SaleManagerId = Request.Unvalidated.Form("SaleManagerId")
        task.SoftwareId = Request.Unvalidated.Form("SoftwareId")
        task.AssignedTo = Request.Unvalidated.Form("AssignedTo")
        
        Dim statusValue = Request.Unvalidated.Form("Status")
        If Not String.IsNullOrEmpty(statusValue) Then
            Dim s As Integer
            If Integer.TryParse(statusValue, s) Then task.Status = CType(s, TaskStatus)
        End If
        
        Dim existing = Await _dataService.GetTaskByIdAsync(id)
        If existing IsNot Nothing Then
            task.Attachments = existing.Attachments
            task.Images = existing.Images
            task.CreatedDate = existing.CreatedDate
        End If
        
        If uploadedFiles IsNot Nothing Then
            For Each postedFile As HttpPostedFileBase In uploadedFiles
                If postedFile IsNot Nothing AndAlso postedFile.ContentLength > 0 Then
                    Dim url = Await _storageService.UploadFileAsync(postedFile, "uploads")
                    If Not String.IsNullOrEmpty(url) Then task.Attachments.Add(url)
                End If
            Next
        End If
        
        If uploadedImages IsNot Nothing Then
            For Each postedImage As HttpPostedFileBase In uploadedImages
                If postedImage IsNot Nothing AndAlso postedImage.ContentLength > 0 Then
                    Dim url = Await _storageService.UploadFileAsync(postedImage, "images")
                    If Not String.IsNullOrEmpty(url) Then task.Images.Add(url)
                End If
            Next
        End If

        Await _dataService.UpdateTaskAsync(task)
        Return RedirectToAction("Index")
    End Function
    
    ' POST: Tasks/Delete/5
    <HttpPost>
    <ActionName("Delete")>
    <ValidateAntiForgeryToken>
    Async Function DeleteConfirmed(id As String) As ThreadingTask.Task(Of ActionResult)
        Try
            Dim taskId = If(Not String.IsNullOrEmpty(Request.Form("id")), Request.Form("id"), id)
            If String.IsNullOrEmpty(taskId) Then
                Return RedirectToAction("Index")
            End If
            
            Dim success = Await _dataService.DeleteTaskAsync(taskId)
            If Not success Then
                ' Có thể thêm thông báo lỗi nếu cần
            End If
        Catch ex As Exception
            ' Log lỗi nếu cần
        End Try
        Return RedirectToAction("Index")
    End Function
    
    ' POST: Tasks/UpdateStatus
    <HttpPost>
    Async Function UpdateStatus(id As String, status As Integer) As ThreadingTask.Task(Of JsonResult)
        Try
            Dim task = Await _dataService.GetTaskByIdAsync(id)
            If task Is Nothing Then Return Json(New With {.success = False})
            
            task.Status = CType(status, TaskStatus)
            If task.Status = TaskStatus.Completed Then task.CompletedDate = DateTime.Now
            
            Dim success = Await _dataService.UpdateTaskAsync(task)
            Return Json(New With {.success = success})
        Catch ex As Exception
            Return Json(New With {.success = False})
        End Try
    End Function

    ' POST: Tasks/ToggleSaved
    <HttpPost>
    Async Function ToggleSaved(id As String, isSaved As Boolean) As ThreadingTask.Task(Of JsonResult)
        Try
            Dim success = Await _dataService.ToggleTaskSavedAsync(id, isSaved)
            Return Json(New With {.success = success})
        Catch ex As Exception
            Return Json(New With {.success = False})
        End Try
    End Function

    ' POST: Tasks/ReplyToCustomer
    <HttpPost>
    <ValidateAntiForgeryToken>
    <ValidateInput(False)>
    Async Function ReplyToCustomer(id As String, responseToCustomer As String, uploadedFiles As HttpPostedFileBase(), uploadedImages As HttpPostedFileBase()) As ThreadingTask.Task(Of JsonResult)
        Try
            ' id và responseToCustomer được MVC tự động bind từ form parameters
            Dim taskId = If(String.IsNullOrEmpty(id), Request.Unvalidated.Form("id"), id)
            Dim responseText = responseToCustomer
            
            If String.IsNullOrEmpty(responseText) Then
                responseText = Request.Unvalidated.Form("responseToCustomer")
            End If
            
            ' Nếu vẫn bị x2 (do MVC merge các field cùng tên), ta chỉ lấy phần nội dung đầu tiên
            If Not String.IsNullOrEmpty(responseText) AndAlso responseText.Contains(",") Then
                ' Kiểm tra xem có thực sự bị lặp không (phân tách bởi dấu phẩy)
                ' Lưu ý: Cách này có thể nguy hiểm nếu nội dung chứa dấu phẩy thật, 
                ' nhưng thường MVC merge các field cùng tên sẽ tạo ra chuỗi "val1,val2"
                ' Tuy nhiên, Quill content là HTML nên thường không bị lặp kiểu đơn giản này.
                ' Để chắc chắn hơn, ta xem lại phía Client.
            End If

            Dim task = Await _dataService.GetTaskByIdAsync(taskId)
            If task Is Nothing Then Return Json(New With {.success = False, .message = "Không tìm thấy công việc"})
            
            task.ResponseToCustomer = responseText
            
            ' Xử lý file đính kèm của phản hồi
            If uploadedFiles IsNot Nothing Then
                For Each postedFile As HttpPostedFileBase In uploadedFiles
                    If postedFile IsNot Nothing AndAlso postedFile.ContentLength > 0 Then
                        Dim url = Await _storageService.UploadFileAsync(postedFile, "uploads/responses")
                        If Not String.IsNullOrEmpty(url) Then task.ResponseAttachments.Add(url)
                    End If
                Next
            End If
            
            ' Xử lý ảnh đính kèm của phản hồi
            If uploadedImages IsNot Nothing Then
                For Each postedImage As HttpPostedFileBase In uploadedImages
                    If postedImage IsNot Nothing AndAlso postedImage.ContentLength > 0 Then
                        Dim url = Await _storageService.UploadFileAsync(postedImage, "images/responses")
                        If Not String.IsNullOrEmpty(url) Then task.ResponseImages.Add(url)
                    End If
                Next
            End If
            
            task.Status = TaskStatus.Completed
            task.CompletedDate = DateTime.Now
            
            Dim success = Await _dataService.UpdateTaskAsync(task)
            Return Json(New With {.success = success})
        Catch ex As Exception
            Return Json(New With {.success = False, .message = ex.Message})
        End Try
    End Function
End Class
