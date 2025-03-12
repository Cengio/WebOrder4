<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/mag/magazzino.Master" CodeBehind="magEndPicking.aspx.vb" Inherits="servisWO.magEndPicking" %>
<%@ Register assembly="DevExpress.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
        EnableTheming="True" Theme="Office2010Black" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn Caption="Riga Nr." FieldName="ordineLine.LineID" 
                VisibleIndex="1" Width="40px">
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Cod. Articolo" FieldName="ordineLine.ItemCode" 
                VisibleIndex="2" Width="100px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Collocazione" FieldName="ordineLine.BinCode" 
                VisibleIndex="4" Width="120px">
                <CellStyle HorizontalAlign="Left">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Q.tà Ordine" FieldName="ordineLine.OriginalQty" 
                VisibleIndex="8" Width="80px">
                <PropertiesTextEdit DisplayFormatString="F0">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Q.tà Picked" FieldName="ordineLine.QtyToShip" 
                VisibleIndex="9" Width="80px">
                <PropertiesTextEdit DisplayFormatString="F0">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Lotto" FieldName="ordineLine.LotNo" VisibleIndex="5" 
                Width="90px">
                <CellStyle HorizontalAlign="Left">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Picked" FieldName="LOADED" 
                VisibleIndex="10" Width="50px" Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Descrizione" Name="DESCRIZIONE"  FieldName="DESCRIZIONE" 
                VisibleIndex="3">
                <CellStyle HorizontalAlign="Left">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="11" Width="40px" ShowClearFilterButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnGoToLine" Visibility="AllDataRows">
                        <Image ToolTip="Vai alla linea" Url="~/images/pickOK.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="OrderCode" FieldName="ordineLine.OrderCode" 
                Visible="False" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ordineLine.RowDiscount" Visible="False" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ordineLine.DiscountQty" Visible="False" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
    </dx:ASPxGridView>
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" ColCount="6" 
        EnableTheming="True" Theme="Office2010Black" Width="100%">
        <Items>
            <dx:LayoutGroup Caption="Dettagli spedizione" ColCount="4" ColSpan="4" 
                Width="50%" Name="dettagliSpedizione" VerticalAlign="Middle">
                <Items>
                    <dx:LayoutItem Caption="Numero colli" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="tb_nrcolli" runat="server" Font-Size="11pt" Width="80px">
                                    <ValidationSettings Display="Dynamic" ErrorTextPosition="Right">
                                        <RegularExpression ErrorText="invalido" ValidationExpression="^[1-9]\d*$" />
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Peso Lordo" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxTextBox ID="tb_peso" runat="server" Font-Size="11pt" Width="80px">
                                    <ValidationSettings Display="Dynamic" ErrorTextPosition="Right">
                                        <RegularExpression ErrorText="invalido" ValidationExpression="^[1-9]\d*$" />
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Corriere" Width="60%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxComboBox ID="comboCorrieri" runat="server" Font-Size="10pt" 
                                    Width="260px">
                                    <ValidationSettings ErrorTextPosition="Right">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" Caption=" ">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                   <dx:ASPxButton ID="btn_Spedisci" runat="server" Font-Size="11pt" 
                            Text="Spedisci" Theme="Office2010Black" Width="160px">
                        </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
                <SettingsItems HorizontalAlign="Left" />
            </dx:LayoutGroup>
            <dx:LayoutItem ShowCaption="False" Width="20%" Name="raccogliAltro" VerticalAlign="Middle">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="btn_NewPick" runat="server" Font-Size="11pt" 
                            Text="Raccogli altro ordine" Theme="Office2010Black" Width="100%" 
                            CausesValidation="False">
                        </dx:ASPxButton>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Name="inviaCTRL" ShowCaption="False" VerticalAlign="Middle">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="btn_InviaCTRL" runat="server" CausesValidation="False" 
                            Font-Size="11pt" Text="Invia a controllo e fatturazione" 
                            Theme="Office2010Black" Width="100%">
                        </dx:ASPxButton>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
        </Items>
        <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" 
            HorizontalAlign="Left" />
        <SettingsItems VerticalAlign="Bottom" />
    </dx:ASPxFormLayout>
</asp:Content>
