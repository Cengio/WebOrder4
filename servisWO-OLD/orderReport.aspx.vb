Imports servisReports.reportordini

Public Class orderReport
    Inherits System.Web.UI.Page
    Private Sub orderReport_Init(sender As Object, e As EventArgs) Handles Me.Init
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
        If Not Session("orderCodeReport") Is Nothing AndAlso Session("orderCodeReport") <> "" Then
            ReportViewer1.Report = GetReport()
        End If
    End Sub

    Private Function GetReport() As repOrdine
        Dim newreport As New repOrdine
        newreport.DataSource = GetData()
        newreport.Name = "ORD" & Session("orderCodeReport")
        Return newreport
    End Function

    Private Function GetData() As orderHeaderReportCollection
        Dim dm As New DataManager
        Dim drm As New dataReportManager
        drm.NAVconnectionString = dm.GetNAVconnectionString
        drm.WORconnectionString = dm.GetWORconnectionString
        drm.workingCompany = dm.GetWorkingCompany
        If Not Session("showReportDataUltimaModifica") Is Nothing AndAlso Session("showReportDataUltimaModifica") = "1" Then
            drm.showDataUltimaModifica = True
        End If
        Dim linesOrderByField As String = "LineID"
        If Not Session("linesOrderByField") Is Nothing AndAlso Session("linesOrderByField") <> "" Then
            linesOrderByField = Session("linesOrderByField")
        End If
        Dim rep As orderHeaderReportCollection = drm.getOrder(Session("orderCodeReport"), linesOrderByField)
        drm = Nothing
        dm = Nothing
        Return rep
    End Function

    Protected Sub ReportViewer1_Unload(sender As Object, e As EventArgs) Handles ReportViewer1.Unload
        ReportViewer1.Report = Nothing
    End Sub


End Class