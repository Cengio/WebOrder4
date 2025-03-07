Public Class usermyprofile
    Inherits System.Web.UI.Page
    Private Sub usermyprofile_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).isAuthenticated Then    'utente amministratore
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
            Call loadData(CType(Session("user"), user).userCode)
        End If
    End Sub

    Private Sub loadData(ByVal selectedwebuser As Integer)
        Dim dm As New datamanager
        Dim dtuser As DataTable = dm.getUser(selectedwebuser)
        If dtuser.Rows.Count > 0 Then
            lb_userid.Text = dtuser.Rows(0).Item("User ID")
            tb_password.Text = dtuser.Rows(0).Item("Password")
            tb_nome.Text = dtuser.Rows(0).Item("Nome")
            tb_cognome.Text = dtuser.Rows(0).Item("Cognome")
            tb_email.Text = dtuser.Rows(0).Item("Email")
            cbox_Bloccato.Checked = IIf(dtuser.Rows(0).Item("Blocked") = 0, False, True)
            'comboTipoUtente.SelectedIndex = comboTipoUtente.Items.IndexOfValue(dtuser.Rows(0).Item("User Type").ToString)
            lb_dataCreazione.Text = dtuser.Rows(0).Item("Date_Creazione")
            lb_dataLastLogin.Text = dtuser.Rows(0).Item("Date_LastLogin")
        End If
        dm = Nothing
    End Sub


End Class