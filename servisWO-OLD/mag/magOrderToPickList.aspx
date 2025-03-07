<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/mag/magazzino.Master" CodeBehind="magOrderToPickList.aspx.vb" Inherits="servisWO.magOrderToPickList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    function grid_EndCallback(s, e) {
        if (s.cpShowConfirmBox) {
            callbackMagazzinieri.PerformCallback();
            pcConfirm.Show();
        }
        if (s.cpreporturl != '') {
            //alert(cpreporturl);
            window.open(s.cpreporturl);
        }
    }

    function Yes_Click() {
        pcConfirm.Hide();
        magazziniere = comboOperatore.GetSelectedItem().value
        changeorderuser.PerformCallback(grid.cpOrderCode + '|' + grid.cpCustomerCode + '|' + magazziniere);
    }

    function No_Click() {
        pcConfirm.Hide()
    }
    //aggiorna collocazioni da BC
    window.onload = function() {
        var currentUrl = window.location.href;
        if (currentUrl.includes("magOrderToPickList.aspx?t=2")) {
            document.getElementById('btnUpdateContainer').style.display = 'block';
        }
    };
    function showModal(message) {
        var modal = document.getElementById("updCollModal");
        var span = document.getElementsByClassName("updCollClose")[0];
        var modalText = document.getElementById("updCollModalText");
        var confermaButtonContainer = document.getElementById("btnConfermaContainer");
        
        modalText.innerHTML = message;
        modal.style.display = "block";
        
        span.onclick = function() {
          modal.style.display = "none";
        }
        
        window.onclick = function(event) {
          if (event.target == modal) {
            modal.style.display = "none";
          }
        }
        if (modalText.textContent === 'Nessuna collocazione da modificare'){
            confermaButtonContainer.style.display = "none"
        }
      }
      function hideModal(){
        var modal = document.getElementById("updCollModal");
        modal.style.display = "none"
      }
    </script>
    <style>
        /*CSS per "aggiorna collocazioni da BC"*/
        .updCollModal {
            display: none;
            position: fixed; 
            z-index: 1; 
            left: 0;
            top: 0;
            width: 100%; 
            height: 100%; 
            overflow: auto; 
            background-color: rgb(0,0,0); 
            background-color: rgba(0,0,0,0.4);
            font-family: Verdana, Geneva, sans-serif
          }
          #btnUpdateContainer{
            text-align: center;
            margin-top: 50px;
            display:none;
          }
          .updCollModal-content {
            background-color: #fefefe;
            margin: 15% auto; 
            padding: 20px;
            border: 1px solid #888;
            width: 300px; 
            position:relative;
          }
          
          .updCollClose {
            color: #aaa;
            position: absolute;
            font-size: 28px;
            font-weight: bold;
            top:0;
            right:10px
          }
          
          .updCollClose:hover,
          .updCollClose:focus {
            color: black;
            text-decoration: none;
            cursor: pointer;
          }
          td{
            border-top-color: transparent!important
          }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxGridView ID="grid" runat="server" Width="100%" 
        AutoGenerateColumns="False" 
        ClientInstanceName="grid" EnableTheming="True" KeyFieldName="Code" 
        Theme="Office2010Black">
        <ClientSideEvents EndCallback="grid_EndCallback" />
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
<%--            <dx:GridViewDataTextColumn FieldName="CompanyName_" VisibleIndex="2" 
                ReadOnly="True" Visible="False">
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn VisibleIndex="4" Caption="Ragione sociale" ReadOnly="True" Name="ragsociale">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CustomerNo" VisibleIndex="3" 
                Caption="Cod. Cliente" ReadOnly="True" 
                Width="90px">
            </dx:GridViewDataTextColumn>
<%--            <dx:GridViewDataTextColumn FieldName="Type" VisibleIndex="5" 
                Width="120px" Visible="False">
            </dx:GridViewDataTextColumn>--%>
