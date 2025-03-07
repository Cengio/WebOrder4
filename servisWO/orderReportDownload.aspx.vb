Imports System.IO
Imports System.Reflection
Imports log4net
Imports servisWO.datamanager

Public Class orderReportDownload
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("id") IsNot Nothing Then
            DownloadPDF(Request("id"))
        End If
    End Sub

    Private Sub DownloadPDF(orderNo As String)
        Try
            Dim dm As New datamanager
            Dim percorsoPDF As String = dm.GetParametroSito(parametriSitoValue.percorsoPDF)
            Dim ReportName As String = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).prefixOrderFileName & "ORD" & orderNo & ".pdf"
            If percorsoPDF <> "" AndAlso Directory.Exists(Server.MapPath("~/" & percorsoPDF)) Then
                Dim fullpath As String = Server.MapPath("~/" & percorsoPDF & "/" & ReportName)
                If File.Exists(fullpath) Then
                    Response.ContentType = "application/pdf"
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" & ReportName)
                    Response.TransmitFile(fullpath)
                    Response.End()
                    Log.Info("Order PDF was downloaded from " & fullpath)
                Else
                    Response.SuppressContent = False
                    Response.Write("No file is found: " & ReportName)
                    Response.End()
                    Return
                End If
            End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Exit Sub
        End Try
    End Sub


End Class