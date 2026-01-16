Imports System

Public Class Question
    Public Property Id As String
    Public Property TechnicianId As String ' ID của kỹ thuật viên đặt câu hỏi
    Public Property TechnicianName As String ' Tên kỹ thuật viên
    Public Property Question As String ' Nội dung câu hỏi
    Public Property Answer As String ' Câu trả lời từ giám đốc
    Public Property Status As QuestionStatus ' Trạng thái: Pending hoặc Answered
    Public Property CreatedDate As DateTime ' Ngày tạo câu hỏi
    Public Property AnsweredDate As DateTime? ' Ngày trả lời
    Public Property Images As List(Of String) ' Danh sách URL hình ảnh
    Public Property Attachments As List(Of String) ' Danh sách URL file đính kèm
    
    Public Sub New()
        Id = Guid.NewGuid().ToString()
        CreatedDate = DateTime.Now
        Status = QuestionStatus.Pending
        Images = New List(Of String)
        Attachments = New List(Of String)
    End Sub
End Class

Public Enum QuestionStatus
    Pending = 0 ' Chờ trả lời
    Answered = 1 ' Đã trả lời
End Enum

