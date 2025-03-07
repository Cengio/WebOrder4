<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="err403.aspx.vb" Inherits="servisWO.err403" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Access Denied</title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="auto-style1" style="vertical-align: middle;">
    
        <div class="auto-style1">
            <br />
            <br />
            <br />
            <dx:ASPxImage ID="ASPxImage1" runat="server" ImageUrl="~/images/accessdenied.png" ShowLoadingImage="True" style="text-align: center">
            </dx:ASPxImage>
            <br />
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="Medium" Text="access denied">
            </dx:ASPxLabel>
            <br />
            <br />
            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="please login here!" Theme="MetropolisBlue">
            </dx:ASPxButton>
        </div>
    
    </div>
    </form>
</body>
</html>
