Imports System.Threading.Tasks
Imports System.Web.Mvc
Imports System.Linq

Public Class TasksController
    Inherits Controller
    
    Private ReadOnly _firebaseService As FirebaseService
    
    Public Sub New()
        _firebaseService = New FirebaseService()
    End Sub
    
    ' GET: Tasks
    Async Function Index() As Task(Of ActionResult)
        Dim tasks = Await _firebaseService.GetAllTasksAsync()
        Return View(tasks)
    End Function
    
    ' GET: Tasks/Details/5
    Async Function Details(id As String) As Task(Of ActionResult)
        If String.IsNullOrEmpty(id) Then
            Return RedirectToAction("Index")
        End If
        
        Dim task = Await _firebaseService.GetTaskByIdAsync(id)
        If task Is Nothing Then
            Return HttpNotFound()
        End If
        
        Return View(task)
    End Function
    
    ' GET: Tasks/Create
    Async Function Create() As Task(Of ActionResult)
        ViewBag.Technicians = Await _firebaseService.GetAllTechniciansAsync()
        ViewBag.SaleManagers = Await _firebaseService.GetAllSaleManagersAsync()
        ViewBag.Software = Await _firebaseService.GetAllSoftwareAsync()
        Return View(New Task())
    End Function
    
    ' POST: Tasks/Create
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Create(task As Task, uploadedFiles As HttpPostedFileBase(), uploadedImages As HttpPostedFileBase()) As Task(Of ActionResult)
        ' Khởi tạo task nếu null
        If task Is Nothing Then
            task = New Task()
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
        
        ' Xử lý Status từ form
        Dim statusValue As String = Request.Form("Status")
        If Not String.IsNullOrEmpty(statusValue) Then
            Dim statusInt As Integer
            If Integer.TryParse(statusValue, statusInt) Then
                task.Status = CType(statusInt, TaskStatus)
            End If
        End If
        
        ' Xử lý AssignedTo từ form
        Dim assignedToValue As String = Request.Form("AssignedTo")
        If Not String.IsNullOrEmpty(assignedToValue) Then
            task.AssignedTo = assignedToValue
        End If
        
        ' Lấy các trường từ form
        Dim descriptionValue As String = Request.Form("Description")
        Dim customerNameValue As String = Request.Form("CustomerName")
        Dim tagValue As String = Request.Form("Tag")
        Dim fileReceivedDateValue As String = Request.Form("FileReceivedDate")
        Dim expectedCompletionDateValue As String = Request.Form("ExpectedCompletionDate")
        Dim completedDateValue As String = Request.Form("CompletedDate")
        Dim supportPlatformValue As String = Request.Form("SupportPlatform")
        Dim saleManagerIdValue As String = Request.Form("SaleManagerId")
        Dim softwareIdValue As String = Request.Form("SoftwareId")
        
        If Not String.IsNullOrEmpty(descriptionValue) Then
            task.Description = descriptionValue
        End If
        If Not String.IsNullOrEmpty(customerNameValue) Then
            task.CustomerName = customerNameValue
        End If
        If Not String.IsNullOrEmpty(tagValue) Then
            task.Tag = tagValue
        End If
        
        ' Xử lý ngày nhận file
        If Not String.IsNullOrEmpty(fileReceivedDateValue) Then
            Dim fileReceivedDate As DateTime
            If DateTime.TryParse(fileReceivedDateValue, fileReceivedDate) Then
                task.FileReceivedDate = fileReceivedDate
            End If
        End If
        
        ' Xử lý ngày dự kiến hoàn thành
        If Not String.IsNullOrEmpty(expectedCompletionDateValue) Then
            Dim expectedDate As DateTime
            If DateTime.TryParse(expectedCompletionDateValue, expectedDate) Then
                task.ExpectedCompletionDate = expectedDate
            End If
        End If
        
        ' Xử lý ngày thực tế hoàn thành
        If Not String.IsNullOrEmpty(completedDateValue) Then
            Dim completedDate As DateTime
            If DateTime.TryParse(completedDateValue, completedDate) Then
                task.CompletedDate = completedDate
            End If
        End If
        
        ' Xử lý nền tảng hỗ trợ
        If Not String.IsNullOrEmpty(supportPlatformValue) Then
            Dim platformInt As Integer
            If Integer.TryParse(supportPlatformValue, platformInt) Then
                task.SupportPlatform = CType(platformInt, SupportPlatform)
            End If
        End If
        
        ' Xử lý Sale Manager
        If Not String.IsNullOrEmpty(saleManagerIdValue) Then
            task.SaleManagerId = saleManagerIdValue
        End If
        
        ' Xử lý Software
        If Not String.IsNullOrEmpty(softwareIdValue) Then
            task.SoftwareId = softwareIdValue
        End If
        
        ' Validate lại
        Dim validationErrors As New List(Of String)
        If String.IsNullOrEmpty(task.Description) Then
            validationErrors.Add("Mô tả")
        End If
        If String.IsNullOrEmpty(task.CustomerName) Then
            validationErrors.Add("Khách hàng")
        End If
        If String.IsNullOrEmpty(task.SaleManagerId) Then
            validationErrors.Add("Sale quản lý")
        End If
        If String.IsNullOrEmpty(task.SoftwareId) Then
            validationErrors.Add("Phần mềm sử dụng")
        End If
        If String.IsNullOrEmpty(task.AssignedTo) Then
            validationErrors.Add("Kỹ thuật phụ trách")
        End If
        
        If validationErrors.Count > 0 Then
            ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin bắt buộc: " & String.Join(", ", validationErrors))
            ViewBag.Technicians = Await _firebaseService.GetAllTechniciansAsync()
            ViewBag.SaleManagers = Await _firebaseService.GetAllSaleManagersAsync()
            ViewBag.Software = Await _firebaseService.GetAllSoftwareAsync()
            Return View(task)
        End If
        
        If Not String.IsNullOrEmpty(task.Description) AndAlso Not String.IsNullOrEmpty(task.CustomerName) AndAlso Not String.IsNullOrEmpty(task.SaleManagerId) AndAlso Not String.IsNullOrEmpty(task.SoftwareId) AndAlso Not String.IsNullOrEmpty(task.AssignedTo) Then
            ' Lấy tên người phụ trách
            If Not String.IsNullOrEmpty(task.AssignedTo) Then
                Dim technicians = Await _firebaseService.GetAllTechniciansAsync()
                Dim tech = technicians.FirstOrDefault(Function(t) t.Id = task.AssignedTo)
                If tech IsNot Nothing Then
                    task.AssignedToName = tech.Name
                End If
            End If
            
            ' Lấy tên Sale Manager
            If Not String.IsNullOrEmpty(task.SaleManagerId) Then
                Dim saleManagers = Await _firebaseService.GetAllSaleManagersAsync()
                Dim sale = saleManagers.FirstOrDefault(Function(s) s.Id = task.SaleManagerId)
                If sale IsNot Nothing Then
                    task.SaleManagerName = sale.Name
                End If
            End If
            
            ' Lấy tên Software
            If Not String.IsNullOrEmpty(task.SoftwareId) Then
                Dim software = Await _firebaseService.GetAllSoftwareAsync()
                Dim soft = software.FirstOrDefault(Function(s) s.Id = task.SoftwareId)
                If soft IsNot Nothing Then
                    task.SoftwareName = soft.Name
                End If
            End If
            
            ' Tạo thư mục nếu chưa có
            Dim uploadsDir = Server.MapPath("~/App_Data/Uploads")
            Dim imagesDir = Server.MapPath("~/App_Data/Images")
            If Not System.IO.Directory.Exists(uploadsDir) Then
                System.IO.Directory.CreateDirectory(uploadsDir)
            End If
            If Not System.IO.Directory.Exists(imagesDir) Then
                System.IO.Directory.CreateDirectory(imagesDir)
            End If
            
            ' Xử lý upload files
            If uploadedFiles IsNot Nothing Then
                For Each uploadedFile In uploadedFiles
                    If uploadedFile IsNot Nothing AndAlso uploadedFile.ContentLength > 0 Then
                        Dim fileName = System.IO.Path.GetFileName(uploadedFile.FileName)
                        Dim uniqueFileName = $"{Guid.NewGuid()}_{fileName}"
                        Dim path = System.IO.Path.Combine(uploadsDir, uniqueFileName)
                        uploadedFile.SaveAs(path)
                        task.Attachments.Add($"/File/Download?filePath=/App_Data/Uploads/{uniqueFileName}")
                    End If
                Next
            End If
            
            ' Xử lý upload images
            If uploadedImages IsNot Nothing Then
                For Each uploadedImage In uploadedImages
                    If uploadedImage IsNot Nothing AndAlso uploadedImage.ContentLength > 0 Then
                        Dim fileName = System.IO.Path.GetFileName(uploadedImage.FileName)
                        Dim uniqueFileName = $"{Guid.NewGuid()}_{fileName}"
                        Dim path = System.IO.Path.Combine(imagesDir, uniqueFileName)
                        uploadedImage.SaveAs(path)
                        task.Images.Add($"/File/Download?filePath=/App_Data/Images/{uniqueFileName}")
                    End If
                Next
            End If
            
            ' Thêm lịch sử
            Dim history = New TaskHistory With {
                .TaskId = task.Id,
                .Action = "Created",
                .Description = "Tạo công việc mới",
                .ChangedBy = "system",
                .ChangedByName = "System"
            }
            task.History.Add(history)
            
            Try
                Dim success = Await _firebaseService.CreateTaskAsync(task)
                If success Then
                    Return RedirectToAction("Index")
                Else
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu công việc vào Firebase. Vui lòng kiểm tra:")
                    ModelState.AddModelError("", "1. Kết nối internet")
                    ModelState.AddModelError("", "2. Cấu hình Firebase URL trong Web.config")
                    ModelState.AddModelError("", "3. Firebase Database Rules cho phép ghi dữ liệu")
                End If
            Catch ex As Exception
                ModelState.AddModelError("", $"Lỗi khi lưu công việc: {ex.Message}")
                If ex.InnerException IsNot Nothing Then
                    ModelState.AddModelError("", $"Chi tiết: {ex.InnerException.Message}")
                End If
            End Try
        End If
        
        ' Populate ViewBag lại nếu có lỗi
        ViewBag.Technicians = Await _firebaseService.GetAllTechniciansAsync()
        ViewBag.SaleManagers = Await _firebaseService.GetAllSaleManagersAsync()
        ViewBag.Software = Await _firebaseService.GetAllSoftwareAsync()
        Return View(task)
    End Function
    
    ' GET: Tasks/Edit/5
    Async Function Edit(id As String) As Task(Of ActionResult)
        If String.IsNullOrEmpty(id) Then
            Return RedirectToAction("Index")
        End If
        
        Dim task = Await _firebaseService.GetTaskByIdAsync(id)
        If task Is Nothing Then
            Return HttpNotFound()
        End If
        
        ViewBag.Technicians = Await _firebaseService.GetAllTechniciansAsync()
        ViewBag.SaleManagers = Await _firebaseService.GetAllSaleManagersAsync()
        ViewBag.Software = Await _firebaseService.GetAllSoftwareAsync()
        Return View(task)
    End Function
    
    ' POST: Tasks/Edit/5
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Edit(id As String, task As Task, uploadedFiles As HttpPostedFileBase(), uploadedImages As HttpPostedFileBase()) As Task(Of ActionResult)
        ' Khởi tạo task nếu null
        If task Is Nothing Then
            task = New Task()
        End If
        
        ' Lấy ID từ route hoặc form
        If String.IsNullOrEmpty(task.Id) AndAlso Not String.IsNullOrEmpty(id) Then
            task.Id = id
        End If
        If String.IsNullOrEmpty(task.Id) Then
            Dim idFromForm = Request.Form("Id")
            If Not String.IsNullOrEmpty(idFromForm) Then
                task.Id = idFromForm
            End If
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
        
        ' Lấy dữ liệu từ form
        Dim descriptionValue As String = Request.Form("Description")
        Dim customerNameValue As String = Request.Form("CustomerName")
        Dim solutionValue As String = Request.Form("Solution")
        Dim responseValue As String = Request.Form("ResponseToCustomer")
        Dim tagValue As String = Request.Form("Tag")
        Dim fileReceivedDateValue As String = Request.Form("FileReceivedDate")
        Dim expectedCompletionDateValue As String = Request.Form("ExpectedCompletionDate")
        Dim completedDateValue As String = Request.Form("CompletedDate")
        Dim supportPlatformValue As String = Request.Form("SupportPlatform")
        Dim saleManagerIdValue As String = Request.Form("SaleManagerId")
        Dim softwareIdValue As String = Request.Form("SoftwareId")
        
        If Not String.IsNullOrEmpty(descriptionValue) Then
            task.Description = descriptionValue
        End If
        If Not String.IsNullOrEmpty(customerNameValue) Then
            task.CustomerName = customerNameValue
        End If
        If Not String.IsNullOrEmpty(solutionValue) Then
            task.Solution = solutionValue
        End If
        If Not String.IsNullOrEmpty(responseValue) Then
            task.ResponseToCustomer = responseValue
        End If
        If Not String.IsNullOrEmpty(tagValue) Then
            task.Tag = tagValue
        End If
        
        ' Xử lý ngày nhận file
        If Not String.IsNullOrEmpty(fileReceivedDateValue) Then
            Dim fileReceivedDate As DateTime
            If DateTime.TryParse(fileReceivedDateValue, fileReceivedDate) Then
                task.FileReceivedDate = fileReceivedDate
            End If
        End If
        
        ' Xử lý ngày dự kiến hoàn thành
        If Not String.IsNullOrEmpty(expectedCompletionDateValue) Then
            Dim expectedDate As DateTime
            If DateTime.TryParse(expectedCompletionDateValue, expectedDate) Then
                task.ExpectedCompletionDate = expectedDate
            End If
        End If
        
        ' Xử lý ngày thực tế hoàn thành
        If Not String.IsNullOrEmpty(completedDateValue) Then
            Dim completedDate As DateTime
            If DateTime.TryParse(completedDateValue, completedDate) Then
                task.CompletedDate = completedDate
            End If
        End If
        
        ' Xử lý nền tảng hỗ trợ
        If Not String.IsNullOrEmpty(supportPlatformValue) Then
            Dim platformInt As Integer
            If Integer.TryParse(supportPlatformValue, platformInt) Then
                task.SupportPlatform = CType(platformInt, SupportPlatform)
            End If
        End If
        
        ' Xử lý Sale Manager
        If Not String.IsNullOrEmpty(saleManagerIdValue) Then
            task.SaleManagerId = saleManagerIdValue
        End If
        
        ' Xử lý Software
        If Not String.IsNullOrEmpty(softwareIdValue) Then
            task.SoftwareId = softwareIdValue
        End If
        
        ' Xử lý Status từ form
        Dim statusValue As String = Request.Form("Status")
        If Not String.IsNullOrEmpty(statusValue) Then
            Dim statusInt As Integer
            If Integer.TryParse(statusValue, statusInt) Then
                task.Status = CType(statusInt, TaskStatus)
            End If
        End If
        
        ' Xử lý AssignedTo từ form
        Dim assignedToValue As String = Request.Form("AssignedTo")
        If Not String.IsNullOrEmpty(assignedToValue) Then
            task.AssignedTo = assignedToValue
        End If
        
        ' Validate lại
        Dim validationErrors As New List(Of String)
        If String.IsNullOrEmpty(task.Description) Then
            validationErrors.Add("Mô tả")
        End If
        If String.IsNullOrEmpty(task.CustomerName) Then
            validationErrors.Add("Khách hàng")
        End If
        If String.IsNullOrEmpty(task.SaleManagerId) Then
            validationErrors.Add("Sale quản lý")
        End If
        If String.IsNullOrEmpty(task.SoftwareId) Then
            validationErrors.Add("Phần mềm sử dụng")
        End If
        If String.IsNullOrEmpty(task.AssignedTo) Then
            validationErrors.Add("Kỹ thuật phụ trách")
        End If
        
        If validationErrors.Count > 0 Then
            ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin bắt buộc: " & String.Join(", ", validationErrors))
            ViewBag.Technicians = Await _firebaseService.GetAllTechniciansAsync()
            ViewBag.SaleManagers = Await _firebaseService.GetAllSaleManagersAsync()
            ViewBag.Software = Await _firebaseService.GetAllSoftwareAsync()
            Return View(task)
        End If
        
        If Not String.IsNullOrEmpty(task.Id) AndAlso Not String.IsNullOrEmpty(task.Description) AndAlso Not String.IsNullOrEmpty(task.CustomerName) AndAlso Not String.IsNullOrEmpty(task.SaleManagerId) AndAlso Not String.IsNullOrEmpty(task.SoftwareId) AndAlso Not String.IsNullOrEmpty(task.AssignedTo) Then
            ' Lấy task hiện tại để giữ lại dữ liệu cũ
            Dim existingTask = Await _firebaseService.GetTaskByIdAsync(task.Id)
            If existingTask IsNot Nothing Then
                ' Giữ lại attachments và images cũ
                If existingTask.Attachments IsNot Nothing Then
                    task.Attachments = existingTask.Attachments
                End If
                If existingTask.Images IsNot Nothing Then
                    task.Images = existingTask.Images
                End If
                If existingTask.History IsNot Nothing Then
                    task.History = existingTask.History
                End If
            End If
            
            ' Lấy tên người phụ trách
            If Not String.IsNullOrEmpty(task.AssignedTo) Then
                Dim technicians = Await _firebaseService.GetAllTechniciansAsync()
                Dim tech = technicians.FirstOrDefault(Function(t) t.Id = task.AssignedTo)
                If tech IsNot Nothing Then
                    task.AssignedToName = tech.Name
                End If
            End If
            
            ' Lấy tên Sale Manager
            If Not String.IsNullOrEmpty(task.SaleManagerId) Then
                Dim saleManagers = Await _firebaseService.GetAllSaleManagersAsync()
                Dim sale = saleManagers.FirstOrDefault(Function(s) s.Id = task.SaleManagerId)
                If sale IsNot Nothing Then
                    task.SaleManagerName = sale.Name
                End If
            End If
            
            ' Lấy tên Software
            If Not String.IsNullOrEmpty(task.SoftwareId) Then
                Dim software = Await _firebaseService.GetAllSoftwareAsync()
                Dim soft = software.FirstOrDefault(Function(s) s.Id = task.SoftwareId)
                If soft IsNot Nothing Then
                    task.SoftwareName = soft.Name
                End If
            End If
            
            ' Tạo thư mục nếu chưa có
            Dim uploadsDir = Server.MapPath("~/App_Data/Uploads")
            Dim imagesDir = Server.MapPath("~/App_Data/Images")
            If Not System.IO.Directory.Exists(uploadsDir) Then
                System.IO.Directory.CreateDirectory(uploadsDir)
            End If
            If Not System.IO.Directory.Exists(imagesDir) Then
                System.IO.Directory.CreateDirectory(imagesDir)
            End If
            
            ' Xử lý upload files mới
            If uploadedFiles IsNot Nothing Then
                For Each uploadedFile In uploadedFiles
                    If uploadedFile IsNot Nothing AndAlso uploadedFile.ContentLength > 0 Then
                        Dim fileName = System.IO.Path.GetFileName(uploadedFile.FileName)
                        Dim uniqueFileName = $"{Guid.NewGuid()}_{fileName}"
                        Dim path = System.IO.Path.Combine(uploadsDir, uniqueFileName)
                        uploadedFile.SaveAs(path)
                        task.Attachments.Add($"/File/Download?filePath=/App_Data/Uploads/{uniqueFileName}")
                    End If
                Next
            End If
            
            ' Xử lý upload images mới
            If uploadedImages IsNot Nothing Then
                For Each uploadedImage In uploadedImages
                    If uploadedImage IsNot Nothing AndAlso uploadedImage.ContentLength > 0 Then
                        Dim fileName = System.IO.Path.GetFileName(uploadedImage.FileName)
                        Dim uniqueFileName = $"{Guid.NewGuid()}_{fileName}"
                        Dim path = System.IO.Path.Combine(imagesDir, uniqueFileName)
                        uploadedImage.SaveAs(path)
                        task.Images.Add($"/File/Download?filePath=/App_Data/Images/{uniqueFileName}")
                    End If
                Next
            End If
            
            ' Thêm lịch sử cập nhật
            Dim history = New TaskHistory With {
                .TaskId = task.Id,
                .Action = "Updated",
                .Description = "Cập nhật công việc",
                .ChangedBy = "system",
                .ChangedByName = "System"
            }
            task.History.Add(history)
            
            ' Nếu status là Completed, set CompletedDate
            If task.Status = TaskStatus.Completed AndAlso Not task.CompletedDate.HasValue Then
                task.CompletedDate = DateTime.Now
            End If
            
            Try
                Dim success = Await _firebaseService.UpdateTaskAsync(task)
                If success Then
                    Return RedirectToAction("Index")
                Else
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật công việc vào Firebase. Vui lòng kiểm tra:")
                    ModelState.AddModelError("", "1. Kết nối internet")
                    ModelState.AddModelError("", "2. Cấu hình Firebase URL trong Web.config")
                    ModelState.AddModelError("", "3. Firebase Database Rules cho phép ghi dữ liệu")
                End If
            Catch ex As Exception
                ModelState.AddModelError("", $"Lỗi khi cập nhật công việc: {ex.Message}")
                If ex.InnerException IsNot Nothing Then
                    ModelState.AddModelError("", $"Chi tiết: {ex.InnerException.Message}")
                End If
            End Try
        End If
        
        ViewBag.Technicians = Await _firebaseService.GetAllTechniciansAsync()
        ViewBag.SaleManagers = Await _firebaseService.GetAllSaleManagersAsync()
        ViewBag.Software = Await _firebaseService.GetAllSoftwareAsync()
        Return View(task)
    End Function
    
    ' GET: Tasks/Delete/5
    Async Function Delete(id As String) As Task(Of ActionResult)
        If String.IsNullOrEmpty(id) Then
            Return RedirectToAction("Index")
        End If
        
        Dim task = Await _firebaseService.GetTaskByIdAsync(id)
        If task Is Nothing Then
            Return HttpNotFound()
        End If
        
        Return View(task)
    End Function
    
    ' POST: Tasks/Delete/5
    <HttpPost>
    <ActionName("Delete")>
    <ValidateAntiForgeryToken>
    Async Function DeleteConfirmed(id As String) As Task(Of ActionResult)
        Dim success = Await _firebaseService.DeleteTaskAsync(id)
        Return RedirectToAction("Index")
    End Function
    
    ' POST: Tasks/UpdateStatus - AJAX endpoint để cập nhật status nhanh
    <HttpPost>
    Async Function UpdateStatus(id As String, status As Integer) As Task(Of JsonResult)
        Try
            If String.IsNullOrEmpty(id) Then
                Return Json(New With {.success = False, .message = "ID công việc không hợp lệ"}, JsonRequestBehavior.AllowGet)
            End If
            
            Dim task = Await _firebaseService.GetTaskByIdAsync(id)
            If task Is Nothing Then
                Return Json(New With {.success = False, .message = "Không tìm thấy công việc"}, JsonRequestBehavior.AllowGet)
            End If
            
            ' Lưu status cũ trước khi thay đổi
            Dim oldStatus = task.Status
            Dim oldStatusText = ""
            Select Case oldStatus
                Case TaskStatus.Pending
                    oldStatusText = "Chưa xử lý"
                Case TaskStatus.InProgress
                    oldStatusText = "Đang xử lý"
                Case TaskStatus.Waiting
                    oldStatusText = "Chờ phản hồi"
                Case TaskStatus.Completed
                    oldStatusText = "Đã hoàn thành"
                Case TaskStatus.Paused
                    oldStatusText = "Tạm dừng"
            End Select
            
            ' Cập nhật status mới
            Dim newStatus = CType(status, TaskStatus)
            task.Status = newStatus
            task.UpdatedDate = DateTime.Now
            
            ' Nếu status là Completed, set CompletedDate
            If task.Status = TaskStatus.Completed AndAlso Not task.CompletedDate.HasValue Then
                task.CompletedDate = DateTime.Now
            End If
            
            ' Thêm vào history
            If task.History Is Nothing Then
                task.History = New List(Of TaskHistory)
            End If
            
            Dim newStatusText = ""
            Select Case newStatus
                Case TaskStatus.Pending
                    newStatusText = "Chưa xử lý"
                Case TaskStatus.InProgress
                    newStatusText = "Đang xử lý"
                Case TaskStatus.Waiting
                    newStatusText = "Chờ phản hồi"
                Case TaskStatus.Completed
                    newStatusText = "Đã hoàn thành"
                Case TaskStatus.Paused
                    newStatusText = "Tạm dừng"
            End Select
            
            task.History.Add(New TaskHistory With {
                .Id = Guid.NewGuid().ToString(),
                .TaskId = task.Id,
                .Action = "StatusChanged",
                .Description = $"Trạng thái thay đổi từ {oldStatusText} sang {newStatusText}",
                .ChangedBy = "System",
                .ChangedByName = "System",
                .ChangedDate = DateTime.Now,
                .OldValue = oldStatusText,
                .NewValue = newStatusText
            })
            
            Dim success = Await _firebaseService.UpdateTaskAsync(task)
            If success Then
                Return Json(New With {.success = True, .message = "Cập nhật trạng thái thành công"}, JsonRequestBehavior.AllowGet)
            Else
                Return Json(New With {.success = False, .message = "Có lỗi xảy ra khi cập nhật"}, JsonRequestBehavior.AllowGet)
            End If
        Catch ex As Exception
            Return Json(New With {.success = False, .message = $"Lỗi: {ex.Message}"}, JsonRequestBehavior.AllowGet)
        End Try
    End Function
    
    ' POST: Tasks/ReplyToCustomer
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function ReplyToCustomer(id As String, responseToCustomer As String) As Task(Of JsonResult)
        Try
            If String.IsNullOrEmpty(id) OrElse String.IsNullOrEmpty(responseToCustomer) Then
                Return Json(New With {.success = False, .message = "ID công việc và nội dung trả lời không được để trống"}, JsonRequestBehavior.AllowGet)
            End If
            
            ' Lấy task từ Firebase
            Dim task = Await _firebaseService.GetTaskByIdAsync(id)
            If task Is Nothing Then
                Return Json(New With {.success = False, .message = "Không tìm thấy công việc"}, JsonRequestBehavior.AllowGet)
            End If
            
            ' Lưu status cũ để ghi history
            Dim oldStatus = task.Status
            Dim oldStatusText = ""
            Select Case oldStatus
                Case TaskStatus.Pending
                    oldStatusText = "Chưa xử lý"
                Case TaskStatus.InProgress
                    oldStatusText = "Đang xử lý"
                Case TaskStatus.Waiting
                    oldStatusText = "Chờ phản hồi"
                Case TaskStatus.Completed
                    oldStatusText = "Đã hoàn thành"
                Case TaskStatus.Paused
                    oldStatusText = "Tạm dừng"
            End Select
            
            ' Cập nhật ResponseToCustomer
            task.ResponseToCustomer = responseToCustomer
            
            ' Chuyển status sang Completed
            task.Status = TaskStatus.Completed
            
            ' Cập nhật CompletedDate và UpdatedDate (luôn cập nhật khi trả lời)
            task.CompletedDate = DateTime.Now
            task.UpdatedDate = DateTime.Now
            
            ' Thêm vào history
            If task.History Is Nothing Then
                task.History = New List(Of TaskHistory)
            End If
            
            task.History.Add(New TaskHistory With {
                .Id = Guid.NewGuid().ToString(),
                .TaskId = task.Id,
                .Action = "Replied",
                .Description = "Đã trả lời khách hàng và chuyển trạng thái sang 'Đã hoàn thành'",
                .ChangedBy = "System",
                .ChangedByName = "System",
                .ChangedDate = DateTime.Now,
                .OldValue = oldStatusText,
                .NewValue = "Đã hoàn thành"
            })
            
            ' Lưu vào Firebase
            Dim success = Await _firebaseService.UpdateTaskAsync(task)
            If success Then
                Return Json(New With {.success = True, .message = "Trả lời đã được gửi thành công. Công việc đã chuyển sang trạng thái 'Đã hoàn thành'."}, JsonRequestBehavior.AllowGet)
            Else
                Return Json(New With {.success = False, .message = "Có lỗi xảy ra khi lưu trả lời"}, JsonRequestBehavior.AllowGet)
            End If
        Catch ex As Exception
            Return Json(New With {.success = False, .message = $"Lỗi: {ex.Message}"}, JsonRequestBehavior.AllowGet)
        End Try
    End Function
End Class
