using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.Mobius.Logging;
using FirstGenesis.UI;
using Mobius.BAL;
using Mobius.CoreLibrary;
using Mobius.Entity;

public partial class ImportCommunities : System.Web.UI.Page
{

    #region Variables
    List<MobiusNHINCommunity> Communities;
    List<MobiusNHINCommunity> nhincommunity = new List<MobiusNHINCommunity>();
    protected MobiusBAL MobiusBAL = null;
    protected const string CSV = ".csv";
    protected const string XLS = ".xls";
    protected const string XLSX = ".xlsx";
    protected const string TRUE = "true";
    protected const string YES = "Yes";
    protected const string NO = "No";
    protected const string COMMUNITY_IDENTIFIER = "communityidentifier";
    protected const string COMMUNITY_DESCRIPTION = "communitydescription";
    protected const string ACTIVE = "active";
    protected const string XLS_PROVIDER = "Provider=Microsoft.Jet.OLEDB.4.0;"; 
    protected const string XLSX_PROVIDER = "Provider=Microsoft.ACE.OLEDB.12.0;";
    protected const string EXCEL_PROPERTIES = "Extended Properties=Excel 8.0;";
    protected const string DATA_SOURCE = "Data Source=";
    protected const string ONE = "1";
    protected const string TEMPLATE_PATH_XLSX = "~/Templates/Import Communities.xlsx";
    protected const string TEMPLATE_PATH_CSV = "~/Templates/Import Communities.csv";
    #endregion Variables

    public bool HasBlankRow { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Redirect("Default.aspx", false);

        MobiusBAL = new MobiusBAL();
        lblErrorMsg.Text = string.Empty;

        if (!Page.IsPostBack)
        {
            ShowBlankGrid();
        }
    }


    protected void ShowBlankGrid()
    {
        try
        {
            grdCommunityUpload.Visible = true;
            grdCommunityUpload.DataSource = this.BindBlankGrid();
            grdCommunityUpload.DataBind();
            btnImport.Visible = false;

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected List<MobiusNHINCommunity> BindBlankGrid()
    {
        List<MobiusNHINCommunity> nhinCommunity = new List<MobiusNHINCommunity>();
        MobiusNHINCommunity Community = new MobiusNHINCommunity();
        nhinCommunity.Add(Community);
        this.HasBlankRow = true;
        return nhinCommunity;
    }

    protected void btnShowCommunity_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdExcel.Checked)
            {
                ShowExcelFileData();
            }
            else
            {
                ShowCsvFileData();
            }
         
            if (grdCommunityUpload.Rows.Count > 0)
            {
               
                btnImport.Visible = true;
            }
            else
            {
                ShowBlankGrid();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }  

    #region ShowExcelFileData


    protected void ShowExcelFileData()
    {
        try
        {
            grdCommunityUpload.DataSource = null;
            grdCommunityUpload.DataBind();
            if (fileuploadExcel.HasFile)
            {
                if (fileuploadExcel.FileContent.Length > 0)
                {
                    string Foldername = string.Empty, filePath = string.Empty; 
                    string Extension = System.IO.Path.GetExtension(fileuploadExcel.PostedFile.FileName);
                    string filename = Path.GetFileName(fileuploadExcel.PostedFile.FileName.ToString());
                    if (Extension.ToLower() == XLS || Extension.ToLower() == XLSX)
                    {
                        Foldername = Path.GetTempPath(); 
                        string newid = Guid.NewGuid().ToString();
                        filePath = Foldername + newid + filename.ToString();
                        fileuploadExcel.PostedFile.SaveAs(filePath);
                        String _connectionStr = string.Empty;
                        switch (Extension)
                        {
                            case XLS: //Excel 97-03
                                _connectionStr = XLS_PROVIDER + DATA_SOURCE + Foldername + "//" + newid + filename + ";" + EXCEL_PROPERTIES;
                                break;

                            case XLSX: //Excel 07
                                _connectionStr = XLSX_PROVIDER + DATA_SOURCE + Foldername + "//" + newid + filename + ";" + EXCEL_PROPERTIES;
                                break;
                        }
                        OleDbConnection conn = new OleDbConnection(_connectionStr);
                        OleDbDataReader odr = null;
                        try
                        {
                            OleDbCommand ocmd = new OleDbCommand("select CommunityIdentifier,CommunityDescription,Active from [Import Communities$]", conn);

                            conn.Open();
                            odr = ocmd.ExecuteReader();
                          
                                if (odr != null)
                                {
                                    while (odr.Read())
                                    {

                                        MobiusNHINCommunity community = new MobiusNHINCommunity();
                                        community.CommunityIdentifier = odr[0].ToString();
                                        community.CommunityDescription = odr[1].ToString();
                                        community.IsHomeCommunity = false;
                                        var actvieString = odr[2].ToString().Trim().ToUpper();
                                        if (actvieString == "1" || actvieString == "TRUE" || actvieString == "Y" || actvieString == "YES")
                                        {
                                            community.Active = true;
                                        }
                                        else
                                        {
                                            community.Active = false;
                                        }
                                        nhincommunity.Add(community);
                                    }
                                    if (nhincommunity.Count > 0)
                                    {
                                        grdCommunityUpload.Visible = true;
                                        grdCommunityUpload.DataSource = nhincommunity;
                                           GlobalSessions.SessionAdd(SessionItem.CommunityList, nhincommunity);
                                        grdCommunityUpload.DataBind();
                                    }

                                }
                                else
                                {
                                    lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Invalid_File_Format);
                                }
                           
                        }
                        catch (Exception ex)
                        {
                            lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.UnknownException) + " : " + ex.Message;
                        }
                        finally
                        {
                            conn.Close();
                            if (odr != null)
                                odr.Close();
                            if (File.Exists(filePath))
                                File.Delete(filePath);
                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Select_Valid_Xls_File);
                    }
                }
            }
            else
            {
                lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Select_Valid_Xls_File);

            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }

