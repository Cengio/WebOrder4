Imports System.Reflection
Imports DevExpress.Web
Imports log4net

Public Class orderNew
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub orderNew_Init(sender As Object, e As EventArgs) Handles Me.Init
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
            Dim codicecliente = CType(Session("cart"), cartManager).Header.CODICECLIENTE
            If codicecliente <> "" Then
                Call loadPreOrderHeader(codicecliente)
                If codicecliente.ToString.StartsWith("N") Then
                    Call loadNewCustomerData(codicecliente)
                Else
                    Call loadCustomerData(codicecliente)
                End If
                Call loadPromoHeader()
                Dim dm As New datamanager
                dm.synchOrderInvoicedFromNAV()
                dm = Nothing
                Dim ordinePrenotazione = Request.QueryString("p")
                If ordinePrenotazione = "1" Then
                    btn_NewOrderPrenotazione.Visible = True
                    comboOrderEvasione.Visible = True
                    lb_DataEvasione.Visible = True
                    btn_NewOrder.Visible = False
                    btn_NewCart.Visible = False
                Else
                    btn_NewOrderPrenotazione.Visible = False
                    comboOrderEvasione.Visible = False
                    lb_DataEvasione.Visible = False
                    btn_NewOrder.Visible = True
                    btn_NewCart.Visible = True
                End If
            Else
                Try
                    Response.Redirect("~/customerList.aspx", False)
                    Context.ApplicationInstance.CompleteRequest()
                Catch ex As Exception
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/customerList.aspx")
                End Try
            End If
        End If
    End Sub

    Private Sub loadPreOrderHeader(ByVal customerNo As String)
        Dim dm As New datamanager
        comboOrderDate.Date = Date.Parse(Date.Now.ToShortDateString)
        comboOrderType.SelectedIndex = -1
        CType(Session("cart"), cartManager).Header.ordineHeader.Type = ""
        comboOrderEvasione.Date = Date.Parse(Date.Now.ToShortDateString)
        dm = Nothing
    End Sub

    Private Sub loadCustomerData(ByVal customerNo As String)
        Dim dm As New datamanager
        Dim dt As DataTable = dm.GetCustomers(customerNo).Tables(0)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            'FATTURAZIONE
            tb_no.Text = dr.Item("No_")
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
                Dim dtBR As DataTable = dm.GetCustomerBlockedReason(dr.Item("Blocked Reason"), dr.Item("Blocked")).Tables(0)
                cb_bloccato.Text = "Spedizione bloccata "
                If dtBR.Rows.Count > 0 Then
                    cb_bloccato.Text &= dtBR.Rows(0).Item("Description")
                End If
            ElseIf dr.Item("Blocked") = 2 Then 'blocco per fatturazione
                cb_bloccato.Checked = True
                Dim dtBR As DataTable = dm.GetCustomerBlockedReason(dr.Item("Blocked Reason"), dr.Item("Blocked")).Tables(0)
                cb_bloccato.Text = "Fatturazione bloccata "
                If dtBR.Rows.Count > 0 Then
                    cb_bloccato.Text &= dtBR.Rows(0).Item("Description")
                End If
            ElseIf dr.Item("Blocked") = 3 Then 'blocco per tutto
                cb_bloccato.Checked = True
                btn_NewOrder.Enabled = False
                cb_bloccato.Text = "Ordini bloccati "
                Dim dtBR As DataTable = dm.GetCustomerBlockedReason(dr.Item("Blocked Reason"), dr.Item("Blocked")).Tables(0)
                If dtBR.Rows.Count > 0 Then
                    cb_bloccato.Text &= dtBR.Rows(0).Item("Description")
                End If
            Else
                cb_bloccato.Checked = False
            End If

            'DESTINAZIONI
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

            'non ci sono destinazioni diverse carico la default che è quella di FATTURAZIONE 
            Dim destinazioni As DataTable = dm.GetShipToAddress(dr.Item("No_")).Tables(0)
            If destinazioni.Rows.Count = 0 Then
                Call loadDestinatario(dr.Item("No_"), "FATTURAZIONE")
            End If

            Dim corrieriList As Hashtable = dm.GetShippingAgent
            For Each item As DictionaryEntry In corrieriList
                comboCorrieri.Items.Add(item.Value, item.Key)
                comboCorrieri.SelectedIndex = -1
            Next
            Dim corrieri As Hashtable = dm.GetShippingAgentForCustomer(customerNo)
            If corrieri.Count > 0 Then
                For Each item As DictionaryEntry In corrieri
                    comboCorrieri.SelectedItem = comboCorrieri.Items.FindByValue(item.Key)
                    Exit For
                Next
            End If

            'PAGAMENTO
            grigliaPagamenti.DataSource = dm.GetPagamentiWO(dr.Item("No_"), dr.Item("Payment Method Code"), dr.Item("Payment Terms Code"))
            grigliaPagamenti.DataBind()

        End If
        dm = Nothing
    End Sub

    Private Sub loadNewCustomerData(ByVal customerNo As String)
        Dim dm As New datamanager
        Dim dt As DataTable = dm.GetNewCustomers(customerNo)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            'FATTURAZIONE
            tb_no.Text = dr.Item("customerNo")
            tb_piva.Text = dr.Item("VATNumber")
            tb_codfis.Text = dr.Item("FiscalCode")
            tb_nome.Text = dr.Item("Name") & " " & dr.Item("Name2")
            tb_indirizzo.Text = dr.Item("Address") & " " & dr.Item("Address2") & " " & dr.Item("AddressNo")
            tb_cap.Text = dr.Item("PostCode")
            tb_provincia.Text = dr.Item("County")
            tb_citta.Text = dr.Item("City")
            tb_Regione.Text = ""
            tb_telefono.Text = dr.Item("Phone1")
            tb_email.Text = dr.Item("EMail")
            tb_fax.Text = dr.Item("FaxNo")
            tb_cellulare.Text = dr.Item("Phone2")
            btnDestinazione.Enabled = False
            tb_desNome.Text = dr.Item("ragsocsped1") & " " & dr.Item("ragsocsped2")
            tb_desIndirizzo.Text = dr.Item("indirizzosped1") & " " & dr.Item("indirizzosped2")
            tb_desCap.Text = dr.Item("capsped")
            tb_desProvincia.Text = dr.Item("provinciasped")
            tb_desCitta.Text = dr.Item("localitasped")
            tb_desRegione.Text = ""
            tb_desTelefono.Text = dr.Item("telefono1sped")
            tb_desEmail.Text = dr.Item("emailsped")
            tb_desFax.Text = dr.Item("faxsped")

            Dim corrieriList As Hashtable = dm.GetShippingAgent
            For Each item As DictionaryEntry In corrieriList
                comboCorrieri.Items.Add(item.Value, item.Key)
                comboCorrieri.SelectedIndex = -1
            Next

            'PAGAMENTO
            grigliaPagamenti.DataSource = dm.getNewCustomerPagamenti(dr.Item("Category"))
            grigliaPagamenti.DataBind()
        End If
        dm = Nothing
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
                Session("DesCode") = tb_desCode.Text 'domenico: salva il valore nella sessione, altrimenti non risulta quando savenewcarrello lo legge
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

    Protected Sub btn_NewCart_Click(sender As Object, e As EventArgs) Handles btn_NewCart.Click
        If CType(Session("cart"), cartManager).Header.IDCART = 0 Then
            Call saveNewCarrello(False)
        Else
            Call checkLavorazione()
        End If
    End Sub

    Protected Sub btn_NewOrder_Click(sender As Object, e As EventArgs) Handles btn_NewOrder.Click
        If CType(Session("cart"), cartManager).Header.IDCART = 0 Then
            Call saveNewCarrello(True)
        Else
            Call checkLavorazione()
        End If
    End Sub

    Protected Sub btn_NewOrderPrenotazione_Click(sender As Object, e As EventArgs) Handles btn_NewOrderPrenotazione.Click
        If CType(Session("cart"), cartManager).Header.IDCART = 0 Then
            Call saveNewCarrello(True, True)
        Else
            Call checkLavorazione()
        End If
    End Sub

    Private Sub checkLavorazione()
        'messaggio controllo carrello in lavorazione
        popupCambioCarrello.ShowOnPageLoad = True
        If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
            popupCambioCarrello.Text = "Si sta lavorando sul carrello: " & CType(Session("cart"), cartManager).Header.IDCART.ToString & " Abbandonare il carrello corrente e andare alla lista clienti oppure andare al carrello."
        Else
            popupCambioCarrello.Text = "Si sta lavorando sull'ordine : " & CType(Session("cart"), cartManager).Header.ordineHeader.Code & " Abbandonare l'ordine correntee e andare alla lista clienti oppure andare all'ordine."
        End If
    End Sub

    Private Sub saveNewCarrello(ByVal createOrder As Boolean, Optional ByVal prenotazione As Boolean = False)
        If Session("DesCode") IsNot Nothing Then
            tb_desCode.Text = Session("DesCode").ToString() 'domenico: ricarica il valore di tb_desCode dalla sessione, altrimenti risulterebbe vuoto
        End If
        If Not comboOrderType.SelectedItem Is Nothing Then
            If Not (comboOrderType.SelectedItem.Value = "Visita Agente" AndAlso comboAgente.SelectedItem Is Nothing) Then
                If tb_desCode.Text.Trim <> "" Or CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N") Then
                    If grigliaPagamenti.Selection.Count > 0 Then  'controlli su scelta pagamento
                        Dim ctrlNuovoCliente_Riba As Boolean = CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N") AndAlso grigliaPagamenti.GetSelectedFieldValues(New String() {"Metodo", "Termine", "Sconto"})(0)(0).ToString() = "RB"
                        Dim ibanOK As Boolean = True
                        If ctrlNuovoCliente_Riba Then
                            Dim dmc As New datamanager
                            ibanOK = (dmc.GetNewCustomerIBAN(Session("selectedNewCustomer")) <> "")
                            dmc = Nothing
                        End If
                        If ibanOK Then
                            Dim comboProfili As ASPxComboBox = CType(gridPromoHeader.FindTitleTemplateControl("comboProfiliSconto"), ASPxComboBox)
                            If comboProfili.Items.Count = 0 Or comboProfili.SelectedItem IsNot Nothing Then
                                Session("infoCustomerNo") = ""
                                Dim dm As New datamanager
                                Dim carrello As cartManager = CType(Session("cart"), cartManager)
                                Dim codicecliente As String = ""
                                Dim codicecontatto As String = ""
                                codicecliente = CType(Session("cart"), cartManager).Header.CODICECLIENTE
                                codicecontatto = CType(Session("cart"), cartManager).Header.CODICECONTATTO
                                carrello.Header.DESCRIPTION = codicecliente & "-" & Now.ToString
                                carrello.Header.UTENTE_CREAZIONE = CType(Session("user"), user).userCode
                                carrello.Header.UTENTE_ULTIMA_MODIFICA = CType(Session("user"), user).userCode
                                carrello.Header.CODICECLIENTE = codicecliente
                                If tb_desCode.Text.Trim <> "FATTURAZIONE" Then
                                    carrello.Header.ordineHeader.ShipAddressCode = tb_desCode.Text.Trim
                                End If
                                Dim pagamentoSelezionatoList As List(Of Object) = grigliaPagamenti.GetSelectedFieldValues(New String() {"Metodo", "Termine", "Sconto"})
                                If pagamentoSelezionatoList.Count > 0 Then
                                    carrello.Header.ordineHeader.PaymentMethodCode = pagamentoSelezionatoList(0)(0)
                                    carrello.Header.ordineHeader.PaymentTermsCode = pagamentoSelezionatoList(0)(1)
                                    carrello.Header.SCONTOPAGAMENTO = pagamentoSelezionatoList(0)(2)
                                End If

                                Dim strday As String = IIf(comboOrderDate.Date.Day.ToString.Length = 2, comboOrderDate.Date.Day.ToString, "0" & comboOrderDate.Date.Day.ToString)
                                Dim strmonth As String = IIf(comboOrderDate.Date.Month.ToString.Length = 2, comboOrderDate.Date.Month.ToString, "0" & comboOrderDate.Date.Month.ToString)
                                Dim stryear As String = comboOrderDate.Date.Year.ToString
                                carrello.Header.ordineHeader.OrderDate = String.Format("{0}/{1}/{2}", strday, strmonth, stryear)
                                carrello.Header.ordineHeader.Type = comboOrderType.SelectedItem.Value
                                carrello.Header.ordineHeader.Notes = tb_note.Text.Trim

                                If Not comboCorrieri.SelectedItem Is Nothing Then
                                    carrello.Header.ordineHeader.ShippingAgentCode = comboCorrieri.SelectedItem.Value
                                End If

                                carrello.Header.IMPORTOSPESESPEDIZIONE = dm.GetSpeseTrasporto

                                If CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Then
                                    carrello.Header.IMPORTOEVADIBILITAORDINE = dm.getparametriEvadibilitaOrdineByContactCode(CType(Session("cart"), cartManager).Header.CODICECONTATTO, datamanager.parametriSitoValue.importoEvadibilitaOrdine)
                                    carrello.Header.IMPORTOPORTOFRANCO = dm.getparametriEvadibilitaOrdineByContactCode(CType(Session("cart"), cartManager).Header.CODICECONTATTO, datamanager.parametriSitoValue.importoPortoFranco)
                                Else
                                    carrello.Header.IMPORTOEVADIBILITAORDINE = dm.GetParametroSito(datamanager.parametriSitoValue.importoEvadibilitaOrdine)
                                    carrello.Header.IMPORTOPORTOFRANCO = dm.GetParametroSito(datamanager.parametriSitoValue.importoPortoFranco)
                                End If

                                If Not comboAgente.SelectedItem Is Nothing Then
                                    carrello.Header.AGENTE = comboAgente.SelectedItem.Value
                                End If

                                If prenotazione Then
                                    carrello.Header.PRENOTAZIONE = 1
                                    carrello.Header.DATA_EVASIONE = New Date(comboOrderEvasione.Date.Year, comboOrderEvasione.Date.Month, comboOrderEvasione.Date.Day)
                                Else
                                    carrello.Header.PRENOTAZIONE = 0
                                    carrello.Header.DATA_EVASIONE = New Date(comboOrderDate.Date.Year, comboOrderDate.Date.Month, comboOrderDate.Date.Day)
                                End If

                                '20200424 - Gestione scontoScaglioni 
                                carrello.Header.SCONTOHEADER = dm.GetScontoScaglioneByCodCliente(codicecliente)

                                Dim idCarrello As Int64 = dm.salvaCarrello2(carrello, True, False)
                                Session("cart") = CType(Session("cart"), cartManager).loadFromDBbyIdCarrello(idCarrello, False)
                                Log.Info(CType(Session("user"), user).nomeCompleto & " created cart " & idCarrello.ToString & " for the customer " & CType(Session("cart"), cartManager).Header.CODICECLIENTE)
                                If createOrder Then
                                    Session("cart") = dm.addOrderFromCarrello(CType(Session("cart"), cartManager))
                                    If CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N") Then
                                        dm.addOrderForNewCustomer(CType(Session("cart"), cartManager).Header.ordineHeader.Code, CType(Session("cart"), cartManager).Header.CODICECLIENTE)
                                    End If
                                    Log.Info(CType(Session("user"), user).nomeCompleto & " created order " & CType(Session("cart"), cartManager).Header.ordineHeader.Code & " for the customer " & CType(Session("cart"), cartManager).Header.CODICECLIENTE)
                                End If

                                dm = Nothing
                                Response.Redirect("~/productFinder.aspx?ea=1", False)
                                Context.ApplicationInstance.CompleteRequest()

                            Else
                                'messaggio errore scelta profilo sconti
                                popupControlli.ShowOnPageLoad = True
                                popupControlli.Text = "Selezionare il Profilo di sconto"
                            End If

                        Else
                            'messaggio errore iban - riba per nuovo cliente
                            popupControlli.ShowOnPageLoad = True
                            popupControlli.Text = "Il pagamento tramite riba non è possibile: IBAN mancante."
                        End If
                        'Else

                        'End If
                    Else
                        'messaggio errore scelta pagamento
                        popupControlli.ShowOnPageLoad = True
                        popupControlli.Text = "Selezionare il METODO DI PAGAMENTO prima di proseguire."
                    End If
                Else
                    'messaggio errore scelta detinazione
                    popupControlli.ShowOnPageLoad = True
                    popupControlli.Text = "Selezionare la DESTINAZIONE MERCE prima di proseguire."
                End If
            Else
                'messaggio errore scelta agente
                popupControlli.ShowOnPageLoad = True
                popupControlli.Text = "Selezionare l'AGENTE prima di proseguire."
                comboAgente.Focus()
            End If
        Else
            'messaggio errore scelta tipo interazione
            popupControlli.ShowOnPageLoad = True
            popupControlli.Text = "Selezionare il TIPO INTERAZIONE prima di proseguire."
            comboOrderType.Focus()
        End If

    End Sub

    Private Sub loadSpeseTrasporto(ByVal IncludeShipCost As Integer)
        Dim dm As New datamanager
        If IncludeShipCost = 1 Then
            cb_speseTrasporto.Checked = True
            lb_speseTrasporto.Text = "€. " & dm.GetSpeseTrasporto.ToString
        Else
            cb_speseTrasporto.Checked = False
            lb_speseTrasporto.Text = ""
        End If
        dm = Nothing
    End Sub

    Protected Sub CallbackPanel_speTrasporto_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles CallbackPanel_speTrasporto.Callback
        If cb_speseTrasporto.Checked Then
            Call loadSpeseTrasporto(1)
        Else
            Call loadSpeseTrasporto(0)
        End If
    End Sub

    Protected Sub grigliaPagamenti_CommandButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCommandButtonEventArgs) Handles grigliaPagamenti.CommandButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        'se ho solo un tipo di pagamento pre-seleziono l'unico possibile
        If grid.VisibleRowCount = 1 AndAlso grid.Selection.Count = 0 Then
            grid.Selection.SelectRow(e.VisibleIndex)
        End If
    End Sub

    Protected Sub gridDestinazioni_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gridDestinazioni.CustomCallback
        If tb_no.Text <> "" Then
            Call loadDestinatari(tb_no.Text)
        End If
    End Sub

    Private Sub loadDestinatari(CustomerNo As String)
        Dim dm As New datamanager
        Dim destinazioni As DataTable = dm.GetShipToAddressByCustomerCode(CustomerNo)
        gridDestinazioni.DataSource = destinazioni
        gridDestinazioni.DataBind()
        dm = Nothing
    End Sub

    Protected Sub btnYES_Click(sender As Object, e As EventArgs)
        popupCambioCarrello.ShowOnPageLoad = False
        Session("infoCustomerNo") = ""
        CType(Session("cart"), cartManager).clearCart()
        Response.Redirect("~/customerList.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub btnNo_Click(sender As Object, e As EventArgs)
        popupCambioCarrello.ShowOnPageLoad = False
        Response.Redirect("~/orderDetails.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub comboAgente_Callback(sender As Object, e As CallbackEventArgsBase) Handles comboAgente.Callback
        If e.Parameter = "Visita Agente" Then
            Call loadAgenti()
            comboAgente.SelectedIndex = -1
        Else
            comboAgente.Items.Clear()
            comboAgente.SelectedIndex = -1
        End If
    End Sub

    Private Sub loadAgenti()
        Dim dm As New datamanager
        Dim dtAgenti As DataTable = dm.getAgentiList
        If dtAgenti.Rows.Count > 0 Then
            comboAgente.DataSource = dtAgenti
            comboAgente.TextField = "description"
            comboAgente.ValueField = "code"
            comboAgente.DataBind()
        End If
        dm = Nothing
    End Sub

    Private Sub loadPromoHeader()
        Dim dm As New datamanager
        If dm.GetSASstatus() = 1 Then
            gridPromoHeader.DataBind()
        End If
        dm = Nothing
    End Sub

    Protected Sub gridPromoHeader_DataBinding(sender As Object, e As EventArgs) Handles gridPromoHeader.DataBinding
        If Not CType(Session("cart"), cartManager).Header.ordineHeader.Type Is Nothing AndAlso CType(Session("cart"), cartManager).Header.ordineHeader.Type <> "" Then
            Dim pm As New promoManager
            Dim lph As List(Of promoHeader) = New List(Of promoHeader)
            Dim cd As intCanaleDirezione = pm.getCanaleDirezioneByOrderType(CType(Session("cart"), cartManager).Header.ordineHeader.Type)
            Dim idprofilo As Int64 = CType(Session("cart"), cartManager).Header.IDPROFILOSCONTO
            lph = pm.getPromoHeaderByCtCode(CType(Session("cart"), cartManager).Header.CODICECONTATTO, CType(Session("cart"), cartManager).Header.getOrderDate(), cd, -1, idprofilo)
            gridPromoHeader.DataSource = lph
            pm = Nothing
        End If
    End Sub

    Protected Sub gridPromoHeader_CustomCallback(sender As Object, e As ASPxGridViewCustomCallbackEventArgs) Handles gridPromoHeader.CustomCallback
        If Not e.Parameters Is Nothing Then
            Dim data() As String = e.Parameters.Split("|")
            If data.Length = 2 AndAlso data(0) IsNot Nothing AndAlso data(1) IsNot Nothing AndAlso data(0) <> "" AndAlso IsNumeric(data(1)) Then
                Dim tipointerazione As String = data(0).ToString()
                Dim idprofilo As Int64 = Convert.ToInt64(data(1))
                CType(Session("cart"), cartManager).Header.ordineHeader.Type = tipointerazione
                CType(Session("cart"), cartManager).Header.IDPROFILOSCONTO = idprofilo
                Call loadPromoHeader()
            Else
                CType(Session("cart"), cartManager).Header.ordineHeader.Type = ""
            End If
        Else
            CType(Session("cart"), cartManager).Header.ordineHeader.Type = ""
        End If
    End Sub

    Protected Sub comboProfiliSconto_Init(sender As Object, e As EventArgs)
        Dim dm As New datamanager
        Dim combo As ASPxComboBox = CType(sender, ASPxComboBox)
        combo.DataSource = dm.getProfiliSconti()
        combo.TextField = "profilo"
        combo.ValueField = "id"
        combo.DataBind()
        combo.SelectedItem = combo.Items.FindByValue(CInt(CType(Session("cart"), cartManager).Header.IDPROFILOSCONTO))
        dm = Nothing
    End Sub


End Class