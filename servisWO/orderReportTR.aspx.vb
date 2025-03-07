Imports servisReports
Imports servisReports.reportordini
Imports Telerik.Reporting
Imports Telerik.Reporting.Services
Imports Telerik.ReportViewer.Html5.WebForms

Public Class orderReportTR
    Inherits System.Web.UI.Page

    Private Sub orderReportTR_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso (CType(Session("user"), user).iSordini Or CType(Session("user"), user).iSmagazzino Or CType(Session("user"), user).iSmagazzinoadmin) Then    'utente amministratore
        Else
            Try
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not Session("orderCodeReport") Is Nothing AndAlso Session("orderCodeReport") <> "" Then
                Dim rptSource = New Telerik.ReportViewer.Html5.WebForms.ReportSource()
                rptSource.IdentifierType = Telerik.ReportViewer.Html5.WebForms.IdentifierType.TypeReportSource
                rptSource.Identifier = GetType(servisWO.Report1).AssemblyQualifiedName

                Dim dm As New datamanager

                rptSource.Parameters.Add("orderCode", Session("orderCodeReport"))
                rptSource.Parameters.Add("NAVconnectionString", dm.GetNAVconnectionString)
                rptSource.Parameters.Add("WORconnectionString", dm.GetWORconnectionString)
                rptSource.Parameters.Add("workingCompany", dm.GetWorkingCompany)
                If Not Session("showReportDataUltimaModifica") Is Nothing AndAlso Session("showReportDataUltimaModifica") = "1" Then
                    rptSource.Parameters.Add("showDataUltimaModifica", True)
                Else
                    rptSource.Parameters.Add("showDataUltimaModifica", False)
                End If
                Dim linesOrderByField As String = "LineID"
                If Not Session("linesOrderByField") Is Nothing AndAlso Session("linesOrderByField") <> "" Then
                    rptSource.Parameters.Add("linesOrderByField", Session("linesOrderByField"))
                Else
                    rptSource.Parameters.Add("linesOrderByField", linesOrderByField)
                End If

                Dim reportLogo As String = "~/images/custom/" & CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).reportLogo
                Dim reportFooterDescription As String = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).reportFooterDescription
                rptSource.Parameters.Add("reportLogo", reportLogo)
                rptSource.Parameters.Add("reportFooterDescription", reportFooterDescription)

                Me.ReportViewer1.ReportSource = rptSource

            End If
        End If
    End Sub
End Class