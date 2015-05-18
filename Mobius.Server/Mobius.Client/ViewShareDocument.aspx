<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ViewShareDocument.aspx.cs"
    Inherits="ViewShareDocument" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Shared Document.</title>
    <link href="App_Themes/FGenesis/style.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="app_Themes/FGenesis/sdmenu.css" />
    <link rel="stylesheet" type="text/css" href="app_Themes/FGenesis/Mobius_style.css" />
    <%--<link rel="stylesheet" type="text/css" href="app_Themes/FGenesis/jquery-ui-1.8.13.custom.css">
    <link rel="stylesheet" type="text/css" href="app_Themes/FGenesis/ui.dropdownchecklist.themeroller.css">--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellspacing="2" cellpadding="0">
            <tr>
                <td colspan="2" align="left" class="Bold_text">
                    <asp:HiddenField ID="hdnForImage" runat="server" />
                    <asp:HiddenField ID="hdnSubject" runat="server" />
                    <asp:HiddenField ID="hdnPatientId" runat="server" />
                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="htnErrorMessage" runat="server" />
        <table>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gridDocument" Width="100%" runat="server" AutoGenerateColumns="false"
                        CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor" PageSize="5"
                        PagerStyle-HorizontalAlign="Right" AllowPaging="true" AllowSorting="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Document ID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                    <asp:Label ID="lblOriginaldocumentID" runat="server" Text='<%# Bind("OriginalDocumentID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Document Title" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("DocumentTitle") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Author" HeaderText="Author" ItemStyle-HorizontalAlign="Left"
                                ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="CreatedDate" ItemStyle-Wrap="true" HeaderText="Created Date"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="74px" />
                            <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                HeaderStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnView" runat="server" ToolTip="View" CommandName="View" ImageUrl="~/images/Add.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
