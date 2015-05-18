<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeFile="ImportCommunities.aspx.cs" Inherits="ImportCommunities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <asp:UpdatePanel ID="imprtcommunity" runat="server" UpdateMode="Conditional">
        <contenttemplate>
        <script type="text/javascript">
            $("[id*=chkHeader]").live("click", function () {

                var chkHeader = $(this);
                var grid = $(this).closest("table");

                $("input[type=checkbox]", grid).each(function () {
                    if (chkHeader.is(":checked")) {
                        $("[id*=chkCommunityRow]").prop("checked", true);
                    } else {
                        $("[id*=chkCommunityRow]").prop("checked", false);
                    }

                });
            });

            $("[id*=chkCommunityRow]").live("click", function () {
                if ($("[id*=chkCommunityRow]").length == $("[id*=chkCommunityRow]:checked").length) {
                    $("[id*=chkHeader]").attr("checked", true);
                }
                else {
                    $("[id*=chkHeader]").attr("checked", false);

                }

            });


</script>
      <table style="width: 100%">
                <tr>
                    <td colspan="3" align="left" class="Bold_text">
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr class="border">
                  <td align="right">
                  <asp:RadioButton ID="rdExcel" CssClass="Permission" AutoPostBack="true" 
                          runat="server" GroupName="Filetpe" Text="Import from Excel" Checked="true" />
                 
                   </td>
                   <td align="left">
                   <asp:RadioButton ID="rdCsv" CssClass="Permission" AutoPostBack="true" runat="server" GroupName="Filetpe" Text="Import from CSV" />
                  </td>
                </tr>
            
                <tr style="width: 100%">
                    <td class="text" align="right" style="width: 20%" valign=middle>
                       File Path
                    </td>
                    <td align="left" style="width: 50%" colspan=2> <asp:FileUpload ID="fileuploadExcel" runat="server" Width="400" />
                    </td >
                </tr>
                <tr>
                    <td class="text" align="right">
                       Worksheet Name:
                    </td>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="worksheetname" runat="server" ValidationGroup="SearchGroup" CssClass="text" Text="Import Communities" Enabled=false></asp:TextBox>
                    </td>
                </tr>
              <tr><td>&nbsp;</td></tr> 
              <tr >
                <td colspan="3" style="background-color:#9DDFFF;" align="left">
                <div class="Bold_text_heading" style="padding:5px;">
               Columns Map (Name of corresponding columns in Excel/CSV)</div>
                 
                </td>
              </tr>


               <tr>
                    <td class="text" align="right">
                       Community Identifier:
                    </td>
                    <td align="left" colspan="2">
                        <asp:Textbox ID="lblCommunityIdentifier" runat="server" enabled=false CssClass="text"
                            Text="CommunityIdentifier"  />
                    </td>
                  
                </tr>


               <tr>
                    <td class="text" align="right">
                       Community Description:
                    </td>
                    <td align="left" colspan="2">
                        <asp:Textbox ID="lblCommunityDescription" runat="server" Text="CommunityDescription" enabled=false CssClass="text"/>
                    </td>
                  
                </tr>


               <tr>
                    <td class="text" align="right">
                       Active:
                    </td>
                    <td align="left" colspan="2">
                         <asp:Textbox ID="lblActive" runat="server" Text="Active" enabled=false CssClass="text"/>

                    </td>
                  
                </tr>
               
               <tr>
                <td class="text" align="left">
                      
                </td>
                <td colspan="2" align="right"> 
                       <asp:Button ID="btnShow" runat="server" Text="Show Communities" OnClick="btnShowCommunity_Click" class="Button" />
                        &nbsp;&nbsp;
                            <asp:Button ID="BtnBack" runat="server" class="Button" onclick="btnBack_Click" 
                             Text="Back" Width="90" /> &nbsp;&nbsp;
                </td>
                </tr>
               
               <tr>
                  <td colspan="3"> 
                      <span style="font-size:12px; float:right;" class="headtext"> Download and use <asp:LinkButton ID="downloadtemplte" runat="server" text="template" 
                          onclick="downloadtemplte_Click" Font-Underline=true ForeColor=blue></asp:LinkButton>  to enable correct import of communities.</span>
                      
                    </td>
               </tr>
               
             <tr >
             <td colspan="3" >
                 <table style="border: 0px solid #ccc;width:100%; max-height:400px;" id="importtable" runat="server" >
                     <tr>
                     <td colspan="4"></td>
                     <td>
                       <span style="float:right; margin-top:0px;"><asp:Button ID="btnImport" runat="server" Text="Import Communities" class="Button"
                            onclick="btnImport_Click" /></span>
                     </td>
                     </tr>
                    
                     <tr>
                        <td colspan="5" align=center  class="style1">
                        <asp:GridView ID="grdCommunityUpload" Width="100%" runat="server"  AutoGenerateColumns="false" ShowFooter="false"
                         CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor" 
                            PageSize="25" PagerStyle-HorizontalAlign="Right" Visible=true
                            AllowPaging="true" AllowSorting="true"  OnPageIndexChanging="GrdCommunityUpload_PageIndexChanging"
                                onrowdatabound="grdCommunityUpload_RowDataBound" >
                                <RowStyle Height="20px" Font-Size="13px" BorderColor="#CCCCCC" BorderStyle="Solid"
                                BorderWidth="1px" />
                            <Columns>
                                

                                <asp:TemplateField  HeaderStyle-Width="2%" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-CssClass="gridth">
                                         <HeaderTemplate>
                                           <%-- <asp:CheckBox ID="Checkbox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckUncheckAll" />--%>
                                           <input type="checkbox" id="chkHeader" runat=server  />
                                         </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCommunityRow" Visible=false runat="server" class="chkHeadercls" />
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Community Identifier"  HeaderStyle-Width="50px">

                                    <ItemTemplate>
                                    <asp:Label ID="CommunityIdentifier" runat="server" Text='<%#Eval("CommunityIdentifier") %>'></asp:Label>
                                    </ItemTemplate>
                                  <%--  <EditItemTemplate>
                                    <asp:TextBox ID="CommunityIdentifier2" runat="server" Text='<%#Eval("CommunityIdentifier") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    <asp:TextBox ID="AddCommunityIdentifier" runat="server"></asp:TextBox>
                                  </FooterTemplate>--%>
                                    <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>


                                <asp:TemplateField HeaderText="Community Description" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                    <asp:Label ID="CommunityDescription" runat="server" Text='<%#Eval("CommunityDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%"></HeaderStyle>
                                    </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Home Community" HeaderStyle-Width="40px">
                                    <ItemTemplate>

                                     <asp:Label ID="IsHomeCommunity"  Visible=false runat=server Enabled=false Text='<%#Eval("IsHomeCommunity") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="12%"></HeaderStyle>
                                    </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                    
                                     <asp:CheckBox ID="Active" runat="server"  Visible=false  Checked='<%#Eval("Active") %>'></asp:CheckBox>
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%"></HeaderStyle>
                                    </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </td>
                        <td class="style1">
                      
                        </td>
                     </tr>
                     <tr>
                     <td colspan=5>&nbsp;</td></tr>
                       <tr>
                     <td colspan=5><span style="float:right; margin-top:0px;">
                         
                         </span></td></tr>
                 </table>
         
          
                        
             </td>
          
           </tr>

            
                <tr>
                    <td colspan=4>
                        &nbsp;</td>
                </tr>
            </table>
            </contenttemplate>
        <triggers>
                <asp:PostBackTrigger ControlID="btnShow"  />
                <asp:PostBackTrigger ControlID="downloadtemplte"  />
            </triggers>
    </asp:UpdatePanel>
</asp:Content>
