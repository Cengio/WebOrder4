Imports System.Reflection

Public Class produzioneMasterPage
    Inherits System.Web.UI.MasterPage
    Private Sub magazzinoMasterPage_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not CType(Session("user"), user) Is Nothing AndAlso (CType(Session("user"), user).iSproduzione) Then
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
        Me.Page.Title = "Web Order v. " & Assembly.GetExecutingAssembly().GetName().Version.Major.ToString & "." & Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString
        logoHeader.ImageUrl = "~/images/custom/" & CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).webHeaderLogo
        Call updateMasterInfo()
        ASPxMenu_Top.Items.FindByName("confermaproduzione").Enabled = CType(Session("user"), user).iSproduzione
        ASPxMenu_Top.Items.FindByName("confermaproduzioneprenotazione").Enabled = CType(Session("user"), user).iSproduzione
    End Sub

    Private Sub updateMasterInfo()
        Try
            ASPxButtonEdit_user.Text = CType(Session("user"), user).nomeCompleto

            'If Session("carrelloMagazzino") Is Nothing Then
            '    Dim carrelloMagazzino As New cartManager
            '    Session.Add("carrelloMagazzino", carrelloMagazzino)
            '    lb_selectedOrdineMag.Text = "-"
            '    lb_selectedClienteMag.Text = "-"
            'Else
            '    If CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code <> "" Then
            '        lb_selectedOrdineMag.Text = CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code
            '    Else
            '        lb_selectedOrdineMag.Text = "-"
            '    End If
            '    If CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.CustomerNo <> "" Then
            '        lb_selectedClienteMag.Text = CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.CustomerNo
            '    Else
            '        lb_selectedClienteMag.Text = "-"
            '    End If
            'End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub ASPxButtonEdit_user_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles ASPxButtonEdit_user.ButtonClick
        Select Case e.ButtonIndex
            Case 0
                ' Response.Redirect("~/userprofile.aspx")
            Case 1
                Session.Abandon()
                Response.Redirect("~/login.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
        End Select
    End Sub

    Protected Sub ASPxCallbackPanel1_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback
        Call updateMasterInfo()
    End Sub


End Class