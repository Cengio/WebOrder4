Public Class magRaccogliOrdine
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dm2 As New datamanager
        'Call dm2.synchNewCustomerCertificazione()
        Call dm2.verificaCertificazioneOrdiniNuociCliente()
        dm2 = Nothing

        If Not Session("prossimoOrdineDaRaccogliere") Is Nothing AndAlso Session("prossimoOrdineDaRaccogliere") <> "" Then
            btnCarica.Enabled = False
            btnInizia.Enabled = True
            btnStampa.Enabled = True
            btnSospendi.Enabled = False
            Call caricaProssimoOrdine()
        Else
            btnCarica.Enabled = True
            btnInizia.Enabled = False
            btnStampa.Enabled = False
            btnSospendi.Enabled = False
            Call resetLabel()
        End If
        If Not CType(Session("carrelloMagazzino"), cartManager) Is Nothing AndAlso CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code <> "" Then
            btnCarica.Enabled = False
            btnInizia.Enabled = False
            btnStampa.Enabled = True
            btnSospendi.Enabled = True
            Call resetLabel()
            lbAvviso.Text = "ORDINE NR. " & CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code & " IN FASE DI RACCOLTA."
            grid.Visible = False
            'ASPxFormLayout1.Visible = False
            ASPxFormLayout2.Visible = False
        Else
            Dim dm As New datamanager
            Dim dtOrdine As DataTable = dm.getOrdineInRaccoltaByUserCode(CType(Session("user"), user).userCode)
            If dtOrdine.Rows.Count > 0 Then
                lbNrOrdine.Text = dtOrdine.Rows(0).Item("Code")
                lbDataOrdine.Text = dtOrdine.Rows(0).Item("OrderDate")
                lbCodiceCliente.Text = dtOrdine.Rows(0).Item("CustomerNo")
                lbRagioneSociale.Text = dm.GetCustomerName(dtOrdine.Rows(0).Item("CustomerNo"))
                lbOperatoreCreazione.Text = dm.getUserNameSurname(dtOrdine.Rows(0).Item("OperatorCode"))
                Dim dtLines As DataTable = dm.getLinesOrdiniMagazzino(dtOrdine.Rows(0).Item("Code"), "BinCode").Tables(0)
                'dtLines.DefaultView.Sort = "BinCode ASC"
                grid.DataSource = dtLines
                grid.DataBind()
                btnCarica.Enabled = False
                btnInizia.Enabled = True
                btnStampa.Enabled = True
                btnSospendi.Enabled = False
            End If
            dm = Nothing
        End If

    End Sub

    Protected Sub btnCarica_Click(sender As Object, e As EventArgs) Handles btnCarica.Click
        Call caricaProssimoOrdine()
    End Sub

    Private Sub caricaProssimoOrdine()
        Dim dm As New datamanager
        Dim dtOrdine As DataTable = dm.getOrdineDaRaccogliere(CType(Session("user"), user).userCode)
        If dtOrdine.Rows.Count > 0 Then
            lbNrOrdine.Text = dtOrdine.Rows(0).Item("Code")
            lbDataOrdine.Text = dtOrdine.Rows(0).Item("dataordine")
            lbCodiceCliente.Text = dtOrdine.Rows(0).Item("CustomerNo")
            lbRagioneSociale.Text = dm.GetCustomerName(dtOrdine.Rows(0).Item("CustomerNo"))
            lbOperatoreCreazione.Text = dm.getUserNameSurname(dtOrdine.Rows(0).Item("OperatorCode"))
            Session("prossimoOrdineDaRaccogliere") = dtOrdine.Rows(0).Item("Code")

            Dim dtLines As DataTable = dm.getLinesOrdiniMagazzino(dtOrdine.Rows(0).Item("Code"), "BinCode").Tables(0)
            'dtLines.DefaultView.Sort = "BinCode ASC"
            grid.DataSource = dtLines
            grid.DataBind()

            btnCarica.Enabled = False
            btnInizia.Enabled = True
            btnStampa.Enabled = True
            btnSospendi.Enabled = False
        Else
            Call resetLabel()
            lbAvviso.Text = "NESSUN ORDINE DISPONIBILE PER LA RACCOLTA"
        End If
        dm = Nothing
    End Sub

    Private Sub resetLabel()
        Session("prossimoOrdineDaRaccogliere") = ""
        lbNrOrdine.Text = ""
        lbDataOrdine.Text = ""
        lbCodiceCliente.Text = ""
        lbRagioneSociale.Text = ""
        lbOperatoreCreazione.Text = ""
        lbAvviso.Text = ""
    End Sub

    Protected Sub btnInizia_Click(sender As Object, e As EventArgs) Handles btnInizia.Click
        Dim dm As New DataManager
        If Not Session("prossimoOrdineDaRaccogliere") Is Nothing AndAlso Session("prossimoOrdineDaRaccogliere") <> "" Then
            'controllo concorrenza per gli ordini non assegnati
            Dim ordineLibero As Boolean = False
            Dim dtH As DataTable = dm.getHeaderOrdiniMagazzino(Session("prossimoOrdineDaRaccogliere")).Tables(0)
            If (dtH.Rows(0).Item("Status") = 2 _
                Or (dtH.Rows(0).Item("Status") = 3 And dtH.Rows(0).Item("User") = "") _
                Or (dtH.Rows(0).Item("Status") = 3 And dtH.Rows(0).Item("User") <> "" AndAlso CLng(dtH.Rows(0).Item("User")) = CType(Session("user"), user).userCode)) Then
                ordineLibero = True
            End If
            If ordineLibero Then
                Dim carrelloMagazzino As New cartManager
                Session("carrelloMagazzino") = carrelloMagazzino.pickOrder(Session("prossimoOrdineDaRaccogliere"), CType(Session("user"), user).userCode)
                Session("currentLineIndex") = 0
                Session("prossimoOrdineDaRaccogliere") = ""
                Response.Redirect("magPicking.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
                'DevExpress.Web.ASPxWebControl.RedirectOnCallback("magPicking.aspx")
            Else
                popupOrdineOccupato.ShowOnPageLoad = True
            End If
        Else
            Dim dtOrdine As DataTable = dm.getOrdineInRaccoltaByUserCode(CType(Session("user"), user).userCode)
            If dtOrdine.Rows.Count > 0 Then
                Dim carrelloMagazzino As New cartManager
                Session("carrelloMagazzino") = carrelloMagazzino.pickOrder(dtOrdine.Rows(0).Item("Code"), CType(Session("user"), user).userCode)
                Session("currentLineIndex") = 0
                Session("prossimoOrdineDaRaccogliere") = ""
                Response.Redirect("magPicking.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        End If
        dm = Nothing
    End Sub

    Protected Sub grid_CustomColumnDisplayText(sender As Object, e As DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs) Handles grid.CustomColumnDisplayText
        If e.Column.FieldName = "RowDiscount" Then
            If e.Value = 1 Then
                e.DisplayText = "X"
            Else
                e.DisplayText = ""
            End If
        End If
    End Sub

    Protected Sub btnOKordineOccupato_Click(sender As Object, e As EventArgs) Handles btnOKordineOccupato.Click
        Session("prossimoOrdineDaRaccogliere") = ""
        Response.Redirect("magRaccogliOrdine.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub ASPxCallback_report_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs) Handles ASPxCallback_report.Callback
        Session("orderCodeReport") = lbNrOrdine.Text.Trim
        Session("linesOrderByField") = "Descrizione"
        Session("showReportDataUltimaModifica") = "1"
    End Sub

    Protected Sub btnSospendi_Click(sender As Object, e As EventArgs) Handles btnSospendi.Click
        Dim dm As New datamanager
        dm.cambiaSospensioneRaccoltaOrdine(CType(Session("carrelloMagazzino"), cartManager).Header.ordineHeader.Code, 1, CType(Session("user"), user).userCode)
        dm = Nothing
        CType(Session("carrelloMagazzino"), cartManager).clearCart()
        Session("prossimoOrdineDaRaccogliere") = ""
        Response.Redirect("magRaccogliOrdine.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

End Class