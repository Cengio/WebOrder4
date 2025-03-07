<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="orderNew.aspx.vb" Inherits="servisWO.orderNew" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

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
        .style2
        {
            height: 19px;
        }
        </style>
</asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
         <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#0099FF" 
    Text="nuovo ordine" 
    Theme="MetropolisBlue">
</dx:ASPxLabel>
     </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    // <![CDATA[
    var postponedCallbackRequired = false;

    function OnCheckSpeTrasportoChange(s, e) {
        CallbackPanelspeTrasporto.PerformCallback();
    }

    function OnAfterPopUpResizing() {
        gridDestinazioni.AdjustControl();
    }

    function RecieveGridDestinazioniValues(values) {
        CallbackPanelDest.PerformCallback(values);
    }

    var lastTipoInterazione = null;
    function OnTipoInterazioneChanged(comboOrderType) {
        if (comboAgente.InCallback()) {
            lastTipoInterazione = comboOrderType.GetValue().toString();
        }
        else {
            comboAgente.PerformCallback(comboOrderType.GetValue().toString());
        }
        var tipointeraz = comboOrderType.GetValue();
        if (tipointeraz != null && comboProfiliSconto.GetValue() != null) {
            gridPromoHeader.PerformCallback(tipointeraz.toString() + '|' + comboProfiliSconto.GetValue().toString());
        }
    }

    function OnComboAgenteEndCallback(s, e) {
        if (lastTipoInterazione) {
            comboAgente.PerformCallback(lastTipoInterazione);
            lastTipoInterazione = null;
        }
    }
    function OnAgenteChanged(s,e) {
        popupControlli.SetContentHTML("L'ordine sarà attibuito all'agente <b>" + s.GetText().toString() + "</b>.");
        popupControlli.Show();
    }

    function OncomboProfiliScontoChanged(s, e) {
        if (comboOrderType.GetValue() != null && comboProfiliSconto.GetValue() != null) {
            gridPromoHeader.PerformCallback(comboOrderType.GetValue().toString() + '|' + comboProfiliSconto.GetValue().toString());
        }
    }
        // ]]> 
    

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
        Theme="MetropolisBlue" Width="360px" CloseAction="None" ShowCloseButton="False">
        <ContentStyle Font-Size="11pt">
        </ContentStyle>
        <FooterContentTemplate>
            <dx:ASPxButton ID="btnYES" runat="server" OnClick="btnYES_Click" Text="Lista clienti">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnNo" runat="server" OnClick="btnNo_Click" Text="Ordine/Carrello">
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
    
    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
        EnableTheming="True" Theme="MetropolisBlue" Width="100%">
        <TabPages>
            <dx:TabPage Name="tb_orderHeader" Text="Dati attività">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table class="dxflInternalEditorTable_BlackGlass">
                            <tr>
                                <td style="width: 15%">
                                    <dx:ASPxLabel ID="lb_DataOrdine" runat="server" Style="font-weight: 700" Text="Data inserimento"></dx:ASPxLabel></td>
                                <td style="width: 15%">
                                    <dx:ASPxLabel ID="lb_DataEvasione" runat="server" Style="font-weight: 700" Text="Data evasione"></dx:ASPxLabel></td>
                                <td style="width: 25%">
                                    Tipo interazione</td>
                                <td style="width: 25%">Agente</td>
                                <td style="width: 5%">
                                    &nbsp;</td>
                                <td style="width: 20%" rowspan="2">
                                    <dx:ASPxCallbackPanel ID="CallbackPanel_speTrasporto" runat="server" 
                                        ClientInstanceName="CallbackPanelspeTrasporto" Width="200px" 
                                        Visible="False">
                                        <PanelCollection>
                                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                <dx:ASPxCheckBox ID="cb_speseTrasporto" runat="server" CheckState="Unchecked" 
                                                    Text="Applica spese di trasporto" Theme="BlackGlass">
                                                    <ClientSideEvents CheckedChanged="OnCheckSpeTrasportoChange" />
                                                </dx:ASPxCheckBox>
                                                <dx:ASPxLabel ID="lb_speseTrasporto" runat="server" style="font-weight: 700">
                                                </dx:ASPxLabel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxDateEdit ID="comboOrderDate" runat="server" Theme="MetropolisBlue">
                                    </dx:ASPxDateEdit>
                                </td>
                                <td>
                                    <dx:ASPxDateEdit ID="comboOrderEvasione" runat="server" Theme="MetropolisBlue">
                                    </dx:ASPxDateEdit>
                                </td>
                                <td>
                                    <dx:ASPxComboBox ID="comboOrderType" runat="server" SelectedIndex="0" Theme="MetropolisBlue" ClientInstanceName="comboOrderType" Width="200px">
                                        <Items>
                                            <dx:ListEditItem Selected="True" Text="Telefonata in Ingresso" 
                                                Value="Telefonata in Ingresso" />
                                            <dx:ListEditItem Text="Telefonata in Uscita" Value="Telefonata in Uscita" />
                                            <dx:ListEditItem Text="Ordine via Fax" Value="Ordine via Fax" />
                                            <dx:ListEditItem Text="Ordine via Email" Value="Ordine via Email" />
                                            <dx:ListEditItem Text="Visita Agente" Value="Visita Agente" />
                                        </Items>
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnTipoInterazioneChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    <dx:ASPxComboBox ID="comboAgente" runat="server" ClientInstanceName="comboAgente" Width="200px">
                                        <ClientSideEvents EndCallback="OnComboAgenteEndCallback" SelectedIndexChanged="function(s, e) { OnAgenteChanged(s,e); }"/>
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    Note</td>
                                <td>
                                    &nbsp;</td>
                                <td>&nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <dx:ASPxTextBox ID="tb_note" runat="server" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
        <Paddings Padding="10px" />
    </dx:ASPxPageControl>

    <dx:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" 
        EnableTheming="True"  Theme="MetropolisBlue" 
        Width="100%">
        <TabPages>
            <dx:TabPage Text="Dati Fatturazione">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table class="dxflInternalEditorTable_BlackGlass">
                            <tr>
                                <td style="width: 20%">
                                    Codice Cliente</td>
                                <td style="width: 20%">
                                    Partita IVA</td>
                                <td style="width: 20%">
                                    Codice fiscale</td>
                                <td style="width: 20%">
                                    &nbsp;</td>
                                <td style="width: 20%">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxTextBox ID="tb_no" runat="server" ReadOnly="True" Width="100%">
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
                                    &nbsp;</td>
                                <td>
                                    <dx:ASPxCheckBox ID="cb_bloccato" runat="server" CheckState="Unchecked" 
                                        ReadOnly="True" Text="[ragione blocco]" Visible="False">
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
                                    <dx:ASPxTextBox ID="tb_nome" runat="server" ReadOnly="True" Width="100%">
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
                                    <dx:ASPxTextBox ID="tb_cap" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_citta" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_provincia" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_Regione" runat="server" ReadOnly="True" Width="100%">
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
                                    <dx:ASPxTextBox ID="tb_telefono" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_fax" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_cellulare" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_email" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>

    <dx:ASPxPageControl ID="ASPxPageControl3" runat="server" ActiveTabIndex="0" 
        EnableTheming="True"  Theme="MetropolisBlue" 
        Width="100%">
        <TabPages>
            <dx:TabPage Text="Dati Spedizione">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxCallbackPanel ID="CallbackPanel_Dest" runat="server" 
                            ClientInstanceName="CallbackPanelDest" Width="100%">
                            <PanelCollection>
                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                    <table class="dxflInternalEditorTable_BlackGlass">
                                        <tr>
                                            <td style="width: 20%">
                                                Destinatario merce</td>
                                            <td colspan="3" style="width: 20%">
                                                &nbsp;</td>
                                            <td style="width: 20%">
                                                &nbsp;</td>
                                            <td rowspan="2" style="width: 20%">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
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
                                        </tr>
                                        <tr>
                                            <td>
                                                Nome</td>
                                            <td>
                                                &nbsp;</td>
                                            <td colspan="3">
                                                Indirizzo</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="tb_desNome" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="tb_desIndirizzo" runat="server" ReadOnly="True" 
                                                    Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CAP</td>
                                            <td colspan="3">
                                                Città</td>
                                            <td>
                                                Provincia</td>
                                            <td>
                                                Regione</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desCap" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="3">
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
                                        </tr>
                                        <tr>
                                            <td>
                                                Telefono</td>
                                            <td colspan="3">
                                                Fax</td>
                                            <td>
                                                Email</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desTelefono" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="tb_desFax" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desEmail" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Corriere da utilizzare</td>
                                            <td colspan="3" class="style2">
                                                </td>
                                            <td class="style2">
                                                </td>
                                            <td class="style2">
                                                </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxComboBox ID="comboCorrieri" runat="server" Theme="Aqua" Width="100%">
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td colspan="3">
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
        </TabPages>
    </dx:ASPxPageControl>

    <dx:ASPxPageControl ID="ASPxPageControl4" runat="server" ActiveTabIndex="0" 
        EnableTheming="True"  Theme="MetropolisBlue" 
        Width="100%">
        <TabPages>
            <dx:TabPage Text="Dati Pagamento">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table class="dxflInternalEditorTable_BlackGlass">
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
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>

    <dx:ASPxPageControl ID="ASPxPageControl5" runat="server" ActiveTabIndex="0" 
        EnableTheming="True"  Theme="MetropolisBlue" 
        Width="100%">
        <TabPages>
            <dx:TabPage Text="Sconti su totale ordine">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table class="dxflInternalEditorTable_BlackGlass" style="width: 100%">
                            <tr>
                                <td>
                                    
                                    <dx:ASPxGridView ID="gridPromoHeader" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridPromoHeader" KeyFieldName="ID" Theme="MetropolisBlue" Width="100%">
                                        <Templates>
                                            <TitlePanel>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="text-align: left">
                                                            <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Profilo di sconto">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxComboBox ID="comboProfiliSconto" runat="server" ClientInstanceName="comboProfiliSconto" ValueType="System.Int32" Width="280px" OnInit="comboProfiliSconto_Init">
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {OncomboProfiliScontoChanged(s,e);}" />
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </TitlePanel>
                                        </Templates>
                                        <SettingsPager Visible="False">
                                        </SettingsPager>
                                        <Settings ShowTitlePanel="True" />
                                        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
                                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="ID" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="PROMO_CODE" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Codice contatto" FieldName="CT_CODE" ShowInCustomizationForm="True" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Importo Ordine MIN" FieldName="TOTAL_ORDER_MIN" ShowInCustomizationForm="True" VisibleIndex="3">
                                                <PropertiesTextEdit DisplayFormatString="C2">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Importo Ordine Max" FieldName="TOTAL_ORDER_MAX" ShowInCustomizationForm="True" VisibleIndex="4">
                                                <PropertiesTextEdit DisplayFormatString="C2">
                                                </PropertiesTextEdit>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Sconto %" FieldName="DISCOUNT_PERCENT" ShowInCustomizationForm="True" VisibleIndex="5">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Valida dal" FieldName="PROMO_DATA_INIZIO" ShowInCustomizationForm="True" VisibleIndex="6">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Scadenza" FieldName="PROMO_DATA_FINE" ShowInCustomizationForm="True" VisibleIndex="7">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowInCustomizationForm="True" Visible="False" VisibleIndex="8" Width="45px">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="btnApplica" Visibility="AllDataRows">
                                                        <Image IconID="actions_apply_32x32" ToolTip="Applica promozione">
                                                        </Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowInCustomizationForm="True" Visible="False" VisibleIndex="9" Width="45px">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="btnRimuovi" Visibility="AllDataRows">
                                                        <Image IconID="actions_cancel_32x32" ToolTip="Rimuovi promozione">
                                                        </Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                        </Columns>
                                        <Styles>
                                            <Header HorizontalAlign="Center">
                                            </Header>
                                            <Cell HorizontalAlign="Center">
                                            </Cell>
                                        </Styles>
                                    </dx:ASPxGridView>
                                    
                                </td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>

    <table style="width: 100%; margin-top: 10px; border-top-style: solid; border-top-width: 1px; border-top-color: #4E4F51;" 
    width="100%">
        <tr>
            <td align="right" style="text-align: right; width: 95%;">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton runat="server" Text="Crea ORDINE" Theme="MetropolisBlue"
                                Width="280px" ID="btn_NewOrder" Font-Size="12pt">
                            </dx:ASPxButton>
                        </td>
                         <td style="width:10px;"></td>
                        <td>
                            <dx:ASPxButton runat="server" Text="Crea ORDINE PRENOTAZIONE" Theme="MetropolisBlue"
                                Width="280px" ID="btn_NewOrderPrenotazione" Font-Size="12pt">
                            </dx:ASPxButton>
                        </td>
                        <td style="width:10px;"></td>
                        <td>
                            <dx:ASPxButton runat="server" Text="Crea CARRELLO/PREVENTIVO" Theme="MetropolisBlue"
                                Width="280px" ID="btn_NewCart" Font-Size="12pt">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="text-align: right; width: 5%;">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="text-align: right; width: 95%;">
                &nbsp;</td>
            <td style="text-align: right; width: 5%;">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="text-align: right; width: 95%;">
                &nbsp;</td>
            <td style="text-align: right; width: 5%;">
                &nbsp;</td>
        </tr>
</table>

</asp:Content>
