Imports System.Reflection
Imports System.Web.SessionState
Imports DevExpress.Web
Imports log4net


Public Class Global_asax
    Inherits System.Web.HttpApplication

    Dim Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(System.Web.Http.GlobalConfiguration.Configuration)
AddHandler DevExpress.Web.ASPxWebControl.CallbackError, AddressOf Application_Error
        Log.Info("Web Order " & Assembly.GetExecutingAssembly().GetName().Version.Major.ToString & "." & Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString & " STARTED")
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        'Log.Info("Session " & Session.SessionID & " opened")
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        'HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(False)
        'HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'HttpContext.Current.Response.Cache.SetNoStore()
        'Response.Cache.SetExpires(DateTime.Now)
        'Response.Cache.SetValidUntilExpires(True)
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
        'Log.Info("Session " & Session.SessionID & " closed")
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
        Log.Info("Web Order STOPPED")
    End Sub

End Class