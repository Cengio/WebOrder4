Imports DevExpress.Web
Imports DevExpress.Data
Imports System.Net.Mail
Imports servisReports.reportordini
Imports System.IO
Imports servisWO.datamanager
Imports log4net
Imports System.Reflection

Public Class orderDetails
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub orderDetails_Init(sender As Object, e As EventArgs) Handles Me.Init
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
            If CType(Session("cart"), cartManager).Header.IDCART > 0 Or CType(Session("cart"), cartManager).Header.ordineHeader.Code <> "" Then
                If Not Page.IsPostBack Then
                    If CType(Session("user"), user).iSzeroprice And Not CType(Session("cart"), cartManager).isOrdineChiuso Then
                        gridOrderLine.Columns("zerobutton").Visible = True
                    Else
                        gridOrderLine.Columns("zerobutton").Visible = False
                    End If
                    ASPxPageControl1.ActiveTabIndex = 0

                    'ordini di prenotazione dichiarati evadibili da produzione
                    If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 2 Then
                        Call assegnaLottiaOrdinePrenotato()
                    End If

                    Call loadCarrelloHeader()
                    Call loadCarrelloLines()
                    Call loadCarrelloFooter()
                    CType(Session("cart"), cartManager).checkPromoHeader()
                    Call loadPromoHeader()
                    If CType(Session("cart"), cartManager).Header.ordineHeader.Code <> "" Then
                        Call loadArticoliMancanti(CType(Session("cart"), cartManager).Header.ordineHeader.Code)
                    End If
                End If
            Else
                Response.Redirect("orderList.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        Catch ex As Exception
            Session.Abandon()
            Response.Redirect("login.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End Try
    End Sub

    Private Sub setButtonVisibility()
        Dim carrello As cartManager = CType(Session("cart"), cartManager)
        Dim currentUser As user = CType(Session("user"), user)

        btn_SaveOrder.Visible = currentUser.iSordini
        btn_SaveOrderAndNew.Visible = currentUser.iSordini
        btn_SaveAndCtrl.Visible = Not currentUser.iSnorevisioneordine
        btn_SaveAndSend.Visible = (currentUser.iSrevisoreordini Or currentUser.iSnorevisioneordine)
        btn_SaveCarrello.Visible = (carrello.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO)
        btn_Report.Visible = (carrello.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE)
        btn_annullaCarrello.Visible = currentUser.iSordini
        btn_Produci.Visible = (carrello.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE And currentUser.iSnorevisioneordine)

        If carrello.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
            btn_annullaCarrello.Text = "Annulla Carrello"
        ElseIf carrello.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
            btn_annullaCarrello.Text = "Annulla Ordine"
        End If

        If carrello.Header.ordineHeader.Status = 7 Then 'in attesa di approvazione
            btn_SaveOrder.Enabled = False
            btn_SaveOrderAndNew.Enabled = False
            btn_SaveAndSend.Text = "Approva ed Invia a Magazzino"
            btn_Produci.Text = "Approva e Richiedi Produzione Archeo"
        End If

        If Not carrello.controlloGiacenze(False) Then
            btn_Produci.Enabled = True
        Else
            btn_Produci.Enabled = False
        End If

        If carrello.Header.PRENOTAZIONE = 1 Then
            btn_SaveAndSend.Text = "Invia a Produzione"
        ElseIf carrello.Header.PRENOTAZIONE = 2 Then
            btn_SaveAndSend.Text = "Invia a Magazzino"
        End If


        'ulteriore controllo di blocco su ordine chiuso ovvero stati: IN LAVORAZIONE DAL MAGAZZINO / SPEDITO / ANNULATO / COMPLETAMENTE IMPORTATO IN NAV
        If carrello.isOrdineChiuso Then
            btn_SaveOrder.Enabled = False
            btn_SaveOrderAndNew.Enabled = False
            btn_SaveAndSend.Enabled = False
            btn_SaveCarrello.Enabled = False
            btn_annullaCarrello.Enabled = False
            btn_SaveAndCtrl.Enabled = False
            btn_Produci.Enabled = False

            comboCorrieri.Enabled = False
            grigliaPagamenti.Enabled = False
            comboOrderType.Enabled = False
            comboOrderDate.Enabled = False
            memoNote.ReadOnly = True
            gridOrderLine.Columns("updbutton").Visible = False
            gridOrderLine.Columns("delbutton").Visible = False
            gridOrderLine.Columns("Disponibilita").Visible = False
            gridOrderLine.Columns("zerobutton").Visible = False
            cb_speseTrasporto.Enabled = False
            btnDestinazione.Enabled = False
            btnApplicaScontoHeader.ClientEnabled = False
            tbScontoHeader.ClientEnabled = False
            ASPxFormLayout_promoHeader.Enabled = False
            comboAgente.Enabled = False
            popupOrdineBloccato.ShowOnPageLoad = True
        End If

    End Sub

    Private Sub loadCustomerData(ByVal customerNo As String)
        Try
            Dim dm As New DataManager
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
                    btn_SaveOrder.Enabled = False
                    'btn_AddFromCart.Enabled = False
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

                grigliaPagamenti.DataSource = dm.GetPagamentiWO(dr.Item("No_"), dr.Item("Payment Method Code"), dr.Item("Payment Terms Code"), CType(Session("cart"), cartManager).GetTotaleMerceConScontoPagamento)
                grigliaPagamenti.DataBind()


            End If
            dm = Nothing
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub loadNewCustomerData(ByVal customerNo As String)
        Dim dm As New DataManager
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
            'comboCodeDestinatario.Enabled = False
            tb_desCode.Text = ""
            tb_desNome.Text = dr.Item("ragsocsped1") & " " & dr.Item("ragsocsped2")
            tb_desIndirizzo.Text = dr.Item("indirizzosped1") & " " & dr.Item("indirizzosped2")
            tb_desCap.Text = dr.Item("capsped")
            tb_desProvincia.Text = dr.Item("provinciasped")
            tb_desCitta.Text = dr.Item("localitasped")
            tb_desRegione.Text = ""
            tb_desTelefono.Text = dr.Item("telefono1sped")
            tb_desEmail.Text = dr.Item("emailsped")
            tb_desFax.Text = dr.Item("faxsped")

            'blocco scelta destinazione per nuovo cliente
            btnDestinazione.Enabled = False

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
            Dim dm As New DataManager
            Dim destPref As New DataTable
            If shipToCode = "FATTURAZIONE" Then 'destinazionatario indirizzo di fatturazione
                destPref = dm.GetCustomers(customerNo).Tables(0)
            Else
                destPref = dm.GetShipToAddress(customerNo, shipToCode).Tables(0)
            End If
            If destPref.Rows.Count > 0 Then
                tb_desCode.Text = shipToCode
                Session("DesCode") = tb_desCode.Text
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
            CType(Session("cart"), cartManager).Header.ordineHeader.ShipAddressCode = e.Parameter
        End If
    End Sub

    Private Sub loadSpeseTrasporto(ByVal IncludeShipCost As Integer)
        Dim dm As New datamanager
        If CType(Session("cart"), cartManager).isOrdineInPortoFranco Then
            cb_speseTrasporto.Checked = False
            cb_speseTrasporto.Enabled = False
            lb_speseTrasporto.Text = "€. " & CType(Session("cart"), cartManager).Header.IMPORTOSPESESPEDIZIONE
            CType(Session("cart"), cartManager).Header.ordineHeader.IncludeShipCost = 0
        Else
            cb_speseTrasporto.Enabled = True
            If IncludeShipCost = 1 Then
                cb_speseTrasporto.Checked = True
                lb_speseTrasporto.Text = "€. " & CType(Session("cart"), cartManager).Header.IMPORTOSPESESPEDIZIONE
                CType(Session("cart"), cartManager).Header.ordineHeader.IncludeShipCost = 1
            Else
                cb_speseTrasporto.Checked = False
                lb_speseTrasporto.Text = "€. " & CType(Session("cart"), cartManager).Header.IMPORTOSPESESPEDIZIONE
                CType(Session("cart"), cartManager).Header.ordineHeader.IncludeShipCost = 0
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub CallbackPanel_speTrasporto_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles CallbackPanel_speTrasporto.Callback
        Dim prevStatus As Boolean = cb_speseTrasporto.Checked
        If CType(Session("cart"), cartManager).isOrdineInPortoFranco Then
            cb_speseTrasporto.Enabled = False
            cb_speseTrasporto.Checked = False
            lb_speseTrasporto.Text = "€. " & CType(Session("cart"), cartManager).Header.IMPORTOSPESESPEDIZIONE
            CType(Session("cart"), cartManager).Header.ordineHeader.IncludeShipCost = 0
        Else
            cb_speseTrasporto.Enabled = True
            lb_speseTrasporto.Text = "€. " & CType(Session("cart"), cartManager).Header.IMPORTOSPESESPEDIZIONE
        End If
        If prevStatus = True Then
            Call saveSpeseTrasporto()
        End If
    End Sub

    Private Sub loadCarrelloHeader()
        Dim dm As New datamanager
        Dim carrello As cartManager = CType(Session("cart"), cartManager)

        If carrello.Header.CODICECLIENTE <> "" Then
            lb_customerNo.Text = carrello.Header.CODICECLIENTE
        Else
            lb_customerNo.Text = "[-]"
        End If

        If carrello.Header.ordineHeader.Code <> "" Then
            lb_orderNr.Text = carrello.Header.ordineHeader.Code
        Else
            lb_orderNr.Text = "CARRELLO " & carrello.Header.IDCART.ToString
        End If

        If carrello.Header.CODICECLIENTE.ToString.StartsWith("N") Then
            Call loadNewCustomerData(carrello.Header.CODICECLIENTE)
        Else
            Call loadCustomerData(carrello.Header.CODICECLIENTE)
        End If

        If carrello.Header.CODICECLIENTE.ToString.StartsWith("N") Then
            lb_customerName.Text = dm.GetNewCustomerName(carrello.Header.CODICECLIENTE)
        Else
            lb_customerName.Text = dm.GetCustomerName(carrello.Header.CODICECLIENTE)
        End If

        If carrello.Header.ordineHeader.OrderDate.Length = 10 Then
            comboOrderDate.Date = carrello.Header.getOrderDate
        End If

        If carrello.Header.PRENOTAZIONE = 1 Or carrello.Header.PRENOTAZIONE = 2 Then
            comboDataEvsione.Date = carrello.Header.DATA_EVASIONE
            comboDataEvsione.Enabled = True
        Else
            comboDataEvsione.Date = carrello.Header.DATA_EVASIONE
            comboDataEvsione.Enabled = False
        End If

        If carrello.Header.ordineHeader.Code <> "" Then
            lb_orderStatus.Text = orderstatus.getDescription(carrello.Header.ordineHeader.Status, carrello.Header.ordineHeader.CompletedImported) 'CompletedImported lo devo passare per i vecchi ordini in cui status è sempre = 0 
            If carrello.Header.PRENOTAZIONE = 1 And carrello.Header.ordineHeader.Status = 2 Then
                lb_orderStatus.Text = "INVIATO A PRODUZIONE"
            End If
            If carrello.Header.PRENOTAZIONE = 2 And carrello.Header.ordineHeader.Status = 0 Then
                lb_orderStatus.Text = "EVADIBILE"
            End If
        Else
            lb_orderStatus.Text = "CARRELLO"
        End If
        'operatore creazione
        If IsNumeric(carrello.Header.UTENTE_CREAZIONE) Then
            lb_utentecreazione.Text = dm.getUserNameSurname(carrello.Header.UTENTE_CREAZIONE)
            lb_utentecreazione.ToolTip = "Creato " & carrello.Header.DATA_CREAZIONE.ToString("f")
        ElseIf carrello.Header.ordineHeader.OperatorCode <> "" Then
            lb_OperatorCode.Text = carrello.Header.ordineHeader.OperatorCode
        Else
            lb_utentecreazione.Text = "[-]"
        End If
        'operatore modifica
        If IsNumeric(carrello.Header.UTENTE_ULTIMA_MODIFICA) Then
            lb_OperatorCode.Text = dm.getUserNameSurname(carrello.Header.UTENTE_ULTIMA_MODIFICA)
            lb_OperatorCode.ToolTip = "Modificato " & carrello.Header.DATA_ULTIMA_MODIFICA.ToString("f")
        ElseIf carrello.Header.ordineHeader.OperatorCode <> "" Then
            lb_OperatorCode.Text = carrello.Header.ordineHeader.OperatorCode
        Else
            lb_OperatorCode.Text = "[-]"
        End If
        'operatore magazziniere
        If IsNumeric(carrello.Header.ordineHeader.User) Then
            lb_user.Text = dm.getUserNameSurname(carrello.Header.ordineHeader.User)
        ElseIf carrello.Header.ordineHeader.User <> "" Then
            lb_user.Text = carrello.Header.ordineHeader.User
        Else
            lb_user.Text = "[-]"
        End If

        tbScontoHeader.Text = carrello.Header.SCONTOHEADER
        If carrello.carrelloHeaderPromos.Count > 0 Then
            btnApplicaScontoHeader.ClientEnabled = False
            tbScontoHeader.ClientEnabled = False
        End If

        tbValoreVoucher.Text = Math.Round(carrello.Header.ordineHeader.Voucher_Value, 2).ToString

        If comboOrderType.Items.FindByValue(carrello.Header.ordineHeader.Type) Is Nothing Then 'gestito per vecchi ordini
            comboOrderType.Items.Add(carrello.Header.ordineHeader.Type, carrello.Header.ordineHeader.Type)
        End If
        comboOrderType.SelectedItem = comboOrderType.Items.FindByValue(carrello.Header.ordineHeader.Type)

        If comboOrderType.SelectedItem.Value = "Visita Agente" Then
            Call loadAgenti()
            If carrello.Header.AGENTE > 0 Then
                comboAgente.SelectedItem = comboAgente.Items.FindByValue(carrello.Header.AGENTE.ToString)
            Else
                comboAgente.SelectedIndex = -1
            End If
        End If

        memoNote.Text = carrello.Header.ordineHeader.Notes

        If carrello.Header.ordineHeader.ShippingAgentCode <> "" Then
            comboCorrieri.SelectedItem = comboCorrieri.Items.FindByValue(carrello.Header.ordineHeader.ShippingAgentCode)
        Else
            comboCorrieri.SelectedIndex = -1
        End If

        If carrello.Header.ordineHeader.ShipAddressCode <> "" Then
            Call loadDestinatario(carrello.Header.CODICECLIENTE, carrello.Header.ordineHeader.ShipAddressCode)
        Else
            If Not carrello.Header.CODICECLIENTE.ToString.StartsWith("N") Then
                Call loadDestinatario(carrello.Header.CODICECLIENTE, "FATTURAZIONE")
            End If
        End If

        If carrello.Header.ORIGINATODAORDINE <> "" Then
            lb_riferimentoOrdine.Text = carrello.Header.ORIGINATODAORDINE
        Else
            lb_riferimentoOrdine.Text = "[-]"
        End If

        Call loadSpeseTrasporto(carrello.Header.ordineHeader.IncludeShipCost)

        grigliaPagamenti.DataBind()

        dm = Nothing
    End Sub

    Private Sub loadCarrelloLines()
        gridOrderLine.DataBind()
    End Sub

    Private Sub loadCarrelloFooter()
        Dim dm As New datamanager
        Dim carrello As cartManager = CType(Session("cart"), cartManager)
        Dim IVAarticoli As Double = 0
        Dim scontoPagamentoPerc As Double = 0
        Dim scontoPagamentoTotale As Double = 0
        Dim totaleimponibileLordo As Double = 0 '20200226  - gestione sconto pagamento su righe: totaleimponibileLordo = totaleimponibileNetto + scontoPagamentTotale 
        Dim totaleimponibileNetto As Double = 0
        Dim speseIncasso As Double = 0
        Dim speseTrasporto As Double = 0
        Dim totalespese As Double = 0
        Dim IVAspese As Double = 0
        Dim scontoTotaleDaFormulaRiga As Double = 0
        Dim valoreVoucher As Double = 0

        If carrello.Header.ordineHeader.IncludeShipCost = 1 Then
            speseTrasporto = CType(Session("cart"), cartManager).Header.IMPORTOSPESESPEDIZIONE
        End If

        scontoPagamentoPerc = carrello.Header.SCONTOPAGAMENTO

        If carrello.Header.ordineHeader.PaymentMethodCode <> "" Then
            speseIncasso = dm.GetSpeseIncasso(carrello.Header.CODICECLIENTE, carrello.Header.ordineHeader.PaymentMethodCode)
        End If

        Dim pagamentoSelezionatoList As List(Of Object) = grigliaPagamenti.GetSelectedFieldValues(New String() {"Metodo", "Termine", "Sconto"})
        Dim PaymentMethodCode As String = ""
        Dim PaymentTermsCode As String = ""
        If pagamentoSelezionatoList.Count > 0 Then
            PaymentMethodCode = pagamentoSelezionatoList(0)(0).ToString()
            PaymentTermsCode = pagamentoSelezionatoList(0)(1).ToString()
            scontoPagamentoPerc = pagamentoSelezionatoList(0)(2)
        End If
        If PaymentMethodCode <> "" Then
            speseIncasso = dm.GetSpeseIncasso(carrello.Header.CODICECLIENTE, PaymentMethodCode)
        End If

        totalespese = speseIncasso + speseTrasporto
        IVAspese = totalespese / 100 * 22

        valoreVoucher = carrello.Header.ordineHeader.Voucher_Value

        For Each item In carrello.carrelloLines
            Dim totaleriga As Double = 0
            If item.ordineLine.RowDiscount = 0 Then
                totaleriga = (item.ordineLine.UnitPrice * item.ordineLine.Quantity)
            Else
                totaleriga = 0.0
            End If
            If item.SCONTORIGA > 0 Then
                totaleriga = totaleriga - (totaleriga / 100 * item.SCONTORIGA)
            End If
            '20200424 - Gestione scontoScaglioni
            If item.SCONTOHEADER <> "0" And item.SCONTOHEADER <> "" Then
                Dim scontiH() As String = item.SCONTOHEADER.ToString.Split("+")
                For i = 0 To scontiH.Count - 1
                    If IsNumeric(scontiH(i)) Then
                        totaleriga = totaleriga - (totaleriga / 100 * scontiH(i))
                    End If
                Next
            End If
            totaleimponibileLordo = totaleimponibileLordo + totaleriga
            If item.SCONTOPAGAMENTO > 0 Then
                scontoPagamentoTotale = scontoPagamentoTotale + (totaleriga / 100 * item.SCONTOPAGAMENTO)
                totaleriga = totaleriga - (totaleriga / 100 * item.SCONTOPAGAMENTO)
            End If
            totaleimponibileNetto += totaleriga
            IVAarticoli += (totaleriga / 100 * Convert.ToInt32(item.IVA))
        Next

        Dim IVAtotale As Double = Math.Round((IVAarticoli + IVAspese), 2)
        totaleimponibileNetto = Math.Round(totaleimponibileNetto, 2)
        totalespese = Math.Round(totalespese, 2)
        Dim TotaleOrdine As Double = totalespese + IVAtotale + totaleimponibileNetto - valoreVoucher


        lb_footerTotaleImponibile.Text = String.Format("€ {0:F2}", totaleimponibileLordo)
        lb_footerScontoPagamento.Text = String.Format("-€ {0:F2}", scontoPagamentoTotale)
        lb_footerTotaleImponibileNetto.Text = String.Format("€ {0:F2}", totaleimponibileNetto)
        lb_footerTotIVA.Text = String.Format("€ {0:F2}", IVAtotale)
        lb_footerScontoPagamentoPerc.Text = String.Format("-{0:F2}%", scontoPagamentoPerc)
        lb_footerSpeseIncasso.Text = String.Format("€ {0:F2}", speseIncasso)
        lb_footerSpeseTrasporto.Text = String.Format("€ {0:F2}", speseTrasporto)
        lb_footerVoucher.Text = String.Format("-€ {0:F2}", valoreVoucher)
        lb_footerTotaleOrdine.Text = String.Format("€ {0:F2}", TotaleOrdine)

        dm = Nothing

        Call setButtonVisibility()

    End Sub

    Protected Sub grigliaPagamenti_CommandButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCommandButtonEventArgs) Handles grigliaPagamenti.CommandButtonInitialize
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        If (e.ButtonType = ColumnCommandButtonType.SelectCheckbox) Then
            If grid.Selection.IsRowSelected(e.VisibleIndex) Then
                CType(Session("cart"), cartManager).Header.ordineHeader.PaymentMethodCode = grid.GetRowValues(e.VisibleIndex, "Metodo")
                CType(Session("cart"), cartManager).Header.ordineHeader.PaymentTermsCode = grid.GetRowValues(e.VisibleIndex, "Termine")
                CType(Session("cart"), cartManager).Header.SCONTOPAGAMENTO = grid.GetRowValues(e.VisibleIndex, "Sconto")
                grid.Selection.SelectRow(e.VisibleIndex)
            End If
            If grid.Selection.Count = 0 Then
                If grid.GetRowValues(e.VisibleIndex, "Metodo") = CType(Session("cart"), cartManager).Header.ordineHeader.PaymentMethodCode And grid.GetRowValues(e.VisibleIndex, "Termine") = CType(Session("cart"), cartManager).Header.ordineHeader.PaymentTermsCode Then
                    grid.Selection.SelectRow(e.VisibleIndex)
                End If
            End If
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

    Protected Sub tbqta_Init(sender As Object, e As EventArgs)
        Dim qtaCtrl As ASPxTextBox = CType(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(qtaCtrl.NamingContainer, GridViewDataItemTemplateContainer)
        Dim CodiceArticolo As String = DataBinder.Eval(container.DataItem, "ordineLine.ItemCode")
        Dim Lotto As String = DataBinder.Eval(container.DataItem, "ordineLine.LotNo")
        Dim IDPROMO As String = DataBinder.Eval(container.DataItem, "IDPROMO")

        Dim dm As New datamanager
        Dim dispoLottoRow() As DataRow = dm.GetDisponibilitaProdottiPerLotto(CodiceArticolo, True).Select("Lotto='" & Lotto & "'")
        Dim dispoLotto As Integer = 0
        If dispoLottoRow.Count > 0 Then
            dispoLotto = dispoLottoRow(0).Item("DisponibilitaLotto")
        End If
        Dim qtaLottoInCart As Integer = CType(Session("cart"), cartManager).getQtaLottoInCart(CodiceArticolo, Lotto)
        Dim qta As Integer = DataBinder.Eval(container.DataItem, "ordineLine.Quantity")
        Dim qtaDiscount As Integer = DataBinder.Eval(container.DataItem, "ordineLine.DiscountQty")
        Dim rowDiscount As Integer = DataBinder.Eval(container.DataItem, "ordineLine.RowDiscount")
        Dim isArcheopatico As Boolean = dm.isArcheopatico(CodiceArticolo)

        If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
            If dispoLotto > 0 Then
                qtaCtrl.Enabled = True
                'qtaCtrl.MaskSettings.Mask = "<0.." & dispoLotto & ">"
            Else
                'qtaCtrl.MaskSettings.Mask = "<0.." & qta & ">"
            End If
        Else
            'qtaCtrl.MaskSettings.Mask = "<0.." & (qta + dispoLotto + qtaDiscount) & ">"
        End If


        If rowDiscount = 1 Then
            qtaCtrl.Text = qtaDiscount
            'If IDPROMO > 0 Then
            ' qtaCtrl.Enabled = False
            ' End If
        Else
            qtaCtrl.Text = qta
        End If
        If isArcheopatico Then
            qtaCtrl.Enabled = True
            If Lotto <> "" Then
                If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                    'qtaCtrl.MaskSettings.Mask = "<0.." & (dispoLotto) & ">"
                Else
                    'qtaCtrl.MaskSettings.Mask = "<0.." & (qta + dispoLotto + qtaDiscount) & ">"
                End If
            Else
                'qtaCtrl.MaskSettings.Mask = "<0..9999>"
            End If
            qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaValidation(s,e,{0},{1}); }}", container.VisibleIndex, 1)
        Else
            qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaValidation(s,e,{0},{1}); }}", container.VisibleIndex, 0)
        End If

        'gestione ordini prenotazione
        If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 1 Then
            qtaCtrl.Enabled = True
            qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaValidation(s,e,{0},{1}); }}", container.VisibleIndex, 1)
        End If

        If DataBinder.Eval(container.DataItem, "ordineLine.Imported") > 0 Or CType(Session("cart"), cartManager).isOrdineChiuso Then
            qtaCtrl.Visible = False
        End If
        dm = Nothing
    End Sub

    Protected Sub lb_Qta_Init(sender As Object, e As EventArgs)
        Dim lbqtaCtrl As ASPxLabel = CType(sender, ASPxLabel)
        Dim container As GridViewDataItemTemplateContainer = TryCast(lbqtaCtrl.NamingContainer, GridViewDataItemTemplateContainer)
        Dim qtaDiscount As String = CInt(DataBinder.Eval(container.DataItem, "ordineLine.DiscountQty")).ToString
        Dim Quantity As String = CInt(DataBinder.Eval(container.DataItem, "ordineLine.Quantity")).ToString
        Dim rowDiscount As Integer = DataBinder.Eval(container.DataItem, "ordineLine.RowDiscount")
        If rowDiscount = 1 Then
            lbqtaCtrl.Text = qtaDiscount
        Else
            lbqtaCtrl.Text = Quantity
        End If
        If DataBinder.Eval(container.DataItem, "ordineLine.Imported") > 0 Or CType(Session("cart"), cartManager).isOrdineChiuso Then
            lbqtaCtrl.Visible = True
        Else
            lbqtaCtrl.Visible = False
        End If
    End Sub

    Protected Sub tbscontopercentuale_Init(sender As Object, e As EventArgs)
        Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
        Dim FormulaSconto As String = DataBinder.Eval(container.DataItem, "ordineLine.FormulaSconto")
        Dim RowDiscount As Integer = DataBinder.Eval(container.DataItem, "ordineLine.RowDiscount")

        '20200226  - gestione sconto pagamento su righe
        Dim scontoRigaPerc As Integer = DataBinder.Eval(container.DataItem, "SCONTORIGA")
        ctrl.Text = scontoRigaPerc

        If RowDiscount = 1 Then
            ctrl.Text = "100"
            ctrl.Enabled = False
        End If
        ctrl.ReadOnly = Not CType(Session("user"), user).iSpromorigascoperc
    End Sub

    Protected Sub lbScontoHeader_Init(sender As Object, e As EventArgs)
        Dim ctrl As ASPxLabel = CType(sender, ASPxLabel)
        Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
        'Dim FormulaSconto As String = DataBinder.Eval(container.DataItem, "ordineLine.FormulaSconto")

        '20200226 - gestione sconto pagamento su righe
        '20200424 - Gestione scontoScaglioni
        Dim scontoHeader As String = DataBinder.Eval(container.DataItem, "SCONTOHEADER")
        If scontoHeader <> "0" Then
            ctrl.Text = scontoHeader
        Else
            ctrl.Text = "0"
        End If
    End Sub

    Protected Sub lbScontoRigaPag_Init(sender As Object, e As EventArgs)
        Dim ctrl As ASPxLabel = CType(sender, ASPxLabel)
        Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
        Dim FormulaSconto As String = DataBinder.Eval(container.DataItem, "ordineLine.FormulaSconto")
        Dim PaymentMethodCode As String = CType(Session("cart"), cartManager).Header.ordineHeader.PaymentMethodCode
        Dim PaymentTermsCode As String = CType(Session("cart"), cartManager).Header.ordineHeader.PaymentTermsCode

        '20200226  - gestione sconto pagamento su righe
        Dim scontoPagamento As Integer = DataBinder.Eval(container.DataItem, "SCONTOPAGAMENTO")
        If scontoPagamento > 0 Then
            ctrl.Text = scontoPagamento
        Else
            ctrl.Text = "0"
        End If
    End Sub

    Protected Sub gridOrderLine_DataBinding(sender As Object, e As EventArgs) Handles gridOrderLine.DataBinding
        Dim clines As List(Of carrelloLine) = CType(Session("cart"), cartManager).carrelloLinesSorted()
        gridOrderLine.DataSource = clines
    End Sub

    Private Sub gridOrderLine_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles gridOrderLine.HtmlRowPrepared
        If e.RowType <> GridViewRowType.Data Then
            Return
        End If

        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim qtaCtrl As ASPxTextBox = grid.FindRowCellTemplateControl(e.VisibleIndex, grid.Columns("Quantita"), "tbqta")
        Dim quantita As Integer = qtaCtrl.Text
        Dim disponibilita As Integer = e.GetValue("DISPONIBILITA")
        Dim dispolotto As Integer = e.GetValue("DISPOLOTTO")
        Dim codiceArticolo As String = e.GetValue("ordineLine.ItemCode")
        Dim ID As Int64 = e.GetValue("IDPROMO")
        Dim RowDiscount As Integer = e.GetValue("ordineLine.RowDiscount")
        Dim FormulaSconto As String = e.GetValue("ordineLine.FormulaSconto")
        'FormulaSconto = FormulaSconto.Replace(",", ".")

        If ID > 0 And RowDiscount = 0 Then
            'e.Row.BackColor = Drawing.Color.LightGreen
            e.Row.ToolTip = "Riga a pagamento associata ad una riga omaggio o con sconto"
        ElseIf RowDiscount = 1 Then
            e.Row.BackColor = Drawing.Color.LightGreen
            e.Row.ToolTip = "Riga Sconto Merce (Omaggio)"
        ElseIf FormulaSconto.Contains("-") Or (CType(Session("cart"), cartManager).Header.SCONTOHEADER = "0" AndAlso IsNumeric(FormulaSconto) AndAlso Convert.ToInt32(FormulaSconto) > 0) Then
            'e.Row.BackColor = Drawing.Color.LightGreen
            e.Row.ToolTip = "Riga Sconto Percentuale"
        End If

        If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
            If dispolotto < 0 Then
                e.Row.BackColor = Drawing.Color.Red
                e.Row.ToolTip = "La giacenza pari a " & dispolotto & " non è sufficiente per evadere " & quantita & " pezzi."
            End If
        End If

        If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
            Dim dm As New datamanager
            If dm.isArcheopatico(codiceArticolo) Then
                If dispolotto < 0 Then
                    e.Row.BackColor = Drawing.Color.Red
                    e.Row.ToolTip = "La giacenza pari a " & dispolotto & " non è sufficiente per evadere " & quantita & " pezzi."
                End If
            End If
            dm = Nothing
        End If

    End Sub

    Protected Sub gridOrderLine_HtmlDataCellPrepared(sender As Object, e As ASPxGridViewTableDataCellEventArgs) Handles gridOrderLine.HtmlDataCellPrepared
        If e.DataColumn.FieldName = "ordineLine.FormulaSconto" Then
            If (IsNumeric(e.CellValue) AndAlso Convert.ToInt32(e.CellValue) > 0) Or e.GetValue("ordineLine.RowDiscount") = 1 Then
                e.Cell.Font.Bold = True
            Else
                e.Cell.Text = ""
            End If
        End If
    End Sub

    Protected Sub gridOrderLine_CustomColumnDisplayText(sender As Object, e As ASPxGridViewColumnDisplayTextEventArgs) Handles gridOrderLine.CustomColumnDisplayText
        If e.Column.FieldName = "ordineLine.FormulaSconto" Then
            If e.GetFieldValue(e.VisibleRowIndex, "ordineLine.RowDiscount") = 1 Then
                e.DisplayText = "100"
            End If
        End If
    End Sub

    Protected Sub gridOrderLine_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles gridOrderLine.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        If e.CellType = GridViewTableCommandCellType.Data Then
            Dim grid As ASPxGridView = CType(sender, ASPxGridView)
            Dim IDPROMO As Integer = grid.GetRowValues(e.VisibleIndex, "IDPROMO")
            Dim rowdiscount As Integer = grid.GetRowValues(e.VisibleIndex, "ordineLine.RowDiscount")
            If e.ButtonID = "zeroPrice" Then
                If IDPROMO > 0 Then
                    e.Enabled = False
                    e.Image.Url = "~/images/zeroPrice_off.png"
                    e.Image.ToolTip = "Riga Promozione"
                Else
                    If rowdiscount > 0 Then
                        e.Enabled = False
                        e.Image.Url = "~/images/zeroPriceApp.png"
                        e.Image.ToolTip = "Riga Omaggio"
                        'e.Image.Url = "~/images/zeroPriceDel.png"
                        'e.Image.ToolTip = "Disattiva Zero Price - Omaggio"
                    Else
                        e.Image.ToolTip = "Aggiungi questo articolo come Omaggio"
                        e.Enabled = True
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridOrderLine_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles gridOrderLine.CustomButtonCallback
        If e.VisibleIndex = -1 Then
            Return
        End If
        Dim dm As New datamanager
        Dim ItemCode As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.ItemCode").ToString
        Dim Lotto As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.LotNo").ToString
        Dim qta As Integer = CType(CType(sender, ASPxGridView).FindRowCellTemplateControl(e.VisibleIndex, Nothing, "tbqta"), ASPxTextBox).Text
        Dim RowDiscount As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.RowDiscount")
        Dim qtaDiscount As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.DiscountQty")
        Dim formulaSconto As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.FormulaSconto")
        Dim scontopercentuale As String = CType(CType(sender, ASPxGridView).FindRowCellTemplateControl(e.VisibleIndex, Nothing, "tbscontopercentuale"), ASPxTextBox).Text
        Dim LotNo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.LotNo")
        Dim lineNo As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ordineLine.LineNo")
        Dim IDPROMO As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "IDPROMO")
        Dim idPromoHeader As Int64 = 0

        If e.ButtonID = "delFromCart" Then
            CType(Session("cart"), cartManager).deleteLine(ItemCode, LotNo, RowDiscount, IDPROMO, scontopercentuale)
        ElseIf e.ButtonID = "updCart" Then
            If qta > 0 Then
                CType(Session("cart"), cartManager).updateScontoPercentuale(lineNo, scontopercentuale)
                If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 1 Then
                    CType(Session("cart"), cartManager).updateLinePrenotazione(ItemCode, qta, LotNo, True, False, RowDiscount, scontopercentuale)
                Else
                    CType(Session("cart"), cartManager).updateLine(ItemCode, qta, LotNo, True, False, RowDiscount, scontopercentuale)
                End If
            End If
        ElseIf e.ButtonID = "zeroPrice" Then
            If RowDiscount = 0 Then
                If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 1 Then
                    CType(Session("cart"), cartManager).addLinePrenotazione(ItemCode, 1, "", False, False, 1, False, "", 0, "")
                Else
                    CType(Session("cart"), cartManager).addLine(ItemCode, 1, "", False, False, 1, False, "", 0, "")
                End If
            End If
        End If

        idPromoHeader = CType(Session("cart"), cartManager).checkPromoHeader(True)
        CType(sender, ASPxGridView).JSProperties("cpScontoHeader") = CType(Session("cart"), cartManager).Header.SCONTOHEADER
        CType(sender, ASPxGridView).JSProperties("cpScontoHeaderDaPromo") = idPromoHeader

        gridOrderLine.DataBind()
        Call loadCarrelloFooter()
        dm = Nothing
    End Sub

    Protected Sub gridOrderLine_CustomJSProperties(sender As Object, e As ASPxGridViewClientJSPropertiesEventArgs) Handles gridOrderLine.CustomJSProperties
        Dim qtyDispoValues As New Dictionary(Of Integer, Integer)()
        Dim qtyOldQtaValues As New Dictionary(Of Integer, Integer)()
        For i = 0 To gridOrderLine.VisibleRowCount - 1
            Dim dispolotto As Integer = gridOrderLine.GetRowValues(i, "DISPOLOTTO")
            'Dim qtaCart As Integer = CType(Session("cart"), cartManager).getQtaLottoInCart(gridOrderLine.GetRowValues(i, "ordineLine.ItemCode"), gridOrderLine.GetRowValues(i, "ordineLine.LotNo"))
            Dim qtaLine As Integer = CType(CType(sender, ASPxGridView).FindRowCellTemplateControl(i, Nothing, "tbqta"), ASPxTextBox).Text
            'Dim qtaLine As Integer = gridOrderLine.GetRowValues(i, "ordineLine.Quantity")
            Dim dispolottototale = dispolotto + qtaLine
            qtyDispoValues.Add(i, dispolottototale)
            qtyOldQtaValues.Add(i, qtaLine)
        Next
        e.Properties("cpQtyDispoValues") = qtyDispoValues
        e.Properties("cpqtyOldQtaValues") = qtyOldQtaValues
    End Sub

    Protected Sub ASPxCallback_report_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles ASPxCallback_report.Callback
        Session("orderCodeReport") = lb_orderNr.Text.Trim
    End Sub

    Protected Sub callbackCtrl_Callback(source As Object, e As CallbackEventArgs) Handles callbackCtrl.Callback
        If Not e.Parameter Is Nothing Then
            Dim escludiArcheopatici As Boolean = False
            Dim salvaEinviaDaCarrello As Boolean = False
            If e.Parameter = "salva" Then
                escludiArcheopatici = True
                salvaEinviaDaCarrello = False
            ElseIf e.Parameter = "salvaelista" Then
                escludiArcheopatici = True
                salvaEinviaDaCarrello = False
            ElseIf e.Parameter = "approvazione" Then
                escludiArcheopatici = True
                salvaEinviaDaCarrello = False
            ElseIf e.Parameter = "produzione" Then
                escludiArcheopatici = True
                salvaEinviaDaCarrello = False
            ElseIf e.Parameter = "invia" Then
                escludiArcheopatici = False
                salvaEinviaDaCarrello = True
            ElseIf e.Parameter = "annulla" Then
                If memoNote.Text.Trim = "" Then
                    e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlannulla", "Esito negativo", "Specificare una motivazione per annullare l'ordine (nel campo note).", e.Parameter)
                    Exit Sub
                Else
                    e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlannullaconferma", "Richiesta conferma", "Confermi eliminazione dell'ordine nr. <b>" & CType(Session("cart"), cartManager).Header.ordineHeader.Code & "</b>?", e.Parameter)
                    Exit Sub
                End If
            End If

            If Not CType(Session("cart"), cartManager).controlloGiacenze(escludiArcheopatici, salvaEinviaDaCarrello) Then 'controllo giacenza
                e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlgiacenze", "Esito negativo", "Controllare le giacenze degli articoli.", e.Parameter)
                Exit Sub
            End If
            If (CType(Session("cart"), cartManager).Header.ordineHeader.Type = "Visita Agente" AndAlso CType(Session("cart"), cartManager).Header.AGENTE = 0) Then 'controllo su scelta agente
                e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlagente", "Esito negativo", "Selezionare l'AGENTE prima di proseguire.", e.Parameter)
                Exit Sub
            End If
            If Not CType(Session("cart"), cartManager).isOrdineInImportoMinimo Then 'controllo importo minimo
                e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlimportominimo", "Richiesta conferma", "L'ordine non raggiunge l'importo minimo di evadibilità di €." & CType(Session("cart"), cartManager).Header.IMPORTOEVADIBILITAORDINE & ". <br> Proseguire ugualmente?", e.Parameter)
                Exit Sub
            End If
            If CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N") Then 'controllo iban - riba per nuovo cliente
                If grigliaPagamenti.GetSelectedFieldValues(New String() {"Metodo", "Termine", "Sconto"})(0)(0).ToString() = "RB" Then
                    Dim dm As New datamanager
                    Dim iban As String = dm.GetNewCustomerIBAN(Session("selectedNewCustomer"))
                    dm = Nothing
                    If iban = "" Then
                        e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlriba", "Esito negativo", "Il pagamento tramite riba non è possibile: IBAN mancante.", e.Parameter)
                        Exit Sub
                    End If
                End If
            End If

            Dim esito() As String = processaOrdine(e.Parameter)
            e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlprocesso", esito(0), esito(1), e.Parameter)
        End If
    End Sub

    Protected Sub callbackProcess_Callback(source As Object, e As CallbackEventArgs) Handles callbackProcess.Callback
        If Not e.Parameter Is Nothing Then
            Dim esito() As String = processaOrdine(e.Parameter)
            e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlprocesso", esito(0), esito(1), e.Parameter)
        End If
    End Sub

    Private Function processaOrdine(ByVal azione As String) As String()
        Dim result() As String = {"", ""}
        Try
            Dim dm As New datamanager
            Log.Info(CType(Session("user"), user).nomeCompleto & " processed order " & CType(Session("cart"), cartManager).Header.ordineHeader.Code & " with action: " & azione)
            If azione = "annulla" Then
                If dm.annullaOrdine(CType(Session("cart"), cartManager).Header.ordineHeader.Code, CType(Session("user"), user).userCode, memoNote.Text.Trim) Then
                    Call closeCurrentWork(azione)
                Else
                    result(0) = "Esito negativo"
                    result(1) = "Eliminazione ordine non riuscita. Riprovare tra qualche istante."
                End If
            Else
                If saveOrder(True) > 0 Then
                    If azione = "salva" Then
                        result(0) = "Esito positivo"
                        result(1) = "Ordine salvato."
                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderDetails.aspx")
                    ElseIf azione = "salvaelista" Then
                        result(0) = "Esito positivo"
                        result(1) = "Ordine salvato."
                        Call closeCurrentWork(azione)
                    ElseIf azione = "produzione" Then
                        If dm.produciOrdine(CType(Session("cart"), cartManager).Header.ordineHeader.Code, CType(Session("user"), user).userCode) Then
                            result(0) = "ok"
                            result(1) = ""
                        Else
                            result(0) = "ko"
                            result(1) = ""
                        End If
                    ElseIf azione = "approvazione" Then
                        If dm.richiediApprovazioneOrdine(CType(Session("cart"), cartManager).Header.ordineHeader.Code, CType(Session("user"), user).userCode) Then
                            Call sendReportEmail(CType(Session("cart"), cartManager).Header.ordineHeader.Code, True)
                            Call closeCurrentWork(azione)
                        Else
                            result(0) = "Esito negativo"
                            result(1) = "Richiesta approvazione ordine non riuscita. Riprovare tra qualche istante."
                        End If
                    ElseIf azione = "invia" Then
                        If dm.closeOrder(CType(Session("cart"), cartManager).Header.ordineHeader.Code, CType(Session("user"), user).userCode) Then
                            Call sendReportEmail(CType(Session("cart"), cartManager).Header.ordineHeader.Code)
                            Call savePDFReport(CType(Session("cart"), cartManager).Header.ordineHeader.Code)
                            Call closeCurrentWork(azione)
                        Else
                            result(0) = "Esito negativo"
                            result(1) = "Invio ordine a magazzino non riuscita. Riprovare tra qualche istante."
                        End If
                    End If

                Else
                    result(0) = "Esito negativo"
                    result(1) = "Salvataggio ordine non riuscito. Riprovare tra qualche istante."
                End If
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex.ToString)
            result(0) = "Esito negativo"
            result(1) = "Salvataggio ordine non riuscito. Riprovare tra qualche istante."
        End Try
        Return result
    End Function

    Private Sub closeCurrentWork(ByVal azione As String)
        Dim isNuovoCLiente As Boolean = CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N")
        'reset variabili di sessione
        Dim isPrenotazione As Boolean = (CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 1 Or CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 2)


        Session("infoCustomerNo") = ""
        Session("orderCodeReport") = ""
        CType(Session("cart"), cartManager).clearCart()

        Log.Info("Close current work and clear cart session")

        'gestione redirect dopo azione su ordine
        If azione = "salva" Then
            'DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=0&ci=0")
        ElseIf azione = "salvaelista" Then
            If isNuovoCLiente Then
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/newcustomerOrderList.aspx")
            Else
                If isPrenotazione Then
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=0,1,2,3,4,5,6,7&ci=0,1&ns=99&nci=99&p=1,2")
                Else
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=0&ci=0")
                End If
            End If
        ElseIf azione = "approvazione" Then
            If isNuovoCLiente Then
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/newcustomerOrderList.aspx")
            Else
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=7&ci=0,1&ns=0&nci=0")
            End If
        ElseIf azione = "produzione" Then
            If isNuovoCLiente Then
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/newcustomerOrderList.aspx")
            Else
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=1&ci=0")
            End If
        ElseIf azione = "invia" Then
            If isNuovoCLiente Then
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/newcustomerOrderList.aspx")
            Else
                If isPrenotazione Then
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=0,1,2,3,4,5,6,7&ci=0,1&ns=99&nci=99&p=1,2")
                Else
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=2&ci=0,1&ns=0&nci=0")
                End If
            End If
        ElseIf azione = "annulla" Then
            If isPrenotazione Then
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=0,1,2,3,4,5,6,7&ci=0,1&ns=99&nci=99&p=1,2")
            Else
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/orderList.aspx?s=6&ci=0,1&ns=0&nci=0")
            End If
        End If
    End Sub

    Private Function saveCarrello() As Integer
        Dim esito As Integer
        If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
            Dim dm As New datamanager
            Call saveHeaderSession()
            esito = dm.salvaCarrello2(CType(Session("cart"), cartManager))
            dm = Nothing
        End If
        Return esito
    End Function

    Private Sub saveHeaderSession()
        If Session("DesCode") IsNot Nothing Then
            tb_desCode.Text = Session("DesCode").ToString() 'domenico: ricarica il valore di tb_desCode dalla sessione, altrimenti risulterebbe vuoto
        End If
        CType(Session("cart"), cartManager).Header.UTENTE_ULTIMA_MODIFICA = CType(Session("user"), user).userCode
        If Not comboCorrieri.SelectedItem Is Nothing Then
            CType(Session("cart"), cartManager).Header.ordineHeader.ShippingAgentCode = comboCorrieri.SelectedItem.Value
        End If
        If tb_desCode.Text.Trim <> "FATTURAZIONE" Then
            CType(Session("cart"), cartManager).Header.ordineHeader.ShipAddressCode = tb_desCode.Text.Trim
        Else
            CType(Session("cart"), cartManager).Header.ordineHeader.ShipAddressCode = ""
        End If
        CType(Session("cart"), cartManager).Header.ordineHeader.Notes = memoNote.Text
        Dim strday As String = IIf(comboOrderDate.Date.Day.ToString.Length = 2, comboOrderDate.Date.Day.ToString, "0" & comboOrderDate.Date.Day.ToString)
        Dim strmonth As String = IIf(comboOrderDate.Date.Month.ToString.Length = 2, comboOrderDate.Date.Month.ToString, "0" & comboOrderDate.Date.Month.ToString)
        Dim stryear As String = comboOrderDate.Date.Year.ToString
        CType(Session("cart"), cartManager).Header.ordineHeader.OrderDate = String.Format("{0}/{1}/{2}", strday, strmonth, stryear)
        CType(Session("cart"), cartManager).Header.DATA_CREAZIONE = New Date(CInt(stryear), CInt(strmonth), CInt(strday), Now.Hour, Now.Minute, Now.Second)
        If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 1 Or CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 2 Then
            CType(Session("cart"), cartManager).Header.DATA_EVASIONE = New Date(comboDataEvsione.Date.Year, comboDataEvsione.Date.Month, comboDataEvsione.Date.Day)
        ElseIf CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 0 Then
            CType(Session("cart"), cartManager).Header.DATA_EVASIONE = New Date(comboOrderDate.Date.Year, comboOrderDate.Date.Month, comboOrderDate.Date.Day)
        End If
    End Sub

    Protected Sub ASPxCallback_saveCarrello_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles ASPxCallback_saveCarrello.Callback
        If Not (CType(Session("cart"), cartManager).Header.ordineHeader.Type = "Visita Agente" AndAlso CType(Session("cart"), cartManager).Header.AGENTE = 0) Then 'controllo su scelta agente
            If saveCarrello() > 0 Then
                e.Result = String.Format("{0}|{1}", "Esito positivo", "Salvataggio carrello completato.")
            Else
                e.Result = String.Format("{0}|{1}", "Esito negativo", "Salvataggio del carrello non riuscito. Riprovare tra qualche istante.")
            End If
        Else
            'messaggio errore scelta agente
            e.Result = String.Format("{0}|{1}", "Esito negativo", "Selezionare l'AGENTE prima di proseguire.")
        End If
    End Sub

    Private Function saveOrder(Optional ByVal lineWithPromo As Boolean = False) As Integer
        Try
            Dim dm As New datamanager
            Dim esito As Integer = 0
            Call saveHeaderSession()
            If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then      'inserisco nuovo ordine dal carrello
                Session("cart") = dm.addOrderFromCarrello(CType(Session("cart"), cartManager))
                If CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N") Then
                    dm.addOrderForNewCustomer(CType(Session("cart"), cartManager).Header.ordineHeader.Code, CType(Session("cart"), cartManager).Header.CODICECLIENTE)
                End If
                esito = 1
            Else    'ordine esistente
                If dm.salvaCarrello2(CType(Session("cart"), cartManager)) > 0 Then
                    esito = 1
                End If
            End If
            Return esito
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex.ToString)
        End Try
    End Function

    ''' <summary>
    ''' Invia report email
    ''' </summary>
    ''' <param name="orderNo">Numero ordine</param>
    ''' <param name="askForCtrl">Email interna per richiesta controllo e approvazione</param>
    Private Sub sendReportEmail(ByVal orderNo As String, Optional ByVal askForCtrl As Boolean = False)
        Try
            Dim dm As New datamanager
            Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
            Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)

            Dim emailBanner As String = My.Settings.emailBanner.ToString()
            Dim emailLink As String = My.Settings.emailLink.ToString()
            Dim EmailFooter As String = ""

            If emailBanner <> "" Then
                EmailFooter &= vbCrLf & "<br>"
                If emailLink <> "" Then EmailFooter &= "<a href=""" & emailLink & """ target=""_blank"">"
                EmailFooter &= "<img src=""" & emailBanner & """>"
                If emailLink <> "" Then EmailFooter &= "</a>"
            End If

            If attivaNotificheEmail = "1" AndAlso emailSistema <> "" Then
                Dim attivaNotificaOrdineAlCliente As String = dm.GetParametroSito(parametriSitoValue.attivaNotificaOrdineAlCliente)
                Dim attivaNotificaNuovoOrdine As String = dm.GetParametroSito(parametriSitoValue.attivaNotificaNuovoOrdine)
                Dim emailNotificaOrdini As String = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini)
                Dim emailNotificaOrdini2 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini2)
                Dim emailNotificaOrdini3 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini3)
                Dim attivaNotificaRevisioneOrdine As String = dm.GetParametroSito(parametriSitoValue.attivaNotificaRevisioneOrdine)
                Dim emailRevisioneOrdini As String = dm.GetParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini)
                Dim emailRevisioneOrdini2 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini2)
                Dim emailRevisioneOrdini3 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini3)
                Dim emailSistemaDesc As String = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)
                If emailSistemaDesc = "" Then emailSistemaDesc = emailSistema
                Dim orderHeader As DataRow = dm.getOrderHeaderWO(orderNo).Tables(0).Rows(0)
                Dim CustomerNo As String = orderHeader("CustomerNo").ToString
                Dim customerName As String = ""
                Dim listaEmailCliente As New List(Of String)
                If CustomerNo.StartsWith("N") Then
                    customerName = dm.GetNewCustomerName(orderHeader("CustomerNo"))
                    listaEmailCliente = dm.GetNewCustomerEmail(CustomerNo)
                Else
                    customerName = dm.GetCustomerName(orderHeader("CustomerNo"))
                    listaEmailCliente = dm.GetCustomerEmail(CustomerNo)
                End If
                Dim Prenotazione As Integer = orderHeader("PRENOTAZIONE")
                Dim DataEvasione As DateTime = orderHeader("DATA_EVASIONE")

                Dim client As SmtpClient = New SmtpClient
                Dim msg As MailMessage = New MailMessage()
                msg.IsBodyHtml = True
                Dim Att As Attachment = createOrderReportForEmail(orderNo)
                If Att IsNot Nothing Then
                    msg.Attachments.Add(Att)
                End If
                msg.From = New MailAddress(emailSistema, emailSistemaDesc)

                If askForCtrl AndAlso attivaNotificaRevisioneOrdine = "1" Then
                    If emailRevisioneOrdini <> "" Then
                        msg.To.Add(emailRevisioneOrdini)
                        If emailRevisioneOrdini2 <> "" Then
                            msg.CC.Add(emailRevisioneOrdini2)
                        End If
                        If emailRevisioneOrdini3 <> "" Then
                            msg.CC.Add(emailRevisioneOrdini3)
                        End If
                        If CType(Session("user"), user).email <> "" Then
                            msg.CC.Add(CType(Session("user"), user).email)
                        End If
                        msg.Subject = "Nuovo ordine web nr. " & orderNo & " del " & orderHeader("OrderDate") & " - RICHIESTA APPROVAZIONE"
                        msg.Body = "Buongiorno, " & vbCrLf & "<br>"
                        msg.Body &= "&egrave; stato inserito nel sistema un nuovo ordine in attesa di APPROVAZIONE." & vbCrLf & "<br>"
                        msg.Body &= "------------------------------" & vbCrLf & "<br>"
                        msg.Body &= "ORDINE NR: " & orderNo & " del " & orderHeader("OrderDate") & vbCrLf & "<br>"
                        msg.Body &= "CLIENTE: " & CustomerNo & " - " & customerName & vbCrLf & "<br>"
                        msg.Body &= "OPERATORE: " & CType(Session("user"), user).nomeCompleto & vbCrLf & "<br>"
                        msg.Body &= "------------------------------" & vbCrLf & "<br>"
                        msg.Body &= EmailFooter
                        Try
                            client.Send(msg)
                            Log.Info("Email " & msg.Subject & " sent to " & msg.To.ToString & " " & msg.CC.ToString)
                        Catch ex As Exception
                            Log.Error(ex.ToString)
                            Exit Sub
                        End Try
                    End If
                ElseIf Not askForCtrl AndAlso attivaNotificaNuovoOrdine = "1" Then
                    If emailNotificaOrdini <> "" Then
                        msg.To.Add(emailNotificaOrdini)
                        If emailNotificaOrdini2 <> "" Then
                            msg.CC.Add(emailNotificaOrdini2)
                        End If
                        If emailNotificaOrdini3 <> "" Then
                            msg.CC.Add(emailNotificaOrdini3)
                        End If
                        If CType(Session("user"), user).email <> "" Then
                            msg.CC.Add(CType(Session("user"), user).email)
                        End If
                        If Not CustomerNo.StartsWith("N") Then
                            msg.Body = "Spettabile " & customerName & vbCrLf & "<br>"
                            If Prenotazione = 0 Then
                                msg.Subject = "Nuovo ordine web nr. " & orderNo & " del " & orderHeader("OrderDate")
                                msg.Body &= "Siamo lieti di informarLa che il Suo ordine &egrave; stato inserito nel sistema." & vbCrLf & "<br>"
                            ElseIf Prenotazione = 1 Then 'prenotato inviato a produzione
                                msg.Subject = "Nuovo ordine web di prenotazione nr. " & orderNo & " del " & orderHeader("OrderDate")
                                msg.Body &= "Siamo lieti di informarLa che il Suo ordine &egrave; stato inserito nel sistema." & vbCrLf & "<br>"
                                msg.Body &= "L'evasione dell'ordine è prevista per il giorno " & DataEvasione.ToString("dd/MM/yyyy") & "." & vbCrLf & "<br>"
                            ElseIf Prenotazione = 2 Then 'evadibile inviato a magazzino
                                msg.Subject = "Ordine web di prenotazione nr. " & orderNo & " del " & orderHeader("OrderDate")
                                msg.Body &= "Siamo lieti di informarLa che il Suo ordine &egrave; stato elaborato ed &egrave; pronto per l'evasione." & vbCrLf & "<br>"
                                msg.Body &= "L'evasione dell'ordine dai Ns. magazzini è prevista dal giorno " & DataEvasione.ToString("dd/MM/yyyy") & "." & vbCrLf & "<br>"
                            End If
                            msg.Body &= "Cordiali saluti," & vbCrLf & "<br>"
                            msg.Body &= "Staff Dr.Giorgini" & vbCrLf & "<br>"
                            msg.Body &= EmailFooter
                        Else
                            msg.Subject = "Nuovo ordine web nr. " & orderNo & " del " & orderHeader("OrderDate") & " - ATTESA CERTIFICAZIONE CLIENTE"
                            msg.Body = "Buongiorno, " & vbCrLf & "<br>"
                            msg.Body &= "&egrave; stato inserito nel sistema un nuovo ordine con cliente in attesa di CERTIFICAZIONE." & vbCrLf & "<br>"
                            msg.Body &= "------------------------------" & vbCrLf & "<br>"
                            msg.Body &= "ORDINE NR: " & orderNo & " del " & orderHeader("OrderDate") & vbCrLf & "<br>"
                            msg.Body &= "CLIENTE: " & CustomerNo & " - " & customerName & vbCrLf & "<br>"
                            msg.Body &= "OPERATORE: " & CType(Session("user"), user).nomeCompleto & vbCrLf & "<br>"
                            msg.Body &= "------------------------------" & vbCrLf & "<br>"
                            msg.Body &= EmailFooter
                        End If
                        Try
                            client.Send(msg)
                            Log.Info("Email " & msg.Subject & " sent to " & msg.To.ToString & " " & msg.CC.ToString)
                        Catch ex As Exception
                            Log.Error(ex.ToString)
                            Exit Sub
                        End Try

                        If attivaNotificaOrdineAlCliente = "1" AndAlso listaEmailCliente.Count > 0 AndAlso Not CustomerNo.StartsWith("N") Then 'per nuovo cliente non viene inviata la notifica al cliente N
                            msg.To.Clear()
                            msg.CC.Clear()
                            For Each email As String In listaEmailCliente
                                msg.To.Add(email)
                            Next
                            'If CType(Session("user"), user).email <> "" Then
                            '    msg.Bcc.Add(CType(Session("user"), user).email)
                            'End If
                            Try
                                client.Send(msg)
                                Log.Info("Email " & msg.Subject & " sent to " & msg.To.ToString & " " & msg.CC.ToString)
                            Catch ex As Exception
                                Log.Error(ex.ToString)
                                Exit Sub
                            End Try
                        End If
                    End If
                End If

            End If
            dm = Nothing
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub savePDFReport(ByVal orderNo As String, Optional ByVal UseTelerik As Boolean = True)
        Dim dm As New datamanager
        Dim percorsoPDF As String = dm.GetParametroSito(parametriSitoValue.percorsoPDF)
        If percorsoPDF <> "" AndAlso Directory.Exists(Server.MapPath("~/" & percorsoPDF)) Then
            'If Not UseTelerik Then
            '    Dim Report As New repOrdine
            '    Report.DataSource = GetData(orderNo)
            '    Report.Name = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).prefixOrderFileName & "ORD" & orderNo
            '    Try
            '        Report.ExportToPdf(MapPath("~/" & percorsoPDF & "/" & Report.Name & ".pdf"))
            '        Log.Info("Order PDF was created in " & MapPath("~/" & percorsoPDF & "/" & Report.Name & ".pdf"))
            '    Catch ex As Exception
            '        Log.Error(ex.ToString)
            '        Exit Sub
            '    End Try
            'Else
            Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
                Dim deviceInfo As New System.Collections.Hashtable()
                Dim rptSource As New Telerik.Reporting.TypeReportSource()
                rptSource.TypeName = GetType(servisWO.Report1).AssemblyQualifiedName
                rptSource.Parameters.Add("orderCode", orderNo)
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
                Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", rptSource, deviceInfo)

                Dim fileName As String = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).prefixOrderFileName & "ORD" & orderNo
                Dim filePath As String = MapPath("~/" & percorsoPDF & "/" & fileName & ".pdf")

                Try
                    Using fs As New System.IO.FileStream(filePath, System.IO.FileMode.Create)
                        fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
                    End Using
                    Log.Info("Order PDF was created in " & filePath)
                Catch ex As Exception
                    Log.Error(ex.ToString)
                    Exit Sub
                End Try

                'End If
                Else
            'percorso PDF non impostato
            Log.Error("PDF save path Not set")
        End If
        dm = Nothing
    End Sub

    Private Function createOrderReportForEmail(ByVal orderNo As String, Optional ByVal UseTelerik As Boolean = True) As Attachment
        Try
            'If Not UseTelerik Then
            '    Dim Report As New repOrdine
            '    Report.DataSource = GetData(orderNo)
            '    Report.Name = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).prefixOrderFileName & "ORD" & orderNo
            '    Dim Mem As New MemoryStream()
            '    Report.ExportToPdf(Mem)
            '    Mem.Seek(0, System.IO.SeekOrigin.Begin)
            '    Dim Att = New Attachment(Mem, Report.Name & ".pdf", "application/pdf")
            '    Return Att
            'Else
            Dim dm As New datamanager
                Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
                Dim deviceInfo As New System.Collections.Hashtable()
                Dim rptSource As New Telerik.Reporting.TypeReportSource()
                rptSource.TypeName = GetType(servisWO.Report1).AssemblyQualifiedName
                rptSource.Parameters.Add("orderCode", orderNo)
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
                Dim result As Telerik.Reporting.Processing.RenderingResult = reportProcessor.RenderReport("PDF", rptSource, deviceInfo)

                Dim fileName As String = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).prefixOrderFileName & "ORD" & orderNo & ".pdf"

                Dim Mem As New MemoryStream()
                Mem.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
                Mem.Flush()

                Mem.Seek(0, System.IO.SeekOrigin.Begin)
                Dim Att = New Attachment(Mem, fileName, "application/pdf")
                Return Att


            'End If
        Catch ex As Exception
            Log.Error(ex.ToString)
            Return Nothing
        End Try
    End Function

    Private Function GetData(ByVal orderNo As String) As orderHeaderReportCollection
        Dim dm As New datamanager
        Dim drm As New dataReportManager
        drm.NAVconnectionString = dm.GetNAVconnectionString
        drm.WORconnectionString = dm.GetWORconnectionString
        drm.workingCompany = dm.GetWorkingCompany
        Return drm.getOrder(orderNo)
        drm = Nothing
        dm = Nothing
    End Function

    Protected Sub ASPxCallbackPanel_footer_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles ASPxCallbackPanel_footer.Callback
        Call loadCarrelloFooter()
    End Sub

    Protected Sub ASPxCallback_saveOrderHeaderFooter_Callback(source As Object, e As CallbackEventArgs) Handles ASPxCallback_saveOrderHeaderFooter.Callback
        If e.Parameter.Split("|")(0) = "pagamenti" Then
            Call savePagamenti()
        ElseIf e.Parameter.Split("|")(0) = "trasporto" Then
            Call saveSpeseTrasporto()
        ElseIf e.Parameter.Split("|")(0) = "agente" Then
            Call saveAgente(e.Parameter.Split("|")(1))
        End If
    End Sub

    Private Sub savePagamenti()
        Dim dm As New datamanager
        Dim pagamentoSelezionatoList As List(Of Object) = grigliaPagamenti.GetSelectedFieldValues(New String() {"Metodo", "Termine", "Sconto"})
        Dim PaymentMethodCode As String = ""
        Dim PaymentTermsCode As String = ""
        Dim scontoPagamentoPerc As Double = 0
        If pagamentoSelezionatoList.Count > 0 Then
            PaymentMethodCode = pagamentoSelezionatoList(0)(0).ToString()
            PaymentTermsCode = pagamentoSelezionatoList(0)(1).ToString()
            scontoPagamentoPerc = pagamentoSelezionatoList(0)(2)
            CType(Session("cart"), cartManager).Header.ordineHeader.PaymentMethodCode = PaymentMethodCode
            CType(Session("cart"), cartManager).Header.ordineHeader.PaymentTermsCode = PaymentTermsCode
            CType(Session("cart"), cartManager).Header.SCONTOPAGAMENTO = scontoPagamentoPerc
            CType(Session("cart"), cartManager).Header.UTENTE_ULTIMA_MODIFICA = CType(Session("user"), user).userCode
            dm.salvaCarrello2(CType(Session("cart"), cartManager), True, True)
        End If
        dm = Nothing
    End Sub

    Private Sub saveSpeseTrasporto()
        Dim dm As New datamanager
        If cb_speseTrasporto.Checked = True And Not CType(Session("cart"), cartManager).isOrdineInPortoFranco Then
            CType(Session("cart"), cartManager).Header.ordineHeader.IncludeShipCost = 1
        Else
            CType(Session("cart"), cartManager).Header.ordineHeader.IncludeShipCost = 0
        End If
        CType(Session("cart"), cartManager).Header.UTENTE_ULTIMA_MODIFICA = CType(Session("user"), user).userCode
        dm.salvaCarrello2(CType(Session("cart"), cartManager), True, True)
        dm = Nothing
    End Sub

    Private Sub saveAgente(ByVal agenteCode As String)
        Dim dm As New datamanager
        Dim agenteCodeInt As Integer
        If IsNumeric(agenteCode) AndAlso Integer.TryParse(agenteCode, agenteCodeInt) AndAlso agenteCodeInt > 0 Then
            CType(Session("cart"), cartManager).Header.AGENTE = agenteCodeInt
        Else
            CType(Session("cart"), cartManager).Header.AGENTE = 0
        End If
        CType(Session("cart"), cartManager).Header.UTENTE_ULTIMA_MODIFICA = CType(Session("user"), user).userCode
        dm.salvaCarrello2(CType(Session("cart"), cartManager), False, True)
        dm = Nothing
    End Sub

    Protected Sub gridOrderLine_AfterPerformCallback(sender As Object, e As DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs) Handles gridOrderLine.AfterPerformCallback
        Call loadCarrelloFooter()
    End Sub

    Protected Sub comboAgente_Callback(sender As Object, e As CallbackEventArgsBase) Handles comboAgente.Callback
        If Not e.Parameter Is Nothing Then
            Call saveOrderType(e.Parameter)
            If e.Parameter = "Visita Agente" Then
                Call loadAgenti()
                comboAgente.SelectedIndex = -1
            Else
                comboAgente.Items.Clear()
                comboAgente.SelectedIndex = -1
                Call saveAgente(0)
            End If
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

    Private Sub saveOrderType(ByVal ordineHeaderType As String)
        Dim dm As New datamanager
        CType(Session("cart"), cartManager).Header.ordineHeader.Type = ordineHeaderType
        CType(Session("cart"), cartManager).Header.UTENTE_ULTIMA_MODIFICA = CType(Session("user"), user).userCode
        dm.salvaCarrello2(CType(Session("cart"), cartManager), False, True)
        dm = Nothing
    End Sub

    Protected Sub popupProduzione_WindowCallback(source As Object, e As PopupWindowCallbackArgs) Handles popupProduzione.WindowCallback
        Dim dm As New datamanager
        Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
        Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
        Dim emailRichiestaProduzione As String = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione)
        Dim emailRichiestaProduzione2 As String = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione2)
        Dim emailRichiestaProduzione3 As String = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione3)
        Dim orderCode As String = CType(Session("cart"), cartManager).Header.ordineHeader.Code

        If attivaNotificheEmail = "1" AndAlso emailSistema <> "" AndAlso emailRichiestaProduzione <> "" Then
            tb_A.Text = emailRichiestaProduzione
            If emailRichiestaProduzione2 <> "" Then
                tb_Cc.Text = emailRichiestaProduzione2
            End If
            If emailRichiestaProduzione3 <> "" Then
                tb_Cc2.Text = emailRichiestaProduzione3
            End If

            'Dim orderHeader As DataRow = dm.GetOrderHeader(e.Parameter).Tables(0).Rows(0)
            Dim customerName As String
            If CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N") Then
                customerName = dm.GetNewCustomerName(CType(Session("cart"), cartManager).Header.CODICECLIENTE)
            Else
                customerName = dm.GetCustomerName(CType(Session("cart"), cartManager).Header.CODICECLIENTE)
            End If

            tb_Oggetto.Text = "Richiesta produzione articoli ordine nr. " & orderCode & " del " & CType(Session("cart"), cartManager).Header.ordineHeader.OrderDate & " per " & customerName

            Dim corpo As String = ""
            corpo &= "Richiesta di produzione dei seguenti articoli e quantità:" & vbCrLf & vbCrLf

            Dim articolidaprodurre As Boolean = False
            For Each line As carrelloLine In CType(Session("cart"), cartManager).carrelloLines
                If line.ordineLine.LotNo = "" Then
                    articolidaprodurre = True
                    corpo &= line.ordineLine.ItemCode & " " & line.DESCRIZIONE & ":  " & CInt(line.ordineLine.OriginalQty) & vbCrLf
                End If
            Next
            If Not articolidaprodurre Then
                corpo &= "Nessun articolo da produrre" & vbCrLf
            End If

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
            btnInvioRichiestaProduzione.Enabled = False
        End If

        dm = Nothing
    End Sub

    Protected Sub callbackInviaEmailProduzione_Callback(source As Object, e As CallbackEventArgs) Handles callbackInviaEmailProduzione.Callback
        Dim dm As New datamanager
        Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
        Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
        Dim emailSistemaDesc As String = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)
        If emailSistemaDesc = "" Then emailSistemaDesc = emailSistema
        If attivaNotificheEmail = "1" AndAlso emailSistema <> "" Then
            Dim orderNo As String = CType(Session("cart"), cartManager).Header.ordineHeader.Code
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
                e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlinvioemail", "Esito positivo", "Email inviata correttamente.", "")
                Call closeCurrentWork("produzione")
            Catch ex As Exception
                e.Result = String.Format("{0}|{1}|{2}|{3}", "ctrlinvioemail", "Esito negativo", "Si è verificato un problema nell'invio dell'email.", "")
                Log.Error(ex.ToString)
            End Try

        End If
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

    Protected Sub callbackScontoHeader_Callback(source As Object, e As CallbackEventArgs) Handles callbackScontoHeader.Callback
        '20200424 - Gestione scontoScaglioni
        'If IsNumeric(tbScontoHeader.Text) AndAlso CInt(tbScontoHeader.Text) >= 0 Then
        Dim scontoH As String = tbScontoHeader.Text.Trim()
        If (scontoH = "") Then
            scontoH = "0"
        End If
        CType(Session("cart"), cartManager).applicaScontoHeader(scontoH)
        'End If
    End Sub

    Protected Sub callbackScontoPagamento_Callback(source As Object, e As CallbackEventArgs) Handles callbackScontoPagamento.Callback
        CType(Session("cart"), cartManager).applicaScontoPagamento()
    End Sub

    Protected Sub gridPromoHeader_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles gridPromoHeader.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        'If e.CellType = GridViewTableCommandCellType.Data Then
        '    Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        '    Dim ID As Integer = grid.GetRowValues(e.VisibleIndex, "ID")
        '    Dim TOTAL_ORDER_MIN As Double = grid.GetRowValues(e.VisibleIndex, "TOTAL_ORDER_MIN")
        '    Dim TOTAL_ORDER_MAX As Double = grid.GetRowValues(e.VisibleIndex, "TOTAL_ORDER_MAX")
        '    Dim totaleordine As Double = CType(Session("cart"), cartManager).GetTotaleMerceConScontoPagamento
        '    If e.ButtonID = "btnApplica" Then
        '        If (totaleordine >= TOTAL_ORDER_MIN And totaleordine <= TOTAL_ORDER_MAX) Or CType(Session("cart"), cartManager).isInPromoHeader(ID) Then
        '            If CType(Session("cart"), cartManager).isInPromoHeader(ID) Then
        '                e.Enabled = False
        '                e.Image.ToolTip = "Promozione applicata"
        '            Else
        '                e.Enabled = True
        '                e.Image.ToolTip = "Applica promozione"
        '            End If
        '        Else
        '            e.Enabled = False
        '            e.Image.ToolTip = "Promo non applicabile"
        '        End If
        '    End If
        '    If e.ButtonID = "btnRimuovi" Then
        '        If totaleordine >= TOTAL_ORDER_MIN And totaleordine <= TOTAL_ORDER_MAX Or CType(Session("cart"), cartManager).isInPromoHeader(ID) Then
        '            If CType(Session("cart"), cartManager).isInPromoHeader(ID) Then
        '                e.Enabled = True
        '                e.Image.ToolTip = "Rimuovi promozione"
        '            Else
        '                e.Enabled = False
        '                e.Image.ToolTip = ""
        '            End If
        '        Else
        '            e.Enabled = False
        '            e.Image.ToolTip = ""
        '        End If
        '    End If
        'End If
    End Sub

    Protected Sub gridPromoHeader_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles gridPromoHeader.CustomButtonCallback
        If e.VisibleIndex = -1 Then
            Return
        End If
        'Dim dm As New datamanager
        'Dim CT_CODE As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CT_CODE").ToString
        'Dim ID As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "ID").ToString
        'Dim PROMO_CODE As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "PROMO_CODE").ToString
        'Dim TOTAL_ORDER_MIN As Double = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "TOTAL_ORDER_MIN")
        'Dim TOTAL_ORDER_MAX As Double = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "TOTAL_ORDER_MAX")
        'Dim DISCOUNT_PERCENT As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "DISCOUNT_PERCENT")
        'Dim PROMO_DATA_INIZIO As DateTime = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "PROMO_DATA_INIZIO")
        'Dim PROMO_DATA_FINE As DateTime = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "PROMO_DATA_FINE")
        'If e.ButtonID = "btnApplica" Then
        '    CType(Session("cart"), cartManager).applicaScontoHeader(DISCOUNT_PERCENT, ID, "ADD")
        '    CType(sender, ASPxGridView).JSProperties("cpScontoHeader") = DISCOUNT_PERCENT
        '    CType(sender, ASPxGridView).JSProperties("cpbtnApplicaScontoHeader") = 0
        'ElseIf e.ButtonID = "btnRimuovi" Then
        '    CType(Session("cart"), cartManager).rimuoviScontoHeader(ID, "DEL")
        '    CType(sender, ASPxGridView).JSProperties("cpScontoHeader") = 0
        '    CType(sender, ASPxGridView).JSProperties("cpbtnApplicaScontoHeader") = 1
        'End If
        'CType(sender, ASPxGridView).JSProperties("cpRefreshAfterButtonClick") = 1
        'dm = Nothing
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
                CType(Session("cart"), cartManager).checkPromoHeader(True)
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

    Private Sub loadArticoliMancanti(ByVal OrderCode As String)
        Dim dm As New datamanager
        Dim dt As DataTable = dm.GetArticoliMancanti(OrderCode).Tables(0)
        If dt.Rows.Count > 0 Then
            tabMancanti.Visible = True
            gridMancanti.DataSource = dt
            gridMancanti.DataBind()
        Else
            tabMancanti.Visible = False
        End If
        dm = Nothing
    End Sub

    Protected Sub callbackSalvaNote_Callback(source As Object, e As CallbackEventArgs) Handles callbackSalvaNote.Callback
        If Not e.Parameter Is Nothing Then
            CType(Session("cart"), cartManager).updateNote(e.Parameter, CType(Session("user"), user).userCode)
            Log.Info(CType(Session("user"), user).nomeCompleto & " update notes on Cart " & CType(Session("cart"), cartManager).Header.IDCART & " (Order " & CType(Session("cart"), cartManager).Header.ordineHeader.Code & "): " & e.Parameter)
        End If
    End Sub

    Protected Sub callbackVoucher_Callback(source As Object, e As CallbackEventArgs) Handles callbackVoucher.Callback
        If IsNumeric(tbValoreVoucher.Text) AndAlso CInt(tbValoreVoucher.Text) >= 0 Then
            Dim valoreVoucher As Double = Double.Parse(tbValoreVoucher.Text.Replace(".", ","))
            CType(Session("cart"), cartManager).applicaVoucher(Math.Round(valoreVoucher, 2))
            Log.Info(CType(Session("user"), user).nomeCompleto & " update voucher on Cart " & CType(Session("cart"), cartManager).Header.IDCART & " (Order " & CType(Session("cart"), cartManager).Header.ordineHeader.Code & "): " & e.Parameter)
        End If
    End Sub

    Public Sub assegnaLottiaOrdinePrenotato()
        Try
            Dim dm As New datamanager
            If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 2 And CType(Session("cart"), cartManager).Header.ordineHeader.Status = 0 Then 'ordine evadibile 
                Dim carrello As cartManager = CType(Session("cart"), cartManager)
                Dim tempLines As List(Of carrelloLine) = New List(Of carrelloLine)

                For Each item As carrelloLine In carrello.carrelloLines
                    If item.ordineLine.LotNo = "" Then
                        tempLines.Add(item)
                    End If
                Next
                If tempLines.Count > 0 Then
                    For Each item In tempLines
                        Dim No As String = item.ordineLine.ItemCode
                        Dim qta As Integer = item.ordineLine.Quantity
                        Dim rowdiscount As Integer = item.ordineLine.RowDiscount
                        Dim idpromo As Integer = item.IDPROMO
                        Dim scontopercentuale As String = item.ordineLine.FormulaSconto
                        carrello.deleteLineByIdcartLine(item.IDCART, item.IDCARTLINE)
                        carrello.addLine(No, qta, "", False, False, rowdiscount, False, "", idpromo, scontopercentuale, True, False)
                    Next
                    dm.salvaCarrello2(carrello)
                    carrello.refreshDispo()
                    Session("cart") = carrello
                End If
            Else
                CType(Session("cart"), cartManager).refreshDispo()
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

End Class