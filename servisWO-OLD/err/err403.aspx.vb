Imports System.Reflection

Public Class err403
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Page.Title = "Web Order v. " & Assembly.GetExecutingAssembly().GetName().Version.Major.ToString & "." & Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString
    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs) Handles ASPxButton1.Click
        Try
            Session.Abandon()
            Response.Redirect("~/login.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/login.aspx")
        End Try
    End Sub
End Class