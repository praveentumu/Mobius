using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI.Base;
using FirstGenesis.Mobius.Logging;
using Mobius.CoreLibrary;
using System.Collections.Generic;
using Mobius.Entity;
using MobiusServiceLibrary;
using System.Linq;
using FirstGenesis.UI;
using MobiusServiceUtility;

public partial class SearchUser : BaseClass
{

    #region Variables
    /// <summary>
    /// dt variable keeps the value stored in (DataTable)Session["SearchData"];
    /// </summary>
    List<MobiusServiceLibrary.Patient> patients = null;

    /// <summary>
    /// Creating object of Logger class.
    /// </summary>
    Logger logger = Logger.GetInstance();

    /// <summary>
    /// FacilityType is a string type variable used to save the facility name from a session object.
    /// </summary>
    string facilityType = String.Empty;

    /// <summary>
    /// Creating object of soapHandler class.
    /// </summary>
    SoapHandler soapHandler;

    private const string NO_RESULT_FOUND = "No Result Found.";
    private const string EXCEPTION_OCCURRED = "Exception Occurred:";
    private const string DOCUMENTLIST_PAGE = "DocumentList.aspx";
    private const string COMMUNITY_DESCRIPTION = "CommunityDescription";
    private const string COMMUNITY_IDENTIFIER = "CommunityIdentifier";
    private const string CHECK_ALL = "Check All";
    #endregion

    #region Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            if (GlobalSessions.SessionItem(SessionItem.FacilityType) != null)
            {
                this.facilityType = GlobalSessions.SessionItem(SessionItem.FacilityType).ToString();
            }

