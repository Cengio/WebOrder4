<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="orderListNav.aspx.vb" Inherits="servisWO.orderListNav" %>

<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dxgvFilterRow .dxgvCommandColumn a {
            color: Red !important;
        }
    </style>
        <script type="text/javascript">
            function OnEndCallback(s, e) {
                if (!grid.cpShowReport)
                    return;
                if (grid.cpShowReport == "1") {
                    popupReport.Show();
                    popupReport.cpShowReport = null;
                }
            }
        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True"
        Font-Size="Medium" ForeColor="#0099FF"
        Text="Storico ordini Navision"
        Theme="MetropolisBlue">
    </dx:ASPxLabel>
</asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="grid" runat="server" Width="100%"
        AutoGenerateColumns="False"
        ClientInstanceName="grid" EnableTheming="True" KeyFieldName="Code" DataSourceID="SqlDataSourceOrdini"
        Theme="MetropolisBlue">
        <ClientSideEvents EndCallback="OnEndCallback" Init="OnInitGrid" CustomButtonClick="function(s, e) {OnEmailClick(e,grid.GetRowKey(e.visibleIndex));}" />
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Code" VisibleIndex="0" ReadOnly="True"
                Caption="Ordine N." Width="90px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Data Ordine" FieldName="OrderDate2"
                VisibleIndex="1" Width="90px" Settings-FilterMode="Value">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"
                    EditFormatString="dd/MM/yyyy">
                    <CalendarProperties TodayButtonText="Oggi">
                    </CalendarProperties>
                </PropertiesDateEdit>
                <Settings AllowHeaderFilter="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="6"
                ReadOnly="True" Caption="Cliente">
                <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CustomerNo" VisibleIndex="5"
                Caption="Cod. Cliente" ReadOnly="True"
                Width="100px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Type" VisibleIndex="7"
                Width="180px" Caption="Tipo Interazione">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="24" Caption="Stato"
                ReadOnly="True" Width="150px">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OperatorCode" VisibleIndex="12"
                Caption="Creato da" Width="180px">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="User" VisibleIndex="15" Width="180px"
                Caption="Magazziniere">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedImported" VisibleIndex="22"
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="25"
                Width="40px" Name="btnUpdate" ShowClearFilterButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnUpdate" Visibility="AllDataRows">
                        <Image ToolTip="Modifica Ordine" Url="~/images/orderMOD.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="26" Width="40px" Caption=" " Name="btnReport">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnReport" Visibility="AllDataRows">
                        <Image AlternateText="Report Ordine" ToolTip="Report Ordine" Url="~/images/pdfdocument.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
        <SettingsBehavior AllowSelectByRowClick="True"
            AllowSelectSingleRowOnly="True" AllowDragDrop="False" AllowGroup="False" AutoFilterRowInputDelay="1000" />
        <SettingsPager PageSize="20" Position="TopAndBottom">
        </SettingsPager>
        <Settings
            ShowVerticalScrollBar="True" VerticalScrollableHeight="360" ShowFilterBar="Auto"
            ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowFilterRow="True" />
    </dx:ASPxGridView>


    <asp:SqlDataSource ID="SqlDataSourceOrdini" runat="server" DataSourceMode="DataSet"></asp:SqlDataSource>
               

       <dx:ASPxPopupControl ID="popupReport" runat="server" 
        ClientInstanceName="popupReport" ContentUrl="~/orderReportTR.aspx" 
        HeaderText="Report ordine" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="TopSides" 
        Theme="MetropolisBlue" AllowDragging="True" AllowResize="True" 
        AutoUpdatePosition="True" Width="1100px" Height="840px" Modal="True">
        <ClientSideEvents Shown="function(s, e) {
	popupReport.RefreshContentUrl();
}
" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>


            </asp:Content>
