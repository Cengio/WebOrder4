
Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function GetOrderProgressive() As Int64
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT Progressive FROM NoWebSeries WHERE [NO_ Type]='orderweb' AND CompanyCode=@CompanyCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Read()
            Dim newprog As Int64 = dr("Progressive") + 1
            dr.Close()
            cmd.CommandText = "UPDATE NoWebSeries SET Progressive= Progressive + 1 WHERE [NO_ Type]='orderweb' AND CompanyCode=@CompanyCode"
            cmd.Connection = conn
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            cmd.ExecuteNonQuery()
            Return CLng(newprog)
        Catch ex As Exception
            Throw New Exception("Numero progressivo ORDINE WEB non impostato correttamente." & " " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetOrderHeader(ByVal Code As String) As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            sql = "SELECT A.* FROM [OrderHeaderFromWeb_Write] AS A WHERE A.[OrderDate]<>'' AND A.Code<>'' AND [Code]=@Code "
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = Code
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ds
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function addOrderFromCarrello(ByVal carrello As cartManager) As cartManager
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim progressivoOrdine As Int64 = GetOrderProgressive()
            carrello.Header.ordineHeader.Code = progressivoOrdine
            carrello.Header.ordineHeader.CustomerNo = carrello.Header.CODICECLIENTE
            carrello.Header.ordineHeader.OperatorCode = carrello.Header.UTENTE_ULTIMA_MODIFICA
            carrello.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE
            Dim ctrl As Integer = 0
            Dim sql As String = "INSERT INTO [OrderHeaderFromWeb_Write] ("
            sql &= " [Code]"
            sql &= ",[CompanyName_]"
            sql &= ",[CustomerNo]"
            sql &= ",[OrderDate]"
            sql &= ",[Type]"
            sql &= ",[ShippingAgentCode]"
            sql &= ",[ShipAddressCode]"
            sql &= ",[Notes]"
            sql &= ",[AttachmentPath]"
            sql &= ",[OperatorCode]"
            sql &= ",[PackageNum]"
            sql &= ",[OverpackageNum]"
            sql &= ",[User]"
            sql &= ",[CompletedImported]"
            sql &= ",[IncludeShipCost]"
            sql &= ",[PaymentMethodCode]"
            sql &= ",[PaymentTermsCode]"
            sql &= ",[Weight]"
            sql &= ",[OrderNoCtrl]"
            sql &= ",[UserCtrl]"
            sql &= ",[Status]"
            sql &= ",[Imported]"
            sql &= ",[Voucher Value]"
            sql &= ") VALUES ("
            sql &= "@Code"
            sql &= ",'SER-VIS'" ',@CompanyName
            sql &= ",@CustomerNo"
            sql &= ",@OrderDate"
            sql &= ",@Type"
            sql &= ",@ShippingAgentCode"
            sql &= ",@ShipAddressCode"
            sql &= ",@Notes"
            sql &= ",@AttachmentPath"
            sql &= ",@OperatorCode"
            sql &= ",@PackageNum"
            sql &= ",@OverpackageNum"
            sql &= ",@User"
            sql &= ",@CompletedImported"
            sql &= ",@IncludeShipCost"
            sql &= ",@PaymentMethodCode"
            sql &= ",@PaymentTermsCode"
            sql &= ",@Weight"
            sql &= ",@OrderNoCtrl"
            sql &= ",@UserCtrl"
            sql &= ",@Status"
            sql &= ",0"
            sql &= ",@Voucher_Value"
            sql &= ")"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.Code
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = carrello.Header.ordineHeader.CompanyName
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.CustomerNo
            cmd.Parameters.Add("@OrderDate", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OrderDate
            cmd.Parameters.Add("@Type", SqlDbType.VarChar, 25).Value = carrello.Header.ordineHeader.Type
            cmd.Parameters.Add("@ShippingAgentCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.ShippingAgentCode
            cmd.Parameters.Add("@ShipAddressCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.ShipAddressCode
            cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 250).Value = carrello.Header.ordineHeader.Notes
            cmd.Parameters.Add("@AttachmentPath", SqlDbType.VarChar, 250).Value = carrello.Header.ordineHeader.AttachmentPath
            cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OperatorCode
            cmd.Parameters.Add("@PackageNum", SqlDbType.Int).Value = carrello.Header.ordineHeader.PackageNum
            cmd.Parameters.Add("@OverpackageNum", SqlDbType.Int).Value = carrello.Header.ordineHeader.OverpackageNum
            cmd.Parameters.Add("@User", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.User
            cmd.Parameters.Add("@CompletedImported", SqlDbType.TinyInt).Value = carrello.Header.ordineHeader.CompletedImported
            cmd.Parameters.Add("@IncludeShipCost", SqlDbType.TinyInt).Value = carrello.Header.ordineHeader.IncludeShipCost
            cmd.Parameters.Add("@PaymentMethodCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.PaymentMethodCode
            cmd.Parameters.Add("@PaymentTermsCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.PaymentTermsCode
            cmd.Parameters.Add("@Weight", SqlDbType.Int).Value = carrello.Header.ordineHeader.Weight
            cmd.Parameters.Add("@OrderNoCtrl", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OrderNoCtrl
            cmd.Parameters.Add("@UserCtrl", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.UserCtrl
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = carrello.Header.ordineHeader.Status
            cmd.Parameters.Add("@Voucher_Value", SqlDbType.Decimal).Value = carrello.Header.ordineHeader.Voucher_Value

            ctrl = cmd.ExecuteNonQuery()

            If ctrl > 0 AndAlso carrello.carrelloLines.Count > 0 Then
                For Each line As carrelloLine In carrello.carrelloLines
                    line.ordineLine.OrderCode = carrello.Header.ordineHeader.Code
                    sql = "INSERT INTO [OrderLineFromWebV2_Write] ("
                    sql &= " [OrderCode]"
                    sql &= ",[ItemCode]"
                    sql &= ",[RowDiscount]"
                    sql &= ",[LineID]"
                    sql &= ",[OldItemCode]"
                    sql &= ",[CompanyName_]"
                    sql &= ",[UnitPrice]"
                    sql &= ",[Quantity]"
                    sql &= ",[DiscountQty]"
                    sql &= ",[Imported]"
                    sql &= ",[BinCode]"
                    sql &= ",[QtyToShip]"
                    sql &= ",[LotNo]"
                    sql &= ",[FormulaSconto]"
                    sql &= ",[LineNo]"
                    sql &= ",[OriginalQty]"
                    sql &= ",[BC_Imported]"
                    sql &= ") VALUES ("
                    sql &= "@OrderCode"
                    sql &= ",@ItemCode"
                    sql &= ",@RowDiscount"
                    sql &= ",@LineID"
                    sql &= ",@OldItemCode"
                    sql &= ",'SER-VIS'" '",@CompanyName"
                    sql &= ",@UnitPrice"
                    sql &= ",@Quantity"
                    sql &= ",@DiscountQty"
                    sql &= ",@Imported"
                    sql &= ",@BinCode"
                    sql &= ",@QtyToShip"
                    sql &= ",@LotNo"
                    sql &= ",@FormulaSconto"
                    sql &= ",@LineNo"
                    sql &= ",@OriginalQty"
                    sql &= ",0"
                    sql &= ")"

                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.Code
                    cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = line.ordineLine.ItemCode
                    'If line.IDPROMO > 0 Then
                    cmd.Parameters.Add("@RowDiscount", SqlDbType.Int).Value = line.ordineLine.RowDiscount
                    'Else
                    'cmd.Parameters.Add("@RowDiscount", SqlDbType.Int).Value = 0
                    'End If
                    cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = line.ordineLine.LineID
                    cmd.Parameters.Add("@OldItemCode", SqlDbType.VarChar, 20).Value = line.ordineLine.OldItemCode
                    cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = line.ordineLine.CompanyName
                    cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = line.ordineLine.UnitPrice
                    'If line.IDPROMO > 0 Then
                    cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = line.ordineLine.Quantity
                    cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = line.ordineLine.DiscountQty
                    'Else
                    'If line.ordineLine.RowDiscount = 1 Then
                    '    cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = line.ordineLine.DiscountQty
                    '    cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = line.ordineLine.Quantity
                    'Else
                    '    cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = line.ordineLine.Quantity
                    '    cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = line.ordineLine.DiscountQty
                    'End If
                    'End If
                    cmd.Parameters.Add("@Imported", SqlDbType.Int).Value = line.ordineLine.Imported
                    cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = line.ordineLine.BinCode
                    cmd.Parameters.Add("@QtyToShip", SqlDbType.Decimal).Value = line.ordineLine.QtyToShip
                    cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = line.ordineLine.LotNo
                    'If line.IDPROMO > 0 Then
                    cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = line.ordineLine.FormulaSconto.Replace(".", ",")
                    'Else
                    'cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = ""
                    'End If
                    cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = line.ordineLine.LineNo

                    If line.ordineLine.RowDiscount = 0 Then
                        cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = line.ordineLine.Quantity
                    Else
                        cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = line.ordineLine.DiscountQty
                    End If
                    ctrl = ctrl + cmd.ExecuteNonQuery()
                Next
            End If

            If ctrl <= 0 Then
                '
            End If

            salvaCarrello2(carrello, False)

            Return carrello

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function updOrderHeader(ByVal carrello As cartManager) As Integer
        Dim conn As New SqlConnection(NAVconnectionString)
        Try
            Dim sql As String = "UPDATE [OrderHeaderFromWeb_Write] SET "
            sql &= " [Code]=@Code"
            sql &= ",[CompanyName_]='SER-VIS'"
            sql &= ",[CustomerNo]=@CustomerNo"
            sql &= ",[OrderDate]=@OrderDate"
            sql &= ",[Type]=@Type"
            sql &= ",[ShippingAgentCode]=@ShippingAgentCode"
            sql &= ",[ShipAddressCode]=@ShipAddressCode"
            sql &= ",[Notes]=@Notes"
            sql &= ",[AttachmentPath]=@AttachmentPath"
            sql &= ",[OperatorCode]=@OperatorCode"
            sql &= ",[PackageNum]=@PackageNum"
            sql &= ",[OverpackageNum]=@OverpackageNum"
            sql &= ",[User]=@User"
            sql &= ",[CompletedImported]=@CompletedImported"
            sql &= ",[IncludeShipCost]=@IncludeShipCost"
            sql &= ",[PaymentMethodCode]=@PaymentMethodCode"
            sql &= ",[PaymentTermsCode]=@PaymentTermsCode"
            sql &= ",[Weight]=@Weight"
            sql &= ",[OrderNoCtrl]=@OrderNoCtrl"
            sql &= ",[UserCtrl]=@UserCtrl"
            sql &= ",[Status]=@Status"
            sql &= ",[Voucher Value]=@Voucher_Value"
            sql &= " WHERE [Code]=@Code"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = carrello.Header.ordineHeader.CompanyName
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.CustomerNo
            cmd.Parameters.Add("@OrderDate", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OrderDate
            cmd.Parameters.Add("@Type", SqlDbType.VarChar, 25).Value = carrello.Header.ordineHeader.Type
            cmd.Parameters.Add("@ShippingAgentCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.ShippingAgentCode
            cmd.Parameters.Add("@ShipAddressCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.ShipAddressCode
            cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 250).Value = carrello.Header.ordineHeader.Notes
            cmd.Parameters.Add("@AttachmentPath", SqlDbType.VarChar, 250).Value = carrello.Header.ordineHeader.AttachmentPath
            cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OperatorCode
            cmd.Parameters.Add("@PackageNum", SqlDbType.Int).Value = carrello.Header.ordineHeader.PackageNum
            cmd.Parameters.Add("@OverpackageNum", SqlDbType.Int).Value = carrello.Header.ordineHeader.OverpackageNum
            cmd.Parameters.Add("@User", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.User
            cmd.Parameters.Add("@CompletedImported", SqlDbType.TinyInt).Value = carrello.Header.ordineHeader.CompletedImported
            cmd.Parameters.Add("@IncludeShipCost", SqlDbType.TinyInt).Value = carrello.Header.ordineHeader.IncludeShipCost
            cmd.Parameters.Add("@PaymentMethodCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.PaymentMethodCode
            cmd.Parameters.Add("@PaymentTermsCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.PaymentTermsCode
            cmd.Parameters.Add("@Weight", SqlDbType.Int).Value = carrello.Header.ordineHeader.Weight
            cmd.Parameters.Add("@OrderNoCtrl", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OrderNoCtrl
            cmd.Parameters.Add("@UserCtrl", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.UserCtrl
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = carrello.Header.ordineHeader.Status
            cmd.Parameters.Add("@Voucher_Value", SqlDbType.Decimal).Value = carrello.Header.ordineHeader.Voucher_Value
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.Code

            conn.Open()
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            'MsgBox(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function updOrderLines(ByVal carrello As cartManager) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim ctrl As Integer = 0
            Dim sql As String
            'elimino tutte le righe dell'ordine
            Dim cmd As New SqlCommand
            cmd.Connection = conn

            sql = "DELETE FROM [OrderLineFromWebV2_Write] WHERE orderCode=@orderCode"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.Code
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = workingCompany
            ctrl = cmd.ExecuteNonQuery()

            'reinserisco le righe aggiornate
            If carrello.carrelloLines.Count > 0 Then
                ctrl = 0
                For Each line As carrelloLine In carrello.carrelloLines
                    sql = "INSERT INTO [OrderLineFromWebV2_Write] ("
                    sql &= " [OrderCode], [ItemCode], [RowDiscount], [LineID], [OldItemCode], [CompanyName_],"
                    sql &= " [UnitPrice], [Quantity], [DiscountQty], [Imported], [BinCode], [QtyToShip], [LotNo],"
                    sql &= " [FormulaSconto], [LineNo], [OriginalQty], [BC_Imported])"
                    sql &= " VALUES (@OrderCode, @ItemCode, @RowDiscount, @LineID, @OldItemCode, 'SER-VIS',"
                    sql &= " @UnitPrice, @Quantity, @DiscountQty, @Imported, @BinCode, @QtyToShip, @LotNo,"
                    sql &= " @FormulaSconto, @LineNo, @OriginalQty, 0)"

                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.Code
                    cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = line.ordineLine.ItemCode
                    cmd.Parameters.Add("@RowDiscount", SqlDbType.Int).Value = line.ordineLine.RowDiscount
                    cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = line.ordineLine.LineID
                    cmd.Parameters.Add("@OldItemCode", SqlDbType.VarChar, 20).Value = line.ordineLine.OldItemCode
                    cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = line.ordineLine.CompanyName
                    cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = line.ordineLine.UnitPrice
                    cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = line.ordineLine.Quantity
                    cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = line.ordineLine.DiscountQty
                    cmd.Parameters.Add("@Imported", SqlDbType.Int).Value = line.ordineLine.Imported
                    cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = line.ordineLine.BinCode
                    cmd.Parameters.Add("@QtyToShip", SqlDbType.Decimal).Value = line.ordineLine.QtyToShip
                    cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = line.ordineLine.LotNo
                    cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = line.ordineLine.FormulaSconto.Replace(".", ",")
                    cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = line.ordineLine.LineNo
                    If line.ordineLine.RowDiscount = 0 Then
                        cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = line.ordineLine.Quantity
                    Else
                        cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = line.ordineLine.DiscountQty
                    End If
                    ctrl = ctrl + cmd.ExecuteNonQuery()
                Next
            End If

            Return ctrl

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function closeOrder(ByVal orderCode As String, ByVal operatorCode As String) As Boolean
        Dim conn As New SqlConnection(NAVconnectionString)
        Dim connWOR As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [OrderHeaderFromWeb_Write] SET "
            sql &= "[OperatorCode]=@OperatorCode"
            sql &= ",[Status]=@Status"
            sql &= " WHERE [Code]=@Code"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 2
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim sql2 As String = "UPDATE [carrelloHeader] SET "
            sql2 &= "[OperatorCode]=@OperatorCode"
            sql2 &= ",UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA"
            sql2 &= ",[Status]=@Status"
            sql2 &= " WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            Dim cmd2 As New SqlCommand(sql2, connWOR)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd2.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd2.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 2
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWOR.Open()
            cmd2.ExecuteNonQuery()

            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function richiediApprovazioneOrdine(ByVal orderCode As String, ByVal operatorCode As String) As Boolean
        Dim conn As New SqlConnection(NAVconnectionString)
        Dim connWOR As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [OrderHeaderFromWeb_Write] SET "
            sql &= "[OperatorCode]=@OperatorCode"
            sql &= ",[Status]=@Status"
            sql &= " WHERE [Code]=@Code"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 7
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim sql2 As String = "UPDATE [carrelloHeader] SET "
            sql2 &= "[OperatorCode]=@OperatorCode"
            sql2 &= ",UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA"
            sql2 &= ",[Status]=@Status"
            sql2 &= " WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            Dim cmd2 As New SqlCommand(sql2, connWOR)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd2.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd2.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 7
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWOR.Open()
            cmd2.ExecuteNonQuery()

            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function riprendiOrdine(ByVal orderCode As String, ByVal operatorCode As String) As Integer
        Dim connNAV As New SqlConnection(NAVconnectionString)
        Dim connWOR As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [OrderHeaderFromWeb_Write] SET "
            sql &= "[OperatorCode]=@OperatorCode"
            sql &= ",[Status]=@Status"
            sql &= " WHERE [Code]=@Code"
            Dim cmd As New SqlCommand(sql, connNAV)
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 0
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connNAV.Open()
            cmd.ExecuteNonQuery()

            Dim sql2 As String = "UPDATE [carrelloHeader] SET "
            sql2 &= "[OperatorCode]=@OperatorCode"
            sql2 &= ",UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA"
            sql2 &= ",[Status]=@Status"
            sql2 &= " WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            Dim cmd2 As New SqlCommand(sql2, connWOR)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd2.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd2.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 0
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWOR.Open()
            cmd2.ExecuteNonQuery()

            Return 1
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If connNAV.State = ConnectionState.Open Then connNAV.Close()
        End Try
    End Function

    Public Function annullaOrdine(ByVal orderCode As String, ByVal operatorCode As String, ByVal notes As String) As Boolean
        Dim connNAV As New SqlConnection(NAVconnectionString)
        Dim connWOR As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [OrderHeaderFromWeb_Write] SET "
            sql &= "[OperatorCode]=@OperatorCode"
            sql &= ",[Status]=@Status"
            sql &= ",[Notes]=@Notes"
            sql &= " WHERE [Code]=@Code "
            Dim cmd As New SqlCommand(sql, connNAV)
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 6
            cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 250).Value = notes
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connNAV.Open()
            cmd.ExecuteNonQuery()

            Dim sql2 As String = "UPDATE [carrelloHeader] SET "
            sql2 &= "[OperatorCode]=@OperatorCode"
            sql2 &= ",UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA"
            sql2 &= ",[Status]=@Status"
            sql2 &= ",[Notes]=@Notes"
            sql2 &= " WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            Dim cmd2 As New SqlCommand(sql2, connWOR)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd2.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd2.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 6
            cmd2.Parameters.Add("@Notes", SqlDbType.VarChar, 250).Value = notes
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWOR.Open()
            cmd2.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If connNAV.State = ConnectionState.Open Then connNAV.Close()
        End Try
    End Function

    Public Function produciOrdine(ByVal orderCode As String, ByVal operatorCode As String) As Boolean
        Dim conn As New SqlConnection(NAVconnectionString)
        Dim connWOR As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [OrderHeaderFromWeb_Write] SET "
            sql &= "[OperatorCode]=@OperatorCode"
            sql &= ",[Status]=@Status"
            sql &= " WHERE [Code]=@Code "
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 1
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            conn.Open()
            cmd.ExecuteNonQuery()

            Dim sql2 As String = "UPDATE [carrelloHeader] SET "
            sql2 &= "[OperatorCode]=@OperatorCode"
            sql2 &= ",UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA"
            sql2 &= ",[Status]=@Status"
            sql2 &= " WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            Dim cmd2 As New SqlCommand(sql2, connWOR)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = operatorCode
            cmd2.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd2.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 1
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWOR.Open()
            cmd2.ExecuteNonQuery()

            Return True

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
            If connWOR.State = ConnectionState.Open Then connWOR.Close()
        End Try
    End Function

    Public Function getOrderHeaderWO(ByVal Code As String) As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT * FROM [carrelloHeader] WHERE [CompanyName_]='" & workingCompany & "' AND [Code]=@Code "
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = Code
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ds
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getOrderLinesWO(ByVal orderCode As String, Optional ByVal orderByField As String = "LineID") As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT * FROM [carrelloLines] WITH(NOLOCK) WHERE [CompanyName_]='" & workingCompany & "' AND [orderCode]=@orderCode "
            If orderByField = "LineID" Then
                sql &= " ORDER BY LineID"
            ElseIf orderByField = "BinCode" Then
                sql &= " ORDER BY "
                sql &= " LEFT(bincode,6) "
                sql &= ",SUBSTRING(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)), 1,2) "
                sql &= ",SUBSTRING(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)) ,3,2) "
                sql &= ",CAST(SUBSTRING(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)), 6, CASE WHEN LEN(SUBSTRING(bincode,CHARINDEX('-',bincode)+1,LEN(bincode)))=8 THEN 1 WHEN LEN(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)))=9 THEN 2 END ) AS INT)"
            End If
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ds
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getOrderListSelectCommand() As String
        Dim sql As String
        sql = "SELECT [UTENTE_ULTIMA_MODIFICA]=(SELECT Cognome + ' ' + Nome FROM WU_users WHERE [User Code] = [UTENTE_ULTIMA_MODIFICA])"
        sql &= " ,[UTENTE_CREAZIONE]=(Select Cognome + ' ' + Nome FROM WU_users WHERE [User Code] = [UTENTE_CREAZIONE])"
        sql &= " ,[MAGAZZINIERE]=(SELECT Cognome + ' ' + Nome FROM WU_users WHERE [User Code] = [User]) "
        sql &= " ,[DATA_ULTIMA_MODIFICA]"
        sql &= " ,[DATA_CREAZIONE]"
        sql &= " ,[Code]"
        sql &= " ,[CustomerNo]"
        sql &= " ,'' AS [CustomerDescription]"
        sql &= " ,[Type]"
        sql &= " ,CAST([CompletedImported] As int) AS CompletedImported"
        sql &= " ,[Status] "
        sql &= " ,[PRENOTAZIONE]"
        sql &= " ,[DATA_EVASIONE]"
        sql &= " FROM carrelloHeader As A With(NOLOCK) "
        sql &= "  WHERE A.[CompanyName_]='" & GetWorkingCompany & "' "
        'sql &= " AND (CHARINDEX(',' + CAST(A.CompletedImported AS varchar(10)) + ',', ',' + @CompletedImported + ',') > 0) "       --> per performance tuning da Massimo G
        sql &= " AND A.CompletedImported in (select Valore from dbo.splitToIntTable (@CompletedImported))"
        'sql &= " AND (CHARINDEX(',' + CAST(A.Status AS varchar(10)) + ',', ',' + @status + ',') > 0) "                             --> idem
        sql &= " AND A.Status in (select Valore from dbo.splitToIntTable (@Status))"
        sql &= " AND (NOT (A.[Status]=@NotStatus AND CompletedImported=@NotCompletedImported) ) "
        'sql &= " AND (CHARINDEX(',' + CAST(A.PRENOTAZIONE AS varchar(10)) + ',', ',' + @Prenotazione + ',') > 0) "                 --> idem
        sql &= " AND A.PRENOTAZIONE in (select Valore from dbo.splitToIntTable (@Prenotazione))"
        sql &= " ORDER BY "
        sql &= "    CASE [CustomerNo] WHEN 'C27866' THEN 1 ELSE 2 END DESC"
        sql &= "    ,A.[DATA_ULTIMA_MODIFICA] DESC"
        Return sql
    End Function

    Public Function getNavOrderListSelectCommand() As String
        Return "EXEC dbo.woGetNavOrderList @CompanyName='" & GetWorkingCompany() & "'"
    End Function


    Public Function getNavNewCustomersOrderListSelectCommand() As String
        Dim sql As String
        sql = "SELECT A.*,B.*,OrderDate2 = convert(datetime,(SUBSTRING(A.[OrderDate],7,4) + '-' + SUBSTRING(A.[OrderDate],4,2) + '-' +SUBSTRING(A.[OrderDate],1,2) + 'T' + '00:00:00.000')) "
        sql &= " FROM [OrderHeaderFromWeb_Write] AS A INNER JOIN CustomersFromOutside AS B ON A.CustomerNo COLLATE Latin1_General_CI_AS = B.[CustomerNo] "
        sql &= " WHERE (A.[OrderDate]<>'') AND (LEN(A.[OrderDate])=10) AND (A.Code<>'') AND (A.CustomerNo LIKE 'N%') ORDER BY OrderDate2 DESC"
        Return sql
    End Function

    Public Sub updateOrderNote(ByVal OrderCode As String, ByVal note As String)
        Dim conn As New SqlConnection(NAVconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand("UPDATE [OrderHeaderFromWeb_Write] SET [Notes]=@Notes WHERE [Code]=@Code", conn)
            cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 250).Value = note
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = OrderCode
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub




    Public Function GetOrderCodeToInvoice() As DataTable
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT Code FROM [carrelloHeader] WITH(NOLOCK) WHERE [CompanyName_]='" & workingCompany & "' AND [Status]=4 AND CompletedImported=0 "
            Dim da As New SqlDataAdapter(sql, conn)
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ds.Tables(0)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function OrderIsInvoiced(ByVal Code As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("woCheckOrderInvoiced", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = Code

            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function SetOrderAsInvoiced(ByVal orderCode As String) As Boolean
        Dim connWOR As New SqlConnection(WORconnectionString)
        Try
            Dim sql2 As String = "UPDATE [carrelloHeader] SET [CompletedImported]=1 WHERE [CompanyName_]=@CompanyName AND [Code]=@Code "
            Dim cmd2 As New SqlCommand(sql2, connWOR)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWOR.Open()
            cmd2.ExecuteNonQuery()
            Log.Info("SetOrderAsInvoiced: " & orderCode)
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If connWOR.State = ConnectionState.Open Then connWOR.Close()
        End Try
    End Function

    Public Sub synchOrderInvoicedFromNAV()
        Try

            Dim CodeFromWO As DataTable = GetOrderCodeToInvoice()
            Log.Info("Start synchOrderInvoicedFromNAV. Order to synch: " & CodeFromWO.Rows.Count.ToString())
            For Each rWO As DataRow In CodeFromWO.Rows
                If OrderIsInvoiced(rWO("Code")) Then
                    SetOrderAsInvoiced(rWO("Code"))
                End If
            Next
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
        Finally
        End Try
    End Sub

End Class