            if (!IsPostBack)
            {
                /// <summary>
                /// To populate  the list box collection from backend on page load for first time. 
                /// User select the community/communities to search+.
                /// </summary>
                GetNhinCommunityResponse nhinCommunityResponse = new GetNhinCommunityResponse();
                if (GlobalSessions.SessionItem(SessionItem.SearchData) != null)
                {
                    GlobalSessions.SessionAdd(SessionItem.SearchData, null);

                }
                trNext.Visible = false;
                trNextTop.Visible = false;
                hdnSelected.Value = "N";
                nhinCommunityResponse = objProxy.GetNhinCommunity();
                if (nhinCommunityResponse.Result.IsSuccess)
                {
                    if (soapHandler.ResponseDecryption(nhinCommunityResponse.SoapProperties, nhinCommunityResponse))
                    {
                        if (nhinCommunityResponse.Communities != null)
                        {
                            List<NHINCommunity> nhinCommunities = new List<NHINCommunity>(nhinCommunityResponse.Communities);
                            this.FillNHINCommunities(nhinCommunities);
                            nhinCommunities = null;
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

    /// <summary>
    /// Handles the PageIndexChanging event of the gridPatients control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
    protected void gridPatients_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (GlobalSessions.SessionItem(SessionItem.SearchData) != null)
            {
                gridPatients.PageIndex = e.NewPageIndex;
                this.BindPatientGrid((List<MobiusServiceLibrary.Patient>)GlobalSessions.SessionItem(SessionItem.SearchData));
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// Handles the Click event of the btnSearch control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            SearchPatientRequest searchPatientRequest = new SearchPatientRequest();
            SearchPatientResponse searchPatientResponse = new SearchPatientResponse();
            SoapProperties soapProperties = new SoapProperties();          

            gridPatients.Visible = true;

            if (txtPatientId.Text.Trim() != null)
            {
                GlobalSessions.SessionAdd(SessionItem.MPIID, txtPatientId.Text.Trim());

            }

            searchPatientRequest.Demographics = new MobiusServiceLibrary.Demographics();
            searchPatientRequest.Demographics.GivenName = txtFirstName.Text.Trim();
            searchPatientRequest.Demographics.FamilyName = txtLastName.Text.Trim();
            searchPatientRequest.Demographics.DOB = txtDOB.Text.Trim();
            //searchPatientRequest.Demographics.SSN = txtSSN.Text.Trim();
            if (!string.IsNullOrEmpty(txtPatientId.Text.Trim()))
            {
                searchPatientRequest.Demographics.LocalMPIID = txtPatientId.Text.Trim();
            }

            searchPatientRequest.Demographics.Gender = (Gender)Enum.Parse(typeof(Gender), radio1.SelectedValue, true);

            List<Community> Communities = new List<Community>();
            Community community = new Community();
            List<NHINCommunity> nhinCommunities = new List<NHINCommunity>();
            IEnumerable<NHINCommunity> nhinCommunity;
            List<NHINCommunity> nhinCommunitiesSession = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);

            for (int i = 1; i < lbCommunities.Items.Count; i++)
            {

                if (lbCommunities.Items[i].Selected)
                {
                    nhinCommunity = nhinCommunitiesSession.Where(t => t.CommunityDescription == lbCommunities.Items[i].Text);
                    if (nhinCommunity.Count() > 0)
                    {
                        nhinCommunities.Add(nhinCommunity.FirstOrDefault());
                    }
                }

            }
            foreach (var nHINCommunity in nhinCommunities)
            {
                community = new Community();
                community.CommunityIdentifier = nHINCommunity.CommunityIdentifier;
                Communities.Add(community);
            }


            searchPatientRequest.NHINCommunities = Communities;            
            NHINCommunity homeNHINCommunity = nhinCommunitiesSession.Where(t => t.IsHomeCommunity).FirstOrDefault();            
            AssertionHelper assertionHelper = new AssertionHelper();
            searchPatientRequest.Assertion = assertionHelper.CreateAssertion(AssertionMode.Default, AssertionAction.PatientDiscovery, PurposeOfUse.TREATMENT,base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);


            soapHandler.RequestEncryption(searchPatientRequest, out soapProperties);
            searchPatientRequest.SoapProperties = soapProperties;
            searchPatientResponse = objProxy.SearchPatient(searchPatientRequest);
            if (searchPatientResponse.Result.IsSuccess)
            {
                if (soapHandler.ResponseDecryption(searchPatientResponse.SoapProperties, searchPatientResponse))
                {
                    if (searchPatientResponse.Patients.Count == 0)
                    {

                        lblErrorMsg.Text = NO_RESULT_FOUND;
                        gridPatients.Visible = false;
                        trNext.Visible = false;
                        trNextTop.Visible = false;
                        return;

                    }
                    else
                    {

                        lblErrorMsg.Text = String.Empty;
                        GlobalSessions.SessionAdd(SessionItem.SelectedPatient, searchPatientResponse.Patients.First());
                        //patients = this.CreateDataTableFromPersonObject(searchPatientResponse.Patients.ToArray());
                        this.BindPatientGrid(searchPatientResponse.Patients);
                        GlobalSessions.SessionAdd(SessionItem.SearchData, searchPatientResponse.Patients);
                    }
                }
                else
                {
                    lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                    gridPatients.Visible = false;
                    trNext.Visible = false;
                    trNextTop.Visible = false;
                    return;
                }
            }
            else
            {
                lblErrorMsg.Text = searchPatientResponse.Result.ErrorMessage;
                gridPatients.Visible = false;
                trNext.Visible = false;
                trNextTop.Visible = false;
                return;
            }

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    /// <summary>
    /// Handles the RowDataBound event of the gridPatients control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The GridViewRowEventArgs instance containing the event data.</param>
    protected void gridPatients_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    RadioButton rdSelect = (RadioButton)e.Row.FindControl("rdSelect");
                    e.Row.Cells[1].Text = this.patients[e.Row.RowIndex].GivenName[0];
                    e.Row.Cells[2].Text = this.patients[e.Row.RowIndex].FamilyName[0];
                    if (rdSelect != null)
                    {
                        string strFirstName = e.Row.Cells[1].Text;
                        string strDob = Convert.ToString(e.Row.Cells[3].Text);
                        string separataor = string.Empty;
                        separataor = "/";
                        string yyyy = strDob.Substring(0, 4);
                        string mm = strDob.Substring(4, 2);
                        string dd = strDob.Substring(6, 2);
                        strDob = mm + separataor + dd + separataor + yyyy;
                        e.Row.Cells[3].Text = strDob;
                        string strSSN = string.Empty;
                        string Gender = e.Row.Cells[4].Text;
                        string strLastName = e.Row.Cells[2].Text;


                        rdSelect.Attributes.Clear();
                        rdSelect.Attributes.Add("onclick", "javascript:return GetSelected('" + rdSelect.ClientID + "','" + e.Row.RowIndex + "');");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }

    /// <summary>
    /// Handles the Click event of the lbtnNext control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void lbtnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnSelected.Value.ToUpper() == "Y")
            {
                this.SetSelectedItemSession();
                Response.Redirect(DOCUMENTLIST_PAGE, false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(SearchUser), "View Documents", "alert('Please select patient to continue');", true);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
   

    /// <summary>
    /// cancels the search operation.
    /// </summary>
    /// <param name="sender"> default instance of Object class ie sender</param>
    /// <param name="e">default instance of EventArgs class ie e</param>
    protected void CancelSearch(object sender, EventArgs e)
    {
        try
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPatientId.Text = "";
            txtDOB.Text = "";
            lblErrorMsg.Text = String.Empty;

            gridPatients.DataSource = null;
            gridPatients.DataBind();

            option1.Selected = true;
            trNext.Visible = false;
            trNextTop.Visible = false;
            hdnSelected.Value = "N";
            for (int count = 0; count < lbCommunities.Items.Count; count++)
            {
                if (lbCommunities.Items[count].Selected)
                {
                    lbCommunities.Items[count].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Binds the patient grid.
    /// </summary>
    /// <param name="dtPatient">The dt patient.</param>
    private void BindPatientGrid(List<MobiusServiceLibrary.Patient> patients)
    {
        this.patients = patients;
        gridPatients.Visible = true;
        gridPatients.DataSource = patients;
        gridPatients.DataBind();
        if (patients.Count > 0)
        {
            trNext.Visible = true;
            trNextTop.Visible = true;
        }
        else
        {
            trNext.Visible = false;
            trNext.Visible = false;
        }
        hdnSelected.Value = "N";
    }

    /// <summary>
    /// Sets the selected item session.
    /// </summary>
    private void SetSelectedItemSession()
    {
        GridViewRow selRow = gridPatients.Rows[Convert.ToInt32(hdnRowIndex.Value)];
        GlobalSessions.SessionAdd(SessionItem.MPIID, gridPatients.DataKeys[0].Value);
        List<string> PatientName = new List<string>();
        PatientName.Add(selRow.Cells[1].Text);
        PatientName.Add(selRow.Cells[2].Text);

        GlobalSessions.SessionAdd(SessionItem.SelectedPatientName, PatientName);

        GlobalSessions.SessionAdd(SessionItem.PatientGender, selRow.Cells[4].Text);
        GlobalSessions.SessionAdd(SessionItem.PatientDOB, Convert.ToDateTime(selRow.Cells[3].Text));
    }

    /// <summary>
    /// Converts the Patient object to data table.
    /// </summary>
    /// <param name="Patients"> Array of patinets is returned from service, which get converted to datatable</param>
    /// <returns> datatable object </returns>
    private DataTable CreateDataTableFromPersonObject(MobiusServiceLibrary.Patient[] patients)
    {
        DataTable dtPatient = new DataTable();
        DataColumn dob = new DataColumn("DOB", typeof(string));
        DataColumn given = new DataColumn("GivenName", typeof(string));
        DataColumn family = new DataColumn("FamilyName", typeof(string));
        DataColumn gender = new DataColumn("Gender", typeof(string));
        DataColumn communityID = new DataColumn("CommunityID", typeof(string));
        DataColumn mpiID = new DataColumn("MPIID", typeof(string));

        dtPatient.Columns.Add(dob);
        dtPatient.Columns.Add(given);
        dtPatient.Columns.Add(family);
        dtPatient.Columns.Add(gender);
        dtPatient.Columns.Add(communityID);
        dtPatient.Columns.Add(mpiID);

        int i = 0;
        DataRow[] row = new DataRow[patients.Length];

        foreach (MobiusServiceLibrary.Patient patient in patients)
        {
            for (i = 0; i < row.Length; i++)
            {
                row[i] = dtPatient.NewRow();
                row[i]["GivenName"] = patient.GivenName[0];
                row[i]["FamilyName"] = patient.FamilyName[0];
                row[i]["DOB"] = patient.DOB;
                row[i]["Gender"] = patient.Gender;
                row[i]["CommunityID"] = patient.CommunityId;
                row[i]["MPIID"] = patient.LocalMPIID;
                dtPatient.Rows.Add(row[i]);
                i++;
                break;
            }

        }
        return dtPatient;
    }

    /// <summary>
    /// list of NHIN communities is passed as parameter.
    /// </summary>
    /// <param name="NHINCommunities">NHINCommunities which is generic list of type NHINCommunity </param>
    private void FillNHINCommunities(List<NHINCommunity> nhinCommunities)
    {
        if (nhinCommunities == null)
        {
            nhinCommunities = new List<NHINCommunity>();
        }
        NHINCommunity NHINCommunity = new NHINCommunity();
        NHINCommunity.CommunityDescription = CHECK_ALL;
        NHINCommunity.CommunityIdentifier = "-1";
        NHINCommunity.IsHomeCommunity = false;
        nhinCommunities.Insert(0, NHINCommunity);
        lbCommunities.DataSource = nhinCommunities;
        lbCommunities.DataTextField = COMMUNITY_DESCRIPTION;
        lbCommunities.DataValueField = COMMUNITY_IDENTIFIER;
        lbCommunities.DataBind();
        GlobalSessions.SessionAdd(SessionItem.CommunityList, nhinCommunities);
        NHINCommunity = null;
    }

    #endregion
}


