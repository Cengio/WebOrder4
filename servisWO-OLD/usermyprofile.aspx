<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="usermyprofile.aspx.vb" Inherits="servisWO.usermyprofile" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.dxbButton_Metropolis
{
	color: #333333;
	vertical-align: middle;
	border: 1px solid #c0c0c0;
	background-color: white;
	padding: 1px;
	cursor: pointer;
	font: normal 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
}

.dxbButton_Metropolis
{
	color: #333333;
	vertical-align: middle;
	border: 1px solid #c0c0c0;
	background-color: white;
	padding: 1px;
	cursor: pointer;
	font: normal 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
}
    </style>

    <script type="text/javascript" language="javascript">
        function saveCallbackComplete(s, e) {
            //popupesito.SetHeaderText(titolo);
            popupesito.SetContentHTML(s.cp_esito);
            popupesito.Show();
        }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#0099FF" 
    Text="profilo utente" 
    Theme="MetropolisBlue">
    </dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_save" runat="server" 
        ClientInstanceName="callbackpanelsave" Theme="Metropolis" Width="100%">
        <ClientSideEvents EndCallback="saveCallbackComplete" />
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxFormLayout ID="ASPxFormLayout_webuser" runat="server" ColCount="3" 
        EnableTheming="True" Theme="Metropolis" Width="100%">
                    <Items>
                        <dx:LayoutItem Caption="User ID">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer1" runat="server" 
                        SupportsDisabledAttribute="True">
                                    <dx:ASPxLabel ID="lb_userid" runat="server" Font-Bold="True" Font-Size="Medium">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Data creazione">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                    <dx:ASPxLabel ID="lb_dataCreazione" runat="server">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Data ultimo login">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                    <dx:ASPxLabel ID="lb_dataLastLogin" runat="server">
                                    </dx:ASPxLabel>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Password">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer ID="LayoutItemNestedControlContainer2" runat="server" 
                        SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_password" runat="server" Width="100%" Password="True" 
                                        Enabled="False">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                            <RequiredField ErrorText="Campo obbligatorio" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem ColSpan="2">
                        </dx:EmptyLayoutItem>
                        <dx:LayoutItem Caption="Cognome">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_cognome" runat="server" Width="100%" Enabled="False">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                            <RequiredField ErrorText="Campo obbligatorio" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Nome">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_nome" runat="server" Width="100%" Enabled="False">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                            <RequiredField ErrorText="Campo obbligatorio" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="E-Mail">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                                    <dx:ASPxTextBox ID="tb_email" runat="server" Width="100%" Enabled="False">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                            <RequiredField ErrorText="Campo obbligatorio" IsRequired="True" />
                                            <RegularExpression ErrorText="E-Mail non valida" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Tipo utente">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                                    <dx:ASPxComboBox ID="comboTipoUtente" runat="server" SelectedIndex="0" 
                            Width="100%" Enabled="False">
                                        <Items>
                                            <dx:ListEditItem Selected="True" Text="Operatore" Value="2" />
                                            <dx:ListEditItem Text="Agente" Value="1" />
                                            <dx:ListEditItem Text="Magazziniere" Value="3" />
                                            <dx:ListEditItem Text="Amministratore" Value="0" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Bloccato">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                                    <dx:ASPxCheckBox ID="cbox_Bloccato" runat="server" CheckState="Unchecked" 
                                        Enabled="False">
                                    </dx:ASPxCheckBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:EmptyLayoutItem>
                        </dx:EmptyLayoutItem>
                        <dx:EmptyLayoutItem ColSpan="3">
                        </dx:EmptyLayoutItem>
                    </Items>
                    <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
                </dx:ASPxFormLayout>
                <table style="width: 100%; border: 1px solid #4E4F51; margin-top: 10px;" 
                    width="100%">
                    <tr>
                        <td align="right" style="text-align: right; width: 95%;">
                            &nbsp;</td>
                        <td style="text-align: right; width: 5%;">
                            <dx:ASPxButton ID="btn_Salva" runat="server" AutoPostBack="False" 
                                Enabled="False" Text="Salva" Theme="Metropolis" Width="180px">
                                <ClientSideEvents Click="function(s, e) {if(ASPxClientEdit.ValidateEditorsInContainer(null)) callbackpanelsave.PerformCallback();}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
