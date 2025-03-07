<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="orderList.aspx.vb" Inherits="servisWO.orderList" %>

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
                    grid.cpShowReport = null;
                } else if (grid.cpShowReport == "2") {                
                    callbackDownloadPDF.PerformCallback(grid.cpShowReportDownload);
                    grid.cpShowReport = null;
                }
            }

            function OnEmailClick(e, keyValue) {
                if (e.buttonID == 'btnEmail') {
                    popup.Show();
                    popup.PerformCallback(keyValue);
                } else {
                    e.processOnServer = true;
                }
            }

            function OnDownloadPDFCallbackComplete(s, e) {
                if (e.result == "NOT_ALLOWED") {
                    alert('Report ordine non disponibile.');
                }
                else {
                    window.location.href = e.result;
                }
            }
        </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True"
        Font-Size="Medium" ForeColor="#0099FF"
        Text="lista ordini"
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
                VisibleIndex="1" Width="90px" Settings-FilterMode="Value" Visible="False">
                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"
                    EditFormatString="dd/MM/yyyy">
                    <CalendarProperties TodayButtonText="Oggi">
                    </CalendarProperties>
                </PropertiesDateEdit>
                <Settings AllowHeaderFilter="False" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="CustomerDescription" VisibleIndex="6"
                ReadOnly="True" Caption="Cliente" Name="CustomerDescription">
                <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CustomerNo" VisibleIndex="5"
                Caption="Cod. Cliente" ReadOnly="True"
                Width="90px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Type" VisibleIndex="7"
                Width="120px" Caption="Tipo Interazione">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="24" Caption="Stato"
                ReadOnly="True" Width="100px">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UTENTE_CREAZIONE" VisibleIndex="12"
                Caption="Creato da" Width="150px">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="MAGAZZINIERE" VisibleIndex="15" Width="120px"
                Caption="Magazziniere">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedImported" VisibleIndex="22"
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="25"
                Width="40px" ShowClearFilterButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnUpdate" Visibility="AllDataRows">
                        <Image ToolTip="Modifica Ordine" Url="~/images/orderMOD.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="26" Width="40px" Caption=" ">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnReport" Visibility="AllDataRows">
                        <Image AlternateText="Report Ordine" ToolTip="Report Ordine" Url="~/images/pdfdocument.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn Caption=" " Name="pulsanteEmail" VisibleIndex="27" Width="120px">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnEmail" Text="Richiedi produzione" Visibility="AllDataRows">
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Data creazione" FieldName="DATA_CREAZIONE" VisibleIndex="1" Width="140px">
                <Settings AllowHeaderFilter="False" AutoFilterCondition="BeginsWith" FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Data richiesta evasione" FieldName="DATA_EVASIONE" VisibleIndex="2" Width="140px" Name="dataevasione">
                <Settings AllowHeaderFilter="False" AutoFilterCondition="BeginsWith" FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Data ultima modifica" FieldName="DATA_ULTIMA_MODIFICA" VisibleIndex="3" Width="140px" Name="dataultimamodifica">
                <Settings AllowHeaderFilter="False" AutoFilterCondition="BeginsWith" FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Modificato da" FieldName="UTENTE_ULTIMA_MODIFICA" VisibleIndex="13" Width="150px">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior AllowSelectByRowClick="True" AllowSort="false" AllowSelectSingleRowOnly="True" AllowDragDrop="False" AllowGroup="False" AutoFilterRowInputDelay="1000" />
        <SettingsPager PageSize="20" Position="TopAndBottom">
        </SettingsPager>
        <Settings
            ShowVerticalScrollBar="True" VerticalScrollableHeight="360" ShowFilterBar="Auto"
            ShowFilterRowMenu="True" ShowFilterRowMenuLikeItem="True" ShowFilterRow="True" ShowHeaderFilterButton="False" />
    </dx:ASPxGridView>


    <asp:SqlDataSource ID="SqlDataSourceOrdini" runat="server" DataSourceMode="DataSet">
        <SelectParameters>
            <asp:QueryStringParameter Name="CompletedImported" Direction="Input" QueryStringField="ci" Size="10" DbType="String" DefaultValue="0" />
            <asp:QueryStringParameter Name="Status" Direction="Input" QueryStringField="s" Size="20" DbType="String" DefaultValue="0,1" />
            <asp:QueryStringParameter Name="NotStatus" Direction="Input" QueryStringField="ns" Size="20" DbType="String" DefaultValue="99" />
            <asp:QueryStringParameter Name="NotCompletedImported" Direction="Input" QueryStringField="nci" Size="10" DbType="String" DefaultValue="99" />
            <asp:QueryStringParameter Name="Prenotazione" Direction="Input" QueryStringField="p" Size="10" DbType="String" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
               

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


    <dx:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="popup" 
        HeaderText="Richiesta articoli da produrre" Height="420px" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        Theme="MetropolisBlue" Width="800px" AllowDragging="True">
        <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" EnableTheming="True" 
        Theme="MetropolisBlue" Width="100%">
        <Items>
            <dx:LayoutGroup Caption="Destinatario" ShowCaption="False">
                <Items>
                    <dx:LayoutItem Caption="A:">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="tb_A" runat="server" Font-Size="11pt" Width="100%">
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True" />
                                        <RegularExpression ErrorText="Email non valida" 
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Cc:">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="tb_Cc" runat="server" Font-Size="11pt" Width="100%">
                                    <ValidationSettings>
                                        <RegularExpression ErrorText="Email non valida" 
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Cc2">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="tb_Cc2" runat="server" Font-Size="11pt" Width="100%">
                                     <ValidationSettings>
                                        <RegularExpression ErrorText="Email non valida" 
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Oggetto:">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="tb_Oggetto" runat="server" Font-Size="11pt" Width="100%">
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup ShowCaption="False">
                <Items>
                    <dx:LayoutItem ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxMemo ID="tb_Corpo" runat="server" Font-Size="11pt" Height="260px" 
                                    Width="100%">
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxMemo>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutItem ShowCaption="False">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server">
                        <dx:ASPxButton ID="btnInvioRichiesta" runat="server" Font-Size="11pt" 
                            Text="Invia richiesta" Theme="MetropolisBlue" Width="170px">
                        </dx:ASPxButton>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
        </Items>
        <SettingsItems Width="100%" />
    </dx:ASPxFormLayout>
    <dx:ASPxHiddenField ID="hiddenOrderCode" runat="server">
    </dx:ASPxHiddenField>
            </dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="popupInvioOK" runat="server" 
        HeaderText="Invio richiesta" Modal="True" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" Theme="MetropolisBlue" Width="360px">
        <ContentStyle Font-Size="12pt">
        </ContentStyle>
        <ContentCollection>
<dx:PopupControlContentControl runat="server">Richiesta inviata correttamente.</dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="popupInvioKO" runat="server" 
        HeaderText="Invio richiesta" Modal="True" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="WindowCenter" Theme="MetropolisBlue" Width="360px">
        <ContentStyle Font-Size="12pt">
        </ContentStyle>
        <ContentCollection>
<dx:PopupControlContentControl runat="server">Si sono verificati problemi nell&#39;invio 
    della email di richiesta alla produzione.<br /> Riprovare tra qualche istante.</dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>


    <dx:ASPxCallback ID="ASPxCallback_downloadPDF" runat="server" ClientInstanceName="callbackDownloadPDF">
                <ClientSideEvents CallbackComplete="OnDownloadPDFCallbackComplete" />
    </dx:ASPxCallback>

            </asp:Content>
