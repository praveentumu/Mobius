using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI;
using Mobius.CoreLibrary;
using Mobius.BAL;
using Mobius.Entity;


public partial class ConfigurationSetting : System.Web.UI.Page
{

    MobiusBAL MobiusBAL = null;
    MobiusAppSettingUpdater MobiusAppSettingUpdater = null;
    Result result = new Result();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblErrorMsg.Text = string.Empty;
            MobiusBAL = new MobiusBAL();
            MobiusAppSettingUpdater = new MobiusAppSettingUpdater();
            if (!Page.IsPostBack)
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
           
            EmergencyAccessTime.Text = MobiusAppSettingReader.EmergencyOverriddenTime.ToString();
            txtAndroidVersion.Text = MobiusAppSettingReader.AndroidApplicationVersion;
            txtCertAuthServerUrl.Text = MobiusAppSettingReader.CertificateAuthorityServerURL;
            txtCertOrgName.Text = MobiusAppSettingReader.CertificateOrgName;
            txtCertOrgUnit.Text = MobiusAppSettingReader.CertificateOrganizationalUnit;
            txtConnectGatewayTimeOut.Text = MobiusAppSettingReader.ConnectGatewayTimeOut.ToString();
            txtDocPath.Text = MobiusAppSettingReader.DocumentPath;
            txtMobiusNistValidationUrl.Text = MobiusAppSettingReader.MobiusNISTValidationServiceURL;
            txtMobiusPatientCorrelation.Text = MobiusAppSettingReader.MobiusPatientCorrelation;
            txtNhinDocumentQueryUrl.Text = MobiusAppSettingReader.NHINDocumentQueryURL;
            txtNhinPatientDiscoveryUrl.Text = MobiusAppSettingReader.NHINPatientDiscoveryURL;
            txtNhinPolicyEngineUrl.Text = MobiusAppSettingReader.NHINPolicyEngineURL;
            txtNhinRetrieveDocumentUrl.Text = MobiusAppSettingReader.NHINRetrieveDocumentURL;
            txtNhinSubmissionDocumentUrl.Text = MobiusAppSettingReader.NHINSubmissionDocumentURL;
            txtOnlineNistServiceUrl.Text = MobiusAppSettingReader.OnlineNISTValidationServiceURL;
            txtServiceNotificationGap.Text = MobiusAppSettingReader.ServiceNotificationGap.ToString();
            txtTempPath.Text = MobiusAppSettingReader.TempPath;
            txtUpgradationNotificationGap.Text = MobiusAppSettingReader.UserUpgradationNotificationGap.ToString();
            txtSmtpHost.Text = MobiusAppSettingReader.SmtpHost;
            txtSmtpUserName.Text = MobiusAppSettingReader.SmtpUserName;
            txtSmtpPort.Text = MobiusAppSettingReader.SmtpPort.ToString().Trim();
            txtSmtpPassword.Attributes.Add("value", MobiusAppSettingReader.SmtpPassword);
            
            if (MobiusAppSettingReader.SmtpEnableSSL)
                SmtpEnableTrue.Checked = true;
            else
                SmtpEnableFalse.Checked = true;

            if (MobiusAppSettingReader.ValidateC32Document)
                ValidateDocumentTrue.Checked = true;
            else
                ValidateDocumentFalse.Checked = true;

            if (MobiusAppSettingReader.UseOnlineService)
                UseNISTServiceTrue.Checked = true;
            else
                UseNISTServiceFalse.Checked = true;

            if (MobiusAppSettingReader.EncryptionFlag)
                EncryptionFlagTrue.Checked = true;
            else
                EncryptionFlagFalse.Checked = true;

            txtHomeCommunityId.Text = MobiusAppSettingReader.LocalHomeCommunityID;

            // Map Data for Emergency provider role
            List<MasterData> masterDataCollection = new List<MasterData>();

            //Clearing out listbox contents.
            lstNonEmergencyRoles.Items.Clear();
            lstEmergencyRoles.Items.Clear();

            //Populating data for mapped provider roles for emergency access.
            lstEmergencyRoles.Items.Insert(0, "Check All");
            lstEmergencyRoles.DataValueField = "key";
            lstEmergencyRoles.DataTextField = "value";
            lstEmergencyRoles.DataSource = MobiusAppSettingReader.LstEmergencyRole;
            lstEmergencyRoles.DataBind();


            //Populating data for available provider roles (which are NOT mapped for emergency access).
            lstNonEmergencyRoles.Items.Insert(0, "Check All");
            lstNonEmergencyRoles.DataValueField = "key";
            lstNonEmergencyRoles.DataTextField = "value";
            lstNonEmergencyRoles.DataSource = MobiusAppSettingReader.LstNonEmergencyRole;
            lstNonEmergencyRoles.DataBind();
            
            
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

  
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {   MobiusAppSettingUpdater = new MobiusAppSettingUpdater();
            MobiusAppSettingUpdater.LstEmergencyRole = new List<string>();
            MobiusAppSettingUpdater.UseOnlineService = UseNISTServiceTrue.Checked;
            MobiusAppSettingUpdater.ValidateC32Document = ValidateDocumentTrue.Checked;
            MobiusAppSettingUpdater.EncryptionFlag = EncryptionFlagTrue.Checked;
            MobiusAppSettingUpdater.EmergencyOverriddenTime = Convert.ToInt32(EmergencyAccessTime.Text);

            MobiusAppSettingUpdater.AndroidApplicationVersion=txtAndroidVersion.Text ;
            MobiusAppSettingUpdater.CertificateAuthorityServerURL = txtCertAuthServerUrl.Text;
            MobiusAppSettingUpdater.CertificateOrgName = txtCertOrgName.Text;
            MobiusAppSettingUpdater.CertificateOrganizationalUnit = txtCertOrgUnit.Text;
            MobiusAppSettingUpdater.ConnectGatewayTimeOut = Convert.ToInt32(txtConnectGatewayTimeOut.Text);
            MobiusAppSettingUpdater.DocumentPath = txtDocPath.Text;
           
            MobiusAppSettingUpdater.MobiusNISTValidationServiceURL = txtMobiusNistValidationUrl.Text;
            MobiusAppSettingUpdater.MobiusPatientCorrelation = txtMobiusPatientCorrelation.Text;
            MobiusAppSettingUpdater.NHINDocumentQueryURL = txtNhinDocumentQueryUrl.Text;
            MobiusAppSettingUpdater.NHINDocumentQueryURL = txtNhinPatientDiscoveryUrl.Text;
            MobiusAppSettingUpdater.NHINPolicyEngineURL = txtNhinPolicyEngineUrl.Text;
            MobiusAppSettingUpdater.NHINRetrieveDocumentURL = txtNhinRetrieveDocumentUrl.Text;
            MobiusAppSettingUpdater.NHINSubmissionDocumentURL = txtNhinSubmissionDocumentUrl.Text;
            MobiusAppSettingUpdater.OnlineNISTValidationServiceURL = txtOnlineNistServiceUrl.Text;
            MobiusAppSettingUpdater.ServiceNotificationGap =Convert.ToInt32(txtServiceNotificationGap.Text);
            MobiusAppSettingUpdater.TempPath = txtTempPath.Text;
            MobiusAppSettingUpdater.UserUpgradationNotificationGap =Convert.ToInt32(txtUpgradationNotificationGap.Text);
            MobiusAppSettingUpdater.SmtpHost = txtSmtpHost.Text;
            MobiusAppSettingUpdater.SmtpUserName = txtSmtpUserName.Text;
            MobiusAppSettingUpdater.SmtpPassword = txtSmtpPassword.Text;
            MobiusAppSettingUpdater.SmtpPort =Convert.ToInt32(txtSmtpPort.Text);
            MobiusAppSettingUpdater.SmtpEnableSSL = SmtpEnableTrue.Checked;


            if (lstEmergencyRoles.Items.Count > 0)
            {
                foreach (var items in lstEmergencyRoles.Items)
                {
                    if (items.ToString().ToUpper() != "CHECK ALL")
                    {
                        MobiusAppSettingUpdater.LstEmergencyRole.Add(((ListItem)(items)).Value);
                    }
                }
            }

            MobiusAppSettingUpdater.UpdateConfigurationFile();
            lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.Record_Successfully_Updated);
            LoadData();

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
            lblErrorMsg.Text = Helper.GetErrorMessage(ErrorCode.UnknownException);
        }
    }

 
  
    protected void imgbtnMoveRight_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
           
            MobiusAppSettingUpdater = new MobiusAppSettingUpdater();
            if (lstNonEmergencyRoles.SelectedIndex > -1)
            {
                string AssetIDList = "";
                List<string> addedEmergencyList = new List<string>();
                foreach (ListItem lstAsset in lstNonEmergencyRoles.Items)
                {
                    if (lstAsset.Selected && lstEmergencyRoles.Items.FindByValue(lstAsset.Value) == null)
                    {
                        lstEmergencyRoles.Items.Add(lstAsset);
                        AssetIDList += AssetIDList.Length > 0 ? "," + lstAsset.Value : lstAsset.Value;
                    }
                }

                string[] arrItemsToRemove = AssetIDList.Split(new char[] { ',' });
                for (int i = 0; i < arrItemsToRemove.Length; i++)
                {
                    ListItem lstSelectedItem = lstNonEmergencyRoles.Items.FindByValue(arrItemsToRemove[i].ToString());
                    if (lstSelectedItem != null)
                    {
                        lstNonEmergencyRoles.Items.Remove(lstSelectedItem);
                    }
                }
                lstNonEmergencyRoles.ClearSelection();
                lstEmergencyRoles.ClearSelection();
                chkSelectAll.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void imgbtnMoveLeft_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            MobiusAppSettingUpdater = new MobiusAppSettingUpdater();
            List<string> LstEmergencyRole = new List<string>();
            if (lstEmergencyRoles.SelectedIndex > -1)
            {
                string AssetIDList = "";
                foreach (ListItem lstAsset in lstEmergencyRoles.Items)
                {
                    LstEmergencyRole.Add(lstAsset.ToString());
                    if (lstAsset.Selected && lstNonEmergencyRoles.Items.FindByValue(lstAsset.Value) == null)
                    {
                        lstNonEmergencyRoles.Items.Add(lstAsset);
                        AssetIDList += AssetIDList.Length > 0 ? "," + lstAsset.Value : lstAsset.Value;
                    }
                }
                string[] arrItemsToRemove = AssetIDList.Split(new char[] { ',' });
                for (int i = 0; i < arrItemsToRemove.Length; i++)
                {
                    ListItem lstSelectedItem = lstEmergencyRoles.Items.FindByValue(arrItemsToRemove[i].ToString());
                    if (lstSelectedItem != null)
                    {
                        lstEmergencyRoles.Items.Remove(lstSelectedItem);
                        LstEmergencyRole.Remove(lstSelectedItem.ToString());
                        LstEmergencyRole.Remove("Check All");
                    }
                }
                lstNonEmergencyRoles.ClearSelection();
                lstEmergencyRoles.ClearSelection();
                chkSelectAll.Checked = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, typeof(Xml), "ViewConfig", "window.open('ViewFile.aspx')", true);
    }

}