Imports System.Data

Partial Public Class cartManager

    Public Function pickOrder(ByVal OrderCode As String, ByVal userCode As Integer) As cartManager
        Dim dm As New datamanager
        If dm.pickOrder(OrderCode, userCode) > 0 Then
            Dim carrello As cartManager = Me.loadFromDBbyOrderCode(OrderCode, True, False)

            Return carrello
        Else
            Return Nothing
        End If
        dm = Nothing
    End Function

    Public Function pickOrderToShip(ByVal OrderCode As String) As cartManager
        Dim dm As New datamanager
        Return Me.loadFromDBbyOrderCode(OrderCode, True, False)
        dm = Nothing
    End Function

    Public Function shipOrder(ByVal orderCode As String, ByVal UserCtrl As String, ByVal peso As Integer, ByVal nrcolli As Integer, ByVal ShippingAgentCode As String) As Integer
        Dim dm As New datamanager
        Return dm.shipOrder(orderCode, UserCtrl, peso, nrcolli, ShippingAgentCode)
        dm = Nothing
    End Function

    Public Function rettificaRigaSuLottoSuccessivo(ByVal currentRow As carrelloLine, ByVal newQuantita As Integer, ByVal newLotto As String, ByVal newCollocazione As String) As Integer
        Dim dm As New datamanager
        Dim newLine As New carrelloLine
        newLine = CType(currentRow.Clone, carrelloLine)
        newLine.ordineLine = CType(currentRow.ordineLine.Clone, orderLine)

        'gestione nuovo numero di riga
        Dim LineIDmax As Integer = 0
        If carrelloLines.Count > 0 Then
            LineIDmax = carrelloLines.Max(Function(l As carrelloLine) l.IDCARTLINE)
        End If
        Dim newLineID As Integer = LineIDmax + 1
        newLine.IDCARTLINE = newLineID
        newLine.ordineLine.LineID = newLineID
        newLine.ordineLine.LineNo = newLineID * 10000

        'aggiungo riferimento alla riga originaria
        newLine.LINEIDSOURCE = currentRow.ordineLine.LineID

        'gestione riga caricata a magazzino
        newLine.LOADED = 1

        'gestione nuovo lotto e collocazione
        newLine.ordineLine.LotNo = newLotto
        newLine.ordineLine.BinCode = newCollocazione

        'gestione se riga sconto 
        If newLine.ordineLine.RowDiscount = 0 Then
            newLine.ordineLine.Quantity = newQuantita
            newLine.TOTALERIGA = newLine.ordineLine.UnitPrice * newLine.ordineLine.Quantity
        Else
            newLine.ordineLine.DiscountQty = newQuantita
            newLine.TOTALERIGA = 0
        End If

        newLine.ordineLine.OriginalQty = newQuantita
        newLine.ordineLine.QtyToShip = newQuantita

        'gestione aggiornamento dispo lotti
        Dim dispoLottoRow() As DataRow = dm.GetDisponibilitaProdottiPerLotto(currentRow.ordineLine.ItemCode, True).Select("Lotto='" & newLotto & "'")
        If dispoLottoRow.Count > 0 Then
            newLine.DISPOLOTTO = dm.GetDisponibilitaProdottiPerLotto(currentRow.ordineLine.ItemCode, True).Select("Lotto='" & newLotto & "'")(0).Item("DisponibilitaLotto")
        End If

        'aggiorno struttura carrello
        carrelloLines.Add(newLine)

        Dim quantitaRigaCorrenteOld As Integer
        If currentRow.ordineLine.RowDiscount = 0 Then
            quantitaRigaCorrenteOld = currentRow.ordineLine.Quantity
        Else
            quantitaRigaCorrenteOld = currentRow.ordineLine.DiscountQty
        End If
        Log.Info("Order " & currentRow.ordineLine.OrderCode & " rettifica riga su lotto successivo from line " & currentRow.IDCARTLINE & ": qty = " & quantitaRigaCorrenteOld & ", lotto = " & currentRow.ordineLine.LotNo & ", collocazione = " & currentRow.ordineLine.BinCode)
        Log.Info("Order " & currentRow.ordineLine.OrderCode & " rettifica riga su lotto successivo to line " & newLine.IDCARTLINE & ": qty = " & newQuantita & ", lotto = " & newLotto & ", collocazione = " & newCollocazione)

        'aggiungo nuove righe in WO e  NAV e aggiorno quantità della riga rettificata in WO e NAV
        Return dm.rettificaRigaSuLottoSuccessivo(newLine)

        dm = Nothing
    End Function

    Public Function eliminaRigaNAV_DoporettificaRiga(ByVal currentRow As carrelloLine) As cartManager
        Dim dm As New datamanager
        dm.eliminaRigaNAV_DoporettificaRiga(currentRow)
        dm = Nothing
        Log.Info("Order " & currentRow.ordineLine.OrderCode & " line " & currentRow.IDCARTLINE & " deleted from NAV after rettifica su lotto successivo")
        Return Me
    End Function

    Public Function restoreRettifica(ByVal currentRow As carrelloLine) As cartManager
        Dim dm As New datamanager
        dm.restoreRettifica(currentRow)
        Dim carrello As cartManager = Me.loadFromDBbyOrderCode(currentRow.ordineLine.OrderCode, True)
        dm = Nothing
        Log.Info("Order " & currentRow.ordineLine.OrderCode & " line " & currentRow.IDCARTLINE & " restore rettifica finished")
        Return carrello
    End Function

    Public Function getWarehouseLinesCollegate(ByVal currentRow As carrelloLine) As List(Of carrelloLine)
        Dim lineeCollegate As New List(Of carrelloLine)
        For Each l As carrelloLine In carrelloLines
            If l.LINEIDSOURCE = currentRow.ordineLine.LineID Then
                lineeCollegate.Add(l)
            End If
        Next
        Return lineeCollegate
    End Function

    Public Function aggiornaQuantitàDaMagazzino(ByVal currentRow As carrelloLine, ByVal quantitaDaCaricare As Integer) As cartManager
        Dim dm As New datamanager
        'restituire la riga modificata ed aggiornarla nel carrello 
        'in modo da non dover ricaricare tutto il carrello da database ad ogni pick
        Dim rowUpdated As carrelloLine = dm.aggiornaQuantitàDaMagazzino(currentRow, quantitaDaCaricare)
        If rowUpdated IsNot Nothing Then
            If carrelloLines.Find(Function(l) l.IDCARTLINE = rowUpdated.IDCARTLINE) IsNot Nothing Then
                carrelloLines.Find(Function(l) l.IDCARTLINE = rowUpdated.IDCARTLINE).LOADED = rowUpdated.LOADED
                carrelloLines.Find(Function(l) l.IDCARTLINE = rowUpdated.IDCARTLINE).ordineLine.QtyToShip = rowUpdated.ordineLine.QtyToShip
                carrelloLines.Find(Function(l) l.IDCARTLINE = rowUpdated.IDCARTLINE).ordineLine.Quantity = rowUpdated.ordineLine.Quantity
                carrelloLines.Find(Function(l) l.IDCARTLINE = rowUpdated.IDCARTLINE).ordineLine.DiscountQty = rowUpdated.ordineLine.DiscountQty
            End If
        End If
        dm = Nothing
        Return Me
    End Function


End Class
