<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/mag/magazzino.Master" CodeBehind="magRaccogliOrdine.aspx.vb" Inherits="servisWO.magRaccogliOrdine" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">


.dxpcLite_Metropolis .dxpc-header,
.dxdpLite_Metropolis .dxpc-header 
{
	color: #0072C6;
	font-size: 1.66em;
	padding: 7px 2px 7px 12px;
	white-space: nowrap;
}

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" EnableTheming="True" 
        Theme="Office2010Black" Width="720px">
        <Items>
            <dx:LayoutGroup Caption="Raccolta ordine" ColCount="4">
                <Items>
                    <dx:LayoutItem Caption="Visualizza prossimo ordine">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxButton ID="btnCarica" runat="server" Height="24px" Text="Visualizza" 
                                    Theme="Office2010Black" Width="140px">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Inizia raccolta">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxButton ID="btnInizia" runat="server" Height="24px" 
                                    Text="Prendi in carico" Theme="Office2010Black" Width="140px">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Stampa report ordine">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxButton ID="btnStampa" runat="server" AutoPostBack="False" 
                                    CausesValidation="False" EnableTheming="True" Height="24px" Text="Stampa" 
                                    Theme="Office2010Black" Width="140px">
                                    <ClientSideEvents Click="function(s, e) {
	callbackReport.PerformCallback();
}" />
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Sospendi raccolta">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxButton ID="btnSospendi" runat="server" Height="24px" Text="Sospendi" Theme="Office2010Black" Width="140px">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
        <SettingsItems VerticalAlign="Top" />
        <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
    </dx:ASPxFormLayout>
    <dx:ASPxLabel ID="lbAvviso" runat="server" Font-Size="12pt" 
        Theme="Office2010Black">
    </dx:ASPxLabel>
    <br />
    <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" EnableTheming="True" 
        Theme="Office2010Black" Width="100%">
        <Items>
            <dx:LayoutGroup Caption="Dettagli ordine" ColCount="5">
                <Items>
                    <dx:LayoutItem Caption="Nr. ordine">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxLabel ID="lbNrOrdine" runat="server" Font-Size="11pt">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Data">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxLabel ID="lbDataOrdine" runat="server" Font-Size="11pt">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Cliente">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxLabel ID="lbCodiceCliente" runat="server" Font-Size="11pt">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Ragione sociale">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxLabel ID="lbRagioneSociale" runat="server" Font-Size="11pt">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Creato da">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxLabel ID="lbOperatoreCreazione" runat="server" Font-Size="11pt">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
        </Items>
        <SettingsItemCaptions Location="Top" />
    </dx:ASPxFormLayout>
    <br />
    <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" 
        EnableTheming="True" Theme="Office2010Black" Width="100%" 
        ClientInstanceName="grid">
        <Columns>
            <dx:GridViewDataTextColumn Caption="Nr. Linea" FieldName="LineNo" 
                VisibleIndex="0" Width="80px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Codice" FieldName="ItemCode" 
                VisibleIndex="1" Width="100px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Descrizione" FieldName="DESCRIZIONE" 
                VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Lotto" FieldName="LotNo" VisibleIndex="3" 
                Width="80px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Collocazione" FieldName="BinCode" 
                VisibleIndex="4" Width="180px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Quantità" FieldName="OriginalQty" 
                VisibleIndex="5" Width="100px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Prezzo" FieldName="UnitPrice" 
                VisibleIndex="6" Width="100px" Visible="False">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Totale riga" Name="totaleriga" 
                UnboundType="Decimal" VisibleIndex="7" Width="120px" Visible="False">
                <PropertiesTextEdit DisplayFormatString="c">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Omaggio" FieldName="RowDiscount" 
                VisibleIndex="8" Width="100px">
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" />
        <SettingsPager Mode="ShowAllRecords" Visible="False">
        </SettingsPager>
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
            AllowInsert="False" />
    </dx:ASPxGridView>
    <br />
    <dx:ASPxPopupControl ID="popupOrdineOccupato" runat="server" Font-Size="11pt" 
        HeaderText="Ordine occupato" Modal="True" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" Theme="Office2010Black" Width="360px">
        <ContentCollection>
<dx:PopupControlContentControl runat="server">Attenzione l&#39;ordine è stato preso in 
    carico
    <br />
    da un altro operatore.
    <br />
    Passare all&#39;ordine successivo.<br />
    <br />
    <dx:ASPxButton ID="btnOKordineOccupato" runat="server" Text="OK" 
        Theme="Office2010Black" Width="120px">
    </dx:ASPxButton>
            </dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>

            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
        ClientInstanceName="loadingpanel" Modal="True" Theme="MetropolisBlue" 
        Text="Caricamento dati&amp;hellip;">
    </dx:ASPxLoadingPanel>

            <dx:ASPxCallback ID="ASPxCallback_report" runat="server" 
        ClientInstanceName="callbackReport">
                <ClientSideEvents EndCallback="function(s, e) {
	loadingpanel.Hide();
	popupReport.Show();
}" BeginCallback="function(s, e) {
	loadingpanel.Show();
}" />
    </dx:ASPxCallback>

    <dx:ASPxPopupControl ID="popupReport" runat="server" 
        ClientInstanceName="popupReport" ContentUrl="~/orderReportTR.aspx" 
        HeaderText="Report ordine" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="TopSides"  
        Theme="MetropolisBlue" AllowDragging="True" AllowResize="True" 
        AutoUpdatePosition="True" Width="1100px" Height="840px" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

            </asp:Content>
