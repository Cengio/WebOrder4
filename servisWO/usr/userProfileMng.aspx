<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="userProfileMng.aspx.vb" Inherits="servisWO.userProfileMng" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function OnTextBoxGRIDKeyPress(s, e) {
            if (e.htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
            }
        }
        function gridProfiles_EndCallback(s, e) {
            if (s.cpShowPopupProfiloInUso)
                popupProfiloInUso.Show();
        }
    </script>
    <style type="text/css">

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
    Text="gestione profili utente" 
    Theme="Metropolis">
    </dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="grid_Profiles" runat="server" AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="idProfile" Theme="Metropolis" Width="100%" ClientInstanceName="gridProfiles">
        <SettingsPager Visible="False">
        </SettingsPager>
        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
        </SettingsEditing>
        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
        <SettingsPopup>
            <EditForm HorizontalAlign="WindowCenter" Modal="True" ShowHeader="False" VerticalAlign="WindowCenter" />
        </SettingsPopup>
        <Columns>
            <dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="140px">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Nome Profilo" FieldName="description" Name="description" VisibleIndex="2" Width="240px">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" Display="Dynamic" ErrorTextPosition="Left">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption="Bloccato" FieldName="blocked" Name="blocked" VisibleIndex="4" Width="40px">
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="idProfile" Name="idProfile" ReadOnly="True" Visible="False" VisibleIndex="1" Width="40px">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ruoli" FieldName="roleStringList" Name="roleList" VisibleIndex="3">
                <EditItemTemplate>
                    <dx:ASPxCheckBoxList ID="cbRuoli" runat="server" OnDataBinding="cbRuoli_DataBinding" RepeatColumns="1" TextField="description" ValueField="roleCode" ValueType="System.String" Width="100%">
                    </dx:ASPxCheckBoxList>
                </EditItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
        <ClientSideEvents EndCallback="gridProfiles_EndCallback" />
    </dx:ASPxGridView>

    <dx:ASPxPopupControl ID="popupProfiloInUso" runat="server" HeaderText="Profilo in uso" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Theme="Metropolis" ClientInstanceName="popupProfiloInUso">
        <ContentCollection>
<dx:PopupControlContentControl runat="server">Impossibile eliminare il profilo. Risulta associato ad un utente.</dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
