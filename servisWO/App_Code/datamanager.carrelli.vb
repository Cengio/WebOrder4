Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function getCarrelloHeader(ByVal idCarrello As Int64) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "SELECT * FROM [carrelloHeader] WHERE [IDCART]=@IDCART"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = idCarrello
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getIDCarrelloByOrderCode(ByVal orderCode As String) As Int64
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim idcart As Int64 = 0
            Dim sql As String = "SELECT IDCART FROM [carrelloHeader] WHERE [Code]=@orderCode AND [CompanyName_]=@CompanyCode"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet()
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                idcart = ds.Tables(0).Rows(0).Item("IDCART")
            End If
            Return idcart
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getCarrelloLinee(Optional ByVal idCarrello As Int64 = 0, Optional ByVal orderByCollocazione As Boolean = False) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [carrelloLines] "
            If idCarrello > 0 Then
                sql &= " WHERE [IDCART]=@IDCART"
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = idCarrello
            End If
            If orderByCollocazione Then
                sql &= " ORDER BY "
                sql &= " LEFT(bincode,6) "
                sql &= ",SUBSTRING(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)), 1,2) "
                sql &= ",SUBSTRING(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)) ,3,2) "
                sql &= ",CAST(SUBSTRING(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)), 6, CASE WHEN LEN(SUBSTRING(bincode,CHARINDEX('-',bincode)+1,LEN(bincode)))=8 THEN 1 WHEN LEN(SUBSTRING(bincode, CHARINDEX('-',bincode)+1,LEN(bincode)))=9 THEN 2 END ) AS INT)"
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getCarrelliHeaderByClienteNo(ByVal CodiceCliente As String) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "Select * FROM [carrelloHeader] WHERE [TIPO]='CARRELLO' AND [CODICECLIENTE]=@CODICECLIENTE AND [CompanyName_]=@CompanyCode ORDER BY IDCART DESC"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CODICECLIENTE", SqlDbType.VarChar, 30).Value = CodiceCliente
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function salvaCarrello2(ByVal carrello As cartManager, Optional ByVal sincronizzaOrdineCollegato As Boolean = True, Optional ByVal soloHeader As Boolean = False) As Int64
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Dim returnIdCarrello As Int64 = 0
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            Dim sql As String = ""

            If carrello.Header.IDCART > 0 Then 'carrello esistente
                sql = "UPDATE [carrelloHeader] SET "
                sql &= " [DESCRIPTION]=@DESCRIPTION,"
                sql &= " [CODICECLIENTE]=@CODICECLIENTE,"
                sql &= " [CODICECONTATTO]=@CODICECONTATTO,"
                sql &= " [UTENTE_ULTIMA_MODIFICA]=@UTENTE_ULTIMA_MODIFICA,"
                sql &= " [AGENTE]=@AGENTE,"
                sql &= " [DATA_ULTIMA_MODIFICA]=GETDATE(),"
                sql &= " [TIPO]=@TIPO,"
                sql &= " [SCONTOPAGAMENTO]=@SCONTOPAGAMENTO,"
                sql &= " [IMPORTOEVADIBILITAORDINE]=@IMPORTOEVADIBILITAORDINE,"
                sql &= " [IMPORTOPORTOFRANCO]=@IMPORTOPORTOFRANCO,"
                sql &= " [IMPORTOSPESESPEDIZIONE]=@IMPORTOSPESESPEDIZIONE,"
                sql &= " [SCONTOHEADER]=@SCONTOHEADER,"
                sql &= " [Type]=@Type,"
                sql &= " [ShippingAgentCode]=@ShippingAgentCode,"
                sql &= " [ShipAddressCode]=@ShipAddressCode,"
                sql &= " [Notes]=@Notes,"
                sql &= " [IncludeShipCost]=@IncludeShipCost,"
                sql &= " [PaymentMethodCode]=@PaymentMethodCode,"
                sql &= " [PaymentTermsCode]=@PaymentTermsCode,"
                sql &= " [CustomerNo]=@CustomerNo,"
                sql &= " [Code]=@Code,"
                sql &= " [OperatorCode]=@OperatorCode,"
                sql &= " [OrderDate]=@OrderDate,"
                sql &= " [idprofilosconto]=@idprofilosconto,"
                sql &= " [Voucher Value]=@Voucher_Value,"
                sql &= " [DATA_EVASIONE]=@DATA_EVASIONE "
                sql &= " WHERE [IDCART]=@IDCART"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@DESCRIPTION", SqlDbType.VarChar, 512).Value = carrello.Header.DESCRIPTION
                cmd.Parameters.Add("@CODICECLIENTE", SqlDbType.VarChar, 50).Value = carrello.Header.CODICECLIENTE
                cmd.Parameters.Add("@CODICECONTATTO", SqlDbType.VarChar, 50).Value = carrello.Header.CODICECONTATTO
                cmd.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = carrello.Header.UTENTE_ULTIMA_MODIFICA
                cmd.Parameters.Add("@AGENTE", SqlDbType.BigInt).Value = carrello.Header.AGENTE
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar, 50).Value = carrello.Header.TIPO.ToString
                cmd.Parameters.Add("@SCONTOPAGAMENTO", SqlDbType.Decimal).Value = carrello.Header.SCONTOPAGAMENTO
                cmd.Parameters.Add("@IMPORTOEVADIBILITAORDINE", SqlDbType.Decimal).Value = carrello.Header.IMPORTOEVADIBILITAORDINE
                cmd.Parameters.Add("@IMPORTOPORTOFRANCO", SqlDbType.Decimal).Value = carrello.Header.IMPORTOPORTOFRANCO
                cmd.Parameters.Add("@IMPORTOSPESESPEDIZIONE", SqlDbType.Decimal).Value = carrello.Header.IMPORTOSPESESPEDIZIONE
                cmd.Parameters.Add("@SCONTOHEADER", SqlDbType.VarChar, 10).Value = carrello.Header.SCONTOHEADER
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 25).Value = carrello.Header.ordineHeader.Type
                cmd.Parameters.Add("@ShippingAgentCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.ShippingAgentCode
                cmd.Parameters.Add("@ShipAddressCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.ShipAddressCode
                cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 250).Value = carrello.Header.ordineHeader.Notes
                cmd.Parameters.Add("@IncludeShipCost", SqlDbType.TinyInt).Value = carrello.Header.ordineHeader.IncludeShipCost
                cmd.Parameters.Add("@PaymentMethodCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.PaymentMethodCode
                cmd.Parameters.Add("@PaymentTermsCode", SqlDbType.VarChar, 10).Value = carrello.Header.ordineHeader.PaymentTermsCode
                cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.CustomerNo
                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.Code
                cmd.Parameters.Add("@OperatorCode", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OperatorCode
                cmd.Parameters.Add("@OrderDate", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.OrderDate
                cmd.Parameters.Add("@idprofilosconto", SqlDbType.BigInt).Value = carrello.Header.IDPROFILOSCONTO
                cmd.Parameters.Add("@Voucher_Value", SqlDbType.Decimal).Value = carrello.Header.ordineHeader.Voucher_Value
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = carrello.Header.IDCART
                cmd.Parameters.Add("@DATA_EVASIONE", SqlDbType.DateTime).Value = carrello.Header.DATA_EVASIONE

                cmd.ExecuteNonQuery()

                If Not soloHeader Then
                    sql = "DELETE FROM [carrelloLines] WHERE [IDCART]=@IDCART"
                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = carrello.Header.IDCART
                    cmd.ExecuteNonQuery()

                    For Each line As carrelloLine In carrello.carrelloLines
                        sql = "INSERT INTO [carrelloLines] ("
                        sql &= "  IDCART"
                        sql &= " ,IDCARTLINE"
                        sql &= " ,IDPROMO"
                        sql &= " ,DESCRIZIONE"
                        sql &= " ,DISPONIBILITA"
                        sql &= " ,UNITPRICELIST"
                        sql &= " ,FORMATO"
                        sql &= " ,IVA"
                        sql &= " ,MARCHIO"
                        sql &= " ,COMPOSIZIONE"
                        sql &= " ,TOTALERIGA"
                        sql &= " ,DISPOLOTTO"
                        sql &= " ,OrderCode"
                        sql &= " ,ItemCode"
                        sql &= " ,RowDiscount"
                        sql &= " ,LineID"
                        sql &= " ,OldItemCode"
                        sql &= " ,[CompanyName_]"
                        sql &= " ,UnitPrice"
                        sql &= " ,Quantity"
                        sql &= " ,DiscountQty"
                        sql &= " ,Imported"
                        sql &= " ,BinCode"
                        sql &= " ,QtyToShip"
                        sql &= " ,LotNo"
                        sql &= " ,FormulaSconto"
                        sql &= " ,[LineNo]"
                        sql &= " ,OriginalQty"
                        sql &= " ,[Loaded]"
                        sql &= " ,[LineIdSource]"
                        sql &= " ,[DataEntry]"
                        sql &= " ,SCONTOHEADER"
                        sql &= " ,SCONTORIGA"
                        sql &= " ,SCONTOPAGAMENTO"
                        sql &= ")"
                        sql &= " VALUES ("
                        sql &= "  @IDCART"
                        sql &= " ,@IDCARTLINE"
                        sql &= " ,@IDPROMO"
                        sql &= " ,@DESCRIZIONE"
                        sql &= " ,@DISPONIBILITA"
                        sql &= " ,@UNITPRICELIST"
                        sql &= " ,@FORMATO"
                        sql &= " ,@IVA"
                        sql &= " ,@MARCHIO"
                        sql &= " ,@COMPOSIZIONE"
                        sql &= " ,@TOTALERIGA"
                        sql &= " ,@DISPOLOTTO"
                        sql &= " ,@OrderCode"
                        sql &= " ,@ItemCode"
                        sql &= " ,@RowDiscount"
                        sql &= " ,@LineID"
                        sql &= " ,@OldItemCode"
                        sql &= " ,@CompanyName"
                        sql &= " ,@UnitPrice"
                        sql &= " ,@Quantity"
                        sql &= " ,@DiscountQty"
                        sql &= " ,@Imported"
                        sql &= " ,@BinCode"
                        sql &= " ,@QtyToShip"
                        sql &= " ,@LotNo"
                        sql &= " ,@FormulaSconto"
                        sql &= " ,@LineNo"
                        sql &= " ,@OriginalQty"
                        sql &= " ,@Loaded"
                        sql &= " ,@LineIdSource"
                        sql &= " ,@DataEntry"
                        sql &= " ,@SCONTOHEADER"
                        sql &= " ,@SCONTORIGA"
                        sql &= " ,@SCONTOPAGAMENTO"
                        sql &= ")"
                        cmd.CommandText = sql
                        cmd.Parameters.Clear()
                        cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = line.IDCART
                        cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = line.IDCARTLINE
                        cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = line.IDPROMO
                        cmd.Parameters.Add("@DESCRIZIONE", SqlDbType.VarChar, 1000).Value = line.DESCRIZIONE
                        cmd.Parameters.Add("@DISPONIBILITA", SqlDbType.BigInt).Value = line.DISPONIBILITA
                        cmd.Parameters.Add("@UNITPRICELIST", SqlDbType.Decimal).Value = line.UNITPRICELIST
                        cmd.Parameters.Add("@FORMATO", SqlDbType.VarChar, 50).Value = line.FORMATO
                        cmd.Parameters.Add("@IVA", SqlDbType.VarChar, 50).Value = line.IVA
                        cmd.Parameters.Add("@MARCHIO", SqlDbType.VarChar, 50).Value = line.MARCHIO
                        cmd.Parameters.Add("@COMPOSIZIONE", SqlDbType.VarChar, 50).Value = line.COMPOSIZIONE
                        cmd.Parameters.Add("@TOTALERIGA", SqlDbType.Decimal).Value = line.TOTALERIGA
                        cmd.Parameters.Add("@DISPOLOTTO", SqlDbType.BigInt).Value = line.DISPOLOTTO
                        cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = line.ordineLine.OrderCode
                        cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = line.ordineLine.ItemCode
                        cmd.Parameters.Add("@RowDiscount", SqlDbType.Int).Value = line.ordineLine.RowDiscount
                        cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = line.ordineLine.LineID
                        cmd.Parameters.Add("@OldItemCode", SqlDbType.VarChar, 20).Value = line.ordineLine.OldItemCode
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
                        cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = line.ordineLine.UnitPrice
                        cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = line.ordineLine.Quantity
                        cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = line.ordineLine.DiscountQty
                        cmd.Parameters.Add("@Imported", SqlDbType.Int).Value = line.ordineLine.Imported
                        cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = line.ordineLine.BinCode
                        cmd.Parameters.Add("@QtyToShip", SqlDbType.Decimal).Value = line.ordineLine.QtyToShip
                        cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = line.ordineLine.LotNo
                        cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = line.ordineLine.FormulaSconto
                        cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = line.ordineLine.LineNo
                        cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = line.ordineLine.OriginalQty
                        cmd.Parameters.Add("@Loaded", SqlDbType.Int).Value = line.LOADED
                        cmd.Parameters.Add("@LineIdSource", SqlDbType.BigInt).Value = line.LINEIDSOURCE
                        cmd.Parameters.Add("@DataEntry", SqlDbType.DateTime).Value = line.DATEENTRY
                        cmd.Parameters.Add("@SCONTOHEADER", SqlDbType.VarChar, 10).Value = line.SCONTOHEADER
                        cmd.Parameters.Add("@SCONTORIGA", SqlDbType.Decimal).Value = line.SCONTORIGA
                        cmd.Parameters.Add("@SCONTOPAGAMENTO", SqlDbType.Decimal).Value = line.SCONTOPAGAMENTO
                        cmd.ExecuteScalar()
                    Next
                End If


                returnIdCarrello = carrello.Header.IDCART

            Else 'nuovo carrello

                'Dim newidcarrello As Int64 = GetCartProgressive()
                If carrello.Header.DESCRIPTION = "" Then
                    carrello.Header.DESCRIPTION = carrello.Header.ordineHeader.CustomerNo & "-" & Now.ToString
                End If

                sql = "INSERT INTO [carrelloHeader] ("
                sql &= "  [DESCRIPTION]"
                sql &= " ,[CODICECLIENTE]"
                sql &= " ,[CODICECONTATTO]"
                sql &= " ,[UTENTE_CREAZIONE]"
                sql &= " ,[DATA_CREAZIONE]"
                sql &= " ,[UTENTE_ULTIMA_MODIFICA]"
                sql &= " ,[AGENTE]"
                sql &= " ,[DATA_ULTIMA_MODIFICA]"
                sql &= " ,[BLOCKED]"
                sql &= " ,[TIPO]"
                sql &= " ,[SCONTOPAGAMENTO]"
                sql &= " ,[IMPORTOEVADIBILITAORDINE]"
                sql &= " ,[IMPORTOPORTOFRANCO]"
                sql &= " ,[IMPORTOSPESESPEDIZIONE]"
                sql &= " ,[SCONTOHEADER]"
                sql &= " ,[Code]"
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
                sql &= ",[IDPROFILOSCONTO]"
                sql &= ",[ORIGINATODAORDINE]"
                sql &= ",[Voucher Value]"
                sql &= ",[PRENOTAZIONE]"
                sql &= ",[DATA_EVASIONE]"
                sql &= ") VALUES ("
                sql &= "@DESCRIPTION"
                sql &= ",@CODICECLIENTE"
                sql &= ",@CODICECONTATTO"
                sql &= ",@UTENTE_CREAZIONE"
                sql &= ",GETDATE()"
                sql &= ",@UTENTE_ULTIMA_MODIFICA"
                sql &= ",@AGENTE"
                sql &= ",GETDATE()"
                sql &= ",0"
                sql &= ",@TIPO"
                sql &= " ,@SCONTOPAGAMENTO"
                sql &= " ,@IMPORTOEVADIBILITAORDINE"
                sql &= " ,@IMPORTOPORTOFRANCO"
                sql &= " ,@IMPORTOSPESESPEDIZIONE"
                sql &= " ,@SCONTOHEADER"
                sql &= ",@Code"
                sql &= ",@CompanyName"
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
                sql &= ",@IDPROFILOSCONTO"
                sql &= ",@ORIGINATODAORDINE"
                sql &= ",@Voucher_Value"
                sql &= ",@PRENOTAZIONE"
                sql &= ",@DATA_EVASIONE"
                sql &= ");"
                sql &= "Select Scope_Identity()"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@DESCRIPTION", SqlDbType.VarChar, 512).Value = carrello.Header.DESCRIPTION
                cmd.Parameters.Add("@CODICECLIENTE", SqlDbType.VarChar, 30).Value = carrello.Header.CODICECLIENTE
                cmd.Parameters.Add("@CODICECONTATTO", SqlDbType.VarChar, 50).Value = carrello.Header.CODICECONTATTO
                cmd.Parameters.Add("@UTENTE_CREAZIONE", SqlDbType.BigInt).Value = carrello.Header.UTENTE_CREAZIONE
                cmd.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = carrello.Header.UTENTE_ULTIMA_MODIFICA
                cmd.Parameters.Add("@AGENTE", SqlDbType.BigInt).Value = carrello.Header.AGENTE
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar, 50).Value = carrello.Header.TIPO.ToString
                cmd.Parameters.Add("@SCONTOPAGAMENTO", SqlDbType.Decimal).Value = carrello.Header.SCONTOPAGAMENTO
                cmd.Parameters.Add("@IMPORTOEVADIBILITAORDINE", SqlDbType.Decimal).Value = carrello.Header.IMPORTOEVADIBILITAORDINE
                cmd.Parameters.Add("@IMPORTOPORTOFRANCO", SqlDbType.Decimal).Value = carrello.Header.IMPORTOPORTOFRANCO
                cmd.Parameters.Add("@IMPORTOSPESESPEDIZIONE", SqlDbType.Decimal).Value = carrello.Header.IMPORTOSPESESPEDIZIONE
                cmd.Parameters.Add("@SCONTOHEADER", SqlDbType.VarChar, 10).Value = carrello.Header.SCONTOHEADER
                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = carrello.Header.ordineHeader.Code
                cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
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
                cmd.Parameters.Add("@IDPROFILOSCONTO", SqlDbType.BigInt).Value = carrello.Header.IDPROFILOSCONTO
                cmd.Parameters.Add("@ORIGINATODAORDINE", SqlDbType.VarChar, 50).Value = carrello.Header.ORIGINATODAORDINE
                cmd.Parameters.Add("@Voucher_Value", SqlDbType.Decimal).Value = carrello.Header.ordineHeader.Voucher_Value
                cmd.Parameters.Add("@PRENOTAZIONE", SqlDbType.TinyInt).Value = carrello.Header.PRENOTAZIONE
                cmd.Parameters.Add("@DATA_EVASIONE", SqlDbType.DateTime).Value = carrello.Header.DATA_EVASIONE

                Dim newidcarrello As Integer = cmd.ExecuteScalar()

                For Each line As carrelloLine In carrello.carrelloLines
                    sql = "INSERT INTO [carrelloLines] ("
                    sql &= "  IDCART"
                    sql &= " ,IDCARTLINE"
                    sql &= " ,IDPROMO"
                    sql &= " ,DESCRIZIONE"
                    sql &= " ,DISPONIBILITA"
                    sql &= " ,UNITPRICELIST"
                    sql &= " ,FORMATO"
                    sql &= " ,IVA"
                    sql &= " ,MARCHIO"
                    sql &= " ,COMPOSIZIONE"
                    sql &= " ,TOTALERIGA"
                    sql &= " ,DISPOLOTTO"
                    sql &= " ,OrderCode"
                    sql &= " ,ItemCode"
                    sql &= " ,RowDiscount"
                    sql &= " ,LineID"
                    sql &= " ,OldItemCode"
                    sql &= " ,[CompanyName_]"
                    sql &= " ,UnitPrice"
                    sql &= " ,Quantity"
                    sql &= " ,DiscountQty"
                    sql &= " ,Imported"
                    sql &= " ,BinCode"
                    sql &= " ,QtyToShip"
                    sql &= " ,LotNo"
                    sql &= " ,FormulaSconto"
                    sql &= " ,[LineNo]"
                    sql &= " ,OriginalQty"
                    sql &= " ,[Loaded]"
                    sql &= " ,[LineIdSource]"
                    sql &= " ,[DataEntry]"
                    sql &= " ,SCONTOHEADER"
                    sql &= " ,SCONTORIGA"
                    sql &= " ,SCONTOPAGAMENTO"
                    sql &= ")"
                    sql &= " VALUES ("
                    sql &= "  @IDCART"
                    sql &= " ,@IDCARTLINE"
                    sql &= " ,@IDPROMO"
                    sql &= " ,@DESCRIZIONE"
                    sql &= " ,@DISPONIBILITA"
                    sql &= " ,@UNITPRICELIST"
                    sql &= " ,@FORMATO"
                    sql &= " ,@IVA"
                    sql &= " ,@MARCHIO"
                    sql &= " ,@COMPOSIZIONE"
                    sql &= " ,@TOTALERIGA"
                    sql &= " ,@DISPOLOTTO"
                    sql &= " ,@OrderCode"
                    sql &= " ,@ItemCode"
                    sql &= " ,@RowDiscount"
                    sql &= " ,@LineID"
                    sql &= " ,@OldItemCode"
                    sql &= " ,@CompanyName"
                    sql &= " ,@UnitPrice"
                    sql &= " ,@Quantity"
                    sql &= " ,@DiscountQty"
                    sql &= " ,@Imported"
                    sql &= " ,@BinCode"
                    sql &= " ,@QtyToShip"
                    sql &= " ,@LotNo"
                    sql &= " ,@FormulaSconto"
                    sql &= " ,@LineNo"
                    sql &= " ,@OriginalQty"
                    sql &= " ,@Loaded"
                    sql &= " ,@LineIdSource"
                    sql &= " ,@DataEntry"
                    sql &= " ,@SCONTOHEADER"
                    sql &= " ,@SCONTORIGA"
                    sql &= " ,@SCONTOPAGAMENTO"
                    sql &= ")"
                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = newidcarrello
                    cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = line.IDCARTLINE
                    cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = line.IDPROMO
                    cmd.Parameters.Add("@DESCRIZIONE", SqlDbType.VarChar, 1000).Value = line.DESCRIZIONE
                    cmd.Parameters.Add("@DISPONIBILITA", SqlDbType.BigInt).Value = line.DISPONIBILITA
                    cmd.Parameters.Add("@UNITPRICELIST", SqlDbType.Decimal).Value = line.UNITPRICELIST
                    cmd.Parameters.Add("@FORMATO", SqlDbType.VarChar, 50).Value = line.FORMATO
                    cmd.Parameters.Add("@IVA", SqlDbType.VarChar, 50).Value = line.IVA
                    cmd.Parameters.Add("@MARCHIO", SqlDbType.VarChar, 50).Value = line.MARCHIO
                    cmd.Parameters.Add("@COMPOSIZIONE", SqlDbType.VarChar, 50).Value = line.COMPOSIZIONE
                    cmd.Parameters.Add("@TOTALERIGA", SqlDbType.Decimal).Value = line.TOTALERIGA
                    cmd.Parameters.Add("@DISPOLOTTO", SqlDbType.BigInt).Value = line.DISPOLOTTO
                    cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = line.ordineLine.OrderCode
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
                    cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = line.ordineLine.FormulaSconto
                    cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = line.ordineLine.LineNo
                    cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = line.ordineLine.OriginalQty
                    cmd.Parameters.Add("@Loaded", SqlDbType.Int).Value = line.LOADED
                    cmd.Parameters.Add("@LineIdSource", SqlDbType.BigInt).Value = line.LINEIDSOURCE
                    cmd.Parameters.Add("@DataEntry", SqlDbType.DateTime).Value = line.DATEENTRY
                    cmd.Parameters.Add("@SCONTOHEADER", SqlDbType.VarChar, 10).Value = line.SCONTOHEADER
                    cmd.Parameters.Add("@SCONTORIGA", SqlDbType.Decimal).Value = line.SCONTORIGA
                    cmd.Parameters.Add("@SCONTOPAGAMENTO", SqlDbType.Decimal).Value = line.SCONTOPAGAMENTO
                    cmd.ExecuteScalar()
                Next

                returnIdCarrello = newidcarrello
            End If

            If Not soloHeader Then
                sql = "DELETE FROM [carrelloHeaderPromo] WHERE [IDCART]=@IDCART"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = carrello.Header.IDCART
                cmd.ExecuteNonQuery()
                For Each item As carrelloHeaderPromo In carrello.carrelloHeaderPromos
                    sql = "INSERT INTO [carrelloHeaderPromo] ([IDCART],[IDPROMO],[NRUTILIZZI],[CTCODE],[ACTION],[ACTIONDATE]) VALUES (@IDCART,@IDPROMO,@NRUTILIZZI,@CTCODE,@ACTION,@ACTIONDATE)"
                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = item.IDCART
                    cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = item.IDPROMO
                    cmd.Parameters.Add("@NRUTILIZZI", SqlDbType.BigInt).Value = item.NRUTILIZZI
                    cmd.Parameters.Add("@CTCODE", SqlDbType.VarChar, 30).Value = item.CTCODE
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar, 10).Value = item.ACTION
                    cmd.Parameters.Add("@ACTIONDATE", SqlDbType.DateTime).Value = item.ACTIONDATE
                    cmd.ExecuteNonQuery()
                Next

                sql = "DELETE FROM [carrelloLinesPromo] WHERE [IDCART]=@IDCART"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = carrello.Header.IDCART
                cmd.ExecuteNonQuery()
                For Each item As carrelloLinePromo In carrello.carrelloLinePromos
                    sql = "INSERT INTO [carrelloLinesPromo] ([IDCART],[IDCARTLINE],[IDPROMO],[NRMULTIPLI],[CTCODE]) VALUES (@IDCART,@IDCARTLINE,@IDPROMO,@NRMULTIPLI,@CTCODE)"
                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = item.IDCART
                    cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = item.IDCARTLINE
                    cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = item.IDPROMO
                    cmd.Parameters.Add("@NRMULTIPLI", SqlDbType.BigInt).Value = item.NRMULTIPLI
                    cmd.Parameters.Add("@CTCODE", SqlDbType.VarChar, 30).Value = item.CTCODE
                    cmd.ExecuteNonQuery()
                Next
            End If

            Try
                If sincronizzaOrdineCollegato Then
                    If carrello.Header.TIPO = carrelloHeader.TIPOCARRELLO.ORDINE AndAlso carrello.Header.ordineHeader.Code <> "" Then
                        updOrderHeader(carrello)
                        If Not soloHeader Then
                            updOrderLines(carrello)
                        End If
                    End If
                End If
            Catch ex As Exception
                Return 0
            End Try

            Return returnIdCarrello
        
        Catch ex As Exception
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Sub salvaDispo(ByVal carrello As cartManager)
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            Dim sql As String = ""
            
            If carrello.Header.IDCART > 0 Then
                For Each line As carrelloLine In carrello.carrelloLines
                    sql = "UPDATE [carrelloLines] SET [DISPONIBILITA]=@DISPONIBILITA, [DISPOLOTTO]=@DISPOLOTTO WHERE [IDCART]=@IDCART AND [IDCARTLINE]=@IDCARTLINE"
                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@DISPONIBILITA", SqlDbType.BigInt).Value = line.DISPONIBILITA
                    cmd.Parameters.Add("@DISPOLOTTO", SqlDbType.BigInt).Value = line.DISPOLOTTO
                    cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = line.IDCART
                    cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = line.IDCARTLINE
                    cmd.ExecuteNonQuery()
                Next
            End If
        Catch ex As Exception
            ' Consider logging the error and/or rethrowing it or showing it to the user.
            ' Example: LogError(ex)
            Throw
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub


    Public Function getCarrelloPromo(ByVal IDCART As Int64) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [carrelloHeaderPromo] WHERE IDCART=@IDCART"
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = IDCART
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function esisteCarrelloPromo(ByVal cp As carrelloHeaderPromo) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT COUNT(*) FROM [carrelloHeaderPromo] WHERE IDCART=@IDCART AND IDPROMO=@IDPROMO"
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cp.IDCART
            cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cp.IDPROMO
            cmd.CommandText = sql
            cmd.Connection = conn
            Return CBool(cmd.ExecuteScalar)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function addCarrelloPromo(ByVal cp As carrelloHeaderPromo) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = ""
            If Not esisteCarrelloPromo(cp) Then
                sql = "INSERT INTO [carrelloHeaderPromo] ([IDCART],[IDPROMO],[NRUTILIZZI],[CTCODE],[ACTION],[ACTIONDATE])"
                sql &= " VALUES (@IDCART,@IDPROMO,@NRUTILIZZI,@CTCODE,@ACTION,@ACTIONDATE)"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cp.IDCART
                cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cp.IDPROMO
                cmd.Parameters.Add("@NRUTILIZZI", SqlDbType.BigInt).Value = cp.NRUTILIZZI
                cmd.Parameters.Add("@CTCODE", SqlDbType.VarChar, 30).Value = cp.CTCODE
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar, 10).Value = cp.ACTION
                cmd.Parameters.Add("@ACTIONDATE", SqlDbType.DateTime).Value = cp.ACTIONDATE
                conn.Open()
                Return cmd.ExecuteScalar
            Else
                sql = "UPDATE [carrelloHeaderPromo] SET NRUTILIZZI=NRUTILIZZI+1, [ACTIONDATE]=@ACTIONDATE "
                sql &= " WHERE IDCART=@IDCART AND IDPROMO=@IDPROMO"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@ACTIONDATE", SqlDbType.DateTime).Value = cp.ACTIONDATE
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cp.IDCART
                cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cp.IDPROMO
                conn.Open()
                Return cmd.ExecuteScalar
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function delCarrelloPromo(ByVal cp As carrelloHeaderPromo) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "DELETE FROM [carrelloHeaderPromo] WHERE IDCART=@IDCART "
            If cp.IDPROMO > 0 Then
                sql &= " AND IDPROMO=@IDPROMO"
            End If
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cp.IDCART
            If cp.IDPROMO > 0 Then
                cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cp.IDPROMO
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Return CBool(cmd.ExecuteScalar)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getCarrelloPromoLines(ByVal IDCART As Int64) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [carrelloLinesPromo] WHERE IDCART=@IDCART"
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = IDCART
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function esisteCarrelloPromoLines(ByVal cpl As carrelloLinePromo) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT COUNT(*) FROM [carrelloLinesPromo] WHERE IDCART=@IDCART AND IDCARTLINE=@IDCARTLINE AND IDPROMO=@IDPROMO"
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cpl.IDCART
            cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = cpl.IDCARTLINE
            cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cpl.IDPROMO
            cmd.CommandText = sql
            cmd.Connection = conn
            Return CBool(cmd.ExecuteScalar)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function addCarrelloPromoLines(ByVal cpl As carrelloLinePromo) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = ""
            If Not esisteCarrelloPromoLines(cpl) Then
                sql = "INSERT INTO [carrelloLinesPromo] ([IDCART],[IDCARTLINE],[IDPROMO],[NRMULTIPLI],[CTCODE])"
                sql &= " VALUES (@IDCART,@IDCARTLINE,@IDPROMO,@NRMULTIPLI,@CTCODE)"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cpl.IDCART
                cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = cpl.IDCARTLINE
                cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cpl.IDPROMO
                cmd.Parameters.Add("@NRMULTIPLI", SqlDbType.BigInt).Value = cpl.NRMULTIPLI
                cmd.Parameters.Add("@CTCODE", SqlDbType.VarChar, 30).Value = cpl.CTCODE
                conn.Open()
                Return cmd.ExecuteScalar
            Else
                sql = "UPDATE [carrelloLinesPromo] SET NRMULTIPLI=NRMULTIPLI+1 "
                sql &= " WHERE IDCART=@IDCART AND IDCARTLINE=@IDCARTLINE AND IDPROMO=@IDPROMO"
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cpl.IDCART
                cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = cpl.IDCARTLINE
                cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cpl.IDPROMO
                conn.Open()
                Return cmd.ExecuteScalar
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            'MsgBox(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function delCarrelloPromoLines(ByVal cpl As carrelloLinePromo) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "DELETE FROM [carrelloLinesPromo] WHERE IDCART=@IDCART AND IDPROMO=@IDPROMO"
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = cpl.IDCART
            cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = cpl.IDPROMO
            cmd.CommandText = sql
            cmd.Connection = conn
            Return CBool(cmd.ExecuteScalar)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function delAllCarrelloPromoLines(ByVal IDCART As Integer) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "DELETE FROM [carrelloLinesPromo] WHERE IDCART=@IDCART "
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = IDCART
            cmd.CommandText = sql
            cmd.Connection = conn
            Return CBool(cmd.ExecuteScalar)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getUtilizziPromoLineByCODECT_IDPROMO(ByVal CodiceContatto As String, ByVal IDPROMO As Int64) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim sql As String = "SELECT COUNT(*) AS numOccorrenze FROM [carrelloHeader] AS A INNER JOIN [carrelloLines] AS B ON A.idcart=B.idcart WHERE A.[CODICECONTATTO]=@CODICECONTATTO AND B.[IDPROMO]=@IDPROMO"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CodiceContatto", SqlDbType.VarChar, 30).Value = CodiceContatto
            cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = IDPROMO
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getUtilizziPromoHeaderByCODECT_IDPROMO(ByVal CodiceContatto As String, ByVal IDPROMO As Int64) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim sql As String = "SELECT COUNT(*) AS numOccorrenze FROM [carrelloHeader] AS A INNER JOIN [carrelloHeaderPromo] AS B ON A.idcart=B.idcart WHERE A.[CODICECONTATTO]=@CODICECONTATTO AND B.[IDPROMO]=@IDPROMO"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CodiceContatto", SqlDbType.VarChar, 30).Value = CodiceContatto
            cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = IDPROMO
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getUtilizziPromoHeaderByIDPROMO(ByVal IDPROMO As Int64) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim sql As String = "SELECT COUNT(*) AS numOccorrenze FROM [carrelloHeaderPromo] WHERE [IDPROMO]=@IDPROMO"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = IDPROMO
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Sub updateCarrelloNote(ByVal IDCART As Integer, ByVal note As String, ByVal UserCode As Integer)
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand("UPDATE [carrelloHeader] SET [Notes]=@Notes, [UTENTE_ULTIMA_MODIFICA]=@UTENTE_ULTIMA_MODIFICA, [DATA_ULTIMA_MODIFICA]=GETDATE() WHERE [IDCART]=@IDCART", conn)
            cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 250).Value = note
            cmd.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = UserCode
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = IDCART
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

End Class
