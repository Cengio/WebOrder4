<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="newcustomerList.aspx.vb" Inherits="servisWO.newcustomerList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
         <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#0099FF" 
    Text="lista nuovi clienti" 
    Theme="MetropolisBlue">
</dx:ASPxLabel>
     </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
    DataSourceID="SqlDataSource_NewCustomer" EnableTheming="True" KeyFieldName="CustomerNo" 
    Theme="MetropolisBlue" ClientInstanceName="grid" Width="100%">
        <ClientSideEvents EndCallback="function(s, e) {
          AdjustGridSize();
	                        }" Init="OnInitGrid" />
    <Columns>
        <dx:GridViewDataTextColumn FieldName="CustomerNo" ReadOnly="True" VisibleIndex="1" 
            Caption="Cod. N. Cliente" Width="100px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3" 
            Caption="Nome Cliente">
            <Settings AutoFilterCondition="Contains" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Address" VisibleIndex="6" 
            Caption="Indirizzo">
            <Settings AutoFilterCondition="Contains" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="City" VisibleIndex="7" Caption="Città">
            <Settings AutoFilterCondition="Contains" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Contact" VisibleIndex="10" 
            Visible="False">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Phone1" VisibleIndex="9" 
            Caption="Telefono">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Provincia" FieldName="County" 
            VisibleIndex="8" Width="80px">
            <Settings HeaderFilterMode="CheckedList" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" 
            Width="60px" Visible="False">
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="newOrder" Visibility="AllDataRows">
                    <Image AlternateText="New Order" ToolTip="Seleziona cliente e crea Nuovo Ordine" Url="~/images/orderNEW.png">
                    </Image>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn Caption="P.Iva" FieldName="VATNumber" 
            VisibleIndex="4">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Cod. Fiscale" FieldName="FiscalCode" 
            VisibleIndex="5">
        </dx:GridViewDataTextColumn>
        <dx:GridViewCommandColumn Caption=" " VisibleIndex="11" ButtonType="Image" Width="60px" ShowClearFilterButton="True">
            <CustomButtons>
                <dx:GridViewCommandColumnCustomButton ID="customerDetails" Visibility="AllDataRows">
                    <Image AlternateText="Customer Details" ToolTip="Dettagli Cliente" Url="~/images/user_select.png">
                    </Image>
                </dx:GridViewCommandColumnCustomButton>
            </CustomButtons>
        </dx:GridViewCommandColumn>
    </Columns>
        <SettingsBehavior AutoFilterRowInputDelay="400" AllowSelectByRowClick="True" 
            AllowSelectSingleRowOnly="True" />
        <SettingsPager PageSize="50" Position="TopAndBottom">
        </SettingsPager>
        <Settings ShowFilterBar="Auto" ShowFilterRow="True" 
            ShowVerticalScrollBar="True" VerticalScrollableHeight="360" />
        <SettingsCommandButton>
            <ClearFilterButton>
                <Image ToolTip="Cancella filtri di riga" Url="~/images/cancelFilter.png">
                </Image>
            </ClearFilterButton>
        </SettingsCommandButton>
</dx:ASPxGridView>
<asp:SqlDataSource ID="SqlDataSource_NewCustomer" runat="server" 
    ConnectionString="<%$ ConnectionStrings:WORconnectionString %>" 
    
        
        SelectCommand="SELECT * FROM [newCustomer] " 
        
    ProviderName="<%$ ConnectionStrings:WORconnectionString.ProviderName %>">
</asp:SqlDataSource>
</asp:Content>
