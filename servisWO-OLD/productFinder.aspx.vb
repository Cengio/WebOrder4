Imports System.Reflection
Imports DevExpress.Utils
Imports DevExpress.Web
Imports log4net

Public Class productFinder
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
    Private SasStatus As Integer = 0

    Private Sub productFinder_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSordini Then    'utente amministratore
            Dim dm As New datamanager
            SasStatus = dm.GetSASstatus()
            dm = Nothing
        Else
            Try
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                Log.Error(ex)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        System.Diagnostics.Debug.WriteLine(Now() & ": START productFinder_Page_Load")
        Try
            If Not Page.IsPostBack Then
                ASPxGridView_Cart.Columns("zerobutton").Visible = CType(Session("user"), user).iSzeroprice
                If CType(Session("user"), user).iSzeroprice And Not CType(Session("cart"), cartManager).isOrdineChiuso Then
                    ASPxGridView_Cart.Columns("zerobutton").Visible = True
                    gridPromoLotti.Columns("zerobuttonLotto").Visible = True
                Else
                    ASPxGridView_Cart.Columns("zerobutton").Visible = False
                    gridPromoLotti.Columns("zerobuttonLotto").Visible = False
                End If
                ASPxGridView_Cart.DataBind()
                CType(Session("cart"), cartManager).checkPromoHeader()
            End If
            'Me.Page.Form.DefaultButton = btn_Pfinder.UniqueID
        Catch ex As Exception
            Log.Error(ex)
            Response.Redirect("~/err/err403.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End Try
        System.Diagnostics.Debug.WriteLine(Now() & ": END productFinder_Page_Load")
    End Sub

    Private Function isFilterActive() As Boolean
        Try
            Dim m As String = ""
            Dim l As String = ""
            Dim f As String = ""
            Dim s As String = ""
            Dim t As String = ""
            Dim filteractive As Boolean = False
            If Request.QueryString("m") <> "" Then
                filteractive = True
            End If
            If Request.QueryString("l") <> "" Then
                filteractive = True
            End If
            If Request.QueryString("f") <> "" Then
                filteractive = True
            End If
            If Request.QueryString("s") <> "" Then
                filteractive = True
            End If
            If Request.QueryString("t") <> "" Then
                filteractive = True
            End If
            Return filteractive
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub ASPxGridView_Cart_DataBinding(sender As Object, e As EventArgs) Handles ASPxGridView_Cart.DataBinding
        Try
            Dim m As String = ""
            Dim l As String = ""
            Dim f As String = ""
            Dim s As String = ""
            Dim t As String = ""
            Dim ee As String = ""
            Dim ea As String = ""
            Dim txt As String = "Filtro articoli per "
            Dim dm As New datamanager
            Dim filteractive As Boolean = False

            If Request.QueryString("m") <> "" Then
                m = Request.QueryString("m")
                txt &= "(macrocategoria): <b>" & dm.getDimensionDescription("macrolinea", m) & "</b>"
                filteractive = True
            End If
            If Request.QueryString("l") <> "" Then
                l = Request.QueryString("l")
                txt &= " (linea): <b>" & dm.getDimensionDescription("linea", l) & "</b>"
                filteractive = True
            End If
            If Request.QueryString("f") <> "" Then
                f = Request.QueryString("f")
                txt &= " (famiglia): <b>" & dm.getDimensionDescription("famiglia", f) & "</b>"
                filteractive = True
            End If
            If Request.QueryString("s") <> "" Then
                s = Request.QueryString("s")
                txt &= " (sottofamiglia): <b>" & dm.getDimensionDescription("sottofamiglia", s) & "</b>"
                filteractive = True
            End If
            If Request.QueryString("t") <> "" Then
                t = Request.QueryString("t")
                txt &= " (frase): <b>" & t & "</b>"
                filteractive = True
            End If
            If filteractive Then
                ASPxGridView_Cart.SettingsText.Title = txt
            End If
            If Request.QueryString("ea") = "1" Then 'escludi archeopatici
                ea = Request.QueryString("ea")
            End If
            If Request.QueryString("ee") = "1" Then 'escludi expo
                ee = Request.QueryString("ee")
            End If

            ASPxGridView_Cart.DataSource = dm.GetWOItems("", t, True, m, l, f, s, ea, True, ee, CType(Session("cart"), cartManager).Header.CODICELISTINO)
            'Session("finderText") = ""

            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub ASPxGridView_Cart_CustomButtonInitialize(sender As Object, e As ASPxGridViewCustomButtonEventArgs) Handles ASPxGridView_Cart.CustomButtonInitialize
        Try
            If e.VisibleIndex = -1 Then
                Return
            End If
            If e.CellType = GridViewTableCommandCellType.Data Then
                ' System.Diagnostics.Debug.WriteLine(Now() & ": ASPxGridView_Cart_CustomButtonInitialize " & e.VisibleIndex)
                Dim Disponibilita As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Disponibilita")
                Dim codmacrolinea As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "cod_macrolinea")
                Dim CodiceArticolo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "CodiceArticolo")

                If e.ButtonID = "addrowtocart" Then
                    'blocco se non c'è il cliente selezionato
                    If CType(Session("cart"), cartManager).Header.CODICECLIENTE = "" Or CType(Session("cart"), cartManager).Header.IDCART = 0 Then
                        e.Enabled = False
                        e.Visible = DefaultBoolean.True
                        e.Image.Url = "~/images/cartrow_off.png"
                        e.Image.ToolTip = "Selezionare un cliente ed iniziare un ordine prima di poter aggiungere articoli."
                    Else
                        If CType(Session("cart"), cartManager).isOrdineChiuso Then 'blocco inserimento articoli perchè ordine è bloccato
                            e.Enabled = False
                            e.Image.Url = "~/images/cartrow_off.png"
                            e.Image.ToolTip = "Ordine non modificabile."
                        Else
                            If Disponibilita <= 0 Then
                                e.Enabled = False
                                e.Visible = DefaultBoolean.False
                            ElseIf Disponibilita > 0 Then
                                e.Enabled = True
                                e.Visible = DefaultBoolean.True
                                e.Image.Url = "~/images/cartrow.png"
                                e.Image.ToolTip = "Aggiungi al carrello"
                            End If
                            'gestione archeopatici
                            If codmacrolinea = "0010" Then
                                e.Enabled = True
                                e.Visible = DefaultBoolean.True
                                e.Image.Url = "~/images/cartrow.png"
                                e.Image.ToolTip = "Aggiungi al carrello"
                            End If
                        End If
                    End If
                ElseIf e.ButtonID = "gestMancanti" Then
                    'gestione archeopatici
                    If codmacrolinea = "0010" Then
                        e.Visible = DefaultBoolean.False
                        Return
                    End If

                    If CType(Session("cart"), cartManager).Header.CODICECLIENTE = "" Then
                        e.Visible = DefaultBoolean.False
                    Else
                        If Disponibilita <= 0 Then
                            If CType(Session("cart"), cartManager).isOrdineChiuso Then 'blocco inserimento articoli perchè ordine è bloccato
                                e.Visible = DefaultBoolean.False
                            Else
                                e.Visible = DefaultBoolean.True
                                If CType(Session("cart"), cartManager).Header.ordineHeader.Code <> "" Then
                                    e.Enabled = True
                                ElseIf CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                                    e.Enabled = True
                                Else
                                    e.Enabled = False
                                    e.Image.Url = "~/images/cartrow_off.png"
                                    e.Image.ToolTip = "La gestione degli articoli mancanti non attiva. Nessun ordine selezionato."
                                End If
                            End If
                        Else
                            e.Visible = DefaultBoolean.False
                        End If
                    End If
                ElseIf e.ButtonID = "zeroPrice" Then
                    If CType(Session("cart"), cartManager).Header.CODICECLIENTE = "" Or CType(Session("cart"), cartManager).Header.IDCART = 0 Then
                        e.Enabled = False
                        e.Image.Url = "~/images/zeroPrice_off.png"
                        e.Image.ToolTip = "Selezionare un cliente ed iniziare un ordine prima di poter aggiungere articoli."
                    Else
                        If CType(Session("cart"), cartManager).isOrdineChiuso Then 'blocco inserimento articoli perchè ordine è bloccato
                            e.Enabled = False
                            e.Image.Url = "~/images/zeroPrice_off.png"
                            e.Image.ToolTip = "Ordine non modificabile."
                        Else
                            If Disponibilita <= 0 Then
                                e.Enabled = False
                                e.Image.Url = "~/images/zeroPrice_off.png"
                                e.Image.ToolTip = "Non disponibile"
                            ElseIf Disponibilita > 0 Then
                                e.Enabled = True
                                e.Image.Url = "~/images/zeroPrice.png"
                                e.Image.ToolTip = "Imposta Riga Omaggio - Prezzo Zero"
                            End If
                            'gestione archeopatici
                            If codmacrolinea = "0010" Then
                                e.Enabled = True
                                e.Image.Url = "~/images/zeroPrice.png"
                                e.Image.ToolTip = "Imposta Riga Omaggio - Prezzo Zero"
                            End If
                        End If
                    End If
                ElseIf e.ButtonID = "showLotti" Then
                    'gestione archeopatici
                    If codmacrolinea = "0010" Then
                        'e.Visible = DefaultBoolean.False
                    End If
                ElseIf e.ButtonID = "delFromCart" Then
                    If CType(Session("cart"), cartManager).getQtaItemInCart(CodiceArticolo) <= 0 Then
                        e.Enabled = False
                        e.Image.Url = "~/images/delCart_off.png"
                        e.Image.ToolTip = " "
                    End If
                End If

            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub ASPxGridView_Cart_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles ASPxGridView_Cart.CustomButtonCallback
        Try
            If e.VisibleIndex = -1 Then
                Return
            End If
            ' Dim dm As New datamanager
            Dim grid As ASPxGridView = CType(sender, ASPxGridView)
            Dim No As String = grid.GetRowValues(e.VisibleIndex, "CodiceArticolo").ToString
            Dim isArcheopatico As Boolean = (grid.GetRowValues(e.VisibleIndex, "cod_macrolinea").ToString = "0010")
            Dim idpromo As Long = 0
            Dim idcart As Integer = CType(Session("cart"), cartManager).Header.IDCART
            Dim qtacolumn As GridViewDataColumn = TryCast(grid.Columns("qta"), GridViewDataColumn)
            Dim scomercecolumn As GridViewDataColumn = TryCast(grid.Columns("scontomerce"), GridViewDataColumn)
            Dim scontopercentualecolumn As GridViewDataColumn = TryCast(grid.Columns("scontopercentuale"), GridViewDataColumn)
            Dim combopromocolumn As GridViewDataColumn = TryCast(grid.Columns("Promozione"), GridViewDataColumn)
            Dim qtaCtrl As ASPxTextBox = CType(grid.FindRowCellTemplateControl(e.VisibleIndex, qtacolumn, "tbqta"), ASPxTextBox)
            Dim qtaScoMerceCtrl As ASPxTextBox = CType(grid.FindRowCellTemplateControl(e.VisibleIndex, scomercecolumn, "tbscontomerce"), ASPxTextBox)
            Dim qtaScoPercentualeCtrl As ASPxTextBox = CType(grid.FindRowCellTemplateControl(e.VisibleIndex, scontopercentualecolumn, "tbscontopercentuale"), ASPxTextBox)
            Dim combopromo As ASPxComboBox = CType(grid.FindRowCellTemplateControl(e.VisibleIndex, combopromocolumn, "comboPromo"), ASPxComboBox)
            Dim qta As Integer = IIf(IsNumeric(qtaCtrl.Text), qtaCtrl.Text, 0)
            Dim qtascomerce As Integer = 0
            Dim scontopercentuale As String = ""
            If Not qtaScoMerceCtrl Is Nothing Then
                qtascomerce = IIf(IsNumeric(qtaScoMerceCtrl.Text), qtaScoMerceCtrl.Text, 0)
            End If
            If Not qtaScoPercentualeCtrl Is Nothing Then
                scontopercentuale = IIf(IsNumeric(qtaScoPercentualeCtrl.Text), qtaScoPercentualeCtrl.Text, "")
            End If
            If Not combopromo Is Nothing AndAlso Not combopromo.SelectedItem Is Nothing Then
                idpromo = combopromo.SelectedItem.GetValue("ID")
            End If

            If e.ButtonID = "addrowtocart" Then
                If IsNumeric(qta) AndAlso CInt(qta) > 0 Then
                    If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 1 Then
                        CType(Session("cart"), cartManager).addLinePrenotazione(No, qta, "", False, False, 0, False, "", idpromo, scontopercentuale)
                        If qtascomerce > 0 Then
                            CType(Session("cart"), cartManager).addLinePrenotazione(No, qtascomerce, "", False, False, 1, False, "", idpromo, "")
                        End If
                    Else
                        CType(Session("cart"), cartManager).addLine(No, qta, "", False, False, 0, False, "", idpromo, scontopercentuale)
                        If qtascomerce > 0 Then
                            CType(Session("cart"), cartManager).addLine(No, qtascomerce, "", False, False, 1, False, "", idpromo, "")
                        End If
                    End If
                End If
                qtaCtrl.Text = 0
                ASPxGridView_Cart.DataBind()
                CType(Session("cart"), cartManager).checkPromoHeader(True)
            ElseIf e.ButtonID = "zeroPrice" Then
                If IsNumeric(qta) AndAlso CInt(qta) > 0 Then
                    CType(Session("cart"), cartManager).addLine(No, qta, , , , 1, False, , , "")
                End If
                qtaCtrl.Text = 0
                ASPxGridView_Cart.DataBind()
                CType(Session("cart"), cartManager).checkPromoHeader(True)
            ElseIf e.ButtonID = "btnDettaglioArticolo" Then
                Session("codiceArticolodettagli") = No
                Session("ClientQueryString") = Page.ClientQueryString.ToString
                ASPxWebControl.RedirectOnCallback("~/productDetails.aspx")
            ElseIf e.ButtonID = "delFromCart" Then
                CType(Session("cart"), cartManager).deleteLine(No, "-1", 0, idpromo, "")
                ASPxGridView_Cart.DataBind()
                CType(Session("cart"), cartManager).checkPromoHeader(True)
            End If
            'dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub ASPxGridView_Cart_CustomColumnDisplayText(sender As Object, e As ASPxGridViewColumnDisplayTextEventArgs) Handles ASPxGridView_Cart.CustomColumnDisplayText
        Try
            If e.Column.Name = "Disponibilita" Then
                If Not Session("cart") Is Nothing Then
                    If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                        e.DisplayText = CInt(e.Value) - CType(Session("cart"), cartManager).getQtaItemInCart(e.GetFieldValue("CodiceArticolo"))
                    End If
                Else
                    Response.Redirect("~/err/err403.aspx", False)
                    Context.ApplicationInstance.CompleteRequest()
                End If
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub ASPxGridView_Cart_HeaderFilterFillItems(sender As Object, e As ASPxGridViewHeaderFilterEventArgs) Handles ASPxGridView_Cart.HeaderFilterFillItems
        Try
            If e.Column.FieldName = "Disponibilita" Then
                e.Values.Clear()
                e.AddShowAll()
                e.AddValue("Solo disponibili", String.Empty, "[Disponibilita] > 0")
                e.AddValue("Solo non disponibili", String.Empty, "[Disponibilita] = 0")
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub lb_qtaArtInCart_Init(sender As Object, e As EventArgs)
        Try
            Dim qtaInCartCtrl As ASPxLabel = CType(sender, ASPxLabel)
            Dim container As GridViewDataItemTemplateContainer = TryCast(qtaInCartCtrl.NamingContainer, GridViewDataItemTemplateContainer)
            Dim CodiceArticolo As String = DataBinder.Eval(container.DataItem, "CodiceArticolo")
            Dim dispoInCart As Integer = CType(Session("cart"), cartManager).getQtaItemInCart(CodiceArticolo)
            If dispoInCart > 0 Then
                qtaInCartCtrl.Text = dispoInCart.ToString
            Else
                qtaInCartCtrl.Text = ""
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub ASPxGridView_Cart_CustomJSProperties(sender As Object, e As ASPxGridViewClientJSPropertiesEventArgs) Handles ASPxGridView_Cart.CustomJSProperties
        Try
            Dim qtyDispoValues As New Dictionary(Of Integer, Integer)()
            For i = 0 To ASPxGridView_Cart.VisibleRowCount - 1
                qtyDispoValues.Add(i, ASPxGridView_Cart.GetRowValues(i, "Disponibilita"))
            Next
            e.Properties("cpQtyDispoValues") = qtyDispoValues

        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub ASPxCallback_Mancanti_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles ASPxCallback_Mancanti.Callback
        Try
            If Not e.Parameter Is Nothing Then
                Dim parametri() As String = e.Parameter.Split("|")
                If parametri.Count = 3 Then
                    Dim codiceArticolo As String = parametri(0)
                    Dim qtaMancante As Integer = CInt(parametri(1))
                    Dim avviso As Boolean = parametri(2)
                    Dim codOrdine As String = CType(Session("cart"), cartManager).Header.ordineHeader.Code
                    Dim codCliente As String = CType(Session("cart"), cartManager).Header.CODICECLIENTE
                    Dim idCarrello As Int64 = CType(Session("cart"), cartManager).Header.IDCART
                    Dim dm As New datamanager
                    Dim qnd As New quantitaNonDisponibile
                    qnd.itemCode = codiceArticolo
                    qnd.orderCode = codOrdine
                    qnd.customerNo = codCliente
                    qnd.quantity = qtaMancante
                    qnd.idCarrello = idCarrello
                    If avviso Then
                        qnd.alert = 1
                    End If
                    dm.addQuantitaNonDisponibile(qnd)
                    dm = Nothing
                End If
            End If
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub tbqta_Init(sender As Object, e As EventArgs)
        Try
            Dim qtaCtrl As ASPxTextBox = CType(sender, ASPxTextBox)
            qtaCtrl.Enabled = False
            If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Then
                Dim container As GridViewDataItemTemplateContainer = TryCast(qtaCtrl.NamingContainer, GridViewDataItemTemplateContainer)
                qtaCtrl.ClientInstanceName = String.Format("tbqta_{0}", container.VisibleIndex)
                qtaCtrl.ClientSideEvents.ValueChanged = String.Format("function(s, e) {{ OnQtaValueChanged(s, e, {0}); }}", container.VisibleIndex)

                Dim dispo As Integer = DataBinder.Eval(container.DataItem, "Disponibilita")
                Dim CodiceArticolo As String = DataBinder.Eval(container.DataItem, "CodiceArticolo")
                Dim dispoInCart As Integer = CType(Session("cart"), cartManager).getQtaItemInCart(CodiceArticolo)
                Dim codmacrolinea As String = DataBinder.Eval(container.DataItem, "cod_macrolinea")

                If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                    If dispo - dispoInCart > 0 Then
                        qtaCtrl.Enabled = True
                        qtaCtrl.Text = 0
                    Else
                        qtaCtrl.Enabled = False
                        qtaCtrl.Text = 0
                    End If
                Else
                    If dispo > 0 Then
                        qtaCtrl.Enabled = True
                        qtaCtrl.Text = 0
                    Else
                        qtaCtrl.Enabled = False
                        qtaCtrl.Text = 0
                    End If
                End If

                'gestione archeopatici
                If codmacrolinea = "0010" Then
                    qtaCtrl.Enabled = True
                    'qtaCtrl.MaskSettings.Mask = "<0..999>"
                    qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaValidation(s,e,{0},{1}); }}", container.VisibleIndex, 1)
                Else
                    qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaValidation(s,e,{0},{1}); }}", container.VisibleIndex, 0)
                End If

                'gestione ordini prenotazione
                If CType(Session("cart"), cartManager).Header.PRENOTAZIONE = 1 Then
                    qtaCtrl.Enabled = True
                    qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaValidation(s,e,{0},{1}); }}", container.VisibleIndex, 1)
                End If

            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbqta_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub comboPromo_Init(sender As Object, e As EventArgs)
        Try
            Dim comboPromo As ASPxComboBox = CType(sender, ASPxComboBox)
            Dim dm As New datamanager
            comboPromo.Enabled = False
            If SasStatus = 1 AndAlso CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Then
                comboPromo.Enabled = CType(Session("user"), user).iSpromorigacombo
                Dim container As GridViewDataItemTemplateContainer = CType(comboPromo.NamingContainer, GridViewDataItemTemplateContainer)
                Dim itemcode As String = DataBinder.Eval(container.DataItem, "CodiceArticolo")
                Dim ctcode As String = CType(Session("cart"), cartManager).Header.CODICECONTATTO
                Dim itemcodelot As String = "" 'DataBinder.Eval(container.DataItem, "Lotto")
                Dim pm As New promoManager
                Dim cd As intCanaleDirezione = pm.getCanaleDirezioneByOrderType(CType(Session("cart"), cartManager).Header.ordineHeader.Type)
                Dim prow As List(Of promoRow) = pm.getPromoRowsByCtCodeItemCode(ctcode, itemcode, itemcodelot, CType(Session("cart"), cartManager).Header.DATA_CREAZIONE, cd)
                If prow.Count > 0 Then
                    comboPromo.DataSource = prow
                    comboPromo.DataBind()
                    comboPromo.SelectedIndex = 0
                    comboPromo.ClientSideEvents.ButtonClick = String.Format("function(s, e) {{ OnComboPromoButtonClick(s, e, {0}); }}", container.VisibleIndex)
                Else
                    comboPromo.Enabled = False
                End If
                pm = Nothing
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub comboPromo_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim comboPromo As ASPxComboBox = CType(sender, ASPxComboBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(comboPromo.NamingContainer, GridViewDataItemTemplateContainer)
            Dim cpQUANTITY_MIN_ColumnValues As New ArrayList()
            Dim cpQUANTITY_GIFT_ColumnValues As New ArrayList()
            Dim cpDISCOUNT_PERCENT_ColumnValues As New ArrayList()
            For Each item As ListEditItem In comboPromo.Items
                cpQUANTITY_MIN_ColumnValues.Add(item.GetValue("QUANTITY_MIN"))
                cpQUANTITY_GIFT_ColumnValues.Add(item.GetValue("QUANTITY_GIFT"))
                cpDISCOUNT_PERCENT_ColumnValues.Add(item.GetValue("DISCOUNT_PERCENT"))
            Next item
            e.Properties("cpQUANTITY_MIN_ColumnValues") = cpQUANTITY_MIN_ColumnValues
            e.Properties("cpQUANTITY_GIFT_ColumnValues") = cpQUANTITY_GIFT_ColumnValues
            e.Properties("cpDISCOUNT_PERCENT_ColumnValues") = cpDISCOUNT_PERCENT_ColumnValues
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontomerce_Init(sender As Object, e As EventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            ctrl.Enabled = False
            If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Then
                ctrl.Enabled = True
                Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
                ctrl.ClientInstanceName = String.Format("tbscomerce_{0}", container.VisibleIndex)
                ctrl.ClientSideEvents.ValueChanged = String.Format("function(s, e) {{ OnScontoMerceValueChanged(s, e, {0}); }}", container.VisibleIndex)
                ctrl.ReadOnly = Not CType(Session("user"), user).iSpromorigascomerce
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontomerce_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontopercentuale_Init(sender As Object, e As EventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            ctrl.Enabled = False
            Dim dm As New datamanager
            If SASstatus = 1 AndAlso CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Then
                ctrl.Enabled = True
                Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
                Dim itemcode As String = DataBinder.Eval(container.DataItem, "CodiceArticolo")
                Dim ctcode As String = CType(Session("cart"), cartManager).Header.CODICECONTATTO
                Dim itemcodelot As String = "" 'DataBinder.Eval(container.DataItem, "Lotto")
                Dim pm As New promoManager
                Dim cd As intCanaleDirezione = pm.getCanaleDirezioneByOrderType(CType(Session("cart"), cartManager).Header.ordineHeader.Type)
                Dim prow As List(Of promoRow) = pm.getPromoRowsByCtCodeItemCode(ctcode, itemcode, itemcodelot, CType(Session("cart"), cartManager).Header.DATA_CREAZIONE(), cd)
                Dim promoLottoExists As Boolean = pm.existsPromoLotto(ctcode, itemcode, CType(Session("cart"), cartManager).Header.DATA_CREAZIONE, cd)
                If prow.Count > 0 Then
                    If prow(0).QUANTITY_GIFT = 0 And prow(0).DISCOUNT_PERCENT > 0 And Not promoLottoExists Then
                        ctrl.Text = prow(0).DISCOUNT_PERCENT
                    End If
                End If
                ctrl.ClientInstanceName = String.Format("tbscoperc_{0}", container.VisibleIndex)
                ctrl.ClientSideEvents.ValueChanged = String.Format("function(s, e) {{ OnScontoPercentualeValueChanged(s, e, {0}); }}", container.VisibleIndex)
                ctrl.ReadOnly = Not CType(Session("user"), user).iSpromorigascoperc
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontopercentuale_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub lb_prezzofinale_Init(sender As Object, e As EventArgs)
        Try
            Dim ctrl As ASPxLabel = CType(sender, ASPxLabel)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            ctrl.ClientInstanceName = String.Format("lbprezzofinale_{0}", container.VisibleIndex)
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub


#Region "Grglia Lotti"

    Protected Sub ASPxCallbackPanel_infolotti_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles ASPxCallbackPanel_infolotti.Callback
        Try
            If Not e.Parameter Is Nothing Then
                Call loadLotti(e.Parameter)
            End If
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub loadLotti(ByVal codicearticolo As String)
        Try
            Dim dm As New datamanager
            Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(codicearticolo)
            gridlotti.DataSource = dtLotti
            gridlotti.DataBind()
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub gridlotti_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles gridlotti.CustomColumnDisplayText
        Try
            If e.Column.Name = "ScadenzaLotto" Then
                If e.Value = "1900-01-01" Then
                    e.DisplayText = " nessuna "
                Else
                    e.DisplayText = String.Format("{0:dd-MM-yyyy}", e.Value)
                End If
            End If
            If e.Column.Name = "DisponibilitaLotto" Then
                Dim CodiceLotto As String = e.GetFieldValue("Lotto")
                If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                    e.DisplayText = CInt(e.Value) - CType(Session("cart"), cartManager).getQtaLottoInCart(e.GetFieldValue("Item No_"), e.GetFieldValue("Lotto"))
                Else
                    e.DisplayText = CInt(e.Value)
                End If
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

#End Region


#Region "Promo su Lotti"

    Protected Sub btnPromoLotto_Init(sender As Object, e As EventArgs)
        Try
            Dim btnPromoLotto As ASPxButton = CType(sender, ASPxButton)
            Dim dm As New datamanager
            btnPromoLotto.Visible = False
            If SASstatus = 1 AndAlso CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Then
                btnPromoLotto.Enabled = CType(Session("user"), user).iSpromorigacombo
                Dim container As GridViewDataItemTemplateContainer = CType(btnPromoLotto.NamingContainer, GridViewDataItemTemplateContainer)
                Dim comboPromo As ASPxComboBox = container.FindControl("comboPromo")
                Dim itemcode As String = DataBinder.Eval(container.DataItem, "CodiceArticolo")
                Dim ctcode As String = CType(Session("cart"), cartManager).Header.CODICECONTATTO
                Dim pm As New promoManager
                Dim cd As intCanaleDirezione = pm.getCanaleDirezioneByOrderType(CType(Session("cart"), cartManager).Header.ordineHeader.Type)
                Dim promoLottoExists As Boolean = pm.existsPromoLotto(ctcode, itemcode, CType(Session("cart"), cartManager).Header.DATA_CREAZIONE, cd)
                comboPromo.Visible = Not promoLottoExists
                btnPromoLotto.Visible = promoLottoExists
                btnPromoLotto.ClientSideEvents.Click = String.Format("function(s, e) {{ OnPromoLottoClick('{0}'); }}", itemcode)
                pm = Nothing
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub cbPanelPromoLotti_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cbPanelPromoLotti.Callback
        Try
            If Not e.Parameter Is Nothing Then
                Call loadGridPromoLotti(e.Parameter)
            End If
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub loadGridPromoLotti(ByVal codicearticolo As String)
        Try
            Dim dm As New datamanager
            Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(codicearticolo)
            Dim PrzRivenditore As Decimal = dm.getPrezzoRivenditore(codicearticolo, CType(Session("cart"), cartManager).Header.CODICELISTINO)
            dtLotti.Columns.Add(New DataColumn With {.ColumnName = "PrzRivenditore", .DataType = Type.GetType("System.Decimal"), .DefaultValue = PrzRivenditore})
            gridPromoLotti.DataSource = dtLotti
            gridPromoLotti.DataBind()
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub gridPromoLotti_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles gridPromoLotti.CustomButtonInitialize
        Try
            If e.VisibleIndex = -1 Then
                Return
            End If
            If e.CellType = GridViewTableCommandCellType.Data Then
                Dim dm As New datamanager
                Dim CodiceArticolo As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Item No_")
                Dim lotto As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Lotto").ToString
                Dim DisponibilitaLotto As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "DisponibilitaLotto")
                Dim isarcheo As Boolean = dm.isArcheopatico(CodiceArticolo)
                dm = Nothing
                If e.ButtonID = "addLottoToCart" Then
                    If CType(Session("cart"), cartManager).Header.CODICECLIENTE = "" Or CType(Session("cart"), cartManager).Header.IDCART = 0 Then
                        e.Enabled = False
                        e.Image.Url = "~/images/cartrowsmall_off.png"
                        e.Image.ToolTip = "Selezionare un cliente ed iniziare un ordine prima di poter aggiungere articoli."
                    Else
                        If CType(Session("cart"), cartManager).isOrdineChiuso Then 'blocco inserimento articoli perchè ordine è bloccato
                            e.Enabled = False
                            e.Image.Url = "~/images/cartrowsmall_off.png"
                            e.Image.ToolTip = "Ordine non modificabile."
                        Else
                            If DisponibilitaLotto <= 0 Then
                                e.Enabled = False
                                e.Image.Url = "~/images/cartrowsmall_off.png"
                                e.Image.ToolTip = "Prodotto non disponibile"
                            ElseIf DisponibilitaLotto > 0 Then
                                e.Enabled = True
                                e.Image.Url = "~/images/cartrowsmall.png"
                                e.Image.ToolTip = "Aggiungi al carrello"
                            End If
                            'gestione archeopatici
                            If isarcheo Then
                                e.Enabled = True
                                e.Image.Url = "~/images/cartrowsmall.png"
                                e.Image.ToolTip = "Aggiungi al carrello"
                            End If
                        End If
                    End If
                ElseIf e.ButtonID = "zeroPriceLotto" Then
                    If CType(Session("cart"), cartManager).Header.CODICECLIENTE = "" Or CType(Session("cart"), cartManager).Header.IDCART = 0 Then
                        e.Enabled = False
                        e.Image.Url = "~/images/zeropricesmall_off.png"
                        e.Image.ToolTip = "Selezionare un cliente ed iniziare un ordine prima di poter aggiungere articoli."
                    Else
                        If CType(Session("cart"), cartManager).isOrdineChiuso Then 'blocco inserimento articoli perchè ordine è bloccato
                            e.Enabled = False
                            e.Image.Url = "~/images/zeropricesmall_off.png"
                            e.Image.ToolTip = "Ordine non modificabile."
                        Else
                            If DisponibilitaLotto <= 0 Then
                                e.Enabled = False
                                e.Image.Url = "~/images/zeropricesmall_off.png"
                                e.Image.ToolTip = "Non disponibile"
                            ElseIf DisponibilitaLotto > 0 Then
                                e.Enabled = True
                                e.Image.Url = "~/images/zeropricesmall.png"
                                e.Image.ToolTip = "Imposta Riga Omaggio - Prezzo Zero"
                            End If
                            'gestione archeopatici
                            If isarcheo Then
                                e.Enabled = True
                                e.Image.Url = "~/images/zeropricesmall.png"
                                e.Image.ToolTip = "Imposta Riga Omaggio - Prezzo Zero"
                            End If
                        End If
                    End If
                ElseIf e.ButtonID = "delLottoFromCart" Then
                    If CType(Session("cart"), cartManager).getQtaLottoInCart(CodiceArticolo, lotto) <= 0 Then
                        e.Enabled = False
                        e.Image.Url = "~/images/delcartrowsmall_off.png"
                        e.Image.ToolTip = " "
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub gridPromoLotti_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles gridPromoLotti.CustomButtonCallback
        Try
            Dim gridPromoLotti As ASPxGridView = CType(sender, ASPxGridView)
            Dim No As String = gridPromoLotti.GetRowValues(e.VisibleIndex, "Item No_").ToString
            Dim lotto As String = gridPromoLotti.GetRowValues(e.VisibleIndex, "Lotto").ToString
            Dim idpromo As Long = 0
            Dim idcart As Integer = CType(Session("cart"), cartManager).Header.IDCART
            Dim qtacolumn As GridViewDataColumn = TryCast(gridPromoLotti.Columns("qtaLotto"), GridViewDataColumn)
            Dim scomercecolumn As GridViewDataColumn = TryCast(gridPromoLotti.Columns("scontomerceLotto"), GridViewDataColumn)
            Dim scontopercentualecolumn As GridViewDataColumn = TryCast(gridPromoLotti.Columns("scontopercentualeLotto"), GridViewDataColumn)
            Dim combopromocolumn As GridViewDataColumn = TryCast(gridPromoLotti.Columns("PromozioneLotto"), GridViewDataColumn)
            Dim qtaCtrl As ASPxTextBox = CType(gridPromoLotti.FindRowCellTemplateControl(e.VisibleIndex, qtacolumn, "tbqtaLotto"), ASPxTextBox)
            Dim qtaScoMerceCtrl As ASPxTextBox = CType(gridPromoLotti.FindRowCellTemplateControl(e.VisibleIndex, scomercecolumn, "tbscontomerceLotto"), ASPxTextBox)
            Dim qtaScoPercentualeCtrl As ASPxTextBox = CType(gridPromoLotti.FindRowCellTemplateControl(e.VisibleIndex, scontopercentualecolumn, "tbscontopercentualeLotto"), ASPxTextBox)
            Dim cmbopromoCtrl As ASPxComboBox = CType(gridPromoLotti.FindRowCellTemplateControl(e.VisibleIndex, combopromocolumn, "comboPromoLotto"), ASPxComboBox)
            Dim qta As Integer = IIf(IsNumeric(qtaCtrl.Text), qtaCtrl.Text, 0)
            Dim qtascomerce As Integer = 0
            Dim scontopercentuale As String = ""
            If Not qtaScoMerceCtrl Is Nothing Then
                qtascomerce = IIf(IsNumeric(qtaScoMerceCtrl.Text), qtaScoMerceCtrl.Text, 0)
            End If
            If Not qtaScoPercentualeCtrl Is Nothing Then
                scontopercentuale = IIf(IsNumeric(qtaScoPercentualeCtrl.Text), qtaScoPercentualeCtrl.Text, "")
            End If
            If Not cmbopromoCtrl Is Nothing AndAlso Not cmbopromoCtrl.SelectedItem Is Nothing Then
                idpromo = cmbopromoCtrl.SelectedItem.GetValue("ID")
            End If

            If e.ButtonID = "addLottoToCart" Then
                If IsNumeric(qta) AndAlso CInt(qta) > 0 Then
                    CType(Session("cart"), cartManager).addLine(No, qta, lotto, False, False, 0, False, "", idpromo, scontopercentuale)
                    If qtascomerce > 0 Then
                        CType(Session("cart"), cartManager).addLine(No, qtascomerce, lotto, False, False, 1, False, "", idpromo, "")
                    End If
                End If
                qtaCtrl.Text = 0
                ' ASPxGridView_Cart.DataBind()
                Call loadGridPromoLotti(No)
                CType(Session("cart"), cartManager).checkPromoHeader(True)
            ElseIf e.ButtonID = "zeroPriceLotto" Then
                If IsNumeric(qta) AndAlso CInt(qta) > 0 Then
                    CType(Session("cart"), cartManager).addLine(No, qta, lotto, , , 1, False, , , "")
                End If
                qtaCtrl.Text = 0
                'ASPxGridView_Cart.DataBind()
                CType(Session("cart"), cartManager).checkPromoHeader(True)
            ElseIf e.ButtonID = "delLottoFromCart" Then
                CType(Session("cart"), cartManager).deleteLine(No, lotto, -1, -1, "-1")
                'ASPxGridView_Cart.DataBind()
                Call loadGridPromoLotti(No)
                CType(Session("cart"), cartManager).checkPromoHeader(True)
            End If
        Catch ex As Exception
            Log.Error(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub gridPromoLotti_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles gridPromoLotti.CustomColumnDisplayText
        Try
            If e.Column.Name = "ScadenzaLotto" Then
                If e.Value = "1900-01-01" Then
                    e.DisplayText = " nessuna "
                Else
                    e.DisplayText = String.Format("{0:dd-MM-yyyy}", e.Value)
                End If
            End If
            If e.Column.Name = "DisponibilitaLotto" Then
                Dim CodiceLotto As String = e.GetFieldValue("Lotto")
                If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                    e.DisplayText = CInt(e.Value) - CType(Session("cart"), cartManager).getQtaLottoInCart(e.GetFieldValue("Item No_"), e.GetFieldValue("Lotto"))
                Else
                    e.DisplayText = CInt(e.Value)
                End If
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub gridPromoLotti_CustomJSProperties(sender As Object, e As ASPxGridViewClientJSPropertiesEventArgs) Handles gridPromoLotti.CustomJSProperties
        Try
            Dim qtyDispoValues As New Dictionary(Of Integer, Integer)()
            For i = 0 To gridPromoLotti.VisibleRowCount - 1
                qtyDispoValues.Add(i, gridPromoLotti.GetRowValues(i, "DisponibilitaLotto"))
            Next
            e.Properties("cpQtyDispoValues") = qtyDispoValues
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbqtaLotto_Init(sender As Object, e As EventArgs)
        Try
            Dim qtaCtrl As ASPxTextBox = CType(sender, ASPxTextBox)
            qtaCtrl.Enabled = False
            If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Then
                Dim container As GridViewDataItemTemplateContainer = TryCast(qtaCtrl.NamingContainer, GridViewDataItemTemplateContainer)
                qtaCtrl.ClientInstanceName = String.Format("tbqtaLotto_{0}", container.VisibleIndex)
                qtaCtrl.ClientSideEvents.ValueChanged = String.Format("function(s, e) {{ OnQtaLottoValueChanged(s, e, {0}); }}", container.VisibleIndex)

                Dim dispoLotto As Integer = DataBinder.Eval(container.DataItem, "DisponibilitaLotto")
                Dim CodiceArticolo As String = DataBinder.Eval(container.DataItem, "Item No_")
                Dim Lotto As String = DataBinder.Eval(container.DataItem, "Lotto")
                Dim dispoLottoInCart As Integer = CType(Session("cart"), cartManager).getQtaLottoInCart(CodiceArticolo, Lotto)
                'Dim codmacrolinea As String = DataBinder.Eval(container.DataItem, "cod_macrolinea")
                Dim dm As New datamanager
                Dim isArcheo As Boolean = dm.isArcheopatico(CodiceArticolo)
                dm = Nothing

                If CType(Session("cart"), cartManager).Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                    If dispoLotto - dispoLottoInCart > 0 Then
                        qtaCtrl.Enabled = True
                        qtaCtrl.Text = 0
                    Else
                        qtaCtrl.Enabled = False
                        qtaCtrl.Text = 0
                    End If
                Else
                    If dispoLotto > 0 Then
                        qtaCtrl.Enabled = True
                        qtaCtrl.Text = 0
                    Else
                        qtaCtrl.Enabled = False
                        qtaCtrl.Text = 0
                    End If
                End If

                'gestione archeopatici
                If isArcheo Then
                    qtaCtrl.Enabled = True
                    qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaLottoValidation(s,e,{0},{1},{2}); }}", container.VisibleIndex, 1, dispoLotto)
                Else
                    qtaCtrl.ClientSideEvents.KeyUp = String.Format("function(s,e){{ onQtaLottoValidation(s,e,{0},{1},{2}); }}", container.VisibleIndex, 0, dispoLotto)
                End If

            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbqtaLotto_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub comboPromoLotto_Init(sender As Object, e As EventArgs)
        Try
            Dim comboPromo As ASPxComboBox = CType(sender, ASPxComboBox)
            Dim dm As New datamanager
            comboPromo.Enabled = False
            If dm.GetSASstatus() = 1 AndAlso CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Then
                comboPromo.Enabled = CType(Session("user"), user).iSpromorigacombo
                Dim container As GridViewDataItemTemplateContainer = CType(comboPromo.NamingContainer, GridViewDataItemTemplateContainer)
                Dim itemcode As String = DataBinder.Eval(container.DataItem, "Item No_")
                Dim ctcode As String = CType(Session("cart"), cartManager).Header.CODICECONTATTO
                Dim itemcodelot As String = DataBinder.Eval(container.DataItem, "Lotto")
                Dim pm As New promoManager
                Dim cd As intCanaleDirezione = pm.getCanaleDirezioneByOrderType(CType(Session("cart"), cartManager).Header.ordineHeader.Type)
                Dim prow As List(Of promoRow) = pm.getPromoRowsByCtCodeItemCode(ctcode, itemcode, itemcodelot, CType(Session("cart"), cartManager).Header.DATA_CREAZIONE, cd)
                If prow.Count > 0 Then
                    comboPromo.DataSource = prow
                    comboPromo.DataBind()
                    comboPromo.SelectedIndex = 0
                    comboPromo.ClientSideEvents.ButtonClick = String.Format("function(s, e) {{ OnComboPromoLottoButtonClick(s, e, {0}); }}", container.VisibleIndex)
                Else
                    comboPromo.Enabled = False
                End If
                pm = Nothing
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub comboPromoLotto_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim comboPromoLotto As ASPxComboBox = CType(sender, ASPxComboBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(comboPromoLotto.NamingContainer, GridViewDataItemTemplateContainer)
            Dim cpQUANTITY_MIN_ColumnValues As New ArrayList()
            Dim cpQUANTITY_GIFT_ColumnValues As New ArrayList()
            Dim cpDISCOUNT_PERCENT_ColumnValues As New ArrayList()
            For Each item As ListEditItem In comboPromoLotto.Items
                cpQUANTITY_MIN_ColumnValues.Add(item.GetValue("QUANTITY_MIN"))
                cpQUANTITY_GIFT_ColumnValues.Add(item.GetValue("QUANTITY_GIFT"))
                cpDISCOUNT_PERCENT_ColumnValues.Add(item.GetValue("DISCOUNT_PERCENT"))
            Next item
            e.Properties("cpQUANTITY_MIN_ColumnValues") = cpQUANTITY_MIN_ColumnValues
            e.Properties("cpQUANTITY_GIFT_ColumnValues") = cpQUANTITY_GIFT_ColumnValues
            e.Properties("cpDISCOUNT_PERCENT_ColumnValues") = cpDISCOUNT_PERCENT_ColumnValues
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontomerceLotto_Init(sender As Object, e As EventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            ctrl.Enabled = False
            If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Then
                ctrl.Enabled = True
                Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
                ctrl.ClientInstanceName = String.Format("tbscomerceLotto_{0}", container.VisibleIndex)
                ctrl.ClientSideEvents.ValueChanged = String.Format("function(s, e) {{ OnScontoMerceLottoValueChanged(s, e, {0}); }}", container.VisibleIndex)
                ctrl.ReadOnly = Not CType(Session("user"), user).iSpromorigascomerce
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontomerceLotto_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontopercentualeLotto_Init(sender As Object, e As EventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            ctrl.Enabled = False
            Dim dm As New datamanager
            If dm.GetSASstatus() = 1 AndAlso CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Then
                ctrl.Enabled = True
                Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
                Dim itemcode As String = DataBinder.Eval(container.DataItem, "Item No_")
                Dim ctcode As String = CType(Session("cart"), cartManager).Header.CODICECONTATTO
                Dim itemcodelot As String = DataBinder.Eval(container.DataItem, "Lotto")
                Dim pm As New promoManager
                Dim cd As intCanaleDirezione = pm.getCanaleDirezioneByOrderType(CType(Session("cart"), cartManager).Header.ordineHeader.Type)
                Dim prow As List(Of promoRow) = pm.getPromoRowsByCtCodeItemCode(ctcode, itemcode, itemcodelot, CType(Session("cart"), cartManager).Header.DATA_CREAZIONE(), cd)
                If prow.Count > 0 Then
                    If prow(0).QUANTITY_GIFT = 0 And prow(0).DISCOUNT_PERCENT > 0 Then
                        ctrl.Text = prow(0).DISCOUNT_PERCENT
                    End If
                End If
                ctrl.ClientInstanceName = String.Format("tbscopercLotto_{0}", container.VisibleIndex)
                ctrl.ClientSideEvents.ValueChanged = String.Format("function(s, e) {{ OnScontoPercentualeLottoValueChanged(s, e, {0}); }}", container.VisibleIndex)
                ctrl.ReadOnly = Not CType(Session("user"), user).iSpromorigascoperc
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub tbscontopercentualeLotto_CustomJSProperties(sender As Object, e As CustomJSPropertiesEventArgs)
        Try
            Dim ctrl As ASPxTextBox = CType(sender, ASPxTextBox)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            e.Properties("cpPrezzoRivenditore") = DataBinder.Eval(container.DataItem, "PrzRivenditore")
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub lb_prezzofinaleLotto_Init(sender As Object, e As EventArgs)
        Try
            Dim ctrl As ASPxLabel = CType(sender, ASPxLabel)
            Dim container As GridViewDataItemTemplateContainer = TryCast(ctrl.NamingContainer, GridViewDataItemTemplateContainer)
            ctrl.ClientInstanceName = String.Format("lbprezzofinaleLotto_{0}", container.VisibleIndex)
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub

    Protected Sub lb_qtaArtLottoInCart_Init(sender As Object, e As EventArgs)
        Try
            Dim qtaInCartLottoCtrl As ASPxLabel = CType(sender, ASPxLabel)
            Dim container As GridViewDataItemTemplateContainer = TryCast(qtaInCartLottoCtrl.NamingContainer, GridViewDataItemTemplateContainer)
            Dim itemcode As String = DataBinder.Eval(container.DataItem, "Item No_")
            Dim itemcodelot As String = DataBinder.Eval(container.DataItem, "Lotto")
            Dim dispoLottoInCart As Integer = CType(Session("cart"), cartManager).getQtaLottoInCart(itemcode, itemcodelot)
            If dispoLottoInCart > 0 Then
                qtaInCartLottoCtrl.Text = dispoLottoInCart.ToString
            Else
                qtaInCartLottoCtrl.Text = ""
            End If
        Catch ex As Exception
            Log.Error(ex)
        End Try
    End Sub





#End Region

End Class