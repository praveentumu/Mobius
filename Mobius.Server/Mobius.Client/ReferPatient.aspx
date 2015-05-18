<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReferPatient.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="ReferPatient" ValidateRequest="false" EnableEventValidation="false" %>

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

        function CheckChange() {
            var rbvalue = $("#<%=rdReferralButton.ClientID%>").find(":checked").val()
            var control = $('#trReferralOutcomeDocument');
            var chkreschdule = $('#' + '<%=chkreschdule.ClientID%>')
            if (rbvalue == 1 && !chkreschdule.attr('checked')) {
                control.show();
            }
            else {
                control.hide();
            }
        }

        function controlVisibily() {
            var control = $('#' + '<%=trReferralCompleted.ClientID%>');
            var trAppointmentDate = $('#' + '<%=trAppointmentDate.ClientID%>');
            var trHideAppointmentDate = $('#' + '<%=trHideAppointmentDate.ClientID%>');
            var chkreschdule = $('#' + '<%=chkreschdule.ClientID%>')
            var txtReadOnlyAppointmentDate = $('#' + '<%=txtReadOnlyAppointmentDate.ClientID %>');


            if (txtReadOnlyAppointmentDate.val() != "") {

                if (chkreschdule.attr('checked')) {
                    control.fadeToggle(0, "swing");
                    trAppointmentDate.show(1600)
                    trHideAppointmentDate.fadeToggle(0, "swing");
                    var rbvalue = $("#<%=rdReferralButton.ClientID%>").find(":checked").val();
                    if (rbvalue == 1) {
                        $('#option1 input:first').attr('checked', 'checked');
                    }
                }
                else {
                    control.show(1600);
                    trAppointmentDate.hide();
                    trHideAppointmentDate.show(1600)
                }

            }
            CheckChange();
            return false;
        }


        $(document).ready(function () {
            controlVisibily();
            $("#<%=rdReferralButton.ClientID%>").change(function (e) {
                CheckChange();

            });

            $("#<%=chkreschdule.ClientID%>").click(function (e) {
                controlVisibily();

            });

        });   

    </script>
    <asp:UpdatePanel ID="updReferPatientNextMain" runat="server">
        <contenttemplate>
            <div id="DivRequester" runat="server">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left">
                            <asp:HiddenField ID="hdnUrl" runat="server" />
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
                                                ID="lblOriginaldocumentID" OnClick="lnkViewOutComeDocument_Click" CommandArgument="" />
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
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td colspan="2" align="left" class="Bold_text">
                            <asp:HiddenField ID="hdnForImage" runat="server" />
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
                                        <div class="text" style="padding: 1px">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left">
                                                        Name:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        Gender:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblgender" runat="server" CssClass="Bold_text"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
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
                                    <td bgcolor="#9DDFFF" align="left" valign="middle" width="17%">
                                        <asp:HiddenField ID="hdnOriginalDocumentID" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnLocation" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnDocumentId" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnSharedDocumentID" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="80%" align="right" valign="middle" style="padding-left: 0px">
                            <table cellspacing="2" cellpadding="2" border="0" width="100%">
                                <tr id="trProvideEmail" runat="server">
                                    <td class="text" align="right" valign="top">
                                        Referred Provider Email:
                                    </td>
                                    <td align="left" class="text" colspan="3">
                                        <asp:TextBox ID="txtProviderEmail" runat="server" CssClass="text" Width="230px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtProviderEmail"
                                            ErrorMessage="" Style="z-index: 101; left: 424px; position: absolute; top: 285px"
                                            ValidationExpression="[^<>]*" ValidationGroup="ValidateBlank"></asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationGroup="ValidateBlank"
                                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtProviderEmail"
                                            ErrorMessage="Invalid Email"></asp:RegularExpressionValidator>
                                        <br>
                                        <asp:RequiredFieldValidator ID="ValidateProviderEmail" runat="server" ValidationGroup="ValidateBlank"
                                            SetFocusOnError="True" ErrorMessage="Enter provider email" ControlToValidate="txtProviderEmail"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="right" valign="top">
                                        Purpose Of Use:
                                    </td>
                                    <td align="left" class="text" colspan="3">
                                        <asp:DropDownList ID="ddlPurposeForUse" runat="server" CssClass="text" Width="165px">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:RequiredFieldValidator ID="ValidateddlPurposeForUse" runat="server" ErrorMessage="Please select purpose of use"
                                            ControlToValidate="ddlPurposeForUse" ValidationGroup="ValidateBlank" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr valign="middle">
                                    <td class="text" align="right" valign="top">
                                        Referral Accomplishment Date:
                                    </td>
                                    <td valign="top" align="left" class="text">
                                        <asp:TextBox ID="txtRuleStartDate" runat="server" CssClass="text" Width="132px"></asp:TextBox>
                                        <asp:Image ID="imgCalRuleStartDate" runat="server" ToolTip="Click to select date"
                                            ImageUrl="~/images/calenderIcon.gif" ImageAlign="Middle" />
                                        <br />
                                        <asp:RequiredFieldValidator ID="validatetxtRuleStartDate" runat="server" ValidationGroup="ValidateBlank"
                                            SetFocusOnError="True" ErrorMessage="Enter accomplishment date" ControlToValidate="txtRuleStartDate"></asp:RequiredFieldValidator>
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
                                </tr>
                                <tr id="trSections" runat="server">
                                    <td class="text" align="right" valign="top">
                                        Available Sections:
                                    </td>
                                    <td class="text" align="left" valign="middle">
                                        <asp:ListBox ID="lbAvalSections" runat="server" Width="165px" Height="100px" SelectionMode="Multiple">
                                        </asp:ListBox>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblMove" runat="server" Text="Move Right" TextAlign="Right" />
                                        <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" TextAlign="Right"
                                            AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" Visible=false />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="imgbtnMoveRight" ImageAlign="AbsMiddle" runat="server" AlternateText="Move Right"
                                            ImageUrl="~/images/btn_forward.gif" OnClick="imgbtnMoveRight_Click" />
                                    </td>
                                    <td class="text" align="right" valign="top">
                                        Shared Sections:
                                    </td>
                                    <td class="text" align="left" valign="middle">
                                        <asp:ListBox ID="lbSelectedsections" runat="server" Width="165px" Height="100px"
                                            BackColor="#99EE99" SelectionMode="Multiple"></asp:ListBox>
                                        <br />
                                        <br />
                                        <asp:ImageButton ID="imgbtnMoveLeft" runat="server" AlternateText="Move Left" ImageAlign="AbsMiddle"
                                            ImageUrl="~/images/btn_backward.gif" OnClick="imgbtnMoveLeft_Click" />
                                        &nbsp;&nbsp;
                                         <asp:Label ID="Label1" runat="server" Text="Move Left" TextAlign="Right" />
                                        <asp:CheckBox ID="chkRemoveAll" runat="server" Text="Select All" TextAlign="Right"
                                            AutoPostBack="true" OnCheckedChanged="chkRemoveAll_CheckedChanged" Visible=false />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="right" valign="top">
                                        Referral Requestor's comments:
                                    </td>
                                    <td class="text" align="left" valign="middle" colspan="3">
                                        <asp:TextBox ID="lstcomment" runat="server" Width="400px" Height="40px" TextMode="MultiLine">
                                        </asp:TextBox>
                                        <br />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="lstcomment"
                                            ErrorMessage="Cannot contain special characters." ValidationExpression="[^<>]*"
                                            ValidationGroup="ValidateBlank"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr id="trAppointmentDate" runat="server">
                                    <td class="text" align="right">
                                        Patient Appointment Date:
                                    </td>
                                    <td valign="top" align="left" class="text">
                                        <asp:TextBox ID="txtRuleEndDate" runat="server" CssClass="text" Width="132px"></asp:TextBox>
                                        <asp:Image ID="imgCalRuleEndDate" runat="server" ToolTip="Click to select date" ImageUrl="~/images/calenderIcon.gif"
                                            ImageAlign="Middle" />
                                        <br />
                                        <%--<asp:RequiredFieldValidator ID="ValidatetxtRuleEndDate" runat="server" ValidationGroup="ValidateBlank"
                                            SetFocusOnError="True" ErrorMessage="Enter rule end date" ControlToValidate="txtRuleEndDate"></asp:RequiredFieldValidator>--%>
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
                                <tr id="trHideAppointmentDate" runat="server" style="display: none">
                                    <td class="text" align="right">
                                        Patient Appointment Date:
                                    </td>
                                    <td valign="top" align="left" class="text">
                                        <asp:TextBox ID="txtReadOnlyAppointmentDate" runat="server" CssClass="text" Width="132px"
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trdispatcherscomments" runat="server">
                                    <td class="text" align="right" valign="middle">
                                        Referral dispatcher's comments:
                                    </td>
                                    <td class="text" align="left" valign="middle">
                                        <asp:TextBox ID="txtdispatcherComment" runat="server" Width="400px" Height="40px"
                                            TextMode="MultiLine">                                                      
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressiondispatcherComment" runat="server"
                                            ControlToValidate="txtdispatcherComment" ErrorMessage="Cannot contain special characters."
                                            ValidationExpression="[^<>]*" ValidationGroup="ValidateBlank"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr id="trReferralAcknowledgement" runat="server" align="right">
                                    <td class="text" align="right" valign="middle">
                                        Referral Acknowledgement:
                                    </td>
                                    <td align="left" class="text">
                                        <asp:RadioButtonList ID="rdStatus" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem id="rdStatuso1" Selected="true" runat="server" Value="Accept" />
                                            <asp:ListItem id="rdStatuso2" runat="server" Value="Decline" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                              <tr id="trOutComeDocument" runat="server" visible="false">
                                    <td class="text" align="right">
                                    </td>
                                    <td valign="top" align="left" class="text">
                                        <asp:LinkButton Text="Click here to view outcome document (C32 summary report)" CssClass="linkOutcomeDoc" runat="server" ID="lnkViewOutComeDocument"
                                            OnClick="lnkViewOutComeDocument_Click" CommandArgument="OutComeDocument" />
                                    </td>
                                </tr>
                                <tr id="trAcknowledgement" runat="server" visible="false">
                                    <td colspan="4" style="width: 100%">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                        </table>
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
            </div>
            <script language="javascript" type="text/javascript">

                var updatePannelObject = Sys.WebForms.PageRequestManager.getInstance();
                updatePannelObject.add_pageLoaded(initializeRequest);
                function initializeRequest(sender, args) {
                    //add code for dropdown with multiple select 
                    $('#' + '<%=lbAvalSections.ClientID%>').dropdownchecklist({ icon: {}, width: 170, emptyText: "Select", firstItemChecksAll: true });
                    $('#' + '<%=lbSelectedsections.ClientID%>').dropdownchecklist({ icon: {}, width: 170, emptyText: "Select", firstItemChecksAll: true });
                    controlVisibily();
                }

            </script>
        </contenttemplate>
    </asp:UpdatePanel>
    <table width="100%" border="0" cellspacing="0" cellpadding="2">
        <tr id="trReschedule" runat="server" visible="false">
            <td class="text" align="right" valign="middle" width="45%">
                Re-schedule :
            </td>
            <td align="left" class="text" width="40%">
                <asp:CheckBox ID="chkreschdule" runat="server" Text="Yes" />
            </td>
        </tr>
        <tr id="trReferralCompleted" runat="server">
            <td class="text" align="right" valign="middle" width="45%">
                Referral completed:
            </td>
            <td align="left" class="text" width="40%">
                <asp:RadioButtonList ID="rdReferralButton" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem id="option1" Selected="true" runat="server" Value="0" Text="Not completed" />
                    <asp:ListItem id="option2" runat="server" Value="1" Text="completed" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr id="trReferralOutcomeDocument" style="display: none" width="45%">
            <td class="text" align="right" valign="middle">
                &nbsp; Referral Outcome Document:
            </td>
            <td align="left" class="text" width="40%">
                <asp:FileUpload ID="FileUpload" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right" colspan="4">
                <asp:Button ID="btnShare" runat="server" CssClass="Button" Text="Send Referral" OnClick="btnReferral_Click"
                    CausesValidation="True" Width="90" ValidationGroup="ValidateBlank" />
                <asp:Button ID="btnAcknowledge" runat="server" CssClass="Button" Text="Acknowledge"
                    Visible="false" OnClick="btnAcknowledge_Click" ValidationGroup="ValidateBlank"
                    CausesValidation="True" Width="90" />
                <asp:Button ID="btnReset" runat="server" CssClass="Button" Text="Reset" OnClick="btnReset_Click"
                    Width="90" />
                <asp:Button ID="btnBack" runat="server" CssClass="Button" Text="Back" OnClick="btnBack_Click"
                    Width="90" />
            </td>
        </tr>
        <%-- <tr>
                        <td>
                            <asp:FileUpload ID="FileUpload" runat="server" Visible="false" />
                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Submit" />
                        </td>
                    </tr>--%>
    </table>
</asp:Content>
