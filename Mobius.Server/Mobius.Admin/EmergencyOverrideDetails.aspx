<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeFile="EmergencyOverrideDetails.aspx.cs" Inherits="EmergencyOverrideDetails" %>

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
            
  <td valign="top" class="text" align="right"  colspan="4">
                 </td>           
        </tr>
        <tr>
            <td colspan="4">
                <table style="border: 0px; width: 100%; max-height: 400px;">
                    <tr>
                        <td colspan="4">

                            <asp:GridView ID="grdPatientDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                ShowFooter="True" CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor"
                                PageSize="25" PagerStyle-HorizontalAlign="Right" 
                                onrowdatabound="grdPatientDetail_RowDataBound">
                                <PagerStyle HorizontalAlign="Right" />
                                <RowStyle Height="20px" Font-Size="13px" BorderColor="#CCCCCC" BorderStyle="Solid"
                                    BorderWidth="1px" />
                                <AlternatingRowStyle CssClass="alternateRowColor" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Patient ID" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="patientID" runat="server" CssClass="text" Text='<%#Eval("MPIID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Patient Name" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="firstName" runat="server" CssClass="text" Text='<%#Eval("PatientName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                   <%--  <asp:TemplateField HeaderText="Last Name" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lastName" runat="server" Text='<%#Eval("PatientName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>--%>
                                     <asp:TemplateField HeaderText="Date Of Incident" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="DOB" runat="server" CssClass="text" Text='<%#Eval("PatientDOB") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Gender" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="gender" runat="server" CssClass="text" Text='<%#Eval("PatientGender") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="5%"></HeaderStyle>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Document ID" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="documentID" runat="server" CssClass="text" Text='<%#Eval("DocumentId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="25%"></HeaderStyle>
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

         <tr>
            <td colspan="4">
                <table style="border: 0px; width: 100%; max-height: 400px;">
                    <tr>
                        <td colspan="4">

                            <asp:GridView ID="grdProviderDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                ShowFooter="True" CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor"
                                PageSize="25" PagerStyle-HorizontalAlign="Right" 
                                onrowdatabound="grdProviderDetail_RowDataBound">
                                <PagerStyle HorizontalAlign="Right" />
                                <RowStyle Height="20px" Font-Size="13px" BorderColor="#CCCCCC" BorderStyle="Solid"
                                    BorderWidth="1px" />
                                <AlternatingRowStyle CssClass="alternateRowColor" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Provider Name" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="providerName" runat="server" CssClass="text" Text='<%#Eval("ProviderName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Email" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="overriddenBy" runat="server" CssClass="text" Text='<%#Eval("OverriddenBy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="User Role" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="providerRole" runat="server" CssClass="text" Text='<%#Eval("ProviderRole") %>'></asp:Label>
                                        </ItemTemplate>
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

         <tr id="trOverrideReason" runat="server">
            <td >
            
            </td>
             <td class="text" align="right">
                <asp:Label ID="lblEmergencyReasons" runat="server"  CssClass="text">Override Reason</asp:Label>
            </td>
             <td  align="left" >
                <asp:Textbox ID="txtEmergencyReason" runat="server" CssClass="text" ReadOnly=true/>

            </td>
            <td>
            
            </td>
        </tr>
         
           <tr id="trIncidentDate" runat="server">
            <td >
            
            </td>
             <td class="text" align="right">
                <asp:Label ID="lblIncidentDate" runat="server"  CssClass="text">Incident Date</asp:Label>
            </td>
             <td  align="left">
                        <asp:Textbox ID="txtIncidentDate" runat="server"  ReadOnly=true CssClass="text"/>
           
            </td>
            <td>
            
            </td>
        </tr>

          <tr id="trDescription" runat="server">
            <td >
            
            </td>
             <td align="right" class="text"  style="vertical-align:top">
                <asp:Label ID="lblDescription" runat="server"  CssClass="text">Override Description</asp:Label>
            </td>
             <td  align="left" class="text">
                         <asp:Textbox ID="txtDescription" runat="server"  CssClass="text" ReadOnly=true
                         TextMode="MultiLine" style=" width:300px; height:100px; resize: none; overflow:scroll-y;"/>
           
            </td>
            <td>
            
            </td>
        </tr>

        <tr id="trReview" runat="server">
            <td >
            
            </td>
              <td align="right" class="text">
                <asp:Label ID="Label3" runat="server" CssClass="text">Close for further review </asp:Label>
            </td>
             <td  align="left" class="text">
                          <asp:RadioButton ID="isAuditedTrue" CssClass="Permission" AutoPostBack="true" 
                                    runat="server" GroupName="AuditStatus" Text="Yes" />
                          <asp:RadioButton ID="isAuditedFalse" CssClass="Permission" runat="server" AutoPostBack="true"
                                    Text="No" GroupName="AuditStatus" />
           
            </td>
            <td>
           <asp:HiddenField ID="HiddenAuditID" runat="server"  />


            </td>
        </tr>


        <tr>
            <td colspan="5" align="right">
            
                <asp:Button ID="btnSubmit" runat="server" Text="Submit"  class="Button"                     onclick="btnSubmit_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnBack" runat="server" Text="Back" class="Button"  onclick="btnBack_Click"  style="margin-right :50px" />
            
            </td>
             
        </tr>




       
       
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>