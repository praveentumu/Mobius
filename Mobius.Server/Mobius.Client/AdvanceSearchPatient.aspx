<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdvanceSearchPatient.aspx.cs"
    Inherits="SearchUser" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>
<asp:Content ID="ContentHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        var PrevRdbRow = 0;
        function GetSelected(obj, RowIndex) {
            document.getElementById('<%=radio1.ClientID%>').checked = true;
            document.getElementById('<%=hdnSelected.ClientID%>').value = 'Y';
            document.getElementById('<%=hdnRowIndex.ClientID%>').value = RowIndex;
            if (PrevRdbRow != 0 && PrevRdbRow != obj) {
                if (document.getElementById(PrevRdbRow) != null) {
                    document.getElementById(PrevRdbRow).checked = false;
                }
            }
            PrevRdbRow = obj;

            return true;
        }
        function ShowSearhIcon() {
            if ((document.getElementById('<%=txtFirstName.ClientID%>').value != "") && (document.getElementById('<%=txtLastName.ClientID%>').value != "") && (document.getElementById('<%=txtDOB.ClientID%>').value != "") && (document.getElementById('<%=lbCommunities.ClientID%>').value != "")) {
                document.getElementById('<%=progress_image.ClientID%>').style.visibility = "visible";
                setTimeout('document.images["<%=progress_image.ClientID%>"].src = "images/Searching.gif"', 1200000);
                return true;
            }
        }

        function HideSearchIcon() {
            document.getElementById('<%=progress_image.ClientID%>').style.visibility = "hidden";
            return true;

        }

        var dvTelephoneLastStateOpen = false;
        var dvMotherMaidenNameStateOpen = false;
        var dvAddressStateOpen = false;
        var dvBirthPlaceAddressStateOpen = false;

        function setDivLastState(controlName, state) {
            if (controlName == 'dvTelephone')
                dvTelephoneLastStateOpen = state;
            if (controlName == 'dvMotherMaidenName')
                dvMotherMaidenNameStateOpen = state;
            if (controlName == 'dvAddress')
                dvAddressStateOpen = state;
            if (controlName == 'dvBirthPlaceAddress')
                dvBirthPlaceAddressStateOpen = state;
        }

        function RestoreDivLastState() {
            if (dvTelephoneLastStateOpen)
                showHideMenu('dvTelephone');
            if (dvMotherMaidenNameStateOpen)
                showHideMenu('dvMotherMaidenName');
            if (dvAddressStateOpen)
                showHideMenu('dvAddress');
            if (dvBirthPlaceAddressStateOpen)
                showHideMenu('dvBirthPlaceAddress');
        }

        function showHideMenu(controlName) {
            if ($("#" + controlName).is(':visible')) {
                $("#" + controlName).fadeToggle(2000);
                setDivLastState(controlName, false);
            }
            else {
                $("#" + controlName).slideToggle();
                setDivLastState(controlName, true);
            }
        }

       
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <table style="width: 100%" border="0">
                <tr>
                    <td align="left" class="Bold_text" colspan="4">
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="SeachFilterRowHead" style="width: 100%;" colspan="4">
                        &nbsp;Patient Demographic Filter
                    </td>
                </tr>
                <tr>
                    <td class="text" align="left" valign="top" colspan="4" style="width: 100%;">
                        <div style="width: 100%;" id="Div1">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                                <tr>
                                    <td class="text" align="left">
                                        &nbsp;Prefix:
                                    </td>
                                    <td class="text" align="left">
                                        <font color="red">*</font>Given Name
                                    </td>
                                    <td class="text" align="left">
                                        &nbsp;Middle Name
                                    </td>
                                    <td class="text" align="left">
                                        <font color="red">*</font>Family Name
                                    </td>
                                    <td class="text" align="left">
                                        &nbsp;Suffix
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="left" valign="top">
                                        <asp:TextBox ID="txtPrefix" runat="server" CssClass="text"></asp:TextBox>
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        <asp:TextBox ID="txtFirstName" runat="server" ValidationGroup="SearchGroup" CssClass="text"></asp:TextBox>
                                        <br />
                                        <asp:RequiredFieldValidator ID="ValidateFirstName" runat="server" ValidationGroup="SearchGroup"
                                            SetFocusOnError="True" ErrorMessage="Enter first name" ControlToValidate="txtFirstName"
                                            CssClass="text"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        <asp:TextBox ID="txtMiddleName" runat="server" CssClass="text"></asp:TextBox>
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        <asp:TextBox ID="txtLastName" runat="server" ValidationGroup="SearchGroup" CssClass="text"></asp:TextBox>
                                        <br />
                                        <asp:RequiredFieldValidator ID="ValidateLastName" runat="server" ValidationGroup="SearchGroup"
                                            SetFocusOnError="True" ErrorMessage="Enter last name" ControlToValidate="txtLastName"
                                            CssClass="text"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        <asp:TextBox ID="txtSuffix" runat="server" ValidationGroup="SearchGroup" CssClass="text"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="text" align="left">
                        &nbsp;Patient ID:
                    </td>
                    <td class="text" align="left">
                        <font color="red">*</font>Date of Birth
                    </td>
                    <td class="text" align="left">
                        Deceased Date
                    </td>
                    <td class="text" align="left">
                        <font color="red">*</font>Sex
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtPatientId" runat="server" CssClass="text"></asp:TextBox>
                    </td>
                    <td align="left" valign="top" style="padding-left: -10;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="txtDOB" runat="server" ValidationGroup="SearchGroup" CssClass="text"></asp:TextBox>
                                    <br />
                                    <asp:RequiredFieldValidator ID="ValidatetxtDOB" runat="server" ValidationGroup="SearchGroup"
                                        SetFocusOnError="True" ErrorMessage="Enter Date of Birth" ControlToValidate="txtDOB"
                                        CssClass="text"></asp:RequiredFieldValidator>
                                </td>
                                <td valign="baseline">
                                    <asp:Image ID="imgcalFrm" runat="server" ToolTip="Click to select date" ImageUrl="~/images/calenderIcon.gif"
                                        ImageAlign="Baseline" />
                                </td>
                                <td>
                                    <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptNegative="Left"
                                        DisplayMoney="Left" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" TargetControlID="txtDOB"
                                        CultureName="en-US" AutoComplete="false" AutoCompleteValue="05/23/1964">
                                    </ajax:MaskedEditExtender>
                                    <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                        ControlToValidate="txtDOB" Display="None" IsValidEmpty="False" MaximumValue="01/01/2210"
                                        EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid" MaximumValueMessage="Message Max"
                                        MinimumValueMessage="Message Min" TooltipMessage="Input a Date" MinimumValue="01/01/1900"
                                        ValidationGroup="MailInRebateVG">
                                    </ajax:MaskedEditValidator>
                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDOB"
                                        PopupButtonID="imgcalFrm" Format="MM/dd/yyyy">
                                    </ajax:CalendarExtender>
                                    <%--   <asp:RequiredFieldValidator id="RFVDOB" runat="server" ValidationGroup="SaveGroup" SetFocusOnError="True" ErrorMessage="Enter Information" ControlToValidate="txtDOB"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="left" valign="top" style="padding-left: -10;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="txtDeceasedDate" runat="server" CssClass="text"></asp:TextBox>
                                </td>
                                <td valign="baseline">
                                    <asp:Image ID="imgcalFrmDeceasedDate" runat="server" ToolTip="Click to select date"
                                        ImageUrl="~/images/calenderIcon.gif" ImageAlign="Baseline" />
                                </td>
                                <td>
                                    <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AcceptNegative="Left"
                                        DisplayMoney="Left" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true"
                                        OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" TargetControlID="txtDeceasedDate"
                                        CultureName="en-US" AutoComplete="false" AutoCompleteValue="05/23/1964">
                                    </ajax:MaskedEditExtender>
                                    <ajax:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender1"
                                        ControlToValidate="txtDeceasedDate" Display="None" IsValidEmpty="False" MaximumValue="01/01/2210"
                                        EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid" MaximumValueMessage="Message Max"
                                        MinimumValueMessage="Message Min" TooltipMessage="Input a Date" MinimumValue="01/01/1900"
                                        ValidationGroup="MailInRebateVG">
                                    </ajax:MaskedEditValidator>
                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDeceasedDate"
                                        PopupButtonID="imgcalFrmDeceasedDate" Format="MM/dd/yyyy">
                                    </ajax:CalendarExtender>
                                    <%--   <asp:RequiredFieldValidator id="RFVDOB" runat="server" ValidationGroup="SaveGroup" SetFocusOnError="True" ErrorMessage="Enter Information" ControlToValidate="txtDOB"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 40%" class="Bold_text" align="left" valign="top">
                        <asp:RadioButtonList ID="radio1" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem id="option1" Selected="true" runat="server" Value="Male" />
                            <asp:ListItem id="option2" runat="server" Value="Female" />
                            <asp:ListItem id="option3" runat="server" Value="UnSpecified" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td class="text" align="left" style="width: 20%" valign="top">
                        <font color="red">*</font> Select Community :
                    </td>
                    <td align="left" valign="top">
                        <asp:ListBox ID="lbCommunities" runat="server" Width="165px" Height="70px" SelectionMode="Multiple"
                            Enabled="true" ValidationGroup="SearchGroup" AutoPostBack="false" AppendDataBoundItems="true"
                            EnableViewState="true" CssClass="text"></asp:ListBox>
                        <br />
                        <asp:RequiredFieldValidator ID="lbCommunitiesdValidator" runat="server" ErrorMessage="Select Community/Communities."
                            ValidationGroup="SearchGroup" SetFocusOnError="True" ControlToValidate="lbCommunities"
                            CssClass="text"></asp:RequiredFieldValidator>
                    </td>
                    <td class="text" align="left" valign="top">
                        &nbsp;SSN :
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtSSN" runat="server" CssClass="text"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('dvMotherMaidenName');">
                        &nbsp;Mother`s Maiden Name Filter
                    </td>
                </tr>
                <tr>
                    <td class="text" align="left" valign="top" colspan="4" style="width: 100%;">
                        <div style="width: 100%; display: none;" id="dvMotherMaidenName">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                                <tr>
                                    <td class="text" align="left">
                                        &nbsp;Prefix:
                                    </td>
                                    <td class="text" align="left">
                                        &nbsp;Given Name
                                    </td>
                                    <td class="text" align="left">
                                        &nbsp;Middle Name
                                    </td>
                                    <td class="text" align="left">
                                        &nbsp;Family Name
                                    </td>
                                    <td class="text" align="left">
                                        &nbsp;Suffix
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="left">
                                        <asp:TextBox ID="txtMotherPrefix" runat="server" CssClass="text"></asp:TextBox>
                                    </td>
                                    <td class="text" align="left">
                                        <asp:TextBox ID="txtMotherGivenName" runat="server" CssClass="text"></asp:TextBox>
                                    </td>
                                    <td class="text" align="left">
                                        <asp:TextBox ID="txtMotherMiddleName" runat="server" CssClass="text"></asp:TextBox>
                                    </td>
                                    <td class="text" align="left">
                                        <asp:TextBox ID="txtMotherFamilyName" runat="server" CssClass="text"></asp:TextBox>
                                    </td>
                                    <td class="text" align="left">
                                        <asp:TextBox ID="txtMotherSuffix" runat="server" CssClass="text"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('dvTelephone');">
                        &nbsp;Telephone Filter
                    </td>
                </tr>
                <td class="text" align="left" valign="top" colspan="4" style="width: 100%;">
                    <div style="width: 100%; display: none;" id="dvTelephone">
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                            <tr>
                                <td class="text" align="left" valign="top" style="width: 20%">
                                    &nbsp;Telephone Number(s):
                                </td>
                                <td align="left" valign="top" colspan="3" class="text">
                                    <asp:TextBox ID="txtPhoneNumbers" runat="server" CssClass="text" Width="300px" Height="40px"
                                        TextMode="MultiLine" ToolTip="Enter multiple values in separate line e.g. #-###-####"></asp:TextBox>
                                </td>
                        </table>
                    </div>
                </td>
                </tr>
                <tr>
                    <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('dvAddress');">
                        &nbsp;Address Filter
                    </td>
                </tr>
                <tr>
                    <td class="text" align="left" valign="top" colspan="4" style="width: 100%;">
                        <div style="width: 100%; display: none;" id="dvAddress">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                                <tr>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Street :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtStreetAddress" Height="40px" Width="300px" CssClass="text"
                                            TextMode="MultiLine" ToolTip="Enter multiple values in separate line" />
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;City :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtCity" CssClass="text" Height="40px" Width="300px"
                                            TextMode="MultiLine" ToolTip="Enter multiple values in separate line" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;State :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtState" CssClass="text" Height="40px" Width="300px"
                                            TextMode="MultiLine" ToolTip="Enter multiple values in separate line" />
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Zip :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtZip" CssClass="text" Height="40px" Width="300px"
                                            TextMode="MultiLine" ToolTip="Enter multiple values in separate line" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Country :
                                    </td>
                                    <td align="left" valign="top" colspan="3">
                                        <asp:TextBox runat="server" ID="txtCountry" CssClass="text" Height="40px" Width="300px"
                                            TextMode="MultiLine"  ToolTip="Enter multiple values in separate line" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('dvBirthPlaceAddress');">
                        &nbsp;Birth Place Address Filter
                    </td>
                </tr>
                <tr>
                    <td class="text" align="left" valign="top" colspan="4" style="width: 100%;">
                        <div style="width: 100%; display: none;" id="dvBirthPlaceAddress">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                                <tr>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Birth Place Street :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtBirthPlaceStreet" CssClass="text" />
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Birth Place City :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtBirthPlaceCity" CssClass="text" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Birth Place State :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtBirthPlaceState" CssClass="text" />
                                    </td>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Birth Place Zip :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtBirthPlaceZip" CssClass="text" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text" align="left" valign="top">
                                        &nbsp;Birth Place Country :
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox runat="server" ID="txtBirthPlaceCountry" CssClass="text" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <br />
                        <asp:Button ID="btnSearch" runat="server" CssClass="Button" Text="Search" OnClientClick="ShowSearhIcon()"
                            OnClick="btnSearch_Click" ValidationGroup="SearchGroup" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="Button" Text="Cancel" OnClientClick="HideSearchIcon()"
                            OnClick="CancelSearch" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <img id="progress_image" alt="" runat="server" style="visibility: hidden;" src="images/Searching.gif" />
                    </td>
                </tr>
                <tr id="trNextTop" runat="server">
                    <td align="right" colspan="4">
                        <asp:LinkButton ID="lbtnNextTop" runat="server" OnClick="lbtnNext_Click" Text="Next"
                            CssClass="bluetext_heading"></asp:LinkButton>
                        <asp:ImageButton ID="ibtnNextTop" runat="server" OnClick="lbtnNext_Click" ImageAlign="Right"
                            ImageUrl="images/next_arrow.gif" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gridPatients" Width="100%" runat="server" AutoGenerateColumns="false"
                            CssClass="grid" DataKeyNames="LocalMPIID" AlternatingRowStyle-CssClass="alternateRowColor"
                            OnRowDataBound="gridPatients_RowDataBound" AllowPaging="true" PageSize="5" AllowSorting="true"
                            PagerSettings-Mode="NumericFirstLast" PagerStyle-HorizontalAlign="Right" OnPageIndexChanging="gridPatients_PageIndexChanging">
                            <Columns>
                                <%--<asp:BoundField DataField="ID" HeaderText="ID"  HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" />--%>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="2%" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="gridth">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rdSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="GivenName" HeaderText="First Name" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="FamilyName" HeaderText="Last Name" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DOB" HeaderText="Date of Birth" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Gender" HeaderText="Gender" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CommunityID" HeaderText="Community" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
                <tr id="trNext" runat="server">
                    <td align="right" colspan="4">
                        <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click" Text="Next"
                            CssClass="bluetext_heading"></asp:LinkButton>
                        <asp:ImageButton ID="ibtnNext" runat="server" OnClick="lbtnNext_Click" ImageAlign="Right"
                            ImageUrl="images/next_arrow.gif" />
                        <asp:HiddenField ID="hdnSelected" runat="server" Value="N" />
                        <asp:HiddenField ID="hdnRowIndex" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
            <script type="text/javascript">

                var updatePannelObject = Sys.WebForms.PageRequestManager.getInstance();
                updatePannelObject.add_pageLoaded(initializeRequest);
                updatePannelObject.add_endRequest(EndRequestHandler);

                function initializeRequest(sender, args) {
                    //add code for dropdown with multiple select 
                    $('#' + '<%=lbCommunities.ClientID%>').dropdownchecklist({ icon: {}, width: 200, emptyText: "Select", firstItemChecksAll: true });

                }
                function EndRequestHandler(sender, args) {
                    RestoreDivLastState();
                }

            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
