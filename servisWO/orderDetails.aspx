<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="orderDetails.aspx.vb" Inherits="servisWO.orderDetails" %>

<%@ Register assembly="DevExpress.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="CPH_titoli" runat="server">
         dettaglio ordine
     </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    // <![CDATA[
        function OnCheckSpeTrasportoChange(s, e) {
            callbackSaveOrderHeaderFooter.PerformCallback('trasporto');
        }

        function OnSpeseIncassoChange(s, e) {
            if (e.isChangedOnServer)
                return
            if (e.isSelected) {
                callbackSaveOrderHeaderFooter.PerformCallback('pagamenti');
                callbackScontoPagamento.PerformCallback();
            }
        }

        function preventEnterKey(htmlEvent) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            }
        }

        function OnAfterPopUpResizing() {
            gridDestinazioni.AdjustControl();
        }

        function RecieveGridDestinazioniValues(values) {
            CallbackPanelDest.PerformCallback(values);
        }

        function saveCallbackComplete(s, e) {          
            var result = e.result.split('|');
            var titolo = result[0];
            var contenuto = result[1];
            popupEsito.SetHeaderText(titolo);
            popupEsito.SetContentHTML(contenuto);
            popupEsito.Show();
        }

        function onQtaValidation(s, e, index, isarcheo) {
            if (gridOrderLine.cpQtyDispoValues) {
                var QtyToAdd = s.GetValue();
                var qtyDispo = gridOrderLine.cpQtyDispoValues[index];
                var oldQty = gridOrderLine.cpqtyOldQtaValues[index];
                if (!isNaN(QtyToAdd)) {
                    if (QtyToAdd > 0) {
                        if (isarcheo == 0 && QtyToAdd > qtyDispo) {
                            popupControlloQta.SetContentHTML("La quantità <b>" + QtyToAdd + "</b> inserita è superiore alla disponibilità totale ammessa di <b>" + qtyDispo + "</b> (Disponibilità del lotto + Quantità nel carrello).");
                            popupControlloQta.Show();
                            s.SetValue(oldQty);
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

        var lastTipoInterazione = null;
        function OnTipoInterazioneChanged(s) {
            if (comboAgente.InCallback()) {
                lastTipoInterazione = s.GetValue().toString();
            }
            else {
                comboAgente.PerformCallback(s.GetValue().toString());
            }
            var tipointeraz = s.GetValue();
            if (tipointeraz != null && comboProfiliSconto.GetValue() != null) {
                gridPromoHeader.PerformCallback(tipointeraz.toString() + '|' + comboProfiliSconto.GetValue().toString());
            }
        }

        function OnComboAgenteEndCallback(s, e) {
            if (lastTipoInterazione) {
                comboAgente.PerformCallback(lastTipoInterazione);
                lastTipoInterazione = null;
            }
        }

        function OnAgenteChanged(s, e) {
            callbackSaveOrderHeaderFooter.PerformCallback('agente' + "|" + s.GetValue());
            popupAttribuzioneAgente.SetContentHTML("L'ordine sarà attibuito all'agente <b>" + s.GetText().toString() + "</b>.");
            popupAttribuzioneAgente.Show();
        }

        var azioneBtnProsegui = "";
        function callbackCtrlComplete(s, e) {
            var result = e.result.split('|');
            var tipoCtrl = result[0];
            var titolo = result[1];
            var contenuto = result[2];
            azioneBtnProsegui = result[3];
            if (tipoCtrl == "ctrlimportominimo" || tipoCtrl == "ctrlannullaconferma") {
                popupControlloConferma.SetHeaderText(titolo);
                popupControlloConferma.SetContentHTML(contenuto);
                popupControlloConferma.Show();
            } else if (tipoCtrl == "ctrlagente" || tipoCtrl == "ctrlgiacenze" || tipoCtrl == "ctrlannulla" || tipoCtrl == "ctrlinvioemail" || tipoCtrl == "ctrlriba" 
                        || (tipoCtrl == "ctrlprocesso" && azioneBtnProsegui != "produzione")
                        || (tipoCtrl == "ctrlprocesso" && azioneBtnProsegui == "produzione" && titolo == "ko")) {
                popupEsito.SetHeaderText(titolo);
                popupEsito.SetContentHTML(contenuto);
                popupEsito.Show();
            } else if (tipoCtrl == "ctrlprocesso" && azioneBtnProsegui == "produzione" && titolo == "ok") {
                popupProduzione.Show();
                popupProduzione.PerformCallback();
            }
        }

        function OnApplicaScontoHeader(s, e) {          
            var scontoHeader = tbScontoHeader.GetValue();
            console.log('OnApplicaScontoHeader: ' + scontoHeader);
            //20200424 - Gestione scontoScaglioni
            if (scontoHeader == null) {
                tbScontoHeader.SetValue("0");
                scontoHeader = "0";
            }
            var scontoHeaderApplicabile = true;
            var res = scontoHeader.split("+");
            if (res.length > 3) {
                scontoHeaderApplicabile = false;
            }
            for (i = 0; i < res.length; i++) {
                if (isNaN(res[i])) {
                    scontoHeaderApplicabile = false;
                    break;
                } else {
                    if (res[i] < 0 || res[i] > 99) {
                        scontoHeaderApplicabile = false;
                        break;
                    }
                }
            }
            console.log('scontoHeaderApplicabile: ' + scontoHeaderApplicabile);
            if (scontoHeaderApplicabile) {
                tbScontoHeader.SetValue(scontoHeader);
                tbScontoHeader.SetIsValid(true);
                callbackScontoHeader.PerformCallback();
            } else {
                tbScontoHeader.SetIsValid(false);
                tbScontoHeader.SetErrorText("Formato sconto non valido. Ex. 5+3+1");
            }
        }

        function OnCheckPromoHeaderComplete(s, e) {
            var cpScontoHeader = gridOrderLine.cpScontoHeader;
            if (cpScontoHeader != null)
                tbScontoHeader.SetValue(cpScontoHeader);
            if (gridOrderLine.cpScontoHeaderDaPromo > 0) {
                btnApplicaScontoHeader.SetEnabled(false);
                tbScontoHeader.SetEnabled(false);
            } else {
                btnApplicaScontoHeader.SetEnabled(true);
                tbScontoHeader.SetEnabled(true);
            }
        }

        function OncomboProfiliScontoChanged(s, e) {
            if (comboOrderType.GetValue() != null && comboProfiliSconto.GetValue() != null) {
                gridPromoHeader.PerformCallback(comboOrderType.GetValue().toString() + '|' + comboProfiliSconto.GetValue().toString());
            }
        }

        function OnGridPromoHeaderEndCallback(s, e){
            gridOrderLine.Refresh();
        }

        function OnSalvaNote(s, e) {
            var updatedNotes = memoNote.GetValue();
            if (updatedNotes == null)
                updatedNotes = "";
            console.log(updatedNotes);
            callbackSalvaNote.PerformCallback(updatedNotes);
        }

        function OnApplicaVoucher(s, e) {
            var voucherVal = tbValoreVoucher.GetValue();
            console.log('OnApplicaVoucher: ' + voucherVal);
            if (!(voucherVal.includes(".") && voucherVal.includes(",")) && voucherVal.split(",").length <= 2 && voucherVal.split(".").length <= 2) {
                voucherVal = voucherVal.replace(',', '.');
                if (!isNaN(parseFloat(voucherVal))) {
                    if (parseFloat(voucherVal) >= 0) {
                        callbackVoucher.PerformCallback();
                    } else {
                        popupControlloVoucher.SetContentHTML("Valore del Voucher deve essere maggiore o uguale a zero.");
                        popupControlloVoucher.Show();
                    }
                } else {
                    popupControlloVoucher.SetContentHTML("Valore del Voucher non è un numero valido.");
                    popupControlloVoucher.Show();
                }
            } else {
                popupControlloVoucher.SetContentHTML("Valore del Voucher non è un numero valido.");
                popupControlloVoucher.Show();
            }
        }

    // ]]> 
    </script>

    <dx:ASPxPopupControl ID="popupDestinazioni" runat="server"
        ClientInstanceName="popupDestinazioni" HeaderText="Destinazioni"
         Theme="MetropolisBlue" Width="720px"
        AllowDragging="True" AllowResize="True" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Height="300px" ScrollBars="Vertical">
        <ClientSideEvents AfterResizing="OnAfterPopUpResizing" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                <dx:ASPxGridView ID="gridDestinazioni" runat="server"
                    ClientInstanceName="gridDestinazioni" Theme="MetropolisBlue"
                    AutoGenerateColumns="False" Width="100%" KeyFieldName="Code">
                    <ClientSideEvents RowDblClick="function(s, e) {
	s.GetRowValues(e.visibleIndex, 'Code', RecieveGridDestinazioniValues);
	popupDestinazioni.Hide();
}" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Codice" FieldName="Code"
                            ShowInCustomizationForm="True" VisibleIndex="0" Width="60px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Destinatario" FieldName="Name"
                            ShowInCustomizationForm="True" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Indirizzo" FieldName="Address"
                            ShowInCustomizationForm="True" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Città" FieldName="City"
                            ShowInCustomizationForm="True" VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CAP" FieldName="Post Code"
                            ShowInCustomizationForm="True" VisibleIndex="4" Width="60px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Prov." FieldName="County"
                            ShowInCustomizationForm="True" VisibleIndex="5" Width="60px">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
                    <SettingsPager Mode="ShowAllRecords" Visible="False">
                    </SettingsPager>
                </dx:ASPxGridView>
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
            <dx:PopupControlContentControl runat="server"></dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

        <dx:ASPxPopupControl ID="popupControlloVoucher" runat="server"
        ClientInstanceName="popupControlloVoucher" EnableTheming="True"
        HeaderText="Controllo Voucher" Modal="True" PopupAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Theme="MetropolisBlue" Width="420px">
        <ContentStyle Font-Size="14pt">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server"></dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupAttribuzioneAgente" runat="server"
        ClientInstanceName="popupAttribuzioneAgente" HeaderText="Assegnazione ordine ad agente" Height="180px"
        Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter"
        Theme="MetropolisBlue" Width="360px">
        <ContentStyle Font-Size="11pt">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True"></dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupControlloConferma" runat="server"
        ClientInstanceName="popupControlloConferma" HeaderText="Attenzione" Height="180px"
        Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter"
        Theme="MetropolisBlue" Width="360px" ShowFooter="True">
        <ContentStyle Font-Size="11pt">
        </ContentStyle>
        <FooterTemplate>
            <div style="text-align: center;">
                <table style="width: 100%; text-align: center" align="center" width="100%">
                    <tr>
                        <td style="width: 50%">
                            <dx:ASPxButton ID="btnImpMinOK" runat="server" Text="Prosegui" Theme="MetropolisBlue" ClientInstanceName="btnImpMinOK" AutoPostBack="False" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {popupControlloConferma.Hide(); callbackProcess.PerformCallback(azioneBtnProsegui);}" />
                            </dx:ASPxButton>
                        </td>
                        <td style="width: 50%">
                            <dx:ASPxButton ID="btnImpMinKO" runat="server" Text="Annulla" Theme="MetropolisBlue" ClientInstanceName="btnImpMinKO" AutoPostBack="False" UseSubmitBehavior="False">
                                <ClientSideEvents Click="function(s, e) {popupControlloConferma.Hide();}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr><td style="height:10px"></td></tr>
                </table>
            </div>
        </FooterTemplate>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True"></dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupEsito" runat="server"
        ClientInstanceName="popupEsito" Font-Size="Large"
        HeaderText="Esito salvataggio" Height="120px" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Style="text-align: left" Theme="MetropolisBlue"
        Width="320px">
        <ClientSideEvents Closing="function(s, e) {
	//window.location.reload();
}" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                Problemi di connessione al database. Riprovare tra qualche istante.
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="popupOrdineBloccato" runat="server"
        ClientInstanceName="popupOrdineBloccato" Font-Size="Large"
        HeaderText="Attenzione" Height="160px" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Style="text-align: left" Theme="MetropolisBlue"
        Width="400px">
        <ClientSideEvents Closing="function(s, e) {
	//window.location.reload();
}" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                L&#39;ordine è bloccato
                <br />
                e non è modificabile.
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
        EnableTheming="True" Theme="MetropolisBlue" Width="100%" Height="285px" ContentStyle-BackColor="#F0F0F0">
        <TabPages>
            <dx:TabPage Name="tab_orderHeader" Text="Testata Ordine">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxFormLayout ID="ASPxFormLayout_orderHeader" runat="server" EnableTheming="True" Theme="MetropolisBlue" Width="100%">
                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="640" />
                            <Items>
                                <dx:LayoutGroup ColCount="8" ShowCaption="False" GroupBoxDecoration="None">
                                    <Items>
                                        <dx:LayoutItem Caption="Numero ordine" Width="8%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxLabel ID="lb_orderNr" runat="server" Font-Bold="True">
                                                    </dx:ASPxLabel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Data ricezione ordine" Width="12%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxDateEdit ID="comboOrderDate" runat="server">
                                                    </dx:ASPxDateEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Data evasione ordine" Width="12%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxDateEdit ID="comboDataEvsione" runat="server">
                                                    </dx:ASPxDateEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Tipo interazione" Width="14%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxComboBox ID="comboOrderType" runat="server" SelectedIndex="0" ClientInstanceName="comboOrderType">
                                                        <Items>
                                                            <dx:ListEditItem Text="Telefonata in Ingresso" Value="Telefonata in Ingresso" Selected="True" />
                                                            <dx:ListEditItem Text="Telefonata in Uscita" Value="Telefonata in Uscita" />
                                                            <dx:ListEditItem Text="Ordine via Fax" Value="Ordine via Fax" />
                                                            <dx:ListEditItem Text="Ordine via Email" Value="Ordine via Email" />
                                                            <dx:ListEditItem Text="Visita Agente" Value="Visita Agente" />
                                                        </Items>
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnTipoInterazioneChanged(s); }" />
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Agente" Width="14%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxComboBox ID="comboAgente" runat="server" ClientInstanceName="comboAgente">
                                                        <ClientSideEvents EndCallback="OnComboAgenteEndCallback" SelectedIndexChanged="function(s, e) { OnAgenteChanged(s,e); }"/>
                                                    </dx:ASPxComboBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Applica spese di trasporto" Width="12%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxCallbackPanel ID="CallbackPanel_speTrasporto" runat="server"
                                                        ClientInstanceName="CallbackPanelspeTrasporto" Width="200px">
                                                        <PanelCollection>
                                                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                                <dx:ASPxCheckBox ID="cb_speseTrasporto" runat="server" CheckState="Unchecked">
                                                                    <ClientSideEvents CheckedChanged="OnCheckSpeTrasportoChange" />
                                                                </dx:ASPxCheckBox>
                                                                <dx:ASPxLabel ID="lb_speseTrasporto" runat="server">
                                                                </dx:ASPxLabel>
                                                            </dx:PanelContent>
                                                        </PanelCollection>
                                                    </dx:ASPxCallbackPanel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Stato" Width="12%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxLabel ID="lb_orderStatus" runat="server" Font-Bold="True" Text="[status]">
                                                    </dx:ASPxLabel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Riferimento ordine" Width="16%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxLabel ID="lb_riferimentoOrdine" runat="server" Font-Bold="True">
                                                    </dx:ASPxLabel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutGroup ColCount="5" ShowCaption="False" GroupBoxDecoration="None">
                                    <Items>
                                        <dx:LayoutItem Caption="Cliente" Width="50%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxLabel ID="lb_customerNo" runat="server" Font-Bold="True">
                                                    </dx:ASPxLabel>
                                                    &nbsp;-&nbsp;
                                                    <dx:ASPxLabel ID="lb_customerName" runat="server" Font-Bold="True">
                                                    </dx:ASPxLabel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:EmptyLayoutItem Width="5%">
                                        </dx:EmptyLayoutItem>
                                        <dx:LayoutItem Caption="Creato da" Width="15%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxLabel ID="lb_utentecreazione" runat="server" Cursor="pointer" Font-Bold="True" Text="[ - ]">
                                                    </dx:ASPxLabel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Modificato da" Width="15%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxLabel ID="lb_OperatorCode" runat="server" Font-Bold="True" Text="[ - ]" Cursor="pointer">
                                                    </dx:ASPxLabel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Magazziniere" Width="15%">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                     <dx:ASPxLabel ID="lb_user" runat="server" Font-Bold="True" Text="[ - ]">
                                                         </dx:ASPxLabel>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutGroup ShowCaption="False" Caption="Note" GroupBoxDecoration="None" VerticalAlign="Top" ColCount="3">
                                    <Items>
                                        <dx:LayoutGroup Caption="Sconto % su totale ordine" ColCount="2" Width="20%" Height="100%" GroupBoxDecoration="Default" GroupBoxStyle-Caption-BackColor="#F0F0F0" GroupBoxStyle-Caption-ForeColor="#339999">
<GroupBoxStyle>
<Caption BackColor="#F0F0F0" ForeColor="#339999"></Caption>
</GroupBoxStyle>
                                            <Items>
                                                <dx:LayoutItem Caption="Sconto%" ShowCaption="False" Width="50%">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxTextBox ID="tbScontoHeader" runat="server" Theme="MetropolisBlue" ClientInstanceName="tbScontoHeader">
                                                                <ValidationSettings ValidateOnLeave="true" EnableCustomValidation="true" ErrorDisplayMode="Text" ErrorTextPosition="Bottom"></ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Applica sconto%" ShowCaption="False" Width="50%">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxButton ID="btnApplicaScontoHeader" runat="server" AutoPostBack="False" CausesValidation="False" Text="Applica" Theme="MetropolisBlue" UseSubmitBehavior="False" ClientInstanceName="btnApplicaScontoHeader">
                                                                <ClientSideEvents Click="function(s, e) {OnApplicaScontoHeader(s);}" />
                                                            </dx:ASPxButton>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                        <dx:LayoutGroup Caption="Sconto Voucher" ColCount="2" Width="20%" Height="100%" GroupBoxDecoration="Default" GroupBoxStyle-Caption-BackColor="#F0F0F0" GroupBoxStyle-Caption-ForeColor="#339999">
<GroupBoxStyle>
<Caption BackColor="#F0F0F0" ForeColor="#339999"></Caption>
</GroupBoxStyle>
                                            <Items>
                                                <dx:LayoutItem Caption="Valore Voucher" ShowCaption="False" Width="50%">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxTextBox ID="tbValoreVoucher" runat="server" Theme="MetropolisBlue" ClientInstanceName="tbValoreVoucher">
                                                            </dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Applica Voucher" ShowCaption="False" Width="50%">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxButton ID="btnApplicaVoucher" runat="server" AutoPostBack="False" CausesValidation="False" Text="Applica" Theme="MetropolisBlue" UseSubmitBehavior="False" ClientInstanceName="btnApplicaVoucher">
                                                                <ClientSideEvents Click="function(s, e) {OnApplicaVoucher(s);}" />
                                                            </dx:ASPxButton>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                        <dx:LayoutGroup Caption="Note" Width="60%" Height="100%" ColCount="2" GroupBoxDecoration="Default" GroupBoxStyle-Caption-BackColor="#F0F0F0" GroupBoxStyle-Caption-ForeColor="#339999">
<GroupBoxStyle>
<Caption BackColor="#F0F0F0" ForeColor="#339999"></Caption>
</GroupBoxStyle>
                                            <Items>
                                                <dx:LayoutItem Caption="Note" ShowCaption="False" Width="85%">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxMemo ID="memoNote" runat="server" ClientInstanceName="memoNote" Height="36px" MaxLength="250" Theme="MetropolisBlue">
                                                            </dx:ASPxMemo>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Salva note" ShowCaption="False" Width="15%">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxButton ID="btnSalvaNote" runat="server" AutoPostBack="False" CausesValidation="False" ClientInstanceName="btnSalvaNote" Text="Salva note" Theme="MetropolisBlue" UseSubmitBehavior="False">
                                                                <ClientSideEvents Click="function(s, e) {
	OnSalvaNote(s);
}" />
                                                            </dx:ASPxButton>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                </dx:LayoutItem>
                                            </Items>
                                        </dx:LayoutGroup>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                            <SettingsItemCaptions HorizontalAlign="Left" Location="Top" VerticalAlign="Top" />
                        </dx:ASPxFormLayout>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Name="tab_fatturazione" Text="Dati Fatturazione">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table class="dxflInternalEditorTable_BlackGlass">
                            <tr>
                                <td style="width: 20%">
                                Codice Cliente</td>
                                <td style="width: 20%">
                                Partita IVA</td>
                                <td style="width: 20%">
                                Codice fiscale</td>
                                <td style="width: 20%">
                                &nbsp;</td>
                                <td style="width: 20%">
                                Bloccato</td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxTextBox ID="tb_no" runat="server" Width="100%" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_piva" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_codfis" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                &nbsp;</td>
                                <td>
                                    <dx:ASPxCheckBox ID="cb_bloccato" runat="server" CheckState="Unchecked" 
                                    Text="[ragione blocco]" ReadOnly="True">
                                    </dx:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                Nome</td>
                                <td>
                                &nbsp;</td>
                                <td>
                                Indirizzo</td>
                                <td>
                                &nbsp;</td>
                                <td>
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxTextBox ID="tb_nome" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td colspan="2" style="width: 80%">
                                    <dx:ASPxTextBox ID="tb_indirizzo" runat="server" ReadOnly="True" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                CAP</td>
                                <td>
                                Città</td>
                                <td>
                                Provincia</td>
                                <td>
                                Regione</td>
                                <td>
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxTextBox ID="tb_cap" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_citta" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_provincia" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_Regione" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                Telefono</td>
                                <td>
                                Fax</td>
                                <td>
                                Cellulare</td>
                                <td>
                                Email</td>
                                <td>
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxTextBox ID="tb_telefono" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_fax" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_cellulare" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                    <dx:ASPxTextBox ID="tb_email" runat="server" Width="100%" ReadOnly="True">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>
                                &nbsp;</td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Name="tab_spedizione" Text="Dati Spedizione">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxCallbackPanel ID="CallbackPanel_Dest" runat="server" Width="100%" 
                        ClientInstanceName="CallbackPanelDest">
                            <PanelCollection>
                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                    <table class="dxflInternalEditorTable_BlackGlass">
                                        <tr>
                                            <td style="width: 20%">
                                            Destinatario merce</td>
                                            <td colspan="3" style="width: 20%">
                                            &nbsp;</td>
                                            <td style="width: 20%">
                                            &nbsp;</td>
                                            <td style="width: 20%" rowspan="2">
                                                &nbsp;</td>
                                            <td style="width: 20%">
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <dx:ASPxTextBox ID="tb_desCode" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="btnDestinazione" runat="server" AutoPostBack="False" 
                                                    Text="Destinazione ... " Theme="MetropolisBlue">
                                                    <ClientSideEvents Click="function(s, e) { gridDestinazioni.PerformCallback(); popupDestinazioni.Show(); }" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                            Nome</td>
                                            <td>
                                            &nbsp;</td>
                                            <td colspan="3">
                                            Indirizzo</td>
                                            <td>
                                            &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="tb_desNome" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="tb_desIndirizzo" runat="server" ReadOnly="True" 
                                                Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                            CAP</td>
                                            <td colspan="3">
                                            Città</td>
                                            <td>
                                            Provincia</td>
                                            <td>
                                            Regione</td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desCap" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="tb_desCitta" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desProvincia" runat="server" ReadOnly="True" 
                                                Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desRegione" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                            Telefono</td>
                                            <td colspan="3">
                                            Fax</td>
                                            <td>
                                            Email</td>
                                            <td>
                                            &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desTelefono" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="tb_desFax" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="tb_desEmail" runat="server" ReadOnly="True" Width="100%">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                            &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Corriere da utilizzare</td>
                                            <td colspan="3">
                                            &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxComboBox ID="comboCorrieri" runat="server" Theme="Aqua" Width="100%">
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td colspan="3">
                                            &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                            <td>
                                            &nbsp;</td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Name="tab_pagamento" Text="Dati Pagamento">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <table class="dxflInternalEditorTable_BlackGlass">
                            <tr>
                                <td>
                                    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_pagamenti" runat="server" 
                                        ClientInstanceName="callbackPagamenti" Width="100%">
                                        <PanelCollection>
                                            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                                <dx:ASPxGridView ID="grigliaPagamenti" runat="server" 
                                                    AutoGenerateColumns="False" KeyFieldName="Numeratore" Width="500px">
                                                    <ClientSideEvents SelectionChanged="OnSpeseIncassoChange" />
                                                    <Columns>
                                                        <dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
                                                            ShowSelectCheckbox="True" VisibleIndex="0" Width="30px">
                                                        </dx:GridViewCommandColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Metodo" ShowInCustomizationForm="True" 
                                                            Visible="False" VisibleIndex="2">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Termine" ShowInCustomizationForm="True" 
                                                            Visible="False" VisibleIndex="3">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Metodo e termine di pagamento" 
                                                            FieldName="Descrizione" ShowInCustomizationForm="True" VisibleIndex="4">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Sconto %" FieldName="Sconto" 
                                                            ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
                                                            <PropertiesTextEdit DisplayFormatString="F2">
                                                            </PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Numeratore" 
                                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Predefinito" 
                                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsBehavior AllowDragDrop="False" AllowGroup="False" 
                                                        AllowSelectSingleRowOnly="True" AllowSort="False" />
                                                    <SettingsPager Visible="False">
                                                    </SettingsPager>
                                                </dx:ASPxGridView>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>
                                </td>
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Name="tab_promo" Text="Promo attive su cliente" Visible="False">
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxGridView ID="ASPxGridView_promo" runat="server" Theme="MetropolisBlue" 
                            Width="100%">
                        </dx:ASPxGridView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
        <Paddings Padding="10px" />

