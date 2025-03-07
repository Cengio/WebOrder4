Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function getOrdiniDaProdurre() As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT ch.Code, ch.CODICECLIENTE, ch.UTENTE_ULTIMA_MODIFICA FROM carrelloHeader ch inner join carrelloLines cl ON ch.IDCART=cl.IDCART "
            sql &= " WHERE ch.TIPO='ORDINE' AND ch.Status=1 AND ch.PRENOTAZIONE=0 AND ch.STATUSPRODUZIONE=0 AND ch.BLOCKED=0 AND cl.LotNo='' AND ch.[CompanyName_]=@CompanyCode GROUP BY ch.Code, ch.CODICECLIENTE, ch.UTENTE_ULTIMA_MODIFICA  ORDER BY ch.Code"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getArticoliDaProdurreByOrderCode(Code As String) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT cl.ItemCode, cl.DESCRIZIONE, cl.LotNo, CAST(cl.OriginalQty AS INT) AS OriginalQty FROM carrelloHeader ch INNER JOIN carrelloLines cl ON ch.IDCART=cl.IDCART "
            sql &= " WHERE ch.TIPO='ORDINE' AND ch.Status=1 AND ch.BLOCKED=0 AND cl.LotNo='' AND ch.Code=@Code AND ch.[CompanyName_]=@CompanyCode "
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = Code
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function confermaProduzione(ByVal orderCode As String, ByVal operatorCode As String) As Boolean
        Dim connNAV As New SqlConnection(NAVconnectionString)
        Dim connWO As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [OrderHeaderFromWeb_Write] SET [Status]=@Status WHERE [Code]=@Code"
            Dim cmd As New SqlCommand(sql, connNAV)
            cmd.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 0
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connNAV.Open()
            cmd.ExecuteNonQuery()

            Dim sql2 As String = "UPDATE [carrelloHeader] SET [STATUSPRODUZIONE]=1 ,[Status]=@Status, UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA, DATA_ULTIMA_MODIFICA=GETDATE() WHERE [CompanyName_]=@CompanyName AND [Code]=@Code"
            'Dim sql2 As String = "UPDATE [carrelloHeader] SET [STATUSPRODUZIONE]=1 ,[Status]=@Status WHERE [CompanyName_]=@CompanyName AND [Code]=@Code"
            Dim cmd2 As New SqlCommand(sql2, connWO)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 0
            cmd2.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWO.Open()
            cmd2.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If connWO.State = ConnectionState.Open Then connWO.Close()
            If connNAV.State = ConnectionState.Open Then connNAV.Close()
        End Try
    End Function

    Public Function getStatusProduzione(orderCode As String) As Integer
        Dim connWO As New SqlConnection(WORconnectionString)
        Try
            Dim STATUSPRODUZIONE As Object
            Dim sql2 As String = "SELECT [STATUSPRODUZIONE] FROM [carrelloHeader] WHERE [CompanyName_]=@CompanyName AND [Code]=@Code"
            Dim cmd2 As New SqlCommand(sql2, connWO)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWO.Open()
            STATUSPRODUZIONE = cmd2.ExecuteScalar()
            If Not STATUSPRODUZIONE Is Nothing Then
                Return CInt(STATUSPRODUZIONE)
            Else
                Return 0
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If connWO.State = ConnectionState.Open Then connWO.Close()
        End Try
    End Function


    Public Function getOrdiniPrenotazioneDaProdurre() As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT ch.Code, ch.CODICECLIENTE, ch.UTENTE_ULTIMA_MODIFICA, ch.DATA_EVASIONE FROM carrelloHeader ch inner join carrelloLines cl ON ch.IDCART=cl.IDCART "
            sql &= " WHERE ch.TIPO='ORDINE' AND ch.Status=2 AND ch.PRENOTAZIONE=1 AND ch.STATUSPRODUZIONE=0 AND ch.BLOCKED=0 AND cl.LotNo='' AND ch.[CompanyName_]=@CompanyCode GROUP BY ch.Code, ch.CODICECLIENTE, ch.UTENTE_ULTIMA_MODIFICA,ch.DATA_EVASIONE  ORDER BY ch.DATA_EVASIONE, ch.Code"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getArticoliPrenotazioneDaProdurreByOrderCode(Code As String) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String
            sql = "SELECT cl.ItemCode, cl.DESCRIZIONE, cl.LotNo, CAST(cl.OriginalQty AS INT) AS OriginalQty FROM carrelloHeader ch INNER JOIN carrelloLines cl ON ch.IDCART=cl.IDCART "
            sql &= " WHERE ch.TIPO='ORDINE' AND ch.Status=2 AND ch.BLOCKED=0 AND cl.LotNo='' AND ch.Code=@Code AND ch.[CompanyName_]=@CompanyCode "
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = Code
            da.SelectCommand.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim ds As New DataSet
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function confermaProduzionePrenotati(ByVal orderCode As String, ByVal operatorCode As String) As Boolean
        Dim connNAV As New SqlConnection(NAVconnectionString)
        Dim connWO As New SqlConnection(WORconnectionString)
        Try
            'Dim sql2 As String = "UPDATE [carrelloHeader] SET [Status]=@Status, [PRENOTAZIONE]=@PRENOTAZIONE, UTENTE_ULTIMA_MODIFICA=@UTENTE_ULTIMA_MODIFICA, DATA_ULTIMA_MODIFICA=GETDATE() WHERE [CompanyName_]=@CompanyName AND [Code]=@Code"
            Dim sql2 As String = "UPDATE [carrelloHeader] SET [Status]=@Status, [PRENOTAZIONE]=@PRENOTAZIONE WHERE [CompanyName_]=@CompanyName AND [Code]=@Code"
            Dim cmd2 As New SqlCommand(sql2, connWO)
            cmd2.Parameters.Clear()
            cmd2.Parameters.Add("@Status", SqlDbType.TinyInt).Value = 0
            cmd2.Parameters.Add("@PRENOTAZIONE", SqlDbType.TinyInt).Value = 2
            'cmd2.Parameters.Add("@UTENTE_ULTIMA_MODIFICA", SqlDbType.BigInt).Value = CInt(operatorCode)
            cmd2.Parameters.Add("@CompanyName", SqlDbType.VarChar, 30).Value = GetWorkingCompany
            cmd2.Parameters.Add("@Code", SqlDbType.VarChar, 20).Value = orderCode
            connWO.Open()
            cmd2.ExecuteNonQuery()

            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return False
        Finally
            If connWO.State = ConnectionState.Open Then connWO.Close()
            If connNAV.State = ConnectionState.Open Then connNAV.Close()
        End Try
    End Function


End Class
