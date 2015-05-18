<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllowLogin.aspx.cs" Inherits="AllowLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AllowLogin</title>
    <link id="MyStyleSheet" runat="server" href="~/App_Themes/FGenesis/Mobius_style.css"
        rel="stylesheet" type="text/css" />
    <script language="javascript" src="Scripts/msPopup.js"></script>
    <style type="text/css">
        .style3
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            font-style: normal;
            color: #000000;
            text-decoration: none;
            width: 179px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function ShowSearhIcon() {
            if (document.getElementById('<%=lblMessage.ClientID%>').innerHTML != "") {
                document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
            }
            var email = document.getElementById('<%=txtMail.ClientID%>').value;
            var pwd = document.getElementById('<%=txtPassword.ClientID%>').value;
            var pwdfilter = /^[\S]*$/;
            var filter = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;

            if (email != " " && pwd != " ") {
                if (pwdfilter.test(email) && pwdfilter.test(pwd)) {
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
    <form id="form1" runat="server">
    <div align="center" style="height: 100px; margin-top: 0;">
        <table width="97%" border="0" align="center" cellpadding="0" cellspacing="5" height="180px">
            <tr>
                <td colspan="2" align="left" style="height: 35px">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" class="Bold_text"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ShowSummary="true"
                        ValidationGroup="Import" ShowMessageBox="false" class="text" />
                </td>
            </tr>
            <tr>
                <td class="text" align="left" style="height: 28px">
                    Email Address:
                </td>
                <td class="Bold_text" align="left" style="height: 28px">
                    <asp:TextBox ID="txtMail" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMail"
                        ErrorMessage="Email address cannot be left blank." EnableClientScript="true"
                        ValidationGroup="Import" Text="*" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                        ControlToValidate="txtMail" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                        EnableClientScript="true" ValidationGroup="Import" Text="*"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="text" align="left" style="height: 28px">
                    Security Password:
                </td>
                <td class="Bold_text" align="left" style="height: 28px">
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Security Password cannot be left blank." Text="*" EnableClientScript="true"
                        ValidationGroup="Import"> </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="left" class="text">
                    User Type:
                </td>
                <td align="left" class="text">
                    <asp:RadioButton ID="rbtProvider" runat="server" GroupName="UserGroup" Text="Provider"
                        Checked="true" />
                    <asp:RadioButton ID="rbtPatient" runat="server" GroupName="UserGroup" Text="Patient" />
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 27px" class="style3" colspan="2">
                    <asp:Button ID="ImportPFXCertificate" runat="server" CssClass="ButtonBG" Text="Submit"
                        OnClick="ImportPFXCertificate_Click" ValidationGroup="Import" OnClientClick="ShowSearhIcon()" />&nbsp;
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
</body>
</html>
