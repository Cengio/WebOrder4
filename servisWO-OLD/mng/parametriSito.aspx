<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="parametriSito.aspx.vb" Inherits="servisWO.parametriSito" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.dxeBase_Metropolis
{
	font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
	color: #333333;
}
    </style>

    <script type="text/javascript" language="javascript">
//        function saveCallbackComplete(s, e) {
//            //popupesito.SetHeaderText(titolo);
//                popupesito.SetContentHTML(s.cp_esito);
//                popupesito.Show();
//            }

            function saveCallbackComplete(s, e) {
                var result = e.result.split('|');
                var titolo = result[0];
                var contenuto = result[1];
                var taberror = parseInt(result[2]);
                popupesito.SetHeaderText(titolo);
                popupesito.SetContentHTML(contenuto);
                popupesito.Show();
                switch (taberror) {
                    case 1: { tabpagecontrol.SetActiveTab(tabpagecontrol.GetTabByName('promozioni')); break; }
                    case 2: { tabpagecontrol.SetActiveTab(tabpagecontrol.GetTabByName('numeratori')); break; }
                    case 3: { tabpagecontrol.SetActiveTab(tabpagecontrol.GetTabByName('magazzino')); break; }
                    case 4: { tabpagecontrol.SetActiveTab(tabpagecontrol.GetTabByName('ordini')); break; }
                    //case 5: { tabpagecontrol.SetActiveTab(tabpagecontrol.GetTabByName('scontipagamento')); break; }
                }
            }

            function ActiveTabChanged(e) {
                //var tab = tabpagecontrol.GetTabByName(cbTabs.GetValue());
                if (e.tab.name == "scontipagamento")
                    btnSalvaParametri.SetEnabled(false)
                else
                    btnSalvaParametri.SetEnabled(true)
            }

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#FF8800" 
    Text="gestione parametri sito" 
    Theme="Metropolis">
    </dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="5" 
        EnableTheming="True" Theme="Metropolis" 
        Width="100%" ClientInstanceName="tabpagecontrol">
        <TabPages>
            <dx:TabPage Text="Ordini - Notifiche" Name="ordini">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxFormLayout ID="ASPxFormLayout5" runat="server" EnableTheming="True" 
                            Theme="Metropolis" Width="100%">
                            <Items>
                                <dx:LayoutGroup Caption="E-mail Sistema" ColCount="3">
                                    <Items>
                                        <dx:LayoutItem Caption="E-mail sistema">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailSistema" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="E-Mail non corretta" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Descrizione e-mail di sistema">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailSistemaDesc" runat="server" Width="440px">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutGroup Caption="Attivazione Notifiche" ColCount="2">
                                    <Items>
                                        <dx:LayoutItem Caption="Attiva notifiche" ColSpan="2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxCheckBox ID="cb_attivaNotificheEmail" runat="server" CheckState="Unchecked">
                                                    </dx:ASPxCheckBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Invia notifica per revisione e controllo ordine">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxCheckBox ID="cb_attivaNotificaRevisioneOrdine" runat="server" CheckState="Unchecked">
                                                    </dx:ASPxCheckBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Invia notifica di creazione nuovo cliente">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxCheckBox ID="cb_attivaNotificaNuovoCliente" runat="server" CheckState="Unchecked">
                                                    </dx:ASPxCheckBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Invia notifica di chiusura ordine">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxCheckBox ID="cb_attivaNotificaNuovoOrdine" runat="server" CheckState="Unchecked">
                                                    </dx:ASPxCheckBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Invia notifica di chiusura ordine al cliente">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxCheckBox ID="cb_attivaNotificaOrdineAlCliente" runat="server" CheckState="Unchecked">
                                                    </dx:ASPxCheckBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                    <SettingsItemCaptions Location="Left" />
                                </dx:LayoutGroup>
                                <dx:LayoutGroup Caption="Email notifiche" ColCount="3">
                                    <Items>
                                        <dx:LayoutItem Caption="E-mail per notifica ordini">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaOrdini" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="E-Mail non corretta" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica ordini 2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaOrdini2" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica ordini 3">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaOrdini3" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica nuovi clienti">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaNuoviClienti" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="E-Mail non corretta" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica nuovi clienti 2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaNuoviClienti2" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica nuovi clienti 3">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaNuoviClienti3" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" 
                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica revisione ordine">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaRevisioneOrdini" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="E-Mail non corretta" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica revisione ordine 2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaRevisioneOrdini2" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail per notifica revisione ordine 3">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailNotificaRevisioneOrdini3" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail richiesta produzione">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailRichiestaProduzione" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                            <RegularExpression ErrorText="E-Mail non corretta" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail richiesta produzione 2">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailRichiestaProduzione2" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="E-mail richiesta produzione 3">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tb_emailRichiestaProduzione3" runat="server" Width="220px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RegularExpression ErrorText="E-Mail non corretta" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutItem Caption="Percorso salvataggio PDF ordini">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="tb_percorsoPDFordini" runat="server" Width="260px">
                                                <ValidationSettings Display="Dynamic">
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                            <SettingsItemCaptions Location="Top" />

                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Name="parametriordini" Text="Ordini - Evasione">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxFormLayout ID="gridMailingOrdini" runat="server" Theme="Metropolis" 
                            Width="100%">
                            <Items>
                                <dx:LayoutGroup Caption="Valori di default" ColCount="3">
                                    <Items>
                                        <dx:LayoutItem Caption="Importo evadibilità ordine">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxSpinEdit ID="spinEvadibilita" runat="server" DecimalPlaces="2" 
                                                        Height="21px" Number="0">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Importo porto franco">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxSpinEdit ID="spinPortoFranco" runat="server" DecimalPlaces="2" 
                                                        Height="21px" Number="0">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Importo spese trasporto">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server" 
                                                    SupportsDisabledAttribute="True">
                                                    <dx:ASPxSpinEdit ID="spinSpeseTrasporto" runat="server" DecimalPlaces="2" 
                                                        Height="21px" Number="0">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutItem Caption="Valori per mailing group (Parametri Globali)">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxGridView ID="gridParametriOrdini" runat="server" 
                                                AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
                                                EnableTheming="True" KeyFieldName="Code" 
                                                Theme="Metropolis">
                                                <Columns>
                                                    <dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" VisibleIndex="0" ShowEditButton="True"/>
                                                    <dx:GridViewDataTextColumn FieldName="Code" ReadOnly="True" 
                                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Descrizione" FieldName="Description" 
                                                        ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataSpinEditColumn Caption="Importo evadibilità ordine" 
                                                        FieldName="importoEvadibilitaOrdine" ShowInCustomizationForm="True" 
                                                        VisibleIndex="3">
                                                        <PropertiesSpinEdit DecimalPlaces="2" DisplayFormatString="g">
                                                            <ValidationSettings>
                                                                <RequiredField IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesSpinEdit>
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataSpinEditColumn Caption="Importo porto franco" 
                                                        FieldName="importoPortoFranco" ShowInCustomizationForm="True" 
                                                        VisibleIndex="4">
                                                        <PropertiesSpinEdit DecimalPlaces="2" DisplayFormatString="g">
                                                            <ValidationSettings>
                                                                <RequiredField IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesSpinEdit>
                                                    </dx:GridViewDataSpinEditColumn>
                                                    <dx:GridViewDataSpinEditColumn Caption="Importo spese spedizione" 
                                                        FieldName="importoSpeseSpedizione" ShowInCustomizationForm="True" 
                                                        VisibleIndex="5">
                                                        <PropertiesSpinEdit DecimalPlaces="2" DisplayFormatString="g">
                                                            <ValidationSettings>
                                                                <RequiredField IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesSpinEdit>
                                                    </dx:GridViewDataSpinEditColumn>
                                                </Columns>
                                                <SettingsEditing Mode="Inline" />
                                            </dx:ASPxGridView>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:WORconnectionString %>" 
                                                SelectCommand="SELECT * FROM [parametriEvadibilitaOrdine]" 
                                                UpdateCommand="UPDATE [parametriEvadibilitaOrdine] SET [Description] = @Description, [importoEvadibilitaOrdine] = @importoEvadibilitaOrdine, [importoPortoFranco] = @importoPortoFranco, [importoSpeseSpedizione] = @importoSpeseSpedizione WHERE [Code] = @Code">
                                                <UpdateParameters>
                                                    <asp:Parameter Name="Description" Type="String" />
                                                    <asp:Parameter Name="importoEvadibilitaOrdine" Type="Decimal" />
                                                    <asp:Parameter Name="importoPortoFranco" Type="Decimal" />
                                                    <asp:Parameter Name="importoSpeseSpedizione" Type="Decimal" />
                                                    <asp:Parameter Name="Code" Type="String" />
                                                </UpdateParameters>
                                            </asp:SqlDataSource>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                            <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Magazzino" Name="magazzino">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxFormLayout ID="ASPxFormLayout4" runat="server" EnableTheming="True" 
                            Theme="Metropolis">
                            <Items>
                                <dx:LayoutItem Caption="Magazzino operativo">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxComboBox ID="combo_magazzini" runat="server" Width="380px">
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                            <SettingsItemCaptions Location="Top" />

                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Numeratori" Name="numeratori">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxFormLayout ID="ASPxFormLayout3" runat="server" EnableTheming="True" 
                            Theme="Metropolis" Width="100%">
                            <Items>
                                <dx:LayoutItem Caption="Progressivo ordini web">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="tb_progOrdini" runat="server" Width="170px">
                                                <ValidationSettings SetFocusOnError="True">
                                                    <RegularExpression ErrorText="Inserire un numero intero" 
                                                        ValidationExpression="^[0-9]+$" />
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Progressivo nuovo cliente (Parametro Globale)">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxTextBox ID="tb_progNewCliente" runat="server" Width="170px">
                                                <ValidationSettings SetFocusOnError="True">
                                                    <RegularExpression ErrorText="Inserire un numero intero" 
                                                        ValidationExpression="^[0-9]+$" />
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                            <SettingsItemCaptions Location="Top" />

                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Promozioni" Name="promozioni">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxFormLayout ID="ASPxFormLayout2" runat="server" EnableTheming="True" 
                            Theme="Metropolis" Width="100%">
                            <Items>
                                <dx:LayoutItem Caption="Fascia di utilizzo">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxComboBox ID="comboFascia" runat="server">
                                                <Items>
                                                    <dx:ListEditItem Text="A" Value="A" />
                                                    <dx:ListEditItem Text="B" Value="B" />
                                                    <dx:ListEditItem Text="C" Value="C" />
                                                    <dx:ListEditItem Text="D" Value="D" />
                                                    <dx:ListEditItem Text="E" Value="E" />
                                                    <dx:ListEditItem Text="F" Value="F" />
                                                    <dx:ListEditItem Text="G" Value="G" />
                                                    <dx:ListEditItem Text="H" Value="H" />
                                                    <dx:ListEditItem Text="I" Value="I" />
                                                    <dx:ListEditItem Text="L" Value="L" />
                                                    <dx:ListEditItem Text="M" Value="M" />
                                                    <dx:ListEditItem Text="N" Value="N" />
                                                    <dx:ListEditItem Text="O" Value="O" />
                                                    <dx:ListEditItem Text="P" Value="P" />
                                                    <dx:ListEditItem Text="Q" Value="Q" />
                                                    <dx:ListEditItem Text="R" Value="R" />
                                                    <dx:ListEditItem Text="S" Value="S" />
                                                    <dx:ListEditItem Text="T" Value="T" />
                                                    <dx:ListEditItem Text="U" Value="U" />
                                                    <dx:ListEditItem Text="V" Value="V" />
                                                    <dx:ListEditItem Text="Z" Value="Z" />
                                                </Items>
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                                <dx:LayoutItem Caption="Stato promozioni SAS">
                                    <LayoutItemNestedControlCollection>
                                        <dx:LayoutItemNestedControlContainer runat="server" 
                                            SupportsDisabledAttribute="True">
                                            <dx:ASPxComboBox ID="comboSASstatus" runat="server" SelectedIndex="0" 
                                                ValueType="System.Int32">
                                                <Items>
                                                    <dx:ListEditItem Selected="True" Text="Attivo" Value="1" />
                                                    <dx:ListEditItem Text="NON attivo" Value="0" />
                                                </Items>
                                            </dx:ASPxComboBox>
                                        </dx:LayoutItemNestedControlContainer>
                                    </LayoutItemNestedControlCollection>
                                </dx:LayoutItem>
                            </Items>
                            <SettingsItemCaptions Location="Top" />

                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Name="scontipagamento" Text="Ordini - Sconti pagamento">
                     <ContentCollection>
                            <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                                     <dx:ASPxFormLayout ID="formLayoutScontiPagamento" runat="server" Theme="Metropolis"  Width="100%">
                                            <Items>
                                                <dx:LayoutItem Caption="" CaptionSettings-HorizontalAlign="Left" CaptionSettings-VerticalAlign="Top">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                                                            <dx:ASPxGridView ID="gridScontiPagamento" runat="server"
                                                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceScontiPagamento"
                                                                EnableTheming="True" KeyFieldName="CustomerNo;PaymentMethodCode;PaymentTermsCode"
                                                                Theme="Metropolis">
                                                                <Settings ShowFilterRow="True" />
                                                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" VisibleIndex="0" ShowEditButton="True" ShowClearFilterButton="True" />
                                                                    <dx:GridViewDataTextColumn FieldName="CustomerNo" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" VisibleIndex="1"></dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="PaymentMethodCode" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" VisibleIndex="2">
                                                                        <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="PaymentTermsCode" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" VisibleIndex="3">
                                                                        <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn FieldName="Description" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" VisibleIndex="4">
                                                                        <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataCheckColumn FieldName="Disabled" ReadOnly="True" ShowInCustomizationForm="True" Visible="True" VisibleIndex="5">
                                                                        <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                                                                    </dx:GridViewDataCheckColumn>
                                                                    <dx:GridViewDataSpinEditColumn Caption="Sconto pagamento" FieldName="DiscountPerc" ShowInCustomizationForm="True" VisibleIndex="6">
                                                                        <PropertiesSpinEdit DecimalPlaces="2" DisplayFormatString="g">
                                                                            <ValidationSettings>
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </PropertiesSpinEdit>
                                                                        <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                                                                    </dx:GridViewDataSpinEditColumn>       
                                                                     </Columns>
                                                                <SettingsEditing Mode="Inline" />
                                                            </dx:ASPxGridView>
                                                            <asp:SqlDataSource ID="SqlDataSourceScontiPagamento" runat="server"
                                                                ConnectionString="<%$ ConnectionStrings:WORconnectionString %>"
                                                                SelectCommand="SELECT * FROM [parametriScontoPagamenti]"
                                                                UpdateCommand="UPDATE [parametriScontoPagamenti] SET [DiscountPerc] = @DiscountPerc WHERE [CustomerNo] = @CustomerNo AND [PaymentMethodCode] = @PaymentMethodCode AND [PaymentTermsCode] = @PaymentTermsCode">
                                                                <UpdateParameters>
                                                                    <asp:Parameter Name="DiscountPerc" Type="Decimal" />
                                                                    <asp:Parameter Name="CustomerNo" Type="String" />
                                                                    <asp:Parameter Name="PaymentMethodCode" Type="String" />
                                                                    <asp:Parameter Name="PaymentTermsCode" Type="String" />
                                                                </UpdateParameters>
                                                            </asp:SqlDataSource>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

