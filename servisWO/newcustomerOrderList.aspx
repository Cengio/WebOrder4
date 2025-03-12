<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="newcustomerOrderList.aspx.vb" Inherits="servisWO.newcustomerOrderList" %>
<%@ Register assembly="DevExpress.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
.dxgvFilterRow .dxgvCommandColumn a 
{
    color:Red!important;
}

</style>
</asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
     <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#0099FF" 
    Text="lista ordini da nuovi clienti" 
    Theme="MetropolisBlue">
</dx:ASPxLabel>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="grid" runat="server" Width="100%" 
        AutoGenerateColumns="False" DataSourceID="SqlDataSource_Header" 
        ClientInstanceName="grid" EnableTheming="True" KeyFieldName="Code"
        Theme="MetropolisBlue">
        <ClientSideEvents EndCallback="function(s, e) {
               AdjustGridSize();
	                        }" Init="OnInitGrid" />
        <Columns>
            <dx:GridViewDataTextColumn FieldName="Code" VisibleIndex="1" ReadOnly="True" 
                Caption="Ordine N." Width="90px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Data Ordine" FieldName="OrderDate2" 
                VisibleIndex="2" Width="90px" Settings-FilterMode="Value">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" 
                    EditFormatString="dd/MM/yyyy">
                    <CalendarProperties TodayButtonText="Oggi">
                    </CalendarProperties>
                </PropertiesDateEdit>
                <Settings AllowHeaderFilter="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="CompanyName_" VisibleIndex="3" 
                ReadOnly="True" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn VisibleIndex="5" Caption="Cliente" ReadOnly="True" 
                FieldName="Name">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CustomerNo" VisibleIndex="4" 
                Caption="Cod. Cliente" ReadOnly="True" 
                Width="90px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Type" VisibleIndex="6" 
                Width="140px" Caption="Tipo"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShippingAgentCode" VisibleIndex="7" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ShipAddressCode" VisibleIndex="8" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="22" Caption="Stato" 
                ReadOnly="True" Width="120px">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Notes" 
                Visible="False" VisibleIndex="21">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AttachmentPath" VisibleIndex="9" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OperatorCode" VisibleIndex="11" Caption="Operatore" Width="140px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PackageNum" VisibleIndex="10" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OverpackageNum" VisibleIndex="12" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="User" VisibleIndex="13" Width="140px" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedImported" VisibleIndex="20" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="IncludeShipCost" VisibleIndex="14" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PaymentMethodCode" VisibleIndex="15" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PaymentTermsCode" VisibleIndex="16" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Weight" VisibleIndex="17" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="OrderNoCtrl" VisibleIndex="18" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UserCtrl" VisibleIndex="19" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="24" 
                Width="40px">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnUpdate" Visibility="AllDataRows">
                        <Image ToolTip="Modifica Ordine" Url="~/images/orderMOD.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="40px" Visible="False" ShowClearFilterButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnReport" Visibility="AllDataRows">
                        <Image AlternateText="Report Ordine" ToolTip="Report Ordine" Url="~/images/pdfdocument.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="false" AllowSelectSingleRowOnly="True" AllowDragDrop="False" AllowGroup="False" AutoFilterRowInputDelay="1000" />
        <SettingsPager PageSize="20" Position="TopAndBottom">
        </SettingsPager>
        <Settings ShowFilterRow="True" ShowGroupPanel="False" 
            ShowVerticalScrollBar="True" VerticalScrollableHeight="360" 
            ShowHeaderFilterButton="False" ShowFilterBar="Auto" 
            ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" />
        <SettingsCommandButton>
            <ClearFilterButton>
                <Image ToolTip="Cancella filtri di riga" Url="~/images/cancelFilter.png">
                </Image>
            </ClearFilterButton>
        </SettingsCommandButton>
    </dx:ASPxGridView>
    <asp:SqlDataSource ID="SqlDataSource_Header" runat="server" DataSourceMode="DataSet"></asp:SqlDataSource>
    </asp:Content>
