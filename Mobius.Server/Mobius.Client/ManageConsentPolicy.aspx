<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageConsentPolicy.aspx.cs"
    Inherits="RolesAndPermission" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>


<asp:Content ID="ContentHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
.border {
   border: solid 1px #143d55;
 } 
}
</style>
    <asp:UpdatePanel ID="Upd1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function checkStartDate(sender, args) {
                    var date = new Date();
                    var currentStartDate = date.toDateString();
                    var selectedStartDate = sender._selectedDate.toDateString();
                    if (Date.parse(selectedStartDate) < Date.parse(currentStartDate)) {
                        alert("You cannot select a day earlier than today!");
                        sender._selectedDate = new Date();
                        // set the date back to the current date
                        sender._textbox.set_Value(sender._selectedDate.format(sender._format))
                    }
                }
                function checkEndDate(sender, args) {
                    var date = new Date();
                    var currentStartDate = date.toDateString();
                    var selectedStartDate = sender._selectedDate.toDateString();
                    if (Date.parse(selectedStartDate) < Date.parse(currentStartDate)) {
                        alert("You cannot select a day earlier than today!");
                        sender._selectedDate = new Date();
                        // set the date back to the current date
                        sender._textbox.set_Value(sender._selectedDate.format(sender._format))
                    }
                }


                function findById(id) {
                    var mandatoryIds = <%= jsonMandatoryLeafs %>
                    var flag = false;
                    var result = $.grep(mandatoryIds, function (e) { return e == id; });
                    if (result.length == 0) {
                        flag = false
                    }
                    else {
                        flag = true;
                    }
                    return flag;
                }

                function DisabledCheckBox() {
                    var treeView1 = $('#<%= trvModules.ClientID %>');
                    var treeImages = treeView1.find('img').not('img[alt=\'\']');
                    for (var i = 0; i < 1; i++) {
                        //Get all the leafs node of the selected root. 
                        var nodeCheckBox = $(treeImages[i]).closest("table").next("div").find("input[type=checkbox]");
                        //Loop the leafs to find if selected checkbox is child of the current node.
                        for (var j = 0; j < nodeCheckBox.length; j++) {
                            var nodeText = $(nodeCheckBox[j])[0].nextSibling.childNodes[0].nodeValue;
                            //Compare the Id`s if the leaf node Id matches with the selected checkBox id then this is desired root to be checked.

                            if (findById(nodeText)) {
                                var checkBoxId = $(nodeCheckBox[j]);
                                if (!(checkBoxId.is(':checked'))) {
                                    checkBoxId.attr("checked", true);
                                }
                                checkBoxId.attr("disabled", true);
                            }
                        }
                     }
                }


                function validatePage()
                {
                   if($('#<%= ddlRole.ClientID%>').val() == 0)
                   {
                       alert('<%=SELECT_ROLE %>');
                       return false;
                   }
                  else if($('#<%= ddlPurpose.ClientID%>').val() == 0)
                   {
                       alert('<%=SELECT_PURPOSE %>');
                       return false;
                   }
                  else if($('#<%= txtRuleStartDate.ClientID%>').val() == 0)
                   {
                       alert('<%=SELECT_RULE_START_DATE %>');
                       return false;
                   }
                   else if($('#<%= txtRuleEndDate.ClientID%>').val() == 0)
                   {
                       alert('<%=SELECT_RULE_END_DATE %>');
                       return false;
                   }

                return true;
                }

              Sys.Application.add_load(function(){
                DisabledCheckBox();
                    $("div[id $= trvModules] input[type=checkbox]").click(function () {
                
                        // On click of root node, it will select/de-select the leafs
                        $(this).closest("table").next("div").find("input[type=checkbox]").attr("checked", this.checked);
                        //Check selected node is leaf and is part of root.
                        if ($(this).closest("table").next("div").find("input[type=checkbox]").length == 0) {
                            //get selected checkBox Id  
                            
                            var checkBoxId = $(this).closest("table").next("div").find("input[type=checkbox]").context.id
                            var treeView1 = $('#<%= trvModules.ClientID %>');
                                var treeImages = treeView1.find('img').not('img[alt=\'\']');
                                //Loop the tree node to find the selected check box
                                // Setting i = 1 in below code, to exclude the root node from the calculation 
                                     var nodeCheckBox = $(treeImages[0]).closest("table").next("div").find("input[type=checkbox]");
                                      $(treeImages[i]).closest("table").find("input[type=checkbox]").attr("checked", true);
                                       var count=0;
                                       for (var j = 0; j < nodeCheckBox.length; j++) {
                                            if($('#' + nodeCheckBox[j].id).is(':checked'))
                                            {
                                             count=count+1;
                                             }
                                       }
                                
                                       if(count==nodeCheckBox.length)
                                        $(treeImages[0]).closest("table").find("input[type=checkbox]").attr("checked", true);
                                       else
                                         $(treeImages[0]).closest("table").find("input[type=checkbox]").attr("checked", false);

                            //If selected check box status is checked then find root node of the selected node to check if it is selected or not. if not then select
                            if ($('#' + checkBoxId).is(':checked')) {
                                for (var i = 1; i < treeImages.length; i++) {
                                    //Get all the leafs node of the selected root. 
                                     
                                    var nodeCheckBox = $(treeImages[i]).closest("table").next("div").find("input[type=checkbox]");
                                    //Loop the leafs to find if selected checkbox is child of the current node.
                                    for (var j = 0; j < nodeCheckBox.length; j++) {
                                   
                                        //Compare the Id`s if the leaf node Id matches with the selected checkBox id then this is desired root to be checked.
                                        if (nodeCheckBox[j].id == checkBoxId) {
                                            //Check root node is selected or not. if not then select 
                                            if (!($(treeImages[i]).closest("table").find("input[type=checkbox]").is(':checked'))) {
                                                $(treeImages[i]).closest("table").find("input[type=checkbox]").attr("checked", this.checked);
                                            }
                                        }
                                        
                                    }
                                }
                                
                                //changes to check parent item

                               
                               }
                                //end changes
                        }
                        else
                        {
                        DisabledCheckBox();
                        }
                    });
                });



            </script>
            <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                <tr style="width: 100%">
                    <td>
                        <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>&nbsp;</tr>
                <tr>&nbsp;</tr>
                <tr>
                    <td style="20%;" class="text" valign="top" align="right">
                        Role<span style="color: #FF0000">*</span>&nbsp;
                    </td>
                    <td class="Bold_text" valign="top" align="left" style="width: 30%">
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="text" ValidationGroup="SaveGroup"
                            AutoPostBack="false" Width="135px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 20%;" class="text" valign="top" align="right">
                        Purpose<span style="color: #FF0000">*</span>&nbsp;
                    </td>
                    <td class="Bold_text" valign="top" align="left" style="width: 30%">
                        <asp:DropDownList ID="ddlPurpose" runat="server" CssClass="text" ValidationGroup="SaveGroup"
                            AutoPostBack="false" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="text" align="right">
                        Rule Start Date<span style="color: #FF0000">*</span>&nbsp;
                    </td>
                    <td valign="top" align="left">
                        <asp:TextBox ID="txtRuleStartDate" runat="server" CssClass="text" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgCalRuleStartDate" runat="server" ImageAlign="Middle" ImageUrl="~/images/calenderIcon.gif"
                            ToolTip="Click to select date" />
                        <ajax:MaskedEditExtender ID="mEERuleStartDate" runat="server" AcceptNegative="Left"
                            AutoComplete="false" AutoCompleteValue="05/23/1964" CultureName="en-US" DisplayMoney="Left"
                            Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                            OnInvalidCssClass="MaskedEditError" TargetControlID="txtRuleStartDate">
                        </ajax:MaskedEditExtender>
                        <ajax:MaskedEditValidator ID="mEVRuleStartDate" runat="server" ControlExtender="mEERuleStartDate"
                            ControlToValidate="txtRuleStartDate" Display="None" EmptyValueMessage="Date is required"
                            InvalidValueMessage="Date is invalid" IsValidEmpty="False" MaximumValue="01/01/2210"
                            MaximumValueMessage="Message Max" MinimumValue="01/01/1900" MinimumValueMessage="Message Min"
                            TooltipMessage="Input a Date" ValidationGroup="MailInRebateVG">
                        </ajax:MaskedEditValidator>
                        <ajax:CalendarExtender ID="cERuleStartDate" runat="server" Format="MM/dd/yyyy" OnClientDateSelectionChanged="checkStartDate"
                            PopupButtonID="imgCalRuleStartDate" TargetControlID="txtRuleStartDate">
                        </ajax:CalendarExtender>
                    </td>
                    <td class="text" align="right">
                        Rule End Date<span style="color: #FF0000">*</span>&nbsp;
                    </td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtRuleEndDate" runat="server" CssClass="text" Width="100px"></asp:TextBox>
                        <asp:Image ID="imgCalRuleEndDate" runat="server" ImageAlign="Middle" ImageUrl="~/images/calenderIcon.gif"
                            ToolTip="Click to select date" />
                        <ajax:MaskedEditExtender ID="mEERuleEndDate" runat="server" AcceptNegative="Left"
                            AutoComplete="false" AutoCompleteValue="05/23/1964" CultureName="en-US" DisplayMoney="Left"
                            Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                            OnInvalidCssClass="MaskedEditError" TargetControlID="txtRuleEndDate">
                        </ajax:MaskedEditExtender>
                        <ajax:MaskedEditValidator ID="mEVRuleEndDate" runat="server" ControlExtender="mEERuleEndDate"
                            ControlToValidate="txtRuleEndDate" Display="None" EmptyValueMessage="Date is required"
                            InvalidValueMessage="Date is invalid" IsValidEmpty="False" MaximumValue="01/01/2210"
                            MaximumValueMessage="Message Max" MinimumValue="01/01/1900" MinimumValueMessage="Message Min"
                            TooltipMessage="Input a Date" ValidationGroup="MailInRebateVG">
                        </ajax:MaskedEditValidator>
                        <ajax:CalendarExtender ID="cERuleEndDate" runat="server" Format="MM/dd/yyyy" OnClientDateSelectionChanged="checkEndDate"
                            PopupButtonID="imgCalRuleEndDate" TargetControlID="txtRuleEndDate">
                        </ajax:CalendarExtender>
                    </td>
                    <tr>
                        <td align="right" class="text">
                            Permission<span style="color: #FF0000">*</span>&nbsp;
                        </td>
                        <td align="left" valign="top">
                            <asp:RadioButtonList ID="rbtnAllow" runat="server" Font-Size="Small" RepeatDirection="Horizontal">
                                <asp:ListItem Enabled="true" Text="Allow" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Enabled="true" Text="Deny" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                         <td class="text" align="right">
                        Active<span style="color: #FF0000">*</span>&nbsp;
                          </td>
                            <td align="left" valign="top">
                            <asp:RadioButtonList ID="rbtnActive" runat="server" Font-Size="Small" RepeatDirection="Horizontal">
                                <asp:ListItem Enabled="true" Text="Yes" Selected="True" Value="1"></asp:ListItem>
                                <asp:ListItem Enabled="true" Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:Button ID="btnUpdatePermission" runat="server" CssClass="Button" OnClick="btnUpdatePermission_Click"
                                OnClientClick="return validatePage();" Text="Update" Width="60px" />
                                <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" class="border">
                            <asp:TreeView ID="trvModules" runat="server" EnableClientScript="true" LeafNodeStyle-CssClass="LeafNodeStyle"
                                ParentNodeStyle-CssClass="ParentNodeStyle" RootNodeStyle-CssClass="RootNodeStyle">
                            </asp:TreeView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <br />
                            <asp:Button ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                Text="Back" Width="60px" />

                            <asp:Button ID="btnUpdatePermission2" runat="server" CssClass="Button" OnClick="btnUpdatePermission_Click"
                                OnClientClick="return validatePage();" Text="Update" Width="60px" />
                        </td>
                    </tr>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        DisabledCheckBox();
    </script>
</asp:Content>
