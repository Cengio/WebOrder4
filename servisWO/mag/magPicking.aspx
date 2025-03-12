<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/mag/magazzino.Master" CodeBehind="magPicking.aspx.vb" Inherits="servisWO.magPicking" %>

<%@ Register assembly="DevExpress.Web.v24.2, Version=24.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function preventEnterKey(htmlEvent) {
            if (htmlEvent.keyCode == 13) {
                ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            }
        }

    function DoProcessEnterKey(htmlEvent, editName) {
        if (htmlEvent.keyCode == 13) {
            ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            if (editName) {
                if (ASPxClientControl.GetControlCollection().GetByName(editName).GetIsValid()) {
                    ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
                }
             } else {
                btnNext.DoClick();
            }
        }
    }

    function DoProcessRettificaEnterKey(htmlEvent, editName) {
        if (htmlEvent.keyCode == 13) {
            ASPxClientUtils.PreventEventAndBubble(htmlEvent);
            if (editName) {
                if (ASPxClientControl.GetControlCollection().GetByName(editName).GetIsValid()) {
                    ASPxClientControl.GetControlCollection().GetByName(editName).SetFocus();
                }
            } else {
                btnconfermarettifica.DoClick();
            }
        }
    }


    function OnQtaValidation(s, e) {
        var qta = e.value;
        var qtaattesa = parseInt(lbqtaattesa.GetText()) - parseInt(hflostquantity.Get("lostquantity"));
        var digits = "0123456789";
        if (qta == null || qta == "") {
            e.isValid = false;
            e.errorText = "valore richiesto.";
            return;
        }
        for (var i = 0; i < qta.length; i++) {
            if (digits.indexOf(qta.charAt(i)) == -1) {
                e.isValid = false;
                e.errorText = "Valore non ammesso.";
                return;
            }
        }
        if (qta > qtaattesa) {
            e.isValid = false;
            e.errorText = "Quantità maggiore di quella attesa.";
            return;
        }
        if (qta < qtaattesa) {
            e.isValid = false;
            e.errorText = "Quantità inferiore di quella attesa.";
            //if (qtaattesa - qta <= 5) {
            showPopUpRettifica();
            //}
            //else
            //{
            //   popupControllaNavision.Show();
            //}
         
        }
    }

    function OnLottoValidation(s, e) {
        if (e.value == null || e.value == "") {
            e.isValid = false;
            e.errorText = "valore richiesto.";
            return;
        }
        var lottiArr = e.value.split(";");
        var lotto = ""
        if (lottiArr.length > 1) {
            lotto = lottiArr[1];
            e.value = lotto;
        } else {
            lotto = e.value;
        }       
        if (!hflottiammessi.Contains(lotto)) {
            e.isValid = false;
            e.errorText = "Lotto non valido.";
            return;
        }
    }

    function OnCollocazioneValidation(s, e) {
        var collocazione = e.value;
        if (collocazione == null || collocazione == "") {
            e.isValid = false;
            e.errorText = "valore richiesto.";
            return;
        }
        if (!hfcollocazioniammesse.Contains(collocazione)) {
            e.isValid = false;
            e.errorText = "Collocazione non valida.";
            return;
        }
    }

    function showPopUpRettifica() {
            popuprettifica.PerformCallback();
            popuprettifica.Show();
    }

    //function OnRettificaValidation(s, e) {
    //    var BinCodeRettifica = e.value;
    //    var BinCodeAtteso = BinCodePopUp.GetText();
    //    if (BinCodeAtteso == null || BinCodeAtteso == "") {
    //        e.isValid = false;
    //        e.errorText = "valore richiesto.";
    //        return;
    //    }
    //    if (BinCodeRettifica != BinCodeAtteso) {
    //        e.isValid = false;
    //        e.errorText = "Collocazione non corretta";
    //        return;
    //    }
    //}

    var selectedLottiRow = [];
    function cblottoSuccessivoSelectionChange(s,e) {
        selectedLottiRow = [];
        callBackGridlotti.PerformCallback();
    }


    function OnRettificaConfermaValidation(s, e) {
        var BinCodeRettifica = BinCodeRettificaPopUp.GetText();
        var BinCodeAtteso = BinCodePopUp.GetText();
        if (BinCodeRettifica == null || BinCodeRettifica == "") {
            //BinCodeRettificaPopUp.isValid = false;
            //BinCodeRettificaPopUp.errorText = "valore richiesto.";
            //BinCodeRettificaPopUp.SetErrorText("valore richiesto.");
            //BinCodeRettificaPopUp.SetIsValid(false);
            popupControlloLotti.SetContentHTML("Conferma collocazione: valore richiesto.");
            popupControlloLotti.Show();
            e.processOnServer = false;
            return;
        }
        if (BinCodeRettifica != BinCodeAtteso) {
            //BinCodeRettificaPopUp.isValid = false;
            //BinCodeRettificaPopUp.errorText = "Collocazione non corretta";
            popupControlloLotti.SetContentHTML("Conferma collocazione: valore non corretto.");
            popupControlloLotti.Show();
            e.processOnServer = false;
            return;
        }

        var lottoSuccessivoSelected = ASPxClientControl.GetControlCollection().GetByName("cbLottoSuccessivo");
        console.log("lottoSuccessivoSelected: " + lottoSuccessivoSelected.GetChecked());
        console.log("Selected lotti rows: " + gridlotti.GetSelectedRowCount());
        if (lottoSuccessivoSelected.GetChecked()) {
            if (gridlotti.GetSelectedRowCount() <= 0) {
                popupControlloLotti.SetContentHTML("Selezionare un lotto alternativo.");
                popupControlloLotti.Show();
                e.processOnServer = false;
                return;
            }

            for (i = 0; i < selectedLottiRow.length; i++) {
                var confermalottosuccessivo = eval('confermalottosuccessivo_' + selectedLottiRow[i].toString()).GetText();
                var confermacollocazionesuccessiva = eval('confermacollocazionesuccessiva_' + selectedLottiRow[i].toString()).GetText();
                var qtaLottoCtrl = eval('qtaLottoCtrl_' + selectedLottiRow[i].toString()).GetValue();;
                //if (!isNaN(confermalottosuccessivo)) {
                if (confermalottosuccessivo == "" || confermalottosuccessivo == null) {
                    popupControlloLotti.SetContentHTML("Confermare il lotto successivo.");
                    popupControlloLotti.Show();
                    e.processOnServer = false;
                    return;
                }
                if (confermacollocazionesuccessiva == "" || confermacollocazionesuccessiva == null) {
                    popupControlloLotti.SetContentHTML("Confermare la collocazione del lotto successivo.");
                    popupControlloLotti.Show();
                    e.processOnServer = false;
                    return;
                }
                if (qtaLottoCtrl <= 0 || qtaLottoCtrl == null) {
                    popupControlloLotti.SetContentHTML("Inserire la quantità del lotto successivo.");
                    popupControlloLotti.Show();
                    e.processOnServer = false;
                    return;
                }
                //}
            }

        }
        //gridlotti.GetRowValues(gridlotti.GetFocusedRowIndex(), 'EmployeeID', OnGetRowValues);
        //gridlotti.GetSelectedFieldValues('ProductName;UnitPrice;Discontinued', OnGetSelectedFieldValues);
    }

    //function OnGetSelectedFieldValues(selectedValues) {
    //    //listBox.ClearItems();
    //    if (selectedValues.length == 0) return;
    //    for (i = 0; i < selectedValues.length; i++) {
    //        s = "";
    //        for (j = 0; j < selectedValues[i].length; j++) {
    //            s = s + selectedValues[i][j] + "&nbsp;";
    //        }
    //        console.log(s);
    //    }
    //}



    
    function OnSelectionChanged(s, e) {
        console.log("Row: " + e.visibleIndex.toString() + " " + e.isSelected);
        if (e.isSelected) {
            selectedLottiRow.push(e.visibleIndex);
        } else {
            var i = selectedLottiRow.indexOf(e.visibleIndex);
            if (i != -1) {
                selectedLottiRow.splice(i, 1);
            }
        }
        console.log(selectedLottiRow);
        if (e.visibleIndex >= 0) {
            var confermalottosuccessivo = eval('confermalottosuccessivo_' + e.visibleIndex.toString());
            var confermacollocazionesuccessiva = eval('confermacollocazionesuccessiva_' + e.visibleIndex.toString())
            var qtaLottoCtrl = eval('qtaLottoCtrl_' + e.visibleIndex.toString());
            confermalottosuccessivo.SetEnabled(e.isSelected);
            confermacollocazionesuccessiva.SetEnabled(e.isSelected);
            qtaLottoCtrl.SetEnabled(e.isSelected);
        }
    }

    function confermalottosuccessivoValueChanged(s, e) {
        var value = s.GetValue();
        console.log(value);
        if (value != null) {
            var arrvalue = value.split(";");
            console.log(arrvalue);
            if (arrvalue.length > 1) {
                s.SetValue(arrvalue[1]);
            } else if (arrvalue.length == 1) {
                s.SetValue(arrvalue[0]);
            } else {
                s.SetValue("");
            }
        }
    }

    //function OnGriLottiEndCallback(s, e) {
    //    console.log(s);
    //    console.log(e);
    //}

    //function onValidateQtaLottoCtrl(s, e) {
    //    var QtyToAdd = s.GetValue();
    //    console.log("QtaLottoAlternativo: " + QtyToAdd);
    //    if (!isNaN(QtyToAdd)) {
    //        if (QtyToAdd <= 0) {
    //            e.isValid = false;
    //        }
    //    }
    //}

    //function onValidateLottoCtrl(s, e) {
    //    var value = s.GetText();
    //    console.log("onValidateLottoCtrl: " + value);
    //    if (!isNaN(value)) {
    //        if (value != "") {
    //            e.isValid = false;
    //        }
    //    }
    //}

    //function onValidateCollocazioneCtrl(s, e) {
    //    var value = s.GetText();
    //    console.log("onValidateCollocazioneCtrl: " + value);
    //    if (!isNaN(value)) {
    //        if (value != "") {
    //            e.isValid = false;
    //        }
    //    }
    //}


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" ColCount="4" 
        EnableTheming="True" Theme="Office2010Black" Width="100%">
        <Items>
            <dx:LayoutGroup ColCount="3" ColSpan="4" ShowCaption="False" Width="100%">
                <Items>
                    <dx:LayoutItem Caption="Nr. Riga" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_LineNo" runat="server" Font-Bold="True" Font-Size="Medium">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Codice Articolo" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_ItemCode" runat="server" Font-Bold="True" 
                                    Font-Size="Medium">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Caption="Descrizione" Width="60%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxLabel ID="lb_Descrizione" runat="server" Font-Bold="True" 
                                    Font-Size="Medium">
                                </dx:ASPxLabel>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutItem RowSpan="5" ShowCaption="False" VerticalAlign="Top" Width="20%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxImage ID="img_Articolo" runat="server" ShowLoadingImage="True" 
                            Width="180px">
                        </dx:ASPxImage>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Quantità attesa" Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxLabel ID="lb_QtaAttesa" runat="server" ClientInstanceName="lbqtaattesa" 
                            Font-Bold="True" Font-Size="Medium" Width="100%">
                        </dx:ASPxLabel>
                        <br />
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Quantità" HorizontalAlign="Left" ColSpan="2" 
                Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxTextBox ID="tb_Qta" runat="server" ClientInstanceName="tbqta" 
                            Font-Size="Medium" Width="100%">
                            <ClientSideEvents KeyDown="function(s, e) {DoProcessEnterKey(e.htmlEvent, 'tblotto');}" 
                                Validation="OnQtaValidation" />
                            <ValidationSettings EnableCustomValidation="True" ErrorTextPosition="Bottom" 
                                SetFocusOnError="True">
                                <RequiredField ErrorText="valore necessario" IsRequired="True" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Lotto atteso" Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxLabel ID="lb_LottoAtteso" runat="server" 
                            ClientInstanceName="lblottoatteso" Font-Bold="True" Font-Size="Medium" 
                            Width="100%">
                        </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Lotto" ColSpan="2" HorizontalAlign="Left" Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxTextBox ID="tb_Lotto" runat="server" ClientInstanceName="tblotto" 
                            Font-Size="Medium" Width="100%">
                            <ClientSideEvents KeyDown="function(s, e) {DoProcessEnterKey(e.htmlEvent, 'tbcollocazione');}" 
                                Validation="OnLottoValidation" />
                            <ValidationSettings ErrorTextPosition="Bottom" SetFocusOnError="True">
                                <RequiredField ErrorText="valore necessario" IsRequired="True" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Collocazione attesa" Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxLabel ID="lb_CollAttesa" runat="server" 
                            ClientInstanceName="lbcollocazioneattesa" Font-Bold="True" Font-Size="Medium" 
                            Width="100%">
                        </dx:ASPxLabel>
                        <dx:ASPxLabel ID="lb_CollAttesaDescription" runat="server">
                        </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Collocazione" HorizontalAlign="Left" Width="20%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxTextBox ID="tb_Collocazione" runat="server" 
                            ClientInstanceName="tbcollocazione" Font-Size="Medium" Width="100%">
                            <ClientSideEvents KeyDown="function(s, e) {DoProcessEnterKey(e.htmlEvent, '');}" 
                                Validation="OnCollocazioneValidation" />
                            <ValidationSettings ErrorTextPosition="Bottom" SetFocusOnError="True">
                                <RequiredField ErrorText="valore necessario" IsRequired="True" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                        <dx:ASPxLabel ID="lb_CollocazioneDescription" runat="server">
                        </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Rettifica" ShowCaption="False" VerticalAlign="Middle" 
                Width="20%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxButton ID="btn_Rettifica" runat="server" AutoPostBack="False" 
                            CausesValidation="False" ClientInstanceName="btnrettifica" Enabled="False" 
                            Text="Rettifica" Theme="Office2010Black" Width="100%">
                            <ClientSideEvents Click="showPopUpRettifica" />
                        </dx:ASPxButton>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:EmptyLayoutItem ColSpan="3" Width="80%">
            </dx:EmptyLayoutItem>
            <dx:LayoutItem Caption="Nota lotti successivi" ColSpan="3" ShowCaption="False" 
                Width="80%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxLabel ID="lb_notaLottiSuccessivi" runat="server" BackColor="Orange" 
                            Font-Size="Small">
                        </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutGroup ColCount="5" ColSpan="4" ShowCaption="False" Width="100%">
                <Items>
                    <dx:LayoutItem Width="20%" HorizontalAlign="Left">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btn_RipristinaRiga" runat="server" Text="Ripristina Riga" 
                                    Theme="Office2010Black" CausesValidation="False" 
                                    Enabled="False" Font-Size="Medium">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Left" Width="20%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btn_FinePicking" runat="server" 
                                    Text="Completa Ordine" Theme="Office2010Black" 
                                    CausesValidation="False" Font-Size="Medium">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Width="20%" HorizontalAlign="Right">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btn_Prev" runat="server" CausesValidation="False" 
                                    Font-Size="Medium" Text="Precedente" Theme="Office2010Black">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem Width="20%" HorizontalAlign="Right">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btn_Skip" runat="server" CausesValidation="False" 
                                    Font-Size="Medium" Text="Salta Riga" Theme="Office2010Black">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Right" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btn_Next" runat="server" ClientInstanceName="btnNext" 
                                    Font-Size="Medium" Text="Successivo" Theme="Office2010Black">
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
                <SettingsItems ShowCaption="False" />

        <SettingsItems ShowCaption="False"></SettingsItems>
    </dx:LayoutGroup>
        </Items>
        <SettingsItems HorizontalAlign="Left" VerticalAlign="Top" />
        <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />

