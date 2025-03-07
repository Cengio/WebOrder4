Imports System.Net.Mail
Imports DevExpress.Web
Imports servisWO.datamanager
Imports log4net
Imports System.Reflection

Public Class proConfirmPrenotazioni
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                gridOrders.DataBind()
            End If
        Catch ex As Exception
            Response.Redirect("~/err/err403.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End Try
    End Sub

    Protected Sub gridOrders_DataBinding(sender As Object, e As EventArgs) Handles gridOrders.DataBinding
        Dim dm As New datamanager
        gridOrders.DataSource = dm.getOrdiniPrenotazioneDaProdurre()
        dm = Nothing
    End Sub

    Protected Sub gridOrders_CustomButtonCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs) Handles gridOrders.CustomButtonCallback
        If e.VisibleIndex = -1 Then
            Return
        End If
        Dim grid As ASPxGridView = CType(sender, ASPxGridView)
        Dim Code As String = grid.GetRowValues(e.VisibleIndex, "Code").ToString
        Dim CODICECLIENTE As String = grid.GetRowValues(e.VisibleIndex, "CODICECLIENTE").ToString
        Dim UTENTE_ULTIMA_MODIFICA As String = grid.GetRowValues(e.VisibleIndex, "UTENTE_ULTIMA_MODIFICA").ToString

        If checkOrderDispo(Code) Then
            Dim dm As New datamanager
            If dm.confermaProduzionePrenotati(Code, CType(Session("user"), user).userCode) Then
                Call notificaEmailProduzione(Code, CODICECLIENTE, CType(Session("user"), user).userCode, UTENTE_ULTIMA_MODIFICA)
            End If
            dm = Nothing
            gridOrders.JSProperties("cpshowpopup") = "0"
        Else
            grid.JSProperties("cpshowpopup") = "1"
            grid.JSProperties("cptitolo") = "Attenzione"
            grid.JSProperties("cpcontenuto") = "Non tutte le righe ordine hanno disponibilità sufficiente. Effetture il carico in NAV."
        End If
        gridOrders.DataBind()
    End Sub

    Protected Sub gridItems_Init(sender As Object, e As EventArgs)
        Dim gridItems As ASPxGridView = CType(sender, ASPxGridView)
        Dim container As GridViewDataItemTemplateContainer = TryCast(gridItems.NamingContainer, GridViewDataItemTemplateContainer)
        Dim Code As String = DataBinder.Eval(container.DataItem, "Code")
        Dim dm As New datamanager
        gridItems.DataSource = dm.getArticoliPrenotazioneDaProdurreByOrderCode(Code)
        gridItems.DataBind()
        dm = Nothing
    End Sub

    Protected Sub lbGiacenza_Init(sender As Object, e As EventArgs)
        Dim lbGiacenza As ASPxLabel = CType(sender, ASPxLabel)
        Dim container As GridViewDataItemTemplateContainer = TryCast(lbGiacenza.NamingContainer, GridViewDataItemTemplateContainer)
        Dim ItemCode As String = DataBinder.Eval(container.DataItem, "ItemCode")
        Dim dm As New datamanager
        Dim dt As DataTable = dm.GetDisponibilitaProdotti(ItemCode, True)
        If dt.Rows.Count > 0 Then
            lbGiacenza.Text = dt.Rows(0).Item("Disponibilita").ToString
        Else
            lbGiacenza.Text = "ND"
        End If
        dm = Nothing
    End Sub

    Private Sub notificaEmailProduzione(orderCode As String, CustomerNo As String, userCode As String, UTENTE_ULTIMA_MODIFICA As String)
        Dim dm As New datamanager
        Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
        Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
        Dim emailRichiedente As String = dm.getUser(UTENTE_ULTIMA_MODIFICA).Rows(0).Item("Email")
        If attivaNotificheEmail = "1" AndAlso emailSistema <> "" Then
            Dim emailNotificaOrdini As String = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini)
            Dim emailNotificaOrdini2 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini2)
            Dim emailNotificaOrdini3 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini3)
            Dim emailSistemaDesc As String = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)
            If emailSistemaDesc = "" Then emailSistemaDesc = emailSistema
            Dim customerName As String
            If CustomerNo.StartsWith("N") Then
                customerName = dm.GetNewCustomerName(CustomerNo)
            Else
                customerName = dm.GetCustomerName(CustomerNo)
            End If
            Dim corpo As String = ""
            corpo &= "Conferma della produzione dei seguenti articoli e quantità:" & vbCrLf & vbCrLf
            Dim dt As DataTable = dm.getArticoliDaProdurreByOrderCode(orderCode)
            For Each r As DataRow In dt.Rows
                corpo &= r("ItemCode") & " " & r("DESCRIZIONE") & ":  " & CInt(r("OriginalQty")) & vbCrLf
            Next
            corpo &= vbCrLf & vbCrLf
            corpo &= "Cordiali Saluti" & vbCrLf
            corpo &= dm.getUserNameSurname(CType(Session("user"), user).userCode) & vbCrLf


            Dim client As SmtpClient = New SmtpClient
            Dim msg As MailMessage = New MailMessage()
            msg.IsBodyHtml = False
            msg.From = New MailAddress(emailSistema, emailSistemaDesc)
            msg.Subject = "Conferma produzione Ordine Prenotato nr. " & orderCode & " del cliente " & CustomerNo & " - " & customerName
            msg.Body = corpo

            If emailNotificaOrdini <> "" Then
                msg.To.Add(emailNotificaOrdini)
                If emailNotificaOrdini2 <> "" Then
                    msg.CC.Add(emailNotificaOrdini2)
                End If
                If emailNotificaOrdini3 <> "" Then
                    msg.CC.Add(emailNotificaOrdini3)
                End If
                If emailRichiedente <> "" Then
                    msg.CC.Add(emailRichiedente)
                End If
                Try
                    client.Send(msg)
                    Log.Info("Email " & msg.Subject & " sent to " & msg.To.ToString & " " & msg.CC.ToString)
                Catch ex As Exception
                    Log.Error(ex.ToString)
                    Exit Sub
                End Try
            End If
        End If
        dm = Nothing
    End Sub

    Private Function checkOrderDispo(orderCode As String) As Boolean
        Dim result As Boolean = False
        Try
            Dim orderCart As cartManager = New cartManager()
            Dim dm As New datamanager
            orderCart.loadFromDBbyOrderCode(orderCode)
            If orderCart.controlloGiacenze_PreDichiarazioneProduzione_OrdinePrenotato() Then
                result = True
            End If
            dm = Nothing
        Catch ex As Exception
            Log.Error("checkOrderDispo:" + ex.ToString())
        End Try
        Return result
    End Function


End Class