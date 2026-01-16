Imports System.Web.Mvc

Public Class SettingsController
    Inherits Controller
    
    ' GET: Settings
    Function Index() As ActionResult
        ' Lấy cấu hình từ Web.config
        ViewBag.FirebaseUrl = System.Configuration.ConfigurationManager.AppSettings("FirebaseUrl")
        ViewBag.FirebaseStorageBucket = System.Configuration.ConfigurationManager.AppSettings("FirebaseStorageBucket")
        
        Return View()
    End Function
End Class







