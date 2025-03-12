<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="orderImport.aspx.vb" Inherits="servisWO.orderImport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnEndCallback(s, e) {
            var cpOrderImport = grid.cpOrderImport;
            var cpClienteNonTrovato = grid.cpClienteNonTrovato;
            //console.log(cpOrderImport);
            if (cpOrderImport != null && cpOrderImport != undefined && cpOrderImport != "") {
                popupOrdineBloccato.Show();
            }
            if (cpClienteNonTrovato != null && cpClienteNonTrovato != undefined && cpClienteNonTrovato != "") {
                popupClienteNonTrovato.Show();
            }
        }
        function refreshGrid() {
            grid.Refresh();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True"
        Font-Size="Medium" ForeColor="#0099FF"
        Text="lista ordini da importare"
        Theme="MetropolisBlue">
    </dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid" EnableTheming="True" KeyFieldName="OrdineAmazon" Theme="MetropolisBlue" Width="100%">
        <ClientSideEvents EndCallback="function(s, e) {
	OnEndCallback(s, e)
}" />
        <SettingsPager AlwaysShowPager="True" Position="TopAndBottom">
        </SettingsPager>
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        <Columns>
            <dx:GridViewDataTextColumn Caption="Riferimento ordine" FieldName="OrdineAmazon" VisibleIndex="0" Width="200px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Data" FieldName="DataIns" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Stato" FieldName="Importato" VisibleIndex="2" Width="150px" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" Caption="Importa" VisibleIndex="3" Width="70px">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnImport" Visibility="AllDataRows">
                        <Image IconID="support_article_32x32" ToolTip="Importa ordine">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <HeaderStyle HorizontalAlign="Center" />
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewCommandColumn>
        </Columns>
    </dx:ASPxGridView>
    <br />

    <dx:ASPxPopupControl ID="popupOrdineBloccato" runat="server"
        ClientInstanceName="popupOrdineBloccato" Font-Size="Large"
        HeaderText="Attenzione" Height="160px" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Style="text-align: left" Theme="MetropolisBlue"
        Width="400px">
        <ClientSideEvents Closing="function(s, e) {	refreshGrid(); }" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                L&#39;ordine è già stato importato da un altro utente.
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

        <dx:ASPxPopupControl ID="popupClienteNonTrovato" runat="server"
        ClientInstanceName="popupClienteNonTrovato" Font-Size="Large"
        HeaderText="Attenzione" Height="160px" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Style="text-align: left" Theme="MetropolisBlue"
        Width="400px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                Il codice cliente o il codice contatto AMAZON non è stato trovato in Navision. Impossibile proseguire con l&#39;importazione.
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    </asp:Content>
