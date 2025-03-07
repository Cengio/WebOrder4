Imports System.Reflection
Imports DevExpress.Web
Imports log4net
Imports System.Linq

Public Class orderImport
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub orderImport_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSordini Then    'utente amministratore
        Else
            Try
                Session.Abandon()
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            grid.DataBind()
        End If
    End Sub

    Protected Sub grid_DataBinding(sender As Object, e As EventArgs) Handles grid.DataBinding
        Dim dm As New datamanager
        grid.DataSource = dm.GetOrdersToImportList()
        dm = Nothing
        grid.JSProperties("cpOrderImport") = ""
        grid.JSProperties("cpClienteNonTrovato") = ""
    End Sub

    Protected Sub grid_CustomButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonEventArgs) Handles grid.CustomButtonInitialize
        If e.VisibleIndex = -1 Then
            Return
        End If
        Try
            If e.CellType = GridViewTableCommandCellType.Data Then
                Dim OrdineAmazon As String = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "OrdineAmazon").ToString
                Dim dm As New datamanager
                If e.ButtonID = "btnImport" Then
                    Dim Importato As Integer = CType(sender, ASPxGridView).GetRowValues(e.VisibleIndex, "Importato")
                    If Importato <> 0 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine già in fase di importazione"
                    ElseIf CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                        e.Enabled = False
                        e.Image.ToolTip = "Ordine in corso. Chiudere l'attività prima di importare l'ordine."
                    End If
                End If
                dm = Nothing
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub grid_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles grid.CustomButtonCallback
        If e.VisibleIndex = -1 Then
            Return
        End If
        Dim dm As New datamanager
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim OrdineAmazon As String = grid.GetRowValues(e.VisibleIndex, "OrdineAmazon").ToString
        Dim DataIns As DateTime = grid.GetRowValues(e.VisibleIndex, "DataIns")
        Session("infoCustomerNo") = ""
        CType(Session("cart"), cartManager).clearCart()
        Dim ordineImportabile As Boolean = dm.isImportabile(OrdineAmazon)       ' TEST su IMPORTATO 

        If (ordineImportabile) Then
            Log.Info(CType(Session("user"), user).nomeCompleto & " starting import ordine " & OrdineAmazon)
            'HEADER
            'Dim codicecliente As String = "C27866"      'Codice cliente AMAZON = C27866 - Codice cliente di test locale = C25052
            Dim CodiceCliente As String = dm.GetOrdersToImportList().Tables(0).AsEnumerable().Where(Function(Row) Row.Field(Of String)("OrdineAmazon").ToLower() = OrdineAmazon.ToLower()).FirstOrDefault().Item("CodiceCliente")
            Dim codicecontatto As String = dm.GetContactCodeByCustomerCode(codicecliente)
            If codicecontatto <> "" Then
                dm.SetOrderImportStatus(OrdineAmazon, 1)

                Dim carrello As cartManager = CType(Session("cart"), cartManager)
                carrello.Header.CODICECLIENTE = codicecliente
                carrello.Header.CODICECONTATTO = codicecontatto
                carrello.Header.ORIGINATODAORDINE = OrdineAmazon
                carrello.Header.DESCRIPTION = CType(Session("cart"), cartManager).Header.CODICECLIENTE & "-" & Now.ToString
                carrello.Header.UTENTE_CREAZIONE = CType(Session("user"), user).userCode
                carrello.Header.UTENTE_ULTIMA_MODIFICA = CType(Session("user"), user).userCode
                carrello.Header.SCONTOPAGAMENTO = 0
                carrello.Header.AGENTE = 0
                carrello.Header.IMPORTOSPESESPEDIZIONE = dm.GetSpeseTrasporto
                If carrello.Header.CODICECONTATTO <> "" Then
                    carrello.Header.IMPORTOEVADIBILITAORDINE = dm.getparametriEvadibilitaOrdineByContactCode(carrello.Header.CODICECONTATTO, datamanager.parametriSitoValue.importoEvadibilitaOrdine)
                    carrello.Header.IMPORTOPORTOFRANCO = dm.getparametriEvadibilitaOrdineByContactCode(carrello.Header.CODICECONTATTO, datamanager.parametriSitoValue.importoPortoFranco)
                Else
                    carrello.Header.IMPORTOEVADIBILITAORDINE = dm.GetParametroSito(datamanager.parametriSitoValue.importoEvadibilitaOrdine)
                    carrello.Header.IMPORTOPORTOFRANCO = dm.GetParametroSito(datamanager.parametriSitoValue.importoPortoFranco)
                End If
                carrello.Header.SCONTOHEADER = 0
                carrello.Header.IDPROFILOSCONTO = 0
                carrello.Header.STATUSPRODUZIONE = 0
                carrello.Header.STATUSMAGAZZINO = 0

                Dim idCarrello As Int64 = dm.salvaCarrello2(carrello, True, True)
                Session("cart") = CType(Session("cart"), cartManager).loadFromDBbyIdCarrello(idCarrello, False)
                Log.Info(CType(Session("user"), user).nomeCompleto & " created cart " & idCarrello.ToString & " for the customer " & CType(Session("cart"), cartManager).Header.CODICECLIENTE)

                'la data ordine è la data di creazione carrello
                Dim dataordine As DateTime = CType(Session("cart"), cartManager).Header.DATA_CREAZIONE
                Dim strday As String = IIf(dataordine.Date.Day.ToString.Length = 2, dataordine.Date.Day.ToString, "0" & dataordine.Date.Day.ToString)
                Dim strmonth As String = IIf(dataordine.Date.Month.ToString.Length = 2, dataordine.Date.Month.ToString, "0" & dataordine.Date.Month.ToString)
                Dim stryear As String = dataordine.Date.Year.ToString
                carrello.Header.ordineHeader.OrderDate = String.Format("{0}/{1}/{2}", strday, strmonth, stryear)
                carrello.Header.ordineHeader.Type = "Ordine via Email"
                carrello.Header.ordineHeader.Notes = ""
                carrello.Header.ordineHeader.ShippingAgentCode = ""
                carrello.Header.ordineHeader.IncludeShipCost = 0
                carrello.Header.ordineHeader.ShipAddressCode = ""
                carrello.Header.ordineHeader.PaymentMethodCode = "RD"
                carrello.Header.ordineHeader.PaymentTermsCode = "60FM"
                carrello.Header.ordineHeader.Status = 0
                Session("cart") = dm.addOrderFromCarrello(CType(Session("cart"), cartManager))
                Log.Info(CType(Session("user"), user).nomeCompleto & " created order header " & CType(Session("cart"), cartManager).Header.ordineHeader.Code & " for the customer " & CType(Session("cart"), cartManager).Header.CODICECLIENTE)

                'ROWS
                CType(Session("cart"), cartManager).loadListino()
                Dim dtlines As DataTable = dm.GetOrderLinesToImport(OrdineAmazon)
                Log.Info(CType(Session("user"), user).nomeCompleto & " Starting import " & dtlines.Rows.Count.ToString & " lines from ordine " & OrdineAmazon)
                Dim nrLinesImported As Integer = 0
                Dim nrLinesMananti As Integer = 0
                For Each r As DataRow In dtlines.Rows

                    Dim dispo As Integer = dm.GetDisponibilitaProdotti(r("CodiceArticolo"), True).Rows(0).Item("Disponibilita")
                    Log.Info("Import item: " & r("CodiceArticolo") & " Qta: " & r("Qta") & " (dispo=" & dispo.ToString & ")" & " from order " & OrdineAmazon)
                    Dim qtaToAdd As Integer = r("Qta")
                    Dim qtaMancante As Integer = 0
                    If dispo < qtaToAdd Then
                        If dispo >= 0 Then
                            qtaMancante = qtaToAdd - dispo
                        Else
                            qtaMancante = qtaToAdd
                        End If
                        qtaToAdd = dispo
                    End If
                    If qtaToAdd > 0 Then
                        CType(Session("cart"), cartManager).addLine(r("CodiceArticolo"), qtaToAdd, "", False, False, 0, False, "", 0, "", False, False)
                        nrLinesImported += 1
                    End If
                    If qtaMancante > 0 Then
                        Dim qnd As New quantitaNonDisponibile
                        qnd.itemCode = r("CodiceArticolo")
                        qnd.orderCode = CType(Session("cart"), cartManager).Header.ordineHeader.Code
                        qnd.customerNo = CType(Session("cart"), cartManager).Header.CODICECLIENTE
                        qnd.quantity = qtaMancante
                        qnd.idCarrello = CType(Session("cart"), cartManager).Header.IDCART
                        qnd.alert = 0
                        dm.addQuantitaNonDisponibile(qnd)
                        nrLinesMananti += 1
                    End If
                Next
                dm.salvaCarrello2(CType(Session("cart"), cartManager))
                Log.Info("End import lines from ordine " & OrdineAmazon & " Imported: " & nrLinesImported.ToString + " Mancanti: " & nrLinesMananti.ToString)

                dm.SetOrderImportStatus(OrdineAmazon, 2)
                dm = Nothing

                Log.Info("End import ordine esterno " & OrdineAmazon & " in wo order " & CType(Session("cart"), cartManager).Header.ordineHeader.Code)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("orderDetails.aspx")
                grid.JSProperties("cpOrderImport") = ""
                grid.JSProperties("cpClienteNonTrovato") = ""
            Else
                Log.Info("Import ordine " & OrdineAmazon & " fallito: impossibile trovare in NAV il CT per il cliente " & codicecliente)
                grid.JSProperties("cpClienteNonTrovato") = codicecliente
            End If
        Else
            Log.Info("Ordine " & OrdineAmazon & " non importabile, in quanto già elaborato")
            grid.JSProperties("cpOrderImport") = OrdineAmazon
        End If

    End Sub




End Class