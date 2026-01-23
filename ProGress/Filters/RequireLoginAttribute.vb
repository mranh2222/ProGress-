Imports System.Web.Mvc

Public Class RequireLoginAttribute
    Inherits ActionFilterAttribute

    Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
        If filterContext Is Nothing OrElse filterContext.HttpContext Is Nothing Then
            MyBase.OnActionExecuting(filterContext)
            Return
        End If

        Dim controller = filterContext.RouteData.Values("controller")?.ToString()
        Dim action = filterContext.RouteData.Values("action")?.ToString()

        ' Allow anonymous for login/logout
        If String.Equals(controller, "Account", StringComparison.OrdinalIgnoreCase) Then
            If String.Equals(action, "Login", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(action, "Logout", StringComparison.OrdinalIgnoreCase) Then
                MyBase.OnActionExecuting(filterContext)
                Return
            End If
        End If

        ' If not logged in, redirect to Login
        Dim session = filterContext.HttpContext.Session
        If session Is Nothing OrElse session("UserId") Is Nothing Then
            filterContext.Result = New RedirectToRouteResult(New System.Web.Routing.RouteValueDictionary(New With {
                .controller = "Account",
                .action = "Login"
            }))
            Return
        End If

        MyBase.OnActionExecuting(filterContext)
    End Sub
End Class




