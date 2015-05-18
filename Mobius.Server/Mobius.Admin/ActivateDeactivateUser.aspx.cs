using System;
using FirstGenesis.UI;
using Mobius.CoreLibrary;
using MobiusServiceLibrary;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using CERTADMINLib;
using Mobius.BAL;
using Mobius.Entity;
public partial class ActivateDeactivateUser : System.Web.UI.Page
{
    #region Variables
    private const string ERROR_MESSAGE = "No result found.";
    private const string REVOKE_ERROR_MESSAGE = "Certificate revocation failed. Unable to deactivate user.";
    private const string USER_CERTIFICATE_NOT_FOUND_MESSAGE = "User certificate not found in the certificate store.";
    private const string ACTIVATE_ERROR_MESSAGE = "User activation failed.";
    private const string SUCCESSFULLY_ACTIVATED_MESSAGE = "User activated successfully.";
    private const string SUCCESSFULLY_DEACTIVATED_MESSAGE = "User deactivated successfully.";
    private const string ACTIVATE_WARNING_MESSAGE = "This account is already active.";
    private const string INACTIVATE_WARNING_MESSAGE = "This account is already inactive.";
    protected MobiusBAL bal = null;
    public string HeaderText = "";
    #endregion

    #region Methods
    protected void Page_Load(object sender, EventArgs e)
    {
        btnRevokeCertificate.Attributes.Add("OnClick", "return CheckMailID('" + hdnSelectedMailID.ClientID + "');");
        btnActivate.Attributes.Add("OnClick", "return CheckMailID('" + hdnSelectedMailID.ClientID + "');");
        btnSearch.Attributes.Add("OnClick", "ClearMessage('" + lblErrorMsg.ClientID + "');");
        txtMail.Focus();
        lblErrorMsg.Text = "";
    }

