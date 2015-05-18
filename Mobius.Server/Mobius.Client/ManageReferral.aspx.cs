using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MobiusServiceLibrary;
using FirstGenesis.UI;
using FirstGenesis.UI.Base;
using MobiusServiceUtility;

public partial class ManageReferrals : BaseClass
{
    #region Private properties
    SoapHandler soapHandler;
    SoapProperties soapProperties = new SoapProperties();
    string loggedInUserEmail = string.Empty;
    List<MobiusServiceLibrary.PatientReferral> MyReferrals = null;
    List<MobiusServiceLibrary.PatientReferral> ReferralsByMe = null;

    public bool HasBlankRow { get; set; }

    #endregion Private properties

    #region Event Handlers
    /// <summary>
    /// This method will occur at page load to populate the grid.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">e</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            if (GlobalSessions.SessionItem(SessionItem.UserEmailAddress) != null)
            {
                loggedInUserEmail = GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString();
            }

            if (!Page.IsPostBack)
            {
                //Populate referrals assigned to logged in user.
                GetReferralsForMe();

                //Populate referrals initiated by logged in user.
                GetReferredByMe();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// This method will load at page indexing change for referral records.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridReferral_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            if (GlobalSessions.SessionItem(SessionItem.MyReferrals) != null)
            {
                this.MyReferrals = (List<PatientReferral>)GlobalSessions.SessionItem(SessionItem.MyReferrals);
                gridReferral.DataSource = this.MyReferrals;
                gridReferral.PageIndex = e.NewPageIndex;
                gridReferral.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// This method will load and map the data for the changing page index.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvReferredByMe_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            if (GlobalSessions.SessionItem(SessionItem.ReferralsByMe) != null)
            {
                this.ReferralsByMe = (List<PatientReferral>)GlobalSessions.SessionItem(SessionItem.ReferralsByMe);
                gvReferredByMe.DataSource = this.ReferralsByMe;
                gvReferredByMe.PageIndex = e.NewPageIndex;
                gvReferredByMe.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// This method will load row bind for referral records.
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">e</param>
    protected void GridReferral_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnDetails = (ImageButton)e.Row.FindControl("ibtnDetails");
                
                if (this.MyReferrals[e.Row.RowIndex] != null)
                {
                    Patient patient = this.MyReferrals[e.Row.RowIndex].Patient;

                    string firstName = string.Empty;
                    string lastName = string.Empty;

                    if (patient.GivenName.Count >= 1)
                    {
                        firstName = patient.GivenName[0].ToString();
                    }

                    if (patient.GivenName.Count >= 1)
                    {
                        lastName = patient.FamilyName[0].ToString();
                    }
                    e.Row.Cells[3].Text = firstName + " " + lastName;
                    e.Row.Cells[4].Text = string.IsNullOrEmpty (patient.DOB) ? "" : Convert.ToDateTime(patient.DOB).ToShortDateString();
                    
                    e.Row.Cells[5].Text = patient.Gender== Mobius.CoreLibrary.Gender.Unspecified ? "" : patient.Gender.ToString();
                    e.Row.Cells[6].Text = this.MyReferrals[e.Row.RowIndex].ReferralOn == null ? "" : Convert.ToDateTime(this.MyReferrals[e.Row.RowIndex].ReferralOn).ToShortDateString();

                    e.Row.Cells[7].Text = this.MyReferrals[e.Row.RowIndex].PatientAppointmentDate == null ? "" : Convert.ToDateTime(this.MyReferrals[e.Row.RowIndex].PatientAppointmentDate).ToShortDateString();
                    e.Row.Cells[8].Text = this.MyReferrals[e.Row.RowIndex].ReferralAccomplishedOn == null ? "" : Convert.ToDateTime(this.MyReferrals[e.Row.RowIndex].ReferralAccomplishedOn).ToShortDateString();
                    
                }

                if (ibtnDetails != null)
                {
                    ibtnDetails.CommandName = "Details";
                    ibtnDetails.CommandArgument = e.Row.RowIndex.ToString();
                }

                if (!this.HasBlankRow)
                {
                    ibtnDetails.Visible = true;
                }
                else if (this.HasBlankRow)
                {
                    ibtnDetails.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// This method will bind rows for referral that has been initiated by me.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvReferredByMe_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnDetails = (ImageButton)e.Row.FindControl("ibtnDetails");

                if (this.ReferralsByMe[e.Row.RowIndex] != null)
                {
                    Patient patient = this.ReferralsByMe[e.Row.RowIndex].Patient;

                    string firstName = string.Empty;
                    string lastName = string.Empty;

                    if (patient.GivenName.Count >= 1)
                    {
                        firstName = patient.GivenName[0].ToString();
                    }

                    if (patient.GivenName.Count >= 1)
                    {
                        lastName = patient.FamilyName[0].ToString();
                    }
                    e.Row.Cells[3].Text = firstName + " " + lastName;
                    e.Row.Cells[4].Text = string.IsNullOrEmpty(patient.DOB) ? "" : Convert.ToDateTime(patient.DOB).ToShortDateString();

                    e.Row.Cells[5].Text = patient.Gender == Mobius.CoreLibrary.Gender.Unspecified ? "" : patient.Gender.ToString();

                    e.Row.Cells[6].Text = this.ReferralsByMe[e.Row.RowIndex].ReferralOn == null ? "" : Convert.ToDateTime(this.ReferralsByMe[e.Row.RowIndex].ReferralOn).ToShortDateString();

                    e.Row.Cells[7].Text = this.ReferralsByMe[e.Row.RowIndex].PatientAppointmentDate == null ? "" : Convert.ToDateTime(this.ReferralsByMe[e.Row.RowIndex].PatientAppointmentDate).ToShortDateString();
                    e.Row.Cells[8].Text = this.ReferralsByMe[e.Row.RowIndex].ReferralAccomplishedOn == null ? "" : Convert.ToDateTime(this.ReferralsByMe[e.Row.RowIndex].ReferralAccomplishedOn).ToShortDateString();

                }

                if (ibtnDetails != null)
                {
                    ibtnDetails.CommandName = "Details";
                    ibtnDetails.CommandArgument = e.Row.RowIndex.ToString();
                }

                if (!this.HasBlankRow)
                {
                    ibtnDetails.Visible = true;
                }
                else if (this.HasBlankRow)
                {
                    ibtnDetails.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// This method will occur at row command of the grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridReferral_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {   
            if (e.CommandName == "Details")
            {
                int iselectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = gridReferral.Rows[iselectedIndex];
                string patientReferralID = ((Label)selRow.FindControl("ID")).Text;
                string documentID = ((Label)selRow.FindControl("DocID")).Text;
                Response.Redirect("ReferPatient.aspx?PatientReferralID=" + patientReferralID + "&DocumentID=" + documentID);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void gvReferredByMe_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Details")
            {
                int iselectedIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow selRow = gvReferredByMe.Rows[iselectedIndex];
                string patientReferralID = ((Label)selRow.FindControl("ID")).Text;
                string documentID = ((Label)selRow.FindControl("DocID")).Text;
                Response.Redirect("ReferPatient.aspx?PatientReferralID=" + patientReferralID + "&DocumentID=" + documentID);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void chkAllReferralsForMe_OnCheckedChanged(object sender, EventArgs e)
    {
        GetReferralsForMe();
    }


    protected void chkAllReferralsByMe_CheckedChanged(object sender, EventArgs e)
    {
        GetReferredByMe();
    }

    #endregion Event Handlers

    #region Private Methods

    /// <summary>
    /// This method gets referrals available/assigned to logged-in user.
    /// </summary>
    private void GetReferralsForMe()
    {
        GetPatientReferralRequest getPatientReferralRequest = new GetPatientReferralRequest();
        GetPatientReferralResponse getPatientReferralResponse = new GetPatientReferralResponse();
        getPatientReferralRequest.patientReferralId = 0;
        getPatientReferralRequest.referredToEmailAddress = this.loggedInUserEmail;

        soapHandler.RequestEncryption(getPatientReferralRequest, out soapProperties);
        getPatientReferralRequest.SoapProperties = soapProperties;
        getPatientReferralResponse = objProxy.GetPatientReferralDetails(getPatientReferralRequest);
        MyReferrals = getPatientReferralResponse.PatientReferrals;

        List<MobiusServiceLibrary.PatientReferral> patientReferrals;

        if (soapHandler.ResponseDecryption(getPatientReferralResponse.SoapProperties, getPatientReferralResponse))
        {
            if (chkAllReferralsForMe.Checked)
            {
                patientReferrals = getPatientReferralResponse.PatientReferrals;
            }
            else
            {
                patientReferrals = getPatientReferralResponse.PatientReferrals.Where(t => t.ReferralCompleted == false).ToList();
            }

            if (getPatientReferralResponse.Result.IsSuccess)
            {
                this.FillMyReferralsGrid(patientReferrals);
            }
            else
            {
                this.FillMyReferralsGrid(null);
                lblErrorMsg.Text = getPatientReferralResponse.Result.ErrorMessage;
            }
        }
        else
        {
            this.FillMyReferralsGrid(null);
            lblErrorMsg.Text = "Invalid response from service.";
        }
    }

    /// <summary>
    /// This method gets referrals created by me.
    /// </summary>
    private void GetReferredByMe()
    {
        GetPatientReferralRequest getPatientReferralRequest = new GetPatientReferralRequest();
        GetPatientReferralResponse getPatientReferralResponse = new GetPatientReferralResponse();
        getPatientReferralRequest.patientReferralId = 0;
        getPatientReferralRequest.referredByEmailAddress = this.loggedInUserEmail;
        
        //TODO: Need to change the implementation : This is DUMMY value
        soapHandler.RequestEncryption(getPatientReferralRequest, out soapProperties);
        getPatientReferralRequest.SoapProperties = soapProperties;
        getPatientReferralResponse = objProxy.GetPatientReferralDetails(getPatientReferralRequest);
        MyReferrals = getPatientReferralResponse.PatientReferrals;

        if (soapHandler.ResponseDecryption(getPatientReferralResponse.SoapProperties, getPatientReferralResponse))
        {
            List<MobiusServiceLibrary.PatientReferral> patientReferrals;
            if (chkAllReferralsByMe.Checked)
            {
                patientReferrals = getPatientReferralResponse.PatientReferrals;
            }
            else
            {
                patientReferrals = getPatientReferralResponse.PatientReferrals.Where(t => t.ReferralCompleted == false).ToList();
            }

            if (getPatientReferralResponse.Result.IsSuccess)
            {
                this.FillReferralsByMeGrid(patientReferrals);
            }
            else
            {
                this.FillReferralsByMeGrid(null);
                lblErrorMsg.Text = getPatientReferralResponse.Result.ErrorMessage;
            }
        }
        else
        {
            this.FillReferralsByMeGrid(null);
            lblErrorMsg.Text = "Invalid response from service.";
        }

    }


    /// <summary>
    /// This method will fill the grid at page load for referral records that is assgined to logged in user.
    /// </summary>
    /// <param name="patientReferrals">List of patient referral records assigned to logged in user.</param>
    /// <returns>none</returns>
    private void FillMyReferralsGrid(List<MobiusServiceLibrary.PatientReferral> patientReferrals)
    {
        if (patientReferrals == null || patientReferrals.Count <=0)
        {
            patientReferrals = new List<PatientReferral>();
            PatientReferral dummyPatientReferral = new PatientReferral();
            patientReferrals.Add(dummyPatientReferral);
            this.HasBlankRow = true;
        }

        this.MyReferrals = patientReferrals;
        GlobalSessions.SessionAdd(SessionItem.MyReferrals, this.MyReferrals);
        gridReferral.DataSource = patientReferrals; 
        gridReferral.DataBind();
        gridReferral.PageIndex = 0;
    }

    /// <summary>
    /// This method will fill the grid at page load for referral records that is assgined to logged in user.
    /// </summary>
    /// <param name="patientReferrals">List of patient referral records assigned to logged in user.</param>
    /// <returns>none</returns>
    private void FillReferralsByMeGrid(List<MobiusServiceLibrary.PatientReferral> patientReferrals)
    {
        if (patientReferrals == null || patientReferrals.Count <= 0)
        {
            patientReferrals = new List<PatientReferral>();
            PatientReferral dummyPatientReferral = new PatientReferral();
            patientReferrals.Add(dummyPatientReferral);
            this.HasBlankRow = true;
        }

        this.ReferralsByMe = patientReferrals;
        GlobalSessions.SessionAdd(SessionItem.ReferralsByMe, this.ReferralsByMe);
        gvReferredByMe.DataSource = patientReferrals;
        gvReferredByMe.DataBind();
        gvReferredByMe.PageIndex = 0;
    }


    #endregion Private Methods

 
}