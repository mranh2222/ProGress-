Imports System.Web.Mvc
Imports ThreadingTask = System.Threading.Tasks

Public Class TechniciansController
    Inherits Controller
    
    Private ReadOnly _dataService As SqlDataService
    
    Public Sub New()
        _dataService = New SqlDataService()
    End Sub
    
    ' GET: Technicians/Create
    Function Create() As ActionResult
        Return View(New Technician())
    End Function
    
    ' POST: Technicians/Create
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Create(technician As Technician) As ThreadingTask.Task(Of ActionResult)
        Try
            Dim name As String = If(Not String.IsNullOrEmpty(technician?.Name), technician.Name, Request.Form("Name"))
            Dim email As String = If(Not String.IsNullOrEmpty(technician?.Email), technician.Email, Request.Form("Email"))
            Dim phone As String = If(Not String.IsNullOrEmpty(technician?.Phone), technician.Phone, Request.Form("Phone"))
            
            If String.IsNullOrEmpty(name) Then
                ModelState.AddModelError("Name", "Tên kỹ thuật viên không được để trống")
                Return View(technician)
            End If
            
            If technician Is Nothing Then technician = New Technician()
            technician.Name = name.Trim()
            technician.Email = If(email, "").Trim()
            technician.Phone = If(phone, "").Trim()
            technician.IsActive = True
            
            If String.IsNullOrEmpty(technician.Id) Then technician.Id = Guid.NewGuid().ToString()
            
            Dim success = Await _dataService.CreateTechnicianAsync(technician)
            
            If success Then
                TempData("SuccessMessage") = "Thêm kỹ thuật viên thành công!"
                Return RedirectToAction("Index", "Dashboard")
            Else
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu vào SQL Server.")
            End If
        Catch ex As Exception
            ModelState.AddModelError("", "Lỗi: " & ex.Message)
        End Try
        
        Return View(technician)
    End Function
End Class
