Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function getListinoByCodiceCliente(ByVal CodiceCliente As String) As String
        Dim conn As SqlConnection = Nothing
        Dim result As String = ""
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT A.CodiceListino FROM Listini A INNER JOIN ListiniClienti B ON (A.CodiceListino=B.CodiceListino AND A.CompanyCode=B.CompanyCode) "
            sql &= " WHERE B.CodiceCliente=@CodiceCliente AND A.CompanyCode=@CompanyCode"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CodiceCliente", SqlDbType.VarChar, 30).Value = CodiceCliente
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet()
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0).Item("CodiceListino")
            End If
            Return result
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getListinoBase() As String
        Dim conn As SqlConnection = Nothing
        Dim result As String = ""
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT CodiceListino FROM Listini WHERE ListinoBase=1 AND CompanyCode=@CompanyCode"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet()
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0).Item("CodiceListino")
            End If
            Return result
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

End Class
