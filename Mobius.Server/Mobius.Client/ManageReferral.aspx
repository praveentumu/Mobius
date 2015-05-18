<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="ManageReferral.aspx.cs" Inherits="ManageReferrals" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="GridCustomControl" Namespace="VMMSF.Components.GridCustomControl"
    TagPrefix="cc1" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel  id ="updtpnl1" runat="server">
        <ContentTemplate>
        <div>
            <tr>
                <td align="left" class="Bold_text">
                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="red"></asp:Label>
                </td>
            </tr>
            <tr align="left">
                <td align="left" style="height: 25px" valign="middle">
                    <asp:Label Font-Size="Medium" ID="lblManageRefferel" Font-Bold="true" runat="server"
                        class="bluetext">Referrals For me</asp:Label>
                </td>
                 <td valign="top" class="text" align="right" width="40%">
                        <asp:CheckBox Text="Show All Records" Checked="false" ID="chkAllReferralsForMe" 
                        OnCheckedChanged="chkAllReferralsForMe_OnCheckedChanged"  runat="server"  AutoPostBack="true" />
                 </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gridReferral" Width="100%" runat="server" AutoGenerateColumns="false"
                        OnRowCommand="GridReferral_RowCommand"
                        CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor" OnRowDataBound="GridReferral_RowDataBound"
                        PageSize="10" PagerStyle-HorizontalAlign="Right" OnPageIndexChanging="GridReferral_PageIndexChanging"
                        AllowPaging="true" AllowSorting="true" Visible="true">
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DocID" HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                    <asp:Label ID="DocID" runat="server" Text='<%# Bind("DocumentId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReferredByEmail" ItemStyle-Wrap="true" HeaderText="Referred By"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />

                            <asp:TemplateField HeaderText="Patient Name" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of birth" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ReferralOn" HeaderText="Referred On"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left"/>

                            <asp:BoundField DataField="PatientAppointmentDate" HeaderText="Appointment Date"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left"/>

                            <asp:BoundField DataField="ReferralAccomplishedOn" HeaderText="Accomplishment Date"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left"/>
                                                      
                            <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                HeaderStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnDetails" Visible="false" runat="server" ToolTip="Details"
                                        ImageUrl="images/share.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            </div>
 </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel  id ="updtpnl2" runat="server">
        <ContentTemplate>
            <tr><td>
            <br /><br /> 
            </td></tr>
            <div>
             <tr align="left">
                <td align="left" style="height: 25px" valign="middle">
                    <asp:Label Font-Size="Medium" ID="Label1" Font-Bold="true" runat="server"
                        class="bluetext">Referred By me</asp:Label>
                </td>
                 <td valign="top" class="text" align="right">
                        <asp:CheckBox Text="Show All Records" Checked="false" ID="chkAllReferralsByMe"  
                            runat="server" oncheckedchanged="chkAllReferralsByMe_CheckedChanged" AutoPostBack="true" />
                 </td>
            </tr>
            <tr>
             <td colspan="3">
                    <asp:GridView ID="gvReferredByMe" Width="100%" runat="server" AutoGenerateColumns="false"
                        OnRowCommand="gvReferredByMe_RowCommand"
                        CssClass="grid" AlternatingRowStyle-CssClass="alternateRowColor" OnRowDataBound="gvReferredByMe_RowDataBound"
                        PageSize="10" PagerStyle-HorizontalAlign="Right" OnPageIndexChanging="gvReferredByMe_PageIndexChanging"
                        AllowPaging="true" AllowSorting="true" Visible="true">
                        <Columns>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                    <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DocID" HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="false" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                    <asp:Label ID="DocID" runat="server" Text='<%# Bind("DocumentId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReferredToEmail" ItemStyle-Wrap="true" HeaderText="Referred To"
                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />

                            <asp:TemplateField HeaderText="Patient Name" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of birth" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gender" ItemStyle-HorizontalAlign="Left"
                                HeaderStyle-HorizontalAlign="Left" Visible="true" ItemStyle-Wrap="true" HeaderStyle-CssClass="gridth">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ReferralOn" HeaderText="Referred On"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left"/>

                            <asp:BoundField DataField="PatientAppointmentDate" HeaderText="Appointment Date"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left"/>

                            <asp:BoundField DataField="ReferralAccomplishedOn" HeaderText="Accomplishment Date"
                                ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left"/>
                                                      
                            <asp:TemplateField HeaderText="Details" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gridth"
                                HeaderStyle-Width="25px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnDetails" Visible="false" runat="server" ToolTip="Details"
                                        ImageUrl="images/share.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
