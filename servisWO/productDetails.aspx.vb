Imports DevExpress.XtraCharts
Imports DevExpress.Web

Public Class productDetails
    Inherits System.Web.UI.Page
    Private Sub productDetails_Init(sender As Object, e As EventArgs) Handles Me.Init
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
        If Not Session("codiceArticolodettagli") Is Nothing AndAlso Session("codiceArticolodettagli") <> "" Then
            If Not Session("ClientQueryString") Is Nothing AndAlso Session("ClientQueryString") <> "" Then
                hl_tornaRicerca.NavigateUrl = "~/productFinder.aspx?" & Session("ClientQueryString")
            End If
            Call loadProductData()
        End If
    End Sub

    Private Sub loadProductData()
        Try
            Dim dm As New datamanager
            Dim pdet As DataTable = dm.GetItemDetails(Session("codiceArticolodettagli"))
            If pdet.Rows.Count > 0 Then
                lb_codicearticolo.Text = pdet.Rows(0).Item("CodiceArticolo").ToString
                If pdet.Rows(0).Item("cod_macrolinea") = "0010" Then
                    lb_codicearticolo.Text &= " (Archeopatico)"
                End If
                lb_descrizione.Text = pdet.Rows(0).Item("Descrizione").ToString
                lb_descrizione2.Text = pdet.Rows(0).Item("Descrizione2").ToString
                imgProdotto.ImageUrl = pdet.Rows(0).Item("imgSmallProdotto").ToString
                lb_farmadati.Text = pdet.Rows(0).Item("farmadati").ToString
                lb_EAN.Text = pdet.Rows(0).Item("codEAN").ToString
                lb_confezione.Text = pdet.Rows(0).Item("TipoConfezione").ToString
                lb_formato.Text = pdet.Rows(0).Item("Formato").ToString
                lb_composizione.Text = pdet.Rows(0).Item("Composizione").ToString


                If pdet.Rows(0).Item("grado_alcoolico").ToString <> "" Then
                    lb_grado_alcolico.Text = pdet.Rows(0).Item("grado_alcoolico").ToString
                Else
                    lb_grado_alcolico.Text = "[nd]"
                End If
                If pdet.Rows(0).Item("Dosi").ToString <> "" Then
                    lb_dosi.Text = pdet.Rows(0).Item("Dosi").ToString
                Else
                    lb_dosi.Text = "[nd]"
                End If
                If pdet.Rows(0).Item("bollino").ToString <> "" Then
                    lb_bollino.Text = pdet.Rows(0).Item("bollino").ToString
                Else
                    lb_bollino.Text = "[nd]"
                End If


                If pdet.Rows(0).Item("Obiettivo").ToString <> "" Then
                    lb_obbiettivo.Text = pdet.Rows(0).Item("Obiettivo").ToString & " " & pdet.Rows(0).Item("Obiettivo2").ToString & " " & pdet.Rows(0).Item("Obiettivo3").ToString
                Else
                    lb_obbiettivo.Text = "[nd]"
                End If
                If pdet.Rows(0).Item("Avvertenze").ToString <> "" Then
                    lb_avvertenza.Text = pdet.Rows(0).Item("Avvertenze").ToString & " " & pdet.Rows(0).Item("Avvertenze2").ToString
                Else
                    lb_avvertenza.Text = "[nd]"
                End If

                If pdet.Rows(0).Item("infoaggiuntive").ToString <> "" Then
                    lit_infoaggiuntive.Text = pdet.Rows(0).Item("infoaggiuntive").ToString
                Else
                    lit_infoaggiuntive.Text = "[nd]"
                End If
                If pdet.Rows(0).Item("ingrediente_rilevante").ToString <> "" Then
                    lb_ingrediente_rilevante.Text = pdet.Rows(0).Item("ingrediente_rilevante").ToString
                Else
                    lb_ingrediente_rilevante.Text = "[nd]"
                End If

                If pdet.Rows(0).Item("info_commerciali").ToString <> "" Then
                    lit_info_commerciali.Text = pdet.Rows(0).Item("info_commerciali").ToString
                Else
                    lit_info_commerciali.Text = "[nd]"
                End If
                If pdet.Rows(0).Item("ModoUso").ToString <> "" Then
                    lb_modouso.Text = pdet.Rows(0).Item("ModoUso").ToString
                Else
                    lb_modouso.Text = "[nd]"
                End If
                If pdet.Rows(0).Item("Ingredienti").ToString <> "" Then
                    lit_ingredienti.Text = pdet.Rows(0).Item("Ingredienti").ToString
                Else
                    lit_ingredienti.Text = "[nd]"
                End If
                If pdet.Rows(0).Item("Legenda").ToString <> "" Then
                    lb_legenda.Text = pdet.Rows(0).Item("Legenda").ToString
                Else
                    lb_legenda.Text = "[nd]"
                End If

                If pdet.Rows(0).Item("addettisettore").ToString <> "" Then
                    lit_addettisettore.Text = pdet.Rows(0).Item("addettisettore").ToString
                Else
                    lit_addettisettore.Text = "[nd]"
                End If

                If pdet.Rows(0).Item("bollini_info").ToString <> "" Then
                    lit_bollini_info.Text = pdet.Rows(0).Item("bollini_info").ToString
                Else
                    lit_bollini_info.Visible = False
                End If


                If pdet.Rows(0).Item("imgTabella").ToString <> "" Then
                    ASPxImage_tabella.ImageUrl = pdet.Rows(0).Item("imgTabella").ToString
                Else
                    linkTabellaValori.Visible = False
                    ASPxFormLayout_proDet.FindItemOrGroupByName("tabellaNutrizionali").Visible = False
                End If

                If pdet.Rows(0).Item("imgNormalProdotto").ToString <> "" Then
                    ASPxPopupControl_largeImg.PopupElementID = "imgsmall"
                    ASPxImage_big.ImageUrl = pdet.Rows(0).Item("imgNormalProdotto").ToString
                End If


                If pdet.Rows(0).Item("senza_Lattosio").ToString <> "" Then
                    imgSenzaLattosio.ImageUrl = pdet.Rows(0).Item("senza_Lattosio").ToString
                Else
                    imgSenzaLattosio.Visible = False
                End If
                If pdet.Rows(0).Item("senza_Glutine").ToString <> "" Then
                    imgSenzaGlutine.ImageUrl = pdet.Rows(0).Item("senza_Glutine").ToString
                Else
                    imgSenzaGlutine.Visible = False
                End If
                If pdet.Rows(0).Item("Vegano").ToString <> "" Then
                    imgVegano.ImageUrl = pdet.Rows(0).Item("Vegano").ToString
                Else
                    imgVegano.Visible = False
                End If

                Dim dispoInCart As Integer = CType(Session("cart"), cartManager).getQtaItemInCart(Session("codiceArticolodettagli"))
                Dim dispo As Integer = dm.GetDisponibilitaProdotti(Session("codiceArticolodettagli"), True).Rows(0).Item("Disponibilita")
                lb_dispoTot.Text = dispo - dispoInCart
                lb_qtaCarrello.Text = dispoInCart
                'If dispo - dispoInCart > 0 Or pdet.Rows(0).Item("cod_macrolinea") = "0010" Then
                '    btn_AddToCart.Enabled = True
                'Else
                '    btn_AddToCart.Enabled = False
                'End If

                'If CType(Session("cart"), cartManager).Header.CODICECLIENTE = "" Or CType(Session("cart"), cartManager).Header.IDCART = 0 Then
                '    btn_AddToCart.Enabled = False
                '    btn_AddToCart.Image.Url = "~/images/cartrow_off.png"
                '    btn_AddToCart.Image.ToolTip = "Selezionare un cliente ed iniziare un ordine prima di poter aggiungere articoli."
                '    tbqta.Enabled = False
                'End If
            End If
            dm = Nothing
        Catch ex As Exception
            'MsgBox(ex.Message)
            Throw New Exception(ex.Message)
        End Try

    End Sub


