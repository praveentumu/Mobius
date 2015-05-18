using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mobius.Entity;
using FirstGenesis.UI;
using Mobius.CoreLibrary;
using FirstGenesis.UI.Base;
using MobiusServiceLibrary;

public partial class EmergencyOverrideDetails : BaseClass
{
    # region Variables

    int auditID = 0;

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

    public void LoadData(int AuditID)
    {
        try
        {


            GetEmergencyAuditRequest getEmergencyAuditRequest = new GetEmergencyAuditRequest();
            getEmergencyAuditRequest.EmergencyAuditId = AuditID;
            GetEmergencyAuditResponse getEmergencyAuditResponse = this.objProxy.GetEmergencyDetailById(getEmergencyAuditRequest);
            if (getEmergencyAuditResponse.ListEmergencyAccess != null && getEmergencyAuditResponse.ListEmergencyAccess.Count > 0)
            {
                grdProviderDetail.DataSource = getEmergencyAuditResponse.ListEmergencyAccess;
                grdProviderDetail.DataBind();

                txtIncidentDate.Text = getEmergencyAuditResponse.ListEmergencyAccess.First().OverrideDate.ToString();
              txtEmergencyReason.Text = getEmergencyAuditResponse.ListEmergencyAccess.First().OverrideReason.ToString();
                txtDescription.Text = getEmergencyAuditResponse.ListEmergencyAccess.First().Description.ToString();
            }
            else
            {
                    trOverrideReason.Visible = false;
                    trIncidentDate.Visible = false;
                    trDescription.Visible = false;
                    lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.UnknownException);

            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }



    protected void grdProviderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            grdProviderDetail.ShowFooter = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblReviewStatus = e.Row.FindControl("reviewStatus") as Label;

                lblReviewStatus.Text =Convert.ToBoolean(lblReviewStatus.Text.ToString())? "Done":"Pending";

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewEmergencyOverride.aspx");
    }
   
}