<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadPatientRecord.aspx.cs"
    Inherits="UploadPatientRecord" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" language="javascript">
        function CloseAndRedirect(redirectionLink) {
            window.opener.location = redirectionLink;
            window.self.close();
        }
    </script>
    <link href="App_Themes/FGenesis/style.css" type="text/css" rel="stylesheet" />    
    <link rel="stylesheet" type="text/css" href="app_Themes/FGenesis/Mobius_style.css" />
    <title>Upload Patient Record(s)</title>
    <base target="_parent" />
</head>
<body >
    <form id="form1" runat="server" >
    <table  width="100%" border="0" cellspacing="0" cellpadding="2"
        height="150px">
        <tr>
            <td align="center">
                &nbsp;
                <asp:Label ID="lblmessage" runat="server" ForeColor="Red" Font-Bold="true" CssClass="text"></asp:Label>
                <asp:HiddenField  runat="server" ID="hdnPatient" /> 
            </td>
        </tr>
        <tr>
            <td align="center" valign="top" class="text">
                C32 Document:
                <asp:FileUpload ID="fileC32Document" runat="server" />
            </td>
        </tr>
        <tr style="height: 5px">
            <td>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
