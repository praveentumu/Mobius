<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeFile="ChangeEmailAddress.aspx.cs" Inherits="ChangeAdminemailAddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="display: none">
        <tr>
            <td class="text" align="left" style="height: 28px">
                Email Address:
            </td>
            <td class="text" align="left" style="height: 28px; width: 185px;">
                <asp:TextBox ID="txtMail" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMail"
                    ErrorMessage="Email address cannot be left blank." EnableClientScript="true"
                    ValidationGroup="Revoke" Text="*" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                    ControlToValidate="txtMail" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                    EnableClientScript="true" ValidationGroup="change" Text="*"></asp:RegularExpressionValidator>
            </td>
            <td align="left" class="text">
                <asp:Button ID="btnSearch" runat="server" ValidationGroup="change" CssClass="Button"
                    OnClick="btnSearch_onClick" Text="Add" Width="80px" />
            </td>
        </tr>
    </table>
    <br />
    <div class="text" style="padding: 1px">
        <asp:GridView ID="gvDetails" DataKeyNames="ID" Width="100%" runat="server" AutoGenerateColumns="False"
            CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor" PageSize="5"
            PagerStyle-HorizontalAlign="Right" AllowSorting="True" OnRowCancelingEdit="gvDetails_RowCancelingEdit"
            OnRowEditing="gvDetails_RowEditing" OnRowUpdating="gvDetails_RowUpdating" OnRowCommand="gvDetails_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Email Address" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="170px">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEMailAddress" runat="server" Text='<%# Bind("Email") %>' Width="500px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEMailAddress"
                            ErrorMessage="Email address cannot be left blank." EnableClientScript="true"
                            ValidationGroup="change" Text="*" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email."
                            ControlToValidate="txtEMailAddress" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            EnableClientScript="true" ValidationGroup="change" Text="*" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEMailAddress" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="bluetext" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update"
                            ValidationGroup="change" CausesValidation="true" CssClass="bluetext" />
                        ||
                        <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"
                            CssClass="bluetext" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>
</asp:Content>
