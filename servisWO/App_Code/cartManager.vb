Imports System.Data
Imports servisWO.datamanager
Imports log4net
Imports System.Reflection

Partial Public Class cartManager

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Public Property Header() As carrelloHeader
    Public Property carrelloLines() As List(Of carrelloLine)
    Public Property carrelloHeaderPromos() As List(Of carrelloHeaderPromo)
    Public Property carrelloLinePromos() As List(Of carrelloLinePromo)

    Public Sub New()
        Me.clearCart()
    End Sub

    Public Function clearCart() As Boolean
        Header = New carrelloHeader
        carrelloLines = New List(Of carrelloLine)
        carrelloHeaderPromos = New List(Of carrelloHeaderPromo)
        carrelloLinePromos = New List(Of carrelloLinePromo)
        loadListino()
        Return True
    End Function

    Public Function loadFromDBbyOrderCode(ByVal orderCode As String, Optional ByVal orderByCollocazione As Boolean = False, Optional ByVal refreshDisponibilita As Boolean = False) As cartManager
        Dim dm As New datamanager
        Dim idcarrello As Int64 = dm.getIDCarrelloByOrderCode(orderCode)
        If idcarrello > 0 Then
            loadFromDBbyIdCarrello(idcarrello, orderByCollocazione)
        End If
        If refreshDisponibilita Then
            Call refreshDispo()
        End If
        dm = Nothing
        Return Me
    End Function

    Public Function loadFromDBbyIdCarrello(ByVal idCarrello As Int64, Optional ByVal orderByCollocazione As Boolean = False) As cartManager
        Me.clearCart()
        Dim dm As New datamanager
        Dim dtH As DataTable = dm.getCarrelloHeader(idCarrello)
        Dim dtL As DataTable = dm.getCarrelloLinee(idCarrello, orderByCollocazione)
        Dim dtP As DataTable = dm.getCarrelloPromo(idCarrello)
        Dim dtPL As DataTable = dm.getCarrelloPromoLines(idCarrello)
        If dtH.Rows.Count > 0 Then
            Dim row As DataRow = dtH.Rows(0)
            Header.IDCART = row("IDCART")
            Header.DESCRIPTION = row("DESCRIPTION")
            Header.CODICECLIENTE = row("CODICECLIENTE")
            Header.CODICECONTATTO = row("CODICECONTATTO")
            Header.UTENTE_CREAZIONE = row("UTENTE_CREAZIONE")
            Header.DATA_CREAZIONE = row("DATA_CREAZIONE")
            Header.UTENTE_ULTIMA_MODIFICA = row("UTENTE_ULTIMA_MODIFICA")
            Header.AGENTE = row("AGENTE")
            Header.DATA_ULTIMA_MODIFICA = row("DATA_ULTIMA_MODIFICA")
            Header.BLOCKED = row("BLOCKED")
            Header.IMPORTOEVADIBILITAORDINE = row("IMPORTOEVADIBILITAORDINE")
            Header.IMPORTOPORTOFRANCO = row("IMPORTOPORTOFRANCO")
            Header.IMPORTOSPESESPEDIZIONE = row("IMPORTOSPESESPEDIZIONE")
            Header.SCONTOHEADER = row("SCONTOHEADER")
            Header.TIPO = [Enum].Parse(GetType(carrelloHeader.TIPOCARRELLO), row("TIPO"))
            Header.SCONTOPAGAMENTO = row("SCONTOPAGAMENTO")
            Header.ordineHeader.Code = row("Code")
            Header.ordineHeader.CompanyName = row("CompanyName_")
            Header.ordineHeader.CustomerNo = row("CustomerNo")
            Header.ordineHeader.OrderDate = row("OrderDate")
            Header.ordineHeader.Type = row("Type")
            Header.ordineHeader.ShippingAgentCode = row("ShippingAgentCode")
            Header.ordineHeader.ShipAddressCode = row("ShipAddressCode")
            Header.ordineHeader.Notes = row("Notes")
            Header.ordineHeader.AttachmentPath = row("AttachmentPath")
            Header.ordineHeader.OperatorCode = row("OperatorCode")
            Header.ordineHeader.PackageNum = row("PackageNum")
            Header.ordineHeader.OverpackageNum = row("OverpackageNum")
            Header.ordineHeader.User = row("User")
            Header.ordineHeader.CompletedImported = row("CompletedImported")
            Header.ordineHeader.IncludeShipCost = row("IncludeShipCost")
            Header.ordineHeader.PaymentMethodCode = row("PaymentMethodCode")
            Header.ordineHeader.PaymentTermsCode = row("PaymentTermsCode")
            Header.ordineHeader.Weight = row("Weight")
            Header.ordineHeader.OrderNoCtrl = row("OrderNoCtrl")
            Header.ordineHeader.UserCtrl = row("UserCtrl")
            Header.ordineHeader.Status = row("Status")
            Header.STATUSMAGAZZINO = row("STATUSMAGAZZINO")
            Header.STATUSPRODUZIONE = row("STATUSPRODUZIONE")
            Header.IDPROFILOSCONTO = row("idprofilosconto")
            Header.ORIGINATODAORDINE = row("ORIGINATODAORDINE")
            Header.ordineHeader.Voucher_Value = row("Voucher Value")
            Header.PRENOTAZIONE = row("PRENOTAZIONE")
            Header.DATA_EVASIONE = row("DATA_EVASIONE")

            Dim prenotazione As Boolean = (Header.PRENOTAZIONE = 1 Or Header.PRENOTAZIONE = 2)

            For Each rowLine As DataRow In dtL.Rows
                Dim cl As New carrelloLine
                cl.IDCART = rowLine("IDCART")
                cl.IDCARTLINE = rowLine("IDCARTLINE")
                cl.IDPROMO = rowLine("IDPROMO")
                cl.DESCRIZIONE = rowLine("DESCRIZIONE")
                cl.DISPONIBILITA = dm.GetDisponibilitaProdotti(rowLine("ItemCode"), True).Rows(0).Item("Disponibilita")
                cl.UNITPRICELIST = rowLine("UNITPRICELIST")
                cl.FORMATO = rowLine("FORMATO")
                cl.IVA = rowLine("IVA")
                cl.MARCHIO = rowLine("MARCHIO")
                cl.COMPOSIZIONE = rowLine("COMPOSIZIONE")
                cl.TOTALERIGA = rowLine("TOTALERIGA")
                'cl.DISPOLOTTO = rowLine("DISPOLOTTO")
                cl.SCONTOHEADER = rowLine("SCONTOHEADER")
                cl.SCONTOPAGAMENTO = rowLine("SCONTOPAGAMENTO")
                cl.SCONTORIGA = rowLine("SCONTORIGA")


                Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(rowLine("ItemCode"))
                If Not prenotazione Then
                    If rowLine("LotNo") <> "" AndAlso dtLotti.Rows.Count > 0 AndAlso dtLotti.Select("Lotto='" & rowLine("LotNo") & "'").Count > 0 Then
                        If Header.ordineHeader.Status <> 6 And Header.ordineHeader.Status <> 4 Then
                            cl.DISPOLOTTO = dtLotti.Select("Lotto='" & rowLine("LotNo") & "'")(0).Item("DisponibilitaLotto") + rowLine("OriginalQty")
                        Else
                            cl.DISPOLOTTO = dtLotti.Select("Lotto='" & rowLine("LotNo") & "'")(0).Item("DisponibilitaLotto")
                        End If
                    Else
                        cl.DISPOLOTTO = 0
                    End If
                End If

                cl.ordineLine.OrderCode = rowLine("OrderCode")
                cl.ordineLine.ItemCode = rowLine("ItemCode")
                cl.ordineLine.RowDiscount = rowLine("RowDiscount")
                cl.ordineLine.LineID = rowLine("LineID")
                cl.ordineLine.OldItemCode = rowLine("OldItemCode")
                cl.ordineLine.CompanyName = rowLine("CompanyName_")
                cl.ordineLine.UnitPrice = rowLine("UnitPrice")
                cl.ordineLine.Quantity = rowLine("Quantity")
                cl.ordineLine.DiscountQty = rowLine("DiscountQty")
                cl.ordineLine.Imported = rowLine("Imported")
                cl.ordineLine.QtyToShip = rowLine("QtyToShip")
                cl.ordineLine.FormulaSconto = rowLine("FormulaSconto")
                cl.ordineLine.LineNo = rowLine("LineNo")
                cl.ordineLine.OriginalQty = rowLine("OriginalQty")
                cl.LOADED = rowLine("Loaded")
                cl.LINEIDSOURCE = rowLine("LineIdSource")
                cl.DATEENTRY = rowLine("DataEntry")
                cl.ordineLine.BinCode = rowLine("BinCode")
                cl.ordineLine.LotNo = rowLine("LotNo")

                If Not prenotazione Then
                    'Procedura di ripresa ordine: se LotNo = BLANCK verifico se esistono lotto e bincode con qta disponibile per aggiornare i campi BinCode e LotNo X ARCHEO
                    If rowLine("LotNo") = "" AndAlso dtLotti.Rows.Count > 0 Then
                        Dim tmpLotto As String = ""
                        Dim tmpBinCode As String = ""
                        Dim tmpDispoLotto As Integer = 0
                        For Each lotto As DataRow In dtLotti.Rows
                            If lotto("DisponibilitaLotto") >= cl.ordineLine.OriginalQty Then
                                tmpLotto = lotto("Lotto")
                                tmpDispoLotto = lotto("DisponibilitaLotto")
                                Exit For
                            End If
                        Next
                        If tmpLotto <> "" Then
                            tmpBinCode = dm.GetDefaultBinCodeByItemcode(rowLine("ItemCode"), dm.GetParametroSito(parametriSitoValue.magazzinodefault))
                        End If
                        If tmpLotto <> "" AndAlso tmpBinCode <> "" Then
                            cl.ordineLine.LotNo = tmpLotto
                            cl.DISPOLOTTO = tmpDispoLotto
                            cl.ordineLine.BinCode = tmpBinCode
                        End If
                    End If
                End If

                carrelloLines.Add(cl)
                'If rowLine("IDPROMO") > 0 Then
                '    promoApplicate.Add(rowLine("IDPROMO"))
                'End If
            Next

            If dtP.Rows.Count > 0 Then
                For Each rowCarrelloPromo As DataRow In dtP.Rows
                    Dim cp As New carrelloHeaderPromo
                    cp.IDCART = rowCarrelloPromo("IDCART")
                    cp.IDPROMO = rowCarrelloPromo("IDPROMO")
                    cp.NRUTILIZZI = rowCarrelloPromo("NRUTILIZZI")
                    cp.CTCODE = rowCarrelloPromo("CTCODE")
                    carrelloHeaderPromos.Add(cp)
                Next
            End If

            If dtPL.Rows.Count > 0 Then
                For Each rowCarrelloLinePromo As DataRow In dtPL.Rows
                    Dim clp As New carrelloLinePromo
                    clp.IDCART = rowCarrelloLinePromo("IDCART")
                    clp.IDCARTLINE = rowCarrelloLinePromo("IDCARTLINE")
                    clp.IDPROMO = rowCarrelloLinePromo("IDPROMO")
                    clp.NRMULTIPLI = rowCarrelloLinePromo("NRMULTIPLI")
                    clp.CTCODE = rowCarrelloLinePromo("CTCODE")
                    carrelloLinePromos.Add(clp)
                Next
            End If

            'gestione listino
            loadListino(Header.CODICECLIENTE)

        End If
        dm = Nothing
        Return Me
    End Function

    'Public Sub loadOrderFromNAV(ByVal ordercode As String)
    '    Dim dm As New datamanager
    '    Dim dtH As DataTable = dm.GetOrderHeader(ordercode).Tables(0)
    '    If dtH.Rows.Count > 0 Then
    '        Dim row As DataRow = dtH.Rows(0)
    '        Header.IDCART = 0
    '        Header.DESCRIPTION = ""
    '        Header.CODICECLIENTE = row("CustomerNo")
    '        Header.CODICECONTATTO = dm.GetContactCodeByCustomerCode(Header.CODICECLIENTE)
    '        'Header.UTENTE_CREAZIONE = row("UTENTE_CREAZIONE")
    '        'Header.DATA_CREAZIONE = row("DATA_CREAZIONE")
    '        'Header.UTENTE_ULTIMA_MODIFICA = row("UTENTE_ULTIMA_MODIFICA")
    '        'Header.DATA_ULTIMA_MODIFICA = row("DATA_ULTIMA_MODIFICA")
    '        'Header.BLOCKED = row("BLOCKED")
    '        Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE
    '        'Header.SCONTOPAGAMENTO = row("SCONTOPAGAMENTO")
    '        Header.IMPORTOEVADIBILITAORDINE = 0
    '        Header.IMPORTOPORTOFRANCO = 0
    '        Header.IMPORTOSPESESPEDIZIONE = dm.GetSpeseTrasporto()
    '        Header.ordineHeader.Code = row("Code")
    '        Header.ordineHeader.CompanyName = row("CompanyName_")
    '        Header.ordineHeader.CustomerNo = row("CustomerNo")
    '        Header.ordineHeader.OrderDate = row("OrderDate")
    '        Header.ordineHeader.Type = row("Type")
    '        Header.ordineHeader.ShippingAgentCode = row("ShippingAgentCode")
    '        Header.ordineHeader.ShipAddressCode = row("ShipAddressCode")
    '        Header.ordineHeader.Notes = row("Notes")
    '        Header.ordineHeader.AttachmentPath = row("AttachmentPath")
    '        Header.ordineHeader.OperatorCode = row("OperatorCode")
    '        Header.ordineHeader.PackageNum = row("PackageNum")
    '        Header.ordineHeader.OverpackageNum = row("OverpackageNum")
    '        Header.ordineHeader.User = row("User")
    '        Header.ordineHeader.CompletedImported = row("CompletedImported")
    '        Header.ordineHeader.IncludeShipCost = row("IncludeShipCost")
    '        Header.ordineHeader.PaymentMethodCode = row("PaymentMethodCode")
    '        Header.ordineHeader.PaymentTermsCode = row("PaymentTermsCode")
    '        Header.ordineHeader.Weight = row("Weight")
    '        Header.ordineHeader.OrderNoCtrl = row("OrderNoCtrl")
    '        Header.ordineHeader.UserCtrl = row("UserCtrl")
    '        Header.ordineHeader.Status = row("Status")

    '        Dim ol As DataTable = dm.GetOrderLines(ordercode).Tables(0)
    '        For Each rowLine As DataRow In ol.Rows
    '            Dim cl As New carrelloLine
    '            cl.IDCART = 0
    '            cl.IDCARTLINE = 0
    '            cl.IDPROMO = 0
    '            Dim dtItem As DataTable = dm.GetWOItems(rowLine("ItemCode"), , False)
    '            If dtItem.Rows.Count > 0 Then
    '                cl.DESCRIZIONE = dtItem.Rows(0).Item("Descrizione")
    '                cl.DISPONIBILITA = dtItem.Rows(0).Item("Disponibilita")
    '                cl.UNITPRICELIST = dtItem.Rows(0).Item("PrzPubblico")
    '                cl.FORMATO = dtItem.Rows(0).Item("Formato")
    '                cl.IVA = dtItem.Rows(0).Item("IVA")
    '                cl.MARCHIO = dtItem.Rows(0).Item("Marchio")
    '                cl.COMPOSIZIONE = dtItem.Rows(0).Item("Composizione")
    '                Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(rowLine("ItemCode"))
    '                If dtLotti.Rows.Count > 0 AndAlso dtLotti.Select("Lotto='" & rowLine("LotNo") & "'").Count > 0 Then
    '                    cl.DISPOLOTTO = dtLotti.Select("Lotto='" & rowLine("LotNo") & "'")(0).Item("DisponibilitaLotto")
    '                End If
    '            End If
    '            If rowLine("RowDiscount") = 1 Then
    '                cl.TOTALERIGA = 0
    '            Else
    '                Dim totRiga As Double = rowLine("UnitPrice") * rowLine("Quantity")
    '                Dim sconti() As String = rowLine("Formulasconto").ToString.Split("-")
    '                If sconti.Count > 0 Then
    '                    For i = 0 To sconti.Count - 1
    '                        If IsNumeric(sconti(i)) Then
    '                            totRiga = totRiga - (totRiga / 100 * sconti(i))
    '                        End If
    '                    Next
    '                End If
    '                cl.TOTALERIGA = totRiga
    '            End If

    '            cl.ordineLine.OrderCode = rowLine("OrderCode")
    '            cl.ordineLine.ItemCode = rowLine("ItemCode")
    '            cl.ordineLine.RowDiscount = rowLine("RowDiscount")
    '            cl.ordineLine.LineID = rowLine("LineID")
    '            cl.ordineLine.OldItemCode = rowLine("OldItemCode")
    '            cl.ordineLine.CompanyName = rowLine("CompanyName_")
    '            cl.ordineLine.UnitPrice = rowLine("UnitPrice")
    '            cl.ordineLine.Quantity = rowLine("Quantity")
    '            cl.ordineLine.DiscountQty = rowLine("DiscountQty")
    '            cl.ordineLine.Imported = rowLine("Imported")
    '            cl.ordineLine.BinCode = rowLine("BinCode")
    '            cl.ordineLine.QtyToShip = rowLine("QtyToShip")
    '            cl.ordineLine.LotNo = rowLine("LotNo")
    '            cl.ordineLine.FormulaSconto = rowLine("FormulaSconto")
    '            cl.ordineLine.LineNo = rowLine("LineNo")
    '            cl.ordineLine.OriginalQty = rowLine("OriginalQty")
    '            carrelloLines.Add(cl)
    '        Next
    '    End If
    '    dm = Nothing
    'End Sub

    Public Function isOrdineChiuso() As Boolean
        Dim ordinechiuso As Boolean = False
        If Me.Header.ordineHeader.Status = 3 Or Me.Header.ordineHeader.Status = 4 Or Me.Header.ordineHeader.Status = 5 Or Me.Header.ordineHeader.Status = 6 Or Me.Header.ordineHeader.CompletedImported = 1 Then
            ordinechiuso = True
        End If
        Return ordinechiuso
    End Function

    Public Function isOrdineInPortoFranco() As Boolean
        Return (Me.GetTotaleMerceConScontoPagamento >= Me.Header.IMPORTOPORTOFRANCO)
    End Function

    Public Function isOrdineInImportoMinimo() As Boolean
        Return ((GetTotaleMerceConScontoPagamento()) >= Header.IMPORTOEVADIBILITAORDINE)
    End Function

    Public Function controlloGiacenze(Optional ByVal escludiArcheopatici As Boolean = False, Optional ByVal salvaEinviaDaCarrello As Boolean = False) As Boolean
        Dim controllo As Boolean = True
        Dim dm As New datamanager

        'aggiorno disponibilità e dispinibilità di lotto nella struttura carrello: DISPONIBILITA' REALI tenendo conto anche delle qta in lavorazione dell'item in altri ordini
        For Each item As carrelloLine In carrelloLines
            item.DISPONIBILITA = dm.GetDisponibilitaProdotti(item.ordineLine.ItemCode, True).Rows(0).Item("Disponibilita")
            Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(item.ordineLine.ItemCode, True)
            If dtLotti.Rows.Count > 0 AndAlso dtLotti.Select("Lotto='" & item.ordineLine.LotNo & "'").Count > 0 Then
                item.DISPOLOTTO = dtLotti.Select("Lotto='" & item.ordineLine.LotNo & "'")(0).Item("DisponibilitaLotto")
            End If
        Next

        'controllo giacenze in base al tipo carrello
        If Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
            'controllo le giacenze, nel carrello-ordine le disponibilità tengono già conto delle quantità righe ordine
            'devo controllare solo gli archeopatici prima di inviare a magazzino
            For Each item As carrelloLine In carrelloLines
                If Not escludiArcheopatici Then
                    If dm.isArcheopatico(item.ordineLine.ItemCode) Then
                        If item.ordineLine.RowDiscount = 0 Then
                            If item.ordineLine.Quantity > item.DISPOLOTTO + item.ordineLine.Quantity Then 'devo aggiungere la qta dell'ordine corrente sottratta precedentemente dalla disponibilità reale
                                controllo = False
                                Exit For
                            End If
                        ElseIf item.ordineLine.RowDiscount = 1 Then
                            If item.ordineLine.DiscountQty > item.DISPOLOTTO + item.ordineLine.DiscountQty Then 'devo aggiungere la qta dell'ordine corrente sottratta precedentemente dalla disponibilità reale
                                controllo = False
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next
        ElseIf Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO AndAlso Not salvaEinviaDaCarrello Then
            'nel carrello-carrello escludo il controllo sugli archeopatici
            For Each item As carrelloLine In carrelloLines
                If Not dm.isArcheopatico(item.ordineLine.ItemCode) Then
                    If item.ordineLine.LotNo = "" Then
                        If item.ordineLine.RowDiscount = 0 Then
                            If item.ordineLine.Quantity > item.DISPONIBILITA Then
                                controllo = False
                                Exit For
                            End If
                        ElseIf item.ordineLine.RowDiscount = 1 Then
                            If item.ordineLine.DiscountQty > item.DISPONIBILITA Then
                                controllo = False
                                Exit For
                            End If
                        End If
                    ElseIf item.ordineLine.LotNo <> "" Then
                        If item.ordineLine.RowDiscount = 0 Then
                            If item.ordineLine.Quantity > item.DISPOLOTTO Then
                                controllo = False
                                Exit For
                            End If
                        ElseIf item.ordineLine.RowDiscount = 1 Then
                            If item.ordineLine.DiscountQty > item.DISPOLOTTO Then
                                controllo = False
                                Exit For
                            End If
                        End If
                    End If
                End If
            Next
        ElseIf Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO AndAlso salvaEinviaDaCarrello Then
            'devo controllare tutte le disponibilità come se fosse un ordine
            For Each item As carrelloLine In carrelloLines
                If Not dm.isArcheopatico(item.ordineLine.ItemCode) Then
                    If item.ordineLine.LotNo = "" Then
                        If item.ordineLine.RowDiscount = 0 Then
                            If item.ordineLine.Quantity > item.DISPOLOTTO Then
                                controllo = False
                                Exit For
                            End If
                        ElseIf item.ordineLine.RowDiscount = 1 Then
                            If item.ordineLine.DiscountQty > item.DISPOLOTTO Then
                                controllo = False
                                Exit For
                            End If
                        End If
                    ElseIf item.ordineLine.LotNo <> "" Then
                        If item.ordineLine.RowDiscount = 0 Then
                            If item.ordineLine.Quantity > item.DISPOLOTTO Then
                                controllo = False
                                Exit For
                            End If
                        ElseIf item.ordineLine.RowDiscount = 1 Then
                            If item.ordineLine.DiscountQty > item.DISPOLOTTO Then
                                controllo = False
                                Exit For
                            End If
                        End If
                    End If
                Else 'se archeopatico
                    If item.ordineLine.RowDiscount = 0 Then
                        If item.ordineLine.Quantity > item.DISPOLOTTO Then
                            controllo = False
                            Exit For
                        End If
                    ElseIf item.ordineLine.RowDiscount = 1 Then
                        If item.ordineLine.DiscountQty > item.DISPOLOTTO Then
                            controllo = False
                            Exit For
                        End If
                    End If
                End If
            Next
        End If

        dm = Nothing
        Return controllo
    End Function

    'Public Sub refreshDispo()
    '    Dim dm As New datamanager
    '    For Each item In carrelloLines
    '        Dim totaleDisponibilità As Integer = dm.GetDisponibilitaProdotti(item.ordineLine.ItemCode, True).Rows(0).Item("Disponibilita")
    '        Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(item.ordineLine.ItemCode)
    '        Dim nrLotti As Integer = dtLotti.Rows.Count
    '        For l = 0 To nrLotti - 1
    '            If item.ordineLine.ItemCode = dtLotti.Rows(l).Item("Item No_") And item.ordineLine.LotNo = dtLotti.Rows(l).Item("Lotto") Then
    '                If Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
    '                    item.DISPONIBILITA = totaleDisponibilità - getQtaItemInCart(item.ordineLine.ItemCode)
    '                    item.DISPOLOTTO = dtLotti.Rows(l).Item("DisponibilitaLotto") - getQtaLottoInCart(item.ordineLine.ItemCode, item.ordineLine.LotNo)
    '                ElseIf Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
    '                    item.DISPONIBILITA = totaleDisponibilità
    '                    item.DISPOLOTTO = dtLotti.Rows(l).Item("DisponibilitaLotto")
    '                End If
    '            End If
    '        Next
    '        If item.ordineLine.LotNo = "" Then 'archeopatico
    '            item.DISPONIBILITA = totaleDisponibilità
    '            item.DISPOLOTTO = -(item.ordineLine.Quantity + item.ordineLine.DiscountQty)
    '        End If
    '    Next
    '    dm.salvaDispo(Me)
    '    dm = Nothing
    'End Sub

    Public Sub refreshDispo()
        If carrelloLines.Count > 0 Then
            Dim dm As New datamanager
            Dim dtDispoTot As DataTable = dm.GetDisponibilitaProdotti2(carrelloLines)
            Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto2(carrelloLines)
            For Each item In carrelloLines
                Dim disponibilitaTotale As Integer = dtDispoTot.Select("[Item No_]='" + item.ordineLine.ItemCode + "'")(0).Item("Disponibilita")
                Dim disponibilitaLotto As Integer = 0
                If item.ordineLine.LotNo <> "" Then
                    If dtLotti.Select("[Item No_]='" + item.ordineLine.ItemCode + "' AND Lotto='" + item.ordineLine.LotNo + "'").Count > 0 Then
                        disponibilitaLotto = dtLotti.Select("[Item No_]='" + item.ordineLine.ItemCode + "' AND Lotto='" + item.ordineLine.LotNo + "'")(0).Item("DisponibilitaLotto")
                    Else
                        disponibilitaLotto = 0
                    End If
                Else
                    disponibilitaLotto = disponibilitaTotale
                End If
                If Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                    item.DISPONIBILITA = disponibilitaTotale - getQtaItemInCart(item.ordineLine.ItemCode)
                    item.DISPOLOTTO = disponibilitaLotto - getQtaLottoInCart(item.ordineLine.ItemCode, item.ordineLine.LotNo)
                ElseIf Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
                    item.DISPONIBILITA = disponibilitaTotale
                    item.DISPOLOTTO = disponibilitaLotto
                End If
                If item.ordineLine.LotNo = "" Then 'archeopatico
                    item.DISPONIBILITA = disponibilitaTotale
                    item.DISPOLOTTO = -(item.ordineLine.Quantity + item.ordineLine.DiscountQty)
                End If
            Next
            dm.salvaDispo(Me)
            dm = Nothing
        End If
    End Sub

    Public Function addLine(ByVal ItemCode As String, Optional ByVal Quantity2 As Integer = 1, Optional ByVal LotNo As String = "",
                            Optional ByVal fromcartpage As Boolean = False, Optional ByVal forzaQuantity As Boolean = False,
                            Optional ByVal rowDiscount As Integer = 0, Optional ByVal forzaNewRow As Boolean = False,
                            Optional ByVal ordercode As String = "", Optional ByVal IDPROMO As Integer = 0, Optional ByVal scontopercentualeString As String = "",
                            Optional ByVal salvaCarrelloOption As Boolean = True, Optional ByVal refreshDispoOption As Boolean = True) As List(Of carrelloLine)
        Dim dm As New datamanager
        If Not ItemCode Is Nothing AndAlso ItemCode <> "" Then

            '20200226 - gestione sconto pagamento su righe
            Dim scontopercentuale As Integer
            If scontopercentualeString = "0" Or scontopercentualeString = "100" Or scontopercentualeString = "" Then
                scontopercentuale = 0
            Else
                If IsNumeric(scontopercentualeString) AndAlso Convert.ToInt32(scontopercentualeString) > 0 Then
                    scontopercentuale = Convert.ToInt32(scontopercentualeString)
                End If
            End If

            'controllo se esiste già nel carrello il prodotto
            Dim esiste As Boolean = False
            Dim lottoImp As String = ""
            Dim Quantity As Integer = 0

            Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(ItemCode)
            Dim nrLotti As Integer = dtLotti.Rows.Count

            Dim isArcheopatico As Boolean = dm.isArcheopatico(ItemCode)
            Dim totaleDisponibilità As Integer = 0
            totaleDisponibilità = dm.GetDisponibilitaProdotti(ItemCode, True).Rows(0).Item("Disponibilita")

            'aggiorno disponibilità lotti in base agli articoli inseriti nel carrello
            For Each item In carrelloLines
                For l = 0 To nrLotti - 1
                    If item.ordineLine.ItemCode = dtLotti.Rows(l).Item("Item No_") And item.ordineLine.LotNo = dtLotti.Rows(l).Item("Lotto") Then
                        If Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                            If Not fromcartpage Then
                                dtLotti.Rows(l).Item("DisponibilitaLotto") = dtLotti.Rows(l).Item("DisponibilitaLotto") - item.ordineLine.Quantity
                                totaleDisponibilità = totaleDisponibilità - dtLotti.Rows(l).Item("DisponibilitaLotto") - item.ordineLine.Quantity
                            End If
                        Else ' se è un ORDINE viene già considerata la sottrazione della disponibilità dalle righe 
                        End If

                    End If
                Next
            Next

            'controllo esistenza articolo nel carrello + controllo disponibilità del lotto con chiavi ITEMCODE + LOTTO + ROWDISCOUNT
            '20170121: aggiungo controllo la chiave FORMULASCONTO per permettere l'inserimento della stessa referenza con e senza sconto di riga
            '20200226 - gestione sconto pagamento su righe
            For Each item In carrelloLines
                If (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = LotNo And item.ordineLine.RowDiscount = rowDiscount And item.SCONTORIGA = scontopercentuale) _
                    Or (item.ordineLine.ItemCode = ItemCode And LotNo = "" And item.ordineLine.RowDiscount = rowDiscount And item.SCONTORIGA = scontopercentuale) Then
                    If dtLotti.Rows.Count > 0 AndAlso dtLotti.Select("Lotto='" & item.ordineLine.LotNo & "'").Count > 0 _
                        AndAlso (dtLotti.Select("Lotto='" & item.ordineLine.LotNo & "'")(0).Item("DisponibilitaLotto") > 0) Then
                        esiste = True
                        If LotNo = "" Then
                            lottoImp = item.ordineLine.LotNo
                        Else
                            lottoImp = LotNo
                        End If
                        Exit For
                    End If
                End If
            Next
            'ulteriore controllo per presenza archeopatico
            If isArcheopatico AndAlso totaleDisponibilità <= 0 Then
                For Each item In carrelloLines
                    If (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = rowDiscount) Then
                        esiste = True
                        Exit For
                    End If
                Next
            End If

            If (esiste And Not forzaNewRow) Then
                updateLine(ItemCode, Quantity2, lottoImp, fromcartpage, forzaQuantity, rowDiscount, scontopercentualeString, IDPROMO, salvaCarrelloOption, refreshDispoOption)
            Else
                Dim LineIDmax As Integer = 0
                If carrelloLines.Count > 0 Then
                    LineIDmax = carrelloLines.Max(Function(linea As carrelloLine) linea.IDCARTLINE)
                End If
                Dim LineID As Integer = LineIDmax + 1

                Dim dr As DataRow = dm.GetWOItems(ItemCode, , False, ,,,,,,, Me.Header.CODICELISTINO).Rows(0)
                If LotNo <> "" Then
                    Dim newCarrelloLine As New carrelloLine
                    newCarrelloLine.IDCART = Header.IDCART
                    newCarrelloLine.IDCARTLINE = LineID
                    newCarrelloLine.ordineLine.ItemCode = dr("CodiceArticolo")
                    newCarrelloLine.DESCRIZIONE = dr("Descrizione")
                    newCarrelloLine.DISPONIBILITA = dr("Disponibilita")
                    newCarrelloLine.UNITPRICELIST = dr("PrzPubblico")
                    newCarrelloLine.IVA = dr("Iva")
                    newCarrelloLine.MARCHIO = dr("Marchio")
                    newCarrelloLine.COMPOSIZIONE = dr("Composizione")
                    newCarrelloLine.FORMATO = dr("Formato")
                    newCarrelloLine.ordineLine.UnitPrice = dr("PrzRivenditore")
                    newCarrelloLine.ordineLine.LotNo = LotNo
                    If dtLotti.Rows.Count > 0 AndAlso dtLotti.Select("Lotto='" & LotNo & "'").Count > 0 Then
                        newCarrelloLine.DISPOLOTTO = dtLotti.Select("Lotto='" & LotNo & "'")(0).Item("DisponibilitaLotto")
                        If newCarrelloLine.DISPOLOTTO >= Quantity2 Then
                            newCarrelloLine.ordineLine.Quantity = Quantity2
                        Else
                            newCarrelloLine.ordineLine.Quantity = newCarrelloLine.DISPOLOTTO
                        End If
                        newCarrelloLine.ordineLine.RowDiscount = rowDiscount
                        newCarrelloLine.ordineLine.OriginalQty = newCarrelloLine.ordineLine.Quantity
                    Else
                        newCarrelloLine.DISPOLOTTO = 0
                        newCarrelloLine.ordineLine.Quantity = 0
                        newCarrelloLine.ordineLine.RowDiscount = 0
                        newCarrelloLine.ordineLine.OriginalQty = 0
                        newCarrelloLine.ordineLine.DiscountQty = 0
                    End If
                    newCarrelloLine.TOTALERIGA = newCarrelloLine.ordineLine.UnitPrice * newCarrelloLine.ordineLine.Quantity
                    If rowDiscount = 1 Then
                        newCarrelloLine.ordineLine.DiscountQty = newCarrelloLine.ordineLine.Quantity
                        newCarrelloLine.ordineLine.Quantity = 0
                        newCarrelloLine.TOTALERIGA = 0
                    End If
                    If IDPROMO > 0 Then
                        newCarrelloLine.IDPROMO = IDPROMO
                        Me.addPromoLine(New carrelloLinePromo With {.IDCART = newCarrelloLine.IDCART, .IDCARTLINE = newCarrelloLine.IDCARTLINE, .IDPROMO = newCarrelloLine.IDPROMO, .CTCODE = Me.Header.CODICECONTATTO, .NRMULTIPLI = 1})
                    End If

                    '20200226 - gestione sconto pagamento su righe
                    newCarrelloLine.SCONTORIGA = scontopercentuale
                    '20200424 - Gestione scontoScaglioni
                    If (scontoHeaderApplicabile(newCarrelloLine)) Then
                        newCarrelloLine.SCONTOHEADER = Me.Header.SCONTOHEADER
                    Else
                        newCarrelloLine.SCONTOHEADER = 0
                    End If
                    If (scontoPagamentoApplicabile(newCarrelloLine)) Then
                        newCarrelloLine.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                    Else
                        newCarrelloLine.SCONTOPAGAMENTO = 0
                    End If
                    newCarrelloLine.ordineLine.FormulaSconto = encodeFormulaSconto(newCarrelloLine.SCONTORIGA, newCarrelloLine.SCONTOHEADER, newCarrelloLine.SCONTOPAGAMENTO)
                    '--------------------------------------------------

                    newCarrelloLine.TOTALERIGA = calcolaTotaleRiga(newCarrelloLine)
                    newCarrelloLine.ordineLine.LineID = LineID
                    newCarrelloLine.ordineLine.LineNo = LineID * 10000
                    newCarrelloLine.ordineLine.OrderCode = Me.Header.ordineHeader.Code
                    newCarrelloLine.ordineLine.CompanyName = dm.GetWorkingCompany
                    newCarrelloLine.ordineLine.BinCode = dm.GetDefaultBinCodeByItemcode(dr("CodiceArticolo"), dm.GetParametroSito(parametriSitoValue.magazzinodefault))
                    carrelloLines.Add(newCarrelloLine)
                Else
                    Dim QuantityINlotti As Integer = Quantity2
                    If isArcheopatico AndAlso totaleDisponibilità <= 0 Then
                        Dim newCarrelloLine As New carrelloLine
                        newCarrelloLine.IDCART = Header.IDCART
                        newCarrelloLine.IDCARTLINE = LineID
                        newCarrelloLine.ordineLine.ItemCode = dr("CodiceArticolo")
                        newCarrelloLine.DESCRIZIONE = dr("Descrizione")
                        newCarrelloLine.DISPONIBILITA = dr("Disponibilita")
                        newCarrelloLine.UNITPRICELIST = dr("PrzPubblico")
                        newCarrelloLine.IVA = dr("Iva")
                        newCarrelloLine.MARCHIO = dr("Marchio")
                        newCarrelloLine.COMPOSIZIONE = dr("Composizione")
                        newCarrelloLine.FORMATO = dr("Formato")
                        newCarrelloLine.ordineLine.UnitPrice = dr("PrzRivenditore")
                        newCarrelloLine.ordineLine.LotNo = LotNo
                        newCarrelloLine.DISPOLOTTO = 0
                        newCarrelloLine.ordineLine.Quantity = Quantity2
                        newCarrelloLine.ordineLine.OriginalQty = newCarrelloLine.ordineLine.Quantity
                        newCarrelloLine.ordineLine.RowDiscount = rowDiscount
                        newCarrelloLine.TOTALERIGA = newCarrelloLine.ordineLine.UnitPrice * newCarrelloLine.ordineLine.Quantity
                        If rowDiscount = 1 Then
                            newCarrelloLine.ordineLine.DiscountQty = newCarrelloLine.ordineLine.Quantity
                            newCarrelloLine.TOTALERIGA = 0
                            newCarrelloLine.ordineLine.Quantity = 0
                        End If
                        If IDPROMO > 0 Then
                            newCarrelloLine.IDPROMO = IDPROMO
                            Me.addPromoLine(New carrelloLinePromo With {.IDCART = newCarrelloLine.IDCART, .IDCARTLINE = newCarrelloLine.IDCARTLINE, .IDPROMO = newCarrelloLine.IDPROMO, .CTCODE = Me.Header.CODICECONTATTO, .NRMULTIPLI = 1})
                        End If

                        '20200226 - gestione sconto pagamento su righe
                        newCarrelloLine.SCONTORIGA = scontopercentuale
                        '20200424 - Gestione scontoScaglioni
                        If (scontoHeaderApplicabile(newCarrelloLine)) Then
                            newCarrelloLine.SCONTOHEADER = Me.Header.SCONTOHEADER
                        Else
                            newCarrelloLine.SCONTOHEADER = 0
                        End If
                        If (scontoPagamentoApplicabile(newCarrelloLine)) Then
                            newCarrelloLine.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                        Else
                            newCarrelloLine.SCONTOPAGAMENTO = 0
                        End If
                        newCarrelloLine.ordineLine.FormulaSconto = encodeFormulaSconto(newCarrelloLine.SCONTORIGA, newCarrelloLine.SCONTOHEADER, newCarrelloLine.SCONTOPAGAMENTO)
                        '--------------------------------------------------

                        newCarrelloLine.TOTALERIGA = calcolaTotaleRiga(newCarrelloLine)
                        newCarrelloLine.ordineLine.LineID = LineID
                        newCarrelloLine.ordineLine.LineNo = LineID * 10000
                        newCarrelloLine.ordineLine.OrderCode = Me.Header.ordineHeader.Code
                        newCarrelloLine.ordineLine.CompanyName = dm.GetWorkingCompany
                        newCarrelloLine.ordineLine.BinCode = ""
                        carrelloLines.Add(newCarrelloLine)
                    Else
                        For l = 0 To nrLotti - 1
                            If QuantityINlotti > 0 Then
                                If carrelloLines.Count > 0 Then
                                    LineIDmax = carrelloLines.Max(Function(linea As carrelloLine) linea.IDCARTLINE)
                                End If
                                LineID = LineIDmax + 1
                                If dtLotti.Rows(l).Item("DisponibilitaLotto") >= QuantityINlotti Then
                                    Dim newCarrelloLine As New carrelloLine
                                    newCarrelloLine.IDCART = Header.IDCART
                                    newCarrelloLine.IDCARTLINE = LineID
                                    newCarrelloLine.ordineLine.ItemCode = dr("CodiceArticolo")
                                    newCarrelloLine.DESCRIZIONE = dr("Descrizione")
                                    newCarrelloLine.DISPONIBILITA = dr("Disponibilita")
                                    newCarrelloLine.UNITPRICELIST = dr("PrzPubblico")
                                    newCarrelloLine.IVA = dr("Iva")
                                    newCarrelloLine.MARCHIO = dr("Marchio")
                                    newCarrelloLine.COMPOSIZIONE = dr("Composizione")
                                    newCarrelloLine.FORMATO = dr("Formato")
                                    newCarrelloLine.ordineLine.UnitPrice = dr("PrzRivenditore")
                                    newCarrelloLine.ordineLine.Quantity = QuantityINlotti
                                    newCarrelloLine.ordineLine.OriginalQty = newCarrelloLine.ordineLine.Quantity
                                    newCarrelloLine.ordineLine.LotNo = dtLotti.Rows(l).Item("Lotto")
                                    newCarrelloLine.DISPOLOTTO = dtLotti.Rows(l).Item("DisponibilitaLotto")
                                    newCarrelloLine.TOTALERIGA = newCarrelloLine.ordineLine.UnitPrice * newCarrelloLine.ordineLine.Quantity
                                    newCarrelloLine.ordineLine.RowDiscount = rowDiscount
                                    If rowDiscount = 1 Then
                                        newCarrelloLine.ordineLine.DiscountQty = newCarrelloLine.ordineLine.Quantity
                                        newCarrelloLine.ordineLine.Quantity = 0
                                        newCarrelloLine.TOTALERIGA = 0
                                    End If
                                    If IDPROMO > 0 Then
                                        newCarrelloLine.IDPROMO = IDPROMO
                                        Me.addPromoLine(New carrelloLinePromo With {.IDCART = newCarrelloLine.IDCART, .IDCARTLINE = newCarrelloLine.IDCARTLINE, .IDPROMO = newCarrelloLine.IDPROMO, .CTCODE = Me.Header.CODICECONTATTO, .NRMULTIPLI = 1})
                                    End If

                                    '20200226 - gestione sconto pagamento su righe
                                    newCarrelloLine.SCONTORIGA = scontopercentuale
                                    '20200424 - Gestione scontoScaglioni
                                    If (scontoHeaderApplicabile(newCarrelloLine)) Then
                                        newCarrelloLine.SCONTOHEADER = Me.Header.SCONTOHEADER
                                    Else
                                        newCarrelloLine.SCONTOHEADER = 0
                                    End If
                                    If (scontoPagamentoApplicabile(newCarrelloLine)) Then
                                        newCarrelloLine.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                                    Else
                                        newCarrelloLine.SCONTOPAGAMENTO = 0
                                    End If
                                    newCarrelloLine.ordineLine.FormulaSconto = encodeFormulaSconto(newCarrelloLine.SCONTORIGA, newCarrelloLine.SCONTOHEADER, newCarrelloLine.SCONTOPAGAMENTO)
                                    '--------------------------------------------------

                                    newCarrelloLine.TOTALERIGA = calcolaTotaleRiga(newCarrelloLine)
                                    newCarrelloLine.ordineLine.LineID = LineID
                                    newCarrelloLine.ordineLine.LineNo = LineID * 10000
                                    newCarrelloLine.ordineLine.OrderCode = Me.Header.ordineHeader.Code
                                    newCarrelloLine.ordineLine.CompanyName = dm.GetWorkingCompany
                                    newCarrelloLine.ordineLine.BinCode = dm.GetDefaultBinCodeByItemcode(dr("CodiceArticolo"), dm.GetParametroSito(parametriSitoValue.magazzinodefault))
                                    carrelloLines.Add(newCarrelloLine)
                                    Exit For
                                ElseIf dtLotti.Rows(l).Item("DisponibilitaLotto") > 0 Then

                                    Dim newCarrelloLine As New carrelloLine
                                    newCarrelloLine.IDCART = Header.IDCART
                                    newCarrelloLine.IDCARTLINE = LineID
                                    newCarrelloLine.ordineLine.ItemCode = dr("CodiceArticolo")
                                    newCarrelloLine.DESCRIZIONE = dr("Descrizione")
                                    newCarrelloLine.DISPONIBILITA = dr("Disponibilita")
                                    newCarrelloLine.UNITPRICELIST = dr("PrzPubblico")
                                    newCarrelloLine.IVA = dr("Iva")
                                    newCarrelloLine.MARCHIO = dr("Marchio")
                                    newCarrelloLine.COMPOSIZIONE = dr("Composizione")
                                    newCarrelloLine.FORMATO = dr("Formato")
                                    newCarrelloLine.ordineLine.UnitPrice = dr("PrzRivenditore")
                                    newCarrelloLine.ordineLine.Quantity = dtLotti.Rows(l).Item("DisponibilitaLotto")
                                    newCarrelloLine.ordineLine.OriginalQty = newCarrelloLine.ordineLine.Quantity
                                    newCarrelloLine.ordineLine.LotNo = dtLotti.Rows(l).Item("Lotto")
                                    newCarrelloLine.DISPOLOTTO = dtLotti.Rows(l).Item("DisponibilitaLotto")
                                    newCarrelloLine.TOTALERIGA = newCarrelloLine.ordineLine.UnitPrice * newCarrelloLine.ordineLine.Quantity
                                    newCarrelloLine.ordineLine.RowDiscount = rowDiscount
                                    If rowDiscount = 1 Then
                                        newCarrelloLine.ordineLine.DiscountQty = newCarrelloLine.ordineLine.Quantity
                                        newCarrelloLine.ordineLine.Quantity = 0
                                        newCarrelloLine.TOTALERIGA = 0
                                    End If
                                    If IDPROMO > 0 Then
                                        newCarrelloLine.IDPROMO = IDPROMO
                                        Me.addPromoLine(New carrelloLinePromo With {.IDCART = newCarrelloLine.IDCART, .IDCARTLINE = newCarrelloLine.IDCARTLINE, .IDPROMO = newCarrelloLine.IDPROMO, .CTCODE = Me.Header.CODICECONTATTO, .NRMULTIPLI = 1})
                                    End If

                                    '20200226 - gestione sconto pagamento su righe
                                    newCarrelloLine.SCONTORIGA = scontopercentuale
                                    '20200424 - Gestione scontoScaglioni
                                    If (scontoHeaderApplicabile(newCarrelloLine)) Then
                                        newCarrelloLine.SCONTOHEADER = Me.Header.SCONTOHEADER
                                    Else
                                        newCarrelloLine.SCONTOHEADER = 0
                                    End If
                                    If (scontoPagamentoApplicabile(newCarrelloLine)) Then
                                        newCarrelloLine.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                                    Else
                                        newCarrelloLine.SCONTOPAGAMENTO = 0
                                    End If
                                    newCarrelloLine.ordineLine.FormulaSconto = encodeFormulaSconto(newCarrelloLine.SCONTORIGA, newCarrelloLine.SCONTOHEADER, newCarrelloLine.SCONTOPAGAMENTO)
                                    '--------------------------------------------------

                                    newCarrelloLine.TOTALERIGA = calcolaTotaleRiga(newCarrelloLine)
                                    newCarrelloLine.ordineLine.LineID = LineID
                                    newCarrelloLine.ordineLine.LineNo = LineID * 10000
                                    newCarrelloLine.ordineLine.OrderCode = Me.Header.ordineHeader.Code
                                    newCarrelloLine.ordineLine.CompanyName = dm.GetWorkingCompany
                                    newCarrelloLine.ordineLine.BinCode = dm.GetDefaultBinCodeByItemcode(dr("CodiceArticolo"), dm.GetParametroSito(parametriSitoValue.magazzinodefault))
                                    carrelloLines.Add(newCarrelloLine)
                                    QuantityINlotti = QuantityINlotti - dtLotti.Rows(l).Item("DisponibilitaLotto")
                                    If isArcheopatico AndAlso QuantityINlotti > 0 AndAlso l = nrLotti - 1 Then
                                        dm.salvaCarrello2(Me)
                                        Me.addLine(ItemCode, QuantityINlotti, , forzaQuantity, , rowDiscount, , Me.Header.ordineHeader.Code, IDPROMO, scontopercentuale, salvaCarrelloOption, refreshDispoOption)
                                        Exit For
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If

                '20200424 - Gestione scontoScaglioni
                Call refreshScontiLines()

                'rendere persistenti le operazioni su database
                If salvaCarrelloOption Then dm.salvaCarrello2(Me)
                'refresh delle disponibilità nel carrello
                If refreshDispoOption Then Call refreshDispo()
            End If
        End If
        dm = Nothing
        Return carrelloLines
    End Function

    Public Function updateLine(ByVal ItemCode As String, ByVal Quantity As Integer, Optional ByVal LotNo As String = "",
                               Optional ByVal fromcartpage As Boolean = False, Optional ByVal forzaQuantity As Boolean = False,
                               Optional ByVal RowDiscount As Integer = 0, Optional ByVal scontopercentualeString As String = "", Optional ByVal IDPROMO As Integer = 0,
                               Optional ByVal salvaCarrelloOption As Boolean = True, Optional ByVal refreshDispoOption As Boolean = True) As List(Of carrelloLine)
        If Not ItemCode Is Nothing AndAlso ItemCode <> "" AndAlso IsNumeric(Quantity) Then

            '20200226 - gestione sconto pagamento su righe
            Dim scontopercentuale As Integer
            If scontopercentualeString = "0" Or scontopercentualeString = "100" Or scontopercentualeString = "" Then
                scontopercentuale = 0
            Else
                If IsNumeric(scontopercentualeString) AndAlso Convert.ToInt32(scontopercentualeString) > 0 Then
                    scontopercentuale = Convert.ToInt32(scontopercentualeString)
                End If
            End If

            If (Quantity > 0) Then
                Dim dm As New datamanager
                Dim isArcheopatico As Boolean = dm.isArcheopatico(ItemCode)
                Dim dtLotti As DataTable = dm.GetDisponibilitaProdottiPerLotto(ItemCode)
                Dim nrLotti As Integer = dtLotti.Rows.Count


                'aggiorno disponibilità lotti in base agli articoli inseriti nel carrello
                dtLotti = dm.GetDisponibilitaProdottiPerLotto(ItemCode)
                nrLotti = dtLotti.Rows.Count
                If LotNo <> "" Then
                    For Each item In carrelloLines
                        For l = 0 To nrLotti - 1
                            If item.ordineLine.ItemCode = dtLotti.Rows(l).Item("Item No_") And item.ordineLine.LotNo = dtLotti.Rows(l).Item("Lotto") Then
                                If Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                                    If fromcartpage Then
                                        dtLotti.Rows(l).Item("DisponibilitaLotto") = dtLotti.Rows(l).Item("DisponibilitaLotto")
                                    Else
                                        dtLotti.Rows(l).Item("DisponibilitaLotto") = dtLotti.Rows(l).Item("DisponibilitaLotto") - item.ordineLine.Quantity
                                    End If
                                ElseIf Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
                                    If fromcartpage Then
                                        dtLotti.Rows(l).Item("DisponibilitaLotto") = dtLotti.Rows(l).Item("DisponibilitaLotto") + item.ordineLine.Quantity + item.ordineLine.DiscountQty
                                    End If
                                End If
                                item.DISPOLOTTO = dtLotti.Select("Lotto='" & LotNo & "'")(0).Item("DisponibilitaLotto")
                            End If
                        Next
                    Next
                End If

                '----------- refresh della disponibilità totale REALE dell'articolo
                For Each item As carrelloLine In carrelloLines
                    Dim dr As DataRow = dm.GetWOItems(ItemCode,  , True,,,,,,,, Me.Header.CODICELISTINO).Rows(0)
                    If item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = LotNo Then
                        item.DISPONIBILITA = dr("Disponibilita")
                        If Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                            If fromcartpage Then
                                item.DISPONIBILITA = item.DISPONIBILITA - Quantity
                            Else
                                item.DISPONIBILITA = item.DISPONIBILITA - (getQtaItemInCart(ItemCode) + Quantity)
                            End If
                        ElseIf Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
                            If fromcartpage Then
                                item.DISPONIBILITA = item.DISPONIBILITA + getQtaItemInCart(ItemCode) - Quantity
                            Else
                                ' item.DISPONIBILITA = item.DISPONIBILITA - Quantity
                            End If
                        End If
                    End If
                    If item.ordineLine.LotNo = "" And isArcheopatico Then 'archeopatico
                        If Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.CARRELLO Then
                            item.DISPONIBILITA = 0
                        ElseIf Me.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE Then
                            item.DISPONIBILITA = dr("Disponibilita")
                        End If
                    End If
                Next
                '-----------

                Dim ef As Boolean = False
                For Each item As carrelloLine In carrelloLines

                    Dim dispolottoreale As Integer = 0
                    If LotNo <> "" Then
                        dispolottoreale = dtLotti.Select("Lotto='" & LotNo & "'")(0).Item("DisponibilitaLotto")
                    End If

                    If item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = LotNo And item.ordineLine.RowDiscount = RowDiscount And item.SCONTORIGA = scontopercentuale Then
                        If dispolottoreale >= Quantity Or (isArcheopatico AndAlso item.DISPONIBILITA <= 0 And dispolottoreale <= 0) Then
                            If Not fromcartpage Then
                                item.ordineLine.Quantity = item.ordineLine.Quantity + Quantity
                            Else
                                item.ordineLine.Quantity = Quantity
                            End If

                            item.ordineLine.OriginalQty = item.ordineLine.Quantity
                            item.TOTALERIGA = item.ordineLine.UnitPrice * item.ordineLine.Quantity

                            If RowDiscount = 1 Then
                                If Not fromcartpage Then
                                    item.ordineLine.DiscountQty = item.ordineLine.DiscountQty + item.ordineLine.Quantity
                                Else
                                    item.ordineLine.DiscountQty = item.ordineLine.Quantity
                                End If
                                item.ordineLine.OriginalQty = item.ordineLine.DiscountQty
                                item.TOTALERIGA = 0
                                item.ordineLine.Quantity = 0
                            End If

                            '20200226 - gestione sconto pagamento su righe
                            item.SCONTORIGA = scontopercentuale
                            '20200424 - Gestione scontoScaglioni
                            If (scontoHeaderApplicabile(item)) Then
                                item.SCONTOHEADER = Me.Header.SCONTOHEADER
                            Else
                                item.SCONTOHEADER = 0
                            End If
                            If (scontoPagamentoApplicabile(item)) Then
                                item.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                            Else
                                item.SCONTOPAGAMENTO = 0
                            End If
                            item.ordineLine.FormulaSconto = encodeFormulaSconto(item.SCONTORIGA, item.SCONTOHEADER, item.SCONTOPAGAMENTO)
                            '--------------------------------------------------

                            item.TOTALERIGA = calcolaTotaleRiga(item)

                            If IDPROMO > 0 Then
                                Me.addPromoLine(New carrelloLinePromo With {.IDCART = item.IDCART, .IDCARTLINE = item.IDCARTLINE, .IDPROMO = IDPROMO, .CTCODE = Me.Header.CODICECONTATTO, .NRMULTIPLI = 1})
                            End If
                            Exit For
                        Else
                            'esaurisco il lotto di riga
                            If LotNo <> "" Then
                                dtLotti.Select("Lotto='" & LotNo & "'")(0).Item("DisponibilitaLotto") = 0
                            End If

                            Dim QuantityInLotto As Integer = IIf(isArcheopatico, 0, dispolottoreale)

                            item.ordineLine.Quantity = item.ordineLine.Quantity + QuantityInLotto
                            item.ordineLine.OriginalQty = item.ordineLine.Quantity
                            item.TOTALERIGA = item.ordineLine.UnitPrice * item.ordineLine.Quantity

                            If RowDiscount = 1 Then
                                item.TOTALERIGA = 0
                                item.ordineLine.DiscountQty = item.ordineLine.Quantity
                                item.ordineLine.Quantity = 0
                            End If

                            '20200226 - gestione sconto pagamento su righe
                            item.SCONTORIGA = scontopercentuale
                            '20200424 - Gestione scontoScaglioni
                            If (scontoHeaderApplicabile(item)) Then
                                item.SCONTOHEADER = Me.Header.SCONTOHEADER
                            Else
                                item.SCONTOHEADER = 0
                            End If
                            If (scontoPagamentoApplicabile(item)) Then
                                item.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                            Else
                                item.SCONTOPAGAMENTO = 0
                            End If
                            item.ordineLine.FormulaSconto = encodeFormulaSconto(item.SCONTORIGA, item.SCONTOHEADER, item.SCONTOPAGAMENTO)
                            '--------------------------------------------------

                            item.TOTALERIGA = calcolaTotaleRiga(item)

                            'inserisco qta mancante su lotto successivo
                            Dim qtaMancante As Integer = Quantity - dispolottoreale
                            For l = 0 To nrLotti - 1
                                If qtaMancante > 0 Then
                                    If dtLotti.Rows(l).Item("DisponibilitaLotto") > 0 Then
                                        If dtLotti.Rows(l).Item("DisponibilitaLotto") > getQtaLottoInCart(ItemCode, dtLotti.Rows(l).Item("Lotto")) Then
                                            Me.addLine(ItemCode, qtaMancante, dtLotti.Rows(l).Item("Lotto"), forzaQuantity, , RowDiscount, , item.ordineLine.OrderCode, item.IDPROMO, scontopercentualeString, salvaCarrelloOption, refreshDispoOption)
                                            ef = True
                                            Exit For
                                        End If
                                    End If
                                Else
                                    Exit For
                                End If
                            Next
                            If Not ef AndAlso isArcheopatico Then
                                dm.salvaCarrello2(Me)
                                Me.addLine(ItemCode, qtaMancante, , forzaQuantity, , RowDiscount, , item.ordineLine.OrderCode, item.IDPROMO, scontopercentualeString, salvaCarrelloOption, refreshDispoOption)
                                ef = True
                                Exit For
                            End If
                        End If
                    End If
                    If ef Then Exit For
                Next

                '20200424 - Gestione scontoScaglioni
                Call refreshScontiLines()

                'rendere persistenti le operazioni su database
                dm.salvaCarrello2(Me)
                'refresh delle disponibilità nel carrello
                Call refreshDispo()

                dm = Nothing
            Else
                deleteLine(ItemCode, LotNo, RowDiscount, -1, scontopercentuale)
            End If
        End If
        Return carrelloLines
    End Function

    ''' <summary>
    ''' Elimina una riga dal carrello
    ''' </summary>
    ''' <param name="ItemCode">Codice articolo</param>
    ''' <param name="lotto">Lotto [= "" per eliminare tutti gli articoli indipendentemente dal lotto]</param>
    ''' <param name="RowDiscount">Riga omaggio</param>
    ''' <param name="IDPROMO">Promo associata alla riga</param>
    ''' <param name="scontopercentualeString">per eliminare solo la riga con formulasconto</param>
    ''' <returns></returns>
    Public Function deleteLine(ByVal ItemCode As String, ByVal lotto As String, Optional ByVal RowDiscount As Integer = -1,
                               Optional ByVal IDPROMO As Integer = -1, Optional ByVal scontopercentualeString As String = "",
                               Optional ByVal salvaCarrelloOption As Boolean = True, Optional ByVal refreshDispoOption As Boolean = True) As List(Of carrelloLine)
        Dim dm As New datamanager

        '20200226 - gestione sconto pagamento su righe
        Dim scontopercentuale As Integer
        If scontopercentualeString = "0" Or scontopercentualeString = "100" Or scontopercentualeString = "" Then
            scontopercentuale = -1
        Else
            If IsNumeric(scontopercentualeString) AndAlso Convert.ToInt32(scontopercentualeString) > 0 Then
                scontopercentuale = Convert.ToInt32(scontopercentualeString)
            End If
        End If

        'If scontopercentuale = "" Or scontopercentuale = "0" Then
        '    scontopercentuale = "-1"
        'End If
        'If scontopercentuale = "100" Then '100% è gestito con rowdiscount
        '    scontopercentuale = "-1"
        'End If
        If lotto = "-1" Then 'gestione eliminazione da productfinder -> vengono eliminati tutte le line di quella referenza e relative promo utilizzate
            Dim cl As List(Of carrelloLine) = Me.carrelloLines.FindAll(Function(item) (item.ordineLine.ItemCode = ItemCode))
            For Each l In cl
                If l.IDPROMO > 0 Then
                    Me.delPromoLine(New carrelloLinePromo With {.IDCART = l.IDCART, .IDPROMO = l.IDPROMO})
                End If
            Next
            carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode))
        End If
        '20200226 - gestione sconto pagamento su righe
        If lotto <> "" Then
            If (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And scontopercentuale > 0 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO And item.SCONTORIGA = scontopercentuale))
            ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And scontopercentuale = -1 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO))
            ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And scontopercentuale > 0 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount And item.SCONTORIGA = scontopercentuale))
            ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And scontopercentuale = -1 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount))
            ElseIf RowDiscount = -1 And IDPROMO = -1 And scontopercentuale > 0 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.SCONTORIGA = scontopercentuale))
            ElseIf RowDiscount = -1 And IDPROMO = -1 And scontopercentuale = -1 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto))
            End If
        ElseIf lotto = "" Then
            If (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And scontopercentuale > 0 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO And item.SCONTORIGA = scontopercentuale))
            ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And scontopercentuale = -1 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO))
            ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And scontopercentuale > 0 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount And item.SCONTORIGA = scontopercentuale))
            ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And scontopercentuale = -1 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount))
            ElseIf RowDiscount = -1 And IDPROMO = -1 And scontopercentuale > 0 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.SCONTORIGA = scontopercentuale))
            ElseIf RowDiscount = -1 And IDPROMO = -1 And scontopercentuale = -1 Then
                carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = ""))
            End If
        End If

        'If lotto <> "" Then
        '    If (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And (IsNumeric(scontopercentuale) AndAlso Convert.ToInt32(scontopercentuale) > 0) Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO And getScontoRigaFromFormulaSconto(item) = scontopercentuale))
        '    ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And scontopercentuale = "-1" Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO))
        '    ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And (IsNumeric(scontopercentuale) AndAlso Convert.ToInt32(scontopercentuale) > 0) Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount And getScontoRigaFromFormulaSconto(item) = scontopercentuale))
        '    ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And scontopercentuale = "-1" Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And item.ordineLine.RowDiscount = RowDiscount))
        '    ElseIf RowDiscount = -1 And IDPROMO = -1 And (IsNumeric(scontopercentuale) AndAlso Convert.ToInt32(scontopercentuale) > 0) Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto And getScontoRigaFromFormulaSconto(item) = scontopercentuale))
        '    ElseIf RowDiscount = -1 And IDPROMO = -1 And scontopercentuale = "-1" Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = lotto))
        '    End If
        'ElseIf lotto = "" Then
        '    If (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And (IsNumeric(scontopercentuale) AndAlso Convert.ToInt32(scontopercentuale) > 0) Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO And getScontoRigaFromFormulaSconto(item) = scontopercentuale))
        '    ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO >= 0 And scontopercentuale = "-1" Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount And item.IDPROMO = IDPROMO))
        '    ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And (IsNumeric(scontopercentuale) AndAlso Convert.ToInt32(scontopercentuale) > 0) Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount And getScontoRigaFromFormulaSconto(item) = scontopercentuale))
        '    ElseIf (RowDiscount = 0 Or RowDiscount = 1) And IDPROMO = -1 And scontopercentuale = "-1" Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And item.ordineLine.RowDiscount = RowDiscount))
        '    ElseIf RowDiscount = -1 And IDPROMO = -1 And (IsNumeric(scontopercentuale) AndAlso Convert.ToInt32(scontopercentuale) > 0) Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = "" And getScontoRigaFromFormulaSconto(item) = scontopercentuale))
        '    ElseIf RowDiscount = -1 And IDPROMO = -1 And scontopercentuale = "-1" Then
        '        carrelloLines.RemoveAll(Function(item) (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = ""))
        '    End If
        'End If

        If IDPROMO > 0 Then 'check promo nel carrello
            If Me.carrelloLinePromos.FindAll(Function(item) (item.IDCART = Me.Header.IDCART And item.IDPROMO = IDPROMO)).Count > 0 Then
                Me.delPromoLine(New carrelloLinePromo With {.IDCART = Me.Header.IDCART, .IDPROMO = IDPROMO})
            End If
        End If

        '20200424 - Gestione scontoScaglioni
        Call refreshScontiLines()

        'rendere persistenti le operazioni su database
        If salvaCarrelloOption Then
            dm.salvaCarrello2(Me)
        End If
        'refresh delle disponibilità nel carrello
        If refreshDispoOption Then
            Call refreshDispo()
        End If

        dm = Nothing
        Return carrelloLines
    End Function

    Public Sub updateScontoPercentuale(ByVal LineNo As Integer, ByVal scontopercentualeString As String)
        Dim scontoHeader As String = Me.Header.SCONTOHEADER
        Dim scontoPagamento As Integer = Me.Header.SCONTOPAGAMENTO

        '20200226 - gestione sconto pagamento su righe
        Dim scontopercentuale As Integer
        If scontopercentualeString = "0" Or scontopercentualeString = "100" Or scontopercentualeString = "" Then
            scontopercentuale = 0
        Else
            If IsNumeric(scontopercentualeString) AndAlso Convert.ToInt32(scontopercentualeString) > 0 Then
                scontopercentuale = Convert.ToInt32(scontopercentualeString)
            End If
        End If

        For Each item As carrelloLine In carrelloLines
            If item.ordineLine.LineNo = LineNo Then
                item.SCONTORIGA = scontopercentuale
                item.ordineLine.FormulaSconto = encodeFormulaSconto(scontopercentuale, scontoHeader, scontoPagamento)
            End If
        Next
    End Sub

    Public Sub deleteLineByIdcartLine(ByVal idcart As Int64, ByVal idcartline As Int64)
        carrelloLines.RemoveAll(Function(item) (item.IDCART = idcart And item.IDCARTLINE = idcartline))
    End Sub

    Public Function getTotaleQta() As String
        Dim totqta As Integer = 0
        For Each item As carrelloLine In carrelloLines
            totqta = totqta + item.ordineLine.Quantity + item.ordineLine.DiscountQty
        Next
        Return totqta.ToString
    End Function

    Public Function getTotaleQtaOmaggio() As String
        Dim totqta As Integer = 0
        For Each item As carrelloLine In carrelloLines
            If item.ordineLine.RowDiscount = 1 Then
                totqta = totqta + item.ordineLine.DiscountQty
            End If
        Next

        Return totqta.ToString
    End Function

    Public Function GetTotaleMerce() As Double
        Dim totale As Decimal = 0
        For Each item As carrelloLine In carrelloLines
            totale = totale + item.TOTALERIGA
        Next
        Return totale
    End Function

    Public Function GetTotaleMerceConScontoPagamento() As Double
        Dim totale As Decimal = 0
        '20200226 - gestione sconto pagamento su righe
        For Each l As carrelloLine In carrelloLines
            Dim totaleriga As Double = l.ordineLine.UnitPrice * l.ordineLine.Quantity
            totale = totale + (totaleriga - (totaleriga / 100 * l.SCONTOPAGAMENTO))
        Next
        Return totale
    End Function

    Public Function getQtaLottoInCart(ByVal prodID As String, ByVal Lotto As String) As Integer
        Dim qtaLottoArticolo As Integer = 0
        For Each item As carrelloLine In carrelloLines
            If item.ordineLine.ItemCode = prodID And item.ordineLine.LotNo = Lotto Then
                qtaLottoArticolo = qtaLottoArticolo + item.ordineLine.Quantity + item.ordineLine.DiscountQty
            End If
        Next
        Return qtaLottoArticolo
    End Function

    Public Function getQtaItemInCart(ByVal prodID As String) As Integer
        Dim qtaArticolo As Integer = 0
        For Each item As carrelloLine In carrelloLines
            If item.ordineLine.ItemCode = prodID Then
                qtaArticolo = qtaArticolo + item.ordineLine.Quantity + item.ordineLine.DiscountQty
            End If
        Next
        Return qtaArticolo
    End Function

    Public Function addPromoHeader(cp As carrelloHeaderPromo) As Integer
        Dim dm As New datamanager
        If Me.carrelloHeaderPromos.FindAll(Function(item) (item.IDCART = cp.IDCART And item.IDPROMO = cp.IDPROMO)).Count > 0 Then
            Dim cph As carrelloHeaderPromo = Me.carrelloHeaderPromos.FindAll(Function(item) (item.IDCART = cp.IDCART And item.IDPROMO = cp.IDPROMO)).First
            If Not cph Is Nothing Then
                cph.NRUTILIZZI += 1
            End If
        Else
            cp.NRUTILIZZI = 1
            Me.carrelloHeaderPromos.Add(cp)
        End If
        Return dm.addCarrelloPromo(cp)
        dm = Nothing
    End Function

    Public Function delPromoHeader(cp As carrelloHeaderPromo) As Integer
        Dim dm As New datamanager
        Me.carrelloHeaderPromos.RemoveAll(Function(item) (item.IDCART = cp.IDCART And item.IDPROMO = cp.IDPROMO))
        Return dm.delCarrelloPromo(cp)
        dm = Nothing
    End Function

    Public Function addPromoLine(cpl As carrelloLinePromo) As Integer
        Dim dm As New datamanager
        If Me.carrelloLinePromos.FindAll(Function(item) (item.IDCART = cpl.IDCART And item.IDCARTLINE = cpl.IDCARTLINE And item.IDPROMO = cpl.IDPROMO)).Count > 0 Then
            Dim foundcpl As carrelloLinePromo = Me.carrelloLinePromos.FindAll(Function(item) (item.IDCART = cpl.IDCART And item.IDCARTLINE = cpl.IDCARTLINE And item.IDPROMO = cpl.IDPROMO)).First
            If Not foundcpl Is Nothing Then
                foundcpl.NRMULTIPLI += 1
            End If
        Else
            cpl.NRMULTIPLI = 1
            Me.carrelloLinePromos.Add(cpl)
        End If
        Return dm.addCarrelloPromoLines(cpl)
        dm = Nothing
    End Function

    Public Function delPromoLine(cpl As carrelloLinePromo) As Integer
        For Each item As carrelloLine In carrelloLines
            If item.IDPROMO = cpl.IDPROMO Then
                item.IDPROMO = 0
            End If
        Next
        Dim dm As New datamanager
        Me.carrelloLinePromos.RemoveAll(Function(item) (item.IDCART = cpl.IDCART And item.IDPROMO = cpl.IDPROMO))
        Return dm.delCarrelloPromoLines(cpl)
        dm = Nothing
    End Function

    Public Function applicaScontoHeader(scontoHeader As String, Optional ID As Integer = 0, Optional action As String = "") As cartManager
        Me.Header.SCONTOHEADER = scontoHeader
        Dim scontoPagamento As Integer = Me.Header.SCONTOPAGAMENTO
        For Each l As carrelloLine In Me.carrelloLines
            '20200424 - Gestione scontoScaglioni
            If scontoHeaderApplicabile(l) Then
                l.SCONTOHEADER = scontoHeader.Replace(".", ",")
            Else
                l.SCONTOHEADER = 0
            End If
            '20200226 - gestione sconto pagamento su righe
            If scontoPagamentoApplicabile(l) Then
                l.SCONTOPAGAMENTO = scontoPagamento
            Else
                l.SCONTOPAGAMENTO = 0
            End If
            l.ordineLine.FormulaSconto = encodeFormulaSconto(l.SCONTORIGA, l.SCONTOHEADER, l.SCONTOPAGAMENTO)
            l.TOTALERIGA = calcolaTotaleRiga(l)
        Next
        Dim dm As New datamanager
        dm.salvaCarrello2(Me)
        dm = Nothing
        If ID > 0 Then
            addPromoHeader(New carrelloHeaderPromo With {.IDCART = Me.Header.IDCART, .CTCODE = Me.Header.CODICECONTATTO, .IDPROMO = ID, .NRUTILIZZI = 1, .ACTION = action, .ACTIONDATE = Now})
        End If
        Return Me
    End Function

    Public Function scontoHeaderApplicabile(l As carrelloLine) As Boolean
        '20200424 - Gestione scontoScaglioni
        Dim res As Boolean = False
        If l.SCONTORIGA = 0 Then
            res = True
        End If
        'Check se esiste nel carrello uno stesso articolo con rowdiscount = 1 o uno stesso articolo con sconto di riga
        For Each line As carrelloLine In Me.carrelloLines
            If (line.ordineLine.ItemCode = l.ordineLine.ItemCode) Then
                If (line.ordineLine.RowDiscount = 1 Or l.ordineLine.RowDiscount = 1) Then
                    line.SCONTOHEADER = 0
                    l.SCONTOHEADER = 0
                    res = False
                End If
                If (line.SCONTORIGA > 0 Or l.SCONTORIGA > 0) Then
                    line.SCONTOHEADER = 0
                    l.SCONTOHEADER = 0
                    res = False
                End If
            End If
        Next
        Return res
    End Function

    'Public Sub rimuoviScontoHeader(Optional ID As Integer = 0, Optional action As String = "")
    '    Dim dm As New datamanager
    '    Dim scontoPagamento As Integer = Me.Header.SCONTOPAGAMENTO
    '    For Each l As carrelloLine In Me.carrelloLines
    '        '20200226 - gestione sconto pagamento su righe
    '        l.SCONTOHEADER = 0
    '        l.ordineLine.FormulaSconto = encodeFormulaSconto(l.SCONTORIGA, 0, scontoPagamento)
    '        l.TOTALERIGA = calcolaTotaleRiga(l)
    '    Next
    '    Me.Header.SCONTOHEADER = 0
    '    dm.salvaCarrello2(Me)
    '    dm = Nothing
    '    If ID > 0 Then
    '        delPromoHeader(New carrelloHeaderPromo With {.IDCART = Me.Header.IDCART, .CTCODE = Me.Header.CODICECONTATTO, .IDPROMO = ID, .NRUTILIZZI = 0, .ACTION = action})
    '    End If
    'End Sub

    Public Function checkPromoHeader(Optional ByVal saveCart As Boolean = False) As Int64
        Dim pm As New promoManager
        Dim dm As New datamanager
        Dim idpromo As Int64 = 0
        Dim totaleordine As Double = 0
        Dim cd As intCanaleDirezione = pm.getCanaleDirezioneByOrderType(Me.Header.ordineHeader.Type)
        'elimino tutte le promo header eventualmente applicate
        If Me.carrelloHeaderPromos.Count > 0 Then
            For Each l As carrelloLine In Me.carrelloLines
                '20200226 - gestione sconto pagamento su righe
                'l.ordineLine.FormulaSconto = getScontoRigaFromFormulaSconto(l)
                l.ordineLine.FormulaSconto = encodeFormulaSconto(l.SCONTORIGA, 0, l.SCONTOPAGAMENTO)
                l.TOTALERIGA = calcolaTotaleRiga(l)
            Next
            Me.Header.SCONTOHEADER = 0
            Me.carrelloHeaderPromos.RemoveAll(Function(item) (item.IDCART = Me.Header.IDCART))
            dm.delCarrelloPromo(New carrelloHeaderPromo With {.IDCART = Me.Header.IDCART})
        End If
        'riapplico la promozione del range totale ordine
        totaleordine = Me.GetTotaleMerceConScontoPagamento()
        Dim ph As promoHeader = pm.getPromoHeaderByCtCode(Me.Header.CODICECONTATTO, Me.Header.getOrderDate(), cd, totaleordine, Me.Header.IDPROFILOSCONTO).FirstOrDefault()
        If Not ph Is Nothing Then
            Dim cp As carrelloHeaderPromo = New carrelloHeaderPromo With {.IDCART = Me.Header.IDCART, .CTCODE = Me.Header.CODICECONTATTO, .IDPROMO = ph.ID, .NRUTILIZZI = 1, .ACTION = "ADD", .ACTIONDATE = Now}
            For Each l As carrelloLine In Me.carrelloLines
                '20200226 - gestione sconto pagamento su righe
                'l.ordineLine.FormulaSconto = encodeFormulaSconto(ph.DISCOUNT_PERCENT, getScontoRigaFromFormulaSconto(l))
                l.ordineLine.FormulaSconto = encodeFormulaSconto(l.SCONTORIGA, ph.DISCOUNT_PERCENT, l.SCONTOPAGAMENTO)
                l.TOTALERIGA = calcolaTotaleRiga(l)
            Next
            Me.Header.SCONTOHEADER = ph.DISCOUNT_PERCENT
            Me.carrelloHeaderPromos.Add(cp)
            dm.addCarrelloPromo(cp)
            idpromo = cp.IDPROMO
        End If
        If saveCart Then
            dm.salvaCarrello2(Me)
        End If
        dm = Nothing
        pm = Nothing
        Return idpromo
    End Function

    Public Function applicaScontoPagamento() As cartManager
        '20200226 - gestione sconto pagamento su righe
        Dim scontoPagamento As Integer = Me.Header.SCONTOPAGAMENTO
        Dim dm As New datamanager
        For Each l As carrelloLine In Me.carrelloLines
            If scontoPagamentoApplicabile(l) Then
                l.SCONTOPAGAMENTO = scontoPagamento
            Else
                l.SCONTOPAGAMENTO = 0
            End If
            l.ordineLine.FormulaSconto = encodeFormulaSconto(l.SCONTORIGA, l.SCONTOHEADER, l.SCONTOPAGAMENTO)
            l.TOTALERIGA = calcolaTotaleRiga(l)
        Next
        dm.salvaCarrello2(Me)
        Return Me
    End Function

    Public Function scontoPagamentoApplicabile(l As carrelloLine) As Boolean
        Dim res As Boolean = False
        '20200424 - Gestione scontoScaglioni
        If l.SCONTORIGA = 0 And l.SCONTOHEADER = "0" Then
            res = True
        End If
        'Check se esiste nel carrello uno stesso articolo con rowdiscount = 1
        For Each line As carrelloLine In Me.carrelloLines
            If (line.ordineLine.ItemCode = l.ordineLine.ItemCode) Then
                If (line.ordineLine.RowDiscount = 1 Or l.ordineLine.RowDiscount = 1) Then
                    line.SCONTOPAGAMENTO = 0
                    l.SCONTOPAGAMENTO = 0
                    res = False
                End If
                If (line.SCONTORIGA > 0 Or l.SCONTORIGA > 0) Then
                    line.SCONTOPAGAMENTO = 0
                    l.SCONTOPAGAMENTO = 0
                    res = False
                End If
            End If
        Next
        'End If
        Return res
    End Function

    Public Function calcolaTotaleRiga(l As carrelloLine) As Double
        Dim totaleriga As Double = l.ordineLine.UnitPrice * l.ordineLine.Quantity
        Dim FormulaSconto As String = l.ordineLine.FormulaSconto
        If FormulaSconto <> "" Then
            Dim sconti() As String = FormulaSconto.ToString.Split("-")
            If sconti.Count > 0 Then
                For i = 0 To sconti.Count - 1
                    If IsNumeric(sconti(i)) Then
                        totaleriga = totaleriga - (totaleriga / 100 * sconti(i))
                        'totaleriga = totaleriga - (totaleriga / 100 * Convert.ToDouble(sconti(i).Replace(".", ",")))
                    End If
                Next
            End If
        End If
        Return totaleriga
    End Function

    Public Function encodeFormulaSconto(scontoRiga As Integer, scontoHeader As String, scontoPagamento As Integer) As String
        '20200226 - gestione sconto pagamento su righe
        Dim arr() As String
        Dim res As String
        '20200424 - Gestione scontoScaglioni
        If (scontoRiga = 0 And scontoHeader = "0") Then
            arr = {scontoPagamento}
        ElseIf (scontoRiga = 0) Then
            arr = {scontoHeader.Replace("+", "-")}
        Else
            arr = {scontoRiga.ToString}
        End If

        res = String.Join("-", arr.Where(Function(x) x <> "0"))

        Return res
    End Function

    Public Sub refreshScontiLines()
        Dim scontoHeader As String = Me.Header.SCONTOHEADER
        Dim scontoPagamento As Integer = Me.Header.SCONTOPAGAMENTO
        For Each l As carrelloLine In Me.carrelloLines
            If scontoHeaderApplicabile(l) Then
                l.SCONTOHEADER = scontoHeader
            Else
                l.SCONTOHEADER = 0
            End If
            If scontoPagamentoApplicabile(l) Then
                l.SCONTOPAGAMENTO = scontoPagamento
            Else
                l.SCONTOPAGAMENTO = 0
            End If
            l.ordineLine.FormulaSconto = encodeFormulaSconto(l.SCONTORIGA, l.SCONTOHEADER, l.SCONTOPAGAMENTO)
            l.TOTALERIGA = calcolaTotaleRiga(l)
        Next
    End Sub

    Public Function carrelloLinesSorted() As List(Of carrelloLine)

        If Header.PRENOTAZIONE = 1 Or Header.PRENOTAZIONE = 2 Then
            Return Me.carrelloLines
        End If

        Dim clinesSortedNew As List(Of carrelloLine) = New List(Of carrelloLine)
        'clinesSortedNew = Me.carrelloLines.OrderBy(Function(x) x.ordineLine.ItemCode).ThenBy(Function(x) x.ordineLine.RowDiscount).ThenBy(Function(x) x.SCONTORIGA).ToList()
        clinesSortedNew = Me.carrelloLines.OrderBy(Function(x) x.ordineLine.LineID).ThenBy(Function(x) x.ordineLine.ItemCode).ThenBy(Function(x) x.ordineLine.RowDiscount).ThenBy(Function(x) x.SCONTORIGA).ToList()
        Return clinesSortedNew


        'Dim clinesSorted As List(Of carrelloLine) = New List(Of carrelloLine)
        'Dim clinesSorted2 As List(Of carrelloLine) = New List(Of carrelloLine)
        'Dim clinesOmaggioConLineApagamento As List(Of carrelloLine) = New List(Of carrelloLine)
        'Dim clinesOmaggio As List(Of carrelloLine) = Me.carrelloLines.FindAll(Function(item) (item.ordineLine.RowDiscount = 1))

        'If Header.PRENOTAZIONE = 1 Or Header.PRENOTAZIONE = 2 Then
        '    Return carrelloLines
        'End If

        'For Each a As carrelloLine In carrelloLines
        '    For Each b As carrelloLine In clinesOmaggio
        '        If a.ordineLine.ItemCode = b.ordineLine.ItemCode And a.ordineLine.LotNo = b.ordineLine.LotNo And a.ordineLine.RowDiscount = 0 And b.ordineLine.RowDiscount = 1 Then
        '            If (clinesOmaggioConLineApagamento.FindAll(Function(x) x.ordineLine.ItemCode = b.ordineLine.ItemCode And x.ordineLine.LotNo = b.ordineLine.LotNo).Count = 0) Then
        '                clinesOmaggioConLineApagamento.Add(b)
        '            End If
        '        End If
        '    Next
        'Next

        'For Each a As carrelloLine In carrelloLines
        '    Dim found As Boolean = False
        '    For Each b As carrelloLine In clinesOmaggioConLineApagamento
        '        If a.ordineLine.ItemCode = b.ordineLine.ItemCode And a.ordineLine.LotNo = b.ordineLine.LotNo And a.ordineLine.RowDiscount = b.ordineLine.RowDiscount Then
        '            found = True
        '            Exit For
        '        End If
        '    Next
        '    If Not found Then clinesSorted.Add(a)
        'Next

        'For Each a As carrelloLine In clinesSorted
        '    clinesSorted2.Add(a)
        '    For Each b As carrelloLine In clinesOmaggioConLineApagamento
        '        If a.ordineLine.ItemCode = b.ordineLine.ItemCode And a.ordineLine.LotNo = b.ordineLine.LotNo Then
        '            'If clinesSorted2.FindAll(Function(item) item.ordineLine.ItemCode = b.ordineLine.ItemCode And item.ordineLine.LotNo = b.ordineLine.LotNo).Count = 0 Then
        '            clinesSorted2.Add(b)
        '            ' End If
        '        End If
        '    Next
        'Next

        'Return clinesSorted2
    End Function

    Public Sub loadListino(Optional ByVal CodiceCliente As String = "")
        Dim dm As New datamanager
        Dim codeCli As String = CodiceCliente
        If codeCli = "" Then
            codeCli = Me.Header.CODICECLIENTE
        End If
        Dim codelist As String = dm.getListinoByCodiceCliente(codeCli)
        If codelist = "" Then
            codelist = dm.getListinoBase()
        End If
        Me.Header.CODICELISTINO = codelist
        dm = Nothing
        Log.Info("Caricato listino " + codelist + " per il cliente " + codeCli)
    End Sub

    Public Sub updateNote(ByVal note As String, ByVal userCode As Integer)
        Dim dm As New datamanager
        If Header.IDCART > 0 Then
            dm.updateCarrelloNote(Header.IDCART, note, userCode)
        End If
        If Header.ordineHeader.Code <> "" Then
            dm.updateOrderNote(Header.ordineHeader.Code, note)
        End If
        Header.ordineHeader.Notes = note
        dm = Nothing
    End Sub

    Public Function applicaVoucher(valoreVoucher As Double) As cartManager
        Dim dm As New datamanager
        Me.Header.ordineHeader.Voucher_Value = valoreVoucher
        dm.salvaCarrello2(Me)
        dm = Nothing
        Return Me
    End Function

    Public Function addLinePrenotazione(ByVal ItemCode As String, Optional ByVal Quantity2 As Integer = 1, Optional ByVal LotNo As String = "",
                            Optional ByVal fromcartpage As Boolean = False, Optional ByVal forzaQuantity As Boolean = False,
                            Optional ByVal rowDiscount As Integer = 0, Optional ByVal forzaNewRow As Boolean = False,
                            Optional ByVal ordercode As String = "", Optional ByVal IDPROMO As Integer = 0, Optional ByVal scontopercentualeString As String = "",
                            Optional ByVal salvaCarrelloOption As Boolean = True, Optional ByVal refreshDispoOption As Boolean = True) As List(Of carrelloLine)
        Dim dm As New datamanager
        If Not ItemCode Is Nothing AndAlso ItemCode <> "" Then

            '20200226 - gestione sconto pagamento su righe
            Dim scontopercentuale As Integer
            If scontopercentualeString = "0" Or scontopercentualeString = "100" Or scontopercentualeString = "" Then
                scontopercentuale = 0
            Else
                If IsNumeric(scontopercentualeString) AndAlso Convert.ToInt32(scontopercentualeString) > 0 Then
                    scontopercentuale = Convert.ToInt32(scontopercentualeString)
                End If
            End If

            Dim esiste As Boolean = False
            Dim lottoImp As String = ""
            Dim Quantity As Integer = 0
            Dim totaleDisponibilità As Integer = 0
            totaleDisponibilità = dm.GetDisponibilitaProdotti(ItemCode, True).Rows(0).Item("Disponibilita")

            'controllo esistenza articolo nel carrello con chiavi ITEMCODE + LOTTO + ROWDISCOUNT
            For Each item In carrelloLines
                If (item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = LotNo And item.ordineLine.RowDiscount = rowDiscount And item.SCONTORIGA = scontopercentuale) _
                    Or (item.ordineLine.ItemCode = ItemCode And LotNo = "" And item.ordineLine.RowDiscount = rowDiscount And item.SCONTORIGA = scontopercentuale) Then
                    esiste = True
                    Exit For
                End If
            Next

            If (esiste) Then
                updateLinePrenotazione(ItemCode, Quantity2, lottoImp, fromcartpage, forzaQuantity, rowDiscount, scontopercentualeString, IDPROMO, salvaCarrelloOption, refreshDispoOption)
            Else
                Dim LineIDmax As Integer = 0
                If carrelloLines.Count > 0 Then
                    LineIDmax = carrelloLines.Max(Function(linea As carrelloLine) linea.IDCARTLINE)
                End If
                Dim LineID As Integer = LineIDmax + 1
                Dim dr As DataRow = dm.GetWOItems(ItemCode, , False, ,,,,,,, Me.Header.CODICELISTINO).Rows(0)
                Dim newCarrelloLine As New carrelloLine
                newCarrelloLine.IDCART = Header.IDCART
                newCarrelloLine.IDCARTLINE = LineID
                newCarrelloLine.ordineLine.ItemCode = dr("CodiceArticolo")
                newCarrelloLine.DESCRIZIONE = dr("Descrizione")
                newCarrelloLine.DISPONIBILITA = dr("Disponibilita")
                newCarrelloLine.UNITPRICELIST = dr("PrzPubblico")
                newCarrelloLine.IVA = dr("Iva")
                newCarrelloLine.MARCHIO = dr("Marchio")
                newCarrelloLine.COMPOSIZIONE = dr("Composizione")
                newCarrelloLine.FORMATO = dr("Formato")
                newCarrelloLine.ordineLine.UnitPrice = dr("PrzRivenditore")
                newCarrelloLine.ordineLine.LotNo = LotNo
                newCarrelloLine.DISPOLOTTO = 0
                newCarrelloLine.ordineLine.Quantity = Quantity2
                newCarrelloLine.ordineLine.OriginalQty = newCarrelloLine.ordineLine.Quantity
                newCarrelloLine.ordineLine.RowDiscount = rowDiscount
                newCarrelloLine.TOTALERIGA = newCarrelloLine.ordineLine.UnitPrice * newCarrelloLine.ordineLine.Quantity
                If rowDiscount = 1 Then
                    newCarrelloLine.ordineLine.DiscountQty = newCarrelloLine.ordineLine.Quantity
                    newCarrelloLine.TOTALERIGA = 0
                    newCarrelloLine.ordineLine.Quantity = 0
                End If
                If IDPROMO > 0 Then
                    newCarrelloLine.IDPROMO = IDPROMO
                    Me.addPromoLine(New carrelloLinePromo With {.IDCART = newCarrelloLine.IDCART, .IDCARTLINE = newCarrelloLine.IDCARTLINE, .IDPROMO = newCarrelloLine.IDPROMO, .CTCODE = Me.Header.CODICECONTATTO, .NRMULTIPLI = 1})
                End If

                newCarrelloLine.SCONTORIGA = scontopercentuale
                '20200424 - Gestione scontoScaglioni
                If (scontoHeaderApplicabile(newCarrelloLine)) Then
                    newCarrelloLine.SCONTOHEADER = Me.Header.SCONTOHEADER
                Else
                    newCarrelloLine.SCONTOHEADER = 0
                End If
                '20200226 - gestione sconto pagamento su righe
                If (scontoPagamentoApplicabile(newCarrelloLine)) Then
                    newCarrelloLine.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                Else
                    newCarrelloLine.SCONTOPAGAMENTO = 0
                End If
                newCarrelloLine.ordineLine.FormulaSconto = encodeFormulaSconto(newCarrelloLine.SCONTORIGA, newCarrelloLine.SCONTOHEADER, Me.Header.SCONTOPAGAMENTO)
                '--------------------------------------------------

                newCarrelloLine.TOTALERIGA = calcolaTotaleRiga(newCarrelloLine)
                newCarrelloLine.ordineLine.LineID = LineID
                newCarrelloLine.ordineLine.LineNo = LineID * 10000
                newCarrelloLine.ordineLine.OrderCode = Me.Header.ordineHeader.Code
                newCarrelloLine.ordineLine.CompanyName = dm.GetWorkingCompany
                newCarrelloLine.ordineLine.BinCode = ""
                carrelloLines.Add(newCarrelloLine)
            End If

            '20200424 - Gestione scontoScaglioni
            Call refreshScontiLines()

            If salvaCarrelloOption Then dm.salvaCarrello2(Me)
            If refreshDispoOption Then Call refreshDispo()
        End If

        dm = Nothing
        Return carrelloLines
    End Function

    Public Function updateLinePrenotazione(ByVal ItemCode As String, ByVal Quantity As Integer, Optional ByVal LotNo As String = "",
                               Optional ByVal fromcartpage As Boolean = False, Optional ByVal forzaQuantity As Boolean = False,
                               Optional ByVal RowDiscount As Integer = 0, Optional ByVal scontopercentualeString As String = "", Optional ByVal IDPROMO As Integer = 0,
                               Optional ByVal salvaCarrelloOption As Boolean = True, Optional ByVal refreshDispoOption As Boolean = True) As List(Of carrelloLine)
        If Not ItemCode Is Nothing AndAlso ItemCode <> "" AndAlso IsNumeric(Quantity) Then

            '20200226 - gestione sconto pagamento su righe
            Dim scontopercentuale As Integer
            If scontopercentualeString = "0" Or scontopercentualeString = "100" Or scontopercentualeString = "" Then
                scontopercentuale = 0
            Else
                If IsNumeric(scontopercentualeString) AndAlso Convert.ToInt32(scontopercentualeString) > 0 Then
                    scontopercentuale = Convert.ToInt32(scontopercentualeString)
                End If
            End If

            If (Quantity > 0) Then
                For Each item As carrelloLine In carrelloLines
                    If item.ordineLine.ItemCode = ItemCode And item.ordineLine.LotNo = LotNo And item.ordineLine.RowDiscount = RowDiscount And item.SCONTORIGA = scontopercentuale Then
                        If Not fromcartpage Then
                            item.ordineLine.Quantity = item.ordineLine.Quantity + Quantity
                        Else
                            item.ordineLine.Quantity = Quantity
                        End If
                        item.ordineLine.OriginalQty = item.ordineLine.Quantity
                        item.TOTALERIGA = item.ordineLine.UnitPrice * item.ordineLine.Quantity
                        If RowDiscount = 1 Then
                            If Not fromcartpage Then
                                item.ordineLine.DiscountQty = item.ordineLine.DiscountQty + item.ordineLine.Quantity
                            Else
                                item.ordineLine.DiscountQty = item.ordineLine.Quantity
                            End If
                            item.ordineLine.OriginalQty = item.ordineLine.DiscountQty
                            item.TOTALERIGA = 0
                            item.ordineLine.Quantity = 0
                        End If

                        item.SCONTORIGA = scontopercentuale
                        '20200424 - Gestione scontoScaglioni
                        If (scontoHeaderApplicabile(item)) Then
                            item.SCONTOHEADER = Me.Header.SCONTOHEADER
                        Else
                            item.SCONTOHEADER = 0
                        End If
                        '20200226 - gestione sconto pagamento su righe
                        If (scontoPagamentoApplicabile(item)) Then
                            item.SCONTOPAGAMENTO = Me.Header.SCONTOPAGAMENTO
                        Else
                            item.SCONTOPAGAMENTO = 0
                        End If
                        item.ordineLine.FormulaSconto = encodeFormulaSconto(item.SCONTORIGA, item.SCONTOHEADER, item.SCONTOPAGAMENTO)
                        '--------------------------------------------------
                        item.TOTALERIGA = calcolaTotaleRiga(item)
                        If IDPROMO > 0 Then
                            Me.addPromoLine(New carrelloLinePromo With {.IDCART = item.IDCART, .IDCARTLINE = item.IDCARTLINE, .IDPROMO = IDPROMO, .CTCODE = Me.Header.CODICECONTATTO, .NRMULTIPLI = 1})
                        End If
                        Exit For
                    End If
                Next

                '20200424 - Gestione scontoScaglioni
                Call refreshScontiLines()

                Dim dm As New datamanager
                dm.salvaCarrello2(Me)
                Call refreshDispo()
                dm = Nothing
            Else
                deleteLine(ItemCode, LotNo, RowDiscount, -1, scontopercentualeString)
            End If
        End If
        Return carrelloLines
    End Function

    Public Function controlloGiacenze_PreDichiarazioneProduzione_OrdinePrenotato() As Boolean
        Dim controllo As Boolean = True
        Dim dm As New datamanager
        Dim groupedLines = carrelloLines.GroupBy(Function(l) New With {Key l.ordineLine.ItemCode}).
                               Select(Function(group) New With {
                                  .ItemCode = group.Key.ItemCode,
                                  .DiscountQty = group.Sum(Function(c) c.ordineLine.DiscountQty),
                                  .Quantity = group.Sum(Function(c) c.ordineLine.Quantity)
                                   })

        For Each item In groupedLines
            Dim dispo As Integer = dm.GetDisponibilitaProdotti(item.ItemCode, True).Rows(0).Item("Disponibilita")
            If dispo < item.Quantity + item.DiscountQty Then
                controllo = False
                Exit For
            End If
        Next
        dm = Nothing
        Return controllo
    End Function



End Class