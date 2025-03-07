<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="quickCart.aspx.vb" Inherits="servisWO.quickCart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .dxgvControl_MetropolisBlue,
        .dxgvDisabled_MetropolisBlue {
            border: 1px Solid #c0c0c0;
            font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
            background-color: White;
            color: #333333;
            cursor: default;
        }

        .dxgvTable_MetropolisBlue {
            -webkit-tap-highlight-color: rgba(0,0,0,0);
        }

        .dxgvTable_MetropolisBlue {
            background-color: White;
            border-width: 0;
            border-collapse: separate !important;
            overflow: hidden;
        }

        .dxgvHeader_MetropolisBlue {
            cursor: pointer;
            white-space: nowrap;
            padding: 4px 6px 5px;
            border: 1px Solid #c0c0c0;
            overflow: hidden;
            font-weight: normal;
            text-align: left;
        }

            .dxgvHeader_MetropolisBlue td {
                white-space: nowrap;
            }

        .dxgvEditFormDisplayRow_MetropolisBlue td.dxgv,
        .dxgvDataRow_MetropolisBlue td.dxgv,
        .dxgvDataRowAlt_MetropolisBlue td.dxgv,
        .dxgvSelectedRow_MetropolisBlue td.dxgv,
        .dxgvFocusedRow_MetropolisBlue td.dxgv {
            overflow: hidden;
            border-bottom: 1px Solid #c0c0c0;
            border-right: 1px Solid #c0c0c0;
            border-top-width: 0;
            border-left-width: 0;
            padding: 3px 6px 4px;
        }

        .dxeTextBoxSys,
        .dxeMemoSys {
            border-collapse: separate !important;
        }

        .dxeTrackBar,
        .dxeIRadioButton,
        .dxeButtonEdit,
        .dxeTextBox,
        .dxeRadioButtonList,
        .dxeCheckBoxList,
        .dxeMemo,
        .dxeListBox,
        .dxeCalendar,
        .dxeColorTable {
            -webkit-tap-highlight-color: rgba(0,0,0,0);
        }

        .dxeTextBox,
        .dxeButtonEdit,
        .dxeIRadioButton,
        .dxeRadioButtonList,
        .dxeCheckBoxList {
            cursor: default;
        }

        .dxeTextBox,
        .dxeMemo {
            background-color: white;
            border: 1px solid #9f9f9f;
        }

        .dxeTextBoxSys td.dxic {
            *padding-left: 3px;
            *padding-top: 2px;
            *padding-bottom: 1px;
        }

        .dxeTextBoxSys td.dxic,
        .dxeButtonEditSys td.dxic {
            padding: 3px 3px 2px 3px;
            overflow: hidden;
        }

        .dxeButtonEditSys .dxeEditAreaSys,
        .dxeButtonEditSys td.dxic,
        .dxeTextBoxSys td.dxic,
        .dxeMemoSys td,
        .dxeEditAreaSys {
            width: 100%;
        }

        td.dxic {
            font-size: 0;
        }

        td.dxic {
            font-size: 0;
        }

        td.dxic {
            font-size: 0;
        }

        td.dxic {
            font-size: 0;
        }

        td.dxic {
            font-size: 0;
        }

        .dxic .dxeEditAreaSys {
            padding: 0px 1px 0px 0px;
        }

        .dxeTextBox .dxeEditArea {
            background-color: white;
        }

        .dxeEditAreaSys {
            height: 14px;
            line-height: 14px;
            border: 0px !important;
            padding: 0px 1px 0px 0px; /* B146658 */
            background-position: 0 0; /* iOS Safari */
        }

        .dxeEditArea {
            font: 12px Tahoma, Geneva, sans-serif;
            border: 1px solid #A0A0A0;
        }

        .dxgvCommandColumn_MetropolisBlue {
            padding: 8px 4px;
        }

        .dxgvFooter_MetropolisBlue {
            white-space: nowrap;
        }

            .dxgvFooter_MetropolisBlue td.dxgv {
                padding: 5px 6px 6px;
                border-bottom: 1px Solid #c0c0c0;
                border-right-width: 0;
            }
    </style>
