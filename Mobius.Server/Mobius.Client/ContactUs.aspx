<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="ContactUs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
         <link id="MyStyleSheet" runat="server" href="~/App_Themes/FGenesis/Mobius_style.css"
        rel="stylesheet" type="text/css" />
    <script language="javascript" src="Scripts/msPopup.js"> </script>
    <script type="text/javascript" src="Scripts/jquery-1.6.1.min.js"></script>
</head>
<body style="background-color: #A5D3ED;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div align="center" style="height: 170px; margin-top: 0;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left" valign="top" class="text" colspan="2">
                            <p  style="padding-left:10px; font-size:11px;">
                                                        <br />
                          <strong><font color=red>Information </font></strong> 
                          <br />    <br /> Your account is expired. To contact system administrator for assistance, click on Submit.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="baseline" class="text">
                            <asp:Label ID="lblErrorMsg" runat="server" class="text" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                        <td colspan="2" align="center" class="text" style="padding-top:10px">
                            <asp:Button ID="btnSubmit" CssClass="Button" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                                Width="55px" />
                            <asp:Button ID="btnclose" Width="55px" runat="server" CssClass="Button" Text="Close"
                                OnClientClick="CloseMSWindow()" />
                        </td>
                    </tr>
                    <%--  <tr valign="middle" height="3px" id="trprogress" runat="server">
                       
                    </tr>--%>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
