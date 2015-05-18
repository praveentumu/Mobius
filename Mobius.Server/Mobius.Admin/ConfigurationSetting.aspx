<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ConfigurationSetting.aspx.cs" Inherits="ConfigurationSetting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="App_Themes/FGenesis/jquery-ui-1.8.13.custom.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/FGenesis/ui.dropdownchecklist.themeroller.css" />
    <style>
        .SeachFilterRowHead
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            padding-top: 6px;
            text-align: left;
            vertical-align: middle;
            height: 18px;
            padding-left: 4px;
            font-style: normal;
            color: #143d55;
            background-image: url(images/dolphin_bg-OVER.gif);
            text-decoration: none;
            font-weight: bold;
        }
        .textnew
        {
            color: Red;
        }
        .style1
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            font-style: normal;
            color: #000000;
            text-decoration: none;
            width: 50%;
        }
    </style>
    <script type="text/javascript" src="Scripts/ui.dropdownchecklist-1.4-min.js"></script>
    <script>

        function HeaderClick(sectionid) {
            if ($("#" + sectionid + " div").is(':hidden')) {
                $("#" + sectionid + " div").fadeToggle(2000);
                $("#" + sectionid + " table th img").attr('src', 'images/minus.gif');
            }
            else {
                $("#" + sectionid + " div").slideToggle(1000);
                $("#" + sectionid + " table th img").attr('src', 'images/plus.gif');
            }
        }


        var dvconfigsetting = false;
        var dvemergencyaccess = false;
        var dvproviderRole = false;
        function setDivLastState(controlName, state) {
            if (controlName == 'configsetting')
                dvconfigsetting = state;
            if (controlName == 'emergencyaccess')
                dvemergencyaccess = state;
            if (controlName == 'providerRole')
                dvproviderRole = state;

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

        function RestoreDivLastState() {
            if (dvconfigsetting)
                showHideMenu('configsetting');
            if (dvemergencyaccess)
                showHideMenu('emergencyaccess');
            if (dvproviderRole)
                showHideMenu('providerRole');

        }

    </script>
    <table style="width: 100%" border="0">
        <tr>
            <td colspan="4" align="left" class="Bold_text">
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="text" align="left" valign="top" colspan="4" style="width: 100%;">
                <div style="width: 100%; display: block;" id="emergencyaccess">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                        <tr>
                            <td colspan="2">
                                <span>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" class="Bold_text"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ShowSummary="true"
                                        ValidationGroup="configGroup" ShowMessageBox="false" class="text" />
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="4">
                <span class="text"><b>Note: </b><i style="color: Red;">Any incorrect/unwanted change,
                    done to the configuration file, might cause application, not work properly. So,
                    ensure the correctness of the data value being mapped, on your own or from previous
                    configuration. </i></span>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('baiscSetting');">
                &nbsp;Basic Settings
            </td>
        </tr>
        <tr>
            <td class="text">
                <div style="width: 100%; display: block;" id="baiscSetting">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                        <tr id="">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Document Path:
                                    <asp:Image ID="Image30" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="File server location where patient documents will be stored. &#013;Sample: \\10.131.10.151\First Genesis Documents\ " />
                                    <p />
                                    <asp:TextBox Text="" ID="txtDocPath" runat="server" Width="300" class="text" />
                                    &nbsp;<p />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator31" ControlToValidate="txtDocPath"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Document Path." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator31" ControlToValidate="txtDocPath"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide valid Document Path." ValidationExpression="^([a-zA-Z]\:|\\)\\(([^\\]+\\)*[^\/:*?<>|]+\\?)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Temp Path:
                                    <asp:Image ID="Image3" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Folder location to store temporary files. &#013;Sample:C:\Users\David\AppData\Local\Temp " />
                                    <p />
                                    <asp:TextBox Text="" ID="txtTempPath" runat="server" Width="300" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqTempPath" ControlToValidate="txtTempPath"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Temp Path." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" ControlToValidate="txtTempPath"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide valid Temp Path." ValidationExpression="^([a-zA-Z]\:|\\)\\(([^\\]+\\)*[^\/:*?<>|]+\\?)?$"
                                        runat="server" />
                                    <%--ValidationExpression="^[C][:]\\[A-Z+a-z ()0-9+.+~\\]+$"--%>
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="Tr3">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    AndroidApplicationVersion:
                                    <asp:Image ID="Image4" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Version of Android Application, which should be same as APK version. &#013;Sample: 1.0 " />
                                    <p />
                                    <asp:TextBox Text="" ID="txtAndroidVersion" runat="server" Width="300" MaxLength="20"
                                        class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqAndroidVersion" ControlToValidate="txtAndroidVersion"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Android Application Version." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                        ControlToValidate="txtAndroidVersion" ValidationExpression="^[1-9]\d*(\.\d+)?$"
                                        EnableClientScript="true" ValidationGroup="configGroup" ForeColor="Red" Text="Please provide valid numeric value."></asp:RegularExpressionValidator>
                                    <p />
                                &nbsp;
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Encryption Flag:
                                    <asp:Image ID="Image11" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Used to enable message encryption." />
                                    <p />
                                    <span id="Span6">
                                        <asp:RadioButton ID="EncryptionFlagTrue" CssClass="Permission" runat="server" GroupName="EncryptionFlagGroup"
                                            Text="Yes" />
                                        <asp:RadioButton ID="EncryptionFlagFalse" CssClass="Permission" runat="server" Text="No"
                                            GroupName="EncryptionFlagGroup" /></span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('certificateSetting');">
                &nbsp;Certificate Settings
            </td>
        </tr>
        <tr>
            <td class="text">
                <div style="width: 100%; display: block;" id="certificateSetting">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                        <tr id="Tr4">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    User Certificate Expiry Notification Gap (Days):
                                    <asp:Image ID="Image7" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Gap, in no. of days, before user notification should start appearing, for expiring user certificate. &#013;Sample: 30" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtUpgradationNotificationGap" runat="server" Width="300"
                                        MaxLength="5" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqUpgradationNotificationGap" ControlToValidate="txtUpgradationNotificationGap"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide User Upgradation Notification Gap." />
                                    <asp:RegularExpressionValidator ID="regexUpgradationNotificationGap" runat="server"
                                        Display="Dynamic" ControlToValidate="txtUpgradationNotificationGap" ValidationExpression="^([0-9]+)$"
                                        EnableClientScript="true" ValidationGroup="configGroup" ForeColor="Red" Text="Please provide valid numeric value."></asp:RegularExpressionValidator>
                                    <p />
                                &nbsp;
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Service Certificate Expiry Notification Gap (Days):
                                    <asp:Image ID="Image8" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Gap, in no. of days, before administrator notification should start appearing, for expiring server certificate. &#013;Sample: 30 " />
                                    <p />
                                    <asp:TextBox Text="" ID="txtServiceNotificationGap" runat="server" Width="300" MaxLength="5"
                                        class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqServiceNotificationGap" ControlToValidate="txtServiceNotificationGap"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Service Notification Gap." />
                                    <asp:RegularExpressionValidator ID="regexServiceNotificationGap" runat="server" Display="Dynamic"
                                        ControlToValidate="txtServiceNotificationGap" ValidationExpression="^([0-9]+)$"
                                        EnableClientScript="true" ValidationGroup="configGroup" ForeColor="Red" Text="Please provide valid numeric value."></asp:RegularExpressionValidator>
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="Tr1">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Certificate Authority Server URL:
                                    <asp:Image ID="Image5" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="URL of Certificate Authority Server. &#013;Sample: 10.131.10.151\MobiusCA " />
                                    <p />
                                    <asp:TextBox Text="" ID="txtCertAuthServerUrl" runat="server" Width="300" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqCertAuthServerUrl" ControlToValidate="txtCertAuthServerUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide CA Server URL." />
                                    <asp:RegularExpressionValidator ID="RegexCertAuthServerUrl" runat="server" Display="Dynamic"
                                        ControlToValidate="txtCertAuthServerUrl" ValidationExpression="^(((?:[\w]\:|\\){2})?((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?))(\\[a-zA-Z_ \-\s0-9]+)+\\?$"
                                        EnableClientScript="true" ValidationGroup="configGroup" ForeColor="Red" Text="Please provide valid CA Server URL."
                                        SetFocusOnError="true"></asp:RegularExpressionValidator>
                                    <p />
                                    &nbsp;<p />
                                    <span id="Span3"></span>
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Organization Unit:
                                    <asp:Image ID="Image6" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Organization Unit (to be used for certificate creation) &#013;Sample: Mobius " />
                                    <p />
                                    <asp:TextBox Text="" ID="txtCertOrgUnit" runat="server" Width="300" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqCertOrgUnit" ControlToValidate="txtCertOrgUnit"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Certificate Organizational Unit." />
                                    <%-- <asp:RegularExpressionValidator ID="RegexCertOrgUnit" ControlToValidate="txtCertOrgUnit"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide valid Certificate Organizational Unit." ValidationExpression="^([-_ A-Za-z0-9]+$)"
                                        runat="server" />--%>
                                    <p />
                                    &nbsp;<p />
                                    <span id="Span4"></span>
                            </td>
                        </tr>
                        <tr id="Tr16">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Certificate Organization Name:
                                    <asp:Image ID="Image2" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Organization Name (to be used for certificate creation) &#013;Sample: First Genesis" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtCertOrgName" runat="server" Width="300" class="text" />
                                    &nbsp;<p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqCertOrgName" ControlToValidate="txtCertOrgName"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Certificate Org Name." />
                                    <%--  <asp:RegularExpressionValidator ID="RegexCertOrgName" ControlToValidate="txtCertOrgName"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide valid Certificate Org Name." ValidationExpression="^([-_ A-Za-z0-9]+$)"
                                        runat="server" />--%>
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('connectspecificsetting');">
                &nbsp;Connect Specific Settings
            </td>
        </tr>
        <tr>
            <td class="text">
                <div style="width: 100%; display: block;" id="connectspecificsetting">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                        <tr id="Tr5">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Local Home Community ID:
                                    <asp:Image ID="Image9" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="This maps the local Home community ID and is controlled from ‘Nhin community management’ page. &#013;Sample: 2.16.840.1.113883.3.1605" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtHomeCommunityId" runat="server" Width="300" ReadOnly="True"
                                        class="text" />
                                    <p />
                                &nbsp;
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Connect Gateway Time Out (MiliSeconds):
                                    <asp:Image ID="Image10" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Time out period i.e. time for which server will wait for response from Connect Gateway. &#013;Sample: 180000" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtConnectGatewayTimeOut" runat="server" Width="300" MaxLength="20"
                                        class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqConnectGatewayTimeOut" ControlToValidate="txtConnectGatewayTimeOut"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Connect Gateway Time Out." />
                                    <asp:RegularExpressionValidator ID="regexConnectGatewayTimeOut" runat="server" Display="Dynamic"
                                        ControlToValidate="txtConnectGatewayTimeOut" ValidationExpression="^([0-9]+)$"
                                        EnableClientScript="true" ValidationGroup="configGroup" ForeColor="Red" Text="Please provide valid numeric value."></asp:RegularExpressionValidator>
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="Tr6">
                            <td align="left" colspan="2">
                                <p class="text">
                                    NHIN Patient Discovery URL:
                                    <asp:Image ID="Image12" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Patient Discovery service URL.   &#013;Sample: http://71.28.159.148:8080/Gateway/PatientDiscovery/1_0/EntityPatientDiscovery" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtNhinPatientDiscoveryUrl" runat="server" Width="805px"
                                        class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqNhinPatientDiscoveryUrl" ControlToValidate="txtNhinPatientDiscoveryUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide NHIN Patient Discovery URL." />
                                    <asp:RegularExpressionValidator ID="RegexNhinPatientDiscoveryUrl" ControlToValidate="txtNhinPatientDiscoveryUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <p class="text">
                                    NHIN Document Query URL:
                                    <asp:Image ID="Image13" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Document Query service URL. &#013;Sample: http://71.28.159.148:8080/Gateway/DocumentQuery/2_0/EntityService/EntityDocQueryUnsecured" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtNhinDocumentQueryUrl" runat="server" Width="805px" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqNhinDocumentQueryUrl" ControlToValidate="txtNhinDocumentQueryUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide NHIN Document Query URL." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtNhinDocumentQueryUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="Tr7">
                            <td align="left" colspan="2">
                                <p class="text">
                                    NHIN Retrieve Document URL:
                                    <asp:Image ID="Image15" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Document Retrieve service URL. &#013;Sample: http://71.28.159.148:8080/Gateway/DocumentRetrieve/2_0/EntityService/EntityDocRetrieve" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtNhinRetrieveDocumentUrl" runat="server" Width="805px"
                                        class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqNhinRetrieveDocumentUrl" ControlToValidate="txtNhinRetrieveDocumentUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide NHIN Retrieve Document URL." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txtNhinRetrieveDocumentUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <p class="text">
                                    NHIN Submission Document URL:
                                    <asp:Image ID="Image14" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Document submission service URL. &#013;Sample: http://71.28.159.148:8080/Gateway/DocumentSubmission/1_1/EntityService/EntityDocSubmissionUnsecured" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtNhinSubmissionDocumentUrl" runat="server" class="text"
                                        Width="805px" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqNhinSubmissionDocumentUrl" ControlToValidate="txtNhinSubmissionDocumentUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide NHIN Submission Document URL." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtNhinSubmissionDocumentUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="Tr8">
                            <td align="left" colspan="2">
                                <p class="text">
                                    NHIN Policy Engine URL:
                                    <asp:Image ID="Image16" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Policy engine service URL. &#013;Sample: http://71.28.159.148:8080/HarrisIntegrationServices/AdapterPolicyEngineIntegrationService" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtNhinPolicyEngineUrl" runat="server" Width="805px" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqNhinPolicyEngineUrl" ControlToValidate="txtNhinPolicyEngineUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide NHIN Policy Engine URL." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate="txtNhinPolicyEngineUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <p class="text">
                                    Mobius Patient Correlation:
                                    <asp:Image ID="Image17" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Patient correlation service URL. &#013;Sample: http://71.28.159.148:8080/MobiusAdapterPCService/MobiusPatientCorrelation" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtMobiusPatientCorrelation" runat="server" class="text"
                                        Width="805px" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqMobiusPatientCorrelation" ControlToValidate="txtMobiusPatientCorrelation"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Mobius Patient Correlation URL." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate="txtMobiusPatientCorrelation"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('docvalidationsetting');">
                &nbsp;Document Validation Settings
            </td>
        </tr>
        <tr>
            <td class="text">
                <div style="width: 100%; display: block;" id="docvalidationsetting">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                        <tr id="Tr14">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Validate Document:
                                    <asp:Image ID="Image26" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Enabling validation check for C32 document." />
                                    <p />
                                    <span id="Span2">
                                        <asp:RadioButton ID="ValidateDocumentTrue" CssClass="Permission" runat="server" GroupName="ValidateDocumentGroup"
                                            Text="Yes" />
                                        <asp:RadioButton ID="ValidateDocumentFalse" CssClass="Permission" runat="server"
                                            Text="No" GroupName="ValidateDocumentGroup" /></span>
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Use NIST online service to validate C32 Document:
                                    <asp:Image ID="Image25" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Mark if NIST online service will be used to validate C32 documents." />
                                    <p />
                                    <span id="Span1">
                                        <asp:RadioButton ID="UseNISTServiceTrue" CssClass="Permission" runat="server" GroupName="UseNISTServiceGroup"
                                            Text="Yes" />
                                        <asp:RadioButton ID="UseNISTServiceFalse" CssClass="Permission" runat="server" Text="No"
                                            GroupName="UseNISTServiceGroup" /></span>
                            </td>
                        </tr>
                        <tr id="Tr9">
                            <td align="left" colspan="2">
                                <p class="text">
                                    Mobius NIST Validation Service URL:
                                    <asp:Image ID="Image18" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Locally hosted Validation Service URL &#013;Sample: http://10.0.30.144:8080/Mobius/ValidationWebService" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtMobiusNistValidationUrl" runat="server" Width="805px"
                                        class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqMobiusNistValidationUrl" ControlToValidate="txtMobiusNistValidationUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Mobius NISTValidation Service URL." />
                                    <asp:RegularExpressionValidator ID="regexMobiusNistValidationUrl" ControlToValidate="txtMobiusNistValidationUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <p class="text">
                                    Online NIST Validation Service URL:
                                    <asp:Image ID="Image19" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="NIST online validation service URL  &#013;Sample: http://hit-testing.nist.gov:11080/ws/services/ValidationWebService" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtOnlineNistServiceUrl" runat="server" Width="805px" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqOnlineNistServiceUrl" ControlToValidate="txtOnlineNistServiceUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Online NISTValidation Service URL." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" ControlToValidate="txtOnlineNistServiceUrl"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Invalid URL" ValidationExpression="^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('smtpvalidationsetting');">
                &nbsp;SMTP Settings
            </td>
        </tr>
        <tr>
            <td class="text">
                <div style="width: 100%; display: block;" id="smtpvalidationsetting">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                        <tr id="Tr10">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Smtp User Name:
                                    <asp:Image ID="Image20" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Email address from which mail should be sent. &#013;Sample: mobiushealthinformation@gmail.com" />
                                </p>
                                <asp:TextBox Text="" ID="txtSmtpUserName" runat="server" Width="300" class="text" />
                                <p />
                                <asp:RequiredFieldValidator runat="server" ID="reqSmtpUserName" ControlToValidate="txtSmtpUserName"
                                    ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                    ForeColor="Red" Text="Please provide Smtp UserName." />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" ControlToValidate="txtSmtpUserName"
                                    ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                    ForeColor="Red" Text="Please provide valid Smtp UserName." ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                    runat="server" />
                                <span id="Span21">
                                    <br />
                                </span>
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Smtp Password:
                                    <asp:Image ID="Image24" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Password of the user account from which message should be sent." />
                                    <p />
                                    <asp:TextBox Text="" ID="txtSmtpPassword" runat="server" Width="300" class="text"
                                        TextMode="Password" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="reqSmtpPassword" ControlToValidate="txtSmtpPassword"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Smtp Password." />
                                    <p />
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="Tr13">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Smtp Host:
                                    <asp:Image ID="Image21" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="SMTP Host name. &#013;Sample: smtp.gmail.com" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtSmtpHost" runat="server" Width="300" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtSmtpHost"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Smtp Host." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" ControlToValidate="txtSmtpHost"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide valid Smtp Host." ValidationExpression="^((([\w]+\.[\w]+)+)|([\w]+)).(([\w]+\.)+)([A-Za-z]{1,3})$"
                                        runat="server" />
                                    <p />
                                &nbsp;
                            </td>
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Smtp Port:
                                    <asp:Image ID="Image23" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="SMTP Port number. &#013;Sample:587" />
                                    <p />
                                    <asp:TextBox Text="" ID="txtSmtpPort" runat="server" Width="300" MaxLength="10" class="text" />
                                    <p />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtSmtpPort"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide Smtp Port." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                        Display="Dynamic" ControlToValidate="txtSmtpPort" ValidationExpression="^([0-9]+)$"
                                        EnableClientScript="true" ValidationGroup="configGroup" ForeColor="Red" Text="Please provide valid numeric value."></asp:RegularExpressionValidator>
                                    <p />
                                    <span id="Span27"></span>
                            </td>
                        </tr>
                        <tr id="Tr11">
                            <td align="left" style="width: 50%">
                                <p class="text">
                                    Smtp Enable SSL:
                                    <asp:Image ID="Image22" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Used to enable SMTP SSL" />
                                    <p />
                                    <span id="Span5">
                                        <asp:RadioButton ID="SmtpEnableTrue" CssClass="Permission" runat="server" GroupName="SmtpEnableGroup"
                                            Text="Yes" />
                                        <asp:RadioButton ID="SmtpEnableFalse" CssClass="Permission" runat="server" Text="No"
                                            GroupName="SmtpEnableGroup" /></span>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <!--Edited by sid-->
        <tr>
            <td class="SeachFilterRowHead" style="width: 100%;" colspan="4" onclick="return showHideMenu('providerRole');">
                &nbsp;Emergency Access Settings
            </td>
        </tr>
        <tr>
            <td class="text" align="left" valign="top" colspan="4">
                <div style="width: 100%; display: block;" id="providerRole">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%">
                        <tr>
                            <td align="left" colspan="2">
                                <p class="text">
                                    Emergency Access Time (Hrs.)
                                    <asp:Image ID="Image27" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                        title="Validity time for which a provider need not to do emergency override again, for a document accessed earlier." />
                                    <p />
                                    <asp:TextBox Text="" ID="EmergencyAccessTime" runat="server" placeholder="Time in hrs."
                                        class="text" MaxLength="10" Width="300px" />
                                    <br />
                                    <asp:RequiredFieldValidator runat="server" ID="reqEmergencyAccessTime" ControlToValidate="EmergencyAccessTime"
                                        ValidationGroup="configGroup" EnableClientScript="true" Display="Dynamic" SetFocusOnError="true"
                                        ForeColor="Red" Text="Please provide valid numeric value." />
                                    <span id="errormsg"></span>
                                    <asp:RegularExpressionValidator ID="validateEmergencyAccessTime" runat="server" Display="Dynamic"
                                        ControlToValidate="EmergencyAccessTime" ValidationExpression="^([0-9]+)$" EnableClientScript="true"
                                        ValidationGroup="configGroup" ForeColor="Red" Text="Please provide valid numeric value."></asp:RegularExpressionValidator>
                                    <span id="Spanemergency">
                                        <br />
                                    </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1 text" align="left" valign="top">
                                Provider Roles:
                                <asp:Image ID="Image28" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                    title="List of available individual provider types  who don't have emergency access rights. &#013;Select the roles which you want to give the access of  Emergency override and click on Move Right button." />
                                <br />
                            </td>
                            <td class="style1 text" align="left" valign="top">
                                Roles mapped for emergency access:
                                <asp:Image ID="Image29" runat="server" Height="16px" ImageUrl="~/images/help_icon.png"
                                    title="List of all the providers who have emergency access rights. &#013;Select the roles which you don't want to give the access of Emergency override and click on Move Left button." />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1" align="left" valign="middle">
                                <asp:ListBox ID="lstNonEmergencyRoles" runat="server" Width="165px" Height="100px"
                                    SelectionMode="Multiple" Enabled="true" AutoPostBack="false" AppendDataBoundItems="true"
                                    EnableViewState="true" CssClass="text"></asp:ListBox>
                                <br />
                                <br />
                                <asp:Label ID="lblMove" runat="server" Text="Move Right" TextAlign="Right" />
                                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" TextAlign="Right"
                                    AutoPostBack="true" Visible="false" />
                                &nbsp;&nbsp;
                                <asp:ImageButton ID="imgbtnMoveRight" ImageAlign="AbsMiddle" runat="server" AlternateText="Move Right"
                                    ImageUrl="~/images/btn_forward.gif" OnClick="imgbtnMoveRight_Click" />
                            </td>
                            <td class="text" align="left" valign="middle">
                                <asp:ListBox ID="lstEmergencyRoles" runat="server" Width="165px" Height="100px" BackColor="#99EE99"
                                    SelectionMode="Multiple" Enabled="true" AutoPostBack="false" AppendDataBoundItems="true"
                                    EnableViewState="true" CssClass="text"></asp:ListBox>
                                <br />
                                <br />
                                <asp:ImageButton ID="imgbtnMoveLeft" runat="server" AlternateText="Move Left" ImageAlign="AbsMiddle"
                                    ImageUrl="~/images/btn_backward.gif" OnClick="imgbtnMoveLeft_Click" />
                                &nbsp;&nbsp;
                                <asp:Label ID="Label1" runat="server" Text="Move Left" TextAlign="Right" />
                                <asp:CheckBox ID="chkRemoveAll" runat="server" Text="Select All" TextAlign="Right"
                                    AutoPostBack="true" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="4">
                <asp:Button ID="btnPreview" runat="server" CssClass="Button" Text="Preview" OnClick="btnPreview_Click"
                    Width="90" />
                <asp:Button ID="btnsubmit" runat="server" CssClass="Button" ValidationGroup="configGroup"
                    Text="Submit" OnClick="btnsubmit_Click" Width="90" />
                <asp:Button ID="btnReset" runat="server" CssClass="Button" Text="Reset" OnClick="btnReset_Click"
                    Width="90" />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        var updatePannelObject = Sys.WebForms.PageRequestManager.getInstance();
        updatePannelObject.add_pageLoaded(initializeRequest);
        updatePannelObject.add_endRequest(EndRequestHandler);
        function initializeRequest(sender, args) {
            //add code for dropdown with multiple select closeRadioOnClick: false,

            //alert("1");
            $('#' + '<%=lstNonEmergencyRoles.ClientID%>').dropdownchecklist({ icon: {}, width: 170, emptyText: "Select", firstItemChecksAll: true, maxDropHeight: 200 });
            $('#' + '<%=lstEmergencyRoles.ClientID%>').dropdownchecklist({ icon: {}, width: 170, emptyText: "Select", firstItemChecksAll: true, maxDropHeight: 200 });


        }

        function EndRequestHandler(sender, args) {

            RestoreDivLastState();
        }

       

    </script>
</asp:Content>
