<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <link id="MyStyleSheet" runat="server" href="~/App_Themes/FGenesis/Mobius_style.css"
        rel="stylesheet" type="text/css" />
    <script language="javascript" src="Scripts/msPopup.js"></script>
    <script language="javascript" type="text/javascript">

        function ShowSearhIcon() {

            if (document.getElementById('<%=txtEmailAddress.ClientID%>').value != "") {
                var email = document.getElementById('<%=txtEmailAddress.ClientID%>').value;
                var pwdfilter = /^[\S]*$/;
                var filter = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
                document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
                if (email != "" && pwdfilter.test(email)) {
                    if (filter.test(email)) {
                        document.getElementById('<%=trprogress.ClientID%>').style.display = "block";
                        document.getElementById('<%=progress_image.ClientID%>').style.visibility = "visible";
                        setTimeout('document.images["<%=progress_image.ClientID%>"].src = "images/searching.gif"', 1200000);
                    }
                }

            }

        }
        

    </script>
</head>
<body style="background-color: #A5D3ED;">
    <script type="text/javascript" src="Scripts/jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.13.custom.min.js"></script>
    <form id="form1" runat="server">
    <div align="center" style="height: 150px; margin-top: 0;">
        <table border="0" cellpadding="4" cellspacing="6" width="100%">
            <tr valign="top" height="10px">
                <td class="text" align="center" colspan="2">
                    <asp:Label Text="" ForeColor="Red" class="text" ID="lblMessage" runat="server" Width="300px" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ShowSummary="true"
                        ValidationGroup="forgot" ShowMessageBox="false" class="text" />
                </td>
            </tr>
            <tr valign="middle" height="6px">
                <td class="text">
                    Email Address :
                </td>
                <td align="left" class="text">
                    <asp:TextBox runat="server" ID="txtEmailAddress" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEmailAddress"
                        ErrorMessage="Email address cannot be left blank." EnableClientScript="true"
                        ValidationGroup="forgot" Text="*" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                        ControlToValidate="txtEmailAddress" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                        EnableClientScript="true" ValidationGroup="forgot" Text="*"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr valign="middle" height="6px">
                <td class="text">
                    User Type:
                </td>
                <td align="left" class="text">
                    <asp:RadioButton ID="rbtProvider" runat="server" GroupName="UserGroup" Text="Provider"
                        Checked="true" />
                    <asp:RadioButton ID="rbtPatient" runat="server" GroupName="UserGroup" Text="Patient" />
                </td>
            </tr>
            <tr valign="middle" height="3px">
                <td align="center" colspan="2">
                    <asp:Button ID="Button1" runat="server" CssClass="ButtonBG" Text="Submit" OnClick="Button1_Click1"
                        ValidationGroup="forgot" OnClientClick="ShowSearhIcon()" />
                    &nbsp;
                    <asp:Button ID="btnclose" runat="server" CssClass="ButtonBG" Text="Close" OnClientClick="CloseMSWindow()" />
                </td>
            </tr>
            <tr valign="middle" id="trprogress" runat="server" height="3px" style="display: none">
                <td align="center" colspan="2">
                    <img id="progress_image" alt="" runat="server" style="visibility: hidden;" src="images/searching.gif" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script type="text/javascript">
        $('#' + '<%=txtEmailAddress.ClientID%>').focus(function () {
            $('#' + '<%=lblMessage.ClientID%>').text("");
        });
    </script>
</body>
</html>
