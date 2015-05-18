<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Login Page</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link id="MyStyleSheet" runat="server" href="~/App_Themes/FGenesis/Mobius_style.css"
        rel="stylesheet" type="text/css" />
    <link id="Link1" runat="server" href="~/App_Themes/FGenesis/MSPopup.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        body
        {
            background-image: url(images/blue_bg.gif);
        }
    </style>
</head>
<script type="text/javascript" src="Scripts/jquery-1.6.1.min.js"></script>
<script type="text/javascript" src="Scripts/msPopup.js"></script>
<script type="text/javascript" src="Scripts/jquery.easydrag.js"></script>
   <script type="text/javascript">
       var divWindow = $('#divWindow');
    </script>
<body>
 <div id="divWindow" onselectstart="return false" style="none repeat scroll 0%; -moz-background-clip: -moz-initial;
        -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;
        z-index: 999; visibility: visible; position: absolute; top: 190px; left: 170px;"
        onselectstart="return 0">
    </div>
 
    <div id="pdqbox">
    </div>
    <script language='JavaScript' type="text/javascript">
        <!--
        //For clearing browser history.        
        history.forward();

        $(function () {
            // add drag and drop functionality to #box1
            //  $("#divWindow").easydrag();

            // set a function to be called on a drop event
            //  $("#divWindow").ondrop();
        });

        function SetFocus() {
            document.form1['txtusername'].focus();
        }
        //window.onload = SetFocus;

        function DoOnclick() {
            var RetVal = "If you select the 'keep me logged in' option, user will be logged in for 2 weeks.";
            alert(RetVal);
        }


        function forgotPassword() {

            var caption = "Forgot Password";
            var url = "ForgotPassword.aspx";
            open_popup('CENTER', 'CENTER', url, 'Forgot Password', 350, 230, false)
            return false;
        }

        function importCertificate() {

            var caption = "Allow Login";
            var url = "AllowLogin.aspx";
            open_popup('CENTER', 'CENTER', url, 'Allow Login', 400, 225, false)
            return false;
        }

        function ActivateUser() {

            var caption = "Activate My Account";
            var url = "ActivateUser.aspx";
            open_popup('CENTER', 'CENTER', url, 'Activate My Account', 400, 230, false)
            return false;
        }
         
        // -->
    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="smRegisterProvider" runat="server" AsyncPostBackTimeout="36000"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updRegisterProvider" runat="server">
        <ContentTemplate>
            <!-- ImageReady Slices (main_page_slice.psd - Slices: 01, 02, 03, 04, 05, 06, 07, 08, 09, 10) -->
            <!-- End ImageReady Slices -->
            <asp:Panel runat="server" DefaultButton="Button1">
                <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="center" valign="middle">
                            <table width="630" border="4" cellpadding="0" cellspacing="0" bordercolor="#CCCCCC">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="9">
                                                    <img src="images/left02.gif" width="9" height="70">
                                                </td>
                                                <td width="277" bgcolor="#ececee">
                                                    <img src="images/first.gif" width="192" height="62">
                                                </td>
                                                <td width="36" bgcolor="#ececee">
                                                    &nbsp;
                                                </td>
                                                <td bgcolor="#ececee">
                                                    <img src="images/logo_big.png" width="466" height="60">
                                                </td>
                                                <td width="9" align="right">
                                                    <img src="images/right_top.gif" width="9" height="70">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" background="images/img.jpg" bgcolor="#E8E8E8">
                                        <table width="90%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 100px">
                                                    &nbsp;
                                                </td>
                                                <td align="right">
                                                    <table width="70%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td height="2" background="images/top_line.gif">
                                                            </td>
                                                            <td width="2">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td background="images/left_line.gif">
                                                            </td>
                                                            <td align="center" bgcolor="#A5D3ED">
                                                                <table width="97%" border="0" align="center" cellpadding="0" cellspacing="5" height="180px">
                                                                    <tr>
                                                                        <td colspan="2" align="left" style="height: 35px">
                                                                            <asp:Label ID="lblmessage" runat="server" ForeColor="Red" class="Bold_text"></asp:Label>
                                                                            <asp:ValidationSummary ID="ValidationSummary" runat="server" ForeColor="Red" ShowSummary="true"
                                                                                ValidationGroup="login" ShowMessageBox="false" class="text" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="text" align="left" style="height: 28px">
                                                                            Email Address:
                                                                        </td>
                                                                        <td class="Bold_text" align="left" style="height: 28px">
                                                                            <asp:TextBox ID="txtMail" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMail"
                                                                                ErrorMessage="Email address cannot be left blank." EnableClientScript="true"
                                                                                ValidationGroup="login" Text="*" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                                                            <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                                                                                ControlToValidate="txtMail" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                                                EnableClientScript="true" ValidationGroup="login" Text="*" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="text" align="left" style="height: 28px">
                                                                            Password:
                                                                        </td>
                                                                        <td class="Bold_text" align="left" style="height: 28px">
                                                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                                                                                ErrorMessage="Password cannot be left blank." Text="*" EnableClientScript="true"
                                                                                ValidationGroup="login"> </asp:RequiredFieldValidator>
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
                                                                            <asp:ImageButton ID="Button1" Width="80" ImageUrl="images/login.gif" runat="server"
                                                                                Text="Login" OnClick="btnLogin_Click" ValidationGroup="login" />
                                                                            <asp:ImageButton ID="btnRegister" Width="80" runat="server" Text="Register" ImageUrl="images/register.gif"
                                                                                OnClick="btnRegister_Click" Height="23px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" style="height: 27px" class="style3" colspan="2">
                                                                            <asp:LinkButton runat="server" ID="lnkForgotPassword" Text="Forgot Password?" CssClass="linkNew"
                                                                                OnClientClick="return forgotPassword()"></asp:LinkButton>
                                                                            &nbsp; | &nbsp;
                                                                            <asp:LinkButton runat="server" ID="lnkImportCertificate" Text="Allow Login"
                                                                                CssClass="linkNew" OnClientClick="return importCertificate()"></asp:LinkButton>
                                                                            &nbsp; | &nbsp;
                                                                            <asp:LinkButton runat="server" ID="lnkActivateUser" Text="Activate My Account" CssClass="linkNew"
                                                                                OnClientClick="return ActivateUser()"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td background="images/right_line.gif">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td height="2" background="images/bot_line.gif">
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <asp:HiddenField ID="hdnFieldFocus" runat="server" />
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="page_screen">
        &nbsp;</div>
   
    </form>
</body>
</html>
