<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="servisWO.login" %>

<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 350px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; margin: 0px; padding: 0px;">
        <table class="style1" align="center" style="width: 100%; text-align: center">
            <tr>
                <td style="height: 400px; font-variant: small-caps;" align="center">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <dx:ASPxFormLayout ID="ASPxFormLayout_login" runat="server" ColCount="2" 
                        EnableTheming="True" Theme="MetropolisBlue">
                        <Items>
                            <dx:LayoutItem ColSpan="2" HorizontalAlign="Center" ShowCaption="False" 
                                VerticalAlign="Bottom">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxImage ID="ASPxFormLayout1_E6" runat="server" 
                                            ImageUrl="~/images/login_headerpng.png" ShowLoadingImage="True">
                                        </dx:ASPxImage>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem RowSpan="6" ShowCaption="False" VerticalAlign="Top">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <br />
                                        <br />
                                        <dx:ASPxImage ID="ASPxFormLayout1_E5" runat="server" Height="96px" 
                                            ImageUrl="~/images/Keys_login.png" ShowLoadingImage="True">
                                        </dx:ASPxImage>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Username" HorizontalAlign="Left">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxTextBox runat="server" Width="170px" Theme="MetropolisBlue" 
                                            ID="tb_userid">
<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
<RequiredField IsRequired="True"></RequiredField>
</ValidationSettings>
</dx:ASPxTextBox>

                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Password" HorizontalAlign="Left">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxTextBox ID="tb_password" runat="server" Password="True" 
                                            Theme="MetropolisBlue" Width="170px">
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Company" HorizontalAlign="Left">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxComboBox ID="comboCompany" runat="server" Theme="MetropolisBlue">
                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem ShowCaption="False" Visible="true" HorizontalAlign="Left" 
                                VerticalAlign="Middle">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxLabel ID="lb_failReason" runat="server">
                                        </dx:ASPxLabel>

                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem HorizontalAlign="Left" ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxCaptcha ID="captchaLogin" runat="server" CodeLength="4" 
                                            Theme="MetropolisBlue">
                                            <ValidationSettings ErrorText="Il codice inserito non è corretto." 
                                                SetFocusOnError="True">
                                                <RequiredField ErrorText="Il codice è obbligatorio" IsRequired="True" />
                                            </ValidationSettings>
                                            <TextBox LabelText="Digita il codice visualizzato sotto" Position="Top" />

<ChallengeImage ForegroundColor="#000000"></ChallengeImage>
                                        </dx:ASPxCaptcha>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem HorizontalAlign="Right" ShowCaption="False">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxButton ID="btn_login" runat="server" Text="Accedi" 
                                            Theme="MetropolisBlue">
                                        </dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:EmptyLayoutItem ColSpan="2">
                            </dx:EmptyLayoutItem>
                            <dx:LayoutItem ColSpan="2" HorizontalAlign="Center" ShowCaption="False" 
                                VerticalAlign="Bottom">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server" 
                                        SupportsDisabledAttribute="True">
                                        <dx:ASPxLabel ID="lb_version" runat="server">
                                        </dx:ASPxLabel>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                                <BorderTop BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" />
                            </dx:LayoutItem>
                        </Items>
                        <SettingsItems HorizontalAlign="Left" />
                        <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
                    </dx:ASPxFormLayout>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: center; font-size: 12px; font-family: Arial; color: #808080;" id="tdBrowser" runat="server">
                    Attenzione: ogni tentativo di accesso verrà monitorato<br />
                    (Indirizzo IP rilevato: <b>
                    <asp:Label ID="Lb_ip" runat="server"></asp:Label>
                    </b>)<br />
                    (Sistem: <b>
                    <asp:Label ID="Lb_browser" runat="server"></asp:Label>
                    </b>) </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
        HeaderText="Sistema in manutenzione" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
       Theme="Metropolis">
        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
    Impossibile raggiungere il database.</dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
    </form>
</body>
</html>
