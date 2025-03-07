Imports DevExpress.Web
Imports servisWO.datamanager
Imports System.Reflection
Imports log4net
Imports DevExpress.Web.Data

Public Class magPicking
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not CType(Session("carrelloMagazzino"), cartManager) Is Nothing AndAlso CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code <> "" Then
            If Not Page.IsPostBack Then
                If Not Session("currentLineIndex") Is Nothing AndAlso Session("currentLineIndex") >= 0 Then
                    Call caricaRiga(Session("currentLineIndex"))
                Else
                    Call caricaRiga(0)
                End If

            End If
        Else
            btn_RipristinaRiga.Enabled = False
            btn_Next.Enabled = False
            btn_Prev.Enabled = False
            btn_Skip.Enabled = False
            btn_FinePicking.Enabled = False
            tb_Collocazione.Enabled = False
            tb_Qta.Enabled = False
            tb_Lotto.Enabled = False
            img_Articolo.Visible = False
        End If
    End Sub

    Private Sub caricaRiga(ByVal lineIndex As Integer)
        Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
        If Not carrello Is Nothing AndAlso carrello.carrelloLines.Count > 0 Then
            Dim dm As New datamanager

            If lineIndex = 0 Then
                btn_Prev.Enabled = False
            Else
                btn_Prev.Enabled = True
            End If

            If lineIndex <= carrello.carrelloLines.Count - 1 Then
                Dim currentRow As carrelloLine = carrello.carrelloLines(lineIndex)
                ' Dim currentItem As DataRow = dm.GetSingleWOItem(currentRow.ordineLine.ItemCode)
                Dim imgSmallProdotto As String = dm.GetItemImageSmall(currentRow.ordineLine.ItemCode)
                lb_LineNo.Text = currentRow.ordineLine.LineNo
                lb_ItemCode.Text = currentRow.ordineLine.ItemCode
                lb_Descrizione.Text = currentRow.DESCRIZIONE
                'If Not IsDBNull(currentItem("imgSmallProdotto")) AndAlso currentItem("imgSmallProdotto") <> "" Then
                '    img_Articolo.ImageUrl = currentItem("imgSmallProdotto")
                'Else
                '    img_Articolo.Visible = False
                'End If
                If imgSmallProdotto <> "" Then
                    img_Articolo.ImageUrl = imgSmallProdotto
                Else
                    img_Articolo.Visible = False
                End If
                'If currentRow.ordineLine.RowDiscount = 0 Then
                '    lb_QtaAttesa.Text = currentRow.ordineLine.OriginalQty 'CInt(currentRow("OriginalQty"))
                'Else
                '    lb_QtaAttesa.Text = CInt(currentRow.ordineLine.OriginalQty)
                'End If
                lb_QtaAttesa.Text = CInt(currentRow.ordineLine.OriginalQty).ToString
                lb_LottoAtteso.Text = currentRow.ordineLine.LotNo
                lb_CollAttesa.Text = currentRow.ordineLine.BinCode
                'If currentRow.ordineLine.BinCode <> "" Then
                '    lb_CollAttesaDescription.Text = dm.GetBinCodeDescription(currentRow.ordineLine.BinCode, dm.GetParametroSito(parametriSitoValue.magazzinodefault))
                'End If
                tb_Lotto.Text = ""
                tb_Qta.Text = ""
                tb_Collocazione.Text = ""

                If currentRow.LOADED = 1 Then
                    tb_Lotto.Text = currentRow.ordineLine.LotNo
                    tb_Collocazione.Text = currentRow.ordineLine.BinCode
                    tb_Qta.Text = CInt(currentRow.ordineLine.QtyToShip)
                    btn_RipristinaRiga.Enabled = True
                Else
                    btn_RipristinaRiga.Enabled = False
                End If

                'controllo unloadlostquantity
                Dim lostQuantity As Integer = dm.getLostQuantityByRowLine(currentRow)
                hf_LostQuantity.Clear()
                hf_LostQuantity.Add("lostquantity", lostQuantity)
                If lostQuantity > 0 Then
                    btn_Rettifica.Enabled = True
                    If currentRow.LOADED = 1 Then
                        If tb_Qta.Text = "" Then
                            tb_Qta.Text = CInt(currentRow.ordineLine.Quantity)
                        End If
                        tb_Qta.ReadOnly = True
                        tb_Lotto.ReadOnly = True
                        tb_Collocazione.ReadOnly = True
                    Else
                        tb_Qta.Text = CInt(currentRow.ordineLine.OriginalQty - lostQuantity)
                        btn_RipristinaRiga.Enabled = True
                    End If
                Else
                    tb_Qta.ReadOnly = False
                    tb_Lotto.ReadOnly = False
                    tb_Collocazione.ReadOnly = False
                    btn_Rettifica.Enabled = False
                End If

                hf_lottiammessi.Clear()
                hf_lottiammessi.Add(currentRow.ordineLine.LotNo, currentRow.ordineLine.LotNo)

                Dim dtCollocazioni As DataTable = dm.GetBinCodeListByItemcode(currentRow.ordineLine.ItemCode, dm.GetParametroSito(parametriSitoValue.magazzinodefault))
                hf_collocazioniammesse.Clear()
                For Each r As DataRow In dtCollocazioni.Rows
                    hf_collocazioniammesse.Add(r("Bin Code"), r("Bin Code"))
                Next

                'impedire ripristino rettifica e rettifica per righe generate da lotti successivi
                If currentRow.LINEIDSOURCE > 0 Then
                    btn_Rettifica.Enabled = False
                    btn_RipristinaRiga.Enabled = False
                    tb_Qta.ReadOnly = True
                    tb_Lotto.ReadOnly = True
                    tb_Collocazione.ReadOnly = True
                    lb_notaLottiSuccessivi.Visible = True
                    lb_notaLottiSuccessivi.Text = "Riga generata da rettifica lotto. Origine riga nr. " & (currentRow.LINEIDSOURCE * 10000).ToString & "."
                Else
                    If (currentRow.ordineLine.RowDiscount = 0 And currentRow.ordineLine.Quantity <= 0) Or (currentRow.ordineLine.RowDiscount = 1 And currentRow.ordineLine.DiscountQty <= 0) Then
                        btn_Rettifica.Enabled = True
                        btn_RipristinaRiga.Enabled = True
                        tb_Qta.ReadOnly = True
                        tb_Lotto.ReadOnly = True
                        tb_Collocazione.ReadOnly = True
                        lb_notaLottiSuccessivi.Visible = True
                        lb_notaLottiSuccessivi.Text = "Riga esaurita da rettifica."
                    Else
                        lb_notaLottiSuccessivi.Visible = False
                    End If
                End If
                tb_Qta.Focus()
                Session("currentLineIndex") = lineIndex
                Log.Info(CType(Session("user"), user).nomeCompleto & " order " & carrello.Header.ordineHeader.Code & " line " & currentRow.IDCARTLINE & " READY: " & currentRow.ordineLine.ItemCode & " Expected=" & currentRow.ordineLine.OriginalQty & " Picked=" & currentRow.ordineLine.QtyToShip & " FlagLoaded=" & currentRow.LOADED)
            Else
                ' fine righe
                Response.Redirect("magEndPicking.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
            dm = Nothing
        End If
        'End If

    End Sub

    Protected Sub btn_Next_Click(sender As Object, e As EventArgs) Handles btn_Next.Click
        If Not Session("currentLineIndex") Is Nothing Then
            Dim currentLineIndex As Integer = Session("currentLineIndex")
            Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)

            If currentLineIndex <= carrello.carrelloLines.Count - 1 Then
                Dim currentRow As carrelloLine = carrello.carrelloLines(currentLineIndex)
                If IsNumeric(tb_Qta.Text.Trim) AndAlso (CInt(tb_Qta.Text.Trim) > 0 Or currentRow.LOADED = 1) Then
                    Session("carrelloMagazzino") = carrello.aggiornaQuantitàDaMagazzino(currentRow, CInt(tb_Qta.Text.Trim))
                    Session("currentLineIndex") = currentLineIndex + 1
                    Log.Info(CType(Session("user"), user).nomeCompleto & " order " & carrello.Header.ordineHeader.Code & " line " & currentRow.IDCARTLINE & " UPDATED: " & currentRow.ordineLine.ItemCode & " Expected=" & currentRow.ordineLine.OriginalQty & " Picked=" & tb_Qta.Text.Trim & " FlagLoaded=" & currentRow.LOADED)
                    If currentLineIndex + 1 <= carrello.carrelloLines.Count - 1 Then
                        Call caricaRiga(currentLineIndex + 1)
                    Else
                        ' fine righe
                        Response.Redirect("magEndPicking.aspx", False)
                        Context.ApplicationInstance.CompleteRequest()
                    End If
                End If
            Else ' fine righe
                Response.Redirect("magEndPicking.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
    End Sub

    Protected Sub btn_Skip_Click(sender As Object, e As EventArgs) Handles btn_Skip.Click
        If Not Session("currentLineIndex") Is Nothing Then
            Dim currentLineIndex As Integer = Session("currentLineIndex")
            Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
            Dim currentRow As carrelloLine = carrello.carrelloLines(currentLineIndex)
            Log.Info(CType(Session("user"), user).nomeCompleto & " order " & carrello.Header.ordineHeader.Code & " line " & currentRow.IDCARTLINE & " skipped " & currentRow.ordineLine.ItemCode)
            If currentLineIndex + 1 <= carrello.carrelloLines.Count - 1 Then
                Session("currentLineIndex") = currentLineIndex + 1
                Call caricaRiga(currentLineIndex + 1)
            Else
                Response.Redirect("magEndPicking.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
    End Sub

    Protected Sub btn_Prev_Click(sender As Object, e As EventArgs) Handles btn_Prev.Click
        If Not Session("currentLineIndex") Is Nothing Then
            Dim currentLineIndex As Integer = Session("currentLineIndex")
            If currentLineIndex - 1 >= 0 Then
                Call caricaRiga(currentLineIndex - 1)
                Session("currentLineIndex") = currentLineIndex - 1
            End If
        End If
    End Sub

    Protected Sub ASPxPopupControl1_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles ASPxPopupControl1.WindowCallback
        Call loadRettificaPopUp()
    End Sub

    Private Sub loadRettificaPopUp()
        'verifico se esiste già rettifica su questa riga e carico i valori
        Dim dm As New datamanager
        Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
        Dim currentRow As carrelloLine = carrello.carrelloLines(Session("currentLineIndex"))

        Dim lostquantity As Integer = dm.getLostQuantityByRowLine(currentRow)
        If lostquantity > 0 Then
            Dim UnloadRow As DataRow = dm.getUnloadLostQuantity(currentRow)
            lb_qtaAttesaPopup.Text = lb_QtaAttesa.Text
            lb_qtaNonDisponibile.Text = UnloadRow("LostQuantity")
            lb_BinCodePopUp.Text = lb_CollAttesa.Text
            lb_lottoPopUp.Text = UnloadRow("LotNo")
            tb_BinCodeRettificaPopUp.Text = UnloadRow("BinCode")
            combo_Reason.SelectedIndex = combo_Reason.Items.IndexOfValue(UnloadRow("InventoryCause"))
        Else
            lb_qtaAttesaPopup.Text = lb_QtaAttesa.Text
            If tb_Qta.Text <> "" AndAlso IsNumeric(tb_Qta.Text) Then
                lb_qtaNonDisponibile.Text = CInt(lb_QtaAttesa.Text) - CInt(tb_Qta.Text)
            Else
                lb_qtaNonDisponibile.Text = CInt(lb_QtaAttesa.Text)
            End If
            lb_BinCodePopUp.Text = lb_CollAttesa.Text
            lb_lottoPopUp.Text = lb_LottoAtteso.Text
            tb_BinCodeRettificaPopUp.Text = ""
        End If

        Dim codiceArticolo As String = lb_ItemCode.Text
        Dim lottoAtteso As String = lb_lottoPopUp.Text
        Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(codiceArticolo)
        If dtLotti.Select("Lotto<>'" & lottoAtteso & "'").Count > 0  Then
            cb_lottoSuccessivo.ReadOnly = False
            cb_lottoSuccessivo.Checked = True
            gridlotti.Enabled = True
            Call loadLotti(codiceArticolo, lostquantity)
            'If (lostquantity > 0) Then
            '    cb_lottoSuccessivo.Checked = False
            '    cb_lottoSuccessivo.ReadOnly = True
            '    gridlotti.Enabled = False
            'End If
        Else
            cb_lottoSuccessivo.Checked = False
            cb_lottoSuccessivo.ReadOnly = True
            gridlotti.Enabled = False
            gridlotti.Selection.UnselectAll()
        End If
        dm = Nothing
    End Sub

    Protected Sub btn_ConfermaRettifica_Click(sender As Object, e As EventArgs) Handles btn_ConfermaRettifica.Click
        Dim dm As New datamanager
        ASPxPopupControl1.ShowOnPageLoad = False
        Dim currentLineIndex As Integer = Session("currentLineIndex")
        Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
        Dim currentRow As carrelloLine = carrello.carrelloLines(currentLineIndex)
        Dim qtaAttesa As Integer = CInt(lb_QtaAttesa.Text)
        Dim qtaCaricata As Integer = CInt(tb_Qta.Text)
        Dim qtaLost As Integer = qtaAttesa - qtaCaricata
        Log.Info(CType(Session("user"), user).nomeCompleto & " order " & carrello.Header.ordineHeader.Code & " line " & currentRow.IDCARTLINE & " Started rettifica: " & currentRow.ordineLine.ItemCode & " Expected=" & qtaAttesa & " Picked=" & qtaCaricata & " Lost=" & qtaLost)

        If tb_BinCodeRettificaPopUp.Text.Trim <> "" AndAlso combo_Reason.SelectedIndex >= 0 Then
            Dim reasoncode As String = combo_Reason.SelectedItem.Value
            Dim bincoderettifica As String = tb_BinCodeRettificaPopUp.Text
            Dim ulq As New unloadlostquantity
            ulq.OrderNo = carrello.Header.ordineHeader.Code
            ulq.RowNo = currentRow.ordineLine.LineID
            ulq.ItemCode = currentRow.ordineLine.ItemCode
            ulq.LotNo = currentRow.ordineLine.LotNo
            ulq.BinCode = tb_BinCodeRettificaPopUp.Text.Trim
            ulq.LostQuantity = qtaLost
            ulq.InventoryCause = combo_Reason.SelectedItem.Value
            ulq.User = CType(Session("user"), user).userCode
            ulq.LocationCode = dm.GetParametroSito(parametriSitoValue.magazzinodefault)
            dm.addLostQuantity(ulq)

            hf_LostQuantity.Clear()
            hf_LostQuantity.Add("lostquantity", ulq.LostQuantity)
        End If

        If cb_lottoSuccessivo.Checked = True Then   'se lotto successivo devo gestire lo split delle righe
            Dim fieldValues As List(Of Object) = gridlotti.GetSelectedFieldValues(New String() {"Item No_", "Lotto"})
            Dim ctrl As Integer = 0
            Dim quantitaRigaCorrenteNew As Integer = currentRow.ordineLine.Quantity



            If currentRow.ordineLine.RowDiscount = 0 Then
                quantitaRigaCorrenteNew = currentRow.ordineLine.Quantity - CInt(qtaLost) 'scalo dalla riga corrente le quantità messe su lotti successivi
            Else
                quantitaRigaCorrenteNew = currentRow.ordineLine.DiscountQty - CInt(qtaLost) 'scalo dalla riga corrente le quantità messe su lotti successivi
            End If
            Session("carrelloMagazzino") = carrello.aggiornaQuantitàDaMagazzino(currentRow, quantitaRigaCorrenteNew)


            For Each item As Object() In fieldValues
                Dim codiceArticolo As String = item(0)
                Dim LottoKey As String = item(1)
                Dim LottoCtrl As ASPxTextBox = CType(gridlotti.FindRowCellTemplateControlByKey(LottoKey, gridlotti.Columns("confermaLotto"), "tb_confermalottosuccessivo"), ASPxTextBox)
                Dim collocazioneCtrl As ASPxTextBox = CType(gridlotti.FindRowCellTemplateControlByKey(LottoKey, gridlotti.Columns("confermaCollocazione"), "tb_confermacollocazionesuccessiva"), ASPxTextBox)
                Dim qtaCtrl As ASPxTextBox = CType(gridlotti.FindRowCellTemplateControlByKey(LottoKey, gridlotti.Columns("confermaQuantita"), "tbqtalotto"), ASPxTextBox)

                Dim Lotto As String = LottoCtrl.Text
                Dim Collocazione As String = collocazioneCtrl.Text.Trim
                Dim newQuantita As String = qtaCtrl.Text

                If Lotto <> "" And Collocazione <> "" And newQuantita <> "" Then
                    If dm.GetDisponibilitaProdottiPerLotto(codiceArticolo).Select("Lotto='" & Lotto & "'").Count > 0 Then 'se il lotto esiste
                        If dm.GetBinCodeListByItemcode(codiceArticolo, dm.GetParametroSito(parametriSitoValue.magazzinodefault)).Rows.Count > 0 Then 'se la colocazione esiste
                            If dm.GetBinCodeListByItemcode(codiceArticolo, dm.GetParametroSito(parametriSitoValue.magazzinodefault)).Select("[Bin Code]='" & Collocazione & "'").Count > 0 Then
                                If IsNumeric(newQuantita) Then
                                    'If currentRow.ordineLine.RowDiscount = 0 Then
                                    '    quantitaRigaCorrenteNew = currentRow.ordineLine.Quantity - CInt(qtaLost) 'scalo dalla riga corrente le quantità messe su lotti successivi
                                    'Else
                                    '    quantitaRigaCorrenteNew = currentRow.ordineLine.DiscountQty - CInt(qtaLost) 'scalo dalla riga corrente le quantità messe su lotti successivi
                                    'End If
                                    ctrl = ctrl + carrello.rettificaRigaSuLottoSuccessivo(currentRow, CInt(newQuantita), Lotto, Collocazione)
                                End If
                            End If
                        End If
                    End If
                End If
            Next item

            'Session("carrelloMagazzino") = carrello.aggiornaQuantitàDaMagazzino(currentRow, quantitaRigaCorrenteNew)
            If quantitaRigaCorrenteNew <= 0 Then 'elimino riga corrente da NAV
                Session("carrelloMagazzino") = carrello.eliminaRigaNAV_DoporettificaRiga(currentRow)
            End If

            If currentLineIndex + 1 <= carrello.carrelloLines.Count - 1 Then
                Dim nextRow As carrelloLine = carrello.carrelloLines(currentLineIndex + 1)
                If nextRow.LOADED = 1 And nextRow.LINEIDSOURCE > 0 Then ' generata da rettifica, sono in coda all'ordine
                    Response.Redirect("magEndPicking.aspx", False)
                    Context.ApplicationInstance.CompleteRequest()
                Else
                    Session("currentLineIndex") = currentLineIndex + 1
                    Call caricaRiga(currentLineIndex + 1)
                End If
            Else
                ' fine righe
                Response.Redirect("magEndPicking.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If

        Else  ' se non viene usato il lotto successivo devo gestire solamente l'update della riga esistente per modificare QuantityToShip

            Session("carrelloMagazzino") = carrello.aggiornaQuantitàDaMagazzino(currentRow, qtaCaricata)

            If qtaCaricata <= 0 Then 'elimino riga corrente da NAV
                Session("carrelloMagazzino") = carrello.eliminaRigaNAV_DoporettificaRiga(currentRow)
            End If

            If currentLineIndex + 1 <= carrello.carrelloLines.Count - 1 Then
                Dim nextRow As carrelloLine = carrello.carrelloLines(currentLineIndex + 1)
                If nextRow.LOADED = 1 And nextRow.LINEIDSOURCE > 0 Then ' generata da rettifica, sono in coda all'ordine
                    Response.Redirect("magEndPicking.aspx", False)
                    Context.ApplicationInstance.CompleteRequest()
                Else
                    Session("currentLineIndex") = currentLineIndex + 1
                    Call caricaRiga(currentLineIndex + 1)
                End If
            Else
                ' fine righe
                Response.Redirect("magEndPicking.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If

        End If

        Log.Info("Order " & carrello.Header.ordineHeader.Code & " line " & currentRow.IDCARTLINE & " rettifica finished ")

        dm = Nothing
    End Sub

    Protected Sub btn_RipristinaRiga_Click(sender As Object, e As EventArgs) Handles btn_RipristinaRiga.Click
        Dim dm As New datamanager
        Dim currentLineIndex As Integer = Session("currentLineIndex")
        Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
        Dim currentRow As carrelloLine = carrello.carrelloLines(currentLineIndex)
        Log.Info(CType(Session("user"), user).nomeCompleto & " order " & carrello.Header.ordineHeader.Code & " started restore rettifica on line " & currentRow.IDCARTLINE & " " & currentRow.ordineLine.ItemCode)
        Session("carrelloMagazzino") = carrello.restoreRettifica(currentRow)
        Call caricaRiga(currentLineIndex)
        dm = Nothing
    End Sub

    Protected Sub btn_FinePicking_Click(sender As Object, e As EventArgs) Handles btn_FinePicking.Click
        Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
        Log.Info(CType(Session("user"), user).nomeCompleto & " order " & carrello.Header.ordineHeader.Code & " clicked on Fine Picking")
        Response.Redirect("magEndPicking.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Private Sub loadLotti(ByVal codicearticolo As String, ByVal lostquantity As Integer)
        Dim dm As New datamanager
        Dim currentLineIndex As Integer = Session("currentLineIndex")
        Dim carrello As cartManager = CType(Session("carrelloMagazzino"), cartManager)
        Dim currentRow As carrelloLine = carrello.carrelloLines(currentLineIndex)
        'controllare se esistono lotti successivi per la riga corrente
        Dim lineeCollegate As List(Of carrelloLine) = carrello.getWarehouseLinesCollegate(currentRow)
        If lineeCollegate.Count > 0 Then
            cb_lottoSuccessivo.ReadOnly = True
            cb_lottoSuccessivo.Checked = True
            gridlotti.Enabled = False
            gridlotti.Visible = False
            btn_ConfermaRettifica.Enabled = False
            btn_CloseRettifica.Text = "Chiudi"
            combo_Reason.Enabled = False
            tb_BinCodeRettificaPopUp.Enabled = False
            gridLottiSuccessivi.Enabled = False
            gridLottiSuccessivi.DataSource = lineeCollegate
            gridLottiSuccessivi.DataBind()
            gridLottiSuccessivi.Selection.SelectAll()
        Else
            gridLottiSuccessivi.Visible = False
            Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(codicearticolo)
            Dim dtLottoFilter As DataTable = dtLotti.Clone
            For Each dr As DataRow In dtLotti.Rows
                If dr("Lotto") <> lb_LottoAtteso.Text Then
                    dtLottoFilter.ImportRow(dr)
                End If
            Next
            gridlotti.DataSource = dtLottoFilter
            gridlotti.DataBind()
            gridlotti.Selection.UnselectAll()
            If (lostquantity > 0) Then
                cb_lottoSuccessivo.Checked = False
                cb_lottoSuccessivo.ReadOnly = True
                gridlotti.Enabled = False
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub gridlotti_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles gridlotti.CustomColumnDisplayText
        If e.Column.Name = "ScadenzaLotto" Then
            If e.Value = "1900-01-01" Then
                e.DisplayText = " nessuna "
            Else
                e.DisplayText = String.Format("{0:dd-MM-yyyy}", e.Value)
            End If
        End If
    End Sub

    Protected Sub gridlotti_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridViewTableRowEventArgs) Handles gridlotti.HtmlRowPrepared
        If e.RowType <> GridViewRowType.Data Then
            Return
        End If
        Dim dm As New datamanager
        Dim ScadenzaLotto As DateTime = e.GetValue("ScadenzaLotto")
        Dim codiceArticolo As String = e.GetValue("Item No_")
        Dim lotto As String = e.GetValue("Lotto")
        Dim scadenzaLottoOrigine As DateTime
        Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(codiceArticolo)
        If dtLotti.Select("Lotto='" & lb_LottoAtteso.Text & "'").Count > 0 Then
            Dim lottoOrigine As DataRow = dtLotti.Select("Lotto='" & lb_LottoAtteso.Text & "'")(0)
            scadenzaLottoOrigine = lottoOrigine("ScadenzaLotto")
            If ScadenzaLotto < scadenzaLottoOrigine AndAlso ScadenzaLotto > New Date(1900, 1, 1) Then
                e.Row.BackColor = System.Drawing.Color.Red
                e.Row.ToolTip = "ATTENZIONE: lotto successivo ha data scadenza minore della data scadenza del lotto di origine. Prima di continuare richiedere conferma all'ufficio vendite."
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub tb_confermalottosuccessivo_Init(sender As Object, e As EventArgs)
        Dim confermalottosuccessivo As ASPxTextBox = CType(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(confermalottosuccessivo.NamingContainer, GridViewDataItemTemplateContainer)
        confermalottosuccessivo.ClientInstanceName = String.Format("confermalottosuccessivo_{0}", container.VisibleIndex)
    End Sub

    Protected Sub tb_confermacollocazionesuccessiva_Init(sender As Object, e As EventArgs)
        Dim confermacollocazionesuccessiva As ASPxTextBox = CType(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(confermacollocazionesuccessiva.NamingContainer, GridViewDataItemTemplateContainer)
        confermacollocazionesuccessiva.ClientInstanceName = String.Format("confermacollocazionesuccessiva_{0}", container.VisibleIndex)
    End Sub

    Protected Sub tbqtalotto_Init(sender As Object, e As EventArgs)
        Dim qtaLottoCtrl As ASPxTextBox = CType(sender, ASPxTextBox)
        Dim container As GridViewDataItemTemplateContainer = TryCast(qtaLottoCtrl.NamingContainer, GridViewDataItemTemplateContainer)
        Dim dispoLotto As Integer = DataBinder.Eval(container.DataItem, "DisponibilitaLotto")
        If dispoLotto > 0 Then
            qtaLottoCtrl.Enabled = True
            If tb_Qta.Text.Trim = "" Then tb_Qta.Text = "0"
            qtaLottoCtrl.MaskSettings.Mask = "<0.." & CInt(lb_QtaAttesa.Text.Trim) - CInt(tb_Qta.Text.Trim) & ">"
        Else
            qtaLottoCtrl.Enabled = False
            qtaLottoCtrl.Text = 0
        End If
        qtaLottoCtrl.ClientInstanceName = String.Format("qtaLottoCtrl_{0}", container.VisibleIndex)
        'qtaLottoCtrl.ClientSideEvents.Validation = "function(s,e){ onValidateQtaLottoCtrl(" & qtaLottoCtrl.ClientInstanceName & ",18,60) }"
        'qtaLottoCtrl.ClientSideEvents.Validation = "function(s,e){ onValidateQtaLottoCtrl(s,e); }"
    End Sub

    Protected Sub ASPxCallbackPanel_callBackGridlotti_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles ASPxCallbackPanel_callBackGridlotti.Callback
        If cb_lottoSuccessivo.Checked = True Then
            gridlotti.Enabled = True
        Else
            gridlotti.Enabled = False
            'gridlotti.Selection.UnselectAll()
        End If
        gridlotti.Selection.UnselectAll()
        gridLottiSuccessivi.Visible = False
    End Sub

    'Protected Sub gridlotti_RowValidating(sender As Object, e As ASPxDataValidationEventArgs) Handles gridlotti.RowValidating

    '    If CBool(e.NewValues("d_audit_failed")) Then
    '        If e.NewValues("d_audit_rpro") Is Nothing Then
    '            e.Errors(gridlotti.Columns("d_audit_rpro")) = "Value cannot be null."
    '        End If
    '        If e.NewValues("d_audit_actual") Is Nothing Then
    '            e.Errors(gridlotti.Columns("d_audit_actual")) = "Value cannot be null."
    '        End If
    '    End If
    'End Sub
End Class