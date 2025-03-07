Imports System.Net.Mail
Imports DevExpress.Web
Imports servisWO.datamanager

Public Class orderListNav
    Inherits System.Web.UI.Page
    Private Sub orderList_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSordini Then    'utente amministratore
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

    ' Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    ' If Not Page.IsPostBack Then
    '         grid.Columns.Item("btnReport").Visible = False
    '         grid.Columns.Item("btnUpdate").Visible = False
    '         Dim dm As New datamanager
    '         SqlDataSourceOrdini.ConnectionString = dm.GetNAVconnectionString()
    '         SqlDataSourceOrdini.SelectCommand = dm.getNavOrderListSelectCommand()
    '         SqlDataSourceOrdini.Select(DataSourceSelectArguments.Empty)
    '         SqlDataSourceOrdini.DataBind()
    '         grid.DataBind()
    '         dm = Nothing
    '     'End If
    ' End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' If Not Page.IsPostBack Then
            grid.Columns.Item("btnReport").Visible = False
            grid.Columns.Item("btnUpdate").Visible = False
            Dim dm As New datamanager
            SqlDataSourceOrdini.ConnectionString = dm.GetNAVconnectionString()
            SqlDataSourceOrdini.SelectCommand = dm.getNavOrderListSelectCommand()
            SqlDataSourceOrdini.Select(DataSourceSelectArguments.Empty)
            SqlDataSourceOrdini.DataBind()
            grid.DataBind()
            dm = Nothing
        ' End If
    End Sub

    Protected Sub grid_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles grid.CustomColumnDisplayText
        Dim dm As New datamanager
        If e.Column.FieldName = "Status" Then
            e.DisplayText = orderstatus.getDescription(e.Value, e.GetFieldValue("CompletedImported"))
        End If
        dm = Nothing
    End Sub

    Private Sub grid_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles grid.HtmlRowPrepared
        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='silver';")
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';")
    End Sub

End Class