</head>
<body style="border-width: 0px; margin: 0px">
    <form id="form1" runat="server">
        <div style="margin: 0px; border-width: 0px;">

            <dx:ASPxGridView ID="gridQuickCart" runat="server" AutoGenerateColumns="False"
                EnableTheming="True" Theme="MetropolisBlue" Width="400px"
                ClientInstanceName="gridQuickCart">
                <ClientSideEvents EndCallback="function(s, e) { CallBMasterCart.PerformCallback(); }" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="totaleriga"
                        ShowInGroupFooterColumn="Quantità" SummaryType="Sum" DisplayFormat="Totale merce {0:C2}" />
                </TotalSummary>
                <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Codice" FieldName="ItemCode" ReadOnly="True"
                        VisibleIndex="0" Width="110px" Visible="False">
                        <Settings AllowHeaderFilter="False" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Descrizione" FieldName="DESCRIZIONE"
                        ReadOnly="True" VisibleIndex="2">
                        <Settings AutoFilterCondition="Contains" AllowHeaderFilter="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="P.Riv." FieldName="UnitPrice"
                        ReadOnly="True" VisibleIndex="6" Width="40px">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                        <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False"
                            AllowSort="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Prezzo Pub."
                        FieldName="ordineLine.UnitPriceList" ReadOnly="True" VisibleIndex="7" Width="50px"
                        Visible="False">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                        <Settings AllowGroup="False" AllowAutoFilter="False" AllowHeaderFilter="False"
                            AllowSort="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Formato" FieldName="Formato"
                        ReadOnly="True" VisibleIndex="4" Width="60px" Visible="False">
                        <Settings HeaderFilterMode="CheckedList" AutoFilterCondition="Contains" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="IVA%" FieldName="IVA" ReadOnly="True"
                        VisibleIndex="10" Width="40px" Visible="False">
                        <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False"
                            AllowHeaderFilter="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Marchio" FieldName="Marchio"
                        ReadOnly="True" VisibleIndex="1" Width="100px" Visible="False">
                        <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Composizione" FieldName="Composizione"
                        ReadOnly="True" VisibleIndex="3" Width="80px" Visible="False">
                        <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DispoLotto" Width="50px" Caption="Dispo L." VisibleIndex="12"
                        Visible="False">
                        <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False"></Settings>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Dispo T." FieldName="Disponibilita"
                        ReadOnly="True" VisibleIndex="13" Width="50px" Visible="False">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Q.tà"
                        VisibleIndex="5" Width="40px" ReadOnly="True" FieldName="Quantity">
                        <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Totale riga" FieldName="TOTALERIGA"
                        VisibleIndex="15" Width="60px">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="17" Width="40px" Caption=" "
                        Visible="False">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="delFromCart" Visibility="AllDataRows">
                                <Image AlternateText="Elimina dal carrello" ToolTip="Elimina dal carrello" Url="~/images/delCart.png">
                                </Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="16" Width="40px" Caption=" "
                        Visible="False">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="updCart" Visibility="AllDataRows">
                                <Image AlternateText="Aggiona carrello" ToolTip="Aggiorna carrello" Url="~/images/updCart.png">
                                </Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="Lotto" FieldName="LotNo" Name="LotNo"
                        VisibleIndex="11" Width="50px" Visible="False">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Sct%" FieldName="FormulaSconto"
                        ReadOnly="True" VisibleIndex="8" Width="50px">
                        <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="IDPROMO" FieldName="IDPROMO"
                        Visible="False" VisibleIndex="20">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="RowDiscount" FieldName="RowDiscount"
                        Visible="False" VisibleIndex="18">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="DiscountQty" FieldName="DiscountQty"
                        Visible="False" VisibleIndex="19">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="P.Sco." FieldName="prezzofinale" VisibleIndex="14" Width="40px">
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowGroup="False" AllowSort="False"
                    AllowDragDrop="False" />
                <SettingsPager Visible="False" Mode="ShowAllRecords">
                </SettingsPager>
                <Settings ShowFooter="True" />
                <Styles>
                    <Footer Font-Bold="true">
                    </Footer>
                </Styles>
            </dx:ASPxGridView>

        </div>
    </form>
</body>
</html>
