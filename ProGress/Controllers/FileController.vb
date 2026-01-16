Imports System.Web.Mvc
Imports System.IO
Imports System.Web
Imports System.Threading.Tasks

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
        
        ' Chuyển đổi URL thành đường dẫn vật lý
        Dim relativePath = filePath
        If relativePath.StartsWith("/") Then
            relativePath = "~" & relativePath
        Else
            relativePath = "~/" & relativePath
        End If
        
        Dim physicalPath = Server.MapPath(relativePath)
        
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
            Case ".edb", ".fdb"
                contentType = "application/octet-stream"
        End Select
        
        Return File(fileBytes, contentType, fileName)
    End Function
End Class
