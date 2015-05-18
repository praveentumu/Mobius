using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI;
using Mobius.Entity;
using Mobius.CoreLibrary;
using MobiusServiceLibrary;
using FirstGenesis.UI.Base;

public partial class ViewEmergencyOverride : BaseClass
{
    public bool hasBlankRow { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
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
            
            var patientId= GlobalSessions.SessionItem(SessionItem.MPIID) != null ? (!string.IsNullOrEmpty(GlobalSessions.SessionItem(SessionItem.MPIID).ToString()) ? GlobalSessions.SessionItem(SessionItem.MPIID).ToString() : null) : null;
            GetEmergencyAuditRequest getEmergencyAuditRequest = new GetEmergencyAuditRequest();
            getEmergencyAuditRequest.MPIID = patientId;
            getEmergencyAuditRequest.EmergencyRecords = EmergencyRecords.All;  //in case of patinet all records will be shown

            GetEmergencyAuditResponse getEmergencyAuditResponse = this.objProxy.GetAllEmergencyAudit(getEmergencyAuditRequest);
            if (getEmergencyAuditResponse.ListEmergencyAccess!=null &&  getEmergencyAuditResponse.ListEmergencyAccess.Count > 0)
            {
                grdEmergencyList.DataSource = getEmergencyAuditResponse.ListEmergencyAccess;
                grdEmergencyList.DataBind();
                lblErrorMsg.Text = string.Empty;
            }
            else
            {
                grdEmergencyList.DataSource = this.BindBlankGrid();
                grdEmergencyList.DataBind();
                lblErrorMsg.Text = Helper.GetErrorMessage(getEmergencyAuditResponse.Result.ErrorCode);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
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

                if (ibtnDetail != null)
                {
                    ibtnDetail.CommandName = "Detail";
                    ibtnDetail.CommandArgument = e.Row.RowIndex.ToString();
                }

               lblAuditStatus.Text =Convert.ToBoolean(lblAuditStatus.Text) ?"Done":"Pending";

                if (this.hasBlankRow)
                {
                    lblIncidentDate.Visible = false;
                    lblReason.Visible = false;
                    ibtnDetail.Visible = false;
                    lblAuditStatus.Text = "";
                }
              

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
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
    
}