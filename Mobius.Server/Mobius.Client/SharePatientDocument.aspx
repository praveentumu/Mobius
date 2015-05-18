<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SharePatientDocument.aspx.cs"
    MasterPageFile="~/MasterPage.master" Inherits="SharePatientDocument" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>
<asp:Content ID="chReferPatientNext" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        var IsShareButtonClick = false;
        function ConfirmationMessage() {
            var message;
            message = confirm('Do you want to remove the sharing?');

            if (message != 0)
                return true;
            else
                return false;
        }

      

    </script>
    <asp:UpdatePanel ID="updReferPatientNextMain" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblmessage" runat="server" ForeColor="Red" Font-Bold="true" class="Bold_text"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="left">
                        <asp:GridView ID="gridDocument" Width="940px" runat="server" AutoGenerateColumns="false"
                            CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor" PagerStyle-HorizontalAlign="Right">
                            <Columns>
                                <asp:TemplateField HeaderText="Document ID" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                    <ItemTemplate>
                                        <asp:LinkButton Text='<%# Bind("DocumentUniqueId") %>' CssClass="linkNew" runat="server"
                                             ID="lblOriginaldocumentID" OnClick="ViewPatientDocument"
                                            CommandArgument="" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Title" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("DocumentTitle") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Author" HeaderText="Author" ItemStyle-HorizontalAlign="Left"
                                    ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" />
                                <asp:BoundField DataField="CreatedOn" ItemStyle-Wrap="true" HeaderText="Created Date"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10px" />
                                <asp:BoundField DataField="DataSource" HeaderText="Data Source" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="20px" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
      

            <table width="100%" border="0" cellspacing="2" cellpadding="0">
                <tr>
                    <td colspan="2" align="left" class="Bold_text">
                        <asp:HiddenField ID="hdnForImage" runat="server" />
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top" width="20%">
                        <table width="100%" border="0" cellspacing="2" cellpadding="0">
                            <tr>
                                <td bgcolor="#9DDFFF" align="left" width="17%">
                                    <div class="Bold_text_heading" style="padding: 5px">
                                        Patient Summary</div>
                                    <%--<div class="text" style="padding: 5px">
                                        <asp:Label ID="lblPatientName" runat="server"></asp:Label></div>--%>
                                    <div class="text" style="padding: 1px">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr runat="server" id="trName">
                                                <td align="left">
                                                    Name:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trGender">
                                                <td align="left">
                                                    Gender:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblgender" runat="server" CssClass="Bold_text"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trDOB">
                                                <td align="left">
                                                    DOB:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDOB" runat="server" CssClass="Bold_text"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ffffff" align="left" valign="top" width="17%">
                                    <asp:HiddenField ID="hdnOriginalDocumentID" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnSharedDocumentID" runat="server" Value="" />
                                    <asp:GridView ID="gridSharedWith" runat="server" CssClass="grid" AutoGenerateColumns="false"
                                        PagerStyle-HorizontalAlign="Right" AllowPaging="true" PageSize="4" AlternatingRowStyle-CssClass="alternateRowColor"
                                        OnPageIndexChanging="gridSharedWith_PageIndexChanging" OnRowDataBound="gridSharedWith_RowDataBound"
                                        OnRowCommand="gridSharedWith_RowCommand">
                                        <Columns>
                                            <asp:BoundField DataField="SharedDocumentID" HeaderText="Shared Document ID" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="OriginalDocumentID" HeaderText="Original Document ID"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="RuleStartDate" HeaderText="RuleStartDate" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="RuleEndDate" HeaderText="RuleEndDate" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ProviderRole" HeaderText="ProviderRole" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="PurposeforUse" HeaderText="PurposeforUse" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Provider" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkSharedWith" runat="server" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="1%" ItemStyle-Width="1%"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnRemoveShare" runat="server" ToolTip="Remove Sharing" CommandName="Delete"
                                                        ImageUrl="~/images/remove_share.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="80%" align="right" valign="top">
                        <table width="100%" cellspacing="0" cellpadding="2">
                            <tr>
                                <td class="text" align="right" style="width: 22%">
                                    Document Type:
                                </td>
                                <td align="left" style="width: 28%">
                                    <asp:Label ID="lblDocumentType" runat="server" CssClass="Bold_text" Text="Patient Summary"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="text" align="right">
                                    Document ID:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDocumentId" runat="server" Enabled="false" CssClass="text" Width="160px"></asp:TextBox>
                                </td>
                                <td class="text" align="right">
                                    Document Title:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDocumentTitle" runat="server" CssClass="text" Width="160px"></asp:TextBox>
                                </td>
                            </tr>
                            <%--pt--%>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td align="left" colspan="4" class="text" valign="top">
                                    <asp:RadioButtonList ID="rbtlst" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdbrolecheckedChanged"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Text="Provider Email"></asp:ListItem>
                                        <asp:ListItem Text="Provider Role"></asp:ListItem>
                                    </asp:RadioButtonList>
                            </tr>
                            <%--addition ends--%>
                            <tr id="trProvideEmail" style="height: 30" runat="server">
                                <td class="text" align="right" width="140px" valign="top">
                                    Provider Email:
                                </td>
                                <td align="left" colspan="3" class="text">
                                    <asp:TextBox ID="txtProviderEmail" runat="server" CssClass="text" Width="165px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationGroup="ValidateBlank"
                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtProviderEmail"
                                        ErrorMessage="Invalid Email"></asp:RegularExpressionValidator>
                                    <br>
                                    <asp:RequiredFieldValidator ID="ValidateProviderEmail" runat="server" ValidationGroup="ValidateBlank"
                                        SetFocusOnError="True" ErrorMessage="Enter provider email" ControlToValidate="txtProviderEmail"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="3">
                                </td>
                            </tr>
                            <tr id="trProvideRole" style="height: 30" runat="server">
                                <td class="text" align="right" valign="top">
                                    Provider Role:
                                </td>
                                <td align="left" colspan="3" class="text">
                                    <asp:DropDownList ID="ddlProviderRole" runat="server" CssClass="text">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:RequiredFieldValidator ID="ValidateProviderrole" runat="server" ValidationGroup="ValidateBlank"
                                        SetFocusOnError="True" ErrorMessage="Please select provider role" ControlToValidate="ddlProviderRole"
                                        InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                           
                            <tr>
                                <td class="text" align="right" valign="top">
                                    Rule Start Date:
                                </td>
                                <td valign="top" align="left" class="text">
                                    <asp:TextBox ID="txtRuleStartDate" runat="server" CssClass="text" Width="132px"></asp:TextBox>
                                    <asp:Image ID="imgCalRuleStartDate" runat="server" ToolTip="Click to select date"
                                        ImageUrl="~/images/calenderIcon.gif" ImageAlign="Middle" />
                                    <br />
                                    <asp:RequiredFieldValidator ID="validatetxtRuleStartDate" runat="server" ValidationGroup="ValidateBlank"
                                        SetFocusOnError="True" ErrorMessage="Enter rule start date" ControlToValidate="txtRuleStartDate"></asp:RequiredFieldValidator>
                                    <ajax:MaskedEditExtender ID="mEERuleStartDate" runat="server" AcceptNegative="Left"
                                        DisplayMoney="Left" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" TargetControlID="txtRuleStartDate"
                                        CultureName="en-US" AutoComplete="false" AutoCompleteValue="05/23/1964">
                                    </ajax:MaskedEditExtender>
                                    <ajax:MaskedEditValidator ID="mEVRuleStartDate" runat="server" ControlExtender="mEERuleStartDate"
                                        ControlToValidate="txtRuleStartDate" Display="None" IsValidEmpty="False" MaximumValue="01/01/2210"
                                        EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid" MaximumValueMessage="Message Max"
                                        MinimumValueMessage="Message Min" TooltipMessage="Input a Date" MinimumValue="01/01/1900"
                                        ValidationGroup="MailInRebateVG">
                                    </ajax:MaskedEditValidator>
                                    <ajax:CalendarExtender ID="cERuleStartDate" runat="server" TargetControlID="txtRuleStartDate"
                                        PopupButtonID="imgCalRuleStartDate" Format="MM/dd/yyyy">
                                    </ajax:CalendarExtender>
                                </td>
                                <td class="text" align="right" valign="top">
                                    Rule End Date:
                                </td>
                                <td valign="top" align="left" class="text">
                                    <asp:TextBox ID="txtRuleEndDate" runat="server" CssClass="text" Width="132px"></asp:TextBox>
                                    <asp:Image ID="imgCalRuleEndDate" runat="server" ToolTip="Click to select date" ImageUrl="~/images/calenderIcon.gif"
                                        ImageAlign="Middle" />
                                    <br />
                                    <asp:RequiredFieldValidator ID="ValidatetxtRuleEndDate" runat="server" ValidationGroup="ValidateBlank"
                                        SetFocusOnError="True" ErrorMessage="Enter rule end date" ControlToValidate="txtRuleEndDate"></asp:RequiredFieldValidator>
                                    <ajax:MaskedEditExtender ID="mEERuleEndDate" runat="server" AcceptNegative="Left"
                                        DisplayMoney="Left" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" TargetControlID="txtRuleEndDate"
                                        CultureName="en-US" AutoComplete="false" AutoCompleteValue="05/23/1964">
                                    </ajax:MaskedEditExtender>
                                    <ajax:MaskedEditValidator ID="mEVRuleEndDate" runat="server" ControlExtender="mEERuleEndDate"
                                        ControlToValidate="txtRuleEndDate" Display="None" IsValidEmpty="False" MaximumValue="01/01/2210"
                                        EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid" MaximumValueMessage="Message Max"
                                        MinimumValueMessage="Message Min" TooltipMessage="Input a Date" MinimumValue="01/01/1900"
                                        ValidationGroup="MailInRebateVG">
                                    </ajax:MaskedEditValidator>
                                    <ajax:CalendarExtender ID="cERuleEndDate" runat="server" TargetControlID="txtRuleEndDate"
                                        PopupButtonID="imgCalRuleEndDate" Format="MM/dd/yyyy">
                                    </ajax:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="text" align="right" valign="top">
                                    Community:
                                </td>
                                <td align="left" class="text">
                                    <asp:DropDownList ID="ddlCommunities" runat="server" CssClass="text" Width="165px">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please select Community"
                                        ControlToValidate="ddlCommunities" ValidationGroup="ValidateBlank" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td class="text" align="right" valign="top">
                                    Purpose Of Use:
                                </td>
                                <td align="left" class="text">
                                    <asp:DropDownList ID="ddlPurposeForUse" runat="server" CssClass="text" Width="165px">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:RequiredFieldValidator ID="ValidateddlPurposeForUse" runat="server" ErrorMessage="Please select purpose of use"
                                        ControlToValidate="ddlPurposeForUse" ValidationGroup="ValidateBlank" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="text" align="right" valign="top">
                                    Available Sections:
                                </td>
                                <td class="text" align="left" valign="top">
                                    <asp:ListBox ID="lbAvalSections" runat="server" Width="165px" Height="100px" SelectionMode="Multiple">
                                    </asp:ListBox>
                                    <br />
                                    <br />
                                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" TextAlign="Right"
                                        AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" Visible="false" />
                                    <asp:Label ID="Label2" runat="server" Text="Move Right" TextAlign="Right" />
                                    &nbsp;&nbsp;
                                    <asp:ImageButton ID="imgbtnMoveRight" ImageAlign="AbsMiddle" runat="server" AlternateText="Move Right"
                                        ImageUrl="~/images/btn_forward.gif" OnClick="imgbtnMoveRight_Click" />
                                </td>
                                <td class="text" align="right" valign="top">
                                    Shared Sections:
                                </td>
                                <td class="text" align="left" valign="top">
                                    <asp:ListBox ID="lbSelectedsections" runat="server" Width="165px" Height="100px"
                                        BackColor="#99EE99" SelectionMode="Multiple"></asp:ListBox>
                                    <br />
                                    <br />
                                    <asp:ImageButton ID="imgbtnMoveLeft" runat="server" AlternateText="Move Left" ImageAlign="AbsMiddle"
                                        ImageUrl="~/images/btn_backward.gif" OnClick="imgbtnMoveLeft_Click" />
                                    &nbsp;&nbsp;
                                    <asp:CheckBox ID="chkRemoveAll" runat="server" Text="Select All" TextAlign="Right"
                                        AutoPostBack="true" Visible="false" OnCheckedChanged="chkRemoveAll_CheckedChanged" />
                                    <asp:Label ID="Label1" runat="server" Text="Move Left" TextAlign="Right" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <script type="text/javascript">

                var updatePannelObject = Sys.WebForms.PageRequestManager.getInstance();
                updatePannelObject.add_pageLoaded(initializeRequest);
                function initializeRequest(sender, args) {
                    //add code for dropdown with multiple select 
                    $('#' + '<%=lbAvalSections.ClientID%>').dropdownchecklist({ icon: {}, width: 170, emptyText: "Select", firstItemChecksAll: true });
                    $('#' + '<%=lbSelectedsections.ClientID%>').dropdownchecklist({ icon: {}, width: 170, emptyText: "Select", firstItemChecksAll: true });
                }

            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%" border="0" cellspacing="0" cellpadding="2">
        <tr>
            <td class="text" align="left" valign="top">
            </td>
            <td valign="top">
                <table>
                    <tr>
                        <td class="text" align="left" valign="baseline" width="145px">
                        </td>
                        <td align="right">
                        </td>
                    </tr>
                </table>
            </td>
            <td class="text" align="left" valign="top">
            </td>
            <td>
                <table>
                    <tr>
                        <td class="text" align="left" valign="top" width="75px">
                        </td>
                        <td align="right" class="text">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
            <td align="right">
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" colspan="4">
                <asp:Button ID="btnShare" runat="server" CssClass="Button" Text="Share" OnClick="btnShare_Click"
                    CausesValidation="True" Width="90" ValidationGroup="ValidateBlank" />
                <asp:Button ID="btnReset" runat="server" CssClass="Button" Text="Reset" OnClick="btnReset_Click"
                    Width="90" />
                <asp:Button ID="Button1" runat="server" CssClass="Button" Text="Back" OnClick="btnBack_Click"
                    Width="90" />
            </td>
        </tr>
    </table>
</asp:Content>
