<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/pro/produzione.Master" CodeBehind="proConfirmPrenotazioni.aspx.vb" Inherits="servisWO.proConfirmPrenotazioni" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function OnEndCallback(s, e) {
            var result = gridOrders.cpshowpopup;
            if (result == "1") {
                var titolo = gridOrders.cptitolo;
                var contenuto = gridOrders.cpcontenuto;
                popupControlli.SetHeaderText(titolo);
                popupControlli.SetContentHTML(contenuto);
                popupControlli.Show();
            }
        }
    </script>

    <dx:ASPxPopupControl ID="popupControlli" runat="server"
        ClientInstanceName="popupControlli" HeaderText="Attenzione" Height="180px"
        Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" 
        Theme="MetropolisBlue" Width="360px">
        <ContentStyle Font-Size="11pt">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxGridView ID="gridOrders" ClientInstanceName="gridOrders" runat="server" Theme="BlackGlass" AutoGenerateColumns="False" Width="100%">
        <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
        <SettingsPager PageSize="20">
        </SettingsPager>
        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" />
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        <Columns>
            <dx:GridViewDataTextColumn Caption="Ordine" FieldName="Code" VisibleIndex="0" Width="120px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Data evasione richiesta" FieldName="DATA_EVASIONE" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="UTENTE_ULTIMA_MODIFICA" FieldName="UTENTE_ULTIMA_MODIFICA" Visible="False" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Cliente" FieldName="CODICECLIENTE" VisibleIndex="2" Width="140px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Articoli e quantità da produrre" VisibleIndex="3">
                <DataItemTemplate>
                    <dx:ASPxGridView ID="gridItems" runat="server" AutoGenerateColumns="False" OnInit="gridItems_Init" Theme="Glass" Width="100%">
                        <SettingsPager Mode="ShowAllRecords" Visible="False">
                        </SettingsPager>
                        <Settings GridLines="Horizontal" ShowColumnHeaders="True" />
                        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="Codice" FieldName="ItemCode" VisibleIndex="0" Width="140px">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Decrizione" FieldName="DESCRIZIONE" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Qta da produrre" FieldName="OriginalQty" VisibleIndex="2" Width="80px">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Giacenza" VisibleIndex="3" Width="80px">
                                <DataItemTemplate>
                                    <dx:ASPxLabel ID="lbGiacenza" runat="server" OnInit="lbGiacenza_Init" Text="0">
                                    </dx:ASPxLabel>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Button" Caption=" " VisibleIndex="4" Width="140px">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnConferma" Text="CONFERMA" Visibility="AllDataRows" >
                        <Image AlternateText="Conferma produzione" Height="34px" IconID="actions_apply_32x32" ToolTip="Conferma Produzione">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
        <Styles>
            <Cell HorizontalAlign="Left" VerticalAlign="Top">
            </Cell>
        </Styles>
    </dx:ASPxGridView>


</asp:Content>
