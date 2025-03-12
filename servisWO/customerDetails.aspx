<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="customerDetails.aspx.vb" Inherits="servisWO.customerDetails" %>

<%@ Register assembly="DevExpress.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
.dxgvControl_MetropolisBlue,
.dxgvDisabled_MetropolisBlue
{
	border: 1px Solid #c0c0c0;
	font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
	background-color: White;
	color: #333333;
	cursor: default;
}

.dxgvTitlePanel_MetropolisBlue, 
.dxgvTable_MetropolisBlue caption
{
	font-size: 15px;
	font-weight: normal;
	padding: 3px 3px 5px;
	text-align: center;
	color: #999999;
	border-bottom: 1px Solid #c0c0c0;
}
.dxgvTable_MetropolisBlue
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxgvTable_MetropolisBlue
{
	background-color: White;
	border-width: 0;
	border-collapse: separate!important;
	overflow: hidden;
}
.dxgvHeader_MetropolisBlue
{
	cursor: pointer;
	white-space: nowrap;
	padding: 4px 6px 5px;
	border: 1px Solid #c0c0c0;
	overflow: hidden;
	font-weight: normal;
	text-align: left;
}
.dxgvCommandColumn_MetropolisBlue
{
	padding: 8px 4px;
}
    </style>
</asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
         dettagli cliente
     </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

    // <![CDATA[
    var postponedCallbackRequired = false;
    function OncomboDestIndexChanged(s, e) {
        var item = comboDest.GetSelectedItem();
        if (CallbackPanelDest.InCallback())
            postponedCallbackRequired = true;
        else
            CallbackPanelDest.PerformCallback();
    }
    function OnEndCallback(s, e) {
        if (postponedCallbackRequired) {
            CallbackPanelDest.PerformCallback();
            postponedCallbackRequired = false;
        }
    }
    // ]]> 

    //function ProcessPromoSelection(index, isSelected) {
    //    if (isSelected == "False") {
    //        gridpromo.SelectRows(index);
    //    }
    //    else {
    //        gridpromo.UnselectRows(index);
    //    }
    //}

    function OnAfterPopUpResizing() {
        gridDestinazioni.AdjustControl();
    }

    function RecieveGridDestinazioniValues(values) {
        CallbackPanelDest.PerformCallback(values);
    }

    </script>

    <script type="text/javascript">
        var doProcessClick;
        var visibleIndexCart;
        function ProcessClick() {
            if (doProcessClick) {

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
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True"></dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="popupCambioCarrello" runat="server" 
        ClientInstanceName="popupCambioCarrello" HeaderText="Attenzione" Height="180px" 
        Modal="True" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter"  ShowFooter="True" 
        Theme="MetropolisBlue" Width="360px">
        <ContentStyle Font-Size="11pt">
        </ContentStyle>
        <FooterContentTemplate>
            <dx:ASPxButton ID="btnYES" runat="server" onclick="btnYES_Click" 
                Text="Conferma">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnNo" runat="server" AutoPostBack="False" Text="Annulla">
                <ClientSideEvents Click="function(s, e) {
	popupCambioCarrello.Hide();
}" />
            </dx:ASPxButton>
        </FooterContentTemplate>
        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True"></dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
 

    <dx:ASPxPopupControl ID="popupDestinazioni" runat="server" 
        ClientInstanceName="popupDestinazioni" HeaderText="Destinazioni" 
         Theme="MetropolisBlue" Width="720px" 
        AllowDragging="True" AllowResize="True" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        Height="300px" ScrollBars="Vertical">
        <ClientSideEvents AfterResizing="OnAfterPopUpResizing" />
        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    <dx:ASPxGridView ID="gridDestinazioni" runat="server" 
        ClientInstanceName="gridDestinazioni" Theme="MetropolisBlue" 
        AutoGenerateColumns="False" Width="100%" KeyFieldName="Code">
        <ClientSideEvents RowDblClick="function(s, e) {
	s.GetRowValues(e.visibleIndex, 'Code', RecieveGridDestinazioniValues);
	popupDestinazioni.Hide();
}" />
        <Columns>
            <dx:GridViewDataTextColumn Caption="Codice" FieldName="Code" 
                ShowInCustomizationForm="True" VisibleIndex="0" Width="60px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Destinatario" FieldName="Name" 
                ShowInCustomizationForm="True" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Indirizzo" FieldName="Address" 
                ShowInCustomizationForm="True" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Città" FieldName="City" 
                ShowInCustomizationForm="True" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="CAP" FieldName="Post Code" 
                ShowInCustomizationForm="True" VisibleIndex="4" Width="60px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Prov." FieldName="County" 
                ShowInCustomizationForm="True" VisibleIndex="5" Width="60px">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
        <SettingsPager Mode="ShowAllRecords" Visible="False">
        </SettingsPager>
    </dx:ASPxGridView>
            </dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>


    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="4" 
        EnableTheming="True" Theme="MetropolisBlue" Width="100%">
    <TabPages>
        <dx:TabPage Name="tab_fatturazione" Text="Dati Fatturazione">
            <ContentCollection>
                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                    <table class="dxflInternalEditorTable_BlackGlass">
                        <tr>
                            <td style="width: 20%">
                                Codice Cliente</td>
                            <td style="width: 20%">
                                Codie Contatto</td>
                            <td style="width: 20%">
                                Partita IVA</td>
                            <td style="width: 20%">
                                Codice fiscale</td>
                            <td style="width: 20%">
                                Bloccato</td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="tb_no" runat="server" Width="100%" ReadOnly="true">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_ctno" runat="server" ReadOnly="True" Width="100%">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_piva" runat="server" ReadOnly="True" Width="100%">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_codfis" runat="server" ReadOnly="True" Width="100%">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxCheckBox ID="cb_bloccato" runat="server" CheckState="Unchecked" 
                                    ReadOnly="True">
                                </dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nome</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                Indirizzo</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxTextBox ID="tb_nome" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td colspan="2" style="width: 80%">
                                <dx:ASPxTextBox ID="tb_indirizzo" runat="server" ReadOnly="True" Width="100%">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                CAP</td>
                            <td>
                                Città</td>
                            <td>
                                Provincia</td>
                            <td>
                                Regione</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="tb_cap" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_citta" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_provincia" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_Regione" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                Telefono</td>
                            <td>
                                Fax</td>
                            <td>
                                Cellulare</td>
                            <td>
                                Email</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="tb_telefono" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_fax" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_cellulare" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="tb_email" runat="server" Width="100%" ReadOnly="True">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
        <dx:TabPage Name="tab_spedizione" Text="Dati Spedizione">
            <ContentCollection>
                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                    <dx:ASPxCallbackPanel ID="CallbackPanel_Dest" runat="server" Width="100%" 
                        ClientInstanceName="CallbackPanelDest">
                        <ClientSideEvents EndCallback="OnEndCallback" />
                        <PanelCollection>
                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                <table class="dxflInternalEditorTable_BlackGlass">
                                    <tr>
                                        <td style="width: 20%">
                                            Codice Destinatario </td>
                                        <td colspan="1" style="width: 20%">
                                            &nbsp;</td>
                                        <td style="width: 20%">
                                            &nbsp;</td>
                                        <td style="width: 20%">
                                            &nbsp;</td>
                                        <td style="width: 20%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <dx:ASPxTextBox ID="tb_desCode" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxButton ID="btnDestinazione" runat="server" AutoPostBack="False" 
                                                Text="Destinazione ... " Theme="MetropolisBlue">
                                                <ClientSideEvents Click="function(s, e) {
	gridDestinazioni.PerformCallback();
	popupDestinazioni.Show();
}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td colspan="1">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            Nome</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Indirizzo</td>
                                        <td>
                                            &nbsp;</td>
                                        <td colspan="1">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <dx:ASPxTextBox ID="tb_desNome" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td colspan="2">
                                            <dx:ASPxTextBox ID="tb_desIndirizzo" runat="server" ReadOnly="True" 
                                                Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CAP</td>
                                        <td colspan="1">
                                            Città</td>
                                        <td>
                                            Provincia</td>
                                        <td>
                                            Paese</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxTextBox ID="tb_desCap" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td colspan="1">
                                            <dx:ASPxTextBox ID="tb_desCitta" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="tb_desProvincia" runat="server" ReadOnly="True" 
                                                Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="tb_desRegione" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Telefono</td>
                                        <td colspan="1">
                                            Fax</td>
                                        <td>
                                            Email</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            Corriere preferenziale</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dx:ASPxTextBox ID="tb_desTelefono" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td colspan="1">
                                            <dx:ASPxTextBox ID="tb_desFax" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="tb_desEmail" runat="server" ReadOnly="True" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <dx:ASPxComboBox ID="comboCorrieri" runat="server" Enabled="False" 
                                                ReadOnly="True" Theme="Aqua" Width="100%">
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td colspan="1">
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
        <dx:TabPage Name="tab_pagamento" Text="Dati Pagamento">
            <ContentCollection>
                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                    <table class="dxflInternalEditorTable_BlackGlass">
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxGridView ID="grigliaPagamenti" runat="server" 
                                    AutoGenerateColumns="False" KeyFieldName="Numeratore" Width="500px">
                                    <Columns>
                                        <dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
                                            ShowSelectCheckbox="True" VisibleIndex="0" Width="30px">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="Metodo" ShowInCustomizationForm="True" 
                                            Visible="False" VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Termine" ShowInCustomizationForm="True" 
                                            Visible="False" VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Metodo e termine di pagamento" 
                                            FieldName="Descrizione" ShowInCustomizationForm="True" VisibleIndex="4">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Sconto %" FieldName="Sconto" 
                                            ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
                                            <PropertiesTextEdit DisplayFormatString="F2">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Numeratore" 
                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Predefinito" 
                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior AllowDragDrop="False" AllowGroup="False" 
                                        AllowSelectSingleRowOnly="True" AllowSort="False" />
                                    <SettingsPager Visible="False">
                                    </SettingsPager>
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
        <dx:TabPage Name="tab_carrelli" Text="Carrelli">
            <ContentCollection>
                <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                    <dx:ASPxGridView ID="gridCart" runat="server" AutoGenerateColumns="False" 
                        ClientInstanceName="cartlist" EnableTheming="True" KeyFieldName="IDCART" 
                        Theme="MetropolisBlue" Width="100%">
                        <ClientSideEvents RowClick="function(s, e) {
	doProcessClick = true;
	visibleIndexCart = e.visibleIndex+1;
	window.setTimeout(ProcessClick,500);
}" RowDblClick="function(s, e) {
	doProcessClick = false;
	var key = s.GetRowKey(e.visibleIndex);
	cartlist.PerformCallback(key);
}" />
                        <Columns>
                            <dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="IDCART" ReadOnly="True" 
                                ShowInCustomizationForm="True" VisibleIndex="1" Caption="ID Carrello">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="DESCRIPTION" 
                                ShowInCustomizationForm="True" VisibleIndex="2" Caption="Descrizione">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="CODICECLIENTE" 
                                ShowInCustomizationForm="True" VisibleIndex="3" Caption="Cliente">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="UTENTE_CREAZIONE" 
                                ShowInCustomizationForm="True" VisibleIndex="4" Caption="Creato da">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="DATA_CREAZIONE" 
                                ShowInCustomizationForm="True" VisibleIndex="5" Caption="Data creazione">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="UTENTE_ULTIMA_MODIFICA" 
                                ShowInCustomizationForm="True" VisibleIndex="6" Caption="Modificato da">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="DATA_ULTIMA_MODIFICA" 
                                ShowInCustomizationForm="True" VisibleIndex="7" Caption="Data ultima modifica">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataCheckColumn FieldName="BLOCKED" ShowInCustomizationForm="True" 
                                VisibleIndex="9" Caption="Bloccato" Width="70px">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataTextColumn Caption="Stato" FieldName="Status" ShowInCustomizationForm="True" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="True" />
                        <Settings ShowTitlePanel="True" />
                        <Templates>
                            <TitlePanel>
                                [Doppio click sulla riga per modificare il carrello]
                            </TitlePanel>
                        </Templates>
                    </dx:ASPxGridView>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
    <Paddings Padding="10px" />
</dx:ASPxPageControl>

    <table style="width: 100%; border: 1px solid #4E4F51; margin-top: 10px;" 
    width="100%">
        <tr>
            <td align="right" style="text-align: left; width: 15%;">
                <dx:ASPxButton runat="server" Text="Inizia nuovo ordine ... " Theme="MetropolisBlue" 
                    Width="180px" ID="btn_NewCart">
                </dx:ASPxButton>
            </td>
            <td style="text-align: right; width: 55%;">
                &nbsp;</td>
        </tr>
</table>

</asp:Content>
