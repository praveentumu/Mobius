using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI;
using FirstGenesis.UI.Base;
using MobiusServiceLibrary;
using VMMSF.Components.GridCustomControl;
using Mobius.CoreLibrary;
using MobiusServiceUtility;


public partial class ManageConsent : BaseClass
{

    #region Local Variables

    public static int pageindex;
    private const string MANAGE_CONSENT_POLICY_PAGE = "ManageConsentPolicy.aspx";
    GetPatientConsentRequest getPatientConsentRequest = null;
    GetPatientConsentResponse getPatientConsentResponse = null;
    SoapHandler soapHandler;
    SoapProperties soapProperties = new SoapProperties();
    List<PatientConsent> patientConsents = null;
    #endregion

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            getPatientConsentRequest = new GetPatientConsentRequest();
            getPatientConsentResponse = new GetPatientConsentResponse();

            if (!IsPostBack)
            {
                btnConfigureConsent.Enabled = false;
                if ((bool)GlobalSessions.SessionItem(SessionItem.IsOptIn))
                {
                    rdOptIn.Checked = true;
                    rdOptOut.Checked = false;
                    btnConfigureConsent.Enabled = true;
                }
                else
                {
                    rdOptIn.Checked = false;
                    rdOptOut.Checked = true;
                }

                btnConfigureConsent.Enabled = false;
                getPatientConsentRequest.MPIID = GlobalSessions.SessionItem(SessionItem.MPIID).ToString();
                soapHandler.RequestEncryption(getPatientConsentRequest, out soapProperties);
                getPatientConsentRequest.SoapProperties = soapProperties;
                getPatientConsentResponse = objProxy.GetPatientConsent(getPatientConsentRequest);
                if (getPatientConsentResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(getPatientConsentResponse.SoapProperties, getPatientConsentResponse))
                    {
                        if ((bool)GlobalSessions.SessionItem(SessionItem.IsOptIn))
                        {
                            rdOptIn.Checked = true;
                            btnConfigureConsent.Enabled = true;
                        }
                        else
                        {
                            rdOptIn.Checked = false;
                        }

                        if (patientConsents == null)
                        {
                            patientConsents = getPatientConsentResponse.PatientConsents;
                            //set the page index of grid for showing the appropriate page after editing the user information.
                            if (GlobalSessions.SessionItem(SessionItem.PageIndex) != null)
                            {
                                if (GlobalSessions.SessionItem(SessionItem.PageIndex) + "" != "")
                                {
                                    gridUser.PageIndex = Convert.ToInt32(GlobalSessions.SessionItem(SessionItem.PageIndex).ToString());
                                    GlobalSessions.SessionRemove(SessionItem.PageIndex);
                                }
                            }
                        }
                        else
                        {
                            return;
                        }

                    }

                }
            }
            BindGridData();
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    #endregion

    #region gridUser_RowEditing
    protected void gridUser_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            int iMapID = Int32.Parse(gridUser.DataKeys[Int32.Parse(e.NewEditIndex.ToString())].Values["PatientConsentID"].ToString());

            if (gridUser.GetKey.Length > 0)
            {
                GlobalSessions.SessionAdd(SessionItem.PatientConsentId, gridUser.GetKey);
                GlobalSessions.SessionAdd(SessionItem.PageIndex, pageindex);
                //GlobalSessions.SessionAdd(SessionItem.UserType, "3");
                GlobalSessions.SessionAdd(SessionItem.IsNew, "false");
                Response.Redirect(MANAGE_CONSENT_POLICY_PAGE, false);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    #endregion

    #region gridUser_RowDeleting
    protected void gridUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DeletePatientConsentPolicyRequest deletePatientConsentPolicyRequest = null;
        DeletePatientConsentPolicyResponse deletePatientConsentPolicyResponse = null;
        try
        {
            Result result = new Result();
            deletePatientConsentPolicyRequest = new DeletePatientConsentPolicyRequest();
            deletePatientConsentPolicyResponse = new DeletePatientConsentPolicyResponse();
            if (gridUser.GetKey.Length > 0)
            {
                deletePatientConsentPolicyRequest.patientConsentId = gridUser.GetKey.ToString();
                soapHandler.RequestEncryption(deletePatientConsentPolicyRequest, out soapProperties);
                deletePatientConsentPolicyRequest.SoapProperties = soapProperties;
                deletePatientConsentPolicyResponse = objProxy.DeletePatientConsentPolicy(deletePatientConsentPolicyRequest);
                if (deletePatientConsentPolicyResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(getPatientConsentResponse.SoapProperties, getPatientConsentResponse))
                    {
                        BindGridData();
                    }
                    else
                    {
                        // to do : add error message
                    }
                }
                btnConfigureConsent.Enabled = true;
            }
            GlobalSessions.SessionRemove(SessionItem.PatientConsentId);
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    #endregion

    #region gridUser_RowDataBound
    protected void gridUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                gridUser.Columns[0].Visible = false;
                LinkButton lnkdelete = (LinkButton)e.Row.FindControl("lnkDelete");
                LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                if (e.Row.FindControl("RuleStartDate") != null)
                {
                    System.Web.UI.WebControls.Label lblRuleStartDate = (System.Web.UI.WebControls.Label)e.Row.FindControl("RuleStartDate");
                    lblRuleStartDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "RuleStartDate")).ToString("MM/dd/yyyy");
                }
                if (e.Row.FindControl("RuleEndDate") != null)
                {
                    System.Web.UI.WebControls.Label lblEndDate = (System.Web.UI.WebControls.Label)e.Row.FindControl("RuleEndDate");
                    lblEndDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "RuleEndDate")).ToString("MM/dd/yyyy");
                }
                // Retrieve the status value for the current row. 
                if (e.Row.FindControl("Allow") != null)
                {
                    string PermissionStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Allow"));
                    System.Web.UI.WebControls.Label lblStatus = (System.Web.UI.WebControls.Label)e.Row.FindControl("Allow");
                    lblStatus.Text = PermissionStatus == "True" ? "Allow" : "Deny";
                }
                // Retrieve the state value for the current row. 
                if (e.Row.FindControl("Active") != null)
                {
                    string isactive = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Active"));
                    System.Web.UI.WebControls.Label lblIsActive = (System.Web.UI.WebControls.Label)e.Row.FindControl("Active");

                    lblIsActive.Text = isactive == "True" ? "Yes" : "No";

                    if (isactive != "True")
                    {
                        if (lblIsActive != null)
                        {
                            lblIsActive.Text = "No";
                        }
                      
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }
    #endregion

    #region BindGridData
    protected void BindGridData()
    {

        // initialize the data table        
        DataTable OldDt = new DataTable();
        DataTable NewDt = new DataTable();
        /*here is the declaration of sorted list*/
        // Column name as in database table pass here
        SortedList htColumnName;
        // Control Type have same sequence as you want 
        SortedList htControlTye;
        // here creating instance of the GridCustomControl 
        GridCustomControl objGridCustomControl = new GridCustomControl();
        try
        {
            getPatientConsentRequest = new GetPatientConsentRequest();
            getPatientConsentResponse = new GetPatientConsentResponse();

            //if (dt == null)
            //{
            getPatientConsentRequest.MPIID = GlobalSessions.SessionItem(SessionItem.MPIID).ToString();
            soapHandler.RequestEncryption(getPatientConsentRequest, out soapProperties);
            getPatientConsentRequest.SoapProperties = soapProperties;
            getPatientConsentResponse = objProxy.GetPatientConsent(getPatientConsentRequest);
            if (getPatientConsentResponse.Result.IsSuccess)
            {
                if (soapHandler.ResponseDecryption(getPatientConsentResponse.SoapProperties, getPatientConsentResponse))
                {
                    patientConsents = getPatientConsentResponse.PatientConsents;
                }
            }
            //}

            gridUser.Columns.Clear();

            objGridCustomControl.SetGridStyle = "grid";
            objGridCustomControl.PageId = "UserAccount";
            objGridCustomControl.GridID = "gridUser";
            objGridCustomControl.IsDelete = true;
            objGridCustomControl.IsUpdate = true;
            objGridCustomControl.EmptyDataText = "No Records To Show";
            gridUser.EmptyDataText = "No Records To Show";

            ArrayList arrColoumn = new ArrayList();

            arrColoumn.Add("PatientConsentID#1#Left");
            arrColoumn.Add("Role#15#Center");

            arrColoumn.Add("Purpose#25#Center");
            arrColoumn.Add("Rule Start Date#10#Center");
            arrColoumn.Add("Rule End Date#10#Center");
            arrColoumn.Add("Permission#10#Center");
            arrColoumn.Add("Active#6#Center");
            arrColoumn.Add("Action#10#Center");

            htColumnName = new SortedList();

            //Control Type have same sequence as you want 
            htControlTye = new SortedList();

            htColumnName.Add(1, "PatientConsentID");
            htColumnName.Add(2, "Role");

            htColumnName.Add(3, "Code");
            htColumnName.Add(4, "RuleStartDate");
            htColumnName.Add(5, "RuleEndDate");
            //htColumnName.Add(6, "Status");
            htColumnName.Add(6, "Allow");
            htColumnName.Add(7, "Active");
            htColumnName.Add(8, "Action");

            htControlTye.Add(1, GridCustomControl.SelectControlType.HiddenField.ToString());
            htControlTye.Add(2, GridCustomControl.SelectControlType.BoundField.ToString());
            htControlTye.Add(3, GridCustomControl.SelectControlType.BoundField.ToString());
            htControlTye.Add(4, GridCustomControl.SelectControlType.Label.ToString());
            htControlTye.Add(5, GridCustomControl.SelectControlType.Label.ToString());
            htControlTye.Add(6, GridCustomControl.SelectControlType.Label.ToString());
            htControlTye.Add(7, GridCustomControl.SelectControlType.Label.ToString());

            htControlTye.Add(8, GridCustomControl.SelectControlType.LinkButton_Edit_Delete.ToString());

            if (patientConsents != null)
            {
                OldDt = patientConsents.ToDataTable();
                //---------------------------------------DataTable,Column Name as in Database table
                NewDt = objGridCustomControl.InsertColumn(OldDt, htColumnName);
                //---------------------------GridView-----------DataTable,Header Name as you want on gridview,ControlType,Page-Path
                objGridCustomControl.LoadData(gridUser, NewDt, htControlTye, arrColoumn);
            }

        }

        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
        finally
        {
            htColumnName = null;
            htControlTye = null;
            objGridCustomControl.Dispose();
            OldDt.Dispose();
            NewDt.Dispose();
        }

    }
    #endregion

    #region btnConfigureConsent_onClick
    protected void btnConfigureConsent_onClick(object sender, EventArgs arg)
    {
        if (rdOptIn.Checked == true)
        {
            GlobalSessions.SessionAdd(SessionItem.IsNew, "true");
            GlobalSessions.SessionRemove(SessionItem.PatientConsentId);
            //GlobalSessions.SessionAdd(SessionItem.PatientConsentId, gridUser.GetKey);
            Response.Redirect(MANAGE_CONSENT_POLICY_PAGE);
        }
    }
    #endregion

    #region btnSave_onClick
    protected void btnSave_onClick(object sender, EventArgs arg)
    {
        UpdateOptInStatusRequest updateOptInStatusRequest = null;
        UpdateOptInStatusResponse updateOptInStatusResponse = null;
        bool ptInStatus = false;
        try
        {
            updateOptInStatusRequest = new UpdateOptInStatusRequest();
            updateOptInStatusResponse = new UpdateOptInStatusResponse();

            btnConfigureConsent.Enabled = true;

            if (rdOptIn.Checked == true)
            {
                ptInStatus = true;
            }
            else if (rdOptOut.Checked == true)
            {
                ptInStatus = false;
            }

            updateOptInStatusRequest.MPIID = GlobalSessions.SessionItem(SessionItem.MPIID).ToString();
            updateOptInStatusRequest.isOptIn = ptInStatus;
            soapHandler.RequestEncryption(updateOptInStatusRequest, out soapProperties);
            updateOptInStatusRequest.SoapProperties = soapProperties;
            updateOptInStatusResponse = objProxy.UpdateOptInStatus(updateOptInStatusRequest);

            if (updateOptInStatusResponse.Result.IsSuccess)
            {
                if (soapHandler.ResponseDecryption(updateOptInStatusResponse.SoapProperties, updateOptInStatusResponse))
                {
                    if (ptInStatus == true && updateOptInStatusResponse.Result.IsSuccess)
                    {
                        GlobalSessions.SessionAdd(SessionItem.IsOptIn, true);
                        rdOptIn.Checked = true;
                        btnConfigureConsent.Enabled = true;
                    }
                    else
                    {
                        GlobalSessions.SessionAdd(SessionItem.IsOptIn, false);
                        rdOptOut.Checked = true;
                        btnConfigureConsent.Enabled = false;
                    }

                    BindGridData();
                }
                else
                {
                    // TO DO:Add error message.
                }
            }
            else
            {
                // TO DO:Add error message.
            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }
    #endregion

    #region rdOptIn_CheckedChanged
    protected void rdOptIn_CheckedChanged(object sender, EventArgs e)
    {
        btnConfigureConsent.Enabled = false;
    }
    #endregion

    #region rdOptOut_CheckedChanged
    protected void rdOptOut_CheckedChanged(object sender, EventArgs e)
    {
        //btnConfigureConsent.Enabled = false;
    }
    #endregion

    #region gridUser_PageIndexChanging
    protected void gridUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridUser.PageIndex = e.NewPageIndex;
            pageindex = Convert.ToInt32(gridUser.PageIndex.ToString());
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    #endregion

}


