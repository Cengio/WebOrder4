Imports DevExpress.Web
Imports System.Net.Mail
Imports servisWO.datamanager
Imports log4net
Imports System.Reflection

Public Class userMng
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub userMng_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).iSusersadmin Then    'utente amministratore
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
            gridUsers.DataBind()
        End If
    End Sub

    Protected Sub gridUsers_DataBinding(sender As Object, e As EventArgs) Handles gridUsers.DataBinding
        Dim dm As New datamanager
        Dim dtUser As DataTable = dm.getUsersList
        'Dim nc As New DataColumn("inviaEmail", GetType(Integer))
        'nc.DefaultValue = 0
        'dtUser.Columns.Add(nc)
        'nc = New DataColumn("confermaPassword", GetType(String))
        'nc.DefaultValue = ""
        'dtUser.Columns.Add(nc)
        gridUsers.DataSource = dtUser
        dm = Nothing
    End Sub

    Protected Sub gridUsers_CellEditorInitialize(sender As Object, e As DevExpress.Web.ASPxGridViewEditorEventArgs) Handles gridUsers.CellEditorInitialize
        e.Editor.SetClientSideEventHandler("KeyPress", "OnTextBoxGRIDKeyPress")
        If e.Column.FieldName = "idProfile" Then
            Dim dm As New datamanager
            Dim cmb As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            cmb.DataSource = dm.getProfilesList(True)
            cmb.ValueField = "idProfile"
            cmb.ValueType = GetType(Int64)
            cmb.TextField = "description"
            cmb.DataBindItems()
            dm = Nothing
        End If
        If e.Column.FieldName = "User ID" Then
            e.Editor.ReadOnly = Not CType(sender, ASPxGridView).IsNewRowEditing
        End If
    End Sub
    Protected Sub gridUsers_CustomColumnDisplayText(sender As Object, e As ASPxGridViewColumnDisplayTextEventArgs) Handles gridUsers.CustomColumnDisplayText
        If e.Column.Name = "idProfile" Then
            If Not IsDBNull(e.Value) Then
                Dim dm As New datamanager
                e.DisplayText = dm.getProfileDescription(e.Value)
                dm = Nothing
            Else
                e.DisplayText = ""
            End If
        End If
    End Sub

    Protected Sub grid_Profiles_RowInserting(sender As Object, e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles gridUsers.RowInserting
        e.Cancel = True
        Try
            Dim grid As ASPxGridView = CType(sender, ASPxGridView)
            Dim dm As New datamanager
            If Not dm.esisteUserID(e.NewValues("User ID")) Then
                Dim u As New user
                u.cognome = e.NewValues("Cognome")
                u.nome = e.NewValues("Nome")
                u.email = e.NewValues("Email")
                u.blocked = e.NewValues("Blocked")
                u.userID = e.NewValues("User ID")
                u.password = FormsAuthentication.HashPasswordForStoringInConfigFile(e.NewValues("Password"), "SHA1")
                If Not e.NewValues("idProfile") Is Nothing Then
                    u.userProfile.idProfile = e.NewValues("idProfile")
                End If
                u.note = e.NewValues("Note")
                dm.addUser(u)
                grid.JSProperties("cp_showpopup") = "1"
                grid.JSProperties("cp_esito") = "Inserimento avvenuto con successo"
                If e.NewValues("inviaEmail") = 1 Then
                    Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
                    Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
                    Dim emailSistemaDesc As String = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)
                    If emailSistemaDesc = "" Then emailSistemaDesc = emailSistema
                    If attivaNotificheEmail = "1" AndAlso emailSistema <> "" Then
                        Dim msg As MailMessage = New MailMessage()
                        msg.From = New MailAddress(emailSistema, emailSistemaDesc)
                        msg.To.Add(u.email)
                        msg.Subject = "Attivazione account Ordini Web"
                        msg.Body = "Gentile " & u.cognome & " " & u.nome & vbCrLf
                        msg.Body &= "ti comunichiamo l'attivazione del tuo nuovo account per accedere ad ordini web." & vbCrLf & vbCrLf
                        msg.Body &= "User ID: " & u.userID & vbCrLf
                        msg.Body &= "Password: " & e.NewValues("Password") & vbCrLf & vbCrLf
                        msg.Body &= "Cordiali Saluti" & vbCrLf
                        Dim client As SmtpClient = New SmtpClient
                        Try
                            client.Send(msg)
                            Log.Info("Email " & msg.Subject & " sent to " & msg.To.ToString & " " & msg.CC.ToString)
                        Catch ex As Exception
                            grid.JSProperties("cp_showpopup") = "1"
                            grid.JSProperties("cp_esito") = ex.Message
                            grid.CancelEdit()
                            Log.Error(ex.ToString)
                            Exit Sub
                        End Try
                    End If
                End If
                grid.CancelEdit()
                grid.DataBind()
            Else
                grid.CancelEdit()
                grid.JSProperties("cp_showpopup") = "1"
                grid.JSProperties("cp_esito") = "User ID già esistente."
            End If
            dm = Nothing
        Catch ex As Exception
            gridUsers.CancelEdit()
            gridUsers.JSProperties("cp_showpopup") = "1"
            gridUsers.JSProperties("cp_esito") = ex.Message
        End Try
    End Sub

    Protected Sub grid_Profiles_RowUpdating(sender As Object, e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs) Handles gridUsers.RowUpdating
        e.Cancel = True
        Try
            Dim grid As ASPxGridView = CType(sender, ASPxGridView)
            Dim dm As New datamanager
            Dim u As New user
            u.userCode = e.Keys("User Code")
            u.cognome = e.NewValues("Cognome")
            u.nome = e.NewValues("Nome")
            u.email = e.NewValues("Email")
            u.blocked = e.NewValues("Blocked")
            If e.NewValues("Password") <> "" AndAlso FormsAuthentication.HashPasswordForStoringInConfigFile(e.NewValues("Password"), "SHA1") <> e.OldValues("Password") Then
                u.password = FormsAuthentication.HashPasswordForStoringInConfigFile(e.NewValues("Password"), "SHA1")
            End If
            If Not e.NewValues("idProfile") Is Nothing Then
                u.userProfile.idProfile = e.NewValues("idProfile")
            End If
            u.note = e.NewValues("Note")
            dm.updUser(u)
            grid.JSProperties("cp_showpopup") = "1"
            grid.JSProperties("cp_esito") = "Aggiornamento avvenuto con successo"
            If e.NewValues("inviaEmail") = 1 Then
                Dim emailSistema As String = dm.GetParametroSito(parametriSitoValue.emailSistema)
                Dim attivaNotificheEmail As String = dm.GetParametroSito(parametriSitoValue.attivaNotificheEmail)
                Dim emailSistemaDesc As String = dm.GetParametroSito(parametriSitoValue.emailSistemaDesc)
                If emailSistemaDesc = "" Then emailSistemaDesc = emailSistema
                If attivaNotificheEmail = "1" AndAlso emailSistema <> "" Then
                    Dim msg As MailMessage = New MailMessage()
                    msg.From = New MailAddress(emailSistema, emailSistemaDesc)
                    msg.To.Add(u.email)
                    msg.Subject = "Attivazione account Ordini Web"
                    msg.Body = "Gentile " & u.cognome & " " & u.nome & vbCrLf
                    msg.Body &= "ti comunichiamo l'attivazione del tuo nuovo account per accedere ad ordini web." & vbCrLf & vbCrLf
                    msg.Body &= "User ID: " & u.userID & vbCrLf
                    msg.Body &= "Password: " & e.NewValues("Password") & vbCrLf & vbCrLf
                    msg.Body &= "Cordiali Saluti" & vbCrLf
                    Dim client As SmtpClient = New SmtpClient
                    Try
                        client.Send(msg)
                        Log.Info("Email " & msg.Subject & " sent to " & msg.To.ToString & " " & msg.CC.ToString)
                    Catch ex As Exception
                        grid.JSProperties("cp_showpopup") = "1"
                        grid.JSProperties("cp_esito") = ex.Message
                        grid.CancelEdit()
                        Log.Error(ex.ToString)
                        Exit Sub
                    End Try
                End If
            End If
            grid.CancelEdit()
            grid.DataBind()
            dm = Nothing
        Catch ex As Exception
            gridUsers.CancelEdit()
            gridUsers.JSProperties("cp_showpopup") = "1"
            gridUsers.JSProperties("cp_esito") = ex.Message
        End Try
    End Sub


End Class