<%--            <dx:GridViewDataTextColumn FieldName="ShippingAgentCode" VisibleIndex="6" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
<%--            <dx:GridViewDataTextColumn FieldName="ShipAddressCode" VisibleIndex="7" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="23" Caption="Stato" 
                ReadOnly="True" Width="120px">
                <Settings FilterMode="DisplayText" />
            </dx:GridViewDataTextColumn>
<%--            <dx:GridViewDataTextColumn FieldName="Notes" 
                Visible="False" VisibleIndex="22">
            </dx:GridViewDataTextColumn>--%>
<%--            <dx:GridViewDataTextColumn FieldName="AttachmentPath" VisibleIndex="9" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="User" VisibleIndex="12" 
                Caption="In lavorazione a" Width="120px">
            </dx:GridViewDataTextColumn>
<%--            <dx:GridViewDataTextColumn FieldName="PackageNum" VisibleIndex="8" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
<%--            <dx:GridViewDataTextColumn FieldName="OverpackageNum" VisibleIndex="10" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="OperatorCode" VisibleIndex="11" Width="120px" 
                Caption="Creato da">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedImported" VisibleIndex="19" 
                Visible="False">
            </dx:GridViewDataTextColumn>
<%--            <dx:GridViewDataTextColumn FieldName="IncludeShipCost" VisibleIndex="13" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
<%--            <dx:GridViewDataTextColumn FieldName="PaymentMethodCode" VisibleIndex="14" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
<%--            <dx:GridViewDataTextColumn FieldName="PaymentTermsCode" VisibleIndex="15" 
                Visible="False">
            </dx:GridViewDataTextColumn>--%>
<%--            <dx:GridViewDataTextColumn FieldName="Weight" VisibleIndex="16" Visible="False">
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn FieldName="OrderNoCtrl" VisibleIndex="17" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UserCtrl" VisibleIndex="18" 
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="25" 
                Width="40px">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnElaboraOrdine" 
                        Visibility="AllDataRows">
                        <Image ToolTip="Elabora spedizione" Url="~/images/magelabora.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="24" Width="40px" Caption=" " ShowClearFilterButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnReport" Visibility="AllDataRows">
                        <Image AlternateText="Report Ordine" ToolTip="Report Ordine" Url="~/images/pdfdocument.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Righe" FieldName="NumRighe" ReadOnly="True" 
                VisibleIndex="20" Width="50px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Pezzi" FieldName="PezziInOrdine" 
                ReadOnly="True" VisibleIndex="21" Width="50px">
            </dx:GridViewDataTextColumn>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " Name="cambiaStato" VisibleIndex="26" Width="40px">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="btnStatoLavorazione" Visibility="AllDataRows">
                        <Image AlternateText="PRENOTA LAVORAZIONE" IconID="setup_properties_32x32" ToolTip="PRENOTA LAVORAZIONE">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
            </dx:GridViewCommandColumn>
        </Columns>
        <SettingsBehavior AllowSelectByRowClick="True" 
            AllowSelectSingleRowOnly="True" />
        <SettingsPager PageSize="20" Position="TopAndBottom" AlwaysShowPager="True">
            <Summary AllPagesText="Pagine: {0} - {1} ({2} ordini)" Text="Pagina {0} di {1} ({2} ordini)" />
        </SettingsPager>
        <Settings 
            ShowVerticalScrollBar="True" VerticalScrollableHeight="300" 
            ShowFilterBar="Auto" />
        <SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />
        <Templates>
            <DetailRow>
                <dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False" KeyFieldName="OrderCode" 
                    oncustomcolumndisplaytext="ASPxGridView2_CustomColumnDisplayText" 
                    Width="100%" 
                    EnableTheming="True" Theme="Office2010Black" OnInit="ASPxGridView2_Init">
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="OrderCode" ReadOnly="True" 
                            VisibleIndex="0" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ItemCode" ReadOnly="True" 
                            VisibleIndex="2" Caption="Codice" Width="100px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RowDiscount" ReadOnly="True" 
                            VisibleIndex="4" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LineID" ReadOnly="True" VisibleIndex="1" Caption="Riga Nr."
                            Width="80px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="OldItemCode" VisibleIndex="5" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="CompanyName_" VisibleIndex="6" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="UnitPrice" VisibleIndex="15" 
                            Caption="Prezzo" Width="80px" Visible="False">
                            <PropertiesTextEdit DisplayFormatString="C2">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="OriginalQty" VisibleIndex="13" 
                            Caption="Q.ta Ordine" Width="80px">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DiscountQty" VisibleIndex="7" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Imported" VisibleIndex="8" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="BinCode" ReadOnly="True" 
                            VisibleIndex="12" Width="180px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="QtyToShip" VisibleIndex="14" 
                            Visible="true" Caption="Q.ta Picked" Width="80px">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LotNo" ReadOnly="True" VisibleIndex="9" 
                            Width="120px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="FormulaSconto" VisibleIndex="10" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LineNo" VisibleIndex="11" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Totale Riga" Name="TotaleRiga" 
                            VisibleIndex="16" Width="100px" FieldName="totaleriga" 
                            UnboundType="Decimal" Visible="False">
                            <PropertiesTextEdit DisplayFormatString="c">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Descrizione" Name="Descrizione" 
                            VisibleIndex="3" FieldName="DESCRIZIONE">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager Visible="False" Mode="ShowAllRecords">
                    </SettingsPager>
                    <Settings ShowFooter="True" />
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
    </dx:ASPxGridView>

    <dx:ASPxCallback ID="CallbackChangeOrderUser" runat="server" 
    ClientInstanceName="changeorderuser">
