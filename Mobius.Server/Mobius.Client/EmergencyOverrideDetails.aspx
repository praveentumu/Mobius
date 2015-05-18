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
      
         <tr>
            <td colspan="4">
                <table style="border: 0px; width: 100%; max-height: 400px;">
                    <tr>
                        <td colspan="4">

                            <asp:GridView ID="grdProviderDetail" Width="100%" runat="server" AutoGenerateColumns="False"
                                ShowFooter="True" CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor"
                                PageSize="25" PagerStyle-HorizontalAlign="Right" 
                                onrowdatabound="grdProviderDetail_RowDataBound" >
                                <PagerStyle HorizontalAlign="Right" />
                                <RowStyle Height="20px" Font-Size="13px" BorderColor="#CCCCCC" BorderStyle="Solid"
                                    BorderWidth="1px" />
                                <AlternatingRowStyle CssClass="alternateRowColor" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Provider Name" >
                                        <ItemTemplate>
                                            <asp:Label ID="providerName" runat="server" CssClass="text" Text='<%#Eval("ProviderName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Document ID" >
                                        <ItemTemplate>
                                            <asp:Label ID="documentID" runat="server" CssClass="text" Text='<%#Eval("DocumentId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Email" >
                                        <ItemTemplate>
                                            <asp:Label ID="overriddenBy" runat="server" CssClass="text" Text='<%#Eval("OverriddenBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="User Role" >
                                        <ItemTemplate>
                                            <asp:Label ID="providerRole" runat="server" CssClass="text" Text='<%#Eval("ProviderRole") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Review by Admin" >
                                        <ItemTemplate>
                                            <asp:Label ID="reviewStatus" runat="server" CssClass="text" Text='<%#Eval("IsAudited") %>'></asp:Label>
                                        </ItemTemplate>
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

         <tr id="trOverrideReason" runat="server" >
            
             <td align="right" class="text">
                <asp:Label ID="lblEmergencyReasons" runat="server"  CssClass="text"  style="margin-right:15px">Override Reason</asp:Label>
            </td>
             <td  align="left" class="text">
                <asp:Textbox ID="txtEmergencyReason" runat="server" ReadOnly=true 
                     CssClass="text" Width="173px"/>
            </td>
        </tr>
        <tr><td colspan="2"></td></tr>

         <tr id="trIncidentDate" runat="server" >
             <td align="right" class="text">
                <asp:Label ID="Label1" runat="server" CssClass="text" style="margin-right:15px">Incident Date</asp:Label>
            </td>
             <td  align="left" class="text">
                        <asp:Textbox ID="txtIncidentDate" runat="server" ReadOnly=true CssClass="text" 
                            Width="171px"/>
           
            </td>
        </tr>

        <tr><td colspan="2"></td></tr>

       <tr id="trDescription" runat="server" >
             <td align="right" class="text"  style="vertical-align:top">
                <asp:Label ID="Label2" runat="server" CssClass="text"  style="margin-right:15px">Override Description</asp:Label>
            </td>
             <td  align="left" class="text">
                         <asp:Textbox ID="txtDescription" runat="server" CssClass="text" 
                        ReadOnly=true TextMode="MultiLine" 
                             style=" width:300px; height:100px; resize:none; overflow:scroll-y;" 
                             Height="130px" Width="314px"/>
           
            </td>
        </tr>

      <tr><td colspan="2"></td></tr>

        <tr>
            <td colspan="2" align="right">
            
                <asp:Button ID="btnBack" runat="server" Text="Back" class="Button"  onclick="btnBack_Click"  style="margin-right :50px" />
            
            </td>
             
        </tr>




       
       
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>