<ContentStyle BackColor="#F0F0F0"></ContentStyle>

        <BorderBottom BorderStyle="Solid" BorderWidth="2px" BorderColor="#4E4F51" />
   <%--     <BorderLeft BorderStyle="Solid" BorderWidth="2px" BorderColor="#0088EE" />
         <BorderRight BorderStyle="Solid" BorderWidth="2px" BorderColor="#0088EE" />--%>
        <ActiveTabStyle Height="30px"></ActiveTabStyle>
    </dx:ASPxPageControl>

    <dx:ASPxFormLayout ID="ASPxFormLayout_promoHeader" runat="server" Width="100%" Theme="MetropolisBlue" BackColor="#FFFFFF">
        <Items>
            <dx:LayoutGroup Caption="Sconti promozionali su totale ordine" Width="100%" GroupBoxDecoration="HeadingLine" GroupBoxStyle-Caption-BackColor="#FFFFFF" GroupBoxStyle-Caption-ForeColor="#339999">
                <Items>
                    <dx:LayoutItem Caption="Promozioni in corso" ShowCaption="False">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxGridView ID="gridPromoHeader" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridPromoHeader" KeyFieldName="ID" Theme="MetropolisBlue" Width="100%">
                                    <ClientSideEvents EndCallback="function(s, e) {	OnGridPromoHeaderEndCallback(); }" />
                                    <Templates>
                                        <TitlePanel>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left">
                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Profilo di sconto" Visible="false">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxComboBox ID="comboProfiliSconto" runat="server" ClientInstanceName="comboProfiliSconto" ValueType="System.Int32" Width="280px" OnInit="comboProfiliSconto_Init">
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {OncomboProfiliScontoChanged(s,e);}" />
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </TitlePanel>
                                        </Templates>
                                    <SettingsPager Visible="False">
                                    </SettingsPager>
                                    <Settings ShowTitlePanel="True" />
                                    <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
                                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn FieldName="ID" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PROMO_CODE" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="CT_CODE" ShowInCustomizationForm="True" VisibleIndex="2" Caption="Codice contatto">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Importo Ordine MIN" FieldName="TOTAL_ORDER_MIN" ShowInCustomizationForm="True" VisibleIndex="3">
                                            <PropertiesTextEdit DisplayFormatString="C2">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Importo Ordine Max" FieldName="TOTAL_ORDER_MAX" ShowInCustomizationForm="True" VisibleIndex="4">
                                            <PropertiesTextEdit DisplayFormatString="C2">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Sconto %" FieldName="DISCOUNT_PERCENT" ShowInCustomizationForm="True" VisibleIndex="5">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Valida dal" FieldName="PROMO_DATA_INIZIO" ShowInCustomizationForm="True" VisibleIndex="6">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Scadenza" FieldName="PROMO_DATA_FINE" ShowInCustomizationForm="True" VisibleIndex="7">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowInCustomizationForm="True" VisibleIndex="8" Width="45px" Visible="False">
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="btnApplica" Visibility="AllDataRows">
                                                    <Image IconID="actions_apply_32x32" ToolTip="Applica promozione">
                                                    </Image>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowInCustomizationForm="True" VisibleIndex="9" Width="45px" Visible="False">
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="btnRimuovi" Visibility="AllDataRows">
                                                    <Image IconID="actions_cancel_32x32" ToolTip="Rimuovi promozione">
                                                    </Image>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:GridViewCommandColumn>
                                    </Columns>
                                    <Styles>
                                        <Header HorizontalAlign="Center">
                                        </Header>
                                        <Cell HorizontalAlign="Center">
                                        </Cell>
                                    </Styles>
                                </dx:ASPxGridView>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
                <SettingsItemCaptions Location="Top" />
            </dx:LayoutGroup>
        </Items>
    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#CCCCCC" />
    </dx:ASPxFormLayout>

    <table style="width: 100%; border-top: 1px solid #FFFFFF; border-bottom: 1px solid #FFFFFF; margin-top: 10px;" width="100%">
        <tr>
            <td style="width: 100%;">
                <dx:ASPxGridView ID="gridOrderLine" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="MetropolisBlue" Width="100%" 
                    ClientInstanceName="gridOrderLine" EnableRowsCache="False">
                    <ClientSideEvents EndCallback="function(s, e) {
	CallbackPanelspeTrasporto.PerformCallback();
	callbackfooter.PerformCallback();
	CallBMasterCart.PerformCallback();
    OnCheckPromoHeaderComplete();
}" />
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="TotaleRiga" SummaryType="Sum" DisplayFormat="c0" ShowInGroupFooterColumn="TotaleRiga"  />
                        <dx:ASPxSummaryItem ShowInColumn="TotaleRiga"  FieldName="TotaleRiga" SummaryType="Custom" Tag="iva" DisplayFormat="c0" />
                    </TotalSummary>
                    <Columns>
 <dx:GridViewDataTextColumn Caption="Codice" FieldName="ordineLine.ItemCode" ReadOnly="True" 
                VisibleIndex="1" Width="110px">
                <Settings AllowHeaderFilter="False" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Descrizione" 
                ReadOnly="True" VisibleIndex="4" Name="Descrizione" 
                            FieldName="DESCRIZIONE">
                <Settings AutoFilterCondition="Contains" AllowHeaderFilter="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Prezzo Riv." FieldName="ordineLine.UnitPrice"
                ReadOnly="True" VisibleIndex="8" Width="74px">
                 <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="False" AllowGroup="False" AllowHeaderFilter="False" 
                    AllowSort="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Prezzo Pub." ReadOnly="True" VisibleIndex="9" 
                            Width="74px" Name="PrezzoPub" FieldName="UNITPRICELIST">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
                <Settings AllowGroup="False" AllowAutoFilter="False" AllowHeaderFilter="False" 
                    AllowSort="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Formato" 
                ReadOnly="True" VisibleIndex="6" Width="60px" Name="Formato" FieldName="FORMATO">
                <Settings HeaderFilterMode="CheckedList" AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="IVA%" ReadOnly="True" VisibleIndex="15" Width="40px" 
                            Name="Iva" FieldName="IVA">
                <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False" 
                    AllowHeaderFilter="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Marchio" 
                ReadOnly="True" VisibleIndex="3" Width="100px" Name="Marchio" Visible="False" 
                            FieldName="MARCHIO">
                <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Composizione" 
                ReadOnly="True" VisibleIndex="5" Width="88px" Name="Composizione" 
                            FieldName="COMPOSIZIONE">
                <Settings AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Dispo Tot." 
                ReadOnly="True" VisibleIndex="13" Width="65px" Name="Disponibilita" 
                            FieldName="DISPONIBILITA" Visible="False">
                <Settings AllowAutoFilter="False" AllowGroup="False" AllowSort="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Q.tà" UnboundType="Integer" VisibleIndex="7" Width="55px" Name="Quantita" ReadOnly="True">
                <Settings AllowAutoFilter="False" AllowSort="False" AllowGroup="False" AllowHeaderFilter="False" AllowDragDrop="False" />
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="tbqta" runat="server" ClientInstanceName="tbqta" oninit="tbqta_Init" Theme="MetropolisBlue" Width="40px">
                        <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                    </dx:ASPxTextBox>
                    <dx:ASPxLabel ID="lb_Qta" runat="server" oninit="lb_Qta_Init" Visible="False" Width="40px">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Totale Riga" Name="totaleriga" 
                VisibleIndex="16" Width="100px" FieldName="TOTALERIGA" 
                UnboundType="Decimal">
                    <PropertiesTextEdit DisplayFormatString="C2">
                    </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Right">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="20" Width="40px" Caption=" " 
                            Name="delbutton">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="delFromCart" Visibility="AllDataRows">
                        <Image AlternateText="Elimina dal carrello" ToolTip="Elimina dal carrello" Url="~/images/delCart.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="17" Width="40px" Caption=" " Name="zerobutton" ShowClearFilterButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="zeroPrice" Visibility="AllDataRows">
                        <Image AlternateText="Prezzo Zero" ToolTip="Imposta Riga Omaggio - Prezzo Zero" Url="~/images/zeroPrice.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
                        
                        <dx:GridViewCommandColumn ButtonType="Image" Caption=" " Name="updbutton" 
                            VisibleIndex="18" Width="40px">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="updCart" Visibility="AllDataRows">
                                    <Image AlternateText="Aggiona carrello" ToolTip="Aggiorna carrello" 
                                        Url="~/images/updCart.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Sct%" VisibleIndex="10" Width="40px" 
                            FieldName="ordineLine.FormulaSconto" Visible="False">
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Line Nr." FieldName="ordineLine.LineNo" 
                            ReadOnly="True" VisibleIndex="0" Width="60px" Visible="False">
                            <Settings AllowAutoFilter="False" AllowDragDrop="False" />
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Lotto" FieldName="ordineLine.LotNo" Name="Lotto" 
                            VisibleIndex="2" Width="60px">
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="IDPROMO" FieldName="IDPROMO" 
                            Visible="False" VisibleIndex="19">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="DiscountQty" FieldName="ordineLine.DiscountQty" 
                            Visible="False" VisibleIndex="21">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="RowDiscount" FieldName="ordineLine.RowDiscount" 
                            Visible="False" VisibleIndex="22">
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Dispo Lotto" FieldName="DISPOLOTTO" 
                            VisibleIndex="14" Width="75px">
                            <Settings AllowAutoFilter="False" AllowGroup="False" AllowSort="False" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Sco%R" Name="scontopercentuale" UnboundType="Integer" VisibleIndex="11" Width="55px">
                            <Settings AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                            <DataItemTemplate>
                                <dx:ASPxTextBox ID="tbscontopercentuale" runat="server" ClientInstanceName="tbscontopercentuale" OnInit="tbscontopercentuale_Init" Theme="MetropolisBlue" Width="40px">
                                    <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                                    <MaskSettings AllowMouseWheel="false" ErrorText="Valore non valido" Mask="&lt;0..100&gt;" ShowHints="True" />
                                </dx:ASPxTextBox>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        
                        <dx:GridViewDataTextColumn Caption="Sco%T" Name="scontoheader" UnboundType="String" VisibleIndex="12" Width="70px">
                            <Settings AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="lbScontoHeader" runat="server" OnInit="lbScontoHeader_Init" Text="0">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="ScoPag" Name="scontopag" UnboundType="Integer" VisibleIndex="13" Width="55px">
                            <Settings AllowGroup="False" AllowHeaderFilter="False" AllowSort="False" />
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="lbScontoRigaPag" runat="server" OnInit="lbScontoRigaPag_Init" Text="0">
                                </dx:ASPxLabel>
                            </DataItemTemplate>
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        
                    </Columns>
                    <SettingsBehavior AllowGroup="False" AllowSort="False" AllowDragDrop="False" ColumnResizeMode="Control" />
                    <SettingsPager Mode="ShowAllRecords">
                    </SettingsPager>
                    <Styles>
                        <Footer Font-Bold="true">
                        </Footer>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>

     <table id="tabMancanti" runat="server" style="width: 100%; margin-top: 10px;" width="100%">
         <tr>
             <td>
                <dx:ASPxGridView ID="gridMancanti" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridMancanti" EnableTheming="True" Theme="MetropolisBlue" Width="100%" Settings-ShowPreview="False" 
                    Settings-ShowTitlePanel="True" SettingsText-Title="Articoli mancanti" Styles-TitlePanel-BackColor="#FFFFFF" Styles-TitlePanel-HorizontalAlign="Left" Styles-TitlePanel-ForeColor="#333333" >
                     <SettingsPager Visible="False" Mode="ShowAllRecords">
                     </SettingsPager>
                     <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
                     <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                     <Columns>
                         <dx:GridViewDataTextColumn Caption="Codice articolo mancante" FieldName="ItemCode" VisibleIndex="0" Width="158px">
                         </dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn Caption="Quantità" FieldName="Quantity" VisibleIndex="4" Width="60px">
                         </dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn FieldName="Descrizione" VisibleIndex="1" Width="360px">
                         </dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn FieldName="Composizione" VisibleIndex="2" Width="90px">
                         </dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn FieldName="Formato" VisibleIndex="3" Width="60px">
                         </dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn Caption=" " FieldName="Note" VisibleIndex="5">
                         </dx:GridViewDataTextColumn>
                     </Columns>
                     <FormatConditions>
                         <dx:GridViewFormatConditionHighlight ApplyToRow="True" Expression="[Note] &lt;&gt; ?" Format="Custom">
                             <RowStyle BackColor="Red" ForeColor="White" />
                         </dx:GridViewFormatConditionHighlight>
                     </FormatConditions>
                 </dx:ASPxGridView>
             </td>
         </tr>
     </table>

    <table style="width: 100%; margin-top: 10px; margin-bottom: 10px; padding-bottom: 10px;" width="100%">
        <tr>
            <td>
                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_footer" runat="server"
                    ClientInstanceName="callbackfooter" Width="100%" EnableTheming="True"
                    Theme="MetropolisBlue">
                    <ClientSideEvents EndCallback="function(s, e) {
	CallBMasterCart.PerformCallback();
}" />
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxFormLayout ID="ASPxFormLayout_footer" runat="server"
                                ClientInstanceName="footer" EnableTheming="True" Theme="MetropolisBlue" BackColor="#F0F0F0"
                                Width="100%">
                                <Items>
                                    <dx:LayoutGroup ColCount="3" ShowCaption="False" GroupBoxDecoration="None">
                                        <Items>
                                            <dx:LayoutItem Caption="Spese di trasporto" Width="30%">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server"
                                                        SupportsDisabledAttribute="True">
                                                        <dx:ASPxLabel ID="lb_footerSpeseTrasporto" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:EmptyLayoutItem Width="40%">
                                            </dx:EmptyLayoutItem>
                                            <dx:LayoutItem Caption="Totale imponibile" Width="30%">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server"
                                                        SupportsDisabledAttribute="True">
                                                        <dx:ASPxLabel ID="lb_footerTotaleImponibile" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem Caption="Spese di incasso">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server"
                                                        SupportsDisabledAttribute="True">
                                                        <dx:ASPxLabel ID="lb_footerSpeseIncasso" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:LayoutItem Caption="Sconto pagamento">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server"
                                                        SupportsDisabledAttribute="True">
                                                        <dx:ASPxLabel ID="lb_footerScontoPagamento" runat="server"
                                                            Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:LayoutItem Caption="Sconto su pagamento %">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server"
                                                        SupportsDisabledAttribute="True">
                                                        <dx:ASPxLabel ID="lb_footerScontoPagamentoPerc" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:LayoutItem Caption="Totale imponibile netto">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server"
                                                        SupportsDisabledAttribute="True">
                                                        <dx:ASPxLabel ID="lb_footerTotaleImponibileNetto" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:LayoutItem Caption="Totale IVA">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server"
                                                        SupportsDisabledAttribute="True">
                                                        <dx:ASPxLabel ID="lb_footerTotIVA" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                             <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:LayoutItem Caption="Valore Voucher">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxLabel ID="lb_footerVoucher" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                            <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:EmptyLayoutItem>
                                            </dx:EmptyLayoutItem>
                                            <dx:LayoutItem Caption="Totale ordine">
                                                <LayoutItemNestedControlCollection>
                                                    <dx:LayoutItemNestedControlContainer runat="server">
                                                        <dx:ASPxLabel ID="lb_footerTotaleOrdine" runat="server" Font-Bold="True">
                                                        </dx:ASPxLabel>
                                                    </dx:LayoutItemNestedControlContainer>
                                                </LayoutItemNestedControlCollection>
                                            </dx:LayoutItem>
                                        </Items>
                                    </dx:LayoutGroup>
                                </Items>
                                <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />

                                <SettingsItemCaptions Location="Top" VerticalAlign="Bottom"></SettingsItemCaptions>
                                <BorderBottom BorderStyle="Solid" BorderWidth="1px" />
                                <BorderTop BorderStyle="Solid" BorderWidth="1px" />
                            </dx:ASPxFormLayout>
                            <div style="background-color:#F9F9F9; text-align: center; width:100%; margin-bottom:20px; padding-top:4px; padding-bottom:4px; margin-top:0px; border-bottom: 1px solid #4E4F51;">
                                <table style=" display:inline-table; width:auto;">
                                    <tr>
                                        <td>
                                            <dx:ASPxButton runat="server" Text="Salva Ordine" Theme="MetropolisBlue"
                                                ID="btn_SaveOrder" Width="160px" AutoPostBack="False"
                                                CausesValidation="False" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {
	callbackCtrl.PerformCallback('salva');
}" />
                                                <Image IconID="save_save_32x32">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxButton runat="server" Text="Salva Ordine e Lista" Theme="MetropolisBlue"
                                                ID="btn_SaveOrderAndNew" Width="160px" AutoPostBack="False"
                                                CausesValidation="False">
                                                <ClientSideEvents Click="function(s, e) {
	callbackCtrl.PerformCallback('salvaelista');
}" />
                                                <Image IconID="save_saveandnew_32x32">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxButton ID="btn_SaveAndCtrl" runat="server" Text="Richiedi Approvazione"
                                                Theme="MetropolisBlue" Width="160px" AutoPostBack="False"
                                                CausesValidation="False">
                                                <ClientSideEvents Click="function(s, e) {
	callbackCtrl.PerformCallback('approvazione');
}" />
                                                <Image IconID="save_savedialog_32x32">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                        <td >
                                            <dx:ASPxButton ID="btn_Produci" runat="server" Text="Richiedi Produzione Archeo"
                                                Theme="MetropolisBlue" Width="160px" AutoPostBack="False"
                                                CausesValidation="False">
                                                <ClientSideEvents Click="function(s, e) {
