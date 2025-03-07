Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Enum parametriSitoValue
        emailNotificaNuovoCliente
        emailNotificaOrdini
        fascia
        magazzinodefault
        percorsoPDF
        emailNotificaNuovoCliente2
        emailNotificaOrdini2
        emailNotificaNuovoCliente3
        emailNotificaOrdini3
        importoSpeseSpedizione
        importoPortoFranco
        importoEvadibilitaOrdine
        emailSistema
        emailSistemaDesc
        attivaNotificheEmail
        attivaNotificaOrdineAlCliente
        attivaNotificaNuovoOrdine
        attivaNotificaNuovoCliente
        attivaNotificaRevisioneOrdine
        emailNotificaRevisioneOrdini
        emailNotificaRevisioneOrdini2
        emailNotificaRevisioneOrdini3
        emailRichiestaProduzione
        emailRichiestaProduzione2
        emailRichiestaProduzione3
    End Enum

    ''' <summary>
    ''' Restituisce i parametri sito configuati nel database
    ''' </summary>
    ''' <param name="nomeparametro">fascia/magazzinodefault/emailNotificaOrdini/emailNotificaNuovoCliente</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetParametroSito(ByVal nomeparametro As parametriSitoValue) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT [Valore] FROM [PARAMETRI_SITO] WHERE [NomeParametro]=@nomeparametro AND CompanyCode=@CompanyCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("nomeparametro", SqlDbType.VarChar, 50).Value = nomeparametro.ToString
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function updParametroSito(ByVal nomeparametro As parametriSitoValue, ByVal valore As String) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [PARAMETRI_SITO] SET [Valore]=@valore WHERE [NomeParametro]=@NomeParametro AND CompanyCode=@CompanyCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@valore", SqlDbType.VarChar, 50).Value = valore
            cmd.Parameters.Add("@NomeParametro", SqlDbType.VarChar, 50).Value = nomeparametro.ToString
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
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

    Public Function GetNoWebSeries() As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "SELECT * FROM NoWebSeries WHERE CompanyCode='ALL' OR CompanyCode=@CompanyCode"
            Dim da As New SqlDataAdapter(sql, conn)
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

    Public Function updNoWebSeries(ByVal nomeparametro As String, ByVal valore As String) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE NoWebSeries SET Progressive=@Progressive WHERE [NO_ Type]=@notype AND CompanyCode=@CompanyCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@Progressive", SqlDbType.BigInt).Value = valore
            cmd.Parameters.Add("@notype", SqlDbType.VarChar, 50).Value = nomeparametro
            If nomeparametro = "nuovocliente" Then
                cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = "ALL"
            Else
                cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            End If
            conn.Open()
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetSASstatus() As Integer
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT TOP 1 [STATO_SAS] FROM [sasStatus] WHERE CompanyCode=@CompanyCode ORDER BY DATA_CAMBIO_STATO DESC"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function changeSASstatus(ByVal active As Integer) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "INSERT INTO [sasStatus](STATO_SAS,DATA_CAMBIO_STATO,CompanyCode) VALUES (@STATO_SAS,GETDATE(),@CompanyCode)"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@STATO_SAS", SqlDbType.TinyInt).Value = active
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            conn.Open()
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Sub synchMailingGroupParamatriSitoFromNAV()
        Try
            Dim MGFromNav As DataTable = GetMailingGroupListFromNAV()
            For Each rNav As DataRow In MGFromNav.Rows
                If Not esiteMailingGroupParamatriSito(rNav("Code")) Then
                    addMailingGrouparamatriSito(rNav("Code"), rNav("Description"))
                End If
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
        End Try
    End Sub

    Public Function GetMailingGroupListFromNAV() As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim sql As String = "SELECT [Code],[Description] FROM [Mailing Group] ORDER BY [Description]"
            Dim da As New SqlDataAdapter(sql, conn)
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getMailingGroupCodeByContact(ByVal ContactNo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim sql As String = " Select [Mailing Group Code] FROM [NAV Contact Mailing Group] WHERE [Contact No_]=@ContactNo"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@ContactNo", SqlDbType.VarChar, 20).Value = ContactNo
            Dim MailingGroupCode As String = cmd.ExecuteScalar
            If Not MailingGroupCode Is Nothing AndAlso MailingGroupCode <> "" Then
                Return MailingGroupCode
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function esiteMailingGroupParamatriSito(ByVal Code As String) As Boolean
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select COUNT(*) FROM [parametriEvadibilitaOrdine] WHERE Code=@Code"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = Code
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

    Public Function addMailingGrouparamatriSito(ByVal Code As String, ByVal Description As String) As Integer
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "INSERT INTO [parametriEvadibilitaOrdine] ([Code],[Description],[importoEvadibilitaOrdine],[importoPortoFranco],[importoSpeseSpedizione]) VALUES(@Code,@Description,@importoevadibilitàordine,@importoportofranco,@importospesespedizione)"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = Code
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, 30).Value = Description
            cmd.Parameters.Add("@importoevadibilitàordine", SqlDbType.Decimal).Value = GetParametroSito(parametriSitoValue.importoEvadibilitaOrdine)
            cmd.Parameters.Add("@importoportofranco", SqlDbType.Decimal).Value = GetParametroSito(parametriSitoValue.importoPortoFranco)
            cmd.Parameters.Add("@importospesespedizione", SqlDbType.Decimal).Value = GetParametroSito(parametriSitoValue.importoSpeseSpedizione)
            Return cmd.ExecuteNonQuery
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getparametriEvadibilitaOrdineByContactCode(ByVal contactNo As String, ByVal parametro As parametriSitoValue) As Double
        Dim conn As New SqlConnection(WORconnectionString)
        Dim importo As Double = 0
        Dim sql As String = ""
        Dim gruppoMailing As String = ""

        Try
            conn.Open()
            Dim cmd As New SqlCommand

            gruppoMailing = getMailingGroupCodeByContact(contactNo)
            If gruppoMailing <> "" AndAlso esiteMailingGroupParamatriSito(gruppoMailing) Then
                sql = "SELECT [" & parametro.ToString & "] FROM [parametriEvadibilitaOrdine] WHERE [Code]=@Code"
                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = gruppoMailing
                cmd.CommandText = sql
                cmd.Connection = conn
                Dim res As Object = cmd.ExecuteScalar
                If Not res Is Nothing Then
                    importo = res
                End If
            Else
                importo = GetParametroSito(parametro)
            End If
            Return importo

        Catch ex As Exception
            ' Log or save error details somewhere you can inspect them.
            LogError("Error in getparametriEvadibilitaOrdineByContactCode", ex, sql, gruppoMailing, contactNo, parametro)
            Throw
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Sub LogError(ByVal location As String, ByVal ex As Exception, ByVal sql As String, ByVal gruppoMailing As String, ByVal contactNo As String, ByVal parametro As parametriSitoValue)
        ' Implement your logging mechanism here.
        ' Save error details to a log file, database, or another storage solution.
        ' Example:
        Dim errorMessage As String = "Location: " & location & vbNewLine &
                             "Error Message: " & ex.Message & vbNewLine &
                             "Stack Trace: " & ex.StackTrace & vbNewLine &
                             "SQL: " & sql & vbNewLine &
                             "Gruppo Mailing: " & gruppoMailing & vbNewLine &
                             "Contact No: " & contactNo & vbNewLine &
                             "Parametro: " & parametro.ToString

        ' Save `errorMessage` somewhere you can access it. For simplicity, we'll print it.
        Console.WriteLine(errorMessage)
    End Sub

    Public Function getLocation() As DataTable
        Dim conn As New SqlConnection(NAVconnectionString)
        Try
            Dim sql As String = "SELECT [Code], [Code]+' - ' +[Name]+' ' +[Name 2] AS Descrizione, [Address]+' '+[Address 2] AS Indirizzo, [City], [Post Code], [E-Mail] FROM [NAV_Location] ORDER BY [Name] "
            conn.Open()
            Dim cmd As New SqlCommand(sql, conn)
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

End Class
