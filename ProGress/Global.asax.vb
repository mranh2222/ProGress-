Imports System.Web.Http
Imports System.Web.Optimization

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub
    
    Protected Sub Application_BeginRequest(sender As Object, e As EventArgs)
        Response.ContentEncoding = System.Text.Encoding.UTF8
        Response.Charset = "utf-8"
        If Response.ContentType Is Nothing OrElse Not Response.ContentType.Contains("charset") Then
            If Response.ContentType Is Nothing Then
                Response.ContentType = "text/html; charset=utf-8"
            Else
                Response.ContentType = Response.ContentType & "; charset=utf-8"
            End If
        End If
    End Sub
End Class
