<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="newcustomerAdd.aspx.vb" Inherits="servisWO.newcustomerAdd" %>
<%@ Register assembly="DevExpress.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnValueChanged(s, e) {
            var combo = ASPxClientGridLookup.Cast(s);
            var grid = ASPxClientGridView.Cast(combo.GetGridView());
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Code;Country Code;State', OnGetRowValues);
        };

        function OnGetRowValues(values) {
            tbcap.SetText(values[0]);
            tbnazione.SetText(values[1]);
            tbprovincia.SetText(values[2]);
        }
        function OnValueChangedDest(s, e) {
            var combo = ASPxClientGridLookup.Cast(s);
            var grid = ASPxClientGridView.Cast(combo.GetGridView());
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Code;Country Code;State', OnGetRowValuesDest);
        };

        function OnGetRowValuesDest(values) {
            tbcapDest.SetText(values[0]);
            tbnazioneDest.SetText(values[1]);
            tbprovinciaDest.SetText(values[2]);
        }

        function saveCallbackComplete(s, e) {
            var result = e.result.split('|');
            var titolo = result[0];
            var contenuto = result[1];
            popupEsito.SetHeaderText(titolo);
            popupEsito.SetContentHTML(contenuto);
            popupEsito.Show();
        }

        function preventEnterKey(htmlEvent) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#0099FF" 
    Text="nuovo cliente" 
    Theme="MetropolisBlue">
</dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxFormLayout ID="ASPxFormLayout_newCustomer" runat="server" 
    EnableTheming="True" Theme="MetropolisBlue" Width="100%">
    <Items>
        <dx:LayoutGroup Caption="Dati Anagrafici" ColCount="5">
            <Items>
                <dx:LayoutItem Caption="Codice nuovo cliente">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer6" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxLabel ID="lb_codiceNewCustomer" runat="server" 
                                Text="N000000" Font-Bold="True" Font-Size="Small">
                            </dx:ASPxLabel>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:EmptyLayoutItem ColSpan="3" Width="80%">
                </dx:EmptyLayoutItem>
                <dx:LayoutItem Caption="Inserito da">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer7" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxLabel ID="lb_username" runat="server">
                            </dx:ASPxLabel>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Categoria clienti">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer8" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxComboBox ID="combo_catClienti" runat="server" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) {
	grigliaPagamenti.PerformCallback(s.GetValue());
}" />
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:EmptyLayoutItem ColSpan="4" Width="40%">
                </dx:EmptyLayoutItem>




                <dx:LayoutItem Caption="Ragione Sociale" ColSpan="2">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer9" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_name" runat="server" Width="100%" MaxLength="50">
                            </dx:ASPxTextBox>
                            <dx:ASPxTextBox ID="tb_name2" runat="server" Width="100%" MaxLength="50">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Natura Giuridica">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer10" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxComboBox ID="combo_naturaGiuridica" runat="server" Width="100%">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Partita Iva">
<LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer11" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_piva" runat="server" Width="100%" MaxLength="20">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Codice Fiscale">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer12" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_codfiscale" runat="server" MaxLength="20" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Indirizzo" ColSpan="4">
                                        <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer13" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_indirizzo" runat="server" Width="100%" MaxLength="50">
                            </dx:ASPxTextBox>
                            <dx:ASPxTextBox ID="tb_indirizzo2" runat="server" MaxLength="50" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Numero Civico">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer17" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_numeroCivico" runat="server" Width="100%" MaxLength="10">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Città">
<LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer14" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxGridLookup ID="lookupCitta" runat="server" AutoGenerateColumns="False" 
                                ClientInstanceName="lookupCitta" DataSourceID="SqlDataSourceCAPPARIO" 
                                IncrementalFilteringDelay="350" IncrementalFilteringMode="StartsWith" 
                                KeyFieldName="Code;City" TextFormatString="{1}" Width="100%">
                                <GridViewProperties>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" />
                                </GridViewProperties>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="Code" ReadOnly="True" 
                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="City" ReadOnly="True" 
                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="City" 
                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="State" ShowInCustomizationForm="True" 
                                        VisibleIndex="3">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Region" ShowInCustomizationForm="True" 
                                        Visible="False" VisibleIndex="4">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Extend State" 
                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Country Code" 
                                        ShowInCustomizationForm="True" VisibleIndex="6">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents ValueChanged="function(s, e) {OnValueChanged(s,e);}" KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}"/>
                            </dx:ASPxGridLookup>
                            <asp:SqlDataSource ID="SqlDataSourceCAPPARIO" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:NAVconnectionString %>" 
                                SelectCommand="SELECT * FROM [Ser-Vis$Post Code]"></asp:SqlDataSource>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="CAP">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer15" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_CAP" runat="server" Width="100%" MaxLength="20" 
                                ClientInstanceName="tbcap" ReadOnly="True">
                                <ReadOnlyStyle BackColor="#DADADA">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Provincia">
                                        <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer16" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_provincia" runat="server" ClientInstanceName="tbprovincia" 
                                MaxLength="30" Width="100%" ReadOnly="True">
                                <ReadOnlyStyle BackColor="#DADADA">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                
                <dx:LayoutItem Caption="Nazione">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer18" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_nazione" runat="server" Width="100%" MaxLength="10" 
                                ClientInstanceName="tbnazione" ReadOnly="True">
                                <ReadOnlyStyle BackColor="#DADADA">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:EmptyLayoutItem Width="20%">
                </dx:EmptyLayoutItem>
                <dx:LayoutItem Caption="Telefono">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer19" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_telefono1" runat="server" Width="100%" MaxLength="30">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Cellulare">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer21" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_telefono2" runat="server" Width="100%" MaxLength="30">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Fax">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer20" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_fax" runat="server" MaxLength="30" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="E-Mail">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer22" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_email" runat="server" MaxLength="80" Width="100%">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:EmptyLayoutItem Width="20%">
                </dx:EmptyLayoutItem>
            </Items>
            <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
            <SettingsItems VerticalAlign="Top" Width="20%" />
        </dx:LayoutGroup>
        <dx:LayoutGroup Caption="Dati Spedizione" ColCount="5">
            <Items>
                <dx:LayoutItem Caption="Destinatario " ColSpan="2">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_nomeDest1" runat="server" Width="100%" MaxLength="50">
                            </dx:ASPxTextBox>
                            <dx:ASPxTextBox ID="tb_nomeDest2" runat="server" Width="100%" MaxLength="50">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Natura Giuridica Destinatario">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxComboBox ID="combo_naturagiuridicaDest" runat="server">
                            </dx:ASPxComboBox>
                        </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:EmptyLayoutItem ColSpan="2">
                </dx:EmptyLayoutItem>
                <dx:LayoutItem Caption="Indirizzo Destinatario" ColSpan="4">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_indirizzoDest" runat="server" Width="100%" 
                                MaxLength="50">
                            </dx:ASPxTextBox>
                            <dx:ASPxTextBox ID="tb_indirizzoDest2" runat="server" Width="100%" 
                                MaxLength="50">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                 <dx:EmptyLayoutItem>
                </dx:EmptyLayoutItem>
                <dx:LayoutItem Caption="Città Destinatario">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxGridLookup ID="lookupCittaDest" runat="server" 
                                AutoGenerateColumns="False" ClientInstanceName="lookupCittaDest" 
                                DataSourceID="SqlDataSourceCAPPARIO2" IncrementalFilteringDelay="350" 
                                IncrementalFilteringMode="StartsWith" KeyFieldName="Code;City" 
                                TextFormatString="{1}" Width="100%">
                                <GridViewProperties>
                                    <SettingsBehavior AllowFocusedRow="True" AllowSelectSingleRowOnly="True" />
                                </GridViewProperties>
                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="Code" ReadOnly="True" 
                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="City" ReadOnly="True" 
                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="City" 
                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="State" ShowInCustomizationForm="True" 
                                        VisibleIndex="3">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Region" ShowInCustomizationForm="True" 
                                        Visible="False" VisibleIndex="4">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Extend State" 
                                        ShowInCustomizationForm="True" Visible="False" VisibleIndex="5">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Country Code" 
                                        ShowInCustomizationForm="True" VisibleIndex="6">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <ClientSideEvents ValueChanged="function(s, e) {
	OnValueChangedDest(s,e);
}" />
                            </dx:ASPxGridLookup>
                            <asp:SqlDataSource ID="SqlDataSourceCAPPARIO2" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:NAVconnectionString %>" 
                                SelectCommand="SELECT * FROM [Ser-Vis$Post Code]"></asp:SqlDataSource>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="CAP Destinatario">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_capDest" runat="server" Width="100%" MaxLength="20" ClientInstanceName="tbcapDest" ReadOnly="True">
                                <ReadOnlyStyle BackColor="#DADADA">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Provincia Destinatario">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_provinciaDest" runat="server" Width="100%" ClientInstanceName="tbprovinciaDest" ReadOnly="True"
                                MaxLength="30">
                                <ReadOnlyStyle BackColor="#DADADA">
                                </ReadOnlyStyle>
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Nazione Destinatario">
                     <LayoutItemNestedControlCollection>
                         <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer4" runat="server" 
                             SupportsDisabledAttribute="True">
                             <dx:ASPxTextBox ID="tb_nazioneDest" runat="server" 
                                 ClientInstanceName="tbnazioneDest" MaxLength="10" ReadOnly="True" Width="100%">
                                 <ReadOnlyStyle BackColor="#DADADA">
                                 </ReadOnlyStyle>
                             </dx:ASPxTextBox>
                         </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                       </dx:LayoutItem>
                <dx:EmptyLayoutItem>
                </dx:EmptyLayoutItem>
                <dx:LayoutItem Caption="Telefono Destinatario">
                                      <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer5" runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_telefonoDest1" runat="server" Width="100%" 
                                MaxLength="30">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Cellulare Destinatario">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer3" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_telefonoDest2" runat="server" Width="100%" 
                                MaxLength="30">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Fax Destinatario">
                   <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_faxDest" runat="server" Width="100%" MaxLength="30">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="E-Mail Destinatario">
                   <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxTextBox ID="tb_emailDest" runat="server" Width="100%" MaxLength="80">
                            </dx:ASPxTextBox>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                   <dx:EmptyLayoutItem>
                </dx:EmptyLayoutItem>
            </Items>
            <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
            <SettingsItems VerticalAlign="Top" Width="20%" />
        </dx:LayoutGroup>
        <dx:LayoutGroup Caption="Pagamento e Dati Bancari">
            <Items>
            <dx:LayoutItem Caption="Pagamento">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView ID="grigliaPagamenti" runat="server" 
                                AutoGenerateColumns="False" ClientInstanceName="grigliaPagamenti" 
                                KeyFieldName="Numeratore" Width="500px">
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
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutGroup Caption="Codice IBAN" ColCount="6" Width="460px">
                    <Items>
                        <dx:LayoutItem Caption="NAZ.">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_naz" runat="server" MaxLength="2" Width="30px" 
                                        ReadOnly="True" Text="IT">
                                        <ReadOnlyStyle BackColor="#DADADA">
                                        </ReadOnlyStyle>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="CD">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_CD" runat="server" MaxLength="2" Width="30px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="CIN">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_cin" runat="server" MaxLength="1" Width="20px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="ABI">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_abi" runat="server" MaxLength="5" Width="50px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="CAB">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_cab" runat="server" MaxLength="5" Width="50px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="CONTO CORRENTE">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                    SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_contocorrente" runat="server" MaxLength="12" 
                                        Width="110px">
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                    <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
                </dx:LayoutGroup>
                
            </Items>
            <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
            <SettingsItems VerticalAlign="Top" />
        </dx:LayoutGroup>
        <dx:LayoutGroup Caption="Comandi" ColCount="5" ShowCaption="False">
            <Items>
                <dx:LayoutItem Caption="Salva Nuovo Cliente">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxButton ID="btn_Salva" runat="server" Text="Salva" 
                                Theme="MetropolisBlue" Width="100%" AutoPostBack="False" 
                                CausesValidation="False">
                                <ClientSideEvents Click="function(s, e) {
	callbackSaveCustomer.PerformCallback();
}" />
                            </dx:ASPxButton>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
                <dx:LayoutItem Caption="Nuovo Ordine">
                    <LayoutItemNestedControlCollection>
                        <dx:LayoutItemNestedControlContainer runat="server" 
                            SupportsDisabledAttribute="True">
                            <dx:ASPxButton ID="btn_NuovoOrdine" runat="server" Enabled="False" 
                                Text="Nuovo Ordine" Theme="MetropolisBlue" Width="100%">
                            </dx:ASPxButton>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                </dx:LayoutItem>
            </Items>
            <SettingsItems ShowCaption="False" Width="20%" />
        </dx:LayoutGroup>
    </Items>
</dx:ASPxFormLayout>

            <dx:ASPxCallback ID="ASPxCallback_saveCustomer" runat="server" 
        ClientInstanceName="callbackSaveCustomer">
                <ClientSideEvents BeginCallback="function(s, e) {
		savingpanel.Show();
}" EndCallback="function(s, e) {
	savingpanel.Hide();
}" CallbackComplete="saveCallbackComplete" />
    </dx:ASPxCallback>

            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel2" runat="server" 
        ClientInstanceName="savingpanel" Text="Salvataggio cliente&amp;hellip;" 
        Theme="MetropolisBlue">
    </dx:ASPxLoadingPanel>

            <dx:ASPxPopupControl ID="popupEsito" runat="server" 
    ClientInstanceName="popupEsito" Font-Size="Large" 
    HeaderText="Esito salvataggio" Height="120px" Modal="True" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
     style="text-align: left" Theme="MetropolisBlue" 
    Width="320px">
                <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    Problemi di connessione al database. Riprovare tra qualche istante.</dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

    <br />

</asp:Content>
