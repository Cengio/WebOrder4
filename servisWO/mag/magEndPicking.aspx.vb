Imports DevExpress.Web
Imports System.Reflection
Imports log4net

Public Class magEndPicking
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub magEndPicking_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not CType(Session("carrelloMagazzino"), cartManager) Is Nothing AndAlso CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code <> "" Then
            Dim dm As New DataManager
            'Dim dtline As DataTable = dm.getWarehouseLines(CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code)
            Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
            ASPxGridView1.DataSource = carrello.carrelloLines
            ASPxGridView1.DataBind()
            If carrello.Header.ordineHeader.Status = 5 Then
                btn_Spedisci.Enabled = True
            Else
                btn_Spedisci.Enabled = False
            End If

            If Request.QueryString("cf") = 1 And CType(Session("user"), user).iSmagazzinoadmin Then 'controllo e fatturazione
                ASPxFormLayout1.FindItemOrGroupByName("dettagliSpedizione").Visible = True
                ASPxFormLayout1.FindItemOrGroupByName("raccogliAltro").Visible = False
                ASPxFormLayout1.FindItemOrGroupByName("inviaCTRL").Visible = False
            Else
                ASPxFormLayout1.FindItemOrGroupByName("dettagliSpedizione").Visible = False
                ASPxFormLayout1.FindItemOrGroupByName("raccogliAltro").Visible = False 'non ha più senso di essere visibile nella logica della slot-machine
                ASPxFormLayout1.FindItemOrGroupByName("inviaCTRL").Visible = True
            End If



            If Not Page.IsPostBack Then
                Dim preSelectedKey As Object = Nothing
                Dim corrieriList As Hashtable = dm.GetShippingAgent
                For Each item As DictionaryEntry In corrieriList
                    comboCorrieri.Items.Add(item.Value, item.Key)
                    If item.Value.ToString.Contains("BART") Then
                        preSelectedKey = item.Key
                    End If
                Next
                If Not preSelectedKey Is Nothing Then
                    comboCorrieri.SelectedItem = comboCorrieri.Items.FindByValue(preSelectedKey)
                End If

                Dim corrieri As Hashtable = dm.GetShippingAgentForCustomer(carrello.Header.ordineHeader.CustomerNo)
                If corrieri.Count > 0 Then
                    For Each item As DictionaryEntry In corrieri
                        If Not comboCorrieri.Items.FindByValue(item.Key) Is Nothing Then
                            comboCorrieri.SelectedItem = comboCorrieri.Items.FindByValue(item.Key)
                            Exit For
                        End If
                    Next
                End If

                If carrello.Header.ordineHeader.ShippingAgentCode <> "" Then
                    Try
                        comboCorrieri.SelectedIndex = comboCorrieri.Items.IndexOfValue(carrello.Header.ordineHeader.ShippingAgentCode)
                    Catch ex As Exception
                    End Try
                End If

            End If

            dm = Nothing
        End If
    End Sub

    Protected Sub ASPxGridView1_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles ASPxGridView1.CustomColumnDisplayText
        Dim dm As New datamanager
        If e.Column.FieldName = "ordineLine.OriginalQty" Then
            If e.GetFieldValue("LINEIDSOURCE") > 0 Then
                e.DisplayText = "Riga " & e.GetFieldValue("LINEIDSOURCE")
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub ASPxGridView1_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles ASPxGridView1.HtmlRowPrepared
        If e.RowType <> GridViewRowType.Data Then
            Return
        End If
        'Dim codArticolo As String = e.GetValue("ItemCode").ToString
        Dim Loaded As Integer = e.GetValue("LOADED")
        Dim QtyToShip As Integer = e.GetValue("ordineLine.QtyToShip")
        Dim OriginalQty As Integer = e.GetValue("ordineLine.OriginalQty")
        If Loaded = 0 Then
            e.Row.BackColor = Drawing.Color.Red
        Else
            e.Row.BackColor = Drawing.Color.LightGreen
        End If
        If QtyToShip > OriginalQty Then
            e.Row.BackColor = Drawing.Color.Red
            Log.Warn("QtyToShip > OriginalQty")
        End If
    End Sub

    Protected Sub ASPxGridView1_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles ASPxGridView1.HtmlDataCellPrepared
        If e.DataColumn.FieldName = "ordineLine.QtyToShip" Then
            If e.CellValue = 0 Then
                e.Cell.BackColor = Drawing.Color.IndianRed
                If e.GetValue("LOADED") = 1 Then
                    e.Cell.ToolTip = "Quantità zero per RETTIFICA"
                Else
                    e.Cell.ToolTip = "Quantità zero per SKIP RIGA"
                End If
            End If
        End If
    End Sub

    Protected Sub ASPxGridView1_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles ASPxGridView1.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        If e.CellType = GridViewTableCommandCellType.Data Then
            If e.ButtonID = "btnGoToLine" Then
                Dim Loaded As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "LOADED")
                If Loaded = 1 Then
                    e.Enabled = True
                    e.Image.Url = "~/images/pickOK.png"
                    e.Image.ToolTip = "Riga OK"
                Else
                    e.Enabled = True
                    e.Image.Url = "~/images/pickKO.png"
                    e.Image.ToolTip = "Vai alla riga ... "
                End If
            End If
        End If
    End Sub

    Protected Sub ASPxGridView1_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles ASPxGridView1.CustomButtonCallback
        Try
            If e.ButtonID = "btnGoToLine" Then
                Dim OrderNo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.OrderCode").ToString
                'Session("selectedmagOrderCode") = OrderNo
                Dim carrelloMagazzino As New cartManager
                Session("carrelloMagazzino") = carrelloMagazzino.loadFromDBbyOrderCode(OrderNo, True)
                Session("currentLineIndex") = e.VisibleIndex
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("magPicking.aspx")
            End If
        Catch ex As Exception
            'sgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btn_Spedisci_Click(sender As Object, e As EventArgs) Handles btn_Spedisci.Click
        If Not CType(Session("carrelloMagazzino"), cartManager) Is Nothing AndAlso CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code <> "" Then
            Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
            Dim dm As New DataManager
            Dim ctrl As Integer = 0
            Dim nrcolli As Integer = CInt(tb_nrcolli.Text.Trim)
            Dim peso As Integer = CInt(tb_peso.Text.Trim)
            Dim ShippingAgentCode As String = ""
            If Not comboCorrieri.SelectedItem Is Nothing Then
                ShippingAgentCode = comboCorrieri.SelectedItem.Value
            End If
            'aggiorno colli, peso, corriere e stato dell'ordine
            ctrl = carrello.shipOrder(carrello.Header.ordineHeader.Code, CType(Session("user"), user).userCode, peso, nrcolli, ShippingAgentCode)
            Log.Info(CType(Session("user"), user).nomeCompleto & " shipped order " & carrello.Header.ordineHeader.Code)
            If ctrl > 0 Then
                'Session("selectedmagOrderCode") = ""
                'Session("selectedmagCodeCustomer") = ""
                Session("carrelloMagazzino") = Nothing
                If Request.QueryString("cf") = 1 Then
                    Response.Redirect("magOrderToPickList.aspx?t=5&cf=1", False)
                    Context.ApplicationInstance.CompleteRequest()
                Else
                    Response.Redirect("magOrderToPickList.aspx?t=2", False)
                    Context.ApplicationInstance.CompleteRequest()
                End If
            End If
            dm = Nothing
        End If
    End Sub

    Protected Sub btn_NewPick_Click(sender As Object, e As EventArgs) Handles btn_NewPick.Click
        Session("carrelloMagazzino") = Nothing
        Response.Redirect("magOrderToPickList.aspx?t=2", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub btn_InviaCTRL_Click(sender As Object, e As EventArgs) Handles btn_InviaCTRL.Click
        Dim dm As New DataManager
        Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
        dm.sendOrderToCtrl(carrello.Header.ordineHeader.Code, CType(Session("user"), user).userCode)
        dm = Nothing
        Log.Info(CType(Session("user"), user).nomeCompleto & " sent to control and invoice order " & carrello.Header.ordineHeader.Code)
        Session("carrelloMagazzino") = Nothing
        Session("prossimoOrdineDaRaccogliere") = ""
        Response.Redirect("magRaccogliOrdine.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub


End Class