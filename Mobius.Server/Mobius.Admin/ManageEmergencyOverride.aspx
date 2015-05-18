<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPage.Master" CodeFile="ManageEmergencyOverride.aspx.cs" Inherits="ManageEmergencyOverride"   %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="mngemergencyoverride" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <script type="text/javascript">
            $("[id*=chkHeader]").live("click", function () {

                var chkHeader = $(this);
                var grid = $(this).closest("table");

                $("input[type=checkbox]", grid).each(function () {
                    if (chkHeader.is(":checked")) {
                            $("[id*=chkEmergencyRow]:not(:disabled)").prop("checked", true);
                    } else {
                        $("[id*=chkEmergencyRow]").prop("checked", false);
                    }

                });
            });

            $("[id*=chkEmergencyRow]").live("click", function () {
                if ($("[id*=chkEmergencyRow]").length == $("[id*=chkEmergencyRow]:checked").length) {
                    $("[id*=chkHeader]").attr("checked", true);
                }
                else {
                    $("[id*=chkHeader]").attr("checked", false);

                }

            });


</script>
    <table style="width: 100%" border="0">
        <tr>
            <td colspan="4" align="left" class="Bold_text">
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr style="height: 25px;">
            
  <td valign="top" class="text" align="right"  colspan="4">
                        <asp:CheckBox Text="Show All Records" Checked="false" ID="chkIsShowAll" 
                        runat="server"  AutoPostBack="true" 
                            oncheckedchanged="chkIsShowAll_CheckedChanged" />
                 </td>           
        </tr>
        <tr>
            <td colspan="4">
                <table style="border: 0px; width: 100%; max-height: 400px;">
                    <tr>
                        <td >

                            <asp:GridView ID="grdEmergencyList" Width="100%" runat="server" AutoGenerateColumns="False"
                                ShowFooter="True" CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor"
                                PageSize="50" PagerStyle-HorizontalAlign="Right" 
                                onrowdatabound="grdEmergencyList_RowDataBound" 
                                onrowcommand="grdEmergencyList_RowCommand">
                                <PagerStyle HorizontalAlign="Right" />
                                <RowStyle Height="20px" Font-Size="13px" BorderColor="#CCCCCC" BorderStyle="Solid"
                                    BorderWidth="1px" />
                                <AlternatingRowStyle CssClass="alternateRowColor" />
                                <Columns>

                                 <asp:TemplateField  HeaderStyle-Width="2%" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="gridth">
                                         <HeaderTemplate>
                                           <%-- <asp:CheckBox ID="Checkbox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckUncheckAll" />--%>
                                           <input type="checkbox" id="chkHeader" runat=server  />
                                         </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEmergencyRow"  runat="server" class="chkHeadercls" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Patient Name" >
                                        <ItemTemplate>
                                            <asp:Label ID="patientName" runat="server" CssClass="text" Text='<%#Eval("PatientName") %>'></asp:Label>
                                             <asp:HiddenField ID="HiddenAuditID" runat="server" Value='<%#Eval("EmergencyAuditId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Patient ID" >
                                        <ItemTemplate>
                                            <asp:Label ID="patientID" runat="server" CssClass="text" Text='<%#Eval("MPIID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Document ID" >
                                          <ItemTemplate>
                                            <asp:Label ID="documentID" runat="server" CssClass="text" Text='<%#Eval("DocumentId") %>'></asp:Label>
                                        </ItemTemplate>
                                   </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date Of Incident" >
                                     <ItemTemplate>
                                            <asp:Label ID="incidentDate" runat="server" CssClass="text" Text='<%#Eval("OverrideDate") %>'></asp:Label>
                                        </ItemTemplate>
                                  </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Performed By" >
                                        <ItemTemplate>
                                            <asp:Label ID="performedBy" runat="server" CssClass="text" Text='<%#Eval("ProviderName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reason">
                                         <ItemTemplate>
                                            <asp:Label ID="reason" runat="server" CssClass="text" Text='<%#Eval("OverrideReason") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Audit Status" >
                                          <ItemTemplate>
                                            <asp:Label ID="auditStatus" runat="server" CssClass="text" Text='<%#Eval("IsAudited") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Details">
                                         <ItemTemplate>
                                            <asp:ImageButton runat="server" CommandName="Detail" Id="ibtnDetail"
                                                 Height="16px" ImageUrl="images/share.png" ToolTip="Details" Width="16px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                       
                    </tr>

                      <tr>
               
                <td  align="right"> 
                       <asp:Button ID="btnCloseReview" runat="server" Text="Close For Review"  
                           class="Button" onclick="btnCloseReview_Click" />
                       
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
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
