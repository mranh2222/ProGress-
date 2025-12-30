Imports System.Threading.Tasks
Imports System.Web.Mvc
Imports System.Diagnostics

Public Class TechniciansController
    Inherits Controller
    
    Private ReadOnly _firebaseService As FirebaseService
    
    Public Sub New()
        _firebaseService = New FirebaseService()
    End Sub
    
    ' GET: Technicians/Create
    Async Function Create() As Task(Of ActionResult)
        Return View(New Technician())
    End Function
    
    ' POST: Technicians/Create
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Create(technician As Technician) As Task(Of ActionResult)
        Try
            ' Lấy dữ liệu từ form nếu model binding không hoạt động
            Dim name As String = If(Not String.IsNullOrEmpty(technician?.Name), technician.Name, Request.Form("Name"))
            Dim email As String = If(Not String.IsNullOrEmpty(technician?.Email), technician.Email, Request.Form("Email"))
            Dim phone As String = If(Not String.IsNullOrEmpty(technician?.Phone), technician.Phone, Request.Form("Phone"))
            
            ' Validate
            If String.IsNullOrEmpty(name) OrElse String.IsNullOrWhiteSpace(name) Then
                ModelState.AddModelError("Name", "Tên kỹ thuật viên không được để trống")
                If technician Is Nothing Then
                    technician = New Technician()
                End If
                Return View(technician)
            End If
            
            ' Tạo Technician mới hoặc cập nhật
            If technician Is Nothing Then
                technician = New Technician()
            End If
            
            technician.Name = name.Trim()
            If Not String.IsNullOrEmpty(email) Then
                technician.Email = email.Trim()
            End If
            If Not String.IsNullOrEmpty(phone) Then
                technician.Phone = phone.Trim()
            End If
            technician.IsActive = True
            
            ' Đảm bảo có ID (Technician constructor đã tạo ID)
            If String.IsNullOrEmpty(technician.Id) Then
                technician.Id = Guid.NewGuid().ToString()
            End If
            
            ' Lưu vào Firebase
            Dim success = Await _firebaseService.CreateTechnicianAsync(technician)
            
            If success Then
                TempData("SuccessMessage") = "Thêm kỹ thuật viên thành công!"
                Return RedirectToAction("Index", "Dashboard")
            Else
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu kỹ thuật viên. Vui lòng kiểm tra kết nối Firebase và thử lại.")
            End If
        Catch ex As Exception
            ModelState.AddModelError("", "Lỗi: " & ex.Message)
            Debug.WriteLine("TechniciansController.Create Error: " & ex.Message)
            Debug.WriteLine("Stack Trace: " & ex.StackTrace)
            If ex.InnerException IsNot Nothing Then
                Debug.WriteLine("Inner Exception: " & ex.InnerException.Message)
            End If
        End Try
        
        If technician Is Nothing Then
            technician = New Technician()
        End If
        Return View(technician)
    End Function
End Class

