<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="userMng.aspx.vb" Inherits="servisWO.userMng" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script language="javascript" type="text/javascript">
        function OnTextBoxGRIDKeyPress(s, e) {
            if (e.htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
            }
        }

        function saveCallbackComplete(s, e) {
            if (s.cp_showpopup == '1') {
                popupesito.SetContentHTML(s.cp_esito);
                popupesito.Show();
                s.cp_showpopup = 0;
            } 
        }
        var passwordMinLength = 8;
        function GetPasswordRating(password) {
            var result = 0;
            if (password) {
                result++;
                if (password.length >= passwordMinLength) {
                    if (/[a-z]/.test(password))
                        result++;
                    if (/[A-Z]/.test(password))
                        result++;
                    if (/\d/.test(password))
                        result++;
                    if (!(/^[a-z0-9]+$/i.test(password)))
                        result++;
                }
            }
            return result;
        }
        function OnPasswordTextBoxInit(s, e) {
            ApplyCurrentPasswordRating();
        }
        function OnPassChanged(s, e) {
            ApplyCurrentPasswordRating();
        }
        function ApplyCurrentPasswordRating() {
            var password = passwordTextBox.GetText();
            var passwordRating = GetPasswordRating(password);
            ApplyPasswordRating(passwordRating);
        }
        function ApplyPasswordRating(value) {
            ratingControl.SetValue(value);
            switch (value) {
                case 0:
                    ratingLabel.SetValue("Password safety");
                    break;
                case 1:
                    ratingLabel.SetValue("Too simple");
                    break;
                case 2:
                    ratingLabel.SetValue("Not safe");
                    break;
                case 3:
                    ratingLabel.SetValue("Normal");
                    break;
                case 4:
                    ratingLabel.SetValue("Safe");
                    break;
                case 5:
                    ratingLabel.SetValue("Very safe");
                    break;
                default:
                    ratingLabel.SetValue("Password safety");
            }
        }

        function GetErrorText(editor) {
            //alert(passwordTextBox.GetText());
            if (gridUsers.IsEditing() == true && gridUsers.IsNewRowEditing() == true) { //nuovo record)
                if (editor.GetText() == '') {
                    return "*"
                }else if (editor === passwordTextBox) {
                    if (ratingControl.GetValue() === 1)
                        return "La password è troppo semplice";
                } else if (editor === confirmPasswordTextBox) {
                    if (passwordTextBox.GetText() !== confirmPasswordTextBox.GetText())
                        return "Le password non coincidono";
                }
                return "";
            } else if (gridUsers.IsEditing() == true && gridUsers.IsNewRowEditing() == false) {//modifica record
                if (editor === passwordTextBox && passwordTextBox.GetText() == '') {
                    return "";
                } else {
                    if (editor === passwordTextBox) {
                        if (ratingControl.GetValue() === 1)
                            return "La password è troppo semplice";
                    } else if (editor === confirmPasswordTextBox) {
                        if (passwordTextBox.GetText() !== confirmPasswordTextBox.GetText())
                            return "Le password non coincidono";
                    }
                }
            }
        }

        function OnPassValidation(s, e) {
            var errorText = GetErrorText(s);
            if (errorText) {
                e.isValid = false;
                e.errorText = errorText;
            }
        }

        </script>
    <style type="text/css">

.dxeBase_Metropolis
{
	font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
	color: #333333;
}

