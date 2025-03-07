<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="productDetails.aspx.vb" Inherits="servisWO.productDetails" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pcX, pxY;
        function preventEnterKey(htmlEvent) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            }
        }

        function OnLinkValoriClick() {
            var top = tabellaValori.GetMainElement().getBoundingClientRect().top;
            splitter.GetPaneByName("contentPane").SetScrollTop(top - 20);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <table cellpadding="0" cellspacing="0" class="dxflInternalEditorTable_SoftOrange">
        <tr>
            <td>
                <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True"
                    Font-Size="Medium" ForeColor="#0099FF"
                    Text="dettagli articolo"
                    Theme="MetropolisBlue">
                </dx:ASPxLabel>
            </td>
            <td style="text-align: right">
                <dx:ASPxHyperLink ID="hl_tornaRicerca" runat="server" Text="torna ai risultati della ricerca" Theme="MetropolisBlue" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxFormLayout ID="ASPxFormLayout_proDet" runat="server" ColCount="2" EnableTheming="True" Theme="MetropolisBlue" Width="100%">
        <Items>
            <dx:LayoutItem RowSpan="12" ShowCaption="False" Width="20%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxImage ID="imgProdotto" runat="server" ShowLoadingImage="True"
                            ClientInstanceName="imgsmall" Cursor="pointer">
                            <ClientSideEvents Click="function(s, e) {popupimgbig.Show();}" />
                        </dx:ASPxImage>
                        <br />
                        <br />
                        <dx:ASPxHyperLink ID="linkTabellaValori" runat="server" ClientInstanceName="linkTabellaValori" Cursor="pointer" Text="Tabella valori nutrizionali">
                            <ClientSideEvents Click="OnLinkValoriClick" />
                        </dx:ASPxHyperLink>
                        <br />
                        <br />
                        <dx:ASPxImage ID="imgSenzaLattosio" runat="server" ShowLoadingImage="True">
                        </dx:ASPxImage>
                        <br />
                        <br />
                        <dx:ASPxImage ID="imgSenzaGlutine" runat="server" ShowLoadingImage="True">
                        </dx:ASPxImage>
                        <br />
                        <br />
                        <dx:ASPxImage ID="imgVegano" runat="server" ShowLoadingImage="True">
                        </dx:ASPxImage>
                        <br />
                        <br />
                        <asp:Literal ID="lit_bollini_info" runat="server"></asp:Literal>
                        <br />
                        <br />
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutGroup Caption="Dettagli articolo" ColCount="4" Width="80%">
                <Items>
                    <dx:LayoutItem ColSpan="2" Caption="Codice articolo">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_codicearticolo" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ColSpan="2" HorizontalAlign="Right" ShowCaption="False" Caption="Carrello">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_addToCart" runat="server"
                                    ClientInstanceName="cbaddtocart" Width="100%">
                                    <ClientSideEvents EndCallback="function(s, e) {
	CallBMasterCart.PerformCallback();
}"></ClientSideEvents>
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                            <table style="text-align: right;">
                                                <tr>
                                                    <td style="text-align: right; vertical-align: middle">Disponibilità<br />
                                                    </td>
                                                    <td style="text-align: left; vertical-align: middle">
                                                        <dx:ASPxLabel ID="lb_dispoTot" runat="server">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                    <td rowspan="3">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right; vertical-align: middle;">Quantità carrello</td>
                                                    <td style="text-align: left; vertical-align: middle;">
                                                        <dx:ASPxLabel ID="lb_qtaCarrello" runat="server">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Descrizione" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_descrizione" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Descrizione 2" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_descrizione2" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Codice farmadati">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_farmadati" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Codice EAN" ColSpan="2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_EAN" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Formato">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_formato" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Confezione">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_confezione" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Composizione">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_composizione" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Grado alcolico">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_grado_alcolico" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Dosi">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">

                                <dx:ASPxLabel ID="lb_dosi" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Bollino">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_bollino" runat="server" Font-Bold="True">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:EmptyLayoutItem>
                    </dx:EmptyLayoutItem>
                    <dx:LayoutItem Caption="Obiettivo" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_obbiettivo" runat="server" Font-Bold="False"
                                    EncodeHtml="False">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Avvertenza" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_avvertenza" runat="server" Font-Bold="False"
                                    EncodeHtml="False">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Informazioni aggiuntive" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <asp:Literal ID="lit_infoaggiuntive" runat="server"></asp:Literal>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Ingrediente rilevante" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_ingrediente_rilevante" runat="server" EncodeHtml="False"
                                    Font-Bold="False">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Info commerciali" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <asp:Literal ID="lit_info_commerciali" runat="server"></asp:Literal>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Modo d'uso" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_modouso" runat="server" Font-Bold="False"
                                    EncodeHtml="False">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Ingredienti" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <asp:Literal ID="lit_ingredienti" runat="server"></asp:Literal>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Legenda" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_legenda" runat="server" EncodeHtml="False"
                                    Font-Bold="False">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Addetti settore" ColSpan="4">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <asp:Literal ID="lit_addettisettore" runat="server"></asp:Literal>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
                <SettingsItemCaptions HorizontalAlign="Right" Location="Left" VerticalAlign="Top" />
                <SettingsItems HorizontalAlign="Left" VerticalAlign="Top" />
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="Apparati - Organi - Effetti" VerticalAlign="Top" Name="chartApparati">
                <Items>
                    <dx:LayoutItem Caption="apparati chart" ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:WebChartControl ID="WebChartControl1" runat="server"
                                    ClientInstanceName="chartapparati" CrosshairEnabled="True"
                                    DataSourceID="SqlDataSource_apparati" Height="360px"
                                    SideBySideEqualBarWidth="True" Width="560px">
                                    <FillStyle>
                                        <optionsserializable>
                                            <dx:SolidFillOptions></dx:SolidFillOptions>
                                        </optionsserializable>
                                    </FillStyle>
                                    <CrosshairOptions>
                                        <commonlabelpositionserializable>
                                            <dx:CrosshairMousePosition ></dx:CrosshairMousePosition>
                                        </commonlabelpositionserializable>
                                    </CrosshairOptions>
                                    <ToolTipOptions>
                                        <tooltippositionserializable>
                                            <dx:ToolTipMousePosition></dx:ToolTipMousePosition>
                                        </tooltippositionserializable>
                                    </ToolTipOptions>
                                    <EmptyChartText Text="[non disponibile]" />
                                    <BorderOptions Visible="False"></BorderOptions>
                                    <DiagramSerializable>
                                        <dx:SimpleDiagram EqualPieSize="False" LabelsResolveOverlappingMinIndent="10"></dx:SimpleDiagram>
                                    </DiagramSerializable>
                                    <Legend EquallySpacedItems="False">
                                        <shadow visible="True"></shadow>
                                    </Legend>
                                    <SeriesSerializable>
                                        <dx:Series ArgumentDataMember="id_Livello1" ArgumentScaleType="Numerical"
                                            LabelsVisibility="True" LegendText="Apparati" Name="Apparati"
                                            SeriesPointsSorting="Descending" SeriesPointsSortingKey="Value_1"
                                            SynchronizePointOptions="False" ToolTipEnabled="True"
                                            ValueDataMembersSerializable="valore_Su_Livello1" ShowInLegend="False">
                                            <viewserializable>
                                                <dx:DoughnutSeriesView ExplodedDistancePercentage="15" ExplodeMode="MaxValue" 
                                                    RuntimeExploding="False">
                                                </dx:DoughnutSeriesView>
                                            </viewserializable>
                                            <labelserializable>
                                                <dx:DoughnutSeriesLabel Font="Tahoma, 9pt" LineVisible="True" 
                                                    ResolveOverlappingMode="Default">
                                                    <shadow visible="True" />
                                                    <border visible="False" />