    protected void btnSearch_onClick(object sender, EventArgs arg)
    {
        try
        {
            bal = new MobiusBAL();
            Result result = new Result();
            List<UserDetails> lstUserDetails = new List<UserDetails>();
            UserDetails userDetails = new UserDetails();
            hdnSelectedMailID.Value = "";
            hdnCertInfo.Value = "";
            btnRevokeCertificate.Visible = false;
            btnActivate.Visible = false;
            userDetails.EmailAddress = txtMail.Text;
            userDetails.UserType = (UserType)Enum.Parse(typeof(UserType), rbtPatient.Checked ? UserType.Patient.ToString() : UserType.Provider.ToString(), true);
            lstUserDetails.Add(userDetails);
            result = bal.GetUserDetials(ref lstUserDetails);
            lblErrorMsg.Text = "";
            if (!result.IsSuccess)
            {
                lblErrorMsg.Text = ERROR_MESSAGE;
                BindGrid(new DataTable());
                return;
            }
            else
            {
                if (lstUserDetails.Count <= 0)
                {
                    BindGrid(new DataTable());
                    lblErrorMsg.Text = ERROR_MESSAGE;
                    return;
                }
                DataTable dtUserdata = CreateDataTableFromCollection(lstUserDetails);
                if (dtUserdata.Rows.Count > 0)
                {
                    btnRevokeCertificate.Visible = true;
                    btnActivate.Visible = true;
                }
                BindGrid(dtUserdata);
            }

            bal = null;
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void GridUserInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RadioButton chkBox = (RadioButton)e.Row.Cells[0].FindControl("chkSelected");
                if (chkBox != null)
                {
                    chkBox.Attributes.Add("OnClick", " GetSelectedMailID('" + GridUserInfo.DataKeys[e.Row.RowIndex].Values[0] + "','" + hdnSelectedMailID.ClientID + "','" + chkBox.ClientID + "','" + hdnCertInfo.ClientID + "','" + GridUserInfo.DataKeys[e.Row.RowIndex].Values[1].ToString().Trim() + "','" + GridUserInfo.DataKeys[e.Row.RowIndex].Values[2].ToString().Trim() + "','" + GridUserInfo.DataKeys[e.Row.RowIndex].Values[3].ToString().Trim() + "')");
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = HeaderText;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void btnRevokeCertificate_Click(object sender, EventArgs e)
    {
        X509Store certificateStore = null;
        X509Certificate2Collection certCollection = null;
        CCertAdmin certAdmin = null;

        try
        {
            Result result = new Result();
            bal = new MobiusBAL();
            if (hdnStatus.Value.Trim().ToLower() != "Active".ToLower())
            {
                lblErrorMsg.Text = INACTIVATE_WARNING_MESSAGE;
                return;
            }

            certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certAdmin = new CCertAdmin();
            certificateStore.Open(OpenFlags.ReadWrite);
            string serialNumber = hdnCertInfo.Value;
            string selectedMailAddress = hdnSelectedMailID.Value;
            certCollection = certificateStore.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);
            //if (certCollection.Count > 0)
            //{
            try
            {

                certAdmin.RevokeCertificate(MobiusAppSettingReader.CertificateAuthorityServerURL, serialNumber, 6, DateTime.Now);
            }
            catch{}
            UserType userType = (UserType)Enum.Parse(typeof(UserType), rbtPatient.Checked ? UserType.Patient.ToString() : UserType.Provider.ToString(), true);
            result = bal.UpdateUserStatus(selectedMailAddress, userType.GetHashCode(), false, "");
            if (!result.IsSuccess)
            {
                lblErrorMsg.Text = REVOKE_ERROR_MESSAGE;
                return;
            }
            else
            {
                btnSearch_onClick(sender, e);
                lblErrorMsg.Text = SUCCESSFULLY_DEACTIVATED_MESSAGE;
            }
            //}
            //else
            //{
            //    lblErrorMsg.Text = USER_CERTIFICATE_NOT_FOUND_MESSAGE;
            //}

        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = REVOKE_ERROR_MESSAGE;
        }
        finally
        {
            certificateStore = null;
            certCollection = null;
            certAdmin = null;
            bal = null;
        }

    }

    protected void chkSelected1_CheckedChanged(object sender,EventArgs e)
    {
        foreach (GridViewRow oldrow in GridUserInfo.Rows)
        {
            ((RadioButton)oldrow.FindControl("chkSelected")).Checked = false;
        }

        RadioButton rb = (RadioButton)sender;
        GridViewRow row = (GridViewRow)rb.NamingContainer;
        ((RadioButton)row.FindControl("chkSelected")).Checked = true;
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        try
        {
            Result result = new Result();
            bal = new MobiusBAL();
            string userName = string.Empty;
            if (hdnStatus.Value.Trim().ToLower() == "Active".ToLower())
            {
                lblErrorMsg.Text = ACTIVATE_WARNING_MESSAGE;
                return;
            }

            UserType userType = (UserType)Enum.Parse(typeof(UserType), rbtPatient.Checked ? UserType.Patient.ToString() : UserType.Provider.ToString(), true);
            result = bal.UpdateUserStatus(hdnSelectedMailID.Value, userType.GetHashCode(), true, hdnUserName.Value);
            if (!result.IsSuccess)
            {
                lblErrorMsg.Text = ACTIVATE_ERROR_MESSAGE;
                return;
            }
            else
            {
                btnSearch_onClick(sender, e);
                lblErrorMsg.Text = SUCCESSFULLY_ACTIVATED_MESSAGE;
            }

        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ACTIVATE_ERROR_MESSAGE;
        }
        finally
        {
            bal = null;
        }
    }
    #endregion

    #region Private Methods
    private void BindGrid(DataTable dtUserInfo)
    {
        GridUserInfo.DataSource = dtUserInfo;
        GridUserInfo.DataBind();
    }

    private DataTable CreateDataTableFromCollection(List<UserDetails> userInfo)
    {
        using (DataTable dtUserInfo = new DataTable())
        {
            DataColumn emailAddress = new DataColumn("EmailAddress", typeof(string));
            DataColumn userType = new DataColumn("UserType", typeof(string));
            DataColumn FamilyName = new DataColumn("FamilyName", typeof(string));
            DataColumn GivenName = new DataColumn("GivenName", typeof(string));
            DataColumn city = new DataColumn("City", typeof(string));
            DataColumn state = new DataColumn("State", typeof(string));
            DataColumn postalCode = new DataColumn("PostalCode", typeof(string));
            DataColumn certSerialNumber = new DataColumn("SerialNumber", typeof(string));
            DataColumn status = new DataColumn("Status", typeof(string));

            dtUserInfo.Columns.Add(emailAddress);
            dtUserInfo.Columns.Add(userType);
            dtUserInfo.Columns.Add(FamilyName);
            dtUserInfo.Columns.Add(GivenName);
            dtUserInfo.Columns.Add(city);
            dtUserInfo.Columns.Add(state);
            dtUserInfo.Columns.Add(postalCode);
            dtUserInfo.Columns.Add(certSerialNumber);
            dtUserInfo.Columns.Add(status);
            DataRow dr;
            for (int count = 0; count < userInfo.Count; count++)
            {
                dr = dtUserInfo.NewRow();
                dr["EmailAddress"] = userInfo[count].EmailAddress;
                dr["UserType"] = rbtPatient.Checked ? "Patient" : "Provider";
                dr["FamilyName"] =  userInfo[count].FamilyName;
                if (string.IsNullOrWhiteSpace(userInfo[count].FamilyName))
                {
                    HeaderText = "Organization Name";
                }
                else
                {
                    HeaderText = "User Name";
                }
                dr["GivenName"] = userInfo[count].GivenName;
                dr["City"] = userInfo[count].City;
                dr["State"] = userInfo[count].State;
                dr["PostalCode"] = userInfo[count].PostalCode;
                dr["SerialNumber"] = userInfo[count].SerialNumber;
                dr["Status"] = userInfo[count].Status == "True" ? "Active" : "Inactive";
                dtUserInfo.Rows.Add(dr);
            }
            return dtUserInfo;
        }


    }
    #endregion


}