<SettingsItems VerticalAlign="Top" HorizontalAlign="Left"></SettingsItems>

<SettingsItemCaptions Location="Top" VerticalAlign="Bottom"></SettingsItemCaptions>
    </dx:ASPxFormLayout>
    <dx:ASPxHiddenField ID="hf_LostQuantity" runat="server" 
        ClientInstanceName="hflostquantity">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="hf_lottiammessi" runat="server" 
        ClientInstanceName="hflottiammessi">
    </dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="hf_collocazioniammesse" runat="server" 
        ClientInstanceName="hfcollocazioniammesse">
    </dx:ASPxHiddenField>

        <dx:ASPxPopupControl ID="popupControlloLotti" runat="server"
        ClientInstanceName="popupControlloLotti" EnableTheming="True"
        HeaderText="Controllo rettifica" Modal="True" PopupAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Theme="MetropolisBlue" Width="420px">
        <ContentStyle Font-Size="14pt">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">test</dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" 
        ClientInstanceName="popuprettifica"  
        Theme="Office2010Black" Width="780px" 
        HeaderText="Rettifica quantità linea ordine" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
        CloseAction="CloseButton" AllowDragging="True" AppearAfter="500">
        <ContentCollection>
        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            <dx:ASPxFormLayout ID="ASPxFormLayout_rettifica" runat="server" EnableTheming="True" Theme="Office2010Black" ColCount="6" Width="100%">
                <Items>
            <dx:LayoutItem Caption="Lotto atteso" Width="20%">
                <LayoutItemNestedControlCollection>
<dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
    <dx:ASPxLabel ID="lb_lottoPopUp" runat="server" 
        ClientInstanceName="lblottoPopUp" Font-Size="Medium" Width="100%">
    </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
</LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Quantità attesa" Width="20%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxLabel ID="lb_qtaAttesaPopup" runat="server" Font-Size="Medium" 
                            ClientInstanceName="lbqtaAttesaPopup" Width="100%">
                        </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Quantità non disponibile" Width="20%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxLabel ID="lb_qtaNonDisponibile" runat="server" Font-Size="Medium" 
                            ClientInstanceName="lbqtaNonDisponibile" Width="100%">
                        </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Collocazione attesa" ColSpan="3" Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxLabel ID="lb_BinCodePopUp" runat="server" Font-Size="Medium" 
                            ClientInstanceName="BinCodePopUp" Width="100%">
                        </dx:ASPxLabel>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Ragione mancata disponibilità" ColSpan="2" Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxComboBox ID="combo_Reason" runat="server" Font-Size="Medium" 
                            ClientInstanceName="comboReason" Width="100%" SelectedIndex="4">
                            <Items>
                                <dx:ListEditItem Text="Scatola e/o etichetta rovinata" Value="SCATOROVIN" />
                                <dx:ListEditItem Text="Qtà. incomplete nel cartone" Value="QTAINCOMPL" />
                                <dx:ListEditItem Text="Confezione vuota" Value="CONFVUOTA" />
                                <dx:ListEditItem Text="Rotto" Value="ROTTO" />
                                <dx:ListEditItem Text="Riallineamento iniziale" Value="RIALLINEAM" Selected="True" />
                                <dx:ListEditItem Text="Altro" Value="ALTRO" />
                            </Items>
                            <%--<ValidationSettings ErrorTextPosition="Bottom" ValidationGroup="rettifica">
                                <RequiredField IsRequired="True" />
                            </ValidationSettings>--%>
                        </dx:ASPxComboBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:LayoutItem Caption="Conferma collocazione" ColSpan="2" Width="40%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" SupportsDisabledAttribute="True">
                        <dx:ASPxTextBox ID="tb_BinCodeRettificaPopUp" runat="server" Font-Size="Medium" 
                            Width="100%" ClientInstanceName="BinCodeRettificaPopUp">
                            <ClientSideEvents KeyDown="function(s, e) {DoProcessRettificaEnterKey(e.htmlEvent, '');}" />
                           <%-- <ValidationSettings EnableCustomValidation="True" ErrorTextPosition="Bottom" SetFocusOnError="True" >
                             <RequiredField IsRequired="True" />
                            </ValidationSettings>--%>
                        </dx:ASPxTextBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
            </dx:LayoutItem>
            <dx:EmptyLayoutItem ColSpan="2" Width="20%">
            </dx:EmptyLayoutItem>
            <dx:LayoutItem ColSpan="6" ShowCaption="False" 
                Caption="Abilita lotto successivo" HorizontalAlign="Left" Width="100%">
                <LayoutItemNestedControlCollection>
                    <dx:LayoutItemNestedControlContainer runat="server" 
                        SupportsDisabledAttribute="True">
                        <dx:ASPxCheckBox ID="cb_lottoSuccessivo" runat="server" Checked="False" 
                            ClientInstanceName="cbLottoSuccessivo" 
                            Text="Inserisci quantità mancanti su lotti successivo" Width="100%">
                            <ClientSideEvents CheckedChanged="function(s, e) { cblottoSuccessivoSelectionChange(s,e); }" />
                        </dx:ASPxCheckBox>
                    </dx:LayoutItemNestedControlContainer>
                </LayoutItemNestedControlCollection>
                 </dx:LayoutItem>
            <dx:LayoutGroup Caption="Gestione lotti alternativi" ColSpan="6" Width="100%">
                <Items>
                    <dx:LayoutItem Caption="Elenco lotti" Width="100%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_callBackGridlotti" runat="server" 
                                    ClientInstanceName="callBackGridlotti" Theme="MetropolisBlue" Width="100%">
                                    <PanelCollection>
                                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">

                                            <dx:ASPxGridView ID="gridlotti" runat="server" AutoGenerateColumns="False" 
                                                ClientInstanceName="gridlotti" EnableTheming="True" Theme="Office2010Black" 
                                                Width="100%" KeyFieldName="Lotto">
                                                <ClientSideEvents SelectionChanged="OnSelectionChanged"  />
                                                <Columns>
                                                    <dx:GridViewCommandColumn ButtonType="Button" Caption=" " 
                                                        ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0" 
                                                        Width="40px">
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataTextColumn Caption="CodiceArticolo" FieldName="Item No_" 
                                                        Name="CodiceArticolo" ReadOnly="True" ShowInCustomizationForm="True" 
                                                        Visible="False" VisibleIndex="1">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Lotto" FieldName="Lotto" Name="Lotto" 
                                                        ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="2">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Scadenza Lotto" FieldName="ScadenzaLotto" 
                                                        Name="ScadenzaLotto" ReadOnly="True" ShowInCustomizationForm="True" 
                                                        UnboundType="DateTime" VisibleIndex="3" Width="110px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Disponibilità Lotto" 
                                                        FieldName="DisponibilitaLotto" Name="DisponibilitaLotto" ReadOnly="True" 
                                                        ShowInCustomizationForm="True" VisibleIndex="4" Width="110px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Conferma Lotto" Name="confermaLotto"
                                                        ShowInCustomizationForm="True" UnboundType="String" VisibleIndex="5" 
                                                        Width="110px">
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="tb_confermalottosuccessivo" runat="server" oninit="tb_confermalottosuccessivo_Init" Width="95px">
                                                                <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" ValueChanged="function(s, e) {confermalottosuccessivoValueChanged(s,e);}"  />
                                                              <%--  <ValidationSettings Display="Dynamic" SetFocusOnError="True" CausesValidation="true" EnableCustomValidation="true">
                                                                    <RequiredField IsRequired="True" />
                                                                </ValidationSettings>--%>
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Collocazione" name="confermaCollocazione"
                                                        ShowInCustomizationForm="True" UnboundType="String" VisibleIndex="6" 
                                                        Width="110px">
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="tb_confermacollocazionesuccessiva" runat="server" oninit="tb_confermacollocazionesuccessiva_Init" Width="95px">
                                                                <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}"  />
                                                               <%--  <ValidationSettings Display="Dynamic" SetFocusOnError="True" CausesValidation="true" EnableCustomValidation="true">
                                                                    <RequiredField IsRequired="True" />
                                                                </ValidationSettings>--%>
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Quantità" ShowInCustomizationForm="True" 
                                                        UnboundType="Integer" VisibleIndex="7" Width="55px" Name="confermaQuantita">
                                                        <DataItemTemplate>
                                                            <dx:ASPxTextBox ID="tbqtalotto" runat="server" oninit="tbqtalotto_Init" Width="40px">
                                                                <ClientSideEvents KeyDown="function(s, e) {preventEnterKey(e.htmlEvent);}" />
                                                                <MaskSettings ErrorText="*" Mask="&lt;0..100&gt;" />
                                                              <%--   <ValidationSettings Display="Dynamic" SetFocusOnError="True" CausesValidation="true" EnableCustomValidation="true" ErrorText="*">
                                                                    <RequiredField IsRequired="True" />
                                                                </ValidationSettings>--%>
                                                            </dx:ASPxTextBox>
                                                        </DataItemTemplate>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior 
                                                    AllowSort="False" />
                                                <SettingsPager Mode="ShowAllRecords">
                                                </SettingsPager>
                                            </dx:ASPxGridView>

                                            <dx:ASPxGridView ID="gridLottiSuccessivi" runat="server" 
                                                AutoGenerateColumns="False" Theme="Office2010Black" Width="100%" 
                                                KeyFieldName="ordineLine.LotNo">
                                                <Columns>
                                                    <dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
                                                        ShowSelectCheckbox="True" VisibleIndex="0" Width="40px">
                                                    </dx:GridViewCommandColumn>
                                                    <dx:GridViewDataTextColumn Caption="Lotto" FieldName="ordineLine.LotNo" Name="Lotto" 
                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Collocazione" FieldName="ordineLine.BinCode" 
                                                        Name="Collocazione" ShowInCustomizationForm="True" VisibleIndex="2">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="Quantità" FieldName="ordineLine.Quantity" 
                                                        Name="Quantita" ShowInCustomizationForm="True" 
                                                        VisibleIndex="3" Width="80px">
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn Caption="ID linea" FieldName="ordineLine.LineID" Name="LineID" 
                                                        ShowInCustomizationForm="True" VisibleIndex="4" Width="80px">
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsBehavior AllowSort="False" />
                                                <SettingsPager Mode="ShowAllRecords">
                                                </SettingsPager>
                                            </dx:ASPxGridView>

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>

                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
            </dx:LayoutGroup>
            <dx:LayoutGroup ColCount="4" ColSpan="6" ShowCaption="False" Width="100%">
                <Items>
                    <dx:LayoutItem ShowCaption="False" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem ShowCaption="False" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Right" ShowCaption="False" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btn_ConfermaRettifica" runat="server" 
                                    ClientInstanceName="btnconfermarettifica" Text="Conferma" CausesValidation="false"
                                    Theme="Office2010Black" ValidationGroup="rettifica" Width="100%">
                                    <ClientSideEvents Click="function(s, e) {OnRettificaConfermaValidation(s,e);}" />
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                    <dx:LayoutItem HorizontalAlign="Right" ShowCaption="False" Width="25%">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server" 
                                SupportsDisabledAttribute="True">
                                <dx:ASPxButton ID="btn_CloseRettifica" runat="server" AutoPostBack="False" 
                                    CausesValidation="False" Text="Annulla" Theme="Office2010Black" Width="100%">
                                    <ClientSideEvents Click="function(s, e) {popuprettifica.Hide();}" />
                                </dx:ASPxButton>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>
                </Items>
                <SettingsItemCaptions HorizontalAlign="Left" />
                <SettingsItems HorizontalAlign="Right" />
            </dx:LayoutGroup>
        </Items>
                <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" HorizontalAlign="Left" />
                <SettingsItems HorizontalAlign="Left" />
            </dx:ASPxFormLayout>
        </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
  
    <dx:ASPxPopupControl ID="popupControllaNavision" runat="server" 
        AllowDragging="True" ClientInstanceName="popupControllaNavision" 
        HeaderText="Attenzione" Height="100px" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
         Theme="Office2010Black" Width="360px">
        <ContentStyle Font-Size="Medium">
        </ContentStyle>
        <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">La 
    quantità mancante supera i 5 pezzi. Ricontrollare la giacenza su Navision.</dx:PopupControlContentControl>
</ContentCollection>
    </dx:ASPxPopupControl>
  
    <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" 
        ClientInstanceName="loading1" Text="Elaborazione in corso&amp;hellip;" 
        Theme="MetropolisBlue">
    </dx:ASPxLoadingPanel>
    <dx:ASPxCallback ID="ASPxCallback_elaboraRettifica" runat="server" ClientInstanceName="CallbackElaboraRettifica">
        <ClientSideEvents CallbackComplete="function(s, e) {
	loading1.Hide();
}" />
    </dx:ASPxCallback>
</asp:Content>
