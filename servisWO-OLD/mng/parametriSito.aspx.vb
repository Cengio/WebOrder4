Imports servisWO.DataManager
Imports System.IO
Imports DevExpress.Web

Public Class parametriSito
    Inherits System.Web.UI.Page
    Private Sub parametriSito_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSwebsiteadmin Then    'utente amministratore
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
            lb_cTitolo.Text &= " (" & CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).code & ")"
            Call loaddata()
            ASPxPageControl1.ActiveTabIndex = 0
        End If
    End Sub

    Public Sub loaddata()
        Dim dm As New DataManager
        comboFascia.SelectedIndex = comboFascia.Items.IndexOfValue(dm.GetParametroSito(parametriSitoValue.fascia))
        Dim dt As DataTable = dm.GetNoWebSeries
        tb_progOrdini.Text = dt.Select("[No_ Type]='orderweb'")(0).Item("Progressive")
        tb_progNewCliente.Text = dt.Select("[No_ Type]='nuovocliente'")(0).Item("Progressive")
        tb_emailNotificaOrdini.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini)
        tb_emailNotificaNuoviClienti.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaNuovoCliente)
        tb_percorsoPDFordini.Text = dm.GetParametroSito(parametriSitoValue.percorsoPDF)

        tb_emailNotificaOrdini2.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini2)
        tb_emailNotificaNuoviClienti2.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaNuovoCliente2)
        tb_emailNotificaOrdini3.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaOrdini3)
        tb_emailNotificaNuoviClienti3.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaNuovoCliente3)

        combo_magazzini.DataSource = dm.getLocation
        combo_magazzini.ValueField = "Code"
        combo_magazzini.TextField = "Descrizione"
        combo_magazzini.DataBind()
        combo_magazzini.SelectedIndex = combo_magazzini.Items.IndexOfValue(dm.GetParametroSito(parametriSitoValue.magazzinodefault))

        comboSASstatus.SelectedIndex = comboSASstatus.Items.IndexOfValue(dm.GetSASstatus)

        spinEvadibilita.Value = dm.GetParametroSito(parametriSitoValue.importoEvadibilitaOrdine)
        spinPortoFranco.Value = dm.GetParametroSito(parametriSitoValue.importoPortoFranco)
        spinSpeseTrasporto.Value = dm.GetParametroSito(parametriSitoValue.importoSpeseSpedizione)

        tb_emailNotificaRevisioneOrdini.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini)
        tb_emailNotificaRevisioneOrdini2.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini2)
        tb_emailNotificaRevisioneOrdini3.Text = dm.GetParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini3)

        tb_emailSistema.Text = dm.GetParametroSito(parametriSitoValue.emailSistema)
        tb_emailSistemaDesc.Text = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)

        cb_attivaNotificheEmail.Checked = IIf(dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail) = "1", True, False)
        cb_attivaNotificaNuovoOrdine.Checked = IIf(dm.GetParametroSito(parametriSitoValue.attivaNotificaNuovoOrdine) = "1", True, False)
        cb_attivaNotificaNuovoCliente.Checked = IIf(dm.GetParametroSito(parametriSitoValue.attivaNotificaNuovoCliente) = "1", True, False)
        cb_attivaNotificaRevisioneOrdine.Checked = IIf(dm.GetParametroSito(parametriSitoValue.attivaNotificaRevisioneOrdine) = "1", True, False)
        cb_attivaNotificaOrdineAlCliente.Checked = IIf(dm.GetParametroSito(parametriSitoValue.attivaNotificaOrdineAlCliente) = "1", True, False)

        tb_emailRichiestaProduzione.Text = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione)
        tb_emailRichiestaProduzione2.Text = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione2)
        tb_emailRichiestaProduzione3.Text = dm.GetParametroSito(parametriSitoValue.emailRichiestaProduzione3)

        'controllo maillinggroup
        Call dm.synchMailingGroupParamatriSitoFromNAV()

        dm = Nothing
    End Sub

    Protected Sub ASPxCallback_save_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles ASPxCallback_save.Callback
        Dim dm As New DataManager

        Dim intTest As Int32
        If tb_progOrdini.Text <> "" AndAlso IsNumeric(tb_progOrdini.Text) AndAlso Int32.TryParse(tb_progOrdini.Text, intTest) Then
            If tb_progNewCliente.Text <> "" AndAlso IsNumeric(tb_progNewCliente.Text) AndAlso Int32.TryParse(tb_progNewCliente.Text, intTest) Then
                If tb_emailNotificaOrdini.Text <> "" AndAlso dm.IsValidEmailFormat(tb_emailNotificaOrdini.Text) Then
                    If tb_emailNotificaNuoviClienti.Text <> "" AndAlso dm.IsValidEmailFormat(tb_emailNotificaNuoviClienti.Text) Then
                        If tb_percorsoPDFordini.Text <> "" Then
                            If tb_emailNotificaRevisioneOrdini.Text <> "" AndAlso dm.IsValidEmailFormat(tb_emailNotificaRevisioneOrdini.Text) Then
                                If tb_emailSistema.Text <> "" AndAlso dm.IsValidEmailFormat(tb_emailSistema.Text) Then
                                    If tb_emailRichiestaProduzione.Text <> "" AndAlso dm.IsValidEmailFormat(tb_emailRichiestaProduzione.Text) Then

                                        'inizio salvataggio parametri
                                        If dm.updParametroSito(parametriSitoValue.fascia, comboFascia.SelectedItem.Value) > 0 Then
                                            If dm.updNoWebSeries("orderweb", tb_progOrdini.Text.Trim) > 0 Then
                                                If dm.updNoWebSeries("nuovocliente", tb_progNewCliente.Text.Trim) > 0 Then
                                                    If dm.updParametroSito(parametriSitoValue.magazzinodefault, combo_magazzini.SelectedItem.Value) > 0 Then
                                                        If dm.updParametroSito(parametriSitoValue.emailNotificaOrdini, tb_emailNotificaOrdini.Text) > 0 Then
                                                            If dm.updParametroSito(parametriSitoValue.emailNotificaNuovoCliente, tb_emailNotificaNuoviClienti.Text) > 0 Then
                                                                If dm.updParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini, tb_emailNotificaRevisioneOrdini.Text) > 0 Then
                                                                    If dm.updParametroSito(parametriSitoValue.emailRichiestaProduzione, tb_emailRichiestaProduzione.Text) > 0 Then
                                                                        If dm.updParametroSito(parametriSitoValue.emailSistema, tb_emailSistema.Text) > 0 Then

                                                                            If dm.updParametroSito(parametriSitoValue.percorsoPDF, tb_percorsoPDFordini.Text) > 0 Then
                                                                                If Not Directory.Exists(Server.MapPath("~/" & tb_percorsoPDFordini.Text.Trim)) Then
                                                                                    Directory.CreateDirectory(Server.MapPath("~/" & tb_percorsoPDFordini.Text.Trim))
                                                                                End If
                                                                                If comboSASstatus.SelectedItem.Value <> dm.GetSASstatus Then
                                                                                    dm.changeSASstatus(comboSASstatus.SelectedItem.Value)
                                                                                End If
                                                                                dm.updParametroSito(parametriSitoValue.emailNotificaOrdini2, tb_emailNotificaOrdini2.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailNotificaOrdini3, tb_emailNotificaOrdini3.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailNotificaNuovoCliente2, tb_emailNotificaNuoviClienti2.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailNotificaNuovoCliente3, tb_emailNotificaNuoviClienti3.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini2, tb_emailNotificaRevisioneOrdini2.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailNotificaRevisioneOrdini3, tb_emailNotificaRevisioneOrdini3.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailRichiestaProduzione2, tb_emailRichiestaProduzione2.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailRichiestaProduzione3, tb_emailRichiestaProduzione3.Text)
                                                                                dm.updParametroSito(parametriSitoValue.emailSistemaDesc, tb_emailSistemaDesc.Text)
                                                                                dm.updParametroSito(parametriSitoValue.attivaNotificheEmail, IIf(cb_attivaNotificheEmail.Checked, "1", "0"))
                                                                                dm.updParametroSito(parametriSitoValue.attivaNotificaNuovoOrdine, IIf(cb_attivaNotificaNuovoOrdine.Checked, "1", "0"))
                                                                                dm.updParametroSito(parametriSitoValue.attivaNotificaNuovoCliente, IIf(cb_attivaNotificaNuovoCliente.Checked, "1", "0"))
                                                                                dm.updParametroSito(parametriSitoValue.attivaNotificaRevisioneOrdine, IIf(cb_attivaNotificaRevisioneOrdine.Checked, "1", "0"))
                                                                                dm.updParametroSito(parametriSitoValue.attivaNotificaOrdineAlCliente, IIf(cb_attivaNotificaOrdineAlCliente.Checked, "1", "0"))
                                                                                dm.updParametroSito(parametriSitoValue.importoEvadibilitaOrdine, spinEvadibilita.Value)
                                                                                dm.updParametroSito(parametriSitoValue.importoPortoFranco, spinPortoFranco.Value)
                                                                                dm.updParametroSito(parametriSitoValue.importoSpeseSpedizione, spinSpeseTrasporto.Value)

                                                                                'risultato OK
                                                                                e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Salvataggio avvenuto con successo.", "0")
                                                                            Else
                                                                                e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare il percorso salvataggio PDF ordini.", "4")
                                                                            End If

                                                                        Else
                                                                            e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare e-mail di sistema.", "4")
                                                                        End If
                                                                    Else
                                                                        e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare e-mail richiesta produzione.", "4")
                                                                    End If
                                                                Else
                                                                    e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare e-mail per la notifica revisione ordini.", "4")
                                                                End If
                                                            Else
                                                                e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare e-mail per la notifica di nuovi clienti.", "4")
                                                            End If
                                                        Else
                                                            e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare e-mail per la notifica degli ordini.", "4")
                                                        End If
                                                    Else
                                                        e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare il magazzino.", "3")
                                                    End If
                                                Else
                                                    e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare il progressivo nuovo cliente.", "2")
                                                End If
                                            Else
                                                e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare il progressivo ordini.", "2")
                                            End If
                                        Else
                                            e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Impossibile modificare la fascia.", "1")
                                        End If
                                        'fine salvataggio parametri



                                    Else
                                        e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. E-Mail per la richiesta produzione non corretta.", "4")
                                    End If
                                Else
                                    e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. E-Mail di sistema non corretta.", "4")
                                End If
                            Else
                                e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. E-Mail per la notifica revisione ordini non corretta.", "4")
                            End If
                        Else
                            e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Percorso salvataggio PDF ordini non valido.", "4")
                        End If
                    Else
                        e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. E-Mail per la notifica nuovi clienti non corretta.", "4")
                    End If
                Else
                    e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. E-Mail per la notifica degli ordini non corretta.", "4")
                End If
            Else
                e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Progressivo nuovo cliente non corretto.", "2")
            End If
        Else
            e.Result = String.Format("{0}|{1}|{2}", "Esito operazione", "Operazione non riuscita. Progressivo ordini non corretto.", "2")
        End If

        dm = Nothing
    End Sub


End Class