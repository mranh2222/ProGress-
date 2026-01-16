Imports System.IO
Imports System.Web
Imports ThreadingTask = System.Threading.Tasks

Public Class FileStorageService
    Private ReadOnly _uploadPath As String

    Public Sub New()
        _uploadPath = HttpContext.Current.Server.MapPath("~/App_Data/Uploads")
        If Not Directory.Exists(_uploadPath) Then
            Directory.CreateDirectory(_uploadPath)
        End If
    End Sub

    ''' <summary>
    ''' Upload file lên thư mục local (Synchronous wrap in Task)
    ''' </summary>
    Public Function UploadFileAsync(postedFile As HttpPostedFileBase, folder As String) As ThreadingTask.Task(Of String)
        Try
            If postedFile Is Nothing OrElse postedFile.ContentLength = 0 Then
                Return ThreadingTask.Task.FromResult(Of String)(Nothing)
            End If

            Dim fileName = Path.GetFileName(postedFile.FileName)
            Dim uniqueFileName = $"{Guid.NewGuid()}_{fileName}"
            
            Dim subPath = _uploadPath
            If Not String.IsNullOrEmpty(folder) Then
                subPath = Path.Combine(_uploadPath, folder)
                If Not Directory.Exists(subPath) Then
                    Directory.CreateDirectory(subPath)
                End If
            End If

            Dim physicalPath = Path.Combine(subPath, uniqueFileName)
            postedFile.SaveAs(physicalPath)

            Dim relativeUrl = "/App_Data/Uploads/"
            If Not String.IsNullOrEmpty(folder) Then
                relativeUrl &= folder & "/"
            End If
            relativeUrl &= uniqueFileName

            Return ThreadingTask.Task.FromResult(relativeUrl)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"FileStorageService.UploadFileAsync Error: {ex.Message}")
            Return ThreadingTask.Task.FromResult(Of String)(Nothing)
        End Try
    End Function

    ''' <summary>
    ''' Upload file từ byte array (Synchronous wrap in Task)
    ''' </summary>
    Public Function UploadFileAsync(fileBytes As Byte(), fileName As String, contentType As String, folder As String) As ThreadingTask.Task(Of String)
        Try
            If fileBytes Is Nothing OrElse fileBytes.Length = 0 Then
                Return ThreadingTask.Task.FromResult(Of String)(Nothing)
            End If

            Dim uniqueFileName = $"{Guid.NewGuid()}_{fileName}"
            Dim subPath = _uploadPath
            If Not String.IsNullOrEmpty(folder) Then
                subPath = Path.Combine(_uploadPath, folder)
                If Not Directory.Exists(subPath) Then
                    Directory.CreateDirectory(subPath)
                End If
            End If

            Dim physicalPath = Path.Combine(subPath, uniqueFileName)
            File.WriteAllBytes(physicalPath, fileBytes)

            Dim relativeUrl = "/App_Data/Uploads/"
            if Not String.IsNullOrEmpty(folder) Then
                relativeUrl &= folder & "/"
            End If
            relativeUrl &= uniqueFileName

            Return ThreadingTask.Task.FromResult(relativeUrl)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"FileStorageService.UploadFileAsync Error: {ex.Message}")
            Return ThreadingTask.Task.FromResult(Of String)(Nothing)
        End Try
    End Function

    Public Function DeleteFileAsync(relativeUrl As String) As ThreadingTask.Task(Of Boolean)
        Try
            If String.IsNullOrEmpty(relativeUrl) Then
                Return ThreadingTask.Task.FromResult(False)
            End If

            Dim physicalPath = HttpContext.Current.Server.MapPath(relativeUrl)
            If File.Exists(physicalPath) Then
                File.Delete(physicalPath)
                Return ThreadingTask.Task.FromResult(True)
            End If
            
            Return ThreadingTask.Task.FromResult(False)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"FileStorageService.DeleteFileAsync Error: {ex.Message}")
            Return ThreadingTask.Task.FromResult(False)
        End Try
    End Function
End Class
