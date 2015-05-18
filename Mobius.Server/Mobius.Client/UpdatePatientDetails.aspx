<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdatePatientDetails.aspx.cs"
    Inherits="UpdatePatientDetails" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
    Title="Update Patient Details" ValidateRequest="false" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>
<asp:Content ID="ContentHolder1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript" src="Scripts/jquery-1.6.1.min.js"></script>
    <script type="text/javascript" language="javascript">

        function OnSave(obj) {
            // Find the row this button is in
            var tr = $(obj).closest("tr");
            // Get the value from the edit control
            var firstNameEdit = tr.find("[id*='firstNameEdit']").val();
            // assign value to hidden input
            tr.find("[id*='firstNameHidden']").val(firstNameEdit);

            var middleNameEdit = tr.find("[id*='middleNameEdit']").val();
            tr.find("[id*='middleNameHidden']").val(middleNameEdit);

            var lastNameEdit = tr.find("[id*='lastNameEdit']").val();
            tr.find("[id*='lastNameHidden']").val(lastNameEdit);

            var prefixNameEdit = tr.find("[id*='prefixNameEdit']").val();
            tr.find("[id*='PrefixNameHidden']").val(prefixNameEdit);

            var suffixNameEdit = tr.find("[id*='suffixNameEdit']").val();
            tr.find("[id*='SuffixNameHidden']").val(suffixNameEdit);

            var NameIDEdit = tr.find("[id*='NameIDEdit']").val();
            tr.find("[id*='hdnNameID']").val(NameIDEdit);

            
        }

    </script>
    <script type="text/javascript" language="javascript">
        function OnSaveTelephone(obj) {
            // Find the row this button is in
            var tr = $(obj).closest("tr");
            // Get the value from the edit control
            var TelephoneIdEdit = tr.find("[id*='TelephoneIDEdit']").val();
            tr.find("[id*='hdnTelephoneID']").val(TelephoneIdEdit);

            var TelephoneTypeEdit = tr.find("[id*='TelephoneTypeEdit']").val();
            // assign value to hidden input
            tr.find("[id*='TelephoneTypeHidden']").val(TelephoneTypeEdit);

            var TelephoneExtensionEdit = tr.find("[id*='TelephoneExtensionEdit']").val();
            tr.find("[id*='TelephoneExtensionHidden']").val(TelephoneExtensionEdit);

            var TelephoneDataEdit = tr.find("[id*='TelephoneDataEdit']").val();
            tr.find("[id*='TelephoneDataHidden']").val(TelephoneDataEdit);

            var TelephoneStatusEdit = tr.find("[id*='TelephoneStatusEdit']").val();
            tr.find("[id*='TelephoneStatusHidden']").val(TelephoneStatusEdit);
        }
    </script>
    <script type="text/javascript" language="javascript">


        function OnSaveAddress(obj) {
            // Find the row this button is in
            var tr = $(obj).closest("tr");
            // Get the value from the edit control


            var AddressLine1Edit = tr.find("[id*='AddressLine1Edit']").val();
            tr.find("[id*='AddressLine1eHidden']").val(AddressLine1Edit);

            var AddressID = tr.find("[id*='AddressIdEdit']").val();
            tr.find("[id*='AddressIdHidden']").val(AddressID);

            var AddressLine2Edit = tr.find("[id*='AddressLine2Edit']").val();
            tr.find("[id*='AddressLine2eHidden']").val(AddressLine2Edit);

            var CountryEdit = tr.find("[id*='CountryEdit']").val();
            tr.find("[id*='CountryHidden']").val(CountryEdit);
            //tr.find("[id*='CountryHidden']").val(tr.find("[id*='CountryEdit']").find("option:selected").text());

            var StateEdit = tr.find("[id*='StateEdit']").val();
            tr.find("[id*='StateHidden']").val(StateEdit);  

            var CityEdit = tr.find("[id*='CityEdit']").val();
            tr.find("[id*='CityHidden']").val(CityEdit);

            var ZipEdit = tr.find("[id*='ZipEdit']").val();
            tr.find("[id*='ZipHidden']").val(ZipEdit);

            var StatusEdit = tr.find("[id*='StatusEdit']").val();
            tr.find("[id*='StatusHidden']").val(StatusEdit);

        }

        function GetLocalityByZipCode(obj) {
            var Zipcode = obj.value;
            var tr = $(obj).closest("tr");

            if (Zipcode.length == 5) {
                PageMethods.GetLocality(Zipcode, onSucess, onError);
            }

            function onSucess(result) {

                var array = result.split(',');
                tr.find("[id*='CityEdit']").val(array[0]);
                tr.find("[id*='StateEdit']").val(array[1]);
                tr.find("[id*='CountryEdit']").val(array[2]);

                tr.find("[id*='NewCity']").val(array[0]);
                tr.find("[id*='NewState']").val(array[1]);
                tr.find("[id*='NewCountry']").val(array[2]);
            }

            function ShowError(objError) {

            }

            function onError(result) {

                alert('Cannot process your request at the moment, please try later.');
            }
        }

        function valid(o, w) {
            o.value = o.value.replace(valid.r[w], '');
        }
        valid.r = {
            'Alpha': /[^\sa-zA-Z'\.,-]+$/g,
            'special': /[\W]/g,
            'quotes': /['\''&'\"']/g,
            'numbers': /[^\d]/g
        }
    </script>
    <asp:UpdatePanel ID="Upd1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
                <%--Error Message--%>
                <tr>
                    <td align="left" class="Bold_text">
                        <asp:Label ID="lblErrorMsg" Visible="false" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;" class="PatientRegisterTable">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <%--Patient Label--%>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblPatient" CssClass="bluetext" runat="server" align="left" Text="Patient"></asp:Label>
                                    <%--<hr size="2" id="Hr1" width="100%" align="left" style="background-color: Black">--%>
                                </td>
                            </tr>
                            <%--Repeater  Control for Name--%>
                            <tr align="left">
                                <td style="width: 90%;">
                                    <asp:Repeater runat="server" ID="rptrName" OnItemDataBound="rptrName_ItemDataBound"
                                        OnItemCommand="rptrName_ItemCommand">
                                        <HeaderTemplate>
                                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                <tr>
                                                  <th  align="left" style="display: none">
                                                     <asp:Label ID="idLblName" runat="server" align="left" CssClass="text" Text="Name ID"></asp:Label>
                                                 </th>
                                                    <th align="left">
                                                        <asp:Label ID="Label17" runat="server" align="left" Text="Prefix" CssClass="text"></asp:Label>
                                                    </th>

                                                    <th align="left">
                                                        <font color="red">*</font>
                                                        <asp:Label ID="Label12" runat="server" align="left" Text="Given Name" CssClass="text"></asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="Label13" runat="server" align="left" CssClass="text" Text="Middle Name"></asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        <font color="red">*</font>
                                                        <asp:Label ID="Label14" runat="server" align="left" CssClass="text" Text="Family Name"></asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="Label18" runat="server" align="left" Text="Suffix" CssClass="text"></asp:Label>
                                                    </th>
                                                </tr>
                                                <%-- <tr>
                                        <th colspan="4">
                                            <hr size="1" id="Hr2" width="100%" align="left" style="background-color: Black">
                                        </th>
                                    </tr>--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                             <td style="display: none">
                                                    <asp:Label runat="server" CssClass="text" ID="NameID"><%# Eval("IDName") %></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="IDNameEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="hdnNameID" visible="false" />
                                                </td>
                                             <td>
                                                    <asp:Label runat="server" CssClass="text" ID="prefixName"><%# Eval("PrefixName") %></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="PrefixNameEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="PrefixNameHidden" visible="false" />
                                                </td>

                                                <td>
                                                    <asp:Label runat="server" CssClass="text" ID="firstName"><%# Eval("FirstName") %></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="firstNameEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="firstNameHidden" visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="text" ID="middleName"><%# Eval("MiddleName") %></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="middleNameEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="middleNameHidden" visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="text" ID="lastName"><%# Eval("LastName") %></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="lastNameEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="lastNameHidden" visible="false" />
                                                </td>
                                                 <td>
                                                    <asp:Label runat="server" CssClass="text" ID="suffixName"><%# Eval("SuffixName") %></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="SuffixNamePlaceHolder" />
                                                    <input type="hidden" runat="server" id="SuffixNameHidden" visible="false" />
                                                </td>

                                                <td align="center">
                                                    <asp:ImageButton ID="Edit" class="imgButton" ImageUrl="images/EditRptr.png" runat="server"
                                                        CommandName="edit" Height="20px" ToolTip="Edit Name" />
                                                    <asp:ImageButton ID="Delete" class="imgButton" ImageUrl="images/DeleteRptr.png" runat="server"
                                                        CommandName="delete" Height="20px" ToolTip="Delete Name" />
                                                </td>

                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <tr style="height: 2px">
                                            </tr>
                                            <tr id="csj">
                                             <td>
                                                    <asp:TextBox runat="server" onkeyup="valid(this,'Alpha')" ID="txtPrefixName" CssClass="text" />
                                                </td>

                                                <td>
                                                    <asp:TextBox runat="server" onkeyup="valid(this,'Alpha')" ID="NewFirstName" CssClass="text" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" onkeyup="valid(this,'Alpha')" ID="NewMiddleName" CssClass="text" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" onkeyup="valid(this,'Alpha')" ID="NewLastName" CssClass="text" />
                                                </td>
                                                 <td>
                                                    <asp:TextBox runat="server" onkeyup="valid(this,'Alpha')" ID="txtSuffixName" CssClass="text" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="add" class="imgButton" ImageUrl="images/AddRptr.png" runat="server"
                                                        OnClick="OnAddName" Height="20px" ToolTip="Add Name" />
                                                </td>
                                            </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                            <%--END Repeater  Control for Name--%>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;" class="PatientRegisterTable">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td align="left" style="width: 100%">
                                    <table border="0" cellpadding="2" cellspacing="2" style="width: 100%;">
                                        <%--first row--%>
                                        <tr>
                                            <th align="left">
                                                <font color="red">*</font>
                                                <asp:Label ID="lblSex" CssClass="text" runat="server" align="left" Text="Sex"></asp:Label>
                                            </th>
                                            <th align="left">
                                                <asp:Label ID="lblSSN" CssClass="text" runat="server" align="left" Text="SSN"></asp:Label>
                                            </th>
                                            <th align="left">
                                                <font color="red">*</font>
                                                <asp:Label ID="lblDOB" CssClass="text" runat="server" align="left" Text="Date of birth"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList Visible="true" runat="server" ID="ddlSex" Width="95" CssClass="text">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox Visible="true" ID="txtSSN" runat="server" TextMode="Password" CssClass="text"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox Visible="true" ID="txtDOB" runat="server" Width="90" CssClass="text"></asp:TextBox>
                                                <asp:Image ID="imgcalFrm" runat="server" ToolTip="Click to select date" ImageUrl="~/images/calenderIcon.gif"
                                                    ImageAlign="AbsMiddle" />
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <th align="left">
                                                <font color="red">*</font>
                                                <asp:Label ID="lblUseremail" CssClass="text" runat="server" align="left" Text="User Email"></asp:Label>
                                            </th>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox Visible="true" Font-Size="9" ID="txtUserEmail" Enabled=false runat="server" CssClass="text"></asp:TextBox>
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="PatientRegisterTable">
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                            <tr style="width: 100%">
                                <td align="left" colspan="4">
                                    <asp:Label ID="Label15" CssClass="bluetext" runat="server" align="left" Text=" Mother`s Maiden Name"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th class="text" align="left">
                                    &nbsp;Prefix:
                                </th>
                                <th class="text" align="left">
                                    &nbsp;Given Name
                                </th>
                                <th class="text" align="left">
                                    &nbsp;Middle Name
                                </th>
                                <th class="text" align="left">
                                    &nbsp;Family Name
                                </th>
                                <th class="text" align="left">
                                    &nbsp;Suffix
                                </th>
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
                    </td>
                </tr>
                <%--First Separator Line--%>
                <tr>
                    <td align="left">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="PatientRegisterTable">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                            <%--Telephone Label--%>
                            <tr style="width: 100%">
                                <td align="left">
                                    <asp:Label ID="Label1" CssClass="bluetext" runat="server" align="left" Text="Patient Telephone"></asp:Label>
                                </td>
                            </tr>
                            <%--Repeater  Control for Telephone--%>
                            <tr align="left">
                                <td style="width: 100%">
                                    <asp:Repeater runat="server" ID="rptrTelephone" OnItemDataBound="rptrTelephone_ItemDataBound"
                                        OnItemCommand="rptrTelephone_ItemCommand">
                                        <HeaderTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                  <th  align="left" style="display: none">
                                                                <asp:Label ID="idLabelTele" runat="server" align="left" CssClass="text" Text="Telephone ID"></asp:Label>
                                                 </th>
                                                    <th align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="text" align="left" Text="Type"></asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="lblData" runat="server" align="left" CssClass="text" Text="Data"></asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        <asp:Label ID="Label10" runat="server" align="left" CssClass="text" Text="Extension"></asp:Label>
                                                    </th>
                                                    <th align="left" colspan="2">
                                                        <asp:Label ID="Label11" runat="server" align="left" CssClass="text" Text="Status"></asp:Label>
                                                    </th>
                                                </tr>
                                                <%--<tr>
                                                            <th colspan="5">
                                                                <%--<hr size="1" id="Hr2" width="100%" align="left" style="background-color: Black">--
                                                            </th>--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>

                                               <td style="display: none">
                                                                <asp:Label CssClass="text" runat="server" ID="TelephoneID"><%# Eval("Id")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="TelephoneIDEditPlaceholder" />
                                                                <input type="hidden" runat="server" id="hdnTelephoneID" visible="false" />
                                                 </td>
                                                <td>
                                                    <asp:Label CssClass="text" runat="server" ID="TelephoneType"><%# Eval("Type")%></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="TelephoneTypeEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="TelephoneTypeHidden" visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="text" runat="server" ID="TelephoneData"><%# Eval("Number")%></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="TelephoneDataEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="TelephoneDataHidden" visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="text" runat="server" ID="TelephoneExtension"><%# Eval("Extensionnumber")%></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="TelephoneExtensionEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="TelephoneExtensionHidden" visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Label CssClass="text" runat="server" ID="TelephoneStatus"><%#GetTelephoneStatus(Eval("Status").ToString())%></asp:Label>
                                                    <asp:PlaceHolder runat="server" ID="TelephoneStatusEditPlaceholder" />
                                                    <input type="hidden" runat="server" id="TelephoneStatusHidden" visible="false" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="EditT" class="imgButton" ImageUrl="images/EditRptr.png" runat="server"
                                                        CommandName="editT" Height="20px" ToolTip="Edit Telephone Details" />
                                                    <asp:ImageButton ID="DeleteT" class="imgButton" ImageUrl="images/DeleteRptr.png"
                                                        runat="server" CommandName="deleteT" Height="20px" ToolTip="Delete Telephone Details" />
                                                    <%--<asp:ImageButton ID="Save" Visible="false" ImageUrl="images/base_floppydisk_32.png" runat="server" CommandName="save" />--%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <tr id="cskl">
                                                <tr style="height: 5px">
                                                </tr>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="NewTelephoneType" Width="150" CssClass="text" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="NewTelephoneData" CssClass="text" />
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="NewTelephoneExtension" CssClass="text" onkeyup="valid(this,'numbers')" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="NewTelephoneStatus" Width="150" CssClass="text" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="add" OnClick="OnAddTelephone" class="imgButton" ImageUrl="images/AddRptr.png"
                                                        runat="server" Height="20px" ToolTip="Add Telephone Details" />
                                                </td>
                                            </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                        <%--Second Separator Line--%>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                    </td>
                </tr>
                <tr>
                    <%--Repeater Address label--%>
                    <td style="width: 100%" class="PatientRegisterTable">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr style="width: 100%">
                                <td align="left">
                                    <asp:Label ID="lblPatientAddress" CssClass="bluetext" runat="server" align="left"
                                        Text="Patient Address"></asp:Label>
                                </td>
                            </tr>
                            <tr align="left">
                                <td style="width: 100%">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 900px">
                                        <tr>
                                            <td>
                                                <asp:Repeater runat="server" ID="rptr_Address" OnItemDataBound="rptr_Address_ItemDataBound"
                                                    OnItemCommand="rptr_Address_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                            <tr>
                                                               <th  align="left" style="display: none">
                                                                <asp:Label ID="idLabel" runat="server" align="left" CssClass="text" Text="Address ID"></asp:Label>
                                                               </th>
                                                                <th align="left" style="width: 30%">
                                                                    <asp:Label ID="Label3" runat="server" align="left" CssClass="text" Text="Address Line"></asp:Label>
                                                                </th>
                                                                <th align="left" style="display: none">
                                                                    <asp:Label ID="Label4" runat="server" align="left" CssClass="text" Text="Address Line2"></asp:Label>
                                                                </th>
                                                                <th align="left" style="width: 10%">
                                                                    <asp:Label ID="Label5" runat="server" align="left" CssClass="text" Text="Zip"></asp:Label>
                                                                </th>
                                                                <th align="left" style="width: 15%">
                                                                    <asp:Label ID="Label6" runat="server" align="left" CssClass="text" Text="City"></asp:Label>
                                                                </th>
                                                                <th align="left" style="width: 10%">
                                                                    <asp:Label ID="Label7" runat="server" align="left" CssClass="text" Text="State"></asp:Label>
                                                                </th>
                                                                <th align="left" style="width: 10%">
                                                                    <asp:Label ID="Label8" runat="server" align="left" CssClass="text" Text="Country"></asp:Label>
                                                                </th>
                                                                <th align="left" colspan="2" style="width: 15%">
                                                                    <asp:Label ID="Label9" runat="server" align="left" CssClass="text" Text="Status"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <%--<th colspan="8">
                                                        <hr size="1" id="Hr2" width="100%" align="left" style="background-color: Black">
                                                    </th>--%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                          <td style="display: none">
                                                                <asp:Label CssClass="text" runat="server" ID="AddressID"><%# Eval("Id")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="AddressIDEditPlaceholder" />
                                                                <input type="hidden" runat="server" id="AddressIdHidden" visible="false" />
                                                            </td>
                                                            <td style="width: 30%">
                                                                <asp:Label CssClass="text" runat="server" ID="AddressLine1"><%# Eval("AddressLine1")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="AddressLine1EditPlaceholder" />
                                                                <input type="hidden" runat="server" id="AddressLine1eHidden" visible="false" />
                                                            </td>
                                                            <td style="display: none">
                                                                <asp:Label CssClass="text" runat="server" ID="AddressLine2"><%# Eval("AddressLine2")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="AddressLine2EditPlaceholder" />
                                                                <input type="hidden" runat="server" id="AddressLine2eHidden" visible="false" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:Label CssClass="text" runat="server" ID="Zip"><%# Eval("Zip")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="ZipEditPlaceholder" />
                                                                <input type="hidden" runat="server" id="ZipHidden" visible="false" />
                                                            </td>
                                                            <td style="width: 15%">
                                                                <asp:Label CssClass="text" runat="server" ID="City"><%# Eval("City.CityName")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="CityEditPlaceholder" />
                                                                <input type="hidden" runat="server" id="CityHidden" visible="false" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:Label CssClass="text" runat="server" ID="State"><%# Eval("City.State.StateName")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="StateEditPlaceholder" />
                                                                <input type="hidden" runat="server" id="StateHidden" visible="false" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:Label CssClass="text" runat="server" ID="Country"><%# Eval("City.State.Country.CountryName")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="CountryEditPlaceholder" />
                                                                <input type="hidden" runat="server" id="CountryHidden" visible="false" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:Label CssClass="text" runat="server" ID="Status"><%# Eval("AddressStatus")%></asp:Label>
                                                                <asp:PlaceHolder runat="server" ID="StatusEditPlaceholder" />
                                                                <input type="hidden" runat="server" id="StatusHidden" visible="false" />
                                                            </td>
                                                            <td align="center" style="width: 15%">
                                                                <asp:ImageButton ID="EditA" class="imgButton" ImageUrl="images/EditRptr.png" runat="server"
                                                                    CommandName="editA" Height="20px" ToolTip="Edit Address" />
                                                                <asp:ImageButton ID="DeleteA" class="imgButton" ImageUrl="images/DeleteRptr.png"
                                                                    runat="server" CommandName="deleteA" Height="20px" ToolTip="Delete Address" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <tr style="height: 5px">
                                                        </tr>
                                                        <tr id="csj">
                                                            <td style="width: 30%">
                                                                <asp:TextBox runat="server" ID="NewAddressLine1" CssClass="text" Width="400" />
                                                            </td>
                                                            <td style="display: none">
                                                                <asp:TextBox runat="server" ID="NewAddressLine2" CssClass="text" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:TextBox CssClass="text" runat="server" ID="NewZip" onkeyup="valid(this,'numbers')"
                                                                    OnBlur="return GetLocalityByZipCode(this)" Width="50" />
                                                            </td>
                                                            <td style="width: 15%">
                                                                <asp:TextBox CssClass="text" runat="server" ID="NewCity" Width="120" Enabled="false" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:TextBox CssClass="text" runat="server" ID="NewState" Width="50" Enabled="false" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:TextBox CssClass="text" runat="server" ID="NewCountry" Width="50" Enabled="false" />
                                                            </td>
                                                            <td style="width: 10%">
                                                                <asp:DropDownList runat="server" ID="NewStatus" Width="100" CssClass="text" />
                                                            </td>
                                                            <td align="right" style="width: 15%">
                                                                <asp:ImageButton ID="add" OnClick="OnAddAddress" class="imgButton" ImageUrl="images/AddRptr.png"
                                                                    runat="server" Height="20px" ToolTip="Add Address" />
                                                            </td>
                                                        </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%--Repeater address--%>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="PatientRegisterTable">
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                            <tr style="width: 100%">
                                <td align="left" colspan="5">
                                    <asp:Label ID="Label16" CssClass="bluetext" runat="server" align="left" Text="Birth Place Address "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" style="width: 40%">
                                    <asp:Label ID="Label3" runat="server" align="left" CssClass="text" Text="Address Line"></asp:Label>
                                </th>
                                <th align="left" style="display: none">
                                    <asp:Label ID="Label4" runat="server" align="left" CssClass="text" Text="Address Line2"></asp:Label>
                                </th>
                                <th align="left" style="width: 10%">
                                    <asp:Label ID="Label5" runat="server" align="left" CssClass="text" Text="Zip"></asp:Label>
                                </th>
                                <th align="left" style="width: 15%">
                                    <asp:Label ID="Label6" runat="server" align="left" CssClass="text" Text="City"></asp:Label>
                                </th>
                                <th align="left" style="width: 10%">
                                    <asp:Label ID="Label7" runat="server" align="left" CssClass="text" Text="State"></asp:Label>
                                </th>
                                <th align="left" style="width: 10%">
                                    <asp:Label ID="Label8" runat="server" align="left" CssClass="text" Text="Country"></asp:Label>
                                </th>
                            </tr>
                            <tr id="csj">
                                <td align="left">
                                    <asp:TextBox runat="server" ID="txtBirthPlacestreetAddress" CssClass="text" Width="400" />
                                </td>
                                <td style="display: none">
                                    <asp:TextBox runat="server" ID="NewAddressLine2" CssClass="text" />
                                </td>
                                <td align="left">
                                    <asp:TextBox CssClass="text" runat="server" ID="txtBirthPlaceZip" onkeyup="valid(this,'numbers')"
                                        Width="50" />
                                </td>
                                <td align="left">
                                    <asp:TextBox CssClass="text" runat="server" ID="txtBirthPlaceCity" Width="120" />
                                </td>
                                <td align="left">
                                    <asp:TextBox CssClass="text" runat="server" ID="txtBirthPlaceState" Width="50" />
                                </td>
                                <td align="left">
                                    <asp:TextBox CssClass="text" runat="server" ID="txtBirthPlaceCountry" Width="50" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="text" valign="bottom" style="width: 100%">
                        <asp:Button ID="btnUpdate" OnClick="btnUpdate_Click" runat="server" CssClass="Button"
                            Text="Update" Style="margin-left: 14px" Width="50px"/>
                        <asp:Button ID="btnCancel" OnClick="CancelRegister" runat="server" CssClass="Button"
                            Text="Cancel" Style="margin-left: 14px" Width="50px" />
                        
                        <asp:HiddenField ID="hdnPatientID" runat="Server" />
                        <asp:HiddenField ID="hdnMPIID" runat="Server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