#Region "Gestione Organi-Effetti"


    Protected Sub ASPxCallbackPanel1_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback
        'Dim sql As String = "SELECT id_Livello2,nome_Livello2, valore_Livello2 from [t_organi_effetti] WHERE ([CodiceArticolo] =@CodiceArticolo) and id_Livello2>0 and id_Livello1=@idlivello1 GROUP BY id_Livello2,nome_Livello2,valore_Livello2"
        'SqlDataSource_organi.SelectCommand = sql
        Dim dm As New DataManager
        Dim strApparato As String = dm.GetOrgEffLivello1_Name(e.Parameter)
        headerText = "Apparato " & strApparato
        dm = Nothing

        SqlDataSource_organi.SelectParameters("CodiceArticolo").DefaultValue = Session("codiceArticolodettagli")
        SqlDataSource_organi.SelectParameters("idlivello1").DefaultValue = e.Parameter
        Session("idOrgEffLivello1") = e.Parameter
        Dim data As IEnumerable = SqlDataSource_organi.Select(DataSourceSelectArguments.Empty)
        Dim dataCount As Integer = 0
        For Each row As DataRowView In data
            dataCount += 1
        Next row
        If dataCount > 0 Then
            ASPxGridView_organi.DataSource = data
        Else
            ASPxGridView_organi.DataSource = getEmptyLivello2Datatable(strApparato)
        End If
        ASPxGridView_organi.DataBind()
    End Sub

    Private Function getEmptyLivello2Datatable(ByVal description As String)
        'id_Livello2,nome_Livello2, valore_Livello2
        Dim dt As New DataTable
        Dim dc As DataColumn = New DataColumn() With {.ColumnName = "id_Livello2", .DataType = Type.GetType("System.Int32")}
        dt.Columns.Add(dc)
        dc = New DataColumn() With {.ColumnName = "nome_Livello2", .DataType = Type.GetType("System.String")}
        dt.Columns.Add(dc)
        dc = New DataColumn() With {.ColumnName = "valore_Livello2", .DataType = Type.GetType("System.Int32")}
        dt.Columns.Add(dc)
        Dim newrow As DataRow = dt.NewRow
        newrow("id_Livello2") = 0
        newrow("nome_Livello2") = description
        newrow("valore_Livello2") = 100
        dt.Rows.Add(newrow)
        Return dt
    End Function

    Private headerText As String = Nothing
    Protected Sub ASPxCallbackPanel1_CustomJSProperties(sender As Object, e As DevExpress.Web.CustomJSPropertiesEventArgs) Handles ASPxCallbackPanel1.CustomJSProperties
        If Not headerText Is Nothing Then
            e.Properties.Add("cpHeaderText", headerText)
        End If
    End Sub

    'Protected Sub WebChartControl1_CustomDrawSeriesPoint(ByVal sender As Object, ByVal e As DevExpress.XtraCharts.Web.CustomDrawSeriesPointEventArgs) Handles WebChartControl1.CustomDrawSeriesPoint
    '    Dim point As DevExpress.XtraCharts.SeriesPoint = e.SeriesPoint
    '    If point IsNot Nothing Then
    '        Dim rowView As DataRowView = CType(point.Tag, DataRowView)
    '        Dim s As String = rowView("nome_Livello1").ToString()
    '        e.LabelText = s
    '        e.Series.ToolTipPointPattern = "{V}%"
    '    End If
    'End Sub


    Protected Sub ASPxGridView_sintomi_BeforePerformDataSelect(sender As Object, e As EventArgs)
        Session("idOrgEffLivello2") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
    End Sub





#End Region

End Class