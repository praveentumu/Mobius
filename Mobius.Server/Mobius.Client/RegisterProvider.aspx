<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterProvider.aspx.cs"
    Inherits="RegisterProvider" MasterPageFile="~/Unregistered.master" EnableEventValidation="false"
    Title="Register Provider" ValidateRequest="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>
<asp:Content ID="chRegisterProvider" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function GetLocalityByZipCode() {
            var Zipcode = document.getElementById('<%=txtPostalCode.ClientID %>').value;

            if (Zipcode.length == 5) {
                PageMethods.GetLocality(Zipcode, onSucess, onError);
            }
        }

        function onSucess(result) {
            var array = result.split(',');
            document.getElementById('<%=txtCity.ClientID %>').value = array[0];
            document.getElementById('<%=txtState.ClientID %>').value = array[1];
            document.getElementById('<%=txtCountry.ClientID %>').value = array[2];

        }

        function onError(result) {
            alert('Cannot process your request at the moment, please try later.');
        }
      
    </script>
    <asp:ScriptManager ID="smRegisterProvider" runat="server" AsyncPostBackTimeout="36000"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="updRegisterProvider" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="2" cellspacing="2">
                <%------------------------Lable Row--------------------------------%>
                <tr style="width: 100%">
                    <td colspan="4" align="left" class="Bold_text">
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <%----------------------Radio List Row-----------------------------------------%>
                <tr style="width: 100%">
                    <td colspan="4" align="left">
                        <asp:RadioButtonList ID="rbtlstProvider" class="bluetext" Font-Bold="true" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="rdbprovidercheckedChanged" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Text="Organizational Provider" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Individual Provider" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <%----------------------Indiviual Control Row-----------------------------------------%>
                <tr style="width: 100%" runat="server" id="rowIndiviual">
                    <td>
                        <tr>
                            <td class="text" valign="top" align="right" style="width: 20%">
                                <font color="red">*</font>First Name:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:TextBox Width="135px" ID="txtFirstName" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                                    ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                            <td class="text" valign="top" align="right" style="width: 20%">
                                <font color="red">*</font>Last Name:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:TextBox Width="135px" ID="txtLastName" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                                    ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="text" valign="top" align="right" style="width: 20%">
                                Middle Name:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:TextBox Width="135px" ID="txtMiddleName" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtMiddleName"
                                    ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                                    ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                            <td class="text" valign="top" align="right">
                                <font color="red">*</font>Gender:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:DropDownList ID="ddlGender" CssClass="text" runat="server" ValidationGroup="ProviderGroup"
                                    Width="135px">
                                </asp:DropDownList>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="text" valign="top" align="right">
                                <font color="red">*</font>E Mail:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:TextBox ID="txtIndividualEmail" runat="server" Width="135px" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationGroup="ProviderGroup"
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtIndividualEmail"
                                    ErrorMessage="Invalid Email"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                            <td class="text" valign="top" align="right">
                                <font color="red">*</font>Password:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:TextBox ID="txtPassword" runat="server" Width="135px" TextMode="Password" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="validatePassword" runat="server" ErrorMessage="Password must contain 8 charactes with a special key & numeric charcter."
                                    ControlToValidate="txtPassword" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
                                    EnableClientScript="false" Display="Dynamic">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </td>
                </tr>
                <%----------------------END Indiviual Row-----------------------------------------%>
                <%----------------------Organization Control Row-----------------------------------------%>
                <tr runat="server" id="rowOrganization" style="width: 100%">
                    <td class="text" align="right" style="width: 20%" valign="top">
                        <asp:Label ID="organizationRed" runat="server" ForeColor="Red" Text="* " AssociatedControlID="txtOrganizationName"
                            ValidationGroup="ProviderGroup"></asp:Label><asp:Label ID="lblorganization" runat="server"
                                ForeColor="Black" Text="Organization Name:" ValidationGroup="ProviderGroup">
                            </asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtOrganizationName" Width="135px" runat="server" ValidationGroup="ProviderGroup"
                            CssClass="text"></asp:TextBox>
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtOrganizationName"
                            ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                            ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                    </td>
                    <td class="text" align="right" valign="top">
                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="* " AssociatedControlID="txtOrganizationName"
                            ValidationGroup="ProviderGroup"></asp:Label><asp:Label ID="Label2" runat="server"
                                ForeColor="Black" Text="Password:" ValidationGroup="ProviderGroup">
                            </asp:Label>
                    </td>
                    <td class="text" align="left" valign="top">
                        <asp:TextBox ID="txtOrgPassword" TextMode="Password" Width="135px" runat="server"
                            ValidationGroup="ProviderGroup" CssClass="text"></asp:TextBox>
                        <br />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Password must contain 8 charactes with a special key & numeric charcter."
                            ControlToValidate="txtOrgPassword" ValidationExpression="(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$"
                            EnableClientScript="false" Display="Dynamic">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <%----------------------Common Controls Row-----------------------------------%>
                <tr style="width: 100%" runat="server" id="rowCommonControls">
                    <td colspan="4" align="right">
                        <tr>
                            <td class="text" align="right" style="width: 20%" valign="top">
                                <font color="red">*</font>Language:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:DropDownList ID="ddlLanguage" CssClass="text" runat="server" ValidationGroup="ProviderGroup"
                                    Width="135px" AutoPostBack="false">
                                </asp:DropDownList>
                                <br />
                            </td>
                            <td class="text" align="right" valign="top">
                                <font color="red">*</font>Medical Records Delivery Email Address:
                            </td>
                            <td align="left" class="style2" valign="top">
                                <asp:TextBox Width="135px" ID="txtdelievryEmail" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                    ValidationGroup="ProviderGroup" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ControlToValidate="txtdelievryEmail" ErrorMessage="Invalid Email"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                        </tr>
                        <%------------------------------------------------------------------------%>
                        <tr>
                            <td class="text" valign="top" align="right">
                                <font color="red">*</font>Type:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:DropDownList ID="ddlProviderType" CssClass="text" runat="server" ValidationGroup="ProviderGroup"
                                    Width="135px">
                                </asp:DropDownList>
                                <br />
                            </td>
                            <td class="text" align="right" valign="top">
                                <font color="red">*</font> Status:
                            </td>
                            <td align="left" class="text" valign="top">
                                <asp:DropDownList ID="ddlProviderStatus" CssClass="text" runat="server" ValidationGroup="ProviderGroup"
                                    Width="135px" AutoPostBack="false">
                                </asp:DropDownList>
                                <br />
                            </td>
                        </tr>
                        <%------------------------------------------------------------------------%>
                        <tr>
                            <td class="text" align="right" style="width: 20%" valign="top">
                                <font color="red">*</font>PostalCode:
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtPostalCode" runat="server" ValidationGroup="ProviderGroup" CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtPostalCode"
                                    ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                                    ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                            <td class="text" align="right" style="width: 20%" valign="top">
                                City:
                            </td>
                            <td align="left" class="text">
                                <asp:TextBox ID="txtCity" CssClass="text" runat="server" ValidationGroup="ProviderGroup"
                                    Width="135px"></asp:TextBox>
                                <br />
                            </td>
                        </tr>
                        <%------------------------------------------------------------------------%>
                        <tr>
                            <td class="text" align="right" valign="top">
                                State:
                            </td>
                            <td align="left" class="style2">
                                <asp:TextBox ID="txtState" CssClass="text" runat="server" ValidationGroup="ProviderGroup"
                                    Width="135px"></asp:TextBox>
                                <br />
                            </td>
                            <td class="text" align="right" valign="top">
                                Country:
                            </td>
                            <td align="left" class="text">
                                <asp:TextBox ID="txtCountry" CssClass="text" runat="server" ValidationGroup="ProviderGroup"
                                    Width="135px"></asp:TextBox>
                                <br />
                            </td>
                        </tr>
                        <%------------------------------------------------------------------------%>
                        <tr>
                            <td class="text" align="right" style="width: 20%" valign="top">
                                Street Number:
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox ID="txtStreetNumber" runat="server" Width="135px" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtStreetNumber"
                                    ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                                    ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                            <td class="text" align="right" valign="top">
                                Street Name:
                            </td>
                            <td align="left" class="style2">
                                <asp:TextBox ID="txtStreetName" Width="135px" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtStreetName"
                                    ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                                    ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                        </tr>
                        <%------------------------------------------------------------------------%>
                        <tr>
                            <td class="text" align="right" style="width: 20%" valign="top">
                                Electronic Service URI:
                            </td>
                            <td align="left">
                                <asp:TextBox Width="135px" ID="txtEURI" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                    ControlToValidate="txtEURI" ErrorMessage="Cannot contain special characters."
                                    ValidationExpression="[^<>]*" ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                            <td class="text" align="right" valign="top">
                                Contact:
                            </td>
                            <td align="left" class="text">
                                <asp:TextBox Width="135px" ID="txtContact" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                    ControlToValidate="txtContact" ErrorMessage="Cannot contain special characters."
                                    ValidationExpression="[^<>]*" ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <%------------------------------------------------------------------------%>
                        <tr>
                            <td class="text" align="right" valign="top">
                                Specialty:
                            </td>
                            <td align="left" class="text">
                                <asp:ListBox ID="lbxSpeciality" CssClass="text" runat="server" Width="135px" ToolTip="Press ctrl to select multiple speciality"
                                    ValidationGroup="ProviderGroup" Height="60px" SelectionMode="Multiple"></asp:ListBox>
                                <br />
                            </td>
                            <td class="text" align="right" valign="top">
                                Identifier:
                            </td>
                            <td align="left" valign="top">
                                <asp:TextBox Width="135px" ID="txtIdentifier" runat="server" ValidationGroup="ProviderGroup"
                                    CssClass="text"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server"
                                    ControlToValidate="txtIdentifier" ErrorMessage="Cannot contain special characters."
                                    ValidationExpression="[^<>]*" ValidationGroup="ProviderGroup"></asp:RegularExpressionValidator>
                                <br />
                            </td>
                        </tr>
                        <%------------------------------------------------------------------------%>
                        <tr style="height: 15px">
                        </tr>
                        <tr>
                            <td class="text" align="right" valign="top">
                                <font color="red">*</font>Certificate Signing Request:
                            </td>
                            <td align="left" class="text" valign="top" colspan="3">
                                <asp:TextBox Width="450px" ID="txtCSR" runat="server" ValidationGroup="ProviderGroup"
                                    Height="50px" TextMode="MultiLine" ReadOnly="true" CssClass="text"></asp:TextBox>
                                <span style="vertical-align: top">
                                    <asp:Button Text="Generate CSR" ID="btnGenerateCSR" runat="server" CssClass="Button"
                                        ValidationGroup="ProviderGroup" OnClick="btnGenerateCSR_Click" /></span>
                            </td>
                            <%-- <td class="text" align="left" valign="top">
                        Certificate Serial Number:
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox Width="135px" ID="txtCertificateSerialNumber" runat="server" ValidationGroup="ProviderGroup"
                            CssClass="text"></asp:TextBox>
                    </td>--%>
                        </tr>
                    </td>
                </tr>
                <%---------------------Note Row-----------------------------%>
                <tr>
                    <td colspan="2" align="left" style="height: 25px" valign="middle">
                        <asp:Label ID="lblNote" runat="server" class="text" ForeColor="red">Note:* Mark fields are compulsory.</asp:Label>
                    </td>
                </tr>
                <%--------------------Button Row-----------------------------------------%>
                <tr align="center">
                    <td align="right" class="text" valign="bottom" colspan="2">
                        <asp:Button ID="btnRegisterProvider" runat="server" CssClass="Button" Text="Register"
                            OnClick="btnRegisterProvider_Click" ValidationGroup="ProviderGroup" Width="60px" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="Button" ValidationGroup="ProviderGroup"
                            Text="Cancel" OnClick="CancelRegister" Style="margin-left: 14px" Width="50px" />
                    </td>
                    <td align="left" colspan="2" class="text">
                        <asp:UpdateProgress ID="Progpanel" runat="server" DynamicLayout="true" DisplayAfter="100"
                            AssociatedUpdatePanelID="updRegisterProvider">
                            <ProgressTemplate>
                                <p id="idSearchInProgress">
                                    <img width="32" height="32" alt="Action In Progress" src="<%= ResolveUrl("images/searching.gif")%>">
                                    Action In Progress...</p>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
