Imports System.Data
Imports System.Data.SqlClient

Partial Public Class datamanager

    Public Function GetCustomers(Optional ByVal customerCode As String = "") As DataSet
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("woGetCustomers", conn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = If(customerCode <> "", customerCode, DBNull.Value)

            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception("Errore GetCustomers: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function


    Public Function GetCustomerName(ByVal customerCode As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("woGetCustomerName", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = customerCode

            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                Return result.ToString()
            Else
                Return "" ' Or handle the case where no customer is found
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    Public Function GetCustomerEmail(ByVal customerCode As String) As List(Of String)
        Dim conn As SqlConnection = Nothing
        Dim listaEmail As New List(Of String)
        Try
            conn = New SqlConnection(NAVconnectionString)
            Dim sql As String = "Select [E-Mail] as email1, [E-Mail 2] as email2 FROM [NAV Customer] WHERE [No_]=@customerCode"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = customerCode
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("email1") <> "" Then
                    listaEmail.Add(dt.Rows(0).Item("email1"))
                End If
                'If dt.Rows(0).Item("email2") <> "" Then
                '    listaEmail.Add(dt.Rows(0).Item("email2"))
                'End If
            Else
                Dim ctCode As String = GetContactCodeByCustomerCode(customerCode)
                If ctCode <> "" Then
                    Dim listaEmailContatto As List(Of String) = GetContactEmail(ctCode)
                    For Each email As String In listaEmailContatto
                        listaEmail.Add(email)
                    Next
                End If
            End If
            Return listaEmail
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return listaEmail
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getCategoriaClienteDescrizione(ByVal customerNo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("woGetCategoriaClienteDescrizione", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@customerNo", SqlDbType.VarChar, 20).Value = customerNo

            Dim res As Object = cmd.ExecuteScalar()
            If Not res Is Nothing Then
                Return res.ToString()
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    Public Function GetCustomerBlockedReason(Optional ByVal Code As String = "", Optional ByVal tipoReason As Integer = 0) As DataSet
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            Dim sqlW As String = " WHERE "
            Dim sqlA As String = " AND "
            sql = " Select [Description] FROM [Reason Blocked] WHERE [Type]=@tipoReason "
            sqlW = ""
            If Code <> "" Then
                If sqlW <> "" Then
                    sql &= sqlW
                    sqlW = ""
                Else
                    sql &= sqlA
                End If
                sql &= " [Description]=@Code"
            End If
            If Code <> "" Then
                cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = Code
            End If
            cmd.Parameters.Add("@tipoReason", SqlDbType.Int).Value = tipoReason
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception("Errore GetCustomerBlockedReason: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetShipToAddress(Optional ByVal customerCode As String = "", Optional ByVal shiptoCode As String = "") As DataSet
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("woGetShipToAddress", conn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = If(customerCode <> "", customerCode, DBNull.Value)
            cmd.Parameters.Add("@shiptoCode", SqlDbType.VarChar, 10).Value = If(shiptoCode <> "", shiptoCode, DBNull.Value)

            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception("Errore GetShipToAddress: " & ex.Message)
        Finally
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    Public Function GetShipToAddressByCustomerCode(ByVal customerCode As String) As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim dtDest As New DataTable
            Dim sql As String = "SELECT [Code],([Name] + ' ' + [Name 2]) AS [Name],([Address] + ' ' + [Address 2]) AS [Address],[City],[Post Code],[County] FROM [Ser-Vis$Ship-to Address] where [Customer No_]=@customerCode ORDER BY [Name]"
            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = customerCode
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            dtDest = ds.Tables(0)
            'aggiungo destinatario stesso della fatturazione
            Dim dt As DataTable = GetCustomers(customerCode).Tables(0)
            If dt.Rows.Count > 0 Then
                Dim newRow As DataRow = dtDest.NewRow
                newRow("Code") = "FATTURAZIONE"
                newRow("Name") = "INDIRIZZO FATTURAZIONE"
                newRow("Address") = dt.Rows(0).Item("Address").ToString & " " & dt.Rows(0).Item("Address 2").ToString
                newRow("City") = dt.Rows(0).Item("City")
                newRow("Post Code") = dt.Rows(0).Item("Post Code")
                newRow("County") = dt.Rows(0).Item("County")
                'dtDest.Rows.Add(newRow)
                dtDest.Rows.InsertAt(newRow, 0)
            End If
            Return dtDest
        Catch ex As Exception
            Throw New Exception("Errore GetShipToAddressByCustomerCode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetShippingAgentForCustomer(ByVal customerNo As String) As Hashtable
        Dim conn As SqlConnection = Nothing
        Try
            Dim sac As New Hashtable
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = " Select [Shipping Agent Code],[Shipping Agent Code II],[Shipping Agent Code III] FROM [NAV Customer] WHERE [No_]=@customerNo"
            cmd.Parameters.Add("@customerNo", SqlDbType.VarChar, 20).Value = customerNo
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If ds.Tables(0).Rows(0).Item("Shipping Agent Code") <> "" AndAlso Not sac.Contains(ds.Tables(0).Rows(0).Item("Shipping Agent Code")) Then
                    sac.Add(ds.Tables(0).Rows(0).Item("Shipping Agent Code"), GetShippingAgentName(ds.Tables(0).Rows(0).Item("Shipping Agent Code")))
                End If
                If ds.Tables(0).Rows(0).Item("Shipping Agent Code II") <> "" AndAlso Not sac.Contains(ds.Tables(0).Rows(0).Item("Shipping Agent Code II")) Then
                    sac.Add(ds.Tables(0).Rows(0).Item("Shipping Agent Code II"), GetShippingAgentName(ds.Tables(0).Rows(0).Item("Shipping Agent Code II")))
                End If
                If ds.Tables(0).Rows(0).Item("Shipping Agent Code III") <> "" AndAlso Not sac.Contains(ds.Tables(0).Rows(0).Item("Shipping Agent Code III")) Then
                    sac.Add(ds.Tables(0).Rows(0).Item("Shipping Agent Code III"), GetShippingAgentName(ds.Tables(0).Rows(0).Item("Shipping Agent Code III")))
                End If
            End If

            Return sac
        Catch ex As Exception
            Throw New Exception("Errore GetShippingAgentForCustomer: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetShippingAgent(Optional ByVal soloWebOrder As Boolean = False) As Hashtable
        Dim conn As SqlConnection = Nothing
        Try
            Dim sac As New Hashtable
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [NAV_Shipping Agent] "
            If soloWebOrder Then '-------------------------------------------questo campo non è stato ancora creato in NAV !!! 
                sql &= " WHERE [Web Order]=@weborder"
            End If
            Dim da As New SqlDataAdapter(sql, conn)
            If soloWebOrder Then
                da.SelectCommand.Parameters.Add("@weborder", SqlDbType.TinyInt).Value = soloWebOrder
            End If
            Dim ds As New DataSet()
            da.Fill(ds)
            For Each row As DataRow In ds.Tables(0).Rows
                sac.Add(row("Code"), row("Name"))
            Next
            Return sac
        Catch ex As Exception
            Throw New Exception("Errore GetShippingAgent: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetShippingAgentName(ByVal code As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim sql As String = " Select Name FROM [NAV_Shipping Agent] WHERE [Code]=@Code"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = code
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception("Errore GetShippingAgent: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetPaymentMethod(Optional ByVal code As String = "") As DataSet
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT Code,Description FROM [NAV_PaymentMethod] "
            If code <> "" Then
                sql &= " WHERE code=@code"
                cmd.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = code
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception("Errore GetPaymentMethodCode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetPagamentiWO(ByVal codeCustomer As String, ByVal Metodo As String, ByVal Termine As String, Optional ByVal totaleOrdine As Double = 0) As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            sql = "  SELECT ROW_NUMBER() OVER ( ORDER BY Predefinito DESC) Numeratore , * FROM ( "
            sql &= " SELECT [PaymentMethodCode] As Metodo, [PaymentTermsCode] AS Termine, [DiscountPerc] AS Sconto, [Description] As Descrizione, '0' As Predefinito "
            sql &= " FROM [parametriScontoPagamenti] WHERE [Disabled]=0 AND [CustomerNo]=@codeCustomer) AS PagamentiPossibili"
            'cmd.Parameters.Add("@metodo", SqlDbType.VarChar).Value = Metodo
            'cmd.Parameters.Add("@termine", SqlDbType.VarChar).Value = Termine
            cmd.Parameters.Add("@codeCustomer", SqlDbType.VarChar).Value = codeCustomer
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)

            Dim dtpagamenti As DataTable = ds.Tables(0)
            If totaleOrdine >= 500 AndAlso dtpagamenti.Select("Metodo='RB'").Count > 0 AndAlso dtpagamenti.Select("Termine='60FM'").Count = 0 Then
                Dim numMax As Integer = dtpagamenti.Select("Numeratore=MAX(Numeratore)")(0).Item(0) + 1
                Dim newR As DataRow = dtpagamenti.NewRow
                newR("Numeratore") = numMax
                newR("Metodo") = "RB"
                newR("Termine") = "60FM"
                newR("Descrizione") = "Ricevuta Bancaria 60 gg fm"
                newR("Sconto") = 0
                newR("Predefinito") = 0
                dtpagamenti.Rows.Add(newR)
            End If
            Return dtpagamenti

        Catch ex As Exception
            Throw New Exception("Errore GetPagamentiWO: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetPaymentTerms(Optional ByVal code As String = "") As DataSet
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT Code,Description FROM [NAV_PaymentTerms] "
            If code <> "" Then
                sql &= " WHERE code=@code"
                cmd.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = code
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception("Errore GetPaymentTermsCode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getCustomerDiscountByPayments(ByVal customerCode As String, ByVal PaymentMethodCode As String, ByVal PaymentTermsCode As String) As Double
        Dim conn As SqlConnection = Nothing
        Try
            'Dim DiscountPerc As Double = 0
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT DiscountPerc FROM CustomerDiscountByPayments "
            sql &= " WHERE [code]=@customerCode AND [PaymentTermsCode]=@PaymentTermsCode AND [PaymentMethodCode]=@PaymentMethodCode "
            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 10).Value = customerCode
            cmd.Parameters.Add("@PaymentTermsCode", SqlDbType.VarChar, 10).Value = PaymentTermsCode
            cmd.Parameters.Add("@PaymentMethodCode", SqlDbType.VarChar, 10).Value = PaymentMethodCode
            cmd.CommandText = sql
            cmd.Connection = conn
            Return cmd.ExecuteScalar
            'If DiscountPerc > 0 Then
            '    DiscountPerc()
            'Else
            '    Return 0
            'End If
        Catch ex As Exception
            Throw New Exception("Errore getCustomerDiscountByPayments: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetSpeseTrasporto() As Double
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT B.[Amount Excl_ VAT] AS speTrasporto FROM [" & workingCompany & "$Sales_Receivable Setup] AS A INNER JOIN [" & workingCompany & "$Standard Sales Line] AS B ON A.[Std Shipment Sales Code]=B.[Standard Sales Code]", conn)
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
            'MsgBox("Errore GetSpeseTrasporto: " & ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetSpeseIncasso(ByVal CustomerNo As String, ByVal PaymentMethodCode As String) As Double
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            'L’informazione è legata al cliente. 
            'Quindi dato un cod. cliente, devi accedere alla tabella “Standard Customer Sales Code” 
            'e prendere tutte le righe che hanno “Payment method Code” = al “Payment Method Code” sulla testata dell’ordine (OrderHeaderWeb). 
            'Dopodiché selezionati quei record (in Servis ne hanno giusto 1 che sono le spese di incasso, ma potresti concettualmente avere altri tipi di spese automatiche), 
            'vai sulla tabella di ieri “Standard Sales Line” dove “Standard Sales Code” = al Code di “Standard Customer Sales Code”.
            Dim sql As String = "SELECT A.[Amount Excl_ VAT] AS speIncasso FROM [" & workingCompany & "$Standard Sales Line] AS A "
            sql &= " INNER JOIN [" & workingCompany & "$Standard Customer Sales Code] AS B ON A.[Standard Sales Code]=B.[Code]"
            sql &= " WHERE B.[Customer No_]=@CustomerNo AND B.[Payment Method Code]=@PaymentMethodCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            cmd.Parameters.Add("@PaymentMethodCode", SqlDbType.VarChar, 10).Value = PaymentMethodCode
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
            'MsgBox("Errore GetSpeseTrasporto: " & ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetContacts(Optional ByVal contactCode As String = "") As DataSet
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            Dim sqlW As String = " WHERE "
            Dim sqlA As String = " AND "

            sql = " Select * FROM [NAV_Contact] "
            If contactCode <> "" Then
                If sqlW <> "" Then
                    sql &= sqlW
                    sqlW = ""
                Else
                    sql &= sqlA
                End If
                sql &= " [No_]=@contactCode"
            End If
            sql &= " ORDER BY [Name]"

            If contactCode <> "" Then
                cmd.Parameters.Add("@contactCode", SqlDbType.VarChar, 20).Value = contactCode
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetCustomersByConctactCode(ByVal PrimaryContactNo As String) As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select * FROM [NAV Customer] WHERE [Primary Contact No_]=@PrimaryContactNo", conn)
            cmd.Parameters.Add("@PrimaryContactNo", SqlDbType.VarChar, 20).Value = PrimaryContactNo
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

    Public Function GetContactCodeByCustomerCode(ByVal customerCode As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select [Primary Contact No_] FROM [NAV Customer] WHERE [No_]=@customerCode", conn)
            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = customerCode
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetContactEmail(ByVal contactCode As String) As List(Of String)
        Dim conn As SqlConnection = Nothing
        Dim listaEmail As New List(Of String)
        Try
            conn = New SqlConnection(NAVconnectionString)
            Dim sql As String = "Select [E-Mail] as email1, [E-Mail 2] as email2 FROM [NAV_Contact] WHERE [No_]=@contactCode"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@contactCode", SqlDbType.VarChar, 20).Value = contactCode
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("email1") <> "" Then
                    listaEmail.Add(dt.Rows(0).Item("email1"))
                End If
                'If dt.Rows(0).Item("email2") <> "" Then
                '    listaEmail.Add(dt.Rows(0).Item("email2"))
                'End If
            End If
            Return listaEmail
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return listaEmail
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    '20200424 - Gestione scontoScaglioni

    Public Function GetScontiScaglioni(ByVal CodCliente As String) As DataTable
        Dim conn As SqlConnection = New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "SELECT CONCAT('Scaglione ',Scaglione,CASE Attivo WHEN 1 THEN ' - Attivo' END) as NomeScaglione,* FROM parametriScontoScaglioni WHERE GETDATE() BETWEEN DataInizio AND Datafine AND CodCliente=@CodCliente ORDER BY Scaglione DESC"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CodCliente", SqlDbType.VarChar).Value = CodCliente
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception("Errore GetScontiScaglioni: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetScontoScaglioneByCodCliente(ByVal CodCliente As String) As String
        Dim conn As SqlConnection = New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "SELECT Sconto FROM parametriScontoScaglioni WHERE GETDATE() BETWEEN DataInizio AND Datafine AND CodCliente=@CodCliente AND Attivo=1"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CodCliente", SqlDbType.VarChar).Value = CodCliente
            conn.Open()
            Dim res As Object = cmd.ExecuteScalar()
            If Not res Is Nothing Then
                Return res.ToString
            Else
                Return "0"
            End If
        Catch ex As Exception
            Throw New Exception("Errore GetScontiScaglioni: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function



#Region "Gestione nuovo cliente N"

    Public Function GetNewCustomerProgressive() As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = "SELECT Progressive FROM NoWebSeries WHERE CompanyCode ='ALL' AND [NO_ Type]='nuovocliente'"
            Dim cmd As New SqlCommand(sql, conn)
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Read()
            Dim newprog As Int64 = dr("Progressive") + 1
            dr.Close()
            cmd.CommandText = "UPDATE NoWebSeries SET Progressive= Progressive + 1 WHERE CompanyCode ='ALL' AND [NO_ Type]='nuovocliente'"
            cmd.Connection = conn
            cmd.ExecuteNonQuery()
            Return "N" & newprog.ToString
        Catch ex As Exception
            Throw New Exception("Numero progressivo NUOVO CLIENTE non impostato correttamente." & " " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetNewCustomersFromNAV(Optional ByVal CustomerNo As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = " Select * FROM [CustomersFromOutside] "
            If CustomerNo <> "" Then
                sql &= " WHERE [CustomerNo]=@CustomerNo"
                cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            End If
            sql &= " ORDER BY [Name]"
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception("Errore GetNewCustomers: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetNewCustomers(Optional ByVal CustomerNo As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            Dim sqlW As String = " WHERE "
            Dim sqlA As String = " AND "

            sql = " Select * FROM [newCustomer] "
            If CustomerNo <> "" Then
                If sqlW <> "" Then
                    sql &= sqlW
                    sqlW = ""
                Else
                    sql &= sqlA
                End If
                sql &= " [CustomerNo]=@CustomerNo"
            End If
            sql &= " ORDER BY [Name]"

            If CustomerNo <> "" Then
                cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception("Errore GetNewCustomers: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Function

    Public Function getNewCustomerEntity(ByVal CustomerNo As String) As newcustomer
        Dim newc As New newcustomer
        Dim dt As DataTable = GetNewCustomers(CustomerNo)
        If dt.Rows.Count > 0 Then
            newc.CustomerNo = dt.Rows(0).Item("CustomerNo")
            newc.Category = dt.Rows(0).Item("Category")
            newc.Name = dt.Rows(0).Item("Name")
            newc.VATNumber = dt.Rows(0).Item("VATNumber")
            newc.FiscalCode = dt.Rows(0).Item("FiscalCode")
            newc.Address = dt.Rows(0).Item("Address")
            newc.AddressNo = dt.Rows(0).Item("AddressNo")
            newc.PostCode = dt.Rows(0).Item("PostCode")
            newc.City = dt.Rows(0).Item("City")
            newc.County = dt.Rows(0).Item("County")
            newc.Phone1 = dt.Rows(0).Item("Phone1")
            newc.Phone2 = dt.Rows(0).Item("Phone2")
            newc.FaxNo = dt.Rows(0).Item("FaxNo")
            newc.Email = dt.Rows(0).Item("Email")
            newc.CountryCode = dt.Rows(0).Item("CountryCode")
            newc.CD = dt.Rows(0).Item("CD")
            newc.CIN = dt.Rows(0).Item("CIN")
            newc.ABI = dt.Rows(0).Item("ABI")
            newc.CAB = dt.Rows(0).Item("CAB")
            newc.CC = dt.Rows(0).Item("CC")
            newc.Imported = dt.Rows(0).Item("Imported")
            newc.Name2 = dt.Rows(0).Item("Name2")
            newc.Address2 = dt.Rows(0).Item("Address2")
            newc.StartingDate = dt.Rows(0).Item("StartingDate")
            newc.JustContact = dt.Rows(0).Item("JustContact")
            newc.CreatedBy = dt.Rows(0).Item("CreatedBy")
            newc.naturagiuridica = dt.Rows(0).Item("naturagiuridica")
            newc.ragsocsped1 = dt.Rows(0).Item("ragsocsped1")
            newc.ragsocsped2 = dt.Rows(0).Item("ragsocsped2")
            newc.naturagiuridicasped = dt.Rows(0).Item("naturagiuridicasped")
            newc.indirizzosped1 = dt.Rows(0).Item("indirizzosped1")
            newc.indirizzosped2 = dt.Rows(0).Item("indirizzosped2")
            newc.capsped = dt.Rows(0).Item("capsped")
            newc.localitasped = dt.Rows(0).Item("localitasped")
            newc.provinciasped = dt.Rows(0).Item("provinciasped")
            newc.paesesped = dt.Rows(0).Item("paesesped")
            newc.telefono1sped = dt.Rows(0).Item("telefono1sped")
            newc.telefono2sped = dt.Rows(0).Item("telefono2sped")
            newc.faxsped = dt.Rows(0).Item("faxsped")
            newc.emailsped = dt.Rows(0).Item("emailsped")
            newc.PaymentMethod = dt.Rows(0).Item("PaymentMethod")
            newc.PaymentTerms = dt.Rows(0).Item("PaymentTerms")
        End If
        Return newc
    End Function

    Public Function addNewCustomer(ByVal newc As newcustomer) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "INSERT INTO [newcustomer]("
            sql &= "[CustomerNo]"
            sql &= ",[Category]"
            sql &= ",[Name]"
            sql &= ",[VATNumber]"
            sql &= ",[FiscalCode]"
            sql &= ",[Address]"
            sql &= ",[AddressNo]"
            sql &= ",[PostCode]"
            sql &= ",[City]"
            sql &= ",[County]"
            sql &= ",[Phone1]"
            sql &= ",[Phone2]"
            sql &= ",[FaxNo]"
            sql &= ",[Email]"
            sql &= ",[CountryCode]"
            sql &= ",[CD]"
            sql &= ",[CIN]"
            sql &= ",[ABI]"
            sql &= ",[CAB]"
            sql &= ",[CC]"
            sql &= ",[Imported]"
            sql &= ",[Name2]"
            sql &= ",[Address2]"
            sql &= ",[StartingDate]"
            sql &= ",[JustContact]"
            sql &= ",[CreatedBy]"
            sql &= ",[naturagiuridica]"
            sql &= ",[ragsocsped1]"
            sql &= ",[ragsocsped2]"
            sql &= ",[naturagiuridicasped]"
            sql &= ",[indirizzosped1]"
            sql &= ",[indirizzosped2]"
            sql &= ",[capsped]"
            sql &= ",[localitasped]"
            sql &= ",[provinciasped]"
            sql &= ",[paesesped]"
            sql &= ",[telefono1sped]"
            sql &= ",[telefono2sped]"
            sql &= ",[faxsped]"
            sql &= ",[emailsped]"
            sql &= ",[PaymentMethod]"
            sql &= ",[PaymentTerms]"
            sql &= ")"
            sql &= "VALUES("
            sql &= "@CustomerNo"
            sql &= ",@Category"
            sql &= ",@Name"
            sql &= ",@VATNumber"
            sql &= ",@FiscalCode"
            sql &= ",@Address"
            sql &= ",@AddressNo"
            sql &= ",@PostCode"
            sql &= ",@City"
            sql &= ",@County"
            sql &= ",@Phone1"
            sql &= ",@Phone2"
            sql &= ",@FaxNo"
            sql &= ",@Email"
            sql &= ",@CountryCode"
            sql &= ",@CD"
            sql &= ",@CIN"
            sql &= ",@ABI"
            sql &= ",@CAB"
            sql &= ",@CC"
            sql &= ",@Imported"
            sql &= ",@Name2"
            sql &= ",@Address2"
            sql &= ",@StartingDate"
            sql &= ",@JustContact"
            sql &= ",@CreatedBy"
            sql &= ",@naturagiuridica"
            sql &= ",@ragsocsped1"
            sql &= ",@ragsocsped2"
            sql &= ",@naturagiuridicasped"
            sql &= ",@indirizzosped1"
            sql &= ",@indirizzosped2"
            sql &= ",@capsped"
            sql &= ",@localitasped"
            sql &= ",@provinciasped"
            sql &= ",@paesesped"
            sql &= ",@telefono1sped"
            sql &= ",@telefono2sped"
            sql &= ",@faxsped"
            sql &= ",@emailsped"
            sql &= ",@PaymentMethod"
            sql &= ",@PaymentTerms"
            sql &= ")"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = newc.CustomerNo
            cmd.Parameters.Add("@Category", SqlDbType.VarChar, 20).Value = newc.Category
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = newc.Name
            cmd.Parameters.Add("@VATNumber", SqlDbType.VarChar, 20).Value = newc.VATNumber
            cmd.Parameters.Add("@FiscalCode", SqlDbType.VarChar, 20).Value = newc.FiscalCode
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 50).Value = newc.Address
            cmd.Parameters.Add("@AddressNo", SqlDbType.VarChar, 10).Value = newc.AddressNo
            cmd.Parameters.Add("@PostCode", SqlDbType.VarChar, 20).Value = newc.PostCode
            cmd.Parameters.Add("@City", SqlDbType.VarChar, 30).Value = newc.City
            cmd.Parameters.Add("@County", SqlDbType.VarChar, 30).Value = newc.County
            cmd.Parameters.Add("@Phone1", SqlDbType.VarChar, 30).Value = newc.Phone1
            cmd.Parameters.Add("@Phone2", SqlDbType.VarChar, 30).Value = newc.Phone2
            cmd.Parameters.Add("@FaxNo", SqlDbType.VarChar, 30).Value = newc.FaxNo
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 80).Value = newc.Email
            cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar, 10).Value = newc.CountryCode
            cmd.Parameters.Add("@CD", SqlDbType.VarChar, 50).Value = newc.CD
            cmd.Parameters.Add("@CIN", SqlDbType.VarChar, 25).Value = newc.CIN
            cmd.Parameters.Add("@ABI", SqlDbType.VarChar, 5).Value = newc.ABI
            cmd.Parameters.Add("@CAB", SqlDbType.VarChar, 5).Value = newc.CAB
            cmd.Parameters.Add("@CC", SqlDbType.VarChar, 30).Value = newc.CC
            cmd.Parameters.Add("@Imported", SqlDbType.TinyInt).Value = newc.Imported
            cmd.Parameters.Add("@Name2", SqlDbType.VarChar, 50).Value = newc.Name2
            cmd.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = newc.Address2
            cmd.Parameters.Add("@StartingDate", SqlDbType.VarChar, 20).Value = newc.StartingDate
            cmd.Parameters.Add("@JustContact", SqlDbType.TinyInt).Value = newc.JustContact
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 30).Value = newc.CreatedBy
            cmd.Parameters.Add("@naturagiuridica", SqlDbType.VarChar, 50).Value = newc.naturagiuridica
            cmd.Parameters.Add("@ragsocsped1", SqlDbType.VarChar, 50).Value = newc.ragsocsped1
            cmd.Parameters.Add("@ragsocsped2", SqlDbType.VarChar, 50).Value = newc.ragsocsped2
            cmd.Parameters.Add("@naturagiuridicasped", SqlDbType.VarChar, 50).Value = newc.naturagiuridicasped
            cmd.Parameters.Add("@indirizzosped1", SqlDbType.VarChar, 50).Value = newc.indirizzosped1
            cmd.Parameters.Add("@indirizzosped2", SqlDbType.VarChar, 50).Value = newc.indirizzosped2
            cmd.Parameters.Add("@capsped", SqlDbType.VarChar, 20).Value = newc.capsped
            cmd.Parameters.Add("@localitasped", SqlDbType.VarChar, 30).Value = newc.localitasped
            cmd.Parameters.Add("@provinciasped", SqlDbType.VarChar, 30).Value = newc.provinciasped
            cmd.Parameters.Add("@paesesped", SqlDbType.VarChar, 30).Value = newc.paesesped
            cmd.Parameters.Add("@telefono1sped", SqlDbType.VarChar, 30).Value = newc.telefono1sped
            cmd.Parameters.Add("@telefono2sped", SqlDbType.VarChar, 30).Value = newc.telefono2sped
            cmd.Parameters.Add("@faxsped", SqlDbType.VarChar, 30).Value = newc.faxsped
            cmd.Parameters.Add("@emailsped", SqlDbType.VarChar, 80).Value = newc.emailsped
            cmd.Parameters.Add("@PaymentMethod", SqlDbType.VarChar, 30).Value = newc.PaymentMethod
            cmd.Parameters.Add("@PaymentTerms", SqlDbType.VarChar, 30).Value = newc.PaymentTerms

            conn.Open()
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Function

    Public Function updNewCustomer(ByVal newc As newcustomer) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [newcustomer] SET "
            sql &= "[Category]=@Category"
            sql &= ",[Name]=@Name"
            sql &= ",[VATNumber]=@VATNumber"
            sql &= ",[FiscalCode]=@FiscalCode"
            sql &= ",[Address]=@Address"
            sql &= ",[AddressNo]=@AddressNo"
            sql &= ",[PostCode]=@PostCode"
            sql &= ",[City]=@City"
            sql &= ",[County]=@County"
            sql &= ",[Phone1]=@Phone1"
            sql &= ",[Phone2]=@Phone2"
            sql &= ",[FaxNo]=@FaxNo"
            sql &= ",[Email]=@Email"
            sql &= ",[CountryCode]=@CountryCode"
            sql &= ",[CD]=@CD"
            sql &= ",[CIN]=@CIN"
            sql &= ",[ABI]=@ABI"
            sql &= ",[CAB]=@CAB"
            sql &= ",[CC]=@CC"
            'sql &= ",[Imported]=@Imported" ' già commentati pre migrazione a BC
            sql &= ",[Name2]=@Name2"
            sql &= ",[Address2]=@Address2"
            'sql &= ",[StartingDate]=@StartingDate" ' già commentati pre migrazione a BC
            sql &= ",[JustContact]=@JustContact"
            sql &= ",[CreatedBy]=@CreatedBy"
            sql &= ",[naturagiuridica]=@naturagiuridica"
            sql &= ",[ragsocsped1]=@ragsocsped1"
            sql &= ",[ragsocsped2]=@ragsocsped2"
            sql &= ",[naturagiuridicasped]=@naturagiuridicasped"
            sql &= ",[indirizzosped1]=@indirizzosped1"
            sql &= ",[indirizzosped2]=@indirizzosped2"
            sql &= ",[capsped]=@capsped"
            sql &= ",[localitasped]=@localitasped"
            sql &= ",[provinciasped]=@provinciasped"
            sql &= ",[paesesped]=@paesesped"
            sql &= ",[telefono1sped]=@telefono1sped"
            sql &= ",[telefono2sped]=@telefono2sped"
            sql &= ",[faxsped]=@faxsped"
            sql &= ",[emailsped]=@emailsped"
            sql &= ",[PaymentMethod]=@PaymentMethod"
            sql &= ",[PaymentTerms]=@PaymentTerms"
            sql &= " WHERE [CustomerNo]=@CustomerNo"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@Category", SqlDbType.VarChar, 20).Value = newc.Category
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = newc.Name
            cmd.Parameters.Add("@VATNumber", SqlDbType.VarChar, 20).Value = newc.VATNumber
            cmd.Parameters.Add("@FiscalCode", SqlDbType.VarChar, 20).Value = newc.FiscalCode
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 50).Value = newc.Address
            cmd.Parameters.Add("@AddressNo", SqlDbType.VarChar, 10).Value = newc.AddressNo
            cmd.Parameters.Add("@PostCode", SqlDbType.VarChar, 20).Value = newc.PostCode
            cmd.Parameters.Add("@City", SqlDbType.VarChar, 30).Value = newc.City
            cmd.Parameters.Add("@County", SqlDbType.VarChar, 30).Value = newc.County
            cmd.Parameters.Add("@Phone1", SqlDbType.VarChar, 30).Value = newc.Phone1
            cmd.Parameters.Add("@Phone2", SqlDbType.VarChar, 30).Value = newc.Phone2
            cmd.Parameters.Add("@FaxNo", SqlDbType.VarChar, 30).Value = newc.FaxNo
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 80).Value = newc.Email
            cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar, 10).Value = newc.CountryCode
            cmd.Parameters.Add("@CD", SqlDbType.VarChar, 50).Value = newc.CD
            cmd.Parameters.Add("@CIN", SqlDbType.VarChar, 25).Value = newc.CIN
            cmd.Parameters.Add("@ABI", SqlDbType.VarChar, 5).Value = newc.ABI
            cmd.Parameters.Add("@CAB", SqlDbType.VarChar, 5).Value = newc.CAB
            cmd.Parameters.Add("@CC", SqlDbType.VarChar, 30).Value = newc.CC
            'cmd.Parameters.Add("@Imported", SqlDbType.TinyInt).Value = newc.Imported ' già commentati pre migrazione a BC
            cmd.Parameters.Add("@Name2", SqlDbType.VarChar, 50).Value = newc.Name2
            cmd.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = newc.Address2
            'cmd.Parameters.Add("@StartingDate", SqlDbType.VarChar, 20).Value = newc.StartingDate ' già commentati pre migrazione a BC
            cmd.Parameters.Add("@JustContact", SqlDbType.TinyInt).Value = newc.JustContact
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 30).Value = newc.CreatedBy
            cmd.Parameters.Add("@naturagiuridica", SqlDbType.VarChar, 50).Value = newc.naturagiuridica
            cmd.Parameters.Add("@ragsocsped1", SqlDbType.VarChar, 50).Value = newc.ragsocsped1
            cmd.Parameters.Add("@ragsocsped2", SqlDbType.VarChar, 50).Value = newc.ragsocsped2
            cmd.Parameters.Add("@naturagiuridicasped", SqlDbType.VarChar, 50).Value = newc.naturagiuridicasped
            cmd.Parameters.Add("@indirizzosped1", SqlDbType.VarChar, 50).Value = newc.indirizzosped1
            cmd.Parameters.Add("@indirizzosped2", SqlDbType.VarChar, 50).Value = newc.indirizzosped2
            cmd.Parameters.Add("@capsped", SqlDbType.VarChar, 20).Value = newc.capsped
            cmd.Parameters.Add("@localitasped", SqlDbType.VarChar, 30).Value = newc.localitasped
            cmd.Parameters.Add("@provinciasped", SqlDbType.VarChar, 30).Value = newc.provinciasped
            cmd.Parameters.Add("@paesesped", SqlDbType.VarChar, 30).Value = newc.paesesped
            cmd.Parameters.Add("@telefono1sped", SqlDbType.VarChar, 30).Value = newc.telefono1sped
            cmd.Parameters.Add("@telefono2sped", SqlDbType.VarChar, 30).Value = newc.telefono2sped
            cmd.Parameters.Add("@faxsped", SqlDbType.VarChar, 30).Value = newc.faxsped
            cmd.Parameters.Add("@emailsped", SqlDbType.VarChar, 80).Value = newc.emailsped
            cmd.Parameters.Add("@PaymentMethod", SqlDbType.VarChar, 30).Value = newc.PaymentMethod
            cmd.Parameters.Add("@PaymentTerms", SqlDbType.VarChar, 30).Value = newc.PaymentTerms
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = newc.CustomerNo

            conn.Open()
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Function

    Public Function GetBusinessRelation(Optional ByVal code As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT Code,Description FROM [NAV_Business Relation] "
            If code <> "" Then
                sql &= " WHERE code=@code"
                cmd.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = code
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception("Errore GetBusinessRelation: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function getNewCustomerPagamenti(ByVal categoriaCliente As String) As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [newCustomerPagamenti] WHERE CategoriaCliente=@CategoriaCliente"
            cmd.Parameters.Add("@CategoriaCliente", SqlDbType.VarChar, 20).Value = categoriaCliente
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception("Errore getNewCustomerPagamenti: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetCountry(Optional ByVal EUCountryRegionCode As String = "") As DataTable
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT [EU Country_Region Code] AS Code, [Name] AS Description FROM [Country_Region] WHERE [EU Country_Region Code]<>'' "
            If EUCountryRegionCode <> "" Then
                sql &= " AND [EU Country_Region Code]]=@EUCountryRegionCode"
                cmd.Parameters.Add("@EUCountryRegionCode", SqlDbType.VarChar, 10).Value = EUCountryRegionCode
            End If
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0)
        Catch ex As Exception
            Throw New Exception("Errore GetCountry: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetNewCustomerName(ByVal CustomerNo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select Name + ' ' + [Name2] AS customerName FROM [newcustomer] WHERE [CustomerNo]=@CustomerNo", conn)
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ""
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function GetNewCustomerEmail(ByVal CustomerNo As String) As List(Of String)
        Dim conn As SqlConnection = Nothing
        Dim listaEmail As New List(Of String)
        Try
            conn = New SqlConnection(WORconnectionString)
            Dim sql As String = "Select [Email] FROM [newcustomer] WHERE [CustomerNo]=@CustomerNo"
            Dim da As New SqlDataAdapter(sql, conn)
            da.SelectCommand.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("Email") <> "" Then
                    listaEmail.Add(dt.Rows(0).Item("Email"))
                End If
            End If
            Return listaEmail
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return listaEmail
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Public Function addOrderForNewCustomer(ByVal orderNo As String, ByVal CustomerNo As String) As Integer
        Dim newc As newcustomer
        newc = getNewCustomerEntity(CustomerNo)
        Dim ctrl As Integer = 0
        Dim conn As New SqlConnection(NAVconnectionString)
        Try
            Dim sql As String = "INSERT INTO [CustomersFromOutside]("
            sql &= "[OrderNo]"
            sql &= ",[CustomerNo]"
            sql &= ",[Category]"
            sql &= ",[Name]"
            sql &= ",[VATNumber]"
            sql &= ",[FiscalCode]"
            sql &= ",[Address]"
            sql &= ",[AddressNo]"
            sql &= ",[PostCode]"
            sql &= ",[City]"
            sql &= ",[County]"
            sql &= ",[Phone1]"
            sql &= ",[Phone2]"
            sql &= ",[FaxNo]"
            sql &= ",[Email]"
            sql &= ",[CountryCode]"
            sql &= ",[CD]"
            sql &= ",[CIN]"
            sql &= ",[ABI]"
            sql &= ",[CAB]"
            sql &= ",[CC]"
            sql &= ",[Imported]"
            sql &= ",[Name2]"
            sql &= ",[Address2]"
            sql &= ",[StartingDate]"
            sql &= ",[JustContact]"
            sql &= ",[CreatedBy]"
            sql &= ",[naturagiuridica]"
            sql &= ",[ragsocsped1]"
            sql &= ",[ragsocsped2]"
            sql &= ",[naturagiuridicasped]"
            sql &= ",[indirizzosped1]"
            sql &= ",[indirizzosped2]"
            sql &= ",[capsped]"
            sql &= ",[localitasped]"
            sql &= ",[provinciasped]"
            sql &= ",[paesesped]"
            sql &= ",[telefono1sped]"
            sql &= ",[telefono2sped]"
            sql &= ",[faxsped]"
            sql &= ",[emailsped]"
            sql &= ",[PaymentMethod]"
            sql &= ",[PaymentTerms]"
            sql &= ")"
            sql &= "VALUES("
            sql &= "@OrderNo"
            sql &= ",@CustomerNo"
            sql &= ",@Category"
            sql &= ",@Name"
            sql &= ",@VATNumber"
            sql &= ",@FiscalCode"
            sql &= ",@Address"
            sql &= ",@AddressNo"
            sql &= ",@PostCode"
            sql &= ",@City"
            sql &= ",@County"
            sql &= ",@Phone1"
            sql &= ",@Phone2"
            sql &= ",@FaxNo"
            sql &= ",@Email"
            sql &= ",@CountryCode"
            sql &= ",@CD"
            sql &= ",@CIN"
            sql &= ",@ABI"
            sql &= ",@CAB"
            sql &= ",@CC"
            sql &= ",@Imported"
            sql &= ",@Name2"
            sql &= ",@Address2"
            sql &= ",@StartingDate"
            sql &= ",@JustContact"
            sql &= ",@CreatedBy"
            sql &= ",@naturagiuridica"
            sql &= ",@ragsocsped1"
            sql &= ",@ragsocsped2"
            sql &= ",@naturagiuridicasped"
            sql &= ",@indirizzosped1"
            sql &= ",@indirizzosped2"
            sql &= ",@capsped"
            sql &= ",@localitasped"
            sql &= ",@provinciasped"
            sql &= ",@paesesped"
            sql &= ",@telefono1sped"
            sql &= ",@telefono2sped"
            sql &= ",@faxsped"
            sql &= ",@emailsped"
            sql &= ",@PaymentMethod"
            sql &= ",@PaymentTerms"
            sql &= ")"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar, 20).Value = orderNo
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = newc.CustomerNo
            cmd.Parameters.Add("@Category", SqlDbType.VarChar, 20).Value = newc.Category
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = newc.Name
            cmd.Parameters.Add("@VATNumber", SqlDbType.VarChar, 20).Value = newc.VATNumber
            cmd.Parameters.Add("@FiscalCode", SqlDbType.VarChar, 20).Value = newc.FiscalCode
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 50).Value = newc.Address
            cmd.Parameters.Add("@AddressNo", SqlDbType.VarChar, 10).Value = newc.AddressNo
            cmd.Parameters.Add("@PostCode", SqlDbType.VarChar, 20).Value = newc.PostCode
            cmd.Parameters.Add("@City", SqlDbType.VarChar, 30).Value = newc.City
            cmd.Parameters.Add("@County", SqlDbType.VarChar, 30).Value = newc.County
            cmd.Parameters.Add("@Phone1", SqlDbType.VarChar, 30).Value = newc.Phone1
            cmd.Parameters.Add("@Phone2", SqlDbType.VarChar, 30).Value = newc.Phone2
            cmd.Parameters.Add("@FaxNo", SqlDbType.VarChar, 30).Value = newc.FaxNo
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 80).Value = newc.Email
            cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar, 10).Value = newc.CountryCode
            cmd.Parameters.Add("@CD", SqlDbType.VarChar, 50).Value = newc.CD
            cmd.Parameters.Add("@CIN", SqlDbType.VarChar, 25).Value = newc.CIN
            cmd.Parameters.Add("@ABI", SqlDbType.VarChar, 5).Value = newc.ABI
            cmd.Parameters.Add("@CAB", SqlDbType.VarChar, 5).Value = newc.CAB
            cmd.Parameters.Add("@CC", SqlDbType.VarChar, 30).Value = newc.CC
            cmd.Parameters.Add("@Imported", SqlDbType.TinyInt).Value = newc.Imported
            cmd.Parameters.Add("@Name2", SqlDbType.VarChar, 50).Value = newc.Name2
            cmd.Parameters.Add("@Address2", SqlDbType.VarChar, 50).Value = newc.Address2
            cmd.Parameters.Add("@StartingDate", SqlDbType.VarChar, 20).Value = newc.StartingDate
            cmd.Parameters.Add("@JustContact", SqlDbType.TinyInt).Value = newc.JustContact
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 30).Value = newc.CreatedBy
            cmd.Parameters.Add("@naturagiuridica", SqlDbType.VarChar, 50).Value = newc.naturagiuridica
            cmd.Parameters.Add("@ragsocsped1", SqlDbType.VarChar, 50).Value = newc.ragsocsped1
            cmd.Parameters.Add("@ragsocsped2", SqlDbType.VarChar, 50).Value = newc.ragsocsped2
            cmd.Parameters.Add("@naturagiuridicasped", SqlDbType.VarChar, 50).Value = newc.naturagiuridicasped
            cmd.Parameters.Add("@indirizzosped1", SqlDbType.VarChar, 50).Value = newc.indirizzosped1
            cmd.Parameters.Add("@indirizzosped2", SqlDbType.VarChar, 50).Value = newc.indirizzosped2
            cmd.Parameters.Add("@capsped", SqlDbType.VarChar, 20).Value = newc.capsped
            cmd.Parameters.Add("@localitasped", SqlDbType.VarChar, 30).Value = newc.localitasped
            cmd.Parameters.Add("@provinciasped", SqlDbType.VarChar, 30).Value = newc.provinciasped
            cmd.Parameters.Add("@paesesped", SqlDbType.VarChar, 30).Value = newc.paesesped
            cmd.Parameters.Add("@telefono1sped", SqlDbType.VarChar, 30).Value = newc.telefono1sped
            cmd.Parameters.Add("@telefono2sped", SqlDbType.VarChar, 30).Value = newc.telefono2sped
            cmd.Parameters.Add("@faxsped", SqlDbType.VarChar, 30).Value = newc.faxsped
            cmd.Parameters.Add("@emailsped", SqlDbType.VarChar, 80).Value = newc.emailsped
            cmd.Parameters.Add("@PaymentMethod", SqlDbType.VarChar, 30).Value = newc.PaymentMethod
            cmd.Parameters.Add("@PaymentTerms", SqlDbType.VarChar, 30).Value = newc.PaymentTerms

            conn.Open()

            ctrl = cmd.ExecuteNonQuery()
            If ctrl > 0 Then
                updNewCustomerSetImported(newc) 'indico che il cliente nuovo ha effettuato un ordine e non sarà più modificabile la sua anagrafica
            End If
            Return ctrl
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Function


    Public Function updNewCustomerSetImported(ByVal newc As newcustomer) As Integer
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            Dim sql As String = "UPDATE [newcustomer] SET "
            sql &= " [Imported]=1"
            sql &= " WHERE [CustomerNo]=@CustomerNo"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@Imported", SqlDbType.TinyInt).Value = newc.Imported
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = newc.CustomerNo

            conn.Open()
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Function

    Public Function GetNewCustomerIBAN(ByVal CustomerNo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select CD,CIN,ABI,CAB,CC FROM [newcustomer] WHERE [CustomerNo]=@CustomerNo", conn)
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            If ds.Tables(0).Rows(0).Item("CD") <> "" And ds.Tables(0).Rows(0).Item("CIN") <> "" And ds.Tables(0).Rows(0).Item("ABI") <> "" And ds.Tables(0).Rows(0).Item("CAB") <> "" And ds.Tables(0).Rows(0).Item("CC") <> "" Then
                Return (ds.Tables(0).Rows(0).Item("CD") & ds.Tables(0).Rows(0).Item("CIN") & ds.Tables(0).Rows(0).Item("ABI") & ds.Tables(0).Rows(0).Item("CAB") & ds.Tables(0).Rows(0).Item("CC"))
            Else
                Return ""
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return ""
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function


    'aggiunta il 2017-05-05: in realtà il controllo era già presente secondo altra logica: vedi synchNewCustomerCertificazione()
    '2017-05-08: si è deciso di utilizzare la seguente procedura in seguito alla modifica alla procedura di certificazione lato NAV che ora esegue l'update del codicecliente in OrderHeaderFromWeb
    Public Sub verificaCertificazioneOrdiniNuociCliente()
        Dim connNAV As New SqlConnection(NAVconnectionString)
        Dim connWO As New SqlConnection(WORconnectionString)
        Try
            Dim cmdWO As New SqlCommand("SELECT [Code] FROM [carrelloHeader] WHERE [CODICECLIENTE] LIKE 'N%'", connWO)
            Dim daWO As New SqlDataAdapter
            daWO.SelectCommand = cmdWO
            Dim dsWO As New DataSet()
            daWO.Fill(dsWO)
            If dsWO.Tables(0).Rows.Count > 0 Then
                For Each r As DataRow In dsWO.Tables(0).Rows
                    Dim orderCode As String = r("Code")
                    Dim cmdNav As New SqlCommand("SELECT [CustomerNo] FROM [OrderHeaderFromWeb_Write] WHERE [Code]=@OrderCode", connNAV)
                    cmdNav.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = orderCode
                    connNAV.Open()
                    Dim CustomerNoFromNav As String = cmdNav.ExecuteScalar()
                    connNAV.Close()
                    If Not CustomerNoFromNav Is Nothing AndAlso CustomerNoFromNav.StartsWith("C") Then
                        Dim cmdWOupd As New SqlCommand("UPDATE [carrelloHeader] SET [CODICECLIENTE]=@CODICECLIENTE, [CustomerNo]=@CustomerNo WHERE [Code]=@OrderCode", connWO)
                        cmdWOupd.Parameters.Add("@CODICECLIENTE", SqlDbType.VarChar, 30).Value = CustomerNoFromNav
                        cmdWOupd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNoFromNav
                        cmdWOupd.Parameters.Add("@OrderCode", SqlDbType.VarChar, 20).Value = orderCode
                        connWO.Open()
                        cmdWOupd.ExecuteNonQuery()
                        connWO.Close()
                    End If
                Next
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If connNAV.State = ConnectionState.Open Then connNAV.Close()
            If connWO.State = ConnectionState.Open Then connWO.Close()
        End Try
    End Sub

#End Region


End Class
