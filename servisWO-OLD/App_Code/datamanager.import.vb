Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function GetOrdersToImportList() As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT OrdineAmazon,DataIns,Importato,CodiceCliente FROM dbo.AmazonOrdini WHERE Importato=0 AND CompanyCode=@CompanyCode Order By DataIns "
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetOrderLinesToImport(ByVal CodiceOrdine As String) As DataTable
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT ID, OrdineAmazon, CompanyCode, CodiceArticolo, Qta FROM [AmazonArticoli] WHERE OrdineAmazon=@OrdineAmazon AND CompanyCode=@CompanyCode ORDER BY IdRiga"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@OrdineAmazon", SqlDbType.VarChar, 50).Value = CodiceOrdine
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function isImportabile(ByVal CodiceOrdine As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT Importato FROM [AmazonOrdini]  WHERE [OrdineAmazon]=@OrdineAmazon AND CompanyCode=@CompanyCode", conn)
            cmd.Parameters.Add("@OrdineAmazon", SqlDbType.VarChar, 50).Value = CodiceOrdine
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            If cmd.ExecuteScalar() = 0 Then
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Importato">
    ''' 0=importabile, 1=in importazione, 2=importato
    ''' </param>
    Public Sub SetOrderImportStatus(ByVal CodiceOrdine As String, ByVal Importato As Integer)
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("UPDATE [AmazonOrdini] SET Importato=@Importato WHERE [OrdineAmazon]=@OrdineAmazon AND CompanyCode=@CompanyCode", conn)
            cmd.Parameters.Add("@Importato", SqlDbType.TinyInt).Value = Importato
            cmd.Parameters.Add("@OrdineAmazon", SqlDbType.VarChar, 50).Value = CodiceOrdine
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub



End Class
