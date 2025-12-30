Public Class Technician
    Public Property Id As String
    Public Property Name As String
    Public Property Email As String
    Public Property Phone As String
    Public Property IsActive As Boolean = True
    
    Public Sub New()
        Id = Guid.NewGuid().ToString()
    End Sub
End Class
