using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mobius.BAL;
using Mobius.Entity;
using Mobius.CoreLibrary;
using FirstGenesis.UI;

public partial class ManageCommunities : System.Web.UI.Page
{
    #region Variables
    List<MobiusNHINCommunity> Communities;
    List<MobiusNHINCommunity> lstNhinCommunity = null;
    MobiusAppSettingUpdater MobiusAppSettingUpdater = null;
    protected MobiusBAL MobiusBAL = null;

    protected const string YES="Yes";
    protected const string NO = "No";
    protected const string NORMAL = "Normal";
    protected const string ALTERNATE = "Alternate";
    protected const string EDIT = "Alternate | Edit";
    protected const string TRUE = "true";


    #endregion Variables


    public bool HasBlankRow { get; set; }
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           // Response.Redirect("Default.aspx", false);
            lblErrorMsg.Text = string.Empty;
            MobiusBAL = new MobiusBAL();
            if (!IsPostBack)
            {
               
                LoadData();
               
            }
        }
        catch(Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    #endregion

    public void LoadData()
    {
        try
        {
            lstNhinCommunity=MobiusBAL.GetAllNhinCommunities();
            if (lstNhinCommunity.Count > 0)
            {
                grdCommunities.DataSource = lstNhinCommunity;
                grdCommunities.DataBind();
            }
            else {

                grdCommunities.DataSource = this.BindBlankGrid();
                grdCommunities.DataBind();
            }
         
        }
        catch(Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }


    protected List<MobiusNHINCommunity> BindBlankGrid()
    {
        List<MobiusNHINCommunity> lstNhinCommunity = new List<MobiusNHINCommunity>();
        MobiusNHINCommunity Community = new MobiusNHINCommunity();
        lstNhinCommunity.Add(Community);
        this.HasBlankRow = true;
        return lstNhinCommunity;
    }

    public void grdCommunities_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            grdCommunities.EditIndex = e.NewEditIndex;
            Label chkIsHomeCommunity = grdCommunities.Rows[grdCommunities.EditIndex].FindControl("IsHomeCommunity") as Label;

            bool IsEditable = false;
            if (chkIsHomeCommunity.Text == YES)
            {
                IsEditable = true;
            }
            LoadData();
            CheckBox chkEditIsHomeCommunity = grdCommunities.Rows[e.NewEditIndex].FindControl("EditIsHomeCommunity") as CheckBox;
            chkEditIsHomeCommunity.Enabled = !IsEditable;
            
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    protected void grdCommunities_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdCommunities.EditIndex = -1;
        LoadData();
    }

    protected void grdCommunities_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            TextBox txtEditCommunityIdentifier = grdCommunities.Rows[e.RowIndex].FindControl("EditCommunityIdentifier") as TextBox;
            TextBox txtEditCommunityDescription = grdCommunities.Rows[e.RowIndex].FindControl("EditCommunityDescription") as TextBox;
            CheckBox chkEditIsHomeCommunity = grdCommunities.Rows[e.RowIndex].FindControl("EditIsHomeCommunity") as CheckBox;
            CheckBox chkEditActive = grdCommunities.Rows[e.RowIndex].FindControl("EditActive") as CheckBox;
            HiddenField hdnID = grdCommunities.Rows[e.RowIndex].FindControl("EditHiddenFieldID") as HiddenField;
            //Update NhinCommunity List
            if (lstNhinCommunity == null)
                lstNhinCommunity = MobiusBAL.GetAllNhinCommunities();
            string message = string.Empty;
            lstNhinCommunity[e.RowIndex].CommunityDescription = txtEditCommunityDescription.Text;
            lstNhinCommunity[e.RowIndex].CommunityIdentifier = txtEditCommunityIdentifier.Text;
            lstNhinCommunity[e.RowIndex].IsHomeCommunity = chkEditIsHomeCommunity.Checked;
           
            lstNhinCommunity[e.RowIndex].Active = chkEditActive.Checked;
            lstNhinCommunity[e.RowIndex].ID = Convert.ToInt32(hdnID.Value);
            Result response = MobiusBAL.UpdateNhinCommunity(lstNhinCommunity[e.RowIndex]);
            if (response.IsSuccess)
            {
                grdCommunities.EditIndex = -1;
                LoadData();
               
                if (lstNhinCommunity[e.RowIndex].IsHomeCommunity)
                {
                    MobiusAppSettingUpdater = new MobiusAppSettingUpdater();
                    MobiusAppSettingUpdater.LocalHomeCommunityID = txtEditCommunityIdentifier.Text;
                    MobiusAppSettingUpdater.UpdateConfigurationFile();
                    MobiusAppSettingUpdater = null;
                }
                lblErrorMsg.Text = response.ErrorMessage;
            }
            else
            {
               
                lblErrorMsg.Text = response.ErrorMessage;
                e.Cancel = true;
                
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void grdCommunities_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (grdCommunities.EditIndex != -1)
            {
                grdCommunities.EditIndex = -1;
            }
            HiddenField ID = grdCommunities.Rows[e.RowIndex].FindControl("HiddenFieldID") as HiddenField;
            Label lblIsHomeCommunity = grdCommunities.Rows[e.RowIndex].FindControl("IsHomeCommunity") as Label;
            Result response = MobiusBAL.DeleteNhinCommunity(Convert.ToInt32(ID.Value), lblIsHomeCommunity.Text);
            if (!response.IsSuccess)
            {
                lblErrorMsg.Text = response.ErrorMessage;
            }
                LoadData();
        }
        catch (Exception ex)
        {

            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void grdCommunities_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
          
            if (e.CommandName.Equals("Insert"))
            {
                if (grdCommunities.EditIndex != -1)
                {
                    grdCommunities.EditIndex = -1;
                }
                TextBox txtAddCommunityIdentifier = grdCommunities.FooterRow.FindControl("AddCommunityIdentifier") as TextBox;
                TextBox txtAddCommunityDescription = grdCommunities.FooterRow.FindControl("AddCommunityDescription") as TextBox;
                CheckBox radAddIsHomeCommunity = grdCommunities.FooterRow.FindControl("AddIsHomeCommunity") as CheckBox;
                CheckBox chkAddActive = grdCommunities.FooterRow.FindControl("AddActive") as CheckBox;
                MobiusNHINCommunity community = new MobiusNHINCommunity();
                string message = string.Empty;
              
                community.CommunityIdentifier = txtAddCommunityIdentifier.Text;
                community.CommunityDescription = txtAddCommunityDescription.Text;
                community.IsHomeCommunity = radAddIsHomeCommunity.Checked;
                community.Active = chkAddActive.Checked;
                List<MobiusNHINCommunity> addnhincommunity = new List<MobiusNHINCommunity>();
                addnhincommunity.Add(community);
                List<string> existingrecordlist = new List<string>();
                Result response = MobiusBAL.AddNhinCommunities(addnhincommunity);

                if (response.IsSuccess)
                {
                    LoadData();
                }
                else
                {
                    lblErrorMsg.Text = response.ErrorMessage;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);

        }
    }

    protected void grdCommunities_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCommunities.PageIndex = e.NewPageIndex;
        LoadData();
    }


    protected void grdCommunities_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (this.HasBlankRow)
            {
                grdCommunities.Columns[4].Visible = false;
                grdCommunities.ShowFooter = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                Label lblHomeCommunity = e.Row.FindControl("IsHomeCommunity") as Label;
                Label lblActive = e.Row.FindControl("Active") as Label;
                string value = ((Mobius.Entity.MobiusNHINCommunity)(e.Row.DataItem)).IsHomeCommunity.ToString();
                if (this.HasBlankRow)
                {
                    lblHomeCommunity.Visible = false;
                    lblActive.Visible = false;

                }
                if (e.Row.RowState.ToString() == NORMAL || e.Row.RowState.ToString() == ALTERNATE)
                {
                   
                    if (value.ToLower() == TRUE)
                    {
                        lblHomeCommunity.Text = YES;
                    }
                    else
                    {
                        lblHomeCommunity.Text = NO;
                    }
                }
                if (e.Row.RowState.ToString() != EDIT)
                {

                    grdCommunities.EditIndex = -1;

                }
            }
          
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
   
   
}