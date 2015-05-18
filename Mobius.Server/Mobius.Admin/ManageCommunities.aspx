<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeFile="ManageCommunities.aspx.cs" Inherits="ManageCommunities" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel ID="mngcomminuty" runat="server" UpdateMode="Always">
        <ContentTemplate>
    <table style="width: 100%" border="0">
        <tr>
            <td colspan="4" align="left" class="Bold_text">
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr style="height: 25px;">
            <td colspan="4" align="right">
                <a href="ImportCommunities.aspx" class="linkNew">Import Communities</a>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table style="border: 0px; width: 100%; max-height: 400px;">
                    <tr>
                        <td colspan="4">

                            <asp:GridView ID="grdCommunities" Width="100%" runat="server" AutoGenerateColumns="false"
                                ShowFooter="true" CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor"
                                PageSize="25" PagerStyle-HorizontalAlign="Right" AllowPaging="true" AllowSorting="true"
                                OnRowEditing="grdCommunities_RowEditing" OnRowCancelingEdit="grdCommunities_RowCancelingEdit"
                                OnRowUpdating="grdCommunities_RowUpdating" OnRowDeleting="grdCommunities_RowDeleting"
                                OnPageIndexChanging="grdCommunities_PageIndexChanging" 
                                onrowcommand="grdCommunities_RowCommand" 
                                onrowdatabound="grdCommunities_RowDataBound">
                                <RowStyle Height="20px" Font-Size="13px" BorderColor="#CCCCCC" BorderStyle="Solid"
                                    BorderWidth="1px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Community Identifier" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="CommunityIdentifier" runat="server" Text='<%#Eval("CommunityIdentifier") %>'></asp:Label>
                                            <asp:HiddenField ID="HiddenFieldID" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="EditCommunityIdentifier" runat="server" Text='<%#Eval("CommunityIdentifier") %>'></asp:TextBox>
                                            <asp:HiddenField ID="EditHiddenFieldID" runat="server" Value='<%# Eval("ID") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="AddCommunityIdentifier" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="EditHiddenFieldID" runat="server" />
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Community Description" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="CommunityDescription" runat="server" Text='<%#Eval("CommunityDescription") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="EditCommunityDescription" runat="server" Text='<%#Eval("CommunityDescription") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="AddCommunityDescription" runat="server"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Home Community" HeaderStyle-Width="50px">
                                        <ItemTemplate>
<%--                                            <asp:RadioButton ID="IsHomeCommunity" runat="server" Enabled="false" Checked='<%#Eval("IsHomeCommunity") %>' />
--%>                                             <asp:Label ID="IsHomeCommunity" runat="server" Text='<%#Eval("IsHomeCommunity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="EditIsHomeCommunity" runat="server" Checked='<%#Eval("IsHomeCommunity") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="AddIsHomeCommunity" runat="server" />
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Active" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                           <%-- <asp:CheckBox ID="Active" runat="server" Enabled="false" Checked='<%#Eval("Active") %>'>
                                            </asp:CheckBox>--%>
                                             <asp:Label ID="Active" runat="server" Text='<%#Eval("Active") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="EditActive" runat="server" Checked='<%#Eval("Active") %>'></asp:CheckBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="AddActive" runat="server"></asp:CheckBox>
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit/Delete" HeaderStyle-Width="50%">
                                        <ItemTemplate>
<%--                                            <asp:LinkButton ID="btnEdit" Text="Edit" runat="server" CommandName="Edit"  />
--%>                                             <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="images/edit.png" ToolTip="Edit" Height="16px" Width="16px" />
                                            <span onclick="return confirm('Sure to delete this record?')">
                                                 <asp:ImageButton ID="btnDelete" CommandName="Delete" runat="server" ImageUrl="images/cross.png" ToolTip="Delete" Height="16px" Width="16px" />
<%--                                                <asp:LinkButton ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" />
--%>                                            </span>
                                        </ItemTemplate>
                                  
                                        <EditItemTemplate>
                                           <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="images/base_floppydisk_32.png" ToolTip="Update" Height="16px" Width="16px" />
                                          <%--  <asp:LinkButton ID="btnUpdate" Text="Update" runat="server" CommandName="Update" />--%>
                                              <asp:ImageButton ID="btnCancel" CommandName="Cancel" runat="server" ImageUrl="images/Close.gif" ToolTip="Cancel" Height="16px" Width="16px" />

<%--                                            <asp:LinkButton ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
--%>                                        </EditItemTemplate>
                                        <FooterTemplate>
                                           <asp:ImageButton ID="btnInsertRecord" CommandName="Insert" runat="server" ImageUrl="images/add_row.png" ToolTip="Add" Height="16px" Width="16px" />

                                            <%--<asp:Button ID="btnInsertRecord" runat="server" Text="Add" ValidationGroup="ValgrpCust"
                                                CommandName="Insert" />--%>
                                        </FooterTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
 
