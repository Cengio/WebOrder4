Imports DevExpress.Web

Public Class newcustomerOrderList
    Inherits System.Web.UI.Page
    Private Sub newcustomerOrderList_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSordini Then
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
        Dim dm As New datamanager
        Call dm.verificaCertificazioneOrdiniNuociCliente()
        'Call dm.synchNewCustomerCertificazione()
        dm = Nothing
        If Not IsPostBack Then
            'grid.DataBind()
            Call bindGrid()
        End If
    End Sub

    Private Sub bindGrid()
        Dim dm As New datamanager
        SqlDataSource_Header.ConnectionString = dm.GetNAVconnectionString()
        SqlDataSource_Header.SelectCommand = dm.getNavNewCustomersOrderListSelectCommand()
        SqlDataSource_Header.Select(DataSourceSelectArguments.Empty)
        SqlDataSource_Header.DataBind()
        grid.DataBind()
        dm = Nothing
    End Sub

    Protected Sub grid_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles grid.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        Try
            If e.CellType = GridViewTableCommandCellType.Data Then
                Dim CustomerNo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CustomerNo").ToString
                Dim Code As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Code").ToString
                If e.ButtonID = "btnUpdate" Then

                    Dim Status As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Status")
                    Dim CompletedImported As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CompletedImported")
                    Dim dm As New datamanager
                    Dim statusproduzione As Integer = dm.getStatusProduzione(Code)
                    dm = Nothing
                    If CompletedImported = 1 Or (Status = 1 And statusproduzione = 0) Or Status = 2 Or Status = 3 Or Status = 4 Or Status = 5 Or Status = 6 Or Status = 7 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine non modificabile"
                        e.Image.Url = "~/images/orderMOD_off.png"
                    ElseIf CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine in corso. Chiudere l'attività prima di poter selezionare un altro cliente."
                        e.Image.Url = "~/images/orderMOD_off.png"
                    Else
                        e.Image.ToolTip = "Vai all'ordine"
                        e.Image.Url = "~/images/orderMOD.png"
                    End If
                End If
                'If e.ButtonID = "btnInfo" Then 'pulsante nascosto
                'End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub grid_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles grid.CustomButtonCallback
        Try
            If e.VisibleIndex = -1 Then
                Return
            End If
            Dim Code As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Code").ToString
            Dim Status As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Status")
            Dim CustomerNo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CustomerNo").ToString
            If Code <> "" Then
                Dim dm As New datamanager
                If e.ButtonID = "btnUpdate" Then
                    CType(Session("cart"), cartManager).clearCart()
                    Session("cart") = CType(Session("cart"), cartManager).loadFromDBbyOrderCode(Code, False, True)
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("orderDetails.aspx")
                ElseIf e.ButtonID = "btnReport" Then
                    Session("orderCodeReport") = Code
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("orderReportTR.aspx")
                End If
                dm = Nothing
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub grid_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles grid.CustomColumnDisplayText
        Dim dm As New datamanager
        If e.Column.FieldName = "Status" Then
            Dim statusproduzione As Integer = dm.getStatusProduzione(e.GetFieldValue("Code"))
            If e.Value = 0 And statusproduzione = 1 Then
                e.DisplayText = "PRODUZIONE COMPLETATA"
            Else
                e.DisplayText = orderstatus.getDescription(e.Value, e.GetFieldValue("CompletedImported"))
            End If
        End If
        If e.Column.FieldName = "OperatorCode" Then
            If e.Value <> "" AndAlso IsNumeric(e.Value) Then
                e.DisplayText = dm.getUserNameSurname(CInt(e.Value))
            End If
        End If
        dm = Nothing
    End Sub

    Private Sub grid_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles grid.HtmlRowPrepared
        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='silver';")
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';")
    End Sub

    Protected Sub grid_ProcessColumnAutoFilter(sender As Object, e As ASPxGridViewAutoFilterEventArgs) Handles grid.ProcessColumnAutoFilter
        If Not String.IsNullOrEmpty(e.Value) Then
            Call bindGrid()
        End If
    End Sub

    Protected Sub grid_PageIndexChanged(sender As Object, e As EventArgs) Handles grid.PageIndexChanged
        Call bindGrid()
    End Sub

End Class