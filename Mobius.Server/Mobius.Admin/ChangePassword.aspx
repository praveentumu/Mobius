<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs"
    Inherits="ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Change Password</title>
    <link id="MyStyleSheet" runat="server" href="~/App_Themes/FGenesis/Mobius_style.css"
        rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        function ShowSearhIcon() {

            if (document.getElementById('<%=txtOldPassword.ClientID%>').value != "" && document.getElementById('<%=txtNewPassword.ClientID%>').value != "" && document.getElementById('<%=txtConfirmPassword.ClientID%>').value != "") {
                if (document.getElementById('<%=txtNewPassword.ClientID%>').value == document.getElementById('<%=txtConfirmPassword.ClientID%>').value) {
                    document.getElementById('<%=lblErrorMsg.ClientID%>').innerHTML = "";
                    document.getElementById('<%=trprogress.ClientID%>').style.display = "block";
                    document.getElementById('<%=progress_image.ClientID%>').style.visibility = "visible";
                    setTimeout('document.images["<%=progress_image.ClientID%>"].src = "images/searching.gif"', 1200000);
                }
            }

        }       
       

    </script>
    <script type="text/javascript" src="Scripts/msPopup.js"></script>
</head>
<body style="background-color: #A5D3ED;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr valign="top" height="30px">
            <td colspan="2" align="left" class="text">
                <asp:Label ID="lblErrorMsg" runat="server" class="text" ForeColor="red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary" runat="server" ForeColor="Red" ShowSummary="true"
                    class="text" ValidationGroup="ValidateBlank" ShowMessageBox="false" DisplayMode="BulletList" />
            </td>
        </tr>
        <tr valign="middle" height="8px">
            <td align="right" class="text" valign="top">
                Old Password:
            </td>
            <td align="left" class="text" valign="top">
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ValidateOldPassword" runat="server" ControlToValidate="txtOldPassword"
                    class="text" ErrorMessage="Enter old password." SetFocusOnError="True" Text="*"
                    ValidationGroup="ValidateBlank"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr valign="middle" height="8px">
            <td align="right" class="text" valign="top">
                New Password:
            </td>
            <td align="left" class="text" valign="top">
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ValidateNewPassword" runat="server" ControlToValidate="txtNewPassword"
                    class="text" ErrorMessage="Enter new password." SetFocusOnError="True" Text="*"
                    ValidationGroup="ValidateBlank"></asp:RequiredFieldValidator>
                <br />
            </td>
        </tr>
        <tr valign="middle" height="8px">
            <td align="right" class="text" valign="top">
                Confirm Password:
            </td>
            <td align="left" class="text" valign="top">
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="validaterequiredconfirm" runat="server" ControlToValidate="txtConfirmPassword"
                    class="text" ErrorMessage="Enter confirm password." SetFocusOnError="True" Text="*"
                    ValidationGroup="ValidateBlank"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="ValidateEqual" runat="server" ControlToCompare="txtNewPassword"
                    class="text" ControlToValidate="txtConfirmPassword" ErrorMessage="New password and Confirm password do not match."
                    ForeColor="red" Text="*" Type="String" ValidationGroup="ValidateBlank" />
            </td>
        </tr>
        <tr valign="middle" height="3px">
            <td align="center" colspan="2">
                <br />
                <asp:Button ID="Button1" runat="server" CssClass="Button" OnClick="btnChangePassword_Click"
                    OnClientClick="ShowSearhIcon()" Text="Change Password" ValidationGroup="ValidateBlank" />&nbsp;
                <asp:Button ID="btnclose" runat="server" CssClass="Button" Text="Close" OnClientClick="CloseMSWindow()" />
            </td>
        </tr>
        <tr valign="middle" height="3px" id="trprogress" runat="server">
            <td align="center" colspan="2">
                <img id="progress_image" alt="" runat="server" style="visibility: hidden;" src="images/searching.gif" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
