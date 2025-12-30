Imports System.Web.Mvc
Imports System.IO
Imports System.Web

Public Class FileController
    Inherits Controller
    
    ' GET: File/Download
    Function Download(filePath As String) As ActionResult
        If String.IsNullOrEmpty(filePath) Then
            Return HttpNotFound()
        End If
        
        ' Bảo mật: chỉ cho phép truy cập file trong App_Data
        If Not filePath.StartsWith("/App_Data/") AndAlso Not filePath.StartsWith("App_Data/") Then
            Return HttpNotFound()
        End If
        
        ' Loại bỏ /App_Data/ để lấy đường dẫn thực
        Dim relativePath = filePath.Replace("/App_Data/", "").Replace("App_Data/", "")
        Dim physicalPath = Server.MapPath("~/App_Data/" & relativePath)
        
        If Not System.IO.File.Exists(physicalPath) Then
            Return HttpNotFound()
        End If
        
        Dim fileBytes = System.IO.File.ReadAllBytes(physicalPath)
        Dim fileName = Path.GetFileName(physicalPath)
        
        ' Xác định content type
        Dim contentType As String = "application/octet-stream"
        Dim ext = Path.GetExtension(fileName).ToLower()
        Select Case ext
            Case ".jpg", ".jpeg"
                contentType = "image/jpeg"
            Case ".png"
                contentType = "image/png"
            Case ".gif"
                contentType = "image/gif"
            Case ".pdf"
                contentType = "application/pdf"
            Case ".doc", ".docx"
                contentType = "application/msword"
            Case ".txt", ".log"
                contentType = "text/plain"
            Case ".edb"
                contentType = "application/octet-stream"
        End Select
        
        Return File(fileBytes, contentType, fileName)
    End Function
End Class
