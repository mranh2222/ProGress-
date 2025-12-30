Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        ' Route cho Tasks với ID (GUID)
        routes.MapRoute(
            name:="Tasks",
            url:="Tasks/{action}/{id}",
            defaults:=New With {.controller = "Tasks", .action = "Index", .id = UrlParameter.Optional},
            constraints:=New With {.id = ".+"}
        )

        routes.MapRoute(
            name:="Default",
            url:="{controller}/{action}/{id}",
            defaults:=New With {.controller = "Dashboard", .action = "Index", .id = UrlParameter.Optional}
        )
    End Sub
End Module