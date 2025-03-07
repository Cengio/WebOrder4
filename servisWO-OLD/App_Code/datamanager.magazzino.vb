Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection
Imports log4net

Partial Public Class datamanager

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Public Function pickOrder(ByVal orderCode As String, ByVal usercode As String) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            Dim ctrl As Integer = 0
            Dim userBC As String = usercode
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT [Utente BC] FROM [WU_Users] WHERE [User Code] = @UserCode"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@UserCode", SqlDbType.VarChar, 20).Value = usercode

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            If reader.Read() AndAlso reader("Utente BC") IsNot DBNull.Value Then
                userBC = reader("Utente BC").ToString()
            End If
            reader.Close()

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_UpdateOrderHeaderFromWeb"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@User", SqlDbType.VarChar, 20).Value = userBC
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 3 'IN LAVORAZIONE DAL MAGAZZINO
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            ctrl = cmd.ExecuteNonQuery()

            cmd.CommandText = "sp_UpdateCarrelloHeader"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@User", SqlDbType.VarChar, 20).Value = usercode
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 3 'IN LAVORAZIONE DAL MAGAZZINO
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            ctrl += cmd.ExecuteNonQuery()

            Return ctrl
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function sendOrderToCtrl(ByVal orderCode As String, ByVal usercode As String) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            Dim userBC As String = usercode 
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT [Utente BC] FROM [WU_Users] WHERE [User Code] = @UserCode"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@UserCode", SqlDbType.VarChar, 20).Value = usercode

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            If reader.Read() AndAlso reader("Utente BC") IsNot DBNull.Value Then
                userBC = reader("Utente BC").ToString()
            End If
            reader.Close()

            Dim ctrl As Integer = 0
            
            ' Using Stored Procedure for DB
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_UpdateOrderHeaderFromWeb"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@User", SqlDbType.VarChar, 20).Value = userBC ' Use userBC value
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 5 'IN FATTURAZIONE
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            ctrl = cmd.ExecuteNonQuery()

            cmd.CommandText = "sp_UpdateCarrelloHeader"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@User", SqlDbType.VarChar, 20).Value = usercode
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 5 'IN FATTURAZIONE
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            ctrl += cmd.ExecuteNonQuery()

            cmd.CommandText = "woRemoveZeroQtyCarrelloLineFromWeb"
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            ctrl += cmd.ExecuteNonQuery()

            Return ctrl
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getLostQuantityByRowLine(ByVal orderLine As carrelloLine) As Integer
        Dim conn As New SqlConnection(NAVconnectionString)
        Try
            If Not orderLine Is Nothing Then
                Dim sql As String = "SELECT LostQuantity FROM [UnloadLostQuantity] WHERE [Ditta]=@Ditta AND [OrderNo]=@OrderNo AND [RowNo]=@RowNo AND [BinCode]=@BinCode AND [LotNo]=@LotNo"
                conn.Open()
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@Ditta", SqlDbType.VarChar, 2).Value = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).dittaid
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.OrderCode
                cmd.Parameters.Add("@RowNo", SqlDbType.Int).Value = orderLine.ordineLine.LineID
                cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.BinCode
                cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.LotNo
                Return cmd.ExecuteScalar
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getUnloadLostQuantity(ByVal orderLine As carrelloLine) As DataRow
        Dim conn As New SqlConnection(NAVconnectionString)
        Try

            Dim sql As String = "SELECT * FROM [UnloadLostQuantity] WHERE [Ditta]=@Ditta AND [OrderNo]=@OrderNo AND [RowNo]=@RowNo AND [BinCode]=@BinCode AND [LotNo]=@LotNo"
            conn.Open()
            Dim cmd As New SqlCommand
            cmd.Parameters.Add("@Ditta", SqlDbType.VarChar, 2).Value = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).dittaid
            cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.OrderCode
            cmd.Parameters.Add("@RowNo", SqlDbType.Int).Value = orderLine.ordineLine.LineID
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.BinCode
            cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.LotNo
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0).Rows(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    'avendo rimosso la sqlTransaction, in caso di errore dopo la DELETE ma prima dell'INSERT non sarà possibile fare rollback
    Public Function addLostQuantity(ByVal lq As unloadlostquantity) As Integer
        Dim conn As New SqlConnection(NAVconnectionString)
        conn.Open()
        Try
            Dim sql As String
            'elimino tutte le righe di unloadlostquantity per quella riga ordine e poi le reinserisco
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            
            sql = "DELETE FROM UnloadLostQuantity WHERE [Ditta]=@Ditta AND [OrderNo]=@OrderNo AND [RowNo]=@RowNo AND [BinCode]=@BinCode AND [LotNo]=@LotNo"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@Ditta", SqlDbType.VarChar, 2).Value = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).dittaid
            cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 20).Value = lq.OrderNo
            cmd.Parameters.Add("@RowNo", SqlDbType.Int).Value = lq.RowNo
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = lq.BinCode
            cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = lq.LotNo
            cmd.ExecuteNonQuery()

            sql = "INSERT INTO [UnloadLostQuantity] ("
            sql &= "ItemCode"
            sql &= " ,LostQuantity"
            sql &= " ,LotNo"
            sql &= " ,LocationCode"
            sql &= " ,InventoryCause"
            sql &= " ,[User]"
            sql &= " ,BinCode"
            sql &= " ,OrderNo"
            sql &= " ,RowNo"
            sql &= " ,Ditta"
            sql &= " ,[Close]"
            sql &= " ,[CloseRestore]"
            sql &= " ,[DateEntry]"
            sql &= ")"
            sql &= " VALUES ("
            sql &= "@ItemCode,"
            sql &= "@LostQuantity,"
            sql &= "@LotNo,"
            sql &= "@LocationCode,"
            sql &= "@InventoryCause,"
            sql &= "@User,"
            sql &= "@BinCode,"
            sql &= "@OrderNo,"
            sql &= "@RowNo,"
            sql &= "@Ditta,"
            sql &= "0,"
            sql &= "0,"
            sql &= "GETDATE()"
            sql &= ")"

            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = lq.ItemCode
            cmd.Parameters.Add("@LostQuantity", SqlDbType.Int).Value = lq.LostQuantity
            cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = lq.LotNo
            cmd.Parameters.Add("@LocationCode", SqlDbType.VarChar, 10).Value = lq.LocationCode
            cmd.Parameters.Add("@InventoryCause", SqlDbType.VarChar, 50).Value = lq.InventoryCause
            cmd.Parameters.Add("@User", SqlDbType.VarChar, 30).Value = lq.User
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = lq.BinCode
            cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 20).Value = lq.OrderNo
            cmd.Parameters.Add("@RowNo", SqlDbType.Int).Value = lq.RowNo
            cmd.Parameters.Add("@Ditta", SqlDbType.VarChar, 2).Value = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).dittaid

            cmd.ExecuteNonQuery()

            Log.Info("Order " & lq.OrderNo & " line " & lq.RowNo & " UnloadLostQuantity updated:" & " lostQty=" & lq.LostQuantity)

            Return 1
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function shipOrder(ByVal orderCode As String, ByVal UserCtrl As String, ByVal peso As Integer, ByVal nrcolli As Integer, ByVal ShippingAgentCode As String) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            Dim ctrl As Integer = 0
            Dim sql As String = ""

            sql = "UPDATE [OrderHeaderFromWeb_Write] SET [OrderNoCtrl]=@OrderNoCtrl, [UserCtrl]=@UserCtrl,[ShippingAgentCode]=@ShippingAgentCode,[PackageNum]=@PackageNum,[Weight]=@Weight,[Status]=@Status,[Imported]=@Imported WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@OrderNoCtrl", SqlDbType.VarChar, 20).Value = orderCode
            cmd.Parameters.Add("@UserCtrl", SqlDbType.VarChar, 20).Value = UserCtrl
            cmd.Parameters.Add("@ShippingAgentCode", SqlDbType.VarChar, 20).Value = ShippingAgentCode
            cmd.Parameters.Add("@PackageNum", SqlDbType.TinyInt).Value = nrcolli
            cmd.Parameters.Add("@Weight", SqlDbType.TinyInt).Value = peso
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 4
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = "SER-VIS"
            'annulla ordine
            cmd.Parameters.Add("@Imported", SqlDbType.Bit).Value = 0
            ctrl = cmd.ExecuteNonQuery()

            sql = "UPDATE [carrelloHeader] SET [OrderNoCtrl]=@OrderNoCtrl, [UserCtrl]=@UserCtrl,[ShippingAgentCode]=@ShippingAgentCode,[PackageNum]=@PackageNum,[Weight]=@Weight,[Status]=@Status WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@OrderNoCtrl", SqlDbType.VarChar, 20).Value = orderCode
            cmd.Parameters.Add("@UserCtrl", SqlDbType.VarChar, 20).Value = UserCtrl
            cmd.Parameters.Add("@ShippingAgentCode", SqlDbType.VarChar, 20).Value = ShippingAgentCode
            cmd.Parameters.Add("@PackageNum", SqlDbType.TinyInt).Value = nrcolli
            cmd.Parameters.Add("@Weight", SqlDbType.TinyInt).Value = peso
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 4
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            ctrl += cmd.ExecuteNonQuery()

            '20190302 - Ordini prenotazione - Sincroniz. se l'ordine è già stato fatturato e quindi gli art. movimentati 
            'al fine del calcolo degli articoli in lavorazione non ancora movimentati in Nav. 
            'Il calcolo avviene lato WO e non più lato NAV essendo i 2 sistemi sincronizzati 
            synchOrderInvoicedFromNAV()

            Return ctrl
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function


    Public Function getMagazzinieri(Optional ByVal addItemTutti As Boolean = True) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT DISTINCT U.[User Code] AS Code, (U.Cognome + ' ' + U.Nome) AS [Description] FROM [WU_users] U inner join WU_usersProfiles UP ON u.[User Code]=UP.[User Code] INNER JOIN WU_profiles P ON UP.idProfile=P.idProfile INNER JOIN WU_profilesRoles PR ON UP.idProfile= PR.idProfile INNER JOIN WU_roles R ON PR.roleCode=R.roleCode WHERE PR.allowed=1 AND P.blocked=0 AND (R.roleCode='magazzino' OR R.roleCode='magazzinoadmin') ORDER BY [Description]"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            If addItemTutti Then
                Dim dr As DataRow = ds.Tables(0).NewRow
                dr("Code") = 0
                dr("Description") = "Visualizza tutti"
                ds.Tables(0).Rows.InsertAt(dr, 0)
            End If
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getOrdiniAssegnati(Optional ByVal userCode As Integer = 0) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT A.Code,A.CustomerNo,A.OperatorCode,A.[status],convert(datetime,(SUBSTRING(A.[OrderDate],7,4) + '-' + SUBSTRING(A.[OrderDate],4,2) + '-' +SUBSTRING(A.[OrderDate],1,2)+ 'T' + '00:00:00.000')) AS dataordine,"
            sql &= " ISNULL(B.id,0) AS id,ISNULL(B.magazziniereCodice,0) AS magazziniereCodice,ISNULL(B.assegnato,0) AS assegnato,ISNULL(B.prioritaRaccolta,0) AS prioritaRaccolta,ISNULL(B.dataAssegnazione,'1900-01-01') AS dataAssegnazione,"
            sql &= " ISNULL((c.Cognome +' ' + C.Nome),'') AS userDescription"
            sql &= " FROM carrelloHeader as A LEFT JOIN MAGordiniAssegnati AS B ON A.Code=B.ordineCodice LEFT JOIN [WU_users] AS C ON B.magazziniereCodice=C.[User Code]"
            sql &= " WHERE A.TIPO='ORDINE' AND [CompanyName_]=@CompanyCode AND (A.[Status]=2 OR(A.[Status]=3 AND A.[User]='')) AND Not A.CustomerNo LIKE 'N%'"
            If userCode > 0 Then
                sql &= " AND (ISNULL(B.magazziniereCodice,0)=@userCode OR ISNULL(B.magazziniereCodice,0)=0)"
            End If
            sql &= " ORDER BY B.dataAssegnazione ASC, convert(datetime,(SUBSTRING(A.[OrderDate],7,4) + '-' + SUBSTRING(A.[OrderDate],4,2) + '-' +SUBSTRING(A.[OrderDate],1,2)+ 'T' + '00:00:00.000')) ASC, A.Code Asc"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            If userCode > 0 Then
                cmd.Parameters.Add("@userCode", SqlDbType.VarChar, 20).Value = userCode
            End If
            conn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Sub assegnaOrdine(ByVal assOrdine As assegnazioneOrdine)
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim cmd As New SqlCommand
            Dim esiste As Boolean = False
            Dim sql As String = "SELECT COUNT(*) FROM [MAGordiniAssegnati] WHERE ordineCodice=@ordineCodice AND CompanyCode=@CompanyCode"
            cmd.Parameters.Add("@ordineCodice", SqlDbType.VarChar).Value = assOrdine.ordineCodice
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            cmd.CommandText = sql
            cmd.Connection = conn
            esiste = CBool(cmd.ExecuteScalar)
            If esiste Then
                If assOrdine.assegnato = 1 Then 'se esiste e assOrdine.assegna=1 -> update
                    sql = "UPDATE MAGordiniAssegnati SET prioritaRaccolta=@prioritaRaccolta WHERE ordineCodice=@ordineCodice AND CompanyCode=@CompanyCode"
                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@prioritaRaccolta", SqlDbType.TinyInt).Value = assOrdine.prioritaRaccolta
                    cmd.Parameters.Add("@ordineCodice", SqlDbType.VarChar).Value = assOrdine.ordineCodice
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
                    cmd.ExecuteNonQuery()
                Else  'se esiste e assOrdine.assegna=0 -> delete
                    sql = "DELETE FROM MAGordiniAssegnati WHERE ordineCodice=@ordineCodice"
                    cmd.CommandText = sql
                    cmd.Parameters.Clear()
                    cmd.Parameters.Add("@ordineCodice", SqlDbType.VarChar).Value = assOrdine.ordineCodice
                    cmd.ExecuteNonQuery()
                End If
            Else 'se non esiste insert 
                sql = "INSERT INTO MAGordiniAssegnati(ordineCodice,magazziniereCodice,assegnato,prioritaRaccolta,dataAssegnazione,CompanyCode) VALUES(@ordineCodice,@magazziniereCodice,1,@prioritaRaccolta,GETDATE(),@CompanyCode)"
                cmd.CommandText = sql
                cmd.Parameters.Clear()
                cmd.Parameters.Add("@ordineCodice", SqlDbType.VarChar).Value = assOrdine.ordineCodice
                cmd.Parameters.Add("@magazziniereCodice", SqlDbType.Int).Value = assOrdine.magazziniereCodice
                cmd.Parameters.Add("@prioritaRaccolta", SqlDbType.TinyInt).Value = assOrdine.prioritaRaccolta
                cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
                cmd.ExecuteNonQuery()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Sub

    Public Function getOrdineDaRaccogliere(ByVal userCode As String) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT A.Code,A.CustomerNo,A.OperatorCode,A.[status],convert(datetime,(SUBSTRING(A.[OrderDate],7,4) + '-' + SUBSTRING(A.[OrderDate],4,2) + '-' +SUBSTRING(A.[OrderDate],1,2)+ 'T' + '00:00:00.000')) AS dataordine,"
            sql &= " ISNULL(B.id,0) AS id,ISNULL(B.magazziniereCodice,0) AS magazziniereCodice,ISNULL(B.assegnato,0) AS assegnato,ISNULL(B.prioritaRaccolta,0) AS prioritaRaccolta,ISNULL(B.dataAssegnazione,'1900-01-01') AS dataAssegnazione,"
            sql &= " ISNULL((c.Cognome +' ' + C.Nome),'') AS userDescription"
            sql &= " FROM carrelloHeader as A LEFT JOIN MAGordiniAssegnati AS B ON A.Code=B.ordineCodice LEFT JOIN [WU_users] AS C ON B.magazziniereCodice=C.[User Code]"
            sql &= " WHERE A.TIPO='ORDINE' AND A.[STATUSMAGAZZINO]=@STATUSMAGAZZINO AND A.[CompanyName_]=@CompanyName AND (A.[Status]=2 OR(A.[Status]=3 AND A.[User]=''))"
            sql &= " AND Not A.CustomerNo LIKE 'N%' AND (ISNULL(B.magazziniereCodice,0)=@userCode OR ISNULL(B.magazziniereCodice,0)=0)"
            sql &= " ORDER BY B.magazziniereCodice DESC, B.prioritaRaccolta DESC, convert(datetime,(SUBSTRING(A.[OrderDate],7,4) + '-' + SUBSTRING(A.[OrderDate],4,2) + '-' +SUBSTRING(A.[OrderDate],1,2)+ 'T' + '00:00:00.000')) ASC, A.Code Asc"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@STATUSMAGAZZINO", SqlDbType.TinyInt).Value = 0
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@userCode", SqlDbType.VarChar, 20).Value = userCode
            conn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getOrdineInRaccoltaByUserCode(ByVal userCode As String) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT * FROM carrelloHeader WHERE TIPO='ORDINE' AND [STATUSMAGAZZINO]=0 AND [CompanyName_]=@CompanyName AND Status=3 AND [User]=@userCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@userCode", SqlDbType.VarChar, 20).Value = userCode
            conn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function rettificaRigaSuLottoSuccessivo(ByVal newLine As carrelloLine) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim ol As orderLine = newLine.ordineLine
            Dim ctrl As Integer = 0
            Dim sql As String = ""
            Dim cmd As New SqlCommand
            cmd.Connection = conn

            ' [OrderLineFromWebV2_Write] Insertion
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
            sql &= ",'SER-VIS'"  '",@CompanyName"
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
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.OrderCode
            cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.ItemCode
            cmd.Parameters.Add("@RowDiscount", SqlDbType.Int).Value = newLine.ordineLine.RowDiscount
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = newLine.ordineLine.LineID
            cmd.Parameters.Add("@OldItemCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.OldItemCode
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = newLine.ordineLine.CompanyName
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = newLine.ordineLine.UnitPrice
            cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = newLine.ordineLine.Quantity
            cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = newLine.ordineLine.DiscountQty
            cmd.Parameters.Add("@Imported", SqlDbType.Int).Value = newLine.ordineLine.Imported
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.BinCode
            cmd.Parameters.Add("@QtyToShip", SqlDbType.Decimal).Value = newLine.ordineLine.QtyToShip
            cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = newLine.ordineLine.LotNo
            cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = newLine.ordineLine.FormulaSconto
            cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = newLine.ordineLine.LineNo
            cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = newLine.ordineLine.OriginalQty
            ctrl = cmd.ExecuteNonQuery()

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
            sql &= " ,GETDATE()"
            sql &= ")"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@IDCART", SqlDbType.BigInt).Value = newLine.IDCART
            cmd.Parameters.Add("@IDCARTLINE", SqlDbType.BigInt).Value = newLine.IDCARTLINE
            cmd.Parameters.Add("@IDPROMO", SqlDbType.BigInt).Value = newLine.IDPROMO
            cmd.Parameters.Add("@DESCRIZIONE", SqlDbType.VarChar, 1000).Value = newLine.DESCRIZIONE
            cmd.Parameters.Add("@DISPONIBILITA", SqlDbType.BigInt).Value = newLine.DISPONIBILITA
            cmd.Parameters.Add("@UNITPRICELIST", SqlDbType.Decimal).Value = newLine.UNITPRICELIST
            cmd.Parameters.Add("@FORMATO", SqlDbType.VarChar, 50).Value = newLine.FORMATO
            cmd.Parameters.Add("@IVA", SqlDbType.VarChar, 50).Value = newLine.IVA
            cmd.Parameters.Add("@MARCHIO", SqlDbType.VarChar, 50).Value = newLine.MARCHIO
            cmd.Parameters.Add("@COMPOSIZIONE", SqlDbType.VarChar, 50).Value = newLine.COMPOSIZIONE
            cmd.Parameters.Add("@TOTALERIGA", SqlDbType.Decimal).Value = newLine.TOTALERIGA
            cmd.Parameters.Add("@DISPOLOTTO", SqlDbType.BigInt).Value = newLine.DISPOLOTTO
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.OrderCode
            cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.ItemCode
            cmd.Parameters.Add("@RowDiscount", SqlDbType.Int).Value = newLine.ordineLine.RowDiscount
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = newLine.ordineLine.LineID
            cmd.Parameters.Add("@OldItemCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.OldItemCode
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = newLine.ordineLine.UnitPrice
            cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = newLine.ordineLine.Quantity
            cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = newLine.ordineLine.DiscountQty
            cmd.Parameters.Add("@Imported", SqlDbType.Int).Value = newLine.ordineLine.Imported
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = newLine.ordineLine.BinCode
            cmd.Parameters.Add("@QtyToShip", SqlDbType.Decimal).Value = newLine.ordineLine.QtyToShip
            cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = newLine.ordineLine.LotNo
            cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = newLine.ordineLine.FormulaSconto
            cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = newLine.ordineLine.LineNo
            cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = newLine.ordineLine.OriginalQty
            cmd.Parameters.Add("@Loaded", SqlDbType.Int).Value = newLine.LOADED
            cmd.Parameters.Add("@LineIdSource", SqlDbType.BigInt).Value = newLine.LINEIDSOURCE
            ctrl = cmd.ExecuteNonQuery()

            Log.Info("rettificaRigaSuLottoSuccessivo " & "Order " & newLine.ordineLine.OrderCode & " line " & newLine.ordineLine.LineID & " Item " & newLine.ordineLine.ItemCode & " " & newLine.ordineLine.LotNo & " " & newLine.ordineLine.Quantity & " " & newLine.ordineLine.DiscountQty)

            Return ctrl

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function eliminaRigaNAV_DoporettificaRiga(ByVal currentRow As carrelloLine) As Integer
        Dim conn As New SqlConnection(NAVconnectionString)
        conn.Open()
        Try
            Dim ctrl As Integer = 0
            Dim sql As String
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            sql = "DELETE FROM [OrderLineFromWebV2_Write] WHERE orderCode=@orderCode AND LineID=@LineID AND [CompanyName_]=@CompanyName"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.OrderCode
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = currentRow.ordineLine.LineID
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            ctrl = cmd.ExecuteNonQuery()

            Log.Info("eliminaRigaNAV_DoporettificaRiga " & currentRow.ordineLine.OrderCode & " " & currentRow.ordineLine.LineID & " " & GetWorkingCompany)

            Return ctrl
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function restoreRettifica(ByVal currentRow As carrelloLine) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim ctrl As Integer = 0
            Dim recExists As Boolean = False
            Dim sql As String
            Dim cmd As New SqlCommand
            cmd.Connection = conn

            ' DELETE statement
            sql = "DELETE FROM UnloadLostQuantity WHERE [Ditta]=@Ditta AND [OrderNo]=@OrderNo AND [RowNo]=@RowNo AND [BinCode]=@BinCode AND [LotNo]=@LotNo"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@Ditta", SqlDbType.VarChar, 2).Value = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).dittaid
            cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.OrderCode
            cmd.Parameters.Add("@RowNo", SqlDbType.Int).Value = currentRow.ordineLine.LineID
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.BinCode
            cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.LotNo
            ctrl += cmd.ExecuteNonQuery()
            Log.Info("Restore UnloadLostQuantity Order " & currentRow.ordineLine.OrderCode & " line " & currentRow.IDCARTLINE)

            ' CHECK IF RECORD EXISTS
            sql = "SELECT COUNT(*) FROM [OrderLineFromWebV2_Write] WHERE orderCode=@orderCode AND LineID=@LineID"
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.OrderCode
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = currentRow.ordineLine.LineID
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            If cmd.ExecuteScalar() > 0 Then
                recExists = True
            End If

            'ricreo riga originaria in NAV
            'If ((currentRow.ordineLine.RowDiscount = 0 And currentRow.ordineLine.Quantity <= 0) Or (currentRow.ordineLine.RowDiscount = 1 And currentRow.ordineLine.DiscountQty <= 0)) And currentRow.LINEIDSOURCE = 0 Then
            If Not recExists And currentRow.LINEIDSOURCE = 0 Then
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
                cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.OrderCode
                cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.ItemCode
                cmd.Parameters.Add("@RowDiscount", SqlDbType.Int).Value = currentRow.ordineLine.RowDiscount
                cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = currentRow.ordineLine.LineID
                cmd.Parameters.Add("@OldItemCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.OldItemCode
                cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = currentRow.ordineLine.CompanyName
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = currentRow.ordineLine.UnitPrice
                cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = currentRow.ordineLine.Quantity
                cmd.Parameters.Add("@DiscountQty", SqlDbType.Int).Value = currentRow.ordineLine.DiscountQty
                cmd.Parameters.Add("@Imported", SqlDbType.Int).Value = currentRow.ordineLine.Imported
                cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.BinCode
                cmd.Parameters.Add("@QtyToShip", SqlDbType.Decimal).Value = currentRow.ordineLine.QtyToShip
                cmd.Parameters.Add("@LotNo", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.LotNo
                cmd.Parameters.Add("@FormulaSconto", SqlDbType.VarChar, 50).Value = currentRow.ordineLine.FormulaSconto
                cmd.Parameters.Add("@LineNo", SqlDbType.Int).Value = currentRow.ordineLine.LineNo
                cmd.Parameters.Add("@OriginalQty", SqlDbType.Decimal).Value = currentRow.ordineLine.OriginalQty
                ctrl = cmd.ExecuteNonQuery()
                Log.Info("Restore deleted line in NAV Order " & currentRow.ordineLine.OrderCode & " line " & currentRow.IDCARTLINE)
            End If

            If currentRow.ordineLine.RowDiscount = 0 Then
                sql = "UPDATE [OrderLineFromWebV2_Write] SET [QtyToShip]=0,[Quantity]=[OriginalQty] WHERE [OrderCode]=@OrderCode AND LineID=@LineID"
            Else
                sql = "UPDATE [OrderLineFromWebV2_Write] SET [QtyToShip]=0,[DiscountQty]=[OriginalQty] WHERE [OrderCode]=@OrderCode AND LineID=@LineID"
            End If
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.OrderCode
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = currentRow.ordineLine.LineID
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            ctrl += cmd.ExecuteNonQuery()
            Log.Info("Restore updated line in NAV Order " & currentRow.ordineLine.OrderCode & " line " & currentRow.ordineLine.LineID)

            If currentRow.ordineLine.RowDiscount = 0 Then
                sql = "UPDATE [carrelloLines] SET [Loaded]=0, [QtyToShip]=0, [Quantity]=[OriginalQty] WHERE [OrderCode]=@OrderCode AND LineID=@LineID AND [CompanyName_]=@CompanyName"
            Else
                sql = "UPDATE [carrelloLines] SET [Loaded]=0, [QtyToShip]=0, [DiscountQty]=[OriginalQty] WHERE [OrderCode]=@OrderCode AND LineID=@LineID AND [CompanyName_]=@CompanyName"
            End If
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = currentRow.ordineLine.OrderCode
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = currentRow.ordineLine.LineID
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            ctrl += cmd.ExecuteNonQuery()
            Log.Info("Restore updated line in WO Order " & currentRow.ordineLine.OrderCode & " line " & currentRow.ordineLine.LineID)

            Log.Info("restoreRettifica Commit WO eseguito")
            Log.Info("restoreRettifica Commit NAV eseguito")

            Call restoreRettificaDaLottiSuccessivi(currentRow)

            Return ctrl
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function restoreRettificaDaLottiSuccessivi(ByVal line As carrelloLine) As Integer
        Dim ctrl As Integer = 0
        Dim lineIdsToDelete As New List(Of Integer)

        Using readConn As New SqlConnection(WORconnectionString)
            readConn.Open()
            Using cmdReader As New SqlCommand("SELECT LineID FROM [carrelloLines] WHERE [OrderCode]=@OrderCode AND LineIdSource=@LineId AND [CompanyName_]=@CompanyName", readConn)
                cmdReader.Parameters.AddWithValue("@OrderCode", line.ordineLine.OrderCode)
                cmdReader.Parameters.AddWithValue("@LineId", line.ordineLine.LineID)
                cmdReader.Parameters.AddWithValue("@CompanyName", GetWorkingCompany())

                Using dr As SqlDataReader = cmdReader.ExecuteReader()
                    While dr.Read()
                        lineIdsToDelete.Add(Convert.ToInt32(dr("LineID")))
                    End While
                End Using
            End Using
        End Using  

        Using writeConn As New SqlConnection(WORconnectionString)
            writeConn.Open()
            For Each lineId As Integer In lineIdsToDelete
                ' Delete da [OrderLineFromWebV2_Write]
                Using cmdDeleteOrderLine As New SqlCommand("DELETE FROM [OrderLineFromWebV2_Write] WHERE [OrderCode]=@OrderCode AND LineId=@LineId AND [CompanyName_]='SER-VIS'", writeConn)
                    cmdDeleteOrderLine.Parameters.AddWithValue("@OrderCode", line.ordineLine.OrderCode)
                    cmdDeleteOrderLine.Parameters.AddWithValue("@LineId", lineId)
                    ctrl += cmdDeleteOrderLine.ExecuteNonQuery()
                End Using

                ' Delete da [carrelloLines]
                Using cmdDeleteCarrello As New SqlCommand("DELETE FROM [carrelloLines] WHERE OrderCode=@OrderCode AND LineIdSource=@LineId AND CompanyName_='Ser-Vis Srl'", writeConn)
                    cmdDeleteCarrello.Parameters.AddWithValue("@OrderCode", line.ordineLine.OrderCode)
                    cmdDeleteCarrello.Parameters.AddWithValue("@LineId", line.ordineLine.LineID)
                    ctrl += cmdDeleteCarrello.ExecuteNonQuery()
                End Using
            Next
        End Using

        Return ctrl
    End Function


    Public Function aggiornaQuantitàDaMagazzino(ByVal orderLine As carrelloLine, ByVal quantitaDaCaricare As Integer) As carrelloLine
        Dim conn As New SqlConnection(WORconnectionString)
        conn.Open()
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn

            Dim ctrl As Integer = 0
            Dim sql As String = ""

            If orderLine.ordineLine.RowDiscount = 0 Then
                sql = "UPDATE [carrelloLines] SET [Loaded]=1, [QtyToShip]=@quantitaDaCaricare, [Quantity]=@quantitaDaCaricare, DataEntry=GETDATE() WHERE [CompanyName_]=@CompanyName AND [OrderCode]=@OrderCode AND LineID=@LineID"
            Else
                sql = "UPDATE [carrelloLines] SET [Loaded]=1, [QtyToShip]=@quantitaDaCaricare, [DiscountQty]=@quantitaDaCaricare, DataEntry=GETDATE() WHERE [CompanyName_]=@CompanyName AND [OrderCode]=@OrderCode AND LineID=@LineID"
            End If
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@quantitaDaCaricare", SqlDbType.Decimal).Value = quantitaDaCaricare
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.OrderCode
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = orderLine.ordineLine.LineID

            ctrl = cmd.ExecuteNonQuery()

            If orderLine.ordineLine.RowDiscount = 0 Then
                sql = "UPDATE [OrderLineFromWebV2_Write] SET [QtyToShip]=@quantitaDaCaricare,[Quantity]=@quantitaDaCaricare WHERE [OrderCode]=@OrderCode AND LineID=@LineID "
            Else
                sql = "UPDATE [OrderLineFromWebV2_Write] SET [QtyToShip]=@quantitaDaCaricare,[DiscountQty]=@quantitaDaCaricare WHERE [OrderCode]=@OrderCode AND LineID=@LineID "
            End If
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@quantitaDaCaricare", SqlDbType.Decimal).Value = quantitaDaCaricare
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = orderLine.ordineLine.OrderCode
            cmd.Parameters.Add("@LineID", SqlDbType.Int).Value = orderLine.ordineLine.LineID

            ctrl += cmd.ExecuteNonQuery()

            If ctrl > 0 Then
                orderLine.LOADED = 1
                orderLine.ordineLine.QtyToShip = quantitaDaCaricare
                If orderLine.ordineLine.RowDiscount = 0 Then
                    orderLine.ordineLine.Quantity = quantitaDaCaricare
                Else
                    orderLine.ordineLine.DiscountQty = quantitaDaCaricare
                End If
            End If

            Return orderLine
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
            Return Nothing
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getLineIdSource(OrderCode As String, LineID As Integer) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim sql As String = "select lineidsource from carrellolines where OrderCode=@OrderCode AND LineID=@LineID AND [CompanyName_]=@CompanyName"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar).Value = OrderCode
            cmd.Parameters.Add("@LineID", SqlDbType.BigInt).Value = LineID
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="orderCode"></param>
    ''' <param name="STATUSMAGAZZINO">0 = da raccogliere / 1 = sospeso</param>
    ''' <param name="operatorCode"></param>
    ''' <returns></returns>
    Public Function cambiaSospensioneRaccoltaOrdine(ByVal orderCode As String, ByVal STATUSMAGAZZINO As Integer, ByVal operatorCode As String) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [carrelloHeader] SET "
            sql &= "[STATUSMAGAZZINO]=@STATUSMAGAZZINO "
            sql &= ",UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA "
            sql &= ",DATA_ULTIMA_MODIFICA=GETDATE() "
            sql &= " WHERE [Code]=@Code AND [CompanyName_]=@CompanyName"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@STATUSMAGAZZINO", SqlDbType.TinyInt).Value = STATUSMAGAZZINO
            cmd.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            conn.Open()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
            Return False
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getOrdiniMagazzinoList(status As Integer, operatorcode As Integer) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT A.[Code],A.[CustomerNo],A.[Status],A.[User],A.[OperatorCode],A.[CompletedImported],A.[OrderNoCtrl],A.[UserCtrl],A.[STATUSMAGAZZINO] "
            sql &= " ,Convert(DateTime, (SUBSTRING(A.[OrderDate], 7, 4) + '-' + SUBSTRING(A.[OrderDate],4,2) + '-' +SUBSTRING(A.[OrderDate],1,2) + 'T' + '00:00:00.000')) as OrderDate2, N.NumRighe,CONVERT(INT,N.PezziInOrdine) AS PezziInOrdine "
            sql &= " FROM [carrelloHeader] AS A WITH(NOLOCK) LEFT JOIN dbo.V_AggrCarrelloLines N ON A.Code = N.OrderCode WHERE  A.[CompanyName_]=@CompanyName  AND (A.[Status]=2 OR A.[Status]=3 OR A.[Status]=5) AND A.Code<>'' AND (LEN(A.[OrderDate])=10) AND A.[OrderDate]<>'' AND NOT A.CustomerNo LIKE 'N%'"

            sql &= " AND (A.PRENOTAZIONE=0 OR A.PRENOTAZIONE=2) "
            If status = 3 Then
                sql &= " AND (A.[Status]=@Status AND A.[User]<>'') "
            ElseIf status = 2 Then
                sql &= " AND (A.[Status]=@Status OR(A.[Status]=3 AND A.[User]='')) "
            Else
                sql &= " AND (A.[Status]=@Status) "
            End If

            If operatorcode >= 0 Then
                sql &= " AND A.[User]=@userCode "
            End If
            sql &= " ORDER BY A.PRENOTAZIONE DESC, A.Code, OrderDate2 ASC"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.CommandText = sql
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@Status", SqlDbType.Int).Value = status
            'If statusMagazzino >= 0 Then
            '    cmd.Parameters.Add("@STATUSMAGAZZINO", SqlDbType.TinyInt).Value = statusMagazzino
            'End If
            If operatorcode >= 0 Then
                cmd.Parameters.Add("@userCode", SqlDbType.VarChar, 20).Value = operatorcode
            End If
            conn.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getHeaderOrdiniMagazzino(ByVal Code As String) As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT * FROM [carrelloHeader] WHERE [CompanyName_]=@CompanyName AND [Code]=@Code"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            da.SelectCommand.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = Code
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
            Return ds
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="orderCode"></param>
    ''' <param name="orderByField">LineID / BinCode</param>
    ''' <returns></returns>
    Public Function getLinesOrdiniMagazzino(ByVal orderCode As String, Optional ByVal orderByField As String = "LineID") As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT * FROM [carrelloLines] WITH(NOLOCK) WHERE [CompanyName_]=@CompanyName AND [orderCode]=@orderCode "
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
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Log.Error(ex.ToString)
            Throw New Exception(ex.Message)
            Return ds
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function




#Region "Gestione BinCode"

    Public Function GetDefaultBinCodeByItemcode(ByVal itemcode As String, ByVal locationcode As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select [Bin Code] FROM [NAV_Bin Content] WHERE [Item No_]=@itemcode AND [Location Code]=@locationcode AND [Bin Code] <> '' ORDER BY [Default] DESC"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@itemcode", SqlDbType.VarChar, 20).Value = itemcode
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = locationcode
            Dim result As Object = cmd.ExecuteScalar()
            If result Is Nothing Then
                result = String.Empty
            End If
            Return result
        Catch ex As Exception
            Throw New Exception("Errore GetBinCodeByItemcode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetBinCodeListByItemcode(ByVal itemcode As String, ByVal locationcode As String) As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select [Bin Code] FROM [NAV_Bin Content] WHERE [Item No_]=@itemcode AND [Location Code]=@locationcode AND [Bin Code] <> '' ORDER BY [Default] DESC"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@itemcode", SqlDbType.VarChar, 20).Value = itemcode
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = locationcode
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception("Errore GetBinCodeListByItemcode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function esiteBinCodeForItemcode(ByVal itemcode As String, ByVal locationcode As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select COUNT(*) FROM [NAV_Bin Content] WHERE [Item No_]=@itemcode AND [Location Code]=@locationcode ORDER BY [Default] DESC"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@itemcode", SqlDbType.VarChar, 20).Value = itemcode
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = locationcode
            Dim count As Integer = 0
            count = cmd.ExecuteScalar
            If count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Errore esiteBinCodeForItemcode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetBinCodeDescription(ByVal bincode As String, ByVal locationcode As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select [Description] FROM [NAV_Bin] WHERE [Code]=@bincode AND [Location Code]=@locationcode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@bincode", SqlDbType.VarChar, 20).Value = bincode
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = locationcode
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception("Errore GetBinCodeByItemcode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetBinCodeListFromNAV() As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT DISTINCT(A.Code),A.[Location Code],A.[Description] FROM [NAV_Bin] AS A LEFT JOIN [NAV_Bin Content] AS B ON A.Code=B.[Bin Code]"
            sql &= " WHERE A.[Location Code]=@locationcode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = GetParametroSito(parametriSitoValue.magazzinodefault)
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

    Public Function GetBinCodeListFromWOR(Optional ByVal BinCode As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT * FROM [BinCode] WHERE [LocationCode]=@locationcode"
            If BinCode <> "" Then
                sql &= " AND BinCode=@BinCode"
            End If
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = GetParametroSito(parametriSitoValue.magazzinodefault)
            If BinCode <> "" Then
                cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = BinCode
            End If
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

    Public Function esiteBinCodeInWOR(ByVal BinCode As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select COUNT(*) FROM [BinCode] WHERE BinCode=@BinCode AND [LocationCode]=@locationcode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = BinCode
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = GetParametroSito(parametriSitoValue.magazzinodefault)
            Dim count As Integer = 0
            count = cmd.ExecuteScalar
            If count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function addBinCodeToWOR(ByVal BinCode As String, ByVal LocationCode As String, ByVal Description As String) As Integer
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "INSERT [BinCode](BinCode,LocationCode,[Description],ImageFileName,WebDescription) VALUES(@BinCode,@locationcode,@Description,@ImageFileName,@WebDescription)"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = BinCode
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = LocationCode
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, 50).Value = Description
            cmd.Parameters.Add("@ImageFileName", SqlDbType.VarChar, 512).Value = ""
            cmd.Parameters.Add("@WebDescription", SqlDbType.VarChar, 512).Value = Description
            Return cmd.ExecuteNonQuery
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Sub synchBinCodeFromNAV()
        Try
            Dim binFromNav As DataTable = GetBinCodeListFromNAV()
            For Each rNav As DataRow In binFromNav.Rows
                If Not esiteBinCodeInWOR(rNav("Code")) Then
                    addBinCodeToWOR(rNav("Code"), rNav("Location Code"), rNav("Description"))
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
        End Try
    End Sub

    Public Function UpdBinCodeImage(ByVal BinCode As String, ByVal ImageFileName As String, ByVal WebDescription As String) As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "UPDATE [BinCode] SET ImageFileName=@ImageFileName, WebDescription=@WebDescription WHERE BinCode=@BinCode AND A.[Location Code]=@locationcode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@ImageFileName", SqlDbType.VarChar, 512).Value = ImageFileName
            cmd.Parameters.Add("@WebDescription", SqlDbType.VarChar, 512).Value = WebDescription
            cmd.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = BinCode
            cmd.Parameters.Add("@locationcode", SqlDbType.VarChar, 10).Value = GetParametroSito(parametriSitoValue.magazzinodefault)
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

    Public Sub CheckBinCodeAfterPick(ByVal orderCode As String)
        Log.Info("CheckBinCodeAfterPick START for order: " & orderCode)
        Dim connNAV As New SqlConnection(NAVconnectionString)
        Dim connWO As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "SELECT ItemCode, BinCode, LineID FROM [carrelloLines] WHERE [CompanyName_]=@CompanyName AND OrderCode=@OrderCode"
            Dim cmd As New SqlCommand(sql, connWO)
            cmd.Parameters.Clear()
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
            Dim da As New SqlDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)

            Dim locationCode As String = GetParametroSito(parametriSitoValue.magazzinodefault)
            Dim cmdWO As New SqlCommand
            Dim cmdNAV As New SqlCommand
            Dim sqlUpdWOR As String = "UPDATE [carrelloLines] SET BinCode=@BinCode WHERE [CompanyName_]=@CompanyName AND OrderCode=@OrderCode AND LineID=@LineID "
            Dim sqlUpdNAV As String = "UPDATE [OrderLineFromWebV2_Write] SET BinCode=@BinCode WHERE OrderCode=@OrderCode AND LineID=@LineID "
            For Each rowLine As DataRow In ds.Tables(0).Rows
                Dim currentLine As Integer = rowLine("LineID")
                Dim currentItemCode As String = rowLine("ItemCode")
                Dim currentBinCode As String = rowLine("BinCode")
                Dim defaultBinCode As String = GetDefaultBinCodeByItemcode(currentItemCode, locationCode)
                If defaultBinCode <> String.Empty AndAlso currentBinCode <> defaultBinCode Then
                    'Update WOR BinCode in Line
                    cmdWO = New SqlCommand(sqlUpdWOR, connWO)
                    cmdWO.Parameters.Clear()
                    cmdWO.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = defaultBinCode
                    cmdWO.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
                    cmdWO.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
                    cmdWO.Parameters.Add("@LineID", SqlDbType.Int).Value = currentLine
                    If connWO.State = ConnectionState.Closed Then connWO.Open()
                    cmdWO.ExecuteNonQuery()
                    'Update NAV BinCode in Line
                    cmdNAV = New SqlCommand(sqlUpdNAV, connNAV)
                    cmdNAV.Parameters.Clear()
                    cmdNAV.Parameters.Add("@BinCode", SqlDbType.VarChar, 20).Value = defaultBinCode
                    cmdNAV.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
                    cmdNAV.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
                    cmdNAV.Parameters.Add("@LineID", SqlDbType.Int).Value = currentLine
                    If connNAV.State = ConnectionState.Closed Then connNAV.Open()
                    cmdNAV.ExecuteNonQuery()
                    Log.Info("Update BinCode from " & currentBinCode & " to " & defaultBinCode & " for ItemCode " & currentItemCode & " in Order " & orderCode & " LineID " & currentLine.ToString)
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If connNAV.State = ConnectionState.Open Then connNAV.Close()
            If connWO.State = ConnectionState.Open Then connWO.Close()
            Log.Info("CheckBinCodeAfterPick END for order: " & orderCode)
        End Try
    End Sub



#End Region



End Class
