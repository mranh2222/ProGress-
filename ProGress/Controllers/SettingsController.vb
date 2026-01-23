Imports System.Web.Mvc
Imports System.Configuration

Public Class SettingsController
    Inherits Controller
    
    ' GET: Settings
    Function Index() As ActionResult
        ' Lấy thông tin connection string từ Web.config
        Dim connectionString = ConfigurationManager.ConnectionStrings("DefaultConnection")
        If connectionString IsNot Nothing Then
            ViewBag.ConnectionString = connectionString.ConnectionString
            ViewBag.DatabaseName = GetDatabaseName(connectionString.ConnectionString)
            ViewBag.ServerName = GetServerName(connectionString.ConnectionString)
        Else
            ViewBag.ConnectionString = "Chưa cấu hình"
            ViewBag.DatabaseName = "N/A"
            ViewBag.ServerName = "N/A"
        End If
        
        Return View()
    End Function
    
    Private Function GetDatabaseName(connString As String) As String
        Dim dbMatch = System.Text.RegularExpressions.Regex.Match(connString, "(?:Database|Initial Catalog)=([^;]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Return If(dbMatch.Success, dbMatch.Groups(1).Value, "N/A")
    End Function
    
    Private Function GetServerName(connString As String) As String
        Dim serverMatch = System.Text.RegularExpressions.Regex.Match(connString, "(?:Server|Data Source)=([^;]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Return If(serverMatch.Success, serverMatch.Groups(1).Value, "N/A")
    End Function
End Class









