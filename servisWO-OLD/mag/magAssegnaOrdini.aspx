<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/mag/magazzino.Master" CodeBehind="magAssegnaOrdini.aspx.vb" Inherits="servisWO.magAssegnaOrdini" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript">
     var checkBoxColumns = ['assegnato'];
     var lastEditedColumn;
     var rowIndex;
     function OnBatchEditStartEditing(s, e) {
         lastEditedColumn = e.focusedColumn.fieldName;
         rowIndex = e.visibleIndex;
     }

     function OnPrioritaRaccoltaCheckedChanged(s, e) {
         //alert(s.GetChecked());
         if (s.GetValue() == -1) s.SetValue(1);
         for (var i = 0; i < checkBoxColumns.length; i++) {
             if (s.GetChecked()) grid.batchEditApi.SetCellValue(rowIndex, checkBoxColumns[i], Number(s.GetChecked()));
         }
     }

     function OnCheckedChanged(s, e) {
//         var IsAllCheckedEditors = s.GetChecked();
//         var IsOneEditorChecked = s.GetChecked();
//         for (var i = 0; i < checkBoxColumns.length; i++) {
//             if (checkBoxColumns[i] != lastEditedColumn) {
//                 IsAllCheckedEditors = IsAllCheckedEditors && grid.batchEditApi.GetCellValue(rowIndex, checkBoxColumns[i]);
//                 IsOneEditorChecked = IsOneEditorChecked || grid.batchEditApi.GetCellValue(rowIndex, checkBoxColumns[i]);
//             }
//         }
//         grid.batchEditApi.SetCellValue(rowIndex, "prioritaRaccolta", Number(IsAllCheckedEditors));
//         if (!IsAllCheckedEditors && IsOneEditorChecked)
//             grid.batchEditApi.SetCellValue(rowIndex, "prioritaRaccolta", -1);
     }
    var intervalId = setInterval(function() {
        var previewButtonSpan = document.querySelector('#ctl00_ContentPlaceHolder1_grid_DXCBtn0_CD span');
        if (previewButtonSpan && previewButtonSpan.textContent === "Preview changes") {
            previewButtonSpan.textContent = "Anteprima modifiche";
        }
        var saveButtonSpan = document.querySelector('#ctl00_ContentPlaceHolder1_grid_DXCBtn2_CD span');
        if (saveButtonSpan && saveButtonSpan.textContent === "Save changes") {
            saveButtonSpan.textContent = "Salva modifiche";
        }
        var cancelButtonSpan = document.querySelector('#ctl00_ContentPlaceHolder1_grid_DXCBtn3_CD span');
        if (cancelButtonSpan && cancelButtonSpan.textContent === "Cancel changes") {
            cancelButtonSpan.textContent = "Annulla modifiche";
        }
        var hidePreviewButtonSpan = document.querySelector('#ctl00_ContentPlaceHolder1_grid_DXCBtn1_CD span');
        if (hidePreviewButtonSpan && hidePreviewButtonSpan.textContent === "Hide preview") {
            hidePreviewButtonSpan.textContent = "Nascondi anteprima";
        }
    }, 100);
    setTimeout(function() {
        clearInterval(intervalId);
    }, 3000);
    </script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
<dx:ASPxFormLayout ID="ASPxFormLayout1" runat="server" EnableTheming="True" 
    Theme="Office2010Black" Width="460px" AlignItemCaptionsInAllGroups="True" 
        ColCount="2">
    <Items>
        <dx:LayoutItem Caption="Magazziniere">
            <LayoutItemNestedControlCollection>
                <dx:LayoutItemNestedControlContainer runat="server">
                    <dx:ASPxComboBox ID="comboMagazzinieri" runat="server" Width="300px" 
                        Theme="Office2010Black">
                    </dx:ASPxComboBox>
                </dx:LayoutItemNestedControlContainer>
            </LayoutItemNestedControlCollection>
        </dx:LayoutItem>
        <dx:LayoutItem Caption="Filtra" ShowCaption="False" VerticalAlign="Bottom">
            <LayoutItemNestedControlCollection>
                <dx:LayoutItemNestedControlContainer runat="server">
                    <dx:ASPxButton ID="btnFiltra" runat="server" Height="24px" Text="Filtra" 
                        Theme="Office2010Black" Width="120px">
                    </dx:ASPxButton>
                </dx:LayoutItemNestedControlContainer>
            </LayoutItemNestedControlCollection>
        </dx:LayoutItem>
    </Items>
    <SettingsItemCaptions Location="Top" VerticalAlign="Bottom" />
</dx:ASPxFormLayout>
<br />
<dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" 
    ClientInstanceName="grid" EnableTheming="True" Theme="Office2010Black" 
    Width="100%" KeyFieldName="Code">
    <ClientSideEvents BatchEditStartEditing="OnBatchEditStartEditing" />
    <Columns>
        <dx:GridViewDataCheckColumn Caption="Assegnato" VisibleIndex="0"
            Width="90px" UnboundType="Boolean" UnboundExpression="false" FieldName="assegnato">
              <PropertiesCheckEdit ClientInstanceName="Checkassegnato">
                <ClientSideEvents CheckedChanged="OnCheckedChanged" />
              </PropertiesCheckEdit>
        </dx:GridViewDataCheckColumn>
        <dx:GridViewDataCheckColumn Caption="Prioritario" FieldName="prioritaRaccolta" VisibleIndex="1"
            Width="90px">
            <PropertiesCheckEdit ClientInstanceName="CheckprioritaRaccolta">
                <ClientSideEvents CheckedChanged="OnPrioritaRaccoltaCheckedChanged" />
            </PropertiesCheckEdit>
        </dx:GridViewDataCheckColumn>
        <dx:GridViewDataTextColumn Caption=" id" FieldName="id" Visible="False" 
            VisibleIndex="2" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="magazziniereCodice" 
            FieldName="magazziniereCodice" Visible="False" VisibleIndex="3" 
            ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Ordine nr." FieldName="Code" 
            VisibleIndex="4" Width="120px" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Data ordine" FieldName="dataordine" 
            VisibleIndex="5" Width="120px" ReadOnly="True">
            <PropertiesTextEdit DisplayFormatString="d">
            </PropertiesTextEdit>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Codice cliente" FieldName="CustomerNo" 
            VisibleIndex="6" Width="140px" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Ragione sociale cliente" VisibleIndex="7" 
            ReadOnly="True" Name="ragsociale">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Magazziniere" FieldName="userDescription" 
            VisibleIndex="8" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Data assegnazione" 
            FieldName="dataAssegnazione" VisibleIndex="9" Width="120px" 
            ReadOnly="True">
                        <PropertiesTextEdit DisplayFormatString="d">
            </PropertiesTextEdit>
        </dx:GridViewDataTextColumn>
    </Columns>
    <SettingsBehavior AllowDragDrop="False" AllowGroup="False" />
    <SettingsPager Mode="ShowAllRecords" Visible="False">
    </SettingsPager>
    <SettingsEditing Mode="Batch">
    </SettingsEditing>
    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
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
                            VisibleIndex="3" Caption="Codice" Width="140px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="RowDiscount" ReadOnly="True" 
                            VisibleIndex="5" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LineID" ReadOnly="True" VisibleIndex="2" Caption="Riga Nr."
                            Width="80px" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="OldItemCode" VisibleIndex="6" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="CompanyName_" VisibleIndex="7" 
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
                        <dx:GridViewDataTextColumn FieldName="DiscountQty" VisibleIndex="8" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Imported" VisibleIndex="9" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="BinCode" ReadOnly="True" 
                            VisibleIndex="1" Width="180px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="QtyToShip" VisibleIndex="14" 
                            Visible="true" Caption="Q.ta Picked" Width="80px">
                            <PropertiesTextEdit DisplayFormatString="n0">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LotNo" ReadOnly="True" VisibleIndex="10" 
                            Width="120px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="FormulaSconto" VisibleIndex="11" 
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LineNo" VisibleIndex="12" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Totale Riga" Name="TotaleRiga" 
                            VisibleIndex="16" Width="100px" FieldName="totaleriga" 
                            UnboundType="Decimal" Visible="False">
                            <PropertiesTextEdit DisplayFormatString="c">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Descrizione" Name="Descrizione" FieldName="DESCRIZIONE" 
                            VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager Visible="False" Mode="ShowAllRecords">
                    </SettingsPager>
                    <Settings ShowFooter="True" />
                </dx:ASPxGridView>
            </DetailRow>
        </Templates>
</dx:ASPxGridView>
</asp:Content>
