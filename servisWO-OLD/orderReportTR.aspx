<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="orderReportTR.aspx.vb" Inherits="servisWO.orderReportTR" %>


<%@ Register Assembly="Telerik.ReportViewer.Html5.WebForms, Version=12.1.18.620, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.Html5.WebForms" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
      <link href="http://kendo.cdn.telerik.com/2020.3.1118/styles/kendo.common.min.css" rel="stylesheet" id="commonCss" />
    <link href="http://kendo.cdn.telerik.com/2020.3.1118/styles/kendo.blueopal.min.css" rel="stylesheet" id="skinCss" />

        <style>
        body {
            margin: 5px;
            font-family: Verdana, Arial;
        }

        #ReportViewer1 {
            position: absolute;
            left: 4px;
            right: 4px;
            top: 0px;
            bottom: 4px;
            overflow: hidden;
            clear: both;
        }

        #theme-switcher {
            float: right;
            width: 12em;
            height: 30px;
        }
    </style>


</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <telerik:ReportViewer ID="ReportViewer1" runat="server" ScaleMode="FitPageWidth" ViewMode="PrintPreview" Width="1010px" Height="760px">
            <ReportSource Identifier="servisReports.Report1, servisReports, Version=4.21.0.0, Culture=neutral, PublicKeyToken=null" IdentifierType="TypeReportSource">
            </ReportSource>
        </telerik:ReportViewer>
    
    </div>
    </form>
</body>
</html>
