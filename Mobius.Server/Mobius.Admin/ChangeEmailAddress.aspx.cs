using System;
using System.Web.UI.WebControls;
using Mobius.BAL;
using System.Collections.Generic;
using Mobius.Entity;
using Mobius.CoreLibrary;

public partial class ChangeAdminemailAddress : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BinduserInformation();
        }
    }

    protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDetails.EditIndex = e.NewEditIndex;
        this.BinduserInformation();
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        AdminDetails adminDetails = new AdminDetails();
        adminDetails.ID = Convert.ToInt32(gvDetails.DataKeys[e.RowIndex].Value.ToString());
        TextBox Email = (TextBox)gvDetails.Rows[e.RowIndex].FindControl("txtEMailAddress");
        adminDetails.Email = Email.Text;
        this.UpdateDetails(adminDetails);
        gvDetails.EditIndex = -1;
        this.BinduserInformation();
    }
    protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDetails.EditIndex = -1;
        this.BinduserInformation();

    }
    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        AdminDetails adminDetails = new AdminDetails();
        adminDetails.ID = Convert.ToInt32(gvDetails.DataKeys[e.RowIndex].Value.ToString());
        this.UpdateDetails(adminDetails);
        this.BinduserInformation();


    }
    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName.Equals("AddNew"))
        //{}
    }

    protected void btnSearch_onClick(object sender, EventArgs arg)
    {
        try
        {
            this.AddAdminDetails(txtMail.Text);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// BinduserInformation
    /// </summary>
    private void BinduserInformation()
    {
        AdminDetails adminDetail = null;
        List<AdminDetails> adminDetails = null;
        try
        {
            MobiusBAL bal = new MobiusBAL();
            Result result = new Result();
            result = bal.GetAdminDetails(adminDetail, out adminDetails);
            if (result.IsSuccess)
            {
                if (adminDetails.Count > 0)
                {
                    gvDetails.DataSource = adminDetails;
                    gvDetails.DataBind();
                }


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    /// <summary>
    /// UpdateDetails
    /// </summary>
    /// <param name="adminDetails"></param>
    private void UpdateDetails(AdminDetails adminDetails)
    {
        try
        {
            MobiusBAL bal = new MobiusBAL();
            Result result = new Result();
            result = bal.UpdateAdminDetails(adminDetails);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Add Admin Details
    /// </summary>
    /// <param name="email"></param>
    private void AddAdminDetails(string email, string password = "")
    {
        try
        {
            MobiusBAL bal = new MobiusBAL();
            Result result = new Result();
            result = bal.AddAdminDetails(email, password);
            if (result.IsSuccess)
            {
                this.BinduserInformation();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}