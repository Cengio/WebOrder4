<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="productFinder.aspx.vb" Inherits="servisWO.productFinder" %>
    <%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
     <asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
         <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True"
             Font-Size="Medium" ForeColor="#0099FF"
             Text="ricerca articoli"
             Theme="MetropolisBlue">
         </dx:ASPxLabel>
     </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >

    <script type="text/javascript">

        function OnInfoLottiClick(key) {
            CallbackPanelinfolotti.SetContentHtml("");
            popuplotti.Show();
            popuplotti.SetHeaderText("Lotti per " + key)
            CallbackPanelinfolotti.PerformCallback(key);
        }

        function preventEnterKey(htmlEvent) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            }
        }

        var rowVisibleIndex;
        var codArtMancante;
        var eArg;

        function grid_CustomButtonClick(s, e) {
            if (e.buttonID == 'gestMancanti') {
                eArg = e;
                rowVisibleIndex = e.visibleIndex;
                s.GetRowValues(e.visibleIndex, 'Disponibilita', OnNoDispo);
            } else if (e.buttonID == 'showLotti') {
                grid.GetRowValues(e.visibleIndex, 'CodiceArticolo', OnInfoLottiClick);
            } else {
                e.processOnServer = true;
            }
        }

        function OnNoDispo(Disponibilita) {
            if (Disponibilita <= 0) {
                grid.GetRowValues(rowVisibleIndex, 'CodiceArticolo', ShowPopupNoDispo);
            } else {
                eArg.processOnServer = true;
            }
        }

        function ShowPopupNoDispo(CodiceArticolo) {
            codArtMancante = CodiceArticolo;
            lbCodiceMancante.SetText(CodiceArticolo);
            popupnodispo.Show();
            tbqtaMancante.Focus();
        }

        function btnYes_Click(s, e) {
            ClosePopup(true);
        }

        function btnNo_Click(s, e) {
            ClosePopup(false);
        }

        function ClosePopup(result) {
            popupnodispo.Hide();
            if (result) CallbackMancanti.PerformCallback(codArtMancante + '|' + tbqtaMancante.GetValue() + '|' + cbAvvisoMancanti.GetChecked());
        }

        function OnComboPromoButtonClick(s, e, visibleIndex) {
            var QUANTITY_MIN = s.cpQUANTITY_MIN_ColumnValues[s.GetSelectedIndex()];
            var QUANTITY_GIFT = s.cpQUANTITY_GIFT_ColumnValues[s.GetSelectedIndex()];
            var DISCOUNT_PERCENT = s.cpDISCOUNT_PERCENT_ColumnValues[s.GetSelectedIndex()];
            //var tbqta = ASPxClientControl.GetControlCollection().GetByName('tbqta_' + visibleIndex);
            var tbqta = eval('tbqta_' + visibleIndex);
            var tbscomerce = eval('tbscomerce_' + visibleIndex);
            var tbscoperc = eval('tbscoperc_' + visibleIndex);
            if (e.buttonIndex == 0 && s.GetValue()!=null) {
                tbqta.SetValue(Number(tbqta.GetValue()) + QUANTITY_MIN);
                tbscomerce.SetValue(Number(tbscomerce.GetValue()) + QUANTITY_GIFT);
                tbscoperc.SetValue(DISCOUNT_PERCENT);
                calcolaPrezzoFinale(s, e, visibleIndex);

                var qtyDispo = grid.cpQtyDispoValues[visibleIndex];
                var qtatotale = Number(tbqta.GetValue()) + Number(tbscomerce.GetValue());
                if (qtatotale > qtyDispo) { 
                    popupControlloQta.SetContentHTML("La quantità <b>" + qtatotale + "</b> inserita è superiore alla disponibilità <b>" + qtyDispo + "</b> dell'articolo.");
                    popupControlloQta.Show();
                }
            } else if (e.buttonIndex == 1) {
                s.SetValue(null);
                tbqta.SetValue(0);
                tbscomerce.SetValue(0);
                tbscoperc.SetValue(0);
                calcolaPrezzoFinale(s, e, visibleIndex);
            }
        }

        function OnQtaValueChanged(s, e, visibleIndex) {
            calcolaPrezzoFinale(s, e, visibleIndex);
        }

        function OnScontoMerceValueChanged(s, e, visibleIndex) {
            calcolaPrezzoFinale(s, e, visibleIndex);
        }

        function OnScontoPercentualeValueChanged(s, e, visibleIndex) {
            calcolaPrezzoFinale(s, e, visibleIndex);
        }

        function calcolaPrezzoFinale(s, e, visibleIndex) {
            var tbqta = eval('tbqta_' + visibleIndex);
            var tbscomerce = eval('tbscomerce_' + visibleIndex);
            var tbscoperc = eval('tbscoperc_' + visibleIndex);
            var lbprezzofinale = eval('lbprezzofinale_' + visibleIndex);
            var prezzorivenditore = Number(s.cpPrezzoRivenditore);
            var qta = Number(tbqta.GetValue());
            var qtasconto = Number(tbscomerce.GetValue());

            if (!(qta==0)) {
                var scontoperc = Number(tbscoperc.GetValue());
                var qtatotale = qta + qtasconto;
                var prezzotot = qta * prezzorivenditore;
                var totscontoperc = prezzotot / 100 * scontoperc;
                var prezzofinale = (prezzotot - totscontoperc)/qtatotale;
                prezzofinale = prezzofinale.toFixed(2);
                lbprezzofinale.SetText('€. ' + prezzofinale);
           } else {
                lbprezzofinale.SetText('€. ' + prezzorivenditore);
            }
        }

        function onQtaValidation(s, e, index, isarcheo) {
            if (grid.cpQtyDispoValues) {
                var QtyToAdd = s.GetValue();
                var qtyDispo = grid.cpQtyDispoValues[index];
                if (!isNaN(QtyToAdd)) {
                    if (QtyToAdd > 0) {
                        if (isarcheo == 0 && QtyToAdd > qtyDispo) {
                            //popupControlloQta.SetHeaderText(titolo);
                            popupControlloQta.SetContentHTML("La quantità <b>" + QtyToAdd + "</b> inserita è superiore alla disponibilità <b>" + qtyDispo + "</b> dell'articolo.");
                            popupControlloQta.Show();
                            s.SetValue(qtyDispo);
                        }
                    } else {
                        popupControlloQta.SetContentHTML("Inserire una quantità maggiore di zero.");
                        popupControlloQta.Show();
                        s.SetValue("");
                    }
                } else {
                    popupControlloQta.SetContentHTML("Il valore inserito <b>" + QtyToAdd + "</b> non valido");
                    popupControlloQta.Show();
                    s.SetValue("");
                }
            }
        }



        //------------------ Promo Lotti Validation -------------------

        function OnPromoLottoClick(key) {
            cbPanelPromoLotti.SetContentHtml("");
            popupPromoLotti.Show();
            popupPromoLotti.SetHeaderText("Promo su Lotti per " + key)
            cbPanelPromoLotti.PerformCallback(key);
        }


        function OnComboPromoLottoButtonClick(s, e, visibleIndex) {
            var QUANTITY_MIN = s.cpQUANTITY_MIN_ColumnValues[s.GetSelectedIndex()];
            var QUANTITY_GIFT = s.cpQUANTITY_GIFT_ColumnValues[s.GetSelectedIndex()];
            var DISCOUNT_PERCENT = s.cpDISCOUNT_PERCENT_ColumnValues[s.GetSelectedIndex()];
            var tbqta = eval('tbqtaLotto_' + visibleIndex);
            var tbscomerce = eval('tbscomerceLotto_' + visibleIndex);
            var tbscoperc = eval('tbscopercLotto_' + visibleIndex);
            if (e.buttonIndex == 0 && s.GetValue() != null) {
                tbqta.SetValue(Number(tbqta.GetValue()) + QUANTITY_MIN);
                tbscomerce.SetValue(Number(tbscomerce.GetValue()) + QUANTITY_GIFT);
                tbscoperc.SetValue(DISCOUNT_PERCENT);
                calcolaPrezzoFinaleLotto(s, e, visibleIndex);

                var qtyDispo = gridPromoLotti.cpQtyDispoValues[visibleIndex];
                var qtatotale = Number(tbqta.GetValue()) + Number(tbscomerce.GetValue());
                if (qtatotale > qtyDispo) {
                    popupControlloQta.SetContentHTML("La quantità <b>" + qtatotale + "</b> inserita è superiore alla disponibilità <b>" + qtyDispo + "</b> dell'articolo.");
                    popupControlloQta.Show();
                }
            } else if (e.buttonIndex == 1) {
                s.SetValue(null);
                tbqta.SetValue(0);
                tbscomerce.SetValue(0);
                tbscoperc.SetValue(0);
                calcolaPrezzoFinaleLotto(s, e, visibleIndex);
            }
        }

        function OnQtaLottoValueChanged(s, e, visibleIndex) {
            calcolaPrezzoFinaleLotto(s, e, visibleIndex);
        }

        function OnScontoMerceLottoValueChanged(s, e, visibleIndex) {
            calcolaPrezzoFinaleLotto(s, e, visibleIndex);
        }

        function OnScontoPercentualeLottoValueChanged(s, e, visibleIndex) {
            calcolaPrezzoFinaleLotto(s, e, visibleIndex);
        }

        function calcolaPrezzoFinaleLotto(s, e, visibleIndex) {
            var tbqta = eval('tbqtaLotto_' + visibleIndex);
            var tbscomerce = eval('tbscomerceLotto_' + visibleIndex);
            var tbscoperc = eval('tbscopercLotto_' + visibleIndex);
            var lbprezzofinale = eval('lbprezzofinaleLotto_' + visibleIndex);
            var prezzorivenditore = Number(s.cpPrezzoRivenditore);
            var qta = Number(tbqta.GetValue());
            var qtasconto = Number(tbscomerce.GetValue());

            if (!(qta == 0)) {
                var scontoperc = Number(tbscoperc.GetValue());
                var qtatotale = qta + qtasconto;
                var prezzotot = qta * prezzorivenditore;
                var totscontoperc = prezzotot / 100 * scontoperc;
                var prezzofinale = (prezzotot - totscontoperc) / qtatotale;
                prezzofinale = prezzofinale.toFixed(2);
                lbprezzofinale.SetText('€. ' + prezzofinale);
            } else {
                lbprezzofinale.SetText('€. ' + prezzorivenditore);
            }
        }

        function onQtaLottoValidation(s, e, index, isarcheo, dispoLotto) {
            if (gridPromoLotti.cpQtyDispoValues) {
                var QtyToAdd = Number(s.GetValue());
                var qtyDispo = gridPromoLotti.cpQtyDispoValues[index];

                var tbscomerce = eval('tbscomerceLotto_' + index);
                var qtasconto = Number(tbscomerce.GetValue());
                var qtatotale = QtyToAdd + qtasconto;

                if (!isNaN(QtyToAdd)) {
                    if (QtyToAdd > 0) {
                        if (isarcheo == 0 && qtatotale > qtyDispo) {
                            popupControlloQta.SetContentHTML("La quantità <b>" + qtatotale + "</b> inserita è superiore alla disponibilità <b>" + qtyDispo + "</b> dell'articolo.");
                            popupControlloQta.Show();
                            //s.SetValue(qtyDispo);
                        }
                    } else {
                        popupControlloQta.SetContentHTML("Inserire una quantità maggiore di zero.");
                        popupControlloQta.Show();
                        s.SetValue("");
                    }
                } else {
                    popupControlloQta.SetContentHTML("Il valore inserito <b>" + QtyToAdd + "</b> non valido");
                    popupControlloQta.Show();
                    s.SetValue("");
                }
            }
        }

        // -----------------------------------------------------------------------



    </script>
     

    <dx:ASPxPopupControl ID="popuplotti" runat="server" AllowDragging="True"
        ClientInstanceName="popuplotti" HeaderText="Gestione lotti"
        PopupHorizontalAlign="WindowCenter" 
        Theme="MetropolisBlue" Width="530px" Modal="True"
        PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_infolotti" runat="server"
                    ClientInstanceName="CallbackPanelinfolotti" Theme="MetropolisBlue"
                    Width="100%" EnableCallbackAnimation="True" HideContentOnCallback="True"
                    >
                    <SettingsLoadingPanel Delay="500"></SettingsLoadingPanel>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView ID="gridlotti" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridlottiinfo" Width="500px" KeyFieldName="CodiceArticolo">
                                <SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" ColumnResizeMode="Control" AllowGroup="False" AllowDragDrop="False" AllowSort="false" />
                                <ClientSideEvents EndCallback="function(s, e) { CallBMasterCart.PerformCallback(); grid.PerformCallback(); }" />
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="CodiceArticolo" FieldName="Item No_"
                                        Name="CodiceArticolo" ReadOnly="True" ShowInCustomizationForm="True"
                                        Visible="False" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Lotto" FieldName="Lotto" Name="Lotto"
                                        ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scadenza Lotto" FieldName="ScadenzaLotto"
                                        Name="ScadenzaLotto" ReadOnly="True" ShowInCustomizationForm="True"
                                        UnboundType="DateTime" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Disponibilità Lotto"
                                        FieldName="DisponibilitaLotto" Name="DisponibilitaLotto" ReadOnly="True"
                                        ShowInCustomizationForm="True" VisibleIndex="3" Width="110px">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                            </dx:ASPxGridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupnodispo" runat="server"
        ClientInstanceName="popupnodispo" 
        Theme="MetropolisBlue" CloseAction="None"
        HeaderText="Gestione articoli mancanti" Modal="True" PopupAction="None"
        PopupElementID="ASPxGridView_Products" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="420px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                Inserire la quantità desiderata per l&#39;articolo<br />
                &nbsp;<dx:ASPxLabel
                    ID="lb_CodiceMancante" runat="server"
                    ClientInstanceName="lbCodiceMancante" Font-Bold="True" Font-Size="Small">
                </dx:ASPxLabel>
                <br />
                <br />
                <dx:ASPxTextBox ID="tb_qtaMancante" runat="server" Width="140px" ClientInstanceName="tbqtaMancante">
                    <MaskSettings AllowMouseWheel="False" Mask="<1..999>" />
                </dx:ASPxTextBox>
                <br />
                <dx:ASPxCheckBox ID="cb_avvisoMancanti" runat="server" CheckState="Unchecked"
                    ClientInstanceName="cbAvvisoMancanti" Text="Avviso quando disponibile">
                </dx:ASPxCheckBox>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="btnYes" runat="server" Text="Conferma" Width="50px"
                                AutoPostBack="False" ClientInstanceName="btnYes">
                                <ClientSideEvents Click="btnYes_Click" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnNo" runat="server" Text="Annulla" Width="50px"
                                AutoPostBack="False">
                                <ClientSideEvents Click="btnNo_Click" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table> 

            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupControlloQta" runat="server"
        ClientInstanceName="popupControlloQta" EnableTheming="True"
        HeaderText="Controllo quantità" Modal="True" PopupAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Theme="MetropolisBlue" Width="420px">
        <ContentStyle Font-Size="14pt">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">test</dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>



    <dx:ASPxPopupControl ID="popupPromoLotti" runat="server" AllowDragging="True"
        ClientInstanceName="popupPromoLotti" HeaderText="Gestione Promo su lotti"
        PopupHorizontalAlign="WindowCenter" 
        Theme="MetropolisBlue" Width="830px" Modal="True"
        PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxCallbackPanel ID="cbPanelPromoLotti" runat="server"
                    ClientInstanceName="cbPanelPromoLotti" Theme="MetropolisBlue"
                    Width="100%" EnableCallbackAnimation="True" HideContentOnCallback="True"
                    >
                    <SettingsLoadingPanel Delay="500"></SettingsLoadingPanel>
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView ID="gridPromoLotti" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridPromoLotti" KeyFieldName="Item No_" Width="100%">
                                  <SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" ColumnResizeMode="Control" AllowGroup="False" AllowDragDrop="False" AllowSort="false" />
                                   <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                <ClientSideEvents EndCallback="function(s, e) { CallBMasterCart.PerformCallback(); grid.Refresh();}" />
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="CodiceArticolo" FieldName="Item No_" Name="CodiceArticolo" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Lotto" FieldName="Lotto" Name="Lotto" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Width="80px" MinWidth="60">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scadenza" FieldName="ScadenzaLotto" Name="ScadenzaLotto" ReadOnly="True" ShowInCustomizationForm="True" UnboundType="DateTime" MinWidth="90" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Dispo" FieldName="DisponibilitaLotto" Name="DisponibilitaLotto" ReadOnly="True" ShowInCustomizationForm="True" Width="45px" VisibleIndex="3" >
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="P. Riv." FieldName="PrzRivenditore" ReadOnly="True" VisibleIndex="4" Width="65px" MinWidth="40">
                                        <PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Promo Lotto" Name="PromozioneLotto" VisibleIndex="5" Width="154px">
                                        <DataItemTemplate>
                                            <dx:ASPxComboBox ID="comboPromoLotto" runat="server" Width="140px" ClientInstanceName="comboPromoLotto" OnInit="comboPromoLotto_Init" OnCustomJSProperties="comboPromoLotto_CustomJSProperties" CaptionSettings-ShowColon="False">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="ID" Visible="False" />
                                                    <dx:ListBoxColumn FieldName="PROMO_CODE" Visible="False" />
                                                    <dx:ListBoxColumn FieldName="DESCRIPTION" Caption="Promo disponibili" />
                                                    <dx:ListBoxColumn FieldName="QUANTITY_MIN" Visible="False" />
                                                    <dx:ListBoxColumn FieldName="QUANTITY_GIFT" Visible="False" />
                                                    <dx:ListBoxColumn FieldName="DISCOUNT_PERCENT" Visible="False" />
                                                </Columns>
                                                <Buttons>
                                                    <dx:EditButton ToolTip="Applica">
                                                        <Image IconID="actions_apply_16x16">
                                                        </Image>
                                                    </dx:EditButton>
                                                    <dx:EditButton ToolTip="Elimina">
                                                        <Image IconID="actions_cancel_16x16">
                                                        </Image>
                                                    </dx:EditButton>
                                                </Buttons>
                                                <ClearButton DisplayMode="Never">
                                                </ClearButton>
                                            </dx:ASPxComboBox>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    
                                      <dx:GridViewDataTextColumn Caption="Q.tà" Name="qtaLotto" UnboundType="Integer" VisibleIndex="6" Width="50px" Visible="true">
                                        <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                                        <DataItemTemplate>
                                            <dx:ASPxTextBox ID="tbqtaLotto" runat="server" ClientInstanceName="tbqtaLotto" Width="40px" Theme="MetropolisBlue" OnInit="tbqtaLotto_Init" OnCustomJSProperties="tbqtaLotto_CustomJSProperties">
                                                <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                                                <MaskSettings ErrorText="Valore non valido" Mask="<0..100>" />
                                            </dx:ASPxTextBox>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="ScoM" Name="scontomerceLotto" UnboundType="Integer" VisibleIndex="7" Width="55px">
                                        <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                                        <DataItemTemplate>
                                            <dx:ASPxTextBox ID="tbscontomerceLotto" runat="server" ClientInstanceName="tbscontomerceLotto" Theme="MetropolisBlue" Width="40px" OnInit="tbscontomerceLotto_Init" OnCustomJSProperties="tbscontomerceLotto_CustomJSProperties">
                                                <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                                                <MaskSettings AllowMouseWheel="false" ErrorText="Valore non valido" Mask="&lt;0..999&gt;" ShowHints="True" />
                                            </dx:ASPxTextBox>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Sco%" Name="scontopercentualeLotto" UnboundType="Integer" VisibleIndex="8" Width="55px">
                                        <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                                        <DataItemTemplate>
                                            <dx:ASPxTextBox ID="tbscontopercentualeLotto" runat="server" ClientInstanceName="tbscontopercentualeLotto" Theme="MetropolisBlue" Width="40px" OnInit="tbscontopercentualeLotto_Init" OnCustomJSProperties="tbscontopercentualeLotto_CustomJSProperties">
                                                <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                                                <MaskSettings AllowMouseWheel="false" ErrorText="Valore non valido" Mask="&lt;0..100&gt;" ShowHints="True" />
                                            </dx:ASPxTextBox>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Q.tà C." VisibleIndex="9" Width="50px" MinWidth="20" Name="qtaincart">
                                        <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False" AllowHeaderFilter="False" />
                                        <DataItemTemplate>
                                            <dx:ASPxLabel ID="lb_qtaArtLottoInCart" runat="server" OnInit="lb_qtaArtLottoInCart_Init">
                                            </dx:ASPxLabel>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="P.Fin." Name="prezzofinaleLotto" VisibleIndex="10"  Width="65px" MinWidth="40">
                                        <PropertiesTextEdit DisplayFormatString="C2">
                                        </PropertiesTextEdit>
                                        <Settings AllowAutoFilterTextInputTimer="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                                        <DataItemTemplate>
                                            <dx:ASPxLabel ID="lb_prezzofinaleLotto" runat="server" ClientInstanceName="lbprezzofinaleLotto" EnableTheming="True" OnInit="lb_prezzofinaleLotto_Init" Text="€. 0,00" Theme="MetropolisBlue">
                                            </dx:ASPxLabel>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="11" Caption=" " Name="zerobuttonLotto" ShowClearFilterButton="False" Width="35px" MinWidth="30">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="zeroPriceLotto" Visibility="AllDataRows">
                                                <Image AlternateText="Prezzo Zero" ToolTip="Imposta Riga Omaggio - Prezzo Zero" Url="~/images/zeropricesmall.png">
                                                </Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>

                                    <dx:GridViewCommandColumn ButtonType="Image" Name="addLottoToCart" Caption=" " VisibleIndex="12" ShowClearFilterButton="True" Width="35px" MinWidth="30">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="addLottoToCart" Visibility="AllDataRows">
                                                <Image AlternateText="Aggiungi al carrello" ToolTip="Aggiungi al carrello" Url="~/images/cartrowsmall.png">
                                                </Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>

                                    <dx:GridViewCommandColumn ButtonType="Image" Name="delLottoFromCart" Caption=" " VisibleIndex="13" Width="35px" MinWidth="30">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="delLottoFromCart" Visibility="AllDataRows">
                                                <Image AlternateText="Elimina dal carrello" ToolTip="Elimina dal carrello" Url="~/images/delcartrowsmall.png">
                                                </Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>

                                </Columns>
                            </dx:ASPxGridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>








    <dx:ASPxGridView ID="ASPxGridView_Cart" runat="server" Theme="MetropolisBlue" Width="100%" ClientInstanceName="grid" AutoGenerateColumns="False" KeyFieldName="CodiceArticolo">
         <ClientSideEvents CustomButtonClick="grid_CustomButtonClick" EndCallback="function(s, e) {CallBMasterCart.PerformCallback();}"></ClientSideEvents>
         <Templates>
             <EmptyDataRow>
                 Nessun risultato per la ricerca effettuata
             </EmptyDataRow>
         </Templates>
        <SettingsPager Position="TopAndBottom" ShowSeparators="True" NumericButtonCount="7" PageSize="50">
            <Summary AllPagesText="Pagine: {0} - {1} ({2} articoli)" Text="Pagina {0} di {1} ({2} articoli)"></Summary>
        </SettingsPager>

        <Settings ShowFilterBar="Auto"
            ShowVerticalScrollBar="False" UseFixedTableLayout="True"
            ShowTitlePanel="True" VerticalScrollableHeight="300"
            VerticalScrollBarMode="Hidden" ShowFilterRow="True" />

        <SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True"
            ColumnResizeMode="Control" AllowGroup="False" AllowDragDrop="False"
            AllowSort="True" />

        <SettingsCommandButton>
            <ClearFilterButton>
                <Image ToolTip="Cancella filtri di riga" Url="~/images/cancelFilter.png">
                </Image>
            </ClearFilterButton>
        </SettingsCommandButton>

        <SettingsPopup>
            <HeaderFilter Height="180px"></HeaderFilter>
        </SettingsPopup>

        <Columns>
            <dx:GridViewCommandColumn Caption=" " VisibleIndex="0" Width="20px" ShowClearFilterButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnDettaglioArticolo" Text="..." Visibility="AllDataRows">
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn Caption="Farmadati" FieldName="Farmadati"  VisibleIndex="1" Width="130px">
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataTextColumn Caption="Codice" FieldName="CodiceArticolo" ReadOnly="True" VisibleIndex="2" Width="130px" MinWidth="65">
                <Settings AllowHeaderFilter="False" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Descrizione" FieldName="Descrizione" ReadOnly="True" VisibleIndex="4">
                <Settings AutoFilterCondition="Contains" AllowHeaderFilter="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="P. Riv." FieldName="PrzRivenditore" ReadOnly="True" VisibleIndex="7" Width="65px" MinWidth="40">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="P. Pub."
                FieldName="PrzPubblico" ReadOnly="True" VisibleIndex="8" Width="65px" MinWidth="40">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
                <Settings AllowGroup="False" AllowAutoFilter="False" AllowHeaderFilter="False" AllowSort="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Formato" FieldName="Formato" ReadOnly="True" VisibleIndex="6" Width="90px">
                <Settings HeaderFilterMode="CheckedList" AutoFilterCondition="Contains" />
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="IVA%" FieldName="IVA" ReadOnly="True"  VisibleIndex="9" Width="40px">
                <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False"  AllowHeaderFilter="False" />
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Marchio" FieldName="Marchio"  ReadOnly="True" VisibleIndex="3" Width="100px" Visible="True">
                <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Composizione" FieldName="Composizione"  ReadOnly="True" VisibleIndex="5" Width="100px" MinWidth="75">
                <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Disp." FieldName="Disponibilita" Name="Disponibilita" ReadOnly="True" VisibleIndex="10" Width="55px" MinWidth="55">
                <Settings AllowAutoFilter="False" AllowGroup="False" AllowSort="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Q.tà C." VisibleIndex="15" Width="45px" MinWidth="20" Name="qtaincart">
                <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False"  AllowHeaderFilter="False" />
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lb_qtaArtInCart" runat="server" OnInit="lb_qtaArtInCart_Init">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>

            <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="17" Caption=" " Width="40px" Name="zerobutton" ShowClearFilterButton="False">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="zeroPrice" Visibility="AllDataRows">
                        <Image AlternateText="Prezzo Zero" ToolTip="Imposta Riga Omaggio - Prezzo Zero" Url="~/images/zeroPrice.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>

            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="18" Width="40px" ShowClearFilterButton="False">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="addrowtocart" Visibility="AllDataRows">
                        <Image AlternateText="Aggiungi al carrello" ToolTip="Aggiungi al carrello" Url="~/images/cartrow.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="gestMancanti" Visibility="AllDataRows">
                        <Image AlternateText="Gestione Mancanti" ToolTip="Gestione Mancanti" Url="~/images/cartrow.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>

            <dx:GridViewCommandColumn ButtonType="Image" Name="delbutton" Width="40px" Caption=" " VisibleIndex="19">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="delFromCart" Visibility="AllDataRows">
                        <Image AlternateText="Elimina dal carrello" ToolTip="Elimina dal carrello" Url="~/images/delCart.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn Caption="Macrolinea" FieldName="cod_macrolinea"  Visible="False" VisibleIndex="20">
            </dx:GridViewDataTextColumn>

            <dx:GridViewCommandColumn Caption="L" Name="detinfolotti" VisibleIndex="21" Width="20px"  MinWidth="20" ShowClearFilterButton="False">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="showLotti" Text=" # " Visibility="AllDataRows">
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            
            <dx:GridViewDataTextColumn Caption="Promozione" Name="Promozione" VisibleIndex="11" Width="154px">
                <DataItemTemplate>
                    <dx:ASPxComboBox ID="comboPromo" runat="server" Width="140px" ClientInstanceName="comboPromo" OnInit="comboPromo_Init" OnCustomJSProperties="comboPromo_CustomJSProperties" CaptionSettings-ShowColon="False">
                        <Columns>
                            <dx:ListBoxColumn FieldName="ID" Visible="False" />
                            <dx:ListBoxColumn FieldName="PROMO_CODE" Visible="False" />
                            <dx:ListBoxColumn FieldName="DESCRIPTION" Caption="Promo disponibili" />
                            <dx:ListBoxColumn FieldName="QUANTITY_MIN" Visible="False" />
                            <dx:ListBoxColumn FieldName="QUANTITY_GIFT" Visible="False" />
                            <dx:ListBoxColumn FieldName="DISCOUNT_PERCENT" Visible="False" />
                        </Columns>
                        <Buttons>
                            <dx:EditButton ToolTip="Applica">
                                <Image IconID="actions_apply_16x16">
                                </Image>
                            </dx:EditButton>
                            <dx:EditButton ToolTip="Elimina">
                                <Image IconID="actions_cancel_16x16">
                                </Image>
                            </dx:EditButton>
                        </Buttons>
                        <ClearButton DisplayMode="Never">
                        </ClearButton>
                    </dx:ASPxComboBox>
                    <dx:ASPxButton ID="btnPromoLotto" runat="server" ClientInstanceName="btnPromoLotto" Text="Promo su Lotto" OnInit="btnPromoLotto_Init" AutoPostBack="false" UseSubmitBehavior="false" CausesValidation="false" Width="140px"></dx:ASPxButton>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataTextColumn Caption="Q.tà" MinWidth="40" Name="qta" UnboundType="Integer" VisibleIndex="12" Width="55px">
                <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="tbqta" runat="server" ClientInstanceName="tbqta" OnInit="tbqta_Init" Theme="MetropolisBlue" Width="40px" OnCustomJSProperties="tbqta_CustomJSProperties">
                        <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                    </dx:ASPxTextBox>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="ScoM" Name="scontomerce" UnboundType="Integer" VisibleIndex="13" Width="55px">
                <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="tbscontomerce" runat="server" ClientInstanceName="tbscontomerce" Theme="MetropolisBlue" Width="40px" OnInit="tbscontomerce_Init" OnCustomJSProperties="tbscontomerce_CustomJSProperties">
                        <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                        <MaskSettings AllowMouseWheel="false" ErrorText="Valore non valido" Mask="&lt;0..999&gt;" ShowHints="True" />
                    </dx:ASPxTextBox>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn Caption="Sco%" Name="scontopercentuale" UnboundType="Integer" VisibleIndex="14" Width="55px">
                <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="tbscontopercentuale" runat="server" ClientInstanceName="tbscontopercentuale" Theme="MetropolisBlue" Width="40px" OnInit="tbscontopercentuale_Init" OnCustomJSProperties="tbscontopercentuale_CustomJSProperties">
                        <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                        <MaskSettings AllowMouseWheel="false" ErrorText="Valore non valido" Mask="&lt;0..100&gt;" ShowHints="True" />
                    </dx:ASPxTextBox>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            
            <dx:GridViewDataTextColumn Caption="P.Fin." MinWidth="40" Name="prezzofinale" VisibleIndex="16" Width="65px">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
                <Settings AllowAutoFilterTextInputTimer="False" AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lb_prezzofinale" runat="server" ClientInstanceName="lbprezzofinale" EnableTheming="True" OnInit="lb_prezzofinale_Init" Text="€. 0,00" Theme="MetropolisBlue">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            
        </Columns>
        <FormatConditions>
            <dx:GridViewFormatConditionHighlight FieldName="Marchio" expression="[Marchio] = 'Bioalma'" Format="YellowFillWithDarkYellowText" />
            <dx:GridViewFormatConditionHighlight FieldName="Descrizione" expression="[Marchio] = 'Bioalma'" Format="YellowFillWithDarkYellowText" />
        </FormatConditions>

        <Styles>
            <AlternatingRow Font-Bold="True">
            </AlternatingRow>
            <TitlePanel HorizontalAlign="Left" VerticalAlign="Middle">
            </TitlePanel>
        </Styles>

    </dx:ASPxGridView>
             
    <dx:ASPxGlobalEvents ID="ge" runat="server">
        <ClientSideEvents ControlsInitialized="OnControlsInitialized" />
    </dx:ASPxGlobalEvents>

    <dx:ASPxCallback ID="ASPxCallback_Mancanti" runat="server"
        ClientInstanceName="CallbackMancanti">
    </dx:ASPxCallback>

</asp:Content>
