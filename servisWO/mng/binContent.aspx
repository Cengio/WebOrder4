<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="binContent.aspx.vb" Inherits="servisWO.binContent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script language="javascript" type="text/javascript">
         function OnUploadTextChanged(s, e) {
             s.Upload();
         }
         function OnFileUploadComplete(s, e) {
             //alert('FileUploadComplete: ' + e.callbackData);
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#FF8800" 
    Text="gestione bin code" 
    Theme="Metropolis">
    </dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="gridBinCode" runat="server" AutoGenerateColumns="False" 
    EnableTheming="True" KeyFieldName="BinCode" Theme="Metropolis" Width="100%">
    <Columns>
        <dx:GridViewCommandColumn VisibleIndex="0" Width="40px" ShowEditButton="True"/>
        <dx:GridViewDataTextColumn Caption="Bin Code" FieldName="BinCode" 
            Name="BinCode" VisibleIndex="1" Width="160px" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Descrizione Nav" FieldName="Description" 
            Name="Description" VisibleIndex="2" Width="200px" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Descrizione Web" FieldName="WebDescription" 
            Name="WebDescription" VisibleIndex="3" Width="220px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Scegli immagine" 
            Name="ImageBrowse" VisibleIndex="6">
            <EditItemTemplate>
                <dx:ASPxUploadControl ID="uploadBinImage" runat="server" 
                    onfileuploadcomplete="uploadBinImage_FileUploadComplete" 
                    ShowClearFileSelectionButton="False" ShowProgressPanel="True" 
                    Theme="Metropolis" UploadMode="Auto" Width="100%">
                    <ClientSideEvents FileUploadComplete="OnFileUploadComplete" 
                        TextChanged="OnUploadTextChanged" />
                    <CancelButton Text="">
                    </CancelButton>
                </dx:ASPxUploadControl>
            </EditItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Nome File" FieldName="ImageFileName" 
            Name="ImageFileName" ReadOnly="True" VisibleIndex="4" Width="160px">
        </dx:GridViewDataTextColumn>
    </Columns>
        <SettingsPager PageSize="20" Position="TopAndBottom">
        </SettingsPager>
        <SettingsEditing Mode="Inline" />
        <Styles>
            <Cell HorizontalAlign="Left" VerticalAlign="Top">
            </Cell>
        </Styles>
</dx:ASPxGridView>
</asp:Content>
