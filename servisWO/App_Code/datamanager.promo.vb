Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function getPromoRowsByCtCodeItemCode(ByVal ctcode As String, ByVal itemcode As String, Optional ByVal itemcodelot As String = "", Optional ByVal dataOrdine As DateTime = Nothing, Optional ByVal cd As intCanaleDirezione = Nothing) As DataTable
        Dim conn As Data.SqlClient.SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String
            'ND Massimo: vado ad esplicitare i soli campi che servono così posso creare un indice SQL più mirato
            'sql = "SELECT * FROM [PROMO_ROWS] WITH(NOLOCK) WHERE CT_CODE=@ctcode AND ITEM_CODE=@itemcode AND Attivata=1"
            sql = "SELECT [ID], [PROMO_CODE], [QUANTITY_MIN], [QUANTITY_GIFT], [DISCOUNT_PERCENT], [InOut], [Attivata] FROM [PROMO_ROWS] WITH(NOLOCK) WHERE CT_CODE=@ctcode And ITEM_CODE=@itemcode And Attivata=1"
            If Not dataOrdine = Nothing Then
                sql &= " AND @dataOrdine BETWEEN PROMO_DATA_INIZIO AND PROMO_DATA_FINE"
            Else
                sql &= " AND GETDATE() BETWEEN PROMO_DATA_INIZIO AND PROMO_DATA_FINE"
            End If
            If itemcodelot <> "" Then
                sql &= " AND ITEM_CODE_LOT=@itemcodelot"
            End If
            If Not cd Is Nothing Then
                sql &= " AND (InOut = 2 OR InOut=@InOut)"
            Else
                sql &= " AND InOut = 2"
            End If
            sql &= " AND CompanyCode=@CompanyCode"

            ' VERSIONE ALTERNATIVA PER STORED PROCEDURE - NON IN USO
            'Dim sqlCmd As New SqlCommand()
            'sqlCmd.Connection = conn
            'sqlCmd.CommandType = CommandType.StoredProcedure
            'sqlCmd.CommandText = "getPromoRowsByCtCodeItemCode"
            'Dim da As New SqlDataAdapter(sqlCmd)

            Dim da As New SqlDataAdapter(Sql, conn)
            da.SelectCommand.Parameters.Add("@ctcode", SqlDbType.NVarChar, 50).Value = ctcode
            da.SelectCommand.Parameters.Add("@itemcode", SqlDbType.NVarChar, 50).Value = itemcode

            If Not dataOrdine = Nothing Then
                da.SelectCommand.Parameters.Add("@dataOrdine", SqlDbType.DateTime).Value = dataOrdine
            End If
            If itemcodelot <> "" Then
                da.SelectCommand.Parameters.Add("@itemcodelot", SqlDbType.NVarChar, 50).Value = itemcodelot
            End If
            If Not cd Is Nothing Then
                da.SelectCommand.Parameters.Add("@InOut", SqlDbType.Bit).Value = cd.IntDirezione
            End If
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

    Public Function getPromoHeaderByCtCode(ByVal ctcode As String, Optional ByVal dataOrdine As DateTime = Nothing, Optional ByVal cd As intCanaleDirezione = Nothing, Optional ByVal totaleOrdine As Double = -1, Optional ByVal idprofilo As Int64 = 0) As DataTable
        Dim conn As Data.SqlClient.SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String
            'ND Massimo: vado ad esplicitare i soli campi che servono. In realtà tutti tranne CompanyCode.
            'sql = "SELECT * FROM [PROMO_HEADER] WITH(NOLOCK) WHERE CT_CODE=@ctcode AND Attivata=1 "
            sql = "SELECT [ID],[PROMO_CODE],[CT_CODE],[TOTAL_ORDER_MIN],[TOTAL_ORDER_MAX],[DISCOUNT_PERCENT],[PROMO_DATA_INIZIO],[PROMO_DATA_FINE],[Attivata],[InOut],[idprofilo] " &
                  "FROM [PROMO_HEADER] WITH(NOLOCK) WHERE CT_CODE=@ctcode AND Attivata=1 "
            If Not dataOrdine = Nothing Then
                sql &= " AND @dataOrdine BETWEEN PROMO_DATA_INIZIO AND PROMO_DATA_FINE"
            Else
                sql &= " AND GETDATE() BETWEEN PROMO_DATA_INIZIO AND PROMO_DATA_FINE"
            End If
            If Not cd Is Nothing Then
                sql &= " AND (InOut = 2 OR InOut=@InOut)"
            Else
                sql &= " AND InOut = 2"
            End If
            If totaleOrdine >= 0 Then
                sql &= " AND @totaleOrdine BETWEEN [TOTAL_ORDER_MIN] AND [TOTAL_ORDER_MAX] "
            End If
            sql &= " AND idprofilo=@idprofilo "
            sql &= " AND CompanyCode=@CompanyCode "
            Dim da As New SqlDataAdapter(sql, conn)

            ' VERSIONE ALTERNATIVA PER STORED PROCEDURE - NON IN USO
            'Dim sqlCmd As New SqlCommand()
            'sqlCmd.Connection = conn
            'sqlCmd.CommandType = CommandType.StoredProcedure
            'sqlCmd.CommandText = "getPromoHeaderByCtCode"
            'Dim da As New SqlDataAdapter(sqlCmd)

            da.SelectCommand.Parameters.Add("@ctcode", SqlDbType.NVarChar, 50).Value = ctcode
            If totaleOrdine >= 0 Then
                da.SelectCommand.Parameters.Add("@totaleOrdine", SqlDbType.Decimal).Value = totaleOrdine
            End If
            If Not dataOrdine = Nothing Then
                da.SelectCommand.Parameters.Add("@dataOrdine", SqlDbType.DateTime).Value = dataOrdine
            End If
            If Not cd Is Nothing Then
                da.SelectCommand.Parameters.Add("@InOut", SqlDbType.Bit).Value = cd.IntDirezione
            End If
            da.SelectCommand.Parameters.Add("@idprofilo", SqlDbType.BigInt).Value = idprofilo
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

    Public Function getPromoDescription() As DataTable
        Dim conn As Data.SqlClient.SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            sql = "SELECT * FROM [PROMO_DESCRIPTION] WITH(NOLOCK) WHERE CompanyCode=@CompanyCode"
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
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

    Public Function getPromoFromCRM(ByVal CTCODE As String) As DataTable
        Dim conn As Data.SqlClient.SqlConnection = Nothing
        Try
            Dim sql As String = "SELECT * FROM [CRM_V2_F_ListPromoOrdWEB] (@CTCODE)"
            Dim da As New SqlDataAdapter(sql, New SqlConnection(WORconnectionString))
            da.SelectCommand.Parameters.Add("@CTCODE", SqlDbType.NVarChar, 20).Value = CTCODE
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function getProfiliSconti() As DataTable
        Dim conn As Data.SqlClient.SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim da As New SqlDataAdapter("SELECT * FROM [ProfiliSconti] WITH(NOLOCK) WHERE CompanyCode=@CompanyCode", conn)
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

    Public Function getProfiloScontoDescrizione(ByVal id As Int64) As String
        Dim conn As Data.SqlClient.SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT profilo FROM [ProfiliSconti] WITH(NOLOCK) WHERE id=@id AND CompanyCode=@CompanyCode", conn)
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim result As Object = cmd.ExecuteScalar()
            If Not result Is Nothing Then
                Return result.ToString()
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function existsPromoLotto(ByVal ctcode As String, ByVal itemcode As String, Optional ByVal dataOrdine As DateTime = Nothing, Optional ByVal cd As intCanaleDirezione = Nothing) As Boolean

        Dim conn As Data.SqlClient.SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String
            sql = "IF EXISTS (SELECT TOP(1) 1 FROM [PROMO_ROWS] WITH(NOLOCK) WHERE CT_CODE=@ctcode AND ITEM_CODE=@itemcode AND ITEM_CODE_LOT <>'' AND Attivata=1 "
            If Not dataOrdine = Nothing Then
                sql &= " AND @dataOrdine BETWEEN PROMO_DATA_INIZIO AND PROMO_DATA_FINE"
            Else
                sql &= " AND GETDATE() BETWEEN PROMO_DATA_INIZIO AND PROMO_DATA_FINE"
            End If
            If Not cd Is Nothing Then
                sql &= " AND (InOut = 2 OR InOut=@InOut)"
            Else
                sql &= " AND InOut = 2"
            End If
            sql &= " AND CompanyCode=@CompanyCode"
            sql &= ") SELECT 1 ELSE SELECT 0"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@ctcode", SqlDbType.NVarChar, 50).Value = ctcode
            da.SelectCommand.Parameters.Add("@itemcode", SqlDbType.NVarChar, 50).Value = itemcode
            If Not dataOrdine = Nothing Then
                da.SelectCommand.Parameters.Add("@dataOrdine", SqlDbType.DateTime).Value = dataOrdine
            End If
            If Not cd Is Nothing Then
                da.SelectCommand.Parameters.Add("@InOut", SqlDbType.Bit).Value = cd.IntDirezione
            End If
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet()
            da.Fill(ds)
            If (ds.Tables(0).Rows(0).Item(0) = 1) Then
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

End Class
