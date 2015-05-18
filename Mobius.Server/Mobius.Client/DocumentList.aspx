<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="DocumentList.aspx.cs" Inherits="DocumentList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        window.name = "DocumentList";
        function OpenUploadWindow(PageUrl, height, width) {
            var centerWidth = (window.screen.width - width) / 2;
            var centerHeight = (window.screen.height - height) / 2;
            window.open(PageUrl, 'UploadFile', 'resizable=0,width=' + width + ',height=' + height + ',left=' + centerWidth + ',top=' + centerHeight);
            return false;
        }

        function ShowSearhIcon() {
            if (document.getElementById('<%=lbCommunities.ClientID%>').value != "") {
                document.getElementById('<%=progress_image.ClientID%>').style.visibility = "visible";
                setTimeout('document.images["<%=progress_image.ClientID%>"].src = "images/Searching.gif"', 1200000);
                return true;
            }
        }

        function HideSearchIcon() {
            document.getElementById('<%=progress_image.ClientID%>').style.visibility = "hidden";
            return true;
        }


    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellspacing="2" cellpadding="0">
                <tr>
                    <td colspan="2" align="left" class="Bold_text">
                        <asp:HiddenField ID="hdnForImage" runat="server" />
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <img src="images/upload.png" width="22" height="21" />
                        <a href="#" onclick="OpenUploadWindow('UploadPatientRecord.aspx',175,500);" class="text_link">
                            Upload New Document</a>
                    </td>
                </tr>
                <tr>
                    <td align="left">
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
                                        Patient:</div>
                                    <div class="text" style="padding: 5px">
                                        <asp:Label ID="lblPatientName" runat="server" Font-Bold="false"></asp:Label></div>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#9DDFFF" align="left" valign="top" width="17%">
                                    <asp:HiddenField ID="hdnOriginalDocumentID" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnSharedDocumentID" runat="server" Value="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="top" class="text">
                        Select Community :
                        <asp:ListBox ID="lbCommunities" runat="server" Width="200px" Height="70px" SelectionMode="Multiple"
                            Enabled="true"></asp:ListBox>
                        <br />
                         <asp:RequiredFieldValidator ID="lbCommunitiesdValidator" runat="server" ErrorMessage="Select Community/Communities."
                            SetFocusOnError="True" ControlToValidate="lbCommunities" ValidationGroup="communityValidatorGroup"></asp:RequiredFieldValidator>
                    </td>
                    <td valign="top" class="text" align="left">
                        <asp:CheckBox Text="Locally Available Documents" Checked="true" ID="chkLocalDocuments"
                            runat="server" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnretrieveDocument" runat="server" Text="Get Documents" OnClick="BtnGetDocuments_Click"
                            OnClientClick="ShowSearhIcon()" ValidationGroup="communityValidatorGroup" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <img id="progress_image" alt="" runat="server" style="visibility: hidden;" src="images/Searching.gif" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="gridDocument" Width="100%" runat="server" AutoGenerateColumns="false"
                            OnRowEditing="GridDocument_RowEditing" OnRowCommand="GridDocument_RowCommand"
                            CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor" OnRowDataBound="GridDocument_RowDataBound"
                            PageSize="10" PagerStyle-HorizontalAlign="Right" OnPageIndexChanging="GridDocument_PageIndexChanging"
                            AllowPaging="true" AllowSorting="true">
                            <Columns>
                                <%--asp:BoundField DataField="ROWID" HeaderText="Sr No." ItemStyle-HorizontalAlign="Center"HeaderStyle-Width="34px" />--%>
                                <asp:TemplateField HeaderText="Document ID" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOriginaldocumentID" runat="server" Text='<%# Bind("DocumentUniqueId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <%-- <ItemTemplate>
                                        <asp:Label ID="lblOriginaldocumentID" runat="server" Text='<%# Bind("OriginalDocumentID") %>'></asp:Label>
                                    </ItemTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document Title" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Wrap="false" HeaderStyle-CssClass="gridth" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("DocumentTitle") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Author" HeaderText="Author" ItemStyle-HorizontalAlign="Left"
                                    ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="CreatedOn" ItemStyle-Wrap="true" HeaderText="Created Date"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="74px" />
                                <asp:BoundField DataField="DataSource" HeaderText="Data Source" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" Visible="false" />
                                <asp:BoundField DataField="Community" HeaderText="Data Source" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="Community" HeaderText="Data Source" ItemStyle-HorizontalAlign="Left"
                                    ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left" Visible="true" HeaderStyle-Width="2px" />
                                <asp:TemplateField HeaderText="Policy File" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                    HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnPolicy" Visible="false" runat="server" ToolTip="Policy File"  OnClientClick="ShowSearhIcon();"
                                            ImageUrl="images/Add.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Refer" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                    HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnEdit" Visible="false" runat="server" ToolTip="Refer Patient" OnClientClick="ShowSearhIcon();"
                                            ImageUrl="images/share.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Custom Share" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                    HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnCustomEdit" Visible="false" runat="server" ToolTip="Custom Share"  OnClientClick="ShowSearhIcon();"
                                            ImageUrl="images/share.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                    HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnView" Visible="false" runat="server" OnClientClick="ShowSearhIcon();"
                                            ToolTip="To load, latest document from server, click on above checkbox." ImageUrl="images/Add.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IsShared" HeaderText="IsShared" ItemStyle-HorizontalAlign="Left"
                                    ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left" Visible="false" HeaderStyle-Width="2px" />
                                <asp:TemplateField HeaderText="Purpose" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                    HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlPurposeForView" Visible="false" runat="server" CssClass="text"
                                            Width="160px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <div class="emergencyPopup" style="background-color: #A5D3ED;">
                <table border="0" cellspacing="0" cellpadding="2" id="tblEmergency">
                    <tr>
                        <td class='msPopupTitle' style="width: 180px; height: 22px;">
                            Emergency Access
                        </td>
                        <td class='msPopupTitle' style="height: 22px; width: 490px;">
                            <img src='images/close.gif' class='msCloseButton' alt='Close' style="cursor: pointer"
                                align='right' onclick="CloseEmergencyDialoge()">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" class="text" colspan="2">
                            <br>
                            <strong>Information:</strong>
                            <br />
                            <p>
                                This option allows user to access patient's document, in case of an emergency situation,
                                as permitted for configured provider roles by administrator. All the fields below
                                are mandatory to get the access of the document.
                            </p>
                            <br />
                            <p>
                                You will be able to access this document, without re-entering the field below for
                                next
                                <asp:Label ID="lblEmergencyAccessTime" runat="server" Text="Label"></asp:Label>
                                hours.<br />
                            </p>
                            <br />
                            <p>
                                <font color="red"><b>Note:</b> </font>This action will be recorded for future audit
                                purpose. Also, an alert will be generated and sent to patient and healthcare facility
                                security officers.
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            &nbsp;
                            <asp:Label ID="lblEmergencyError" runat="server" ForeColor="Red" Font-Bold="True"
                                CssClass="text"></asp:Label>
                            <asp:HiddenField runat="server" ID="hdnPatient" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; margin-left: 15px" align="right" class="text">
                            <asp:Label ID="lblEmergencyReasons" runat="server" CssClass="text"><font color="red">*</font>Reason</asp:Label>
                        </td>
                        <td valign="top" class="text" align="left">
                            <asp:DropDownList ID="ddlEmergencyReasons" runat="server" Width="200px" CssClass="text"
                                ValidationGroup="EmergencyGroup">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="ddlEmergencyReasonsValidator" runat="server" ErrorMessage="Select Reason"
                                ValidationGroup="EmergencyGroup" SetFocusOnError="True" ControlToValidate="ddlEmergencyReasons"
                                CssClass="text" InitialValue="0"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr style="height: 5px">
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; margin-left: 15px" align="right" class="text">
                            <asp:Label ID="Label2" runat="server" CssClass="text"><font color="red">*</font>Description</asp:Label>
                        </td>
                        <td valign="top" class="text" style="width: 200px;" align="left">
                            <asp:TextBox ID="txtDescription" TextMode="multiline" Columns="1" Rows="5" runat="server"
                                CssClass="textAreaSize" ValidationGroup="EmergencyGroup" Width="350px" Style="overflow: scroll-y;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <img id="loadingImage" alt="" runat="server" style="visibility: hidden;" src="images/Searching.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style3" colspan="2" align="center">
                            <asp:Button ID="btnSubmitEmergency" runat="server" Text="Submit" OnClick="btnSubmitEmergency_Click"
                                OnClientClick="return CheckConfirmation()" class="ButtonBG" ValidationGroup="EmergencyGroup" />
                        </td>
                    </tr>
                    <%-- <tr>
                    <td col></td></tr>--%>
                </table>
            </div>
            <asp:HiddenField ID="hdnErrorMessage" runat="server" />


        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        var updatePannelObject = Sys.WebForms.PageRequestManager.getInstance();
        updatePannelObject.add_pageLoaded(initializeRequest);
        function initializeRequest(sender, args) {
            //add code for dropdown with multiple select 
            $('#' + '<%=lbCommunities.ClientID%>').dropdownchecklist({ icon: {}, width: 200, emptyText: "Select", firstItemChecksAll: true });

            if (($('#' + '<%=hdnErrorMessage.ClientID%>').val() != "")) {
                alert($('#' + '<%=hdnErrorMessage.ClientID%>').val());
                $('#' + '<%=hdnErrorMessage.ClientID%>').val("");
            }
        }

        function CheckLength(textBox, maxLength) {
            if (textBox.value.length > maxLength) {
                alert("Max characters allowed are " + maxLength);
                textBox.value = textBox.value.substr(0, maxLength);
            }
        }

        function CloseEmergencyDialoge() {
            $('.emergencyPopup').hide();
            LightenMasterPage();
            ResetEmergencyDialog();
        }

        function OpenEmergencyDialoge() {
            DarkenMasterPage();
            $('#' + '<%=lblErrorMsg.ClientID%>').text('');
            return $(".emergencyPopup").animate({ opacity: 'toggle', height: 'toggle' }, "fast");
        }

        function ResetEmergencyDialog() {
            $('#' + '<%=ddlEmergencyReasons.ClientID%>').val(0);
            $('#' + '<%=txtDescription.ClientID%>').val('');

        }

        function CheckConfirmation() {

            if ($('#' + '<%=ddlEmergencyReasons.ClientID%>').val() == 0 || $.trim($('#' + '<%=txtDescription.ClientID%>').val()) == "") {
                alert('Select Reason and Enter Description');
                return false;
            }

            var answer = confirm("Are you sure to access this document in emergency situation? Click 'OK' to continue");
            if (answer == false)
                return false;

            document.getElementById('<%=loadingImage.ClientID%>').style.visibility = "visible";
            setTimeout('document.images["<%=loadingImage.ClientID%>"].src = "images/Searching.gif"', 1200000);
            return true;
        }

        
    </script>
    <style type="text/css">
        .emergencyPopup
        {
            background-color: #FFFFFF;
            border: 1px solid #999999;
            cursor: default;
            display: none;
            text-align: left;
            z-index: 100;
            position: absolute;
            top: 25%;
            left: 35%;
            width: 600px;
        }
        
        .emergencyPopup.div
        {
            border-bottom: 1px solid #EFEFEF;
            margin: 8px 0;
            padding-bottom: 8px;
        }
        .textAreaSize
        {
            resize: none;
        }
        .style3
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            font-style: normal;
            color: #000000;
            text-decoration: none;
            padding-bottom: 10px;
        }
    </style>
</asp:Content>