<Shadow Visible="True"></Shadow>
<Border Visible="False"></Border>
                                                    <fillstyle>
                                                        <optionsserializable>
                                                            <dx:SolidFillOptions />
                                                        </optionsserializable>
                                                    </fillstyle>
                                                    <linestyle thickness="2" />

<LineStyle Thickness="2"></LineStyle>
                                                    <pointoptionsserializable>
                                                        <dx:PiePointOptions PercentOptions-PercentageAccuracy="2">
                                                        <ValueNumericOptions Format="Percent" Precision="0"></ValueNumericOptions>
                                                        </dx:PiePointOptions>
                                                    </pointoptionsserializable>
                                                </dx:DoughnutSeriesLabel>
                                            </labelserializable>
                                            <legendpointoptionsserializable>
                                                <dx:PiePointOptions PercentOptions-PercentageAccuracy="2">
                                                <ValueNumericOptions Format="Percent" Precision="0"></ValueNumericOptions>
                                                </dx:PiePointOptions>
                                            </legendpointoptionsserializable>
                                        </dx:Series>
                                    </SeriesSerializable>
                                    <SeriesTemplate>
                                        <viewserializable>
                                            <dx:DoughnutSeriesView RuntimeExploding="False">
                                            </dx:DoughnutSeriesView>
                                        </viewserializable>
                                        <labelserializable>
                                            <dx:DoughnutSeriesLabel LineVisible="True">
                                                <fillstyle>
                                                    <optionsserializable>
                                                        <dx:SolidFillOptions />
                                                    </optionsserializable>
                                                </fillstyle>
                                                <pointoptionsserializable>
                                                    <dx:PiePointOptions>
                                                <ValueNumericOptions Format="General"></ValueNumericOptions>
                                                    </dx:PiePointOptions>
                                                </pointoptionsserializable>
                                            </dx:DoughnutSeriesLabel>
                                        </labelserializable>
                                        <legendpointoptionsserializable>
                                            <dx:PiePointOptions>
                                         <ValueNumericOptions Format="General"></ValueNumericOptions>
                                            </dx:PiePointOptions>
                                        </legendpointoptionsserializable>
                                    </SeriesTemplate>
                                    <Titles>
                                        <dx:ChartTitle Font="Tahoma, 12pt" Indent="0" Text="Apparati interessati" />
                                        <dx:ChartTitle Font="Tahoma, 9pt"
                                            Text="(clicca sull'apparato per visualizzare gli organi/sintomi)" />
                                    </Titles>
                                    <ClientSideEvents ObjectSelected="function(s, e) {
                                        
                                         if (e.hitInfo.inSeries) {
	                                       var obj = e.additionalHitObject;
	                                       if (obj != null){
		                                       pcX = e.absoluteX;
		                                       pcY = e.absoluteY;
		                                       cbp.PerformCallback(obj.argument);
		                                     }
                                          }
                                    }"></ClientSideEvents>
                                </dx:WebChartControl>

                                <br />
                                <br />

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup Caption="Tabella valori nutrizionali" VerticalAlign="Top" Name="tabellaNutrizionali">
                <Items>
                    <dx:LayoutItem ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server"
                                SupportsDisabledAttribute="True">
                                <dx:ASPxImage ID="ASPxImage_tabella" runat="server" ShowLoadingImage="True" ClientInstanceName="tabellaValori">
                                </dx:ASPxImage>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
                <SettingsItems HorizontalAlign="Left" VerticalAlign="Top" />
            </dx:LayoutGroup>
        </Items>
        <SettingsItems VerticalAlign="Top" HorizontalAlign="Left"></SettingsItems>
        <SettingsItemCaptions Location="Top" VerticalAlign="Bottom"></SettingsItemCaptions>
    </dx:ASPxFormLayout>

    <asp:SqlDataSource ID="SqlDataSource_apparati" runat="server"
        ConnectionString="<%$ ConnectionStrings:WORconnectionString %>"
        SelectCommand="SELECT id_Livello1,nome_Livello1, valore_Su_Livello1 FROM [t_organi_effetti] WHERE ([CodiceArticolo] = @CodiceArticolo) GROUP BY id_Livello1, nome_Livello1, valore_Su_Livello1">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="" Name="CodiceArticolo" SessionField="codiceArticolodettagli" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource_organi" runat="server"
        ConnectionString="<%$ ConnectionStrings:WORconnectionString %>"
        SelectCommand="SELECT id_Livello2,nome_Livello2, valore_Livello2 FROM [t_organi_effetti] WHERE ([CodiceArticolo] =@CodiceArticolo) and id_Livello2>0 and id_Livello1=@idlivello1 GROUP BY id_Livello2,nome_Livello2,valore_Livello2 order by valore_Livello2 DESC">
        <SelectParameters>
            <asp:SessionParameter Name="CodiceArticolo" SessionField="codiceArticolodettagli" Type="String" />
            <asp:SessionParameter Name="idlivello1" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource_sintomi" runat="server"
        ConnectionString="<%$ ConnectionStrings:WORconnectionString %>"
        SelectCommand="SELECT id_StrDesc,Sintomo,valore,nome_Classe FROM [t_organi_effetti] WHERE [CodiceArticolo]=@CodiceArticolo and id_Livello1=@idlivello1 and id_Livello2=@idlivello2 GROUP BY id_StrDesc,Sintomo,nome_Classe,valore ORDER BY Sintomo,nome_Classe">
        <SelectParameters>
            <asp:SessionParameter Name="CodiceArticolo" SessionField="codiceArticolodettagli" Type="String" />
            <asp:SessionParameter Name="idlivello1" SessionField="idOrgEffLivello1" Type="Int32" />
            <asp:SessionParameter Name="idlivello2" SessionField="idOrgEffLivello2" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server"
        ClientInstanceName="pc"  Theme="MetropolisBlue"
        Width="420px" HeaderText="Apparato" AllowDragging="True"
        AllowResize="True" AppearAfter="500" AutoUpdatePosition="True"
        Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter">
        <HeaderStyle Font-Size="Large" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server"
                    ClientInstanceName="cbp" Width="100%" Theme="MetropolisBlue">
                    <ClientSideEvents EndCallback="function(s, e) {
	        pc.SetHeaderText(s.cpHeaderText);
            pc.Show();
        }" />
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView ID="ASPxGridView_organi" runat="server"
                                AutoGenerateColumns="False" Theme="MetropolisBlue" Width="100%"
                                KeyFieldName="id_Livello2">
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="id_Livello2" FieldName="id_Livello2"
                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="nome_Livello2"
                                        ShowInCustomizationForm="True" VisibleIndex="1" Width="80%">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="valore_Livello2"
                                        ShowInCustomizationForm="True" VisibleIndex="2" Width="20%">
                                        <CellStyle HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowSort="False" />
                                <SettingsPager Visible="False">
                                </SettingsPager>
                                <Settings ShowColumnHeaders="False" ShowGroupButtons="False" />
                                <SettingsText EmptyDataRow="(Nessun organo specifico trovato)" />
                                <SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />
                                <Templates>
                                    <DetailRow>
                                        <dx:ASPxGridView ID="ASPxGridView_sintomi" runat="server"
                                            DataSourceID="SqlDataSource_sintomi" EnableTheming="True"
                                            Theme="MetropolisBlue" Width="100%"
                                            OnBeforePerformDataSelect="ASPxGridView_sintomi_BeforePerformDataSelect"
                                            AutoGenerateColumns="False" KeyFieldName="id_StrDesc">
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="id_StrDesc" Visible="False"
                                                    VisibleIndex="0">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Descrizione sintomo" FieldName="Sintomo"
                                                    VisibleIndex="1" Width="70%">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Classe - Valore" VisibleIndex="2"
                                                    Width="30%">
                                                    <DataItemTemplate>
                                                        <dx:ASPxLabel ID="lb_nomeClasse" runat="server" Text='<%# Eval("nome_Classe") %>'>
                                                        </dx:ASPxLabel>
                                                        <dx:ASPxRatingControl ID="ASPxRatingControl_valoreClasse" runat="server"
                                                            Theme="MetropolisBlue" ReadOnly="True" Value='<%# Eval("valore") %>'>
                                                        </dx:ASPxRatingControl>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Center">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsPager NumericButtonCount="4" PageSize="5">
                                            </SettingsPager>
                                            <Settings ShowColumnHeaders="False" />
                                            <SettingsText EmptyDataRow="(nessun sintomo trovato)" />
                                        </dx:ASPxGridView>
                                    </DetailRow>
                                </Templates>
                            </dx:ASPxGridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="ASPxPopupControl_tabella" runat="server"
         AllowDragging="True" AllowResize="True"
        AutoUpdatePosition="True" ClientInstanceName="popuptabella"
        HeaderText="Valori nutrizionali" Modal="True"
        Theme="MetropolisBlue" BackColor="Gray" DragElement="Window"
        ShowHeader="False">
        <HeaderStyle Font-Size="Medium" VerticalAlign="Middle" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="ASPxPopupControl_largeImg" runat="server"
        AllowDragging="True" AllowResize="True" AutoUpdatePosition="True"
        ClientInstanceName="popupimgbig" DragElement="Window"
        EnableTheming="True" 
        ShowHeader="False" Theme="MetropolisBlue" BackColor="Gray" Modal="True">
        <ClientSideEvents PopUp="function(s, e) {
	s.UpdatePosition();
}" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxImage ID="ASPxImage_big" runat="server" ShowLoadingImage="True">
                </dx:ASPxImage>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

</asp:Content>
