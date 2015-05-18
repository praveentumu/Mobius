<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllowOtherDevices.aspx.cs"
    Inherits="AllowOtherDevices" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Forgot Password</title>
    <link id="MyStyleSheet" runat="server" href="~/App_Themes/FGenesis/Mobius_style.css"
        rel="stylesheet" type="text/css" />
    <script language="javascript" src="Scripts/msPopup.js"> </script>
    <script type="text/javascript" src="Scripts/jquery-1.6.1.min.js"></script>
<script language="javascript" type="text/javascript">
    function hasWhiteSpace(pwd) {
        if (document.getElementById(pwd).value.indexOf(' ') >= 0) {
            alert('Please remove white space.');
            return false;
        }
       
    }
</script>
      
   
</head>
<body style="background-color: #A5D3ED;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div align="center" style="height: 150px; margin-top: 0;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left" valign="top" class="text" colspan="2">
                            <br>
                            <strong>Information:</strong>
                            <br />
                            <p>
                                This option allows user to share details for mobius application access from other
                                devices (than he registered) like tablet(s), mobile(s), PC(s).
                            </p>
                            <p>
                                Information shared is protected by password chosen by the user.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="text" valign="top">
                            Security Password:
                        </td>
                        <td align="left" class="text" valign="top">
                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="176px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="text" align="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="Please enter security password." EnableClientScript="true"> </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" class="text">
                            <asp:Button ID="btnPwd" CssClass="Button" runat="server" Text="OK"  OnClick="btnPwd_Click"
                                Width="55px"  />
                            <asp:Button ID="btnclose" Width="55px" runat="server"   CssClass="Button" Text="Close"
                                OnClientClick="CloseMSWindow()" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="baseline" class="text">
                            <br />
                            <asp:Label ID="lblErrorMsg" runat="server" class="text" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="middle" height="3px" id="trprogress" runat="server">
                        <td align="center" class="text" colspan="2">
                            <asp:UpdateProgress ID="Progpanel" runat="server" DynamicLayout="true" DisplayAfter="100"
                                AssociatedUpdatePanelID="updatePanel1">
                                <ProgressTemplate>
                                    <p id="idSearchInProgress">
                                        <img width="32" height="32" alt="Action In Progress" src="<%= ResolveUrl("images/searching.gif")%>">
                                        Action In Progress...</p>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
