Public Class SaleManager
    Public Property Id As String
    Public Property Name As String
    Public Property IsActive As Boolean = True
    
    Public Sub New()
        Id = Guid.NewGuid().ToString()
    End Sub
End Class