    #endregion

    #region ShowCsvFileData

    protected void ShowCsvFileData()
    {
        try
        {
            grdCommunityUpload.DataSource = null;
            grdCommunityUpload.DataBind();

            if (fileuploadExcel.HasFile)
            {
                if (fileuploadExcel.FileContent.Length > 0)
                {
                    string Foldername;
                    string Extension = System.IO.Path.GetExtension(fileuploadExcel.PostedFile.FileName);
                    string filename = Path.GetFileName(fileuploadExcel.PostedFile.FileName.ToString());
                    if (Extension.ToLower() == CSV)
                    {
                        Foldername = Server.MapPath("~/Templates/");
                        string newid = Guid.NewGuid().ToString();
                        fileuploadExcel.PostedFile.SaveAs(Foldername + newid + filename.ToString());
                        string communitiesdata = string.Empty;
                        StreamReader streamreader = new StreamReader(Foldername + newid + filename);
                        communitiesdata = streamreader.ReadToEnd();
                        string[] rowArray = communitiesdata.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        string[] hdrArray = rowArray[0].Split(',');
                        if (hdrArray[0].ToLower() == COMMUNITY_IDENTIFIER && hdrArray[1].ToLower() == COMMUNITY_DESCRIPTION && hdrArray[2].ToLower() == ACTIVE)
                        {

                            for (int i = 1; i < rowArray.Length - 1; i++)
                            {
                                string[] cellArray = rowArray[i].Split(',');
                                MobiusNHINCommunity community = new MobiusNHINCommunity();
                                community.CommunityIdentifier = cellArray[0].ToString();
                                community.CommunityDescription = cellArray[1].ToString();
                                community.IsHomeCommunity = false;
                                if (cellArray[2].ToString() == ONE || cellArray[2].ToString().ToLower() == TRUE)
                                {
                                    community.Active = true;
                                }
                                else
                                {
                                    community.Active = false;
                                }
                                nhincommunity.Add(community);
                            }

                            try
                            {
                                if (nhincommunity.Count > 0)
                                {
                                    grdCommunityUpload.Visible = true;
                                    grdCommunityUpload.DataSource = nhincommunity;
                                    GlobalSessions.SessionAdd(SessionItem.CommunityList, nhincommunity);
                                    grdCommunityUpload.DataBind();
                                }
                            }
                            catch (Exception ex)
                            {
                                lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Invalid_File_Format);
                                grdCommunityUpload.DataSource = null;
                                grdCommunityUpload.DataBind();
                            }

                            finally
                            {
                                streamreader.Close();
                                File.Delete(Foldername + newid + filename.ToString());
                            }
                        }
                        else
                        {
                            lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Invalid_File_Format);

                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Select_Valid_Csv_File);
                    }
                }
            }
            else
            {
                lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Select_Valid_Csv_File);
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    protected void GrdCommunityUpload_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            if (GlobalSessions.SessionItem(SessionItem.CommunityList) != null)
            {
                grdCommunityUpload.DataSource = (List<MobiusNHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);
                grdCommunityUpload.PageIndex = e.NewPageIndex;
                grdCommunityUpload.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdCommunityUpload.Rows.Count>0)
            {
                foreach (GridViewRow dr in grdCommunityUpload.Rows)
                {
                    CheckBox chk = dr.Cells[0].FindControl("chkCommunityRow") as CheckBox;
                    if (chk.Checked)
                    {
                        MobiusNHINCommunity community = new MobiusNHINCommunity();
                        Label CommunityIdentifier = dr.Cells[1].FindControl("CommunityIdentifier") as Label;
                        community.CommunityIdentifier = CommunityIdentifier.Text.ToString();

                        Label CommunityDescription = dr.Cells[2].FindControl("CommunityDescription") as Label;
                        community.CommunityDescription = CommunityDescription.Text.ToString();

                        Label chkEditIsHomeCommunity = dr.Cells[3].FindControl("IsHomeCommunity") as Label;
                        community.IsHomeCommunity = false;

                        CheckBox Active = dr.Cells[4].FindControl("Active") as CheckBox;
                        community.Active = Active.Checked;
                        nhincommunity.Add(community);
                    }
                }
                Result response = MobiusBAL.AddNhinCommunities(nhincommunity);
                    lblErrorMsg.Text = response.ErrorMessage;
            }
            else
            {
                lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Select_Valid_File);
                grdCommunityUpload.DataSource = null;
                grdCommunityUpload.DataBind();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageCommunities.aspx", false);
    }

    protected void grdCommunityUpload_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!HasBlankRow)
                {
                    string value = ((Mobius.Entity.MobiusNHINCommunity)(e.Row.DataItem)).IsHomeCommunity.ToString();
                    Label lblHomeCommunity = e.Row.FindControl("IsHomeCommunity") as Label;
                    Label CommunityDescription = e.Row.FindControl("CommunityDescription") as Label;
                    Label CommunityIdentifier = e.Row.FindControl("CommunityIdentifier") as Label;
                    CheckBox Active = e.Row.FindControl("Active") as CheckBox;
                    CheckBox chkCommunityRow=e.Row.FindControl("chkCommunityRow") as CheckBox;

                    lblHomeCommunity.Visible = true;
                    CommunityDescription.Visible = true;
                    CommunityIdentifier.Visible = true;
                     Active.Visible = true;
                    chkCommunityRow.Visible = true;


                    if (value.ToLower() == TRUE)
                    {
                        lblHomeCommunity.Text = YES;
                    }
                    else
                    {
                        lblHomeCommunity.Text = NO;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    protected void downloadtemplte_Click(object sender, EventArgs e)
    {
        try
        {
            string path = string.Empty;
            if (rdExcel.Checked)
            {
                path = Server.MapPath(TEMPLATE_PATH_XLSX);
            }
            else
            {
                path = Server.MapPath(TEMPLATE_PATH_CSV);
            }
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + file.Name + "\"");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "text/plain";
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
       
    }

    private void logMessage(string message)
    {

        try
        {
            Logger logger;
            logger = Logger.GetInstance();
            logger.WriteLog(LogSeverity.DEBUG, "MobiusLog", message);
        }
        catch (Exception ex)
        {

        }

    }

    
}