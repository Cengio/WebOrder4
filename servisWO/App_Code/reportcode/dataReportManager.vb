Imports System.Data.SqlClient
Imports System.Data
Imports servisReports.reportordini

Public Class dataReportManager
    Public Property NAVconnectionString() As String = [String].Empty
    Public Property WORconnectionString() As String = [String].Empty
    Public Property workingCompany() As String = [String].Empty
    Public Property showDataUltimaModifica As Boolean = False

    Private Function getOrderHeaderWO(ByVal Code As String) As DataSet
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

    Private Function GetOrderLinesWO(ByVal orderCode As String) As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select * FROM [carrelloLines] WHERE [CompanyName_]='" & workingCompany & "' AND [orderCode]=@orderCode ORDER BY LineID"
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

    Private Function GetShipToAddress(ByVal customerCode As String, ByVal shiptoCode As String) As DataRow

        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            sql = "Select Name, Address, City, County, [Post Code] FROM [Ser-Vis$Ship-to Address] WHERE [Customer No_]=@customerCode AND [Code]=@shiptoCode"
            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = customerCode
            cmd.Parameters.Add("@shiptoCode", SqlDbType.VarChar, 10).Value = shiptoCode
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0).Rows(0)
        Catch ex As Exception
            Throw New Exception("Errore GetShipToAddress: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function GetShipToAddressFromOrder(ByVal orderCode As String) As DataRow
        ' Funzione aggiunta da Massimo G. in affiancamento alla GetShipToAddress
        ' per prelevare i dati spedizione dalla testata dell'ordine di Navision anzichè dalle destinazioni diverse.
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            sql = "SELECT [Ship-to Name] + [Ship-to Name 2] as Name" &
                  ",[Ship-to Address] + [Ship-to Address 2] as Address " &
                  ",[Ship-to City] as City " &
                  ",[Ship-to County] as County " &
                  ",[Ship-to Post Code] as [Post Code] " &
                 "FROM [dbo].[workingCompany$Sales Header] " &
                 "where No_ = @orderCode"
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar, 20).Value = orderCode
            cmd.CommandText = sql.Replace("workingCompany", workingCompany)
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            If (ds.Tables(0).Rows.Count > 0) Then
                Return ds.Tables(0).Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw New Exception("Errore GetShipToAddress: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function GetShippingAgentForCustomer(ByVal customerNo As String) As Hashtable
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
            If ds.Tables(0).Rows(0).Item("Shipping Agent Code") <> "" Then
                sac.Add(ds.Tables(0).Rows(0).Item("Shipping Agent Code"), GetShippingAgentName(ds.Tables(0).Rows(0).Item("Shipping Agent Code")))
            End If
            If ds.Tables(0).Rows(0).Item("Shipping Agent Code II") <> "" Then
                sac.Add(ds.Tables(0).Rows(0).Item("Shipping Agent Code II"), GetShippingAgentName(ds.Tables(0).Rows(0).Item("Shipping Agent Code II")))
            End If
            If ds.Tables(0).Rows(0).Item("Shipping Agent Code III") <> "" Then
                sac.Add(ds.Tables(0).Rows(0).Item("Shipping Agent Code III"), GetShippingAgentName(ds.Tables(0).Rows(0).Item("Shipping Agent Code III")))
            End If
            Return sac
        Catch ex As Exception
            Throw New Exception("Errore GetShippingAgentForCustomer: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function GetShippingAgentName(ByVal code As String) As String
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

    Private Function GetCustomers(ByVal customerCode As String) As DataRow
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String
            sql = " Select * FROM [NAV Customer] WHERE [No_]=@customerCode"
            cmd.Parameters.Add("@customerCode", SqlDbType.VarChar, 20).Value = customerCode
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0).Rows(0)
        Catch ex As Exception
            Throw New Exception("Errore GetCustomers: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function GetNewCustomers(ByVal CustomerNo As String) As DataRow
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = " Select * FROM [newCustomer] WHERE [CustomerNo]=@CustomerNo"
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            cmd.CommandText = sql
            cmd.Connection = conn
            Dim da As New SqlDataAdapter
            da.SelectCommand = cmd
            Dim ds As New DataSet()
            da.Fill(ds)
            Return ds.Tables(0).Rows(0)
        Catch ex As Exception
            Throw New Exception("Errore GetNewCustomers: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Function

    Private Function GetPaymentMethod(ByVal code As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT Description FROM [NAV_PaymentMethod]  WHERE code=@code"
            cmd.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = code
            cmd.CommandText = sql
            cmd.Connection = conn
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception("Errore GetPaymentMethodCode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function GetPaymentTerms(ByVal code As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT Description FROM [NAV_PaymentTerms] WHERE code=@code"
            cmd.Parameters.Add("@code", SqlDbType.VarChar, 10).Value = code
            cmd.CommandText = sql
            cmd.Connection = conn
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception("Errore GetPaymentTermsCode: " & ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function GetSpeseTrasporto() As Double
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT B.[Amount Excl_ VAT] AS speTrasporto FROM [" & workingCompany & "$Sales_Receivables Setup] AS A INNER JOIN [" & workingCompany & "$Standard Sales Line] AS B ON A.[Std Shipment Sales Code]=B.[Standard Sales Code]", conn)
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function GetSpeseIncasso(ByVal CustomerNo As String, ByVal PaymentMethodCode As String) As Double
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim sql As String = "SELECT A.[Amount Excl_ VAT] AS speIncasso FROM [Ser-Vis Srl$Standard Sales Line] AS A "
            sql &= " INNER JOIN [Ser-Vis Srl$Standard Customer Sales Code] AS B ON A.[Standard Sales Code]=B.[Code]"
            sql &= " WHERE B.[Customer No_]=@CustomerNo AND B.[Payment Method Code]=@PaymentMethodCode"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.Add("@CustomerNo", SqlDbType.VarChar, 20).Value = CustomerNo
            cmd.Parameters.Add("@PaymentMethodCode", SqlDbType.VarChar, 10).Value = PaymentMethodCode
            Return cmd.ExecuteScalar
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return 0
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Function

    Private Function getCategoriaClienteDescrizione(ByVal customerNo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(NAVconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("SELECT A.[Name] FROM [SerVisDimensionValue] AS A INNER JOIN [NAV Customer] AS B ON A.Code=B.[Global Dimension 1 Code] WHERE A.[Dimension Code]='CLI_CATEGORIA' AND A.Blocked=0 and A.[Dimension Value Type]=0 AND B.[No_]=@customerNo", conn)
            cmd.Parameters.Add("@customerNo", SqlDbType.VarChar, 20).Value = customerNo
            Dim res As Object = cmd.ExecuteScalar
            If Not res Is Nothing Then
                Return res
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

    Private Function esisteUserByCode(ByVal UserCode As Integer) As Boolean
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            If UserCode > 0 Then
                conn.Open()
                Dim cmd As New SqlCommand
                Dim sql As String = "SELECT COUNT(*) FROM [WU_users] WHERE [User Code]=@UserCode"
                cmd.Parameters.Add("@UserCode", SqlDbType.Int).Value = UserCode
                cmd.CommandText = sql
                cmd.Connection = conn
                Return CBool(cmd.ExecuteScalar)
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try

    End Function

    Private Function getUserNameSurname(ByVal UserCode As Integer) As String
        Dim conn As New SqlConnection(WORconnectionString)
        Dim userNameSurname As String = ""
        Try
            If esisteUserByCode(UserCode) Then
                Dim sql As String = "SELECT Cognome + ' ' + Nome AS userNameSurname FROM [WU_users] WHERE [User Code]=@UserCode"
                conn.Open()
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.Add("@UserCode", SqlDbType.BigInt).Value = UserCode
                Return cmd.ExecuteScalar
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

    Private Function getCarrelloHeaderByOrderCode(ByVal orderCode As String) As DataTable
        Dim conn As New SqlConnection(WORconnectionString)
        Try
            conn.Open()
            Dim idcart As Int64 = 0
            Dim cmd As New SqlCommand
            Dim sql As String = "SELECT * FROM [carrelloHeader] WHERE [Code]=@orderCode"
            cmd.Parameters.Add("@orderCode", SqlDbType.VarChar).Value = orderCode
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


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ordercode"></param>
    ''' <param name="linesOrderByField"></param>
    ''' <returns></returns>
    Public Function getOrder(ByVal ordercode As String, Optional ByVal linesOrderByField As String = "LineID", Optional ByVal reportLogo As String = "", Optional ByVal reportFooterDescription As String = "") As orderHeaderReportCollection
        Dim odr As New orderHeaderReportCollection
        Dim drH As DataRow = getOrderHeaderWO(ordercode).Tables(0).Rows(0)
        Dim h As New orderHeaderReport(ordercode)
        Dim drCustomer As DataRow

        If drH.Item("CustomerNo").ToString.StartsWith("N") Then
            drCustomer = GetNewCustomers(drH.Item("CustomerNo"))
            h.nomecliente = drCustomer.Item("Name") & " " & drCustomer.Item("Name2")
            h.indirizzocliente = drCustomer.Item("Address") & " " & drCustomer.Item("AddressNo")
            h.cittacliente = drCustomer.Item("City")
            h.provinciacliente = drCustomer.Item("County")
            h.capcliente = drCustomer.Item("PostCode")
            h.telefono = drCustomer.Item("Phone1")
            h.cellulare = drCustomer.Item("Phone2")
            h.fax = drCustomer.Item("FaxNo")
        Else
            drCustomer = GetCustomers(drH.Item("CustomerNo"))
            h.nomecliente = drCustomer.Item("Name")
            h.indirizzocliente = drCustomer.Item("Address")
            h.cittacliente = drCustomer.Item("City")
            h.provinciacliente = drCustomer.Item("County")
            h.capcliente = drCustomer.Item("Post Code")
            h.telefono = drCustomer.Item("Phone No_")
            h.cellulare = drCustomer.Item("Mobile No_")
            h.fax = drCustomer.Item("Fax No_")
            h.categoriaMerceologica = ""
            Dim catmerc As String = getCategoriaClienteDescrizione(drH.Item("CustomerNo"))
            If catmerc <> "" Then
                h.categoriaMerceologica = "(" & catmerc & ")"
            End If
        End If
        h.OrderDate = drH.Item("OrderDate")
        If showDataUltimaModifica Then
            h.DATA_ULTIMA_MODIFICA = CDate(drH.Item("DATA_ULTIMA_MODIFICA")).ToString("dd/MM/yyyy HH:mm:ss")
        Else
            h.DATA_ULTIMA_MODIFICA = ""
        End If
        h.CustomerNo = drH.Item("CustomerNo")
        h.QRcode = h.CustomerNo & "-" & h.Code
        h.Notes = drH.Item("Notes")


        If drH.Item("ShipAddressCode") <> "" Then
            ' Modifica by Massimo G:
            ' precedentemente alla mia modifica, per prendere i dati di destinazione, si andava dritti sulla tabella delle destinazioni diverse, ma non era corretto 
            ' perchè la tabella delle destinazioni è dinamica (nel tempo le destinazioni cambiano e pertanto uno stesso ordine ristampato in tempi diversi può presentare dati diversi)
            ' es inoltre se quella destinazione diversa non esiste più, aprendo il report dell'ordine si ottiene una bella EXCEPTION (es. ordine 233960).

            ' Ora, si va PRIMA a vedere se l'ordine è già stato importato in Navision e in caso affermativo, si prendono i dati di spedizione staticizzati nella testata ordine.
            ' Poi, se e SOLO SE l'ordine non è stato ancora importato in NAV, allora si va ad aprire la tabella delle destinazioni diverse come veniva fatto precedentemente.

            'Dim drShipTo As DataRow = GetShipToAddress(h.CustomerNo, drH.Item("ShipAddressCode"))
            Dim drShipTo As DataRow = GetShipToAddressFromOrder(ordercode)

            If Not drShipTo Is Nothing Then
                ' ho trovato l'ordine nella tabella order header di Nav
                h.nomespedizione = drShipTo.Item("Name")
                h.indirizzospedizione = drShipTo.Item("Address")
                h.cittaspedizione = drShipTo.Item("City")
                h.provinciaspedizione = drShipTo.Item("County")
                h.capspedizione = drShipTo.Item("Post Code")
            Else
                ' Se nell'order header di Nav non ho trovato l'ordine allora interpello le destinazioni diverse
                drShipTo = GetShipToAddress(h.CustomerNo, drH.Item("ShipAddressCode"))
                h.nomespedizione = drShipTo.Item("Name")
                h.indirizzospedizione = drShipTo.Item("Address")
                h.cittaspedizione = drShipTo.Item("City")
                h.provinciaspedizione = drShipTo.Item("County")
                h.capspedizione = drShipTo.Item("Post Code")
            End If
        End If

        Dim speseincasso As Double = 0
        If drH.Item("PaymentMethodCode") <> "" Then
            h.PaymentMethodCode = drH.Item("PaymentMethodCode")
            h.metododipagamento = GetPaymentMethod(drH.Item("PaymentMethodCode"))
            speseincasso = GetSpeseIncasso(h.CustomerNo, drH.Item("PaymentMethodCode"))
        End If
        If drH.Item("PaymentTermsCode") <> "" Then
            h.PaymentTermsCode = drH.Item("PaymentTermsCode")
            h.terminedipagamento = GetPaymentTerms(drH.Item("PaymentTermsCode"))
        End If
        If drH.Item("ShippingAgentCode") <> "" Then
            h.spedizioniere = GetShippingAgentName(drH.Item("ShippingAgentCode"))
        End If
        'spese di trasporto
        Dim speseditrasporto As Double = 0
        If drH("IncludeShipCost") = 1 Then
            speseditrasporto = GetSpeseTrasporto()
        End If

        Dim dtCarrello As DataTable = getCarrelloHeaderByOrderCode(ordercode)
        If dtCarrello.Rows.Count > 0 Then
            h.User = getUserNameSurname(dtCarrello.Rows(0).Item("UTENTE_ULTIMA_MODIFICA")) 'utente ultima_modifica
            h.OperatorCode = getUserNameSurname(dtCarrello.Rows(0).Item("UTENTE_CREAZIONE")) 'utente creazione
        Else
            h.User = drH.Item("User")
            h.OperatorCode = drH.Item("OperatorCode")
        End If

        'sconto pagamento
        h.scontoPagamentoPerc = drH.Item("SCONTOPAGAMENTO")

        'prenotazioni
        h.PRENOTAZIONE = drH.Item("PRENOTAZIONE")
        h.DATA_EVASIONE = CDate(drH.Item("DATA_EVASIONE")).ToString("dd/MM/yyyy")

        Dim totaleiva As Double = 0
        Dim totaleimponibileNetto As Double = 0
        Dim totaleimponibileLordo As Double = 0 '20200226  - gestione sconto pagamento su righe: totaleimponibileLordo = totaleimponibileNetto + scontoPagamentTotale 
        Dim scontoPagamentoPerc As Double = h.scontoPagamentoPerc
        Dim scontoPagamentoTotale As Double = 0
        Dim valoreVoucher As Double = drH.Item("Voucher Value")

        Dim dtL As DataTable = GetOrderLinesWO(ordercode).Tables(0)
        For Each r As DataRow In dtL.Rows
            Dim totaleriga As Double = 0
            Dim l As New orderLineReport(r("lineid"))
            l.RowDiscount = r("RowDiscount")
            l.OrderCode = h.Code
            l.ItemCode = r("ItemCode")
            l.FormulaSconto = r("FormulaSconto")
            If l.FormulaSconto <> "" AndAlso l.FormulaSconto <> "0" Then
                l.ScontoRiga = l.FormulaSconto.Replace("-", "+")
            End If

            If l.RowDiscount = 0 Then
                l.Quantity = CInt(r("Quantity")).ToString
                l.UnitPrice = String.Format("€ {0:F2}", r("UnitPrice"))
                totaleriga = (r("UnitPrice") * r("Quantity"))

                '20200226  - gestione sconto pagamento su righe: ScoRiga + ScoTestata + ScoPagamento
                If r("SCONTORIGA") > 0 Then
                    totaleriga = totaleriga - (totaleriga / 100 * Convert.ToInt32(r("SCONTORIGA")))
                End If
                '20200424 - Gestione scontoScaglioni
                If r("SCONTOHEADER") <> "0" And r("SCONTOHEADER") <> "" Then
                    Dim scontiH() As String = r("SCONTOHEADER").ToString.Split("+")
                    For i = 0 To scontiH.Count - 1
                        If IsNumeric(scontiH(i)) Then
                            totaleriga = totaleriga - (totaleriga / 100 * scontiH(i))
                        End If
                    Next
                End If
                totaleimponibileLordo = totaleimponibileLordo + totaleriga
                If r("SCONTOPAGAMENTO") > 0 Then
                    scontoPagamentoTotale = scontoPagamentoTotale + (totaleriga / 100 * Convert.ToInt32(r("SCONTOPAGAMENTO")))
                    totaleriga = totaleriga - (totaleriga / 100 * Convert.ToInt32(r("SCONTOPAGAMENTO")))
                End If

            Else
                l.Quantity = CInt(r("DiscountQty")).ToString
                l.UnitPrice = String.Format("€ {0:F2}", r("UnitPrice"))
                totaleriga = 0.0
                l.ScontoRiga = "100"
            End If

            totaleiva += (totaleriga / 100 * r("IVA"))
            totaleimponibileNetto += totaleriga

            l.LotNo = r("LotNo")
            l.Farmadati = GetItemFarmadati(r("ItemCode"))
            l.Descrizione = r("DESCRIZIONE")
            l.Formato = IIf(r("FORMATO") = "Non definito", "-", r("FORMATO"))
            l.IVA = r("IVA")
            l.TotaleRiga = String.Format("€ {0:F2}", totaleriga)
            h.Add(l)
        Next

        h.orderLines.Sort(New orderLinesReportComparer(linesOrderByField))

        totaleiva = totaleiva + (speseditrasporto / 100 * 22) + (speseincasso / 100 * 22)
        '20200222 - Modifica per calcolo sconto su riga + sconto pagamento 
        'commento riga seguente
        'scontoPagamento = totaleimponibileNetto / 100 * scontoPagamentoPerc

        totaleiva = Math.Round((totaleiva), 2)
        totaleimponibileNetto = Math.Round(totaleimponibileNetto, 2)
        speseditrasporto = Math.Round(speseditrasporto, 2)
        speseincasso = Math.Round(speseincasso, 2)

        h.totaleIMPONIBILE = String.Format("€ {0:F2}", totaleimponibileLordo)
        h.totaleIMPONIBILEnetto = String.Format("€ {0:F2}", totaleimponibileNetto)
        h.totaleIVA = String.Format("€ {0:F2}", totaleiva)
        h.scontoPagamentoPerc = String.Format("-{0:F2}%", scontoPagamentoPerc)
        h.scontoTotale = String.Format("-€ {0:F2}", scontoPagamentoTotale)
        h.speseTrasporto = String.Format("€ {0:F2}", speseditrasporto)
        h.speseIncasso = String.Format("€ {0:F2}", speseincasso)
        h.totaleOrdine = String.Format("€ {0:F2}", (totaleiva + totaleimponibileNetto + speseditrasporto + speseincasso - valoreVoucher))
        h.valoreVoucher = String.Format("-€ {0:F2}", valoreVoucher)

        'qta mancanti
        Dim dtQtaManancti As DataTable = GetQuantitaNonDisponibili(ordercode).Tables(0)
        For Each r As DataRow In dtQtaManancti.Rows
            Dim qnd As New quantitaNonDisponibileReport()
            qnd.itemCode = r("ItemCode")
            qnd.quantity = r("Quantity")
            qnd.Descrizione = GetItemDescription(r("ItemCode"))
            qnd.idCarrello = r("idCarrello")
            qnd.orderCode = r("orderCode")
            h.Add(qnd)
        Next
        h.quantitaNonDisponibiliCount = dtQtaManancti.Rows.Count

        If Not HttpContext.Current Is Nothing Then
            h.reportLogo = "~/images/custom/" & CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).reportLogo
            h.reportFooterDescription = CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).reportFooterDescription
        Else
            h.reportLogo = reportLogo
            h.reportFooterDescription = reportFooterDescription
        End If

        odr.Add(h)

        Return odr

    End Function

    Private Function GetQuantitaNonDisponibili(ByVal orderCode As String) As DataSet
        Dim conn As SqlConnection = Nothing
        Dim ds As New DataSet()
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim sql As String = " Select * FROM [QuantitaNonDisponibile] WHERE [CompanyName]='" & workingCompany & "' AND [orderCode]=@orderCode"
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

    Private Function GetItemDescription(ByVal CodiceArticolo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select isNULL(Descrizione,'') FROM [Items_Nav] WHERE [CodiceArticolo]=@CodiceArticolo AND CompanyCode=@CompanyCode", conn)
            cmd.Parameters.Add("@CodiceArticolo", SqlDbType.VarChar, 20).Value = CodiceArticolo
            cmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 30).Value = workingCompany
            Dim res As Object = cmd.ExecuteScalar
            If IsDBNull(res) Or res = Nothing Then
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

    Private Function GetItemFarmadati(ByVal CodiceArticolo As String) As String
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(WORconnectionString)
            conn.Open()
            Dim cmd As New SqlCommand("Select isNULL(Farmadati,'') FROM [Items_Nav] WHERE [CodiceArticolo]=@CodiceArticolo AND CompanyCode=@CompanyCode", conn)
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

    Public Function getOrder(ByVal ordercode As String, ByVal NAVconnectionString As String, ByVal WORconnectionString As String, ByVal workingCompany As String, ByVal showDataUltimaModifica As Boolean, Optional ByVal linesOrderByField As String = "LineID", Optional ByVal reportLogo As String = "", Optional ByVal reportFooterDescription As String = "") As orderHeaderReportCollection

        _NAVconnectionString = NAVconnectionString
        _WORconnectionString = WORconnectionString
        _workingCompany = workingCompany
        _showDataUltimaModifica = showDataUltimaModifica

        Return getOrder(ordercode, linesOrderByField, reportLogo, reportFooterDescription)

    End Function



End Class