callbackCtrl.PerformCallback('produzione');
}" />
                                                <Image IconID="support_version_32x32">
                                                </Image>

                                            </dx:ASPxButton>
                                        </td>
                                        <td >
                                            <dx:ASPxButton ID="btn_SaveAndSend" runat="server" Text="Invia a Magazzino"
                                                Theme="MetropolisBlue" Width="160px" AutoPostBack="False"
                                                CausesValidation="False">
                                                <ClientSideEvents Click="function(s, e) {
	callbackCtrl.PerformCallback('invia');
}" />
                                                <Image IconID="businessobjects_boorderitem_32x32">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                        <td >
                                            <dx:ASPxButton ID="btn_Report" runat="server" Text="Visualizza Report"
                                                Theme="MetropolisBlue" Width="160px" AutoPostBack="False"
                                                CausesValidation="False">
                                                <ClientSideEvents Click="function(s, e) {
	callbackReport.PerformCallback();
}" />
                                                <Image IconID="export_exporttopdf_32x32">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                        <td >
                                            <dx:ASPxButton ID="btn_SaveCarrello" runat="server" Text="Salva carrello"
                                                Theme="MetropolisBlue" Width="160px" AutoPostBack="False"
                                                CausesValidation="False">
                                                <ClientSideEvents Click="function(s, e) {
	callbackSaveCarrello.PerformCallback();
}" />
                                                <Image IconID="other_shoppingcart_32x32gray">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                        <td >
                                            <dx:ASPxButton ID="btn_annullaCarrello" runat="server" Text="Annulla Carrello/Ordine"
                                                Theme="MetropolisBlue" Width="160px" AutoPostBack="False" CausesValidation="False" UseSubmitBehavior="False">
                                                <ClientSideEvents Click="function(s, e) {
	callbackCtrl.PerformCallback('annulla');
}" />
                                                <Image IconID="actions_deleteitem_32x32gray">
                                                </Image>
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </td>
        </tr>
    </table>




            <dx:ASPxCallback ID="ASPxCallback_report" runat="server" 
        ClientInstanceName="callbackReport">
                <ClientSideEvents EndCallback="function(s, e) {
	loadingpanel.Hide();
	popupReport.Show();
}" BeginCallback="function(s, e) {
	loadingpanel.Show();
}" />
    </dx:ASPxCallback>

            <dx:ASPxCallback ID="callbackCtrl" runat="server" 
        ClientInstanceName="callbackCtrl">
                <ClientSideEvents CallbackComplete="callbackCtrlComplete" BeginCallback="function(s, e) {
	savingpanel.Show();	
}" EndCallback="function(s, e) {
	savingpanel.Hide();
}" />
    </dx:ASPxCallback>

            <dx:ASPxCallback ID="callbackProcess" runat="server" 
        ClientInstanceName="callbackProcess">
                <ClientSideEvents CallbackComplete="callbackCtrlComplete" BeginCallback="function(s, e) {
	savingpanel.Show();	
}" EndCallback="function(s, e) {
	savingpanel.Hide();
}" />
    </dx:ASPxCallback>

            <dx:ASPxCallback ID="callbackInviaEmailProduzione" runat="server" 
        ClientInstanceName="callbackInviaEmailProduzione">
                <ClientSideEvents CallbackComplete="callbackCtrlComplete" BeginCallback="function(s, e) {
	sendingEmail.Show();	
}" EndCallback="function(s, e) {
	sendingEmail.Hide();
}" />
    </dx:ASPxCallback>

            <dx:ASPxCallback ID="ASPxCallback_saveCarrello" runat="server" 
        ClientInstanceName="callbackSaveCarrello">
                <ClientSideEvents BeginCallback="function(s, e) {
		savingCarrelloPanel.Show();
}" EndCallback="function(s, e) {
	savingCarrelloPanel.Hide();
	popupSalvaCarrello.Show();
}" CallbackComplete="saveCallbackComplete" />
    </dx:ASPxCallback>

            <dx:ASPxCallback ID="ASPxCallback_saveOrderHeaderFooter" runat="server" ClientInstanceName="callbackSaveOrderHeaderFooter">
                <ClientSideEvents EndCallback="function(s, e) {
	callbackfooter.PerformCallback();
}" />
    </dx:ASPxCallback>

            <dx:ASPxCallback ID="callbackScontoHeader" runat="server"
        ClientInstanceName="callbackScontoHeader">
        <ClientSideEvents BeginCallback="function(s, e) { savingCarrelloPanel.Show(); }"
                          EndCallback="function(s, e) {  gridOrderLine.Refresh(); savingCarrelloPanel.Hide(); }" />
    </dx:ASPxCallback>
    
      <dx:ASPxCallback ID="callbackVoucher" runat="server"
        ClientInstanceName="callbackVoucher">
        <ClientSideEvents BeginCallback="function(s, e) { savingCarrelloPanel.Show(); }"
                          EndCallback="function(s, e) {  callbackfooter.PerformCallback(); savingCarrelloPanel.Hide(); }" />
    </dx:ASPxCallback>

                <dx:ASPxCallback ID="callbackScontoPagamento" runat="server"
        ClientInstanceName="callbackScontoPagamento">
        <ClientSideEvents BeginCallback="function(s, e) { savingCarrelloPanel.Show(); }"
                          EndCallback="function(s, e) {  gridOrderLine.Refresh(); savingCarrelloPanel.Hide(); }" />
    </dx:ASPxCallback>

            <dx:ASPxCallback ID="callbackSalvaNote" runat="server"
        ClientInstanceName="callbackSalvaNote">
        <ClientSideEvents BeginCallback="function(s, e) { savingNotePanel.Show(); }"
                          EndCallback="function(s, e) { savingNotePanel.Hide(); }" />
    </dx:ASPxCallback>


            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
        ClientInstanceName="loadingpanel" Modal="True" Theme="MetropolisBlue" 
        Text="Caricamento dati&amp;hellip;">
    </dx:ASPxLoadingPanel>

            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel2" runat="server" 
        ClientInstanceName="savingpanel" Text="Salvataggio ordine&amp;hellip;" 
        Theme="MetropolisBlue">
    </dx:ASPxLoadingPanel>

            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel3" runat="server" 
        ClientInstanceName="savingCarrelloPanel" Text="Salvataggio carrello&amp;hellip;" 
        Theme="MetropolisBlue">
    </dx:ASPxLoadingPanel>

            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel4" runat="server" 
        ClientInstanceName="sendingEmail" Text="Invio email in corso&amp;hellip;" 
        Theme="MetropolisBlue">
    </dx:ASPxLoadingPanel>

            <dx:ASPxLoadingPanel ID="ASPxLoadingPanel5" runat="server" 
        ClientInstanceName="savingNotePanel" Text="Salvataggio note&amp;hellip;" 
        Theme="MetropolisBlue">
    </dx:ASPxLoadingPanel>

    <dx:ASPxPopupControl ID="popupReport" runat="server" 
        ClientInstanceName="popupReport" ContentUrl="~/orderReportTR.aspx" 
        HeaderText="Report ordine" PopupHorizontalAlign="WindowCenter" 
        PopupVerticalAlign="TopSides" 
        Theme="MetropolisBlue" AllowDragging="True" AllowResize="True" 
        AutoUpdatePosition="True" Width="800px" Height="720px" Modal="True">
        <ClientSideEvents Shown="function(s, e) {
	popupReport.RefreshContentUrl();
}
" />
        <ContentCollection>
            <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>


    <dx:ASPxPopupControl ID="popupProduzione" runat="server" ClientInstanceName="popupProduzione"
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
                                    <dx:ASPxButton ID="btnInvioRichiestaProduzione" runat="server" Font-Size="11pt"
                                        Text="Invia richiesta" Theme="MetropolisBlue" Width="170px" AutoPostBack="False" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {
	popupProduzione.Hide();
	callbackInviaEmailProduzione.PerformCallback();
}" />
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                    <SettingsItems Width="100%" />
                </dx:ASPxFormLayout>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    
            </asp:Content>
