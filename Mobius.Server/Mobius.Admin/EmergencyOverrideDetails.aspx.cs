using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mobius.Entity;
using Mobius.BAL;
using FirstGenesis.UI;
using Mobius.CoreLibrary;

public partial class EmergencyOverrideDetails : System.Web.UI.Page
{
    # region Variables

    int auditID = 0;
    EmergencyAudit emergencyAudit = null;
    List<EmergencyAudit> lstEmergencyAudit = null;
    MobiusBAL mobiusBAL = null;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(Request.QueryString["AuditID"]) != 0)
            {
                auditID = Convert.ToInt32(Request.QueryString["AuditID"]);

            }
            else
            {
                Response.Redirect("login.aspx");
            }
            mobiusBAL = new MobiusBAL();
            if (!IsPostBack)
            {
                LoadData(auditID);
            }


        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }

    /// <summary>
    /// load the data on page load
    /// </summary>
    /// <param name="AuditID"></param>
    public void LoadData(int AuditID)
    {
        try
        {
            lstEmergencyAudit = new List<EmergencyAudit>();

            Result result = mobiusBAL.GetEmergencyDetailById(AuditID, out emergencyAudit);
            if (result.IsSuccess)
            {
                lstEmergencyAudit.Add(emergencyAudit);
                if (emergencyAudit != null)
                {
                    grdPatientDetail.DataSource = lstEmergencyAudit;
                    grdProviderDetail.DataSource = lstEmergencyAudit;
                    grdPatientDetail.DataBind();
                    grdProviderDetail.DataBind();

                }
                txtIncidentDate.Text = emergencyAudit.OverrideDate.ToString();
                txtEmergencyReason.Text = emergencyAudit.OverrideReason.ToString();
                txtDescription.Text = emergencyAudit.Description.ToString();
                if (emergencyAudit.IsAudited)
                {
                    isAuditedTrue.Checked = true;
                }
                else
                {
                    isAuditedFalse.Checked = true;
                }
            }
            else
            {
                trOverrideReason.Visible = false;
                trIncidentDate.Visible = false;
                trDescription.Visible = false;
                trReview.Visible = false;
                btnSubmit.Visible = false;
                lblErrorMsg.Text = Helper.GetErrorMessage(result.ErrorCode);
            }
           
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
            lblErrorMsg.Text = ex.Message;
        }
    }

    protected void grdPatientDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        grdPatientDetail.ShowFooter = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblGender = e.Row.FindControl("Gender") as Label;
            lblGender.Text = ((Gender)(Convert.ToInt32( lblGender.Text))).ToString();
        }
    }
    protected void grdProviderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        grdProviderDetail.ShowFooter = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Result result = new Result();
        bool IsAuditStatus = false;

        if (isAuditedTrue.Checked)
            IsAuditStatus = true;
        else
            IsAuditStatus = false;

        try
        {
           
                List<int> auditIdList = new List<int>();
                EmergencyAudit emergencyAudit = new EmergencyAudit();
                emergencyAudit.EmergencyAuditId = Convert.ToInt32(auditID);
                auditIdList.Add(emergencyAudit.EmergencyAuditId);
                result = mobiusBAL.UpdateOverrideDetails(auditIdList, IsAuditStatus);

                if (result.IsSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "alert('" + Helper.GetErrorMessage(result.ErrorCode) + "');" +
                                                                                          " window.location='ManageEmergencyOverride.aspx';", true);

                }
                else
                {
                    lblErrorMsg.Text = Helper.GetErrorMessage(result.ErrorCode);
                }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
            lblErrorMsg.Text = ex.Message;
        }

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageEmergencyOverride.aspx");
    }
}