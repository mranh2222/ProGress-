Imports System
Imports System.Web.Mvc

Public Class Task
    Public Property Id As String
    Public Property Tag As String ' Tag công việc
    <AllowHtml>
    Public Property Description As String ' Mô tả lỗi / nội dung hỗ trợ
    Public Property CustomerName As String
    Public Property FileReceivedDate As DateTime? ' Ngày nhận file
    Public Property SupportPlatform As SupportPlatform ' Nền tảng hỗ trợ
    Public Property SaleManagerId As String ' Sale quản lý ID
    Public Property SaleManagerName As String ' Sale quản lý tên
    Public Property SoftwareId As String ' Phần mềm sử dụng ID
    Public Property SoftwareName As String ' Phần mềm sử dụng tên
    Public Property AssignedTo As String ' Kỹ thuật phụ trách ID
    Public Property AssignedToName As String ' Kỹ thuật phụ trách tên
    Public Property Status As TaskStatus ' Tình trạng
    Public Property ExpectedCompletionDate As DateTime? ' Ngày dự kiến hoàn thành
    Public Property CompletedDate As DateTime? ' Ngày thực tế hoàn thành
    Public Property CreatedDate As DateTime
    Public Property UpdatedDate As DateTime?
    Public Property Solution As String
    Public Property ResponseToCustomer As String
    Public Property IsSaved As Boolean ' Đánh dấu câu trả lời đã lưu
    Public Property Attachments As List(Of String) ' URLs của file đính kèm (câu hỏi)
    Public Property Images As List(Of String) ' URLs của hình ảnh (câu hỏi)
    Public Property ResponseAttachments As List(Of String) ' URLs của file đính kèm (phản hồi)
    Public Property ResponseImages As List(Of String) ' URLs của hình ảnh (phản hồi)
    Public Property History As List(Of TaskHistory)
    
    Public Sub New()
        Id = Guid.NewGuid().ToString()
        CreatedDate = DateTime.Now
        Status = TaskStatus.Pending
        SupportPlatform = SupportPlatform.Zalo
        Attachments = New List(Of String)
        Images = New List(Of String)
        ResponseAttachments = New List(Of String)
        ResponseImages = New List(Of String)
        History = New List(Of TaskHistory)
    End Sub
End Class
