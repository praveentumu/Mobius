<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActivateDeactivateUser.aspx.cs"
    MasterPageFile="~/MasterPage.master" Inherits="ActivateDeactivateUser" %>

<asp:Content ID="chReferPatientNext" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language='JavaScript' type="text/javascript">
        function GetSelectedMailID(EmailAddress, hdnField, chkID, hdnCertInfo, serialNumber,userName,status) {
            document.getElementById(hdnField).value = "";
            document.getElementById(hdnCertInfo).value = "";
            document.getElementById('<%=hdnUserName.ClientID%>').value = "";
            document.getElementById('<%=hdnStatus.ClientID%>').value = "";
            
            if (document.getElementById(chkID).checked == true) {
                document.getElementById(hdnField).value = EmailAddress;
                document.getElementById(hdnCertInfo).value = serialNumber;
                document.getElementById('<%=hdnUserName.ClientID%>').value = userName;
                document.getElementById('<%=hdnStatus.ClientID%>').value = status;
            }
        }

        function CheckMailID(hdnField) {
            if (document.getElementById(hdnField).value == '') {
                alert('Please select a user.');
                return false;
            }
        }
        function ClearMessage(lblErrorMessage) {
            document.getElementById(lblErrorMessage).innerHTML = "";
        }
     
    </script>
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div id="DivRequester" runat="server">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left" width="100%">
                            <%--Start Search Criteria--%>
                            <div class="text" style="padding: 1px">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="#9DDFFF" align="left" width="17%" colspan="4">
                                            <div class="Bold_text_heading" style="padding: 5px">
                                                Search User:</div>
                                            <div class="text" style="padding: 5px">
                                                <asp:Label ID="lblPatientName" runat="server" Font-Bold="false"></asp:Label></div>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ShowSummary="true"
                                                ValidationGroup="Revoke" ShowMessageBox="false" class="text" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="text" align="left" style="height: 28px">
                                            Email Address:
                                        </td>
                                        <td class="text" align="left" style="height: 28px;">
                                            <asp:TextBox ID="txtMail" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMail"
                                                ErrorMessage="Email address cannot be left blank." EnableClientScript="true"
                                                ValidationGroup="Revoke" Text="*" ForeColor="Red" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email address."
                                                ControlToValidate="txtMail" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                EnableClientScript="true" ValidationGroup="Revoke" Text="*"></asp:RegularExpressionValidator>
                                        </td>
                                        <td align="left" class="text">
                                            User Type:
                                        </td>
                                        <td align="left" class="text">
                                            <asp:RadioButton ID="rbtProvider" runat="server" GroupName="UserGroup" Text="Provider"
                                                Checked="true" />
                                            <asp:RadioButton ID="rbtPatient" runat="server" GroupName="UserGroup" Text="Patient" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnSearch" runat="server" ValidationGroup="Revoke" CssClass="Button"
                                                OnClick="btnSearch_onClick" Text="Search" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--End Search Criteria--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                     <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--Start Display Grid--%>
                            <div class="text" style="padding: 1px">
                                <asp:GridView ID="GridUserInfo" DataKeyNames="EmailAddress,SerialNumber,GivenName,Status" Width="100%"
                                    runat="server" AutoGenerateColumns="false" CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor"
                                    OnRowDataBound="GridUserInfo_RowDataBound" PageSize="5" PagerStyle-HorizontalAlign="Right"
                                    AllowSorting="true">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="2px" />
                                            <ItemTemplate>
                                                <%--<asp:CheckBox runat="server" ID="chkSelected" />--%>
                                                <asp:RadioButton runat="server" ID="chkSelected" GroupName="grp1" AutoPostBack="True" 
			                                     OnCheckedChanged="chkSelected1_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Name" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("GivenName") %>' ></asp:Label>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("FamilyName") %>' ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email Address" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="170px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEMailAddress" runat="server" Text='<%# Bind("EmailAddress") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserType" runat="server" Text='<%# Bind("UserType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="State" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Postal Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPostalCode" runat="server" Text='<%# Bind("PostalCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:Button ID="btnRevokeCertificate" runat="server" Visible="false" Text="Deactivate"
                                    CssClass="Button" Width="120px" OnClick="btnRevokeCertificate_Click" />
                                <asp:Button ID="btnActivate" runat="server" Visible="false" Text="Activate" CssClass="Button"
                                    Width="120px" OnClick="btnActivate_Click" />
                            </div>
                            <%--End Display Grid--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" class="text">
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
            <asp:HiddenField ID="hdnSelectedMailID" runat="server" />
            <asp:HiddenField ID="hdnCertInfo" runat="server" />
            <asp:HiddenField ID="hdnUserType" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnStatus" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
