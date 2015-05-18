<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageConsent.aspx.cs" Inherits="ManageConsent" %>

<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <head id="Head1">
                <title>Manage Consent</title>
            </head>
            <body>
                <div>
                    <table class="style1" width="100%">
                        <tr>
                            <td style="width: 33%">
                                <asp:RadioButton ID="rdOptOut" CssClass="Permission" AutoPostBack="true" OnCheckedChanged="rdOptOut_CheckedChanged"
                                    runat="server" GroupName="OpStatus" Text="OptOut" />
                            </td>
                            <td style="width: 33%">
                                <asp:RadioButton ID="rdOptIn" CssClass="Permission" runat="server" AutoPostBack="true"
                                    OnCheckedChanged="rdOptIn_CheckedChanged" Text="OptIn" GroupName="OpStatus" />
                            </td>
                            <td style="width: 33%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 33%">
                                &nbsp;
                            </td>
                            <td style="width: 33%">
                                &nbsp;
                            </td>
                            <td style="width: 33%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <cc1:GridCustomControl ID="gridUser" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    EnableNextPrevNumericPager="True" EnableSelection="False" GetKey="" HighlightColor=""
                                    OnPageIndexChanging="gridUser_PageIndexChanging" OnRowEditing="gridUser_RowEditing"
                                    OnRowDeleting="gridUser_RowDeleting" OnRowDataBound="gridUser_RowDataBound" SetGridStyle=""
                                    ShowSortDirection="False" ShowSortImageBeforeHeaderText="False" SortDirectionAlt="Ascending"
                                    DataKeyNames="PatientConsentID" EmptyDataText="No records To show" SortExpressionAlt=""
                                    Width="100%">
                                </cc1:GridCustomControl>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 33%">
                                &nbsp;
                            </td>
                            <td style="width: 33%" align="right">
                                <asp:Button ID="btnSave" OnClick="btnSave_onClick" CssClass="Button" runat="server"
                                    Text="Save" />
                            </td>
                            <td style="width: 33%" align="left">
                                <asp:Button ID="btnConfigureConsent" CssClass="Button" OnClick="btnConfigureConsent_onClick"
                                    runat="server" Text="Configure Consent" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- </form>--%>
            </body>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