<CaptionSettings HorizontalAlign="Left" VerticalAlign="Top"></CaptionSettings>
                                                </dx:LayoutItem>
                                            </Items>
                                     </dx:ASPxFormLayout>
                            </dx:ContentControl>
                     </ContentCollection>
            </dx:TabPage>
        </TabPages>
        <ClientSideEvents ActiveTabChanged="function(s, e) {
	ActiveTabChanged(e);
}" />
    </dx:ASPxPageControl>

&nbsp;<table style="width: 100%; border: 1px solid #4E4F51; margin-top: 10px;" 
                            width="100%">
        <tr>
            <td align="right" style="text-align: right; width: 95%;">
                                    &nbsp;</td>
            <td style="text-align: right; width: 5%;">
                <dx:ASPxButton runat="server" Text="Salva Parametri" ID="btn_Salva" Theme="Metropolis" ClientInstanceName="btnSalvaParametri"
                                    Width="180px" AutoPostBack="False">
                    <ClientSideEvents Click="function(s, e) {if(ASPxClientEdit.ValidateEditorsInContainer(null)) callbacksave.PerformCallback();}" />
                </dx:ASPxButton>
    
            </td>
        </tr>
    </table>

    <dx:ASPxPopupControl ID="popup_esito" runat="server" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" 
         AllowDragging="True" AppearAfter="500" AutoUpdatePosition="True" 
        ClientInstanceName="popupesito"  
        Theme="Metropolis" HeaderText="Esito operazione" Width="360px">
        <ContentStyle Font-Size="Medium">
        </ContentStyle>
        <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True"></dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>

    <br />
    <dx:ASPxCallback ID="ASPxCallback_save" runat="server" 
        ClientInstanceName="callbacksave">
        <ClientSideEvents BeginCallback="function(s, e) {
	ASPxLoadingPanel1.Show();
}" EndCallback="function(s, e) {
	ASPxLoadingPanel1.Hide();
}" CallbackComplete="saveCallbackComplete" />
    </dx:ASPxCallback>

    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
        ClientInstanceName="ASPxLoadingPanel1" Modal="True" 
        Text="Salvataggio parametri&amp;hellip;" Theme="Metropolis">
    </dx:ASPxLoadingPanel>

</asp:Content>
