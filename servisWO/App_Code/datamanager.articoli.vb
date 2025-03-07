Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function getDimensionDescription(ByVal dimensiontype As String, ByVal code As String) As String
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = ""
            Select Case dimensiontype
                Case "macrolinea"
                    sql = "SELECT TOP 1 Macrolinea FROM [Items_Nav] WHERE CompanyCode=@CompanyCode AND NonVisInOrdWeb=0 AND bloccato=0 AND cod_macrolinea<>'0110' AND cod_macrolinea=@code"
                Case "linea"
                    sql = "SELECT TOP 1 Linea FROM [Items_Nav] WHERE CompanyCode=@CompanyCode AND NonVisInOrdWeb=0 AND bloccato=0 AND cod_linea<>'9998' AND cod_linea=@code"
                Case "famiglia"
                    sql = "SELECT Famiglia FROM [Items_Nav] WHERE CompanyCode=@CompanyCode AND NonVisInOrdWeb=0 AND bloccato=0 AND cod_famiglia<>'FAM_340' AND cod_famiglia=@code"
                Case "sottofamiglia"
                    sql = "SELECT SottoFamiglia FROM [Items_Nav] CompanyCode=@CompanyCode AND WHERE NonVisInOrdWeb=0 AND bloccato=0 AND cod_SottFam<>'SOT_110' AND cod_SottFam=@code "
            End Select
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 10).Value = code
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function


    ''' <summary>
    ''' Restituisce l'elenco degli articoli con disponibilità totale
    ''' </summary>
    ''' <param name="itemcode"></param>
    ''' <param name="filterString"></param>
    ''' <param name="soloAttivi"></param>
    ''' <param name="cod_macrolinea"></param>
    ''' <param name="cod_linea"></param>
    ''' <param name="cod_famiglia"></param>
    ''' <param name="cod_sottofamiglia"></param>
    ''' <param name="escludiArcheopatici"></param>
    ''' <param name="dispoReale">se False restituisce solo le qta fatturare</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWOItems(Optional ByVal itemcode As String = "", Optional ByVal filterString As String = "", Optional ByVal soloAttivi As Boolean = True, Optional ByVal cod_macrolinea As String = "", Optional ByVal cod_linea As String = "", Optional ByVal cod_famiglia As String = "", Optional ByVal cod_sottofamiglia As String = "", Optional ByVal escludiArcheopatici As String = "", Optional ByVal dispoReale As Boolean = True, Optional ByVal escludiExpo As String = "", Optional ByVal CodiceListino As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            'System.Diagnostics.Debug.WriteLine(Now() & ": START richiesta Item_NAV")
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            Dim sqlW As String = " WHERE "
            Dim sqlA As String = " AND "
            If CodiceListino = "" Then
                CodiceListino = getListinoBase()
            End If
            'sql = "SELECT CodiceArticolo,Farmadati,CodEAN,Obiettivo,Descrizione,CONVERT(Decimal(18,2),PrzRivenditore) AS PrzRivenditore,CONVERT(Decimal(18,2),PrzPubblico) AS PrzPubblico,Formato,IVA,Marchio,Composizione,cod_macrolinea FROM [Items_Nav]  with(NOLOCK) "
            sql = "SELECT A.CodiceArticolo,Farmadati,CodEAN,Obiettivo,A.Descrizione,Formato,Marchio,Composizione,cod_macrolinea"
            sql &= " ,CONVERT(Decimal(18,2),B.PrezzoRivenditore) As PrzRivenditore, CONVERT(Decimal(18,2),B.PrezzoPubblico) As PrzPubblico, B.IVA FROM [Items_Nav] As A With(NOLOCK) "
            sql &= " INNER JOIN ListiniArticoli As B On (A.CodiceArticolo=B.CodiceArticolo AND A.CompanyCode=B.CompanyCode) "
            sql &= " WHERE B.CodiceListino=@CodiceListino "
            sql &= " AND A.CompanyCode=@CompanyCode AND B.CompanyCode=@CompanyCode "
            If soloAttivi Then
                sql &= " AND NonVisInOrdWeb=0 And bloccato=0 "
            Else
                sql &= " AND (NonVisInOrdWeb=0 Or bloccato=0 Or NonVisInOrdWeb=1 Or bloccato=1) "
            End If

            cmd.Parameters.Add("@CodiceListino", SqlDbType.VarChar, 50).Value = CodiceListino
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany

            If itemcode <> "" Then
                sql &= " And A.CodiceArticolo=@itemcode"
                cmd.Parameters.Add("@itemcode", SqlDbType.NVarChar, 20).Value = itemcode
            End If
            If filterString <> "" Then
                sql &= " And (A.CodiceArticolo Like @filterString Or A.Descrizione Like @filterString Or A.Farmadati Like @filterString Or A.CodEAN Like @filterString)"
                cmd.Parameters.Add("@filterString", SqlDbType.NVarChar).Value = "%" & filterString & "%"
            End If
            If cod_macrolinea <> "" Then
                sql &= " And cod_macrolinea=@cod_macrolinea "
                cmd.Parameters.Add("@cod_macrolinea", SqlDbType.NVarChar, 10).Value = cod_macrolinea
            End If
            If cod_linea <> "" Then
                sql &= " And cod_linea=@cod_linea "
                cmd.Parameters.Add("@cod_linea", SqlDbType.NVarChar, 10).Value = cod_linea
            End If
            If cod_famiglia <> "" Then
                sql &= " And cod_famiglia=@cod_famiglia "
                cmd.Parameters.Add("@cod_famiglia", SqlDbType.NVarChar, 10).Value = cod_famiglia
            End If
            If cod_sottofamiglia <> "" Then
                sql &= " And cod_sottfam=@cod_sottofamiglia "
                cmd.Parameters.Add("@cod_sottofamiglia", SqlDbType.NVarChar, 10).Value = cod_sottofamiglia
            End If
            If escludiArcheopatici = "1" AndAlso cod_macrolinea <> "0010" Then
                sql &= " And cod_macrolinea<>'0010' "
            End If
            If escludiExpo = "1" Then
                sql &= " AND A.CodiceArticolo NOT IN (SELECT CodiceExpo FROM Expo WHERE CompanyCode=@CompanyCode) "
            End If
            sql &= " ORDER BY A.Descrizione "


            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Dim dtItemNav As New DataTable
            dtItemNav = ds.Tables(0)
            dtItemNav.Columns.Add(New DataColumn With {.ColumnName = "Disponibilita", .DataType = Type.GetType("System.Int64"), .DefaultValue = 0})

            Dim dtDispo As New DataTable
            Dim dtTempResult As New DataTable
            Dim dtResult As New DataTable
            dtTempResult = dtItemNav.Clone
            dtResult = dtItemNav.Clone
            'dtDispo = GetDisponibilitaProdotti(itemcode, dispoReale)
            dtDispo = GetDisponibilitaProdotti(itemcode, dispoReale, filterString)

            Dim result = (From item In dtItemNav.AsEnumerable() Join dispo In dtDispo.AsEnumerable()
                         On item.Field(Of String)("CodiceArticolo") Equals dispo.Field(Of String)("Item No_")
                          Select dtTempResult.LoadDataRow(New Object() {item.Field(Of String)("CodiceArticolo"),
                                                                    item.Field(Of String)("Farmadati"),
                                                                    item.Field(Of String)("CodEAN"),
                                                                    item.Field(Of String)("Obiettivo"),
                                                                    item.Field(Of String)("Descrizione"),
                                                                    item.Field(Of String)("Formato"),
                                                                    item.Field(Of String)("Marchio"),
                                                                    item.Field(Of String)("Composizione"),
                                                                    item.Field(Of String)("cod_macrolinea"),
                                                                    item.Field(Of Decimal)("PrzRivenditore"),
                                                                    item.Field(Of Decimal)("PrzPubblico"),
                                                                    item.Field(Of String)("IVA"),
                                                                    dispo.Field(Of Int64)("Disponibilita")},
                                                                 True))
            If result.LongCount > 0 Then
                dtResult = result.CopyToDataTable()
            End If

            Return dtResult
        Catch ex As Exception
            Throw New Exception(ex.Message)
            'MsgBox("Errore GetItems: (" & itemcode & ") " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
            'System.Diagnostics.Debug.WriteLine(Now() & ": END richiesta Item_NAV")
        End Try
    End Function

    ''' <summary>
    ''' Restituisce la disponibilità totale del prodotto regitrata in NAV
    ''' </summary>
    ''' <param name="CodiceArticolo"></param>
    ''' <param name="dispoReale">True: sottrae le quantità già in lavorazione non ancora registrate in NAV; Default = False</param>
    ''' <returns></returns>
    Public Function GetDisponibilitaProdotti(Optional ByVal CodiceArticolo As String = "", Optional dispoReale As Boolean = False, Optional ByVal filterString As String = "") As DataTable
        'Public Function GetDisponibilitaProdotti(Optional ByVal CodiceArticolo As String = "", Optional dispoReale As Boolean = False) As DataTable
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()

            Dim cmd As New SqlCommand("[dbo].[woGetDisponibilitaProdotti_NEW]", conn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@CodiceArticolo", SqlDbType.NVarChar, 20).Value = If(String.IsNullOrEmpty(CodiceArticolo), DBNull.Value, CodiceArticolo)
            cmd.Parameters.Add("@LocationCode", SqlDbType.NVarChar, 50).Value = GetParametroSito(parametriSitoValue.magazzinodefault)
            cmd.Parameters.Add("@filterString", SqlDbType.NVarChar, 100).Value = If(String.IsNullOrEmpty(filterString), DBNull.Value, filterString)

            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds)
            Dim dtresult As DataTable = ds.Tables(0)

            If dispoReale Then  'tolgo le quantità già in lavorazione non ancora registrate in nav
                Dim dtQtaImp As DataTable = getQtaInLavorazione(CodiceArticolo)
                For Each row As DataRow In dtQtaImp.Rows
                    For Each r As DataRow In dtresult.Rows
                        If row("ItemCode") = r("Item No_") Then
                            If row("qtaLavorazione") > 0 Then
                                r("Disponibilita") = r("Disponibilita") - row("qtaLavorazione")
                            End If
                            Exit For
                        End If
                    Next
                Next
            End If
            ' Non mostrare quantità negative
            For Each row As DataRow In dtresult.Rows
                If row("Disponibilita") < 0 Then
                    row("Disponibilita") = 0
                End If
            Next
            If dtresult.Rows.Count = 0 AndAlso CodiceArticolo <> "" Then
                dtresult.Rows.Add(CodiceArticolo, 0)
            End If
            Return dtresult
        Catch ex As Exception
            Throw
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function


    ''' <summary>
    ''' Restituisce i lotti disponibili per l'articolo specificato
    ''' </summary>
    ''' <param name="CodiceArticolo">Item Code</param>
    ''' <param name="dispoReale">se False ignora le qta del prodotto per lotto in lavorazione non ancora fatturate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDisponibilitaProdottiPerLotto(Optional ByVal CodiceArticolo As String = "", Optional ByVal dispoReale As Boolean = True) As DataTable
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String
            sql = "SELECT [Item No_], [Lot No_] AS Lotto, [Expiration Date] AS ScadenzaLotto, ISNULL(CONVERT(bigint, SUM([Remaining Quantity])), 0) AS DisponibilitaLotto, "
            sql &= " CASE Len([Lot No_]) When 8 Then SUBSTRING([Lot No_] , 2, 2) "
            sql &= " 				 	 WHEN 7 THEN SUBSTRING([Lot No_] , 1, 2) END AS P, "
            sql &= " CASE LEN([Lot No_]) WHEN 8 THEN SUBSTRING([Lot No_] , 4, 2) "
            sql &= " 		             WHEN 7 THEN SUBSTRING([Lot No_] , 3, 2) END AS W, "
            sql &= " CASE LEN([Lot No_]) WHEN 8 THEN SUBSTRING([Lot No_] , 6, 1) "
            sql &= " 					 WHEN 7 THEN SUBSTRING([Lot No_] , 5, 1) END AS D, "
            sql &= " CASE LEN([Lot No_]) WHEN 8 THEN SUBSTRING([Lot No_] , 7, 2) "
            sql &= " 					 WHEN 7 THEN SUBSTRING([Lot No_] , 6, 2) END AS Y "
            sql &= " FROM [BCData].[dbo].[Ser-Vis$Item Ledger Entry] With(NOLOCK) "
            sql &= " WHERE [Open]=1 And [Remaining Quantity]>0 And (LEN([Lot No_])=8 Or LEN([Lot No_])=7) And ([Expiration Date]>=GETDATE() Or [Expiration Date]='1900-01-01' Or [Expiration Date]='1753-01-01' OR [Product Group Code]='COSMETICI' OR [Product Group Code]='COSMETICO' OR [Product Group Code]='USOESTERNO') "
            sql &= " AND ([Return Reason Code] <> 'R_ROTTO') AND ([Return Reason Code] <> 'R_SCADUTO') AND ([Return Reason Code] <> 'R_NONCONF') AND ([Reason Code] <> 'P_ROTTO') AND ([Reason Code] <> 'P_SCADUTO') AND ([Reason Code] <> 'P_NONCONF') "
            sql &= " AND [Location Code]='SADRIANO2'"
            If CodiceArticolo <> "" Then
                sql &= " AND [item No_] = @CodiceArticolo"
            End If
            sql &= " GROUP BY [item No_], [Lot No_], [Expiration Date] "
            sql &= "ORDER BY [item No_], Y, W, D, P "

            Dim cmd As New SqlCommand(sql, conn)
            If CodiceArticolo <> "" Then
                cmd.Parameters.Add("@CodiceArticolo", SqlDbType.VarChar, 20).Value = CodiceArticolo
            End If
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds)
            Dim dtresult As DataTable = ds.Tables(0)

            If dispoReale Then  'tolgo le quantità già in lavorazione non ancora registrate in nav
                Dim dtQtaImp As DataTable = getQtaInLavorazionePerLotto(CodiceArticolo)
                For Each row As DataRow In dtQtaImp.Rows
                    For Each r As DataRow In dtresult.Rows
                        If row("ItemCode") = r("Item No_") And row("LotNo") = r("Lotto") Then
                            If row("qtaLavorazione") > 0 Then
                                r("DisponibilitaLotto") = r("DisponibilitaLotto") - row("qtaLavorazione")
                            End If
                            Exit For
                        End If
                    Next
                Next
            End If

            Return dtresult
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getQtaInLavorazione(Optional ByVal CodiceArticolo As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String
            sql = "WITH InitialResult AS ("
            sql &= " SELECT B.[ItemCode], SUM(B.[Quantity] + B.[DiscountQty]) AS qtaLavorazione "
            sql &= " FROM [carrelloHeader] AS A WITH (NOLOCK) "
            sql &= " INNER JOIN [carrelloLines] AS B WITH (NOLOCK) "
            sql &= " ON A.Code = B.OrderCode AND A.[CompanyName_] = B.[CompanyName_] "
            sql &= " WHERE A.[Status] <> 6 AND A.[CompletedImported] = 0 AND A.TIPO = 'ORDINE' "
            sql &= " AND (A.PRENOTAZIONE = 0 OR A.PRENOTAZIONE = 2) "
            sql &= " AND A.[CompanyName_] = @CompanyCode "
            If CodiceArticolo <> "" Then
                sql &= " AND B.[ItemCode] = @CodiceArticolo"
            End If
            sql &= " GROUP BY B.[ItemCode] "
            sql &= ") "
            sql &= "SELECT IR.[ItemCode], IR.qtaLavorazione + ISNULL(SUM(ResEnt.[Quantity (Base)]), 0) AS qtaLavorazione "
            sql &= "FROM InitialResult AS IR "
            sql &= "LEFT JOIN SerVisReservationEntryLocalCopy AS ResEnt "
            sql &= "ON IR.[ItemCode] = ResEnt.[Item No_] "
            sql &= "GROUP BY IR.[ItemCode], IR.qtaLavorazione"

            Dim cmd As New SqlCommand(sql, conn)
            If CodiceArticolo <> "" Then
                cmd.Parameters.Add("@CodiceArticolo", SqlDbType.VarChar, 20).Value = CodiceArticolo
            End If
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet()
            da.SelectCommand = cmd
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    ''' <summary>
    ''' Restituisce le qta del prodotto per lotto in lavorazione non ancora fatturate
    ''' </summary>
    ''' <param name="CodiceArticolo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getQtaInLavorazionePerLotto(Optional ByVal CodiceArticolo As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String
            sql = "WITH InitialResult AS ("
            sql &= " SELECT B.[ItemCode], B.[LotNo], SUM(B.[Quantity] + B.[DiscountQty]) AS qtaLavorazione "
            sql &= " FROM [carrelloHeader] AS A WITH (NOLOCK) "
            sql &= " INNER JOIN [carrelloLines] AS B WITH (NOLOCK) "
            sql &= " ON A.Code = B.OrderCode AND A.[CompanyName_] = B.[CompanyName_] "
            sql &= " WHERE A.[Status] <> 6 AND A.[CompletedImported] = 0 AND A.TIPO = 'ORDINE' "
            sql &= " AND (A.PRENOTAZIONE = 0 OR A.PRENOTAZIONE = 2) "
            sql &= " AND A.[CompanyName_] = @CompanyCode "
            If CodiceArticolo <> "" Then
                sql &= " AND B.[ItemCode] = @CodiceArticolo"
            End If
            sql &= " GROUP BY B.[ItemCode], B.[LotNo] "
            sql &= ") "
            sql &= "SELECT IR.[ItemCode], IR.[LotNo], IR.qtaLavorazione + ISNULL(SUM(ResEnt.[Quantity (Base)]), 0) AS qtaLavorazione "
            sql &= "FROM InitialResult AS IR "
            sql &= "LEFT JOIN SerVisReservationEntryLocalCopy AS ResEnt "
            sql &= "ON IR.[ItemCode] = ResEnt.[Item No_] "
            sql &= "GROUP BY IR.[ItemCode], IR.[LotNo], IR.qtaLavorazione"

            Dim cmd As New SqlCommand(sql, conn)
            If CodiceArticolo <> "" Then
                cmd.Parameters.Add("@CodiceArticolo", SqlDbType.VarChar, 20).Value = CodiceArticolo
            End If
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet()
            da.SelectCommand = cmd
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetItemImageSmall(ByVal CodiceArticolo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select isNULL(imgSmallProdotto,'') FROM [Items_Nav] WHERE [CodiceArticolo]=@CodiceArticolo AND CompanyCode=@CompanyCode", conn)
            cmd.Parameters.Add("@CodiceArticolo", SqlDbType.VarChar, 20).Value = CodiceArticolo
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim res As Object = cmd.ExecuteScalar
            If IsDBNull(res) Then
                Return ""
            Else
                Return res.ToString()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ""
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetItemDescription(ByVal CodiceArticolo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select isNULL(Descrizione,'') FROM [Items_Nav] WHERE [CodiceArticolo]=@CodiceArticolo AND CompanyCode=@CompanyCode", conn)
            cmd.Parameters.Add("@CodiceArticolo", SqlDbType.VarChar, 20).Value = CodiceArticolo
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim res As Object = cmd.ExecuteScalar
            If IsDBNull(res) Then
                Return ""
            Else
                Return res.ToString()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ""
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetItemDetails(ByVal CodiceArticolo As String) As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT * FROM [Items_Nav] AS A LEFT JOIN [t_organi_effetti] AS B ON A.CodiceArticolo=B.Codicearticolo WHERE A.[CodiceArticolo] = @CodiceArticolo AND A.CompanyCode=@CompanyCode", conn)
            cmd.Parameters.Add("@CodiceArticolo", SqlDbType.VarChar, 20).Value = CodiceArticolo
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
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

    Public Function isArcheopatico(ByVal CodiceArticolo As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            sql = "SELECT COUNT(*) FROM [Items_Nav] WHERE cod_macrolinea='0010' AND CodiceArticolo=@CodiceArticolo AND CompanyCode=@CompanyCode"
            cmd.Parameters.Add("@CodiceArticolo", SqlDbType.NVarChar, 20).Value = CodiceArticolo
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            cmd.CommandText = sql
            cmd.Connection = conn
            If cmd.ExecuteScalar > 0 Then
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

    Public Function GetDisponibilitaProdotti2(ByVal cl As List(Of carrelloLine)) As DataTable
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            'Dim groupedLines = cl.GroupBy(Function(l) New With {Key l.ordineLine.ItemCode}).Select(Function(group) New With {.ItemCode = group.Key.ItemCode})
            Dim groupItemCode = (From l In cl Group By itemCode = l.ordineLine.ItemCode Into lineItemGroup = Group Select New With {.itemCode = itemCode}).ToList()

            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String
            sql = "SELECT [Item No_], ISNULL(convert(bigint, SUM((CASE WHEN ([Expiration Date]<=GETDATE() AND [Expiration Date]<>'1900-01-01' AND [Expiration Date] <> '1753-01-01' AND [Product Group Code]<>'COSMETICI' AND [Product Group Code]<>'COSMETICO' AND [Product Group Code]<>'USOESTERNO') THEN 0 ELSE [Remaining Quantity] END) )) , 0) AS Disponibilita "
            sql &= " FROM [BCData].[dbo].[Ser-Vis$Item Ledger Entry] with(NOLOCK) "
            sql &= " WHERE ([Return Reason Code] <> 'R_ROTTO') AND ([Return Reason Code] <> 'R_SCADUTO') AND ([Return Reason Code] <> 'R_NONCONF') AND ([Reason Code] <> 'P_ROTTO') AND ([Reason Code] <> 'P_SCADUTO') AND ([Reason Code] <> 'P_NONCONF')"
            sql &= " AND [Location Code]='SADRIANO2'"
            If cl.Count > 0 Then
                sql &= " AND ("
            End If
            For Each item In groupItemCode
                If groupItemCode.IndexOf(item) > 0 Then sql &= " OR "
                sql &= " [item No_]='" & item.itemCode & "' "
            Next
            If groupItemCode.Count > 0 Then
                sql &= " ) "
            End If
            sql &= " GROUP BY [item No_]"

            Dim cmd As New SqlCommand(sql, conn)
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds)
            Dim dtresult As DataTable = ds.Tables(0)

            For Each item In groupItemCode
                Dim dtQtaImp As DataTable = getQtaInLavorazione(item.itemCode)
                For Each row As DataRow In dtQtaImp.Rows
                    For Each r As DataRow In dtresult.Rows
                        If row("ItemCode") = r("Item No_") Then
                            If row("qtaLavorazione") > 0 Then
                                r("Disponibilita") = r("Disponibilita") - row("qtaLavorazione")
                            End If
                            Exit For
                        End If
                    Next
                Next
            Next

            ' Ensure all negative values in the "Disponibilita" column are replaced with 0
            For Each row As DataRow In dtresult.Rows
                If Convert.ToInt64(row("Disponibilita")) < 0 Then
                    row("Disponibilita") = 0
                End If
            Next

            If dtresult.Rows.Count = 0 Then
                For Each item In groupItemCode
                    dtresult.Rows.Add(item.itemCode, 0)
                Next
            End If

            Return dtresult
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetDisponibilitaProdottiPerLotto2(ByVal cl As List(Of carrelloLine)) As DataTable
        'Dim groupItemCode = (From l In cl Group By itemCode = l.ordineLine.ItemCode, l.ordineLine.LotNo Into lineItemGroup = Group Select New With {.itemCode = itemCode, .LotNo = LotNo}).ToList()
        Dim groupItemCode = (From l In cl Group By itemCode = l.ordineLine.ItemCode Into lineItemGroup = Group Select New With {.itemCode = itemCode}).ToList()
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String
            sql = "SELECT [Item No_], [Lot No_] AS Lotto, [Expiration Date] AS ScadenzaLotto, ISNULL(CONVERT(bigint, SUM([Remaining Quantity])), 0) AS DisponibilitaLotto, "
            sql &= " CASE Len([Lot No_]) When 8 Then SUBSTRING([Lot No_] , 2, 2) "
            sql &= " 				 	 WHEN 7 THEN SUBSTRING([Lot No_] , 1, 2) END AS P, "
            sql &= " CASE LEN([Lot No_]) WHEN 8 THEN SUBSTRING([Lot No_] , 4, 2) "
            sql &= " 		             WHEN 7 THEN SUBSTRING([Lot No_] , 3, 2) END AS W, "
            sql &= " CASE LEN([Lot No_]) WHEN 8 THEN SUBSTRING([Lot No_] , 6, 1) "
            sql &= " 					 WHEN 7 THEN SUBSTRING([Lot No_] , 5, 1) END AS D, "
            sql &= " CASE LEN([Lot No_]) WHEN 8 THEN SUBSTRING([Lot No_] , 7, 2) "
            sql &= " 					 WHEN 7 THEN SUBSTRING([Lot No_] , 6, 2) END AS Y "
            sql &= " FROM [BCData].[dbo].[Ser-Vis$Item Ledger Entry] With(NOLOCK) "
            sql &= " WHERE [Open]=1 And [Remaining Quantity]>0 And (LEN([Lot No_])=8 Or LEN([Lot No_])=7) And ([Expiration Date]>=GETDATE() Or [Expiration Date]='1900-01-01' Or [Expiration Date]='1753-01-01' OR [Product Group Code]='COSMETICI' OR [Product Group Code]='COSMETICO' OR [Product Group Code]='USOESTERNO') "
            sql &= " AND ([Return Reason Code] <> 'R_ROTTO') AND ([Return Reason Code] <> 'R_SCADUTO') AND ([Return Reason Code] <> 'R_NONCONF') AND ([Reason Code] <> 'P_ROTTO') AND ([Reason Code] <> 'P_SCADUTO') AND ([Reason Code] <> 'P_NONCONF') "
            sql &= " AND [Location Code]='SADRIANO2'"
            If cl.Count > 0 Then
                sql &= " AND ("
            End If
            For Each item In groupItemCode
                If groupItemCode.IndexOf(item) > 0 Then sql &= " OR "
                sql &= " [item No_]='" & item.itemCode & "' "
            Next
            If groupItemCode.Count > 0 Then
                sql &= " ) "
            End If
            sql &= " GROUP BY [item No_], [Lot No_], [Expiration Date] "
            sql &= "ORDER BY [item No_], Y, W, D, P "

            Dim cmd As New SqlCommand(sql, conn)
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds)
            Dim dtresult As DataTable = ds.Tables(0)

            For Each item In groupItemCode
                Dim dtQtaImp As DataTable = getQtaInLavorazionePerLotto(item.itemCode)
                For Each row As DataRow In dtQtaImp.Rows
                    For Each r As DataRow In dtresult.Rows
                        If row("ItemCode") = r("Item No_") And row("LotNo") = r("Lotto") Then
                            If row("qtaLavorazione") > 0 Then
                                r("DisponibilitaLotto") = r("DisponibilitaLotto") - row("qtaLavorazione")
                            End If
                            Exit For
                        End If
                    Next
                Next
            Next

            Return dtresult
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getPrezzoRivenditore(codiceArticolo As String, codiceListno As String) As Decimal
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim Sql As String = "SELECT CONVERT(Decimal(18,2), PrezzoRivenditore) As PrzRivenditore FROM ListiniArticoli WHERE CodiceListino=@CodiceListino AND CodiceArticolo=@CodiceArticolo AND CompanyCode=@CompanyCode"
            Dim cmd As New SqlCommand(Sql, conn)
            cmd.Parameters.Add("@CodiceListino", SqlDbType.NVarChar, 20).Value = codiceListno
            cmd.Parameters.Add("@CodiceArticolo", SqlDbType.NVarChar, 20).Value = codiceArticolo
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function



#Region "Gestione apparati - organi - effetti"

    Public Function GetOrgEffLivello1_Name(ByVal codeLivello1 As Integer) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT [nome_Livello1] FROM [t_organi_effetti] WHERE id_Livello1=@idlivello1", conn)
            cmd.Parameters.Add("@idlivello1", SqlDbType.Int).Value = codeLivello1
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

#End Region




#Region "Gestione articoli mancanti non disponibili"

    Public Function addQuantitaNonDisponibile(ByVal qnd As quantitaNonDisponibile) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "INSERT INTO [QuantitaNonDisponibile] ("
            sql &= "  [CompanyName]"
            sql &= " ,[CustomerNo]"
            sql &= " ,[OrderCode]"
            sql &= " ,[idCarrello]"
            sql &= " ,[ItemCode]"
            sql &= " ,[Quantity]"
            sql &= " ,[DateAdd]"
            sql &= " ,[Alert]"
            sql &= ")"
            sql &= " VALUES ("
            sql &= "@CompanyName,"
            sql &= "@CustomerNo,"
            sql &= "@OrderCode,"
            sql &= "@idCarrello,"
            sql &= "@ItemCode,"
            sql &= "@Quantity,"
            sql &= "GETDATE(),"
            sql &= "@Alert"
            sql &= ");"
            sql &= " Select Scope_Identity()"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = qnd.customerNo
            cmd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = qnd.orderCode
            cmd.Parameters.Add("@idCarrello", SqlDbType.BigInt).Value = qnd.idCarrello
            cmd.Parameters.Add("@ItemCode", SqlDbType.VarChar, 20).Value = qnd.itemCode
            cmd.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = qnd.quantity
            cmd.Parameters.Add("@Alert", SqlDbType.TinyInt).Value = qnd.alert
            conn.Open()
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
            'MsgBox(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetArticoliMancanti(ByVal orderCode As String) As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "Select A.ItemCode, CONVERT(Int,A.Quantity) AS Quantity, B.Descrizione, B.Formato, B.Composizione, (CASE WHEN B.descrizione is null then 'Articolo non presente in anagrafica' END) AS Note FROM [QuantitaNonDisponibile] A LEFT JOIN [Items_Nav] B on A.ItemCode=b.CodiceArticolo WHERE A.[orderCode]=@orderCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
            'MsgBox(ex.Message)
            Return ds
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function


#End Region


#Region "Gestione ricerca treeview"

    Public Function getMacrolineeTreeSelectCommand() As String
        Dim sql As String
        sql = "Select DISTINCT(cod_macrolinea), Macrolinea FROM [Items_Nav] WHERE NonVisInOrdWeb=0 And bloccato=0 And cod_macrolinea<>'' And cod_macrolinea<>'0110' ORDER BY Macrolinea"
        Return sql
    End Function

    Public Function getLineeTreeSelectCommand() As String
        Dim sql As String
        sql = "SELECT DISTINCT(cod_linea), Linea FROM [Items_Nav] WHERE NonVisInOrdWeb=0 AND cod_linea<>'' AND bloccato=0 AND cod_linea<>'9998' AND cod_macrolinea=@cod_macrolinea ORDER BY Linea"
        Return sql
    End Function
    Public Function getFamiglieTreeSelectCommand() As String
        Dim sql As String
        sql = "SELECT DISTINCT(cod_Famiglia), Famiglia FROM [Items_Nav] WHERE NonVisInOrdWeb=0 AND cod_Famiglia<>'' AND bloccato=0 AND cod_famiglia<>'FAM_340' AND cod_linea=@cod_linea ORDER BY Famiglia"
        Return sql
    End Function

    Public Function getSottofamiglieTreeSelectCommand() As String
        Dim sql As String
        sql = "SELECT DISTINCT(cod_SottFam), SottoFamiglia FROM [Items_Nav] WHERE NonVisInOrdWeb=0 AND cod_SottFam<>'' AND bloccato=0 AND cod_SottFam<>'SOT_110' AND cod_famiglia=@cod_famiglia ORDER BY Sottofamiglia"
        Return sql
    End Function


#End Region





End Class
