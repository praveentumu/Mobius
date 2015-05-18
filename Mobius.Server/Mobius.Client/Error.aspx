<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body style="background-color: #A5D3ED;">
    <form id="form1" runat="server">
    <table cellpadding="2" cellspacing="2" width="100%">
        <tr>
            <td align="left">
                <ul style="color:Red">
                    <li> <asp:Label ID="lblerrorMessage" runat="server" ForeColor="Red" CssClass="formLabel"
                    Font-Size="Small"></asp:Label>   </li>
                </ul>
                
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