</dx:ASPxCallback>

    <dx:ASPxPopupControl ID="pcConfirm" runat="server" ClientInstanceName="pcConfirm"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            HeaderText="Richiesta conferma cambio magazziniere" Theme="Office2010Black" 
    Width="360px">
            <ContentStyle Font-Size="Medium">
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                    <table width="100%">
                        <tr>
                            <td colspan="2" align="center">
								<br />
                                L&#39;ordine è già in lavorazione.<br /> Confermi di volerlo assegnare al 
                                seguente operatore?<br /> <br />
                                <dx:ASPxCallbackPanel ID="callbackMagazzinieri" runat="server" 
                                    ClientInstanceName="callbackMagazzinieri" Width="200px">
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">
                                            <dx:ASPxComboBox ID="comboOperatore" runat="server" 
                                                ClientInstanceName="comboOperatore">
                                            </dx:ASPxComboBox>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                                <br />
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                &nbsp;<dx:ASPxButton ID="btn_Yes" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnyes" EnableTheming="True" Text="Si" 
                                    Theme="Office2010Black" Width="80px">
                                    <ClientSideEvents Click="function(s, e) {
Yes_Click();	
}" />
                                </dx:ASPxButton>
                            </td>
                            <td align="center">
                                &nbsp;<dx:ASPxButton ID="btn_No" runat="server" AutoPostBack="False" 
                                    ClientInstanceName="btnno" EnableTheming="True" Text="No" 
                                    Theme="Office2010Black" Width="80px">
                                    <ClientSideEvents Click="function(s, e) {
No_Click();	
}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <div id="btnUpdateContainer">
            <asp:Button ID="btnUpdateCollocazionePreview" runat="server" Height="24px" Text="Allinea Collocazioni da BC" Width="240px" OnClick="woUpdateCollocazioneDaBCPreview_Click" />
        </div>
        <div id="updCollModal" class="updCollModal">

            <div class="updCollModal-content">
              <span class="updCollClose">&times;</span>
              <p id="updCollModalText">Some text in the Modal..</p>
              <div id="btnConfermaContainer"><asp:Button ID="btnUpdateCollocazione" runat="server" Height="24px" Text="Conferma" Width="120px" OnClick="woUpdateCollocazioneDaBC_Click" /></div>
            </div>
            
          
          </div>
    </asp:Content>

