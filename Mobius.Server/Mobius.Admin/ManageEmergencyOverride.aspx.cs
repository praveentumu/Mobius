using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI;
using Mobius.Entity;
using Mobius.BAL;
using Mobius.CoreLibrary;

public partial class ManageEmergencyOverride : System.Web.UI.Page
{
    public bool hasBlankRow { get; set; }
    protected  MobiusBAL mobiusBAL=null;
    List<EmergencyAudit> lstEmergencyAudit = null;
    public bool isShowAll { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            mobiusBAL = new MobiusBAL();
            if (!IsPostBack)
            {
                this.isShowAll = false;
                LoadData();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    public void LoadData()
    {
        try
        {
            //Get all the instance of Emergency Access

            EmergencyRecords emergencyRecords;
            if (isShowAll)
                emergencyRecords = EmergencyRecords.All;
            else
                emergencyRecords = EmergencyRecords.Open;

            Result result = mobiusBAL.GetAllEmergencyAudit(emergencyRecords, out lstEmergencyAudit);
            if (result.IsSuccess)
            {
                if (lstEmergencyAudit.Count > 0)
                {
                    grdEmergencyList.DataSource = lstEmergencyAudit;
                    grdEmergencyList.DataBind();
                    lblErrorMsg.Text = string.Empty;
                }
                else
                {
                    grdEmergencyList.DataSource = this.BindBlankGrid();
                    grdEmergencyList.DataBind();
                    lblErrorMsg.Text = Helper.GetErrorMessage(result.ErrorCode);

                }
            }
            else
            {
                grdEmergencyList.DataSource = this.BindBlankGrid();
                grdEmergencyList.DataBind();
                lblErrorMsg.Text = Helper.GetErrorMessage(result.ErrorCode);

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
            lblErrorMsg.Text = ex.Message;
        }

    }

    protected List<EmergencyAudit> BindBlankGrid()
    {
        EmergencyAudit emergencyAudit = new EmergencyAudit();
        List<EmergencyAudit> lstEmergencyAudit = new List<EmergencyAudit>();
        lstEmergencyAudit.Add(emergencyAudit);
        this.hasBlankRow = true;
        return lstEmergencyAudit;
    }

    protected void grdEmergencyList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            grdEmergencyList.ShowFooter = false;
          
           if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblIncidentDate = e.Row.FindControl("incidentDate") as Label;
                Label lblReason = e.Row.FindControl("reason") as Label;
                Label lblAuditStatus = e.Row.FindControl("auditStatus") as Label;
                ImageButton ibtnDetail = (ImageButton)e.Row.FindControl("ibtnDetail");
                CheckBox chkEmergencyRow = (CheckBox)e.Row.FindControl("chkEmergencyRow");

                if (ibtnDetail != null)
                {
                    ibtnDetail.CommandName = "Detail";
                    ibtnDetail.CommandArgument = e.Row.RowIndex.ToString();
                }
                if ( Convert.ToBoolean(lblAuditStatus.Text.ToString()) == false)
                {
                    lblAuditStatus.Text = "Review Required";
                }
                else {
                    lblAuditStatus.Text = "Review Done";
                    chkEmergencyRow.Enabled = false;
                }
                if (this.hasBlankRow)
                {
                    lblIncidentDate.Visible = false;
                    lblReason.Visible = false;
                    lblAuditStatus.Visible=false;
                    ibtnDetail.Visible = false;
                    chkEmergencyRow.Visible = false;
                    btnCloseReview.Visible = false;
                   
                }

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void chkIsShowAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIsShowAll.Checked)
        {
            this.isShowAll = true;
        }
        else
        {
            this.isShowAll = false;
        }

        LoadData();
   
    }
    protected void grdEmergencyList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Detail")
            {
                int iselectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = grdEmergencyList.Rows[iselectedIndex];
                int AuditID = Convert.ToInt32(((HiddenField)selRow.FindControl("HiddenAuditID")).Value);
                Response.Redirect("EmergencyOverrideDetails.aspx?AuditID=" + AuditID);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    protected void btnCloseReview_Click(object sender, EventArgs e)
    {
        List<int> auditIdList = new List<int>();
        try
        {
            if (grdEmergencyList.Rows.Count > 0)
            {
                foreach (GridViewRow dr in grdEmergencyList.Rows)
                {
                    CheckBox chk = dr.Cells[0].FindControl("chkEmergencyRow") as CheckBox;
                    if (chk.Checked)
                    {
                        HiddenField hdnAuditId = dr.Cells[0].FindControl("HiddenAuditID") as HiddenField;
                        EmergencyAudit emergencyAudit = new EmergencyAudit();
                        emergencyAudit.EmergencyAuditId = Convert.ToInt32(hdnAuditId.Value);
                        auditIdList.Add(emergencyAudit.EmergencyAuditId);
                    }
                }
            }
            Result result = mobiusBAL.UpdateOverrideDetails(auditIdList, true);
            if (result.IsSuccess)
            {
                LoadData();
            }
            else {
                lblErrorMsg.Text = Helper.GetErrorMessage(result.ErrorCode);
            }

        }
        catch(Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
}