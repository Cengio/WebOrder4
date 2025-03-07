Imports System.Net.Mail
Imports DevExpress.Web
Imports servisWO.datamanager
Imports log4net
Imports System.Reflection
Imports System.IO

Public Class orderList
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Call loadOrders()
        End If
    End Sub

    Private Sub loadOrders()
        grid.Columns.Item("pulsanteEmail").Visible = False
        Dim orderStatus As String = "0"
        Dim completedImported As String = "0"
        Dim notCompletedImported As String = "99"
        Dim notOrderStatus As String = "99"
        Dim Prenotazione As String = "0"
        If Not Request.QueryString("s") Is Nothing Then
            orderStatus = Request.QueryString("s")
        End If
        If Not Request.QueryString("ci") Is Nothing Then
            completedImported = Request.QueryString("ci")
        End If
        If Not Request.QueryString("nci") Is Nothing Then
            notCompletedImported = Request.QueryString("nci")
        End If
        If Not Request.QueryString("ns") Is Nothing Then
            notOrderStatus = Request.QueryString("ns")
        End If
        If Not Request.QueryString("p") Is Nothing Then
            Prenotazione = Request.QueryString("p")
        End If
        If orderStatus = "0,1,2,3,4,5,6,7" Then
            If Prenotazione = "1,2" Then
                lb_cTitolo.Text = "lista ordini prenotati"
            Else
                lb_cTitolo.Text = "lista ordini (tutti)"
            End If
        Else
            Dim osInt As Integer
            If Integer.TryParse(orderStatus, osInt) Then
                Select Case osInt
                    Case 0
                        lb_cTitolo.Text = "lista ordini salvati"
                    Case 1
                        lb_cTitolo.Text = "lista ordini in attesa di produzione"
                       ' grid.Columns.Item("pulsanteEmail").Visible = True
                    Case 2
                        lb_cTitolo.Text = "lista ordini inviati a magazzino"
                    Case 3
                        lb_cTitolo.Text = "lista ordini in lavorazione"
                    Case 4
                        lb_cTitolo.Text = "lista ordini spediti"
                    Case 5
                        lb_cTitolo.Text = "lista ordini in fatturazione"
                    Case 6
                        lb_cTitolo.Text = "lista ordini annullati"
                    Case 7
                        lb_cTitolo.Text = "lista ordini da approvare"
                    Case Else
                        lb_cTitolo.Text = "lista ordini (?)"
                End Select

            Else
                lb_cTitolo.Text = "lista ordini (tutti)"
            End If
        End If
        If Prenotazione = "1,2" Then
            grid.Columns.Item("dataevasione").Visible = True
        Else
            grid.Columns.Item("dataevasione").Visible = False
        End If
        Call bindGrid()

    End Sub

    Private Sub bindGrid()
        Dim dm As New datamanager
        SqlDataSourceOrdini.ConnectionString = dm.GetWORconnectionString()
        SqlDataSourceOrdini.SelectCommand = dm.getOrderListSelectCommand()
        SqlDataSourceOrdini.Select(DataSourceSelectArguments.Empty)
        SqlDataSourceOrdini.DataBind()
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
                Dim dm As New datamanager
                If e.ButtonID = "btnUpdate" Then
                    Dim Status As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Status")
                    Dim CompletedImported As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CompletedImported")
                    If CompletedImported = 1 Or Status = 3 Or Status = 4 Or Status = 5 Or Status = 6 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine non modificabile [" & orderstatus.getDescription(Status, CompletedImported) & "]"
                        e.Image.Url = "~/images/orderMOD_off.png"
                    ElseIf CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine in corso. Chiudere l'attività prima di poter selezionare un altro cliente."
                        e.Image.Url = "~/images/orderMOD_off.png"
                    Else
                        e.Image.ToolTip = "Vai all'ordine"
                        e.Image.Url = "~/images/orderMOD.png"
                    End If
                ElseIf e.ButtonID = "btnEmail" Then
                ElseIf e.ButtonID = "btnReport" Then
                    Dim Status As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Status")
                    Dim CompletedImported As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CompletedImported")
                    If Status = 6 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine non modificabile [" & orderstatus.getDescription(Status, CompletedImported) & "]"
                        e.Image.Url = "~/images/pdfdocumentOFF.png"
                    End If
                End If
                dm = Nothing
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
            Dim CompletedImported As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CompletedImported")
            Dim CustomerNo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CustomerNo").ToString
            Dim Prenotazione As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "PRENOTAZIONE")
            If Code <> "" Then
                Dim dm As New datamanager
                If e.ButtonID = "btnUpdate" Then
                    If Prenotazione = 1 Or Prenotazione = 2 Then
                        Session("cart") = CType(Session("cart"), cartManager).loadFromDBbyOrderCode(Code, False, False)
                    Else
                        Session("cart") = CType(Session("cart"), cartManager).loadFromDBbyOrderCode(Code, False, True)
                    End If
                    Log.Info(CType(Session("user"), user).nomeCompleto & " loaded the order " & Code)
                    If CType(Session("cart"), cartManager).isOrdineChiuso Then
                    Else
                        If CType(Session("cart"), cartManager).Header.ordineHeader.Status = 2 Then
                            CType(Session("cart"), cartManager).Header.ordineHeader.Status = 0
                            dm.riprendiOrdine(Code, CType(Session("user"), user).userCode)
                        End If
                    End If
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("orderDetails.aspx")
                ElseIf e.ButtonID = "btnReport" Then
                    If (CompletedImported Or Status = 3 Or Status = 4 Or Status = 5 Or Status = 6) Then
                        grid.JSProperties("cpShowReport") = "2"
                        grid.JSProperties("cpShowReportDownload") = Code
                    Else
                        Session("orderCodeReport") = Code
                        grid.JSProperties("cpShowReport") = "1"
                    End If
                End If
                dm = Nothing
            End If

        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub grid_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles grid.CustomColumnDisplayText
        Dim dm As New datamanager
        If e.Column.FieldName = "Status" Then
            e.DisplayText = orderstatus.getDescription(e.Value, e.GetFieldValue("CompletedImported"))
            If e.Value = 2 And e.GetFieldValue("PRENOTAZIONE") = 1 Then
                e.DisplayText = "INVIATO A PRODUZIONE"
            End If
            If e.Value = 0 And e.GetFieldValue("PRENOTAZIONE") = 2 Then
                e.DisplayText = "EVADIBILE"
            End If
        End If
        If e.Column.FieldName = "CustomerDescription" Then
            e.DisplayText = dm.GetCustomerName(e.GetFieldValue("CustomerNo"))
        End If
        dm = Nothing
    End Sub

    Private Sub grid_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles grid.HtmlRowPrepared
        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='silver';")
        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white';")
    End Sub

    Protected Sub popup_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles popup.WindowCallback
        If Not e.Parameter Is Nothing Then
            Dim dm As New datamanager
            Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
            Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
            Dim emailRichiestaProduzione As String = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione)
            Dim emailRichiestaProduzione2 As String = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione2)
            Dim emailRichiestaProduzione3 As String = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione3)

            If attivaNotificheEmail = "1" AndAlso emailSistema <> "" AndAlso emailRichiestaProduzione <> "" Then
                hiddenOrderCode.Clear()
                hiddenOrderCode.Add("orderNo", e.Parameter)
                tb_A.Text = emailRichiestaProduzione
                If emailRichiestaProduzione2 <> "" Then
                    tb_Cc.Text = emailRichiestaProduzione2
                End If
                If emailRichiestaProduzione3 <> "" Then
                    tb_Cc2.Text = emailRichiestaProduzione3
                End If

                Dim orderHeader As DataRow = dm.getOrderHeaderWO(e.Parameter).Tables(0).Rows(0)
                Dim customerName As String
                If orderHeader("CustomerNo").ToString.StartsWith("N") Then
                    customerName = dm.GetNewCustomerName(orderHeader("CustomerNo"))
                Else
                    customerName = dm.GetCustomerName(orderHeader("CustomerNo"))
                End If

                tb_Oggetto.Text = "Richiesta produzione articoli ordine nr. " & e.Parameter & " del " & orderHeader("OrderDate") & " per " & customerName

                Dim dt As DataTable = dm.getOrderLinesWO(e.Parameter).Tables(0)
                Dim corpo As String = ""
                corpo &= "Richiesta di produzione dei seguenti articoli e quantità:" & vbCrLf & vbCrLf
                For Each row As DataRow In dt.Rows
                    If row("LotNo") = "" Then
                        corpo &= row("ItemCode") & " " & dm.GetItemDescription(row("ItemCode")) & ":  " & CInt(row("OriginalQty")) & vbCrLf
                    End If
                Next
                corpo &= vbCrLf & vbCrLf
                corpo &= "Cordiali Saluti" & vbCrLf
                corpo &= dm.getUserNameSurname(CType(Session("user"), user).userCode) & vbCrLf
                tb_Corpo.Text = corpo
            Else
                tb_A.Enabled = False
                tb_Cc.Enabled = False
                tb_Cc2.Enabled = False
                tb_Corpo.Enabled = False
                tb_Oggetto.Enabled = False
                btnInvioRichiesta.Enabled = False
            End If

            dm = Nothing

        End If
    End Sub

    Protected Sub btnInvioRichiesta_Click(sender As Object, e As EventArgs) Handles btnInvioRichiesta.Click
        Dim dm As New datamanager
        Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
        Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
        Dim emailSistemaDesc As String = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)
        If emailSistemaDesc = "" Then emailSistemaDesc = emailSistema
        If attivaNotificheEmail = "1" AndAlso emailSistema <> "" Then
            Dim orderNo As String = hiddenOrderCode.Get("orderNo")
            Dim msg As MailMessage = New MailMessage()
            msg.IsBodyHtml = False
            msg.From = New MailAddress(emailSistema, emailSistemaDesc)
            msg.To.Add(tb_A.Text)
            If tb_Cc.Text <> "" Then
                msg.CC.Add(tb_Cc.Text)
            End If
            If tb_Cc2.Text <> "" Then
                msg.CC.Add(tb_Cc2.Text)
            End If
            If CType(Session("user"), user).email <> "" Then
                msg.CC.Add(CType(Session("user"), user).email)
            End If
            msg.Subject = tb_Oggetto.Text
            msg.Body = tb_Corpo.Text
            Dim client As SmtpClient = New SmtpClient
            Try
                client.Send(msg)
                popup.ShowOnPageLoad = False
                popupInvioOK.ShowOnPageLoad = True
                Log.Info("Email " & msg.Subject & " sent to " & msg.To.ToString & " " & msg.CC.ToString)
            Catch ex As Exception
                popup.ShowOnPageLoad = False
                popupInvioKO.ShowOnPageLoad = True
                Log.Error(ex.ToString)
                Exit Sub
            End Try
        End If

    End Sub

    Protected Sub grid_ProcessColumnAutoFilter(sender As Object, e As ASPxGridViewAutoFilterEventArgs) Handles grid.ProcessColumnAutoFilter
        If Not String.IsNullOrEmpty(e.Value) Then
            Call bindGrid()
        End If
    End Sub

    Protected Sub grid_PageIndexChanged(sender As Object, e As EventArgs) Handles grid.PageIndexChanged
        Call bindGrid()
    End Sub

    Protected Sub ASPxCallback_downloadPDF_Callback(source As Object, e As CallbackEventArgs) Handles ASPxCallback_downloadPDF.Callback
        If e.Parameter <> "" Then
            Dim orderNo As String = e.Parameter
            Dim dm As New datamanager
            Dim percorsoPDF As String = dm.GetParametroSito(parametriSitoValue.percorsoPDF)
            Dim ReportName As String = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).prefixOrderFileName & "ORD" & orderNo & ".pdf"
            If percorsoPDF <> "" AndAlso Directory.Exists(Server.MapPath("~/" & percorsoPDF)) Then
                Dim fullpath As String = Server.MapPath("~/" & percorsoPDF & "/" & ReportName)
                If File.Exists(fullpath) Then
                    e.Result = String.Format("orderReportDownload.aspx?id={0}", orderNo)
                Else
                    e.Result = "NOT_ALLOWED"
                End If
            Else
                e.Result = "NOT_ALLOWED"
            End If
        End If
    End Sub

End Class