.dxeBase_Metropolis
{
	font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
	color: #333333;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#FF8800" 
    Text="gestione utenti" 
    Theme="MetropolisBlue">
    </dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="gridUsers" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="User Code" Theme="MetropolisBlue" Width="100%" ClientInstanceName="gridUsers">
        <ClientSideEvents EndCallback="saveCallbackComplete" />
        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
        </SettingsEditing>
        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
        <SettingsDataSecurity AllowDelete="False" />
        <SettingsPopup>
            <EditForm HorizontalAlign="WindowCenter" Modal="True" ShowHeader="False" VerticalAlign="WindowCenter" />
        </SettingsPopup>
        <Columns>
            <dx:GridViewCommandColumn ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="140px">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Cognome" FieldName="Cognome" Name="Cognome" VisibleIndex="6">
                <PropertiesTextEdit MaxLength="50">
                    <ValidationSettings CausesValidation="True" Display="Dynamic">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption="Bloccato" FieldName="Blocked" Name="blocked" VisibleIndex="10" Width="40px">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="User Code" Name="Usercode" ReadOnly="True" Visible="False" VisibleIndex="1" Width="40px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Password" FieldName="Password" Name="Password" VisibleIndex="3" Visible="False">
                <PropertiesTextEdit ClientInstanceName="passwordTextBox" Password="True" MaxLength="50" NullText="**********">
                    <ClientSideEvents Init="OnPasswordTextBoxInit" KeyUp="OnPassChanged" Validation="OnPassValidation" />
                    <ValidationSettings CausesValidation="True" Display="Dynamic" EnableCustomValidation="True" ErrorTextPosition="Bottom" SetFocusOnError="True" ErrorDisplayMode="ImageWithText">
                    </ValidationSettings>
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Nome" FieldName="Nome" Name="Nome" VisibleIndex="7">
                <PropertiesTextEdit MaxLength="50">
                    <ValidationSettings CausesValidation="True" Display="Dynamic">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" Name="Email" VisibleIndex="8">
                <PropertiesTextEdit MaxLength="50">
                    <ValidationSettings CausesValidation="True" Display="Dynamic">
                        <RegularExpression ErrorText="Email non valida" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="User ID" FieldName="User ID" Name="Userid" Visible="False" VisibleIndex="2">
                <PropertiesTextEdit MaxLength="50">
                    <ReadOnlyStyle BackColor="#00FFCC" Font-Bold="True">
                    </ReadOnlyStyle>
                    <ValidationSettings CausesValidation="True" Display="Dynamic">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ultimo Accesso" FieldName="Date_LastLogin" Name="Datelastlogin" VisibleIndex="11" Width="100px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Data Creazione" FieldName="Date_Creazione" Name="Datecreazione" VisibleIndex="12" Width="100px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn Caption="Profilo utente" FieldName="idProfile" Name="idProfile" VisibleIndex="9">
                <PropertiesComboBox AllowNull="True" ConvertEmptyStringToNull="False" NullDisplayText="Nessuno">
                    <ClearButton DisplayMode="Always">
                    </ClearButton>
                    <ValidationSettings CausesValidation="True" Display="Dynamic">
                    </ValidationSettings>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataCheckColumn Caption="Invia credenziali via email" Name="inviaEmail" Visible="False" VisibleIndex="13" FieldName="inviaEmail">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn Caption="Conferma Password" FieldName="confermaPassword" Name="confermaPassword" Visible="False" VisibleIndex="4">
                <PropertiesTextEdit ClientInstanceName="confirmPasswordTextBox" Password="True" MaxLength="50" NullText="**********">
                    <ClientSideEvents Validation="OnPassValidation" />
                    <ValidationSettings CausesValidation="True" Display="Dynamic" EnableCustomValidation="True" SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                    </ValidationSettings>
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Sicurezza" Name="ratingControl" ReadOnly="True" Visible="False" VisibleIndex="5">
                <EditFormSettings Visible="True" />
                <EditItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxRatingControl ID="ratingControl" runat="server" ReadOnly="true" ItemCount="5" Value="0" ClientInstanceName="ratingControl" Theme="MetropolisBlue" />
                            </td>
                            <td style="padding-left: 5px; width: 100px">
                                <dx:ASPxLabel ID="ratingLabel" runat="server" ClientInstanceName="ratingLabel" Text="" />
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataMemoColumn Caption="Note" FieldName="Note" Name="Note" Visible="False" VisibleIndex="14">
                <PropertiesMemoEdit ConvertEmptyStringToNull="False">
                </PropertiesMemoEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataMemoColumn>
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxPopupControl ID="popup_esito" runat="server" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" 
         AllowDragging="True" AppearAfter="500" AutoUpdatePosition="True" 
        ClientInstanceName="popupesito"  
        Theme="MetropolisBlue" HeaderText="Esito operazione" Width="360px">
        <ContentStyle Font-Size="Medium">
        </ContentStyle>
        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True"></dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
    </asp:Content>
