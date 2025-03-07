Imports DevExpress.Web

Public Class customerDetails
    Inherits System.Web.UI.Page
    Private Sub customerDetails_Init(sender As Object, e As EventArgs) Handles Me.Init
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
        Try
            If Not Page.IsPostBack Then
                ASPxPageControl1.ActiveTabIndex = 0
                Dim codicecliente = CType(Session("cart"), cartManager).Header.CODICECLIENTE
                If codicecliente <> "" Or Session("infoCustomerNo") <> "" Then
                    If Session("infoCustomerNo") <> "" Then
                        Call loadCustomerData(Session("infoCustomerNo"))
                    ElseIf codicecliente <> "" Then
                        Call loadCustomerData(codicecliente)
                    End If
                End If
                gridCart.DataBind()
                If Request.QueryString("tab") = "q" Then
                    ASPxPageControl1.ActiveTabIndex = 3
                End If
                If CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                    btn_NewCart.Enabled = False
                    gridCart.Selection.UnselectAll()
                    gridCart.Enabled = False
                End If

                If codicecliente = "" And Session("infoCustomerNo") = "" Then
                    btn_NewCart.Enabled = False
                    gridCart.Enabled = False
                    btnDestinazione.Enabled = False
                    grigliaPagamenti.Enabled = False
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub loadCustomerData(ByVal CustomerNo As String)
        Try
            Dim dm As New DataManager
            Dim dt As DataTable = dm.GetCustomers(CustomerNo).Tables(0)
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                'FATTURAZIONE
                tb_no.Text = dr.Item("No_")
                tb_ctno.Text = dr.Item("Primary Contact No_")
                tb_piva.Text = dr.Item("VAT Registration No_")
                tb_codfis.Text = dr.Item("Fiscal Code")
                tb_nome.Text = dr.Item("Name") & " " & dr.Item("Name 2")
                tb_indirizzo.Text = dr.Item("Address") & " " & dr.Item("Address 2")
                tb_cap.Text = dr.Item("Post Code")
                tb_provincia.Text = dr.Item("County")
                tb_citta.Text = dr.Item("City")
                tb_Regione.Text = dr.Item("Country_Region Code")
                tb_telefono.Text = dr.Item("Phone No_")
                tb_email.Text = dr.Item("E-Mail")
                tb_fax.Text = dr.Item("Fax No_")
                tb_cellulare.Text = dr.Item("Mobile No_")

                If dr.Item("Blocked") = 1 Then 'blocco per spedizione
                    cb_bloccato.Checked = True
                ElseIf dr.Item("Blocked") = 2 Then 'blocco per fatturazione
                    cb_bloccato.Checked = True
                ElseIf dr.Item("Blocked") = 3 Then 'blocco per tutto
                    cb_bloccato.Checked = True
                Else
                    cb_bloccato.Checked = False
                End If

                If dr.Item("Blocked") > 0 Then
                    btn_NewCart.Enabled = False
                    gridCart.Enabled = False
                    grigliaPagamenti.Enabled = False
                End If

                'DESTINAZIONI
                If CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                    Dim codicedestinazione As String = IIf(CType(Session("cart"), cartManager).Header.ordineHeader.ShipAddressCode <> "", CType(Session("cart"), cartManager).Header.ordineHeader.ShipAddressCode, "FATTURAZIONE")
                    Call loadDestinatario(CType(Session("cart"), cartManager).Header.CODICECLIENTE, codicedestinazione)
                Else
                    tb_desCode.Text = ""
                    tb_desNome.Text = ""
                    tb_desIndirizzo.Text = ""
                    tb_desCap.Text = ""
                    tb_desProvincia.Text = ""
                    tb_desCitta.Text = ""
                    tb_desRegione.Text = ""
                    tb_desTelefono.Text = ""
                    tb_desEmail.Text = ""
                    tb_desFax.Text = ""
                End If

                Dim corrieri As Hashtable = dm.GetShippingAgentForCustomer(CustomerNo)
                If corrieri.Count > 0 Then
                    For Each item As DictionaryEntry In corrieri
                        comboCorrieri.Items.Add(item.Value, item.Key)
                    Next
                    comboCorrieri.SelectedIndex = 0
                End If

                'PAGAMENTO
                grigliaPagamenti.DataSource = dm.GetPagamentiWO(CustomerNo, dr.Item("Payment Method Code"), dr.Item("Payment Terms Code"))
                grigliaPagamenti.DataBind()

            End If
            dm = Nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub loadDestinatario(ByVal customerNo As String, ByVal shipToCode As String)
        Try
            'SPEDIZIONE / DESTINATARI
            Dim dm As New datamanager
            Dim destPref As New DataTable
            'comboCodeDestinatario.SelectedItem = comboCodeDestinatario.Items.FindByValue(shipToCode)
            If shipToCode = "FATTURAZIONE" Then 'destinazionatario indirizzo di fatturazione
                destPref = dm.GetCustomers(customerNo).Tables(0)
            Else
                destPref = dm.GetShipToAddress(customerNo, shipToCode).Tables(0)
            End If
            If destPref.Rows.Count > 0 Then
                tb_desCode.Text = shipToCode
                tb_desNome.Text = destPref.Rows(0).Item("Name") & " " & destPref.Rows(0).Item("Name 2")
                tb_desIndirizzo.Text = destPref.Rows(0).Item("Address") & " " & destPref.Rows(0).Item("Address 2")
                tb_desCap.Text = destPref.Rows(0).Item("Post Code")
                tb_desProvincia.Text = destPref.Rows(0).Item("County")
                tb_desCitta.Text = destPref.Rows(0).Item("City")
                tb_desRegione.Text = destPref.Rows(0).Item("Country_Region Code")
                tb_desTelefono.Text = destPref.Rows(0).Item("Phone No_")
                tb_desEmail.Text = destPref.Rows(0).Item("E-Mail")
                tb_desFax.Text = destPref.Rows(0).Item("Fax No_")
            Else
                tb_desCode.Text = ""
                tb_desNome.Text = ""
                tb_desIndirizzo.Text = ""
                tb_desCap.Text = ""
                tb_desProvincia.Text = ""
                tb_desCitta.Text = ""
                tb_desRegione.Text = ""
                tb_desTelefono.Text = ""
                tb_desEmail.Text = ""
                tb_desFax.Text = ""
            End If

            dm = Nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub CallbackPanel_Dest_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles CallbackPanel_Dest.Callback
        If Not e.Parameter Is Nothing Then
            Call loadDestinatario(tb_no.Text.Trim, e.Parameter)
        End If
    End Sub

    Protected Sub gridDestinazioni_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gridDestinazioni.CustomCallback
        If tb_no.Text <> "" Then
            Call loadDestinatari(tb_no.Text)
        End If
    End Sub

    Private Sub loadDestinatari(CustomerNo As String)
        Dim dm As New DataManager
        Dim destinazioni As DataTable = dm.GetShipToAddressByCustomerCode(CustomerNo)
        gridDestinazioni.DataSource = destinazioni
        gridDestinazioni.DataBind()
        dm = Nothing
    End Sub

    Protected Sub btn_NewCart_Click(sender As Object, e As EventArgs) Handles btn_NewCart.Click
        If CType(Session("cart"), cartManager).Header.IDCART = 0 Then
            Call loadNewCarrelloRedirect()
        Else
            'messaggio controllo carrello in lavorazione
            popupCambioCarrello.ShowOnPageLoad = True
            popupCambioCarrello.Text = "Si sta lavorando sul carrello: " & CType(Session("cart"), cartManager).Header.DESCRIPTION & " Abbandonare il carrello corrente?"
        End If
    End Sub

    Private Sub loadNewCarrelloRedirect()
        CType(Session("cart"), cartManager).clearCart()
        CType(Session("cart"), cartManager).Header.CODICECLIENTE = tb_no.Text.Trim
        CType(Session("cart"), cartManager).Header.CODICECONTATTO = tb_ctno.Text.Trim
        Response.Redirect("~/orderNew.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub grigliaPagamenti_CommandButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCommandButtonEventArgs) Handles grigliaPagamenti.CommandButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        If (e.ButtonType = ColumnCommandButtonType.SelectCheckbox) Then
            If grid.GetRowValues(e.VisibleIndex, "Metodo") = CType(Session("cart"), cartManager).Header.ordineHeader.PaymentMethodCode And grid.GetRowValues(e.VisibleIndex, "Termine") = CType(Session("cart"), cartManager).Header.ordineHeader.PaymentTermsCode Then
                grid.Selection.SelectRow(e.VisibleIndex)
            End If
            If grid.GetRowValues(e.VisibleIndex, "Predefinito") = "1" Then
                grid.Selection.SelectRow(e.VisibleIndex)
            End If
        End If
    End Sub

    Protected Sub btnYES_Click(sender As Object, e As EventArgs)
        popupCambioCarrello.ShowOnPageLoad = False
        Call loadNewCarrelloRedirect()
    End Sub



#Region "Carrelli"

    Protected Sub gridCart_DataBinding(sender As Object, e As EventArgs) Handles gridCart.DataBinding
        If tb_no.Text <> "" Then
            Dim dm As New DataManager
            gridCart.DataSource = dm.getCarrelliHeaderByClienteNo(tb_no.Text)
            dm = Nothing
        End If
    End Sub

    Protected Sub gridCart_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gridCart.CustomCallback
        If Not e.Parameters Is Nothing Then
            Dim idcarrello As Integer = e.Parameters
            Dim CODICECLIENTE As String = CType(sender, ASPxGridView).GetRowValuesByKeyValue(idcarrello, "CODICECLIENTE")
            Dim dm As New DataManager
            Dim carrello As New cartManager
            carrello = carrello.loadFromDBbyIdCarrello(idcarrello)
            carrello.refreshDispo()
            Session("cart") = carrello
            dm = Nothing
            DevExpress.Web.ASPxWebControl.RedirectOnCallback("orderDetails.aspx")
        End If
    End Sub

    Protected Sub gridCart_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles gridCart.CustomColumnDisplayText
        If e.Column.FieldName = "UTENTE_CREAZIONE" Then
            Dim dm As New DataManager
            e.DisplayText = dm.getUserNameSurname(e.Value)
            dm = Nothing
        End If
        If e.Column.FieldName = "UTENTE_ULTIMA_MODIFICA" Then
            Dim dm As New DataManager
            e.DisplayText = dm.getUserNameSurname(e.Value)
            dm = Nothing
        End If
        If e.Column.FieldName = "Status" Then
            If e.Value = 6 Then
                e.DisplayText = orderstatus.getDescription(e.Value, 0)
            End If
        End If
    End Sub

#End Region







End Class