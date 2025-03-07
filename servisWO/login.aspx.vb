Imports System.Reflection
Imports log4net

Public Class login
    Inherits System.Web.UI.Page

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub login_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim dm As New datamanager
        If Not (dm.TestDbConnection(dm.GetNAVconnectionString) And dm.TestDbConnection(dm.GetWORconnectionString)) Then
            btn_login.Enabled = False
            tb_password.Enabled = False
            tb_userid.Enabled = False
            comboCompany.Enabled = False
            ASPxPopupControl1.ShowOnPageLoad = True
        End If
        dm = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
        If Not Page.IsPostBack Then
            If Session("tentativi") Is Nothing Then
                Session.Add("tentativi", 1)
            End If
            Dim dm As New datamanager
            comboCompany.DataSource = dm.GetCompany
            comboCompany.ValueField = "code"
            comboCompany.TextField = "code"
            comboCompany.DataBind()
            If comboCompany.Items IsNot Nothing AndAlso comboCompany.Items.IndexOfValue("Ser-Vis Srl") >= 0 Then
                comboCompany.SelectedIndex = comboCompany.Items.IndexOfValue("Ser-Vis Srl")
            End If
            dm = Nothing
            End If
        If Session("tentativi") > 1 Then
            captchaLogin.Visible = True
        Else
            captchaLogin.Visible = False
        End If
        tdBrowser.Visible = False
        tb_userid.Focus()
        lb_version.Text = "Web Order v. " & Assembly.GetExecutingAssembly().GetName().Version.Major.ToString & "." & Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString

        'System.Diagnostics.Debug.WriteLine("CurrentUICulture: {0}", Globalization.CultureInfo.CurrentUICulture)
        'System.Diagnostics.Debug.WriteLine("CurrentCulture: {0}", Globalization.CultureInfo.CurrentCulture)

    End Sub

    Protected Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        Session("tentativi") = Session("tentativi") + 1
        Dim user_id As String = tb_userid.Text
        Dim pass As String = tb_password.Text
        Try
            Dim ds As New DataSet
            Dim dm As New datamanager

            Dim u As New user
            u = dm.authenticate(user_id, FormsAuthentication.HashPasswordForStoringInConfigFile(pass, "SHA1"))
            If u.isAuthenticated Then
                Log.Info("LOGIN: " & u.nomeCompleto)
                lb_failReason.Text = ""
                Session.Clear()
                Session.Add("user", u)
                Dim wkcManager As New workingCompanyManager
                wkcManager.setWorkingCompany(comboCompany.SelectedItem.Value)
                'workingCompanyManager.setWorkingCompany(comboCompany.SelectedItem.Value)
                Session.Add("wkcManager", wkcManager)
                Log.Info("setWorkingCompany: " & comboCompany.SelectedItem.Value)

                If Session("cart") Is Nothing Then
                    Dim cart As New cartManager
                    Session.Add("cart", cart)
                End If
                If Session("carrelloMagazzino") Is Nothing Then
                    Dim carrelloMagazzino As New cartManager
                    Session.Add("carrelloMagazzino", carrelloMagazzino)
                End If

                If u.iSmagazzinoONLY Then
                    If u.iSmagazzinoadmin Then
                        Response.Redirect("~/mag/magOrderToPickList.aspx?t=2&cf=0", False)
                        Context.ApplicationInstance.CompleteRequest()
                    ElseIf u.iSmagazzino Then
                        Response.Redirect("~/mag/magRaccogliOrdine.aspx", False)
                        Context.ApplicationInstance.CompleteRequest()
                    End If
                ElseIf u.iSproduzioneONLY Then
                    Response.Redirect("~/pro/proConfirmProduction.aspx", False)
                    Context.ApplicationInstance.CompleteRequest()
                Else
                    Response.Redirect("~/customerList.aspx", False)
                    Context.ApplicationInstance.CompleteRequest()
                End If

            Else
                lb_failReason.Text = "Credenziali errate o utente non autorizzato"
            End If


        Catch exc As Exception
            '       lb_failReason.Text = exc.Message
            Log.Error(exc.ToString)
        End Try
    End Sub
End Class