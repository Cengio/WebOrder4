<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/default.Master" CodeBehind="userAudit.aspx.vb" Inherits="servisWO.userAudit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_titoli" runat="server">
    <dx:ASPxLabel ID="lb_cTitolo" runat="server" Font-Bold="True" 
    Font-Size="Medium" ForeColor="#FF8800" 
    Text="accessi utenti" 
    Theme="MetropolisBlue">
</dx:ASPxLabel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Height="100%" Width="100%">
    <Panes>
        <dx:SplitterPane ScrollBars="Auto">
            <ContentCollection>
                <dx:SplitterContentControl runat="server">
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" EnableTheming="True" KeyFieldName="id" Theme="MetropolisBlue" Width="100%">
                        <SettingsPager PageSize="50" Position="TopAndBottom" ShowSeparators="True">
                        </SettingsPager>
                        <Settings ShowFilterRow="True" ShowGroupPanel="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                        <Columns>
                            <dx:GridViewCommandColumn ShowClearFilterButton="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="User Code" ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Utente" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="Event Date" ShowInCustomizationForm="True" VisibleIndex="4">
                                <PropertiesDateEdit DisplayFormatString="G">
                                </PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn FieldName="Event" ShowInCustomizationForm="True" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WORconnectionString %>" SelectCommand="select a.id,u.[User Code],(u.cognome + ' ' + u.nome) as Utente,a.[Event Date],a.[Event] from wu_audit a inner join wu_users u on a.[User Code]=u.[User Code] ORDER BY a.[Event Date] DESC"></asp:SqlDataSource>
                </dx:SplitterContentControl>
            </ContentCollection>
        </dx:SplitterPane>
        <dx:SplitterPane AutoWidth="True" ScrollBars="Auto">
            <Separator Visible="False">
            </Separator>
            <ContentCollection>
                <dx:SplitterContentControl runat="server">
                    <dx:WebChartControl ID="WebChartControl1" runat="server" CrosshairEnabled="True" DataSourceID="SqlDataSource2" Height="640px" Width="500px">
                        <DiagramSerializable>
                            <dx:XYDiagram Rotated="True">
                            <axisx visibleinpanesserializable="-1" minorcount="1">
                                
                            </axisx>
                            <axisy visibleinpanesserializable="-1">
                            </axisy>
                            </dx:XYDiagram>
                        </DiagramSerializable>
                        <SeriesSerializable>
                            <dx:Series ArgumentDataMember="Utente" Name="Series 1" ShowInLegend="False" ToolTipHintDataMember="Utente" ValueDataMembersSerializable="Accessi" >
                            <viewserializable>
                                <dx:SideBySideBarSeriesView BarWidth="0.5" ColorEach="True">
                                </dx:SideBySideBarSeriesView>
                            </viewserializable>
                            </dx:Series>
                        </SeriesSerializable>
                        <SeriesTemplate ArgumentDataMember="Utente" ValueDataMembersSerializable="Accessi" />
                    </dx:WebChartControl>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:WORconnectionString %>" SelectCommand="select COUNT(*) as Accessi ,(u.cognome + ' ' + u.nome) as Utente from wu_audit a inner join wu_users u on a.[User Code]=u.[User Code] WHERE a.[Event]='LOGIN' GROUP BY (u.cognome + ' ' + u.nome) ORDER BY Accessi "></asp:SqlDataSource>
                </dx:SplitterContentControl>
            </ContentCollection>
        </dx:SplitterPane>
    </Panes>
</dx:ASPxSplitter>
</asp:Content>
