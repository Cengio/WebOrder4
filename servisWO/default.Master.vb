Imports System.Reflection
Imports System.Xml
Imports log4net
Imports DevExpress.Web

Public Class _default
    Inherits System.Web.UI.MasterPage

    Private ReadOnly Log As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).isAuthenticated Then

            End If
        Catch ex As Exception
            Try
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex2 As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        'System.Diagnostics.Debug.WriteLine(Now() & ": START defaultMaster_Page_Load")

        logoHeader.ImageUrl = "~/images/custom/" & CType(HttpContext.Current.Session("wkcManager"), workingCompanyManager).webHeaderLogo


        If Not Page.IsPostBack Then
            If Request.QueryString("ea") = "1" Or Request.QueryString("ea") Is Nothing Then
                cb_ESCLUDIarcheopatici.Checked = True
            Else
                cb_ESCLUDIarcheopatici.Checked = False
            End If
            If Request.QueryString("ee") = "1" Or Request.QueryString("ee") Is Nothing Then
                cb_ESCLUDIexpo.Checked = True
            Else
                cb_ESCLUDIexpo.Checked = False
            End If
        End If

        If Not CType(Session("user"), user) Is Nothing AndAlso CType(Session("user"), user).isAuthenticated = True Then
            Me.Page.Title = "Web Order v. " & Assembly.GetExecutingAssembly().GetName().Version.Major.ToString & "." & Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString
            TimeoutControl1.TimeOutUrl = "~/login.aspx"

            If Session("cart") Is Nothing Then
                Dim cart As New cartManager
                Session.Add("cart", cart)
                lb_masterCartQta.Text = 0
                lb_masterCartTot.Text = String.Format("{0:C}", 0)
            End If

            ASPxMenu_Main.Items.FindByName("carrello").Enabled = CType(Session("user"), user).iSordini
            ASPxMenu_Main.Items.FindByName("cerca").Enabled = CType(Session("user"), user).iSordini
            ASPxMenu_Main.Items.FindByName("cercadimensioni").Visible = CType(Session("user"), user).iScercadimensioni
            ASPxMenu_Main.Items.FindByName("ordini").Enabled = CType(Session("user"), user).iSordini
            ASPxMenu_Main.Items.FindByName("clienti").Enabled = CType(Session("user"), user).iSordini
            ASPxMenu_Main.Items.FindByName("magazzino").Visible = (CType(Session("user"), user).iSmagazzino Or CType(Session("user"), user).iSmagazzinoadmin)
            ASPxMenu_Main.Items.FindByName("produzione").Visible = CType(Session("user"), user).iSproduzione
            ASPxMenu_Main.Items.FindByName("gestione").Visible = (CType(Session("user"), user).iSusersadmin Or CType(Session("user"), user).iSwebsiteadmin)
            ASPxMenu_Main.Items.FindByName("utenti").Visible = CType(Session("user"), user).iSusersadmin
            ASPxMenu_Main.Items.FindByName("parametrisito").Visible = CType(Session("user"), user).iSwebsiteadmin
            ASPxMenu_Main.Items.FindByName("ordinidaapprovare").Visible = CType(Session("user"), user).iSrevisoreordini
            cercaprodotto.Visible = CType(Session("user"), user).iSordini


            lbQuickCart.Visible = CType(Session("user"), user).iSordini

            If CType(Session("cart"), cartManager).Header.ordineHeader.Code <> "" Then
                ASPxMenu_Main.Items.FindByName("carrello").Text = "Ordine (N. " & CType(Session("cart"), cartManager).Header.ordineHeader.Code.ToString & ")"
            Else
                'ASPxMenu_Main.Items.FindByName("carrello").Text = "Carrello (N. " & CType(Session("cart"), cartManager).Header.IDCART.ToString & ")"
                ASPxMenu_Main.Items.FindByName("carrello").Text = "Carrello"
            End If

            If CType(Session("cart"), cartManager).Header.IDCART = 0 Then
                ASPxMenu_Main.Items.FindByName("carrello").Enabled = False
                ASPxMenu_Main.Items.FindByName("carrello").Text = "Carrello (Vuoto)"
            End If

            Call updateMasterCartInfo()
            'tb_quickSearch.Text = Session("finderText")

            Call loadPromo()
            Call loadScontoScaglioni()

            'System.Diagnostics.Debug.WriteLine(Now() & ": END defaultMaster_Page_Load")

        Else
            Try
                Response.Redirect("~/err/err403.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Catch ex As Exception
                DevExpress.Web.ASPxWebControl.RedirectOnCallback("~/err/err403.aspx")
            End Try
        End If
    End Sub

    Protected Sub btn_finder_Click(sender As Object, e As EventArgs)
        Dim btn As ASPxButton = CType(sender, ASPxButton)
        Dim qpar As String = "?t=" & tb_quickSearch.Text.Trim
        If cb_ESCLUDIarcheopatici.Checked Then
            qpar &= "&" & "ea=1"
        Else
            qpar &= "&" & "ea=0"
        End If
        If cb_ESCLUDIexpo.Checked Then
            qpar &= "&" & "ee=1"
        Else
            qpar &= "&" & "ee=0"
        End If
        Response.Redirect("~/productFinder.aspx" & qpar, False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub ASPxCallbackPanel1_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles CallBMasterCart.Callback
        Call updateMasterCartInfo()
    End Sub

    Private Sub updateMasterCartInfo()
        Try
            Dim dm As New datamanager
            'Gestione visualizzazione informazioni veloci Carrello / Ordine selezionato 
            If CType(Session("cart"), cartManager).Header.ordineHeader.Code <> "" Then
                lb_ProdottiNelCarrelloOrdine.Text = "Prodotti nell'ordine:"
            Else
                lb_ProdottiNelCarrelloOrdine.Text = "Prodotti nel carrello:"
            End If

            lb_masterCartQta.Text = CType(Session("cart"), cartManager).getTotaleQta
            lb_masterCartTot.Text = CType(Session("cart"), cartManager).GetTotaleMerceConScontoPagamento.ToString("C")
            lb_masterCartQtaOmaggio.Text = CType(Session("cart"), cartManager).getTotaleQtaOmaggio
            lb_masterCartScontoHeader.Text = CType(Session("cart"), cartManager).Header.SCONTOHEADER.ToString()
            lb_masterCartProfiloSconto.Text = dm.getProfiloScontoDescrizione(CType(Session("cart"), cartManager).Header.IDPROFILOSCONTO)

            If CType(Session("cart"), cartManager).Header.ordineHeader.ShipAddressCode <> "" Then
                Dim dt As DataTable = dm.GetShipToAddress(CType(Session("cart"), cartManager).Header.CODICECLIENTE, CType(Session("cart"), cartManager).Header.ordineHeader.ShipAddressCode).Tables(0)
                If dt.Rows.Count > 0 Then
                    'lb_MasterDestinazione.Text = dt.Rows(0).Item("Name") & " " & dt.Rows(0).Item("Address") & " " & dt.Rows(0).Item("City") & " " & dt.Rows(0).Item("County")
                    lb_MasterDestinazione.Text = dt.Rows(0).Item("Address") & " - " & dt.Rows(0).Item("City") & " - " & dt.Rows(0).Item("County")
                End If
            Else
                If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Then
                    lb_MasterDestinazione.Text = "IDEM FATTURAZIONE"
                Else
                    lb_MasterDestinazione.Text = "-"
                End If

            End If

            ASPxButtonEdit_user.Text = CType(Session("user"), user).nomeCompleto


            If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Then
                If CType(Session("cart"), cartManager).Header.CODICECLIENTE.ToString.StartsWith("N") Then
                    HyperLink_masterSelectedCustomer.Text = CType(Session("cart"), cartManager).Header.CODICECLIENTE & " - " & dm.GetNewCustomers(CType(Session("cart"), cartManager).Header.CODICECLIENTE).Rows(0).Item("Name").ToString
                Else
                    HyperLink_masterSelectedCustomer.Text = CType(Session("cart"), cartManager).Header.CODICECLIENTE & " - " & dm.GetCustomers(CType(Session("cart"), cartManager).Header.CODICECLIENTE).Tables(0).Rows(0).Item("Name").ToString
                End If
                cb_DeselezionaCliente.Visible = True
                'cb_DeselezionaCliente.Checked = True
                Dim catCliente As String = dm.getCategoriaClienteDescrizione(CType(Session("cart"), cartManager).Header.CODICECLIENTE)
                If catCliente <> "" Then
                    lb_masterCategoriaCliente.Text = "(" & catCliente & ")"
                    lb_masterClienteLabel.Visible = False
                End If
            ElseIf Not Session("infoCustomerNo") Is Nothing AndAlso Session("infoCustomerNo") <> "" Then
                HyperLink_masterSelectedCustomer.Text = Session("infoCustomerNo") & " - " & dm.GetCustomers(Session("infoCustomerNo")).Tables(0).Rows(0).Item("Name").ToString
                cb_DeselezionaCliente.Visible = True
                Dim catCliente As String = dm.getCategoriaClienteDescrizione(Session("infoCustomerNo"))
                If catCliente <> "" Then
                    lb_masterCategoriaCliente.Text = "(" & catCliente & ")"
                    lb_masterClienteLabel.Visible = False
                End If
            Else
                HyperLink_masterSelectedCustomer.Text = "-"
                HyperLink_masterSelectedCustomer.Enabled = False
                cb_DeselezionaCliente.Visible = False
            End If

            dm = Nothing
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub ASPxButtonEdit_user_ButtonClick(source As Object, e As DevExpress.Web.ButtonEditClickEventArgs) Handles ASPxButtonEdit_user.ButtonClick
        Dim dm As New datamanager
        Select Case e.ButtonIndex
            Case 0
                Response.Redirect("~/userprofile.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            Case 1
                Log.Info("LOGOUT: " & CType(Session("user"), user).nomeCompleto)
                dm.regUserAudit(CType(Session("user"), user).userCode, userAuditEvent.LOGOUT)
                Session.Abandon()
                Response.Redirect("~/login.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
        End Select
        dm = Nothing
    End Sub

    Protected Sub cb_DeselezionaCliente_CheckedChanged(sender As Object, e As EventArgs) Handles cb_DeselezionaCliente.CheckedChanged
        If cb_DeselezionaCliente.Checked = False Then
            If Session("infoCustomerNo") <> "" Then
                Log.Info(CType(Session("user"), user).nomeCompleto & " deselected customer: " & Session("infoCustomerNo"))
            Else
                Log.Info(CType(Session("user"), user).nomeCompleto & " deselected customer: " & CType(Session("cart"), cartManager).Header.CODICECLIENTE)
            End If
            Session("infoCustomerNo") = ""
            CType(Session("cart"), cartManager).clearCart()
            Response.Redirect("~/customerList.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End If
    End Sub

    Private Sub loadPromo()
        If CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Or Session("infoCustomerNo") <> "" Then
            Dim dm As New datamanager
            Dim CTCODE As String = ""
            If Session("infoCustomerNo") <> "" Then
                CTCODE = dm.GetContactCodeByCustomerCode(Session("infoCustomerNo"))
            ElseIf CType(Session("cart"), cartManager).Header.CODICECONTATTO <> "" Then
                CTCODE = CType(Session("cart"), cartManager).Header.CODICECONTATTO
            End If
            If CTCODE <> "" Then
                Dim dtpromo As DataTable = dm.getPromoFromCRM(CTCODE)
                dataViewPromo.DataSource = dtpromo
            End If
            dm = Nothing
        End If
    End Sub

    Protected Sub formLayoutPromo_DataBinding(sender As Object, e As EventArgs)
        Dim fl As ASPxFormLayout = TryCast(sender, ASPxFormLayout)
        Dim container As DataViewItemTemplateContainer = TryCast(fl.NamingContainer, DataViewItemTemplateContainer)
        fl.DataSource = container.DataItem
    End Sub

    Protected Sub formLayoutPromo_LayoutItemDataBound(sender As Object, e As LayoutItemDataBoundEventArgs)
        If e.LayoutItem.FieldName = "num_Max_Utilizzi" Then
            Dim lb As ASPxLabel = TryCast(e.LayoutItem.GetNestedControl(), ASPxLabel)
            Dim lbcontent As String = e.NestedControlValue.ToString()
            If lbcontent = "1000000000" Then
                lb.Text = "-"
            End If
        ElseIf e.LayoutItem.FieldName = "data_Attivazione" Then
            Dim lb As ASPxLabel = TryCast(e.LayoutItem.GetNestedControl(), ASPxLabel)
            Dim lbcontent As String = e.NestedControlValue.ToString()
            lb.Text = Left(lbcontent, 10)
        End If
    End Sub

    Protected Sub btn_DettaglioCarrello_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/orderDetails.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub

    Protected Sub btn_DettaglioCarrello_Init(sender As Object, e As EventArgs)
        Dim pulsante As ASPxButton = CType(sender, ASPxButton)
        If Not CType(Session("cart"), cartManager) Is Nothing Then
            If CType(Session("cart"), cartManager).Header.IDCART > 0 Then
                pulsante.Enabled = True
            Else
                pulsante.Enabled = False
            End If
        End If
    End Sub

    Private Sub loadScontoScaglioni()
        If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Or Session("infoCustomerNo") <> "" Then
            Dim CodiceCliente As String = ""
            If CType(Session("cart"), cartManager).Header.CODICECLIENTE <> "" Then
                CodiceCliente = CType(Session("cart"), cartManager).Header.CODICECLIENTE
            ElseIf Session("infoCustomerNo") <> "" Then
                CodiceCliente = Session("infoCustomerNo")
            End If
            Dim dm As New datamanager
            Dim dt As DataTable = dm.GetScontiScaglioni(CodiceCliente)
            dataViewScontoScaglioni.DataSource = dt
            dm = Nothing
        End If
    End Sub

    Protected Sub formLayoutScontoScaglioni_DataBinding(sender As Object, e As EventArgs)
        Dim fl As ASPxFormLayout = TryCast(sender, ASPxFormLayout)
        Dim container As DataViewItemTemplateContainer = TryCast(fl.NamingContainer, DataViewItemTemplateContainer)
        fl.DataSource = container.DataItem
    End Sub

    Protected Sub formLayoutScontoScaglioni_LayoutItemDataBound(sender As Object, e As LayoutItemDataBoundEventArgs)
        Dim fl As ASPxFormLayout = TryCast(sender, ASPxFormLayout)
        If e.LayoutItem.FieldName = "Attivo" Then
            If e.NestedControlValue.ToString() = "True" Then
                fl.BackColor = Drawing.Color.FromArgb(154, 205, 50)
                Dim lb As ASPxLabel = TryCast(e.LayoutItem.GetNestedControl(), ASPxLabel)
                lb.Text = "SI"
            Else
                fl.BackColor = Drawing.Color.FromArgb(0, 255, 160)
                Dim lb As ASPxLabel = TryCast(e.LayoutItem.GetNestedControl(), ASPxLabel)
                lb.Text = "NO"
            End If
        End If
        If e.LayoutItem.FieldName = "Datafine" Then
            Dim lb As ASPxLabel = TryCast(e.LayoutItem.GetNestedControl(), ASPxLabel)
            lb.Text = Convert.ToDateTime(e.NestedControlValue).ToString("dd-MM-yyyy")
        End If
        If e.LayoutItem.FieldName = "FatturatoDa" Then
            Dim lb As ASPxLabel = TryCast(e.LayoutItem.GetNestedControl(), ASPxLabel)
            lb.Text = Convert.ToDecimal(e.NestedControlValue).ToString("c2")
        End If
        If e.LayoutItem.FieldName = "Upselling" Then
            Dim lb As ASPxLabel = TryCast(e.LayoutItem.GetNestedControl(), ASPxLabel)
            lb.Text = Convert.ToDecimal(e.NestedControlValue).ToString("c2")
        End If
    End Sub

#Region "treeview"

    Protected Sub ASPxTreeView_catalogo_VirtualModeCreateChildren(source As Object, e As DevExpress.Web.TreeViewVirtualModeCreateChildrenEventArgs)
        Dim item As MenuItem = ASPxMenu_Main.Items.FindByName("cercadimensioni")
        Dim dsmacrolinee As SqlDataSource = CType(item.FindControl("dsmacrolinee"), SqlDataSource)
        Dim dslinee As SqlDataSource = CType(item.FindControl("dslinee"), SqlDataSource)
        Dim dsfamiglie As SqlDataSource = CType(item.FindControl("dsfamiglie"), SqlDataSource)
        Dim dssottofamiglie As SqlDataSource = CType(item.FindControl("dssottofamiglie"), SqlDataSource)

        Dim dm As New datamanager
        dsmacrolinee.ConnectionString = dm.GetWORconnectionString()
        dsmacrolinee.SelectCommand = dm.getMacrolineeTreeSelectCommand()
        dslinee.ConnectionString = dm.GetWORconnectionString()
        dslinee.SelectCommand = dm.getLineeTreeSelectCommand()
        dsfamiglie.ConnectionString = dm.GetWORconnectionString()
        dsfamiglie.SelectCommand = dm.getFamiglieTreeSelectCommand()
        dssottofamiglie.ConnectionString = dm.GetWORconnectionString()
        dssottofamiglie.SelectCommand = dm.getSottofamiglieTreeSelectCommand()
        dm = Nothing

        Dim list As List(Of TreeViewVirtualNode) = New List(Of TreeViewVirtualNode)()
        If e.NodeName Is Nothing Then
            For Each rw As DataRowView In dsmacrolinee.Select(DataSourceSelectArguments.Empty)
                Dim name As String = String.Format("m{0},{1}", rw.Row(0), Guid.NewGuid())
                Dim node As New TreeViewVirtualNode(name, FormatText(rw.Row(1).ToString()), "", "~/productFinder.aspx?m=" & rw.Row(0).ToString, "_self")
                node.IsLeaf = Not HasChildNodes(dslinee, "cod_macrolinea", rw.Row(0).ToString())
                list.Add(node)
            Next rw
            e.Children = list
            Return
        End If
        If e.NodeName(0) = "m"c Then
            dslinee.SelectParameters("cod_macrolinea").DefaultValue = GetId(e.NodeName)
            For Each rw As DataRowView In dslinee.Select(DataSourceSelectArguments.Empty)
                Dim name As String = String.Format("l{0},{1}", rw.Row(0), Guid.NewGuid())
                Dim node As New TreeViewVirtualNode(name, FormatText(rw.Row(1).ToString), "", "~/productFinder.aspx?l=" & rw.Row(0).ToString, "_self")
                node.IsLeaf = Not HasChildNodes(dsfamiglie, "cod_linea", rw.Row(0).ToString())
                list.Add(node)
            Next rw
            e.Children = list
            Return
        End If
        If e.NodeName(0) = "l"c Then
            dsfamiglie.SelectParameters("cod_linea").DefaultValue = GetId(e.NodeName)
            For Each rw As DataRowView In dsfamiglie.Select(DataSourceSelectArguments.Empty)
                Dim name As String = String.Format("s{0},{1}", rw.Row(0), Guid.NewGuid())
                Dim node As New TreeViewVirtualNode(name, FormatText(rw.Row(1).ToString), "", "~/productFinder.aspx?f=" & rw.Row(0).ToString, "_self")
                node.IsLeaf = Not HasChildNodes(dssottofamiglie, "cod_famiglia", rw.Row(0).ToString())
                list.Add(node)
            Next rw
            e.Children = list
            Return
        End If
        If e.NodeName(0) = "s"c Then
            dssottofamiglie.SelectParameters("cod_famiglia").DefaultValue = GetId(e.NodeName)
            For Each rw As DataRowView In dssottofamiglie.Select(DataSourceSelectArguments.Empty)
                Dim node As New TreeViewVirtualNode(Guid.NewGuid().ToString(), FormatText(rw.Row(1).ToString()), "", "~/productFinder.aspx?s=" & rw.Row(0).ToString, "_self")
                node.IsLeaf = True
                list.Add(node)
            Next rw
            e.Children = list
        End If
    End Sub

    Private Function GetId(ByVal nodeName As String) As String
        Return nodeName.Split(","c)(0).Substring(1)
    End Function

    Private Function FormatText(ByVal text As String) As String
        If text.Length > 40 Then
            Return text.Substring(0, 40) & "..."
        End If
        Return text
    End Function

    Private Function HasChildNodes(ByVal ds As SqlDataSource, ByVal ParamArray parameters() As String) As Boolean
        If parameters IsNot Nothing Then
            ds.SelectParameters(parameters(0)).DefaultValue = parameters(1)
        End If
        Dim dw As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
        If IsNothing(dw) Then
            Return False
        Else
            Return dw.Table.Rows.Count > 0
        End If
        'Return dw.Table.Rows.Count > 0
    End Function


#End Region


End Class