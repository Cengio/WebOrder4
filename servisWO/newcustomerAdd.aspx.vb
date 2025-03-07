Imports servisWO.datamanager
Imports System.Net.Mail
Imports DevExpress.Web
Imports log4net
Imports System.Reflection

Public Class newcustomerAdd
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub newcustomerAdd_Init(sender As Object, e As EventArgs) Handles Me.Init
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
            Call loadselectionData()
            If Not Session("selectedNewCustomer") Is Nothing AndAlso Session("selectedNewCustomer") <> "" Then
                btn_NuovoOrdine.Enabled = True
                Call loadNewCustomerData()
            Else
                Dim dm As New DataManager
                lb_codiceNewCustomer.Text = dm.GetNewCustomerProgressive
                dm = Nothing
                btn_NuovoOrdine.Enabled = False
                lb_username.Text = CType(Session("user"), user).nomeCompleto
            End If
        End If
    End Sub

    Private Sub loadselectionData()
        Dim dm As New DataManager

        combo_catClienti.DataSource = dm.GetBusinessRelation
        combo_catClienti.ValueField = "Code"
        combo_catClienti.TextField = "Description"
        combo_catClienti.DataBind()

        Dim natgiur As New naturaGiuridica
        combo_naturaGiuridica.DataSource = natgiur.getList
        combo_naturaGiuridica.ValueField = "Code"
        combo_naturaGiuridica.TextField = "Description"
        combo_naturaGiuridica.DataBind()
        natgiur = Nothing

        'combo_Nazione.DataSource = dm.GetCountry
        'combo_Nazione.ValueField = "Code"
        'combo_Nazione.TextField = "Description"
        'combo_Nazione.DataBind()
        'combo_Nazione.SelectedIndex = combo_Nazione.Items.IndexOfValue("IT")

        Dim natgiurDest As New naturaGiuridica
        combo_naturagiuridicaDest.DataSource = natgiurDest.getList
        combo_naturagiuridicaDest.ValueField = "Code"
        combo_naturagiuridicaDest.TextField = "Description"
        combo_naturagiuridicaDest.DataBind()
        natgiurDest = Nothing

        dm = Nothing
    End Sub

    Private Sub loadNewCustomerData()
        Dim dm As New DataManager
        Dim dt As DataTable = dm.GetNewCustomers(Session("selectedNewCustomer"))
        If dt.Rows.Count > 0 Then
            Dim newc As DataRow = dt.Rows(0)
            lb_codiceNewCustomer.Text = newc("CustomerNo")
            If newc("CreatedBy").ToString.Split(";").Count = 2 Then
                lb_username.Text = newc("CreatedBy").ToString.Split(";")(1)
            Else
                lb_username.Text = CType(Session("user"), user).nomeCompleto
            End If
            If newc("Category") <> "" Then
                combo_catClienti.SelectedIndex = combo_catClienti.Items.IndexOfValue(newc("Category"))
            End If
            tb_name.Text = newc("Name")
            tb_piva.Text = newc("VATNumber")
            tb_codfiscale.Text = newc("FiscalCode")
            tb_indirizzo.Text = newc("Address")
            tb_numeroCivico.Text = newc("AddressNo")
            tb_CAP.Text = newc("PostCode")
            If newc("City") <> "" Then
                lookupCitta.Text = newc("City")
            End If
            tb_provincia.Text = newc("County")
            tb_telefono1.Text = newc("Phone1")
            tb_telefono2.Text = newc("Phone2")
            tb_fax.Text = newc("FaxNo")
            tb_email.Text = newc("Email")
            tb_nazione.Text = newc("CountryCode")
            'tb_naz.Text = newc("CountryCode")
            tb_CD.Text = newc("CD")
            tb_cin.Text = newc("CIN")
            tb_abi.Text = newc("ABI")
            tb_cab.Text = newc("CAB")
            tb_contocorrente.Text = newc("CC")
            tb_name2.Text = newc("Name2")
            tb_indirizzo2.Text = newc("Address2")
            'newc("StartingDate")
            'newc("CreatedBy")
            If newc("naturagiuridica") <> "" Then
                combo_naturaGiuridica.SelectedIndex = combo_naturaGiuridica.Items.IndexOfValue(newc("naturagiuridica"))
            End If
            tb_nomeDest1.Text = newc("ragsocsped1")
            tb_nomeDest2.Text = newc("ragsocsped2")
            If newc("naturagiuridicasped") <> "" Then
                combo_naturagiuridicaDest.SelectedIndex = combo_naturagiuridicaDest.Items.IndexOfValue(newc("naturagiuridicasped"))
            End If
            tb_indirizzoDest.Text = newc("indirizzosped1")
            tb_indirizzoDest2.Text = newc("indirizzosped2")
            tb_capDest.Text = newc("capsped")
            If newc("localitasped") <> "" Then
                lookupCittaDest.Text = newc("localitasped")
            End If
            tb_provinciaDest.Text = newc("provinciasped")
            tb_nazioneDest.Text = newc("paesesped")

            tb_nazioneDest.Text = newc("paesesped")
            tb_telefonoDest1.Text = newc("telefono1sped")
            tb_telefonoDest2.Text = newc("telefono2sped")
            tb_faxDest.Text = newc("faxsped")
            tb_emailDest.Text = newc("emailsped")

            If newc("Category") <> "" Then
                grigliaPagamenti.DataSource = dm.getNewCustomerPagamenti(newc("Category"))
                grigliaPagamenti.DataBind()
            End If

            If newc("PaymentMethod") <> "" AndAlso newc("PaymentTerms") <> "" Then
                'combo_ModalitaPAgamento.SelectedIndex = combo_ModalitaPAgamento.Items.IndexOfValue(newc("PaymentMethod"))
            End If
            'If newc("PaymentTerms") <> "" Then
            '    combo_TerminiPagamento.SelectedIndex = combo_TerminiPagamento.Items.IndexOfValue(newc("PaymentTerms"))
            'End If
        End If
        dm = Nothing
    End Sub

    Private Sub inviaEmailNotifica(ByVal newc As newcustomer, ByVal modificaNuovoCliente As Boolean)
        Try
            Dim dm As New datamanager
            Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
            Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
            Dim attivaNotificaNuovoCliente As String = dm.GetParametroSito(parametriSitoValue.attivaNotificaNuovoCliente)

            If attivaNotificheEmail = "1" AndAlso attivaNotificaNuovoCliente = "1" AndAlso emailSistema <> "" Then
                Dim emailNotificaNuovoCliente As String = dm.GetParametroSito(parametriSitoValue.emailNotificaNuovoCliente)
                Dim emailNotificaNuovoCliente2 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaNuovoCliente2)
                Dim emailNotificaNuovoCliente3 As String = dm.GetParametroSito(parametriSitoValue.emailNotificaNuovoCliente3)
                Dim emailSistemaDesc As String = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)
                If emailSistemaDesc = "" Then emailSistemaDesc = emailSistema

                If emailNotificaNuovoCliente <> "" Then
                    Dim msg As MailMessage = New MailMessage()
                    msg.IsBodyHtml = True
                    msg.From = New MailAddress(emailSistema, emailSistemaDesc)
                    msg.To.Add(emailNotificaNuovoCliente)
                    If emailNotificaNuovoCliente2 <> "" Then
                        msg.CC.Add(emailNotificaNuovoCliente2)
                    End If
                    If emailNotificaNuovoCliente3 <> "" Then
                        msg.CC.Add(emailNotificaNuovoCliente3)
                    End If
                    If CType(Session("user"), user).email <> "" Then
                        msg.CC.Add(CType(Session("user"), user).email)
                    End If
                    If modificaNuovoCliente Then
                        msg.Subject = "Modifica Nuovo Cliente web nr. " & newc.CustomerNo & " - " & newc.Name
                    Else
                        msg.Subject = "Inserimento Nuovo Cliente web nr. " & newc.CustomerNo & " - " & newc.Name
                    End If
                    msg.Body = "Il" & Date.Now & " l'utente " & newc.CreatedBy & " ha inserito il seguente nuovo cliente:" & vbCrLf & vbCrLf & "<br>"
                    msg.Body &= "Codice cliente: " & newc.CustomerNo & vbCrLf & "<br>"
                    msg.Body &= "Categoria: " & newc.Category & vbCrLf & vbCrLf & "<br>" & "<br>"
                    msg.Body &= "Dati Fatturazione" & vbCrLf & "<br>"
                    msg.Body &= "Ragione sociale: " & newc.Name & " " & newc.Name2 & vbCrLf & "<br>"
                    msg.Body &= "Natura giuridica: " & newc.naturagiuridica & vbCrLf & "<br>"
                    msg.Body &= "Partita IVA: " & newc.VATNumber & vbCrLf & "<br>"
                    msg.Body &= "Codice Fiscale: " & newc.FiscalCode & vbCrLf & "<br>"
                    msg.Body &= "Indirizzo: " & newc.Address & " " & newc.Address2 & " " & newc.AddressNo & vbCrLf & "<br>"
                    msg.Body &= "CAP: " & newc.PostCode & vbCrLf & "<br>"
                    msg.Body &= "Citt&agrave;: " & newc.City & vbCrLf & "<br>"
                    msg.Body &= "Paese: " & newc.CountryCode & vbCrLf & "<br>"
                    msg.Body &= "Provincia: " & newc.County & vbCrLf & "<br>"
                    msg.Body &= "Telefono: " & newc.Phone1 & vbCrLf & "<br>"
                    msg.Body &= "Cellulare: " & newc.Phone2 & vbCrLf & "<br>"
                    msg.Body &= "Fax: " & newc.FaxNo & vbCrLf & "<br>"
                    msg.Body &= "Email: " & newc.Email & vbCrLf & vbCrLf & "<br>" & "<br>"
                    msg.Body &= "Dati Spedizione" & vbCrLf & "<br>"
                    msg.Body &= "Ragione sociale: " & newc.ragsocsped1 & " " & newc.ragsocsped2 & vbCrLf & "<br>"
                    msg.Body &= "Natura giuridica: " & newc.naturagiuridicasped & vbCrLf & "<br>"
                    msg.Body &= "Indirizzo: " & newc.indirizzosped1 & " " & newc.indirizzosped2 & vbCrLf & "<br>"
                    msg.Body &= "Indirizzo: " & newc.capsped & " " & newc.localitasped & " " & newc.provinciasped & " " & newc.paesesped & vbCrLf & "<br>"
                    msg.Body &= "Telefono: " & newc.telefono1sped & vbCrLf & "<br>"
                    msg.Body &= "Cellulare Destinazione: " & newc.telefono2sped & vbCrLf & "<br>"
                    msg.Body &= "Fax: " & newc.faxsped & vbCrLf & "<br>"
                    msg.Body &= "Email: " & newc.emailsped & vbCrLf & vbCrLf & "<br>" & "<br>"
                    msg.Body &= "Dati Bancari" & vbCrLf & "<br>"
                    msg.Body &= "CD: " & newc.CD & vbCrLf & "<br>"
                    msg.Body &= "CIN: " & newc.CIN & vbCrLf & "<br>"
                    msg.Body &= "ABI: " & newc.ABI & vbCrLf & "<br>"
                    msg.Body &= "CAB: " & newc.CAB & vbCrLf & "<br>"
                    msg.Body &= "CC: " & newc.CC & vbCrLf & "<br>"
                    msg.Body &= "Medotodo di pagamento: " & newc.PaymentMethod & vbCrLf & "<br>"
                    msg.Body &= "Condizioni di pagamento: " & newc.PaymentTerms & vbCrLf & vbCrLf & "<br>" & "<br>"
                    msg.Body &= "Starting date: " & newc.StartingDate & vbCrLf & "<br>"

                    Dim client As SmtpClient = New SmtpClient
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
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Protected Sub btn_NuovoOrdine_Click(sender As Object, e As EventArgs) Handles btn_NuovoOrdine.Click
        CType(Session("cart"), cartManager).clearCart()
        CType(Session("cart"), cartManager).Header.CODICECLIENTE = Session("selectedNewCustomer")
        CType(Session("cart"), cartManager).Header.CODICECONTATTO = ""
        Response.Redirect("~/orderNew.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub


    'Protected Sub grigliaPagamenti_DataBinding(sender As Object, e As EventArgs) Handles grigliaPagamenti.DataBinding
    '    Dim dm As New DataManager
    '    grigliaPagamenti.DataSource = dm.getNewCustomerPagamenti()
    '    dm = Nothing
    'End Sub

    Protected Sub grigliaPagamenti_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles grigliaPagamenti.CustomCallback
        If Not e.Parameters Is Nothing AndAlso e.Parameters <> "" Then
            Dim dm As New DataManager
            grigliaPagamenti.DataSource = dm.getNewCustomerPagamenti(e.Parameters)
            grigliaPagamenti.DataBind()
            'grigliaPagamenti.Selection.CancelSelection()
            dm = Nothing
        End If
    End Sub

    Protected Sub grigliaPagamenti_CommandButtonInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewCommandButtonEventArgs) Handles grigliaPagamenti.CommandButtonInitialize
        If Session("selectedNewCustomer") <> "" Then
            Dim grid As ASPxGridView = CType(sender, ASPxGridView)
            If (e.ButtonType = ColumnCommandButtonType.SelectCheckbox) Then
                If grid.Selection.Count = 0 Then
                    Dim dm As New DataManager
                    Dim dt As DataTable = dm.GetNewCustomers(Session("selectedNewCustomer"))
                    If dt.Rows.Count > 0 Then
                        Dim newc As DataRow = dt.Rows(0)
                        If grid.GetRowValues(e.VisibleIndex, "Metodo") = newc("PaymentMethod") And grid.GetRowValues(e.VisibleIndex, "Termine") = newc("PaymentTerms") Then
                            grid.Selection.SelectRow(e.VisibleIndex)
                        End If
                    End If
                    dm = Nothing
                End If
            End If
        End If
    End Sub

    Protected Sub ASPxCallback_saveCustomer_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles ASPxCallback_saveCustomer.Callback
        'controlli su compilazione
        Dim ctrlPagamentoRiba As Boolean = False
        Dim ctrlPagamentoSelezionato As Boolean = False
        Dim pagamentoSelezionatoList As List(Of Object) = grigliaPagamenti.GetSelectedFieldValues(New String() {"Metodo", "Termine", "Sconto"})
        If pagamentoSelezionatoList.Count > 0 AndAlso pagamentoSelezionatoList(0)(0) = "RB" Then
            If (tb_naz.Text <> "" And tb_CD.Text <> "" And tb_cin.Text.Trim <> "" And tb_abi.Text <> "" And tb_cab.Text <> "" And tb_contocorrente.Text <> "") Then
                ctrlPagamentoRiba = True
            Else
                ctrlPagamentoRiba = False
            End If
        Else
            ctrlPagamentoRiba = True
        End If

        If grigliaPagamenti.VisibleRowCount > 0 AndAlso pagamentoSelezionatoList.Count = 0 Then
            ctrlPagamentoSelezionato = False
        Else
            ctrlPagamentoSelezionato = True
        End If

        'CTRL : categoria cliente, Ragione sociale, forma societaria, partita IVA, codice fiscale, indirizzo, civico, città (che a cascata compila cap, prov, regione, stato) e almeno uno fra telefono e cellulare
        If Not combo_catClienti.SelectedItem Is Nothing Then
            If tb_name.Text <> "" Then
                If Not combo_naturaGiuridica.SelectedItem Is Nothing Then
                    If tb_piva.Text <> "" Then
                        If tb_codfiscale.Text <> "" Then
                            If tb_indirizzo.Text <> "" Then
                                If tb_numeroCivico.Text <> "" Then
                                    If lookupCitta.Text <> "" Then
                                        If tb_telefono1.Text <> "" Or tb_telefono2.Text <> "" Then
                                            If ctrlPagamentoSelezionato Then
                                                If ctrlPagamentoRiba Then
                                                    Dim newc As New newcustomer
                                                    newc.CustomerNo = lb_codiceNewCustomer.Text.Trim.ToUpper
                                                    If Not combo_catClienti.SelectedItem.Value Is Nothing Then
                                                        newc.Category = combo_catClienti.SelectedItem.Value
                                                    Else
                                                        newc.Category = ""
                                                    End If
                                                    newc.Name = tb_name.Text.Trim.ToUpper
                                                    newc.VATNumber = tb_piva.Text.Trim.ToUpper
                                                    newc.FiscalCode = tb_codfiscale.Text.Trim.ToUpper
                                                    newc.Address = tb_indirizzo.Text.Trim.ToUpper ' & " " & tb_numeroCivico.Text.Trim.ToUpper
                                                    newc.AddressNo = tb_numeroCivico.Text.Trim.ToUpper
                                                    newc.PostCode = tb_CAP.Text.Trim.ToUpper
                                                    newc.City = lookupCitta.Text.Trim.ToUpper
                                                    newc.County = tb_provincia.Text.Trim.ToUpper
                                                    newc.Phone1 = tb_telefono1.Text.Trim.ToUpper
                                                    newc.Phone2 = tb_telefono2.Text.Trim.ToUpper
                                                    newc.FaxNo = tb_fax.Text.Trim.ToUpper
                                                    newc.Email = tb_email.Text.Trim
                                                    newc.CountryCode = tb_nazione.Text.Trim.ToUpper
                                                    newc.CD = tb_CD.Text.Trim.ToUpper
                                                    newc.CIN = tb_cin.Text.Trim.ToUpper
                                                    newc.ABI = tb_abi.Text.Trim.ToUpper
                                                    newc.CAB = tb_cab.Text.Trim.ToUpper
                                                    newc.CC = tb_contocorrente.Text.Trim.ToUpper
                                                    newc.Imported = 0
                                                    newc.Name2 = tb_name2.Text.Trim.ToUpper
                                                    newc.Address2 = tb_indirizzo2.Text.Trim.ToUpper
                                                    newc.StartingDate = Now().ToString("dd/MM/yyyy")
                                                    newc.JustContact = 0
                                                    newc.CreatedBy = CType(Session("user"), user).userCode & ";" & CType(Session("user"), user).nomeCompleto
                                                    If Not combo_naturaGiuridica.SelectedItem Is Nothing Then
                                                        newc.naturagiuridica = combo_naturaGiuridica.SelectedItem.Value
                                                    Else
                                                        newc.naturagiuridica = ""
                                                    End If
                                                    newc.ragsocsped1 = tb_nomeDest1.Text.Trim.ToUpper
                                                    newc.ragsocsped2 = tb_nomeDest2.Text.Trim.ToUpper
                                                    If Not combo_naturagiuridicaDest.SelectedItem Is Nothing Then
                                                        newc.naturagiuridicasped = combo_naturagiuridicaDest.SelectedItem.Value
                                                    Else
                                                        newc.naturagiuridicasped = ""
                                                    End If
                                                    newc.indirizzosped1 = tb_indirizzoDest.Text.Trim.ToUpper
                                                    newc.indirizzosped2 = tb_indirizzoDest2.Text.Trim.ToUpper
                                                    newc.capsped = tb_capDest.Text.Trim.ToUpper
                                                    newc.localitasped = lookupCittaDest.Text.Trim.ToUpper
                                                    newc.provinciasped = tb_provinciaDest.Text.Trim.ToUpper
                                                    newc.paesesped = tb_nazioneDest.Text.Trim.ToUpper
                                                    newc.telefono1sped = tb_telefonoDest1.Text.Trim.ToUpper
                                                    newc.telefono2sped = tb_telefonoDest2.Text.Trim.ToUpper
                                                    newc.faxsped = tb_faxDest.Text.Trim.ToUpper
                                                    newc.emailsped = tb_emailDest.Text.Trim.ToUpper
                                                    If pagamentoSelezionatoList.Count > 0 Then
                                                        newc.PaymentMethod = pagamentoSelezionatoList(0)(0)
                                                        newc.PaymentTerms = pagamentoSelezionatoList(0)(1)
                                                    Else
                                                        newc.PaymentMethod = ""
                                                        newc.PaymentTerms = ""
                                                    End If

                                                    Dim res As Integer = 0
                                                    Dim dm As New datamanager
                                                    Dim modificaNuovoCliente As Boolean = (Not Session("selectedNewCustomer") Is Nothing AndAlso Session("selectedNewCustomer") <> "")
                                                    If modificaNuovoCliente Then
                                                        res = dm.updNewCustomer(newc)
                                                    Else
                                                        res = dm.addNewCustomer(newc)
                                                    End If

                                                    If res > 0 Then
                                                        inviaEmailNotifica(newc, modificaNuovoCliente)
                                                        Session("selectedNewCustomer") = newc.CustomerNo
                                                        e.Result = String.Format("{0}|{1}", "Esito positivo", "Salvataggio nuovo cliente riuscito.")
                                                        DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/newcustomerAdd.aspx")

                                                    Else
                                                        e.Result = String.Format("{0}|{1}", "Esito negativo", "Salvataggio del cliente non riuscito. Riprovare tra qualche istante.")
                                                    End If

                                                    dm = Nothing

                                                Else
                                                    e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare i campi relativi all'IBAN.")
                                                    tb_naz.Focus()
                                                End If
                                            Else
                                                e.Result = String.Format("{0}|{1}", "Attenzione", "Selezionare un metodo di pagamento.")
                                            End If
                                        Else
                                            e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare almeno uno tra telefono e cellulare.")
                                            tb_telefono1.Focus()
                                        End If
                                    Else
                                        e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare la città.")
                                        lookupCitta.Focus()
                                    End If
                                Else
                                    e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare il numero civico.")
                                    tb_numeroCivico.Focus()
                                End If
                            Else
                                e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare l'indirizzo.")
                                tb_indirizzo.Focus()
                            End If
                        Else
                            e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare il codice fiscale.")
                            tb_codfiscale.Focus()
                        End If
                    Else
                        e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare la partita iva.")
                        tb_piva.Focus()
                    End If
                Else
                    e.Result = String.Format("{0}|{1}", "Attenzione", "Scegliere la natura giuridica.")
                    combo_naturaGiuridica.Focus()
                End If
            Else
                e.Result = String.Format("{0}|{1}", "Attenzione", "Compilare la ragione sociale del cliente.")
                tb_name.Focus()
            End If
        Else
            e.Result = String.Format("{0}|{1}", "Attenzione", "Scegliere la categoria cliente.")
            combo_catClienti.Focus()
        End If

    End Sub


End Class