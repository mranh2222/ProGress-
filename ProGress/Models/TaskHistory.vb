Imports System

Public Class TaskHistory
    Public Property Id As String
    Public Property TaskId As String
    Public Property Action As String ' "Created", "Assigned", "StatusChanged", "Updated"
    Public Property Description As String
    Public Property ChangedBy As String
    Public Property ChangedByName As String
    Public Property ChangedDate As DateTime
    Public Property OldValue As String
    Public Property NewValue As String
    
    Public Sub New()
        Id = Guid.NewGuid().ToString()
        ChangedDate = DateTime.Now
    End Sub
End Class











