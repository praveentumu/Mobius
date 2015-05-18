using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstGenesis.UI.Base;
using Mobius.CoreLibrary;
using System.Collections.Generic;
using Mobius.Entity;
using FirstGenesis.UI;
using MobiusServiceLibrary;

using System.Linq;
using MobiusServiceUtility;
public partial class RolesAndPermission : BaseClass
{
    #region Variables
    int countArrayCategory = 0, countCheckBox = 0, categoryValue = 0, PurposeType = 0, userType = 0;
    string strCategoryValue = string.Empty;

    CheckBoxList chk = new CheckBoxList();
    CheckBoxList chkmiddle = new CheckBoxList();
    CheckBoxList chkRight = new CheckBoxList();
    RadioButtonList rbtselect = new RadioButtonList();

    GetPatientConsentRequest getPatientConsentRequest = null;
    GetPatientConsentResponse getPatientConsentResponse = null;
    GetMasterDataResponse getMasterDataResponse = null;
    GetMasterDataRequest getMasterDataRequest = null;
    SoapProperties soapProperties = new SoapProperties();
    List<MobiusServiceLibrary.C32Section> C32Sections = null;

    protected const string MPIID = "Unable to get MPIID. Please try again.";
    protected const string SELECT_ROLE = "Please Select Role.";
    protected const string SELECT_PURPOSE = "Please Select Purpose.";
    protected const string SELECT_RULE_START_DATE = "Please Select Rule Start Date.";
    protected const string SELECT_RULE_END_DATE = "Please Select Rule End Date.";
    protected const string SELECT_RULE_START_END_DATE = "Please enter Rule Start Date & Rule End Date in MM/DD/YYYY format.";
    protected const string RULE_START_DATE_VALIDATION = "You cannot select a Rule Start Date earlier than today!";
    protected const string RULE_END_DATE_VALIDATION = "You cannot select a Rule End Date earlier than today!";
    protected const string RULE_END_DATE_VALIDATION_CHECK = "Rule End Date cannot occur before Rule Start Date!";

    private MobiusClient _HISEProxy = null;
    private SoapHandler _soapHandler = null;
    protected string jsonMandatoryLeafs;

    #endregion

    #region Properties
    public SoapHandler soapHandler
    {
        get { return _soapHandler != null ? _soapHandler : _soapHandler = new SoapHandler(); }
        set
        {
            if (_soapHandler == null) _soapHandler = new SoapHandler();
            _soapHandler = value;
        }
    }

    public MobiusClient HISEProxy
    {
        get { return _HISEProxy != null ? _HISEProxy : _HISEProxy = new MobiusClient(); }
        set
        {
            if (_HISEProxy == null) _HISEProxy = new MobiusClient();
            _HISEProxy = value;
        }
    }

    #endregion Properties

    #region Methods
    // Function call on page load.
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (!IsPostBack)
            {
                FillPurposeDropDown();
                FillRoleDropDown();
                //Page is called with PatientConsentId and MPIID for edit/modify Consent 
                if ((GlobalSessions.SessionItem(SessionItem.PatientConsentId) != null && !string.IsNullOrEmpty(Convert.ToString(GlobalSessions.SessionItem(SessionItem.PatientConsentId)))))
                {
                    getPatientConsent(Convert.ToString(GlobalSessions.SessionItem(SessionItem.PatientConsentId)));
                }
                else // Page is load to create new consent
                {
                    //Load Master data from service and create tree.
                    getC32Modules();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }

    }

       /// <summary>
    /// Function to update permission and category for particular UserType.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdatePermission_Click(object sender, EventArgs e)
    {
        Result result = new Result();
        try
        {
            // calling function for setting the permission and category for this user type.
            if (ValidatePage())
            {
                UpdatePatientConsentPolicyRequest request = new UpdatePatientConsentPolicyRequest();
                if (GlobalSessions.SessionItem(SessionItem.MPIID) == null)
                {
                    lblErrorMsg.Text = MPIID;
                    return;
                }
                request.MPIID = Convert.ToString(GlobalSessions.SessionItem(SessionItem.MPIID));
                //
                if (GlobalSessions.SessionItem(SessionItem.PatientConsentId) != null && !string.IsNullOrWhiteSpace(Convert.ToString(GlobalSessions.SessionItem(SessionItem.PatientConsentId))))
                {
                    request.PatientConsentID = Convert.ToInt32(GlobalSessions.SessionItem(SessionItem.PatientConsentId));
                }

                request.Permission = 1;
                request.RoleId = Convert.ToInt32(ddlRole.SelectedValue);
                
                request.PurposeOfUseId = Convert.ToInt32(ddlPurpose.SelectedValue);
                request.Allow = rbtnAllow.SelectedItem.Value == "1" ? true : false;
                request.Active = rbtnActive.SelectedItem.Value == "1" ? true : false;
                request.RuleStartDate = txtRuleStartDate.Text;
                request.RuleEndDate = txtRuleEndDate.Text;


                request.PatientConsentPolicy = GetPatientConsent();
                
                UpdatePatientConsentPolicyResponse response = objProxy.UpdatePatientConsentPolicy(request);
                if (response.Result.IsSuccess)
                {
                    GlobalSessions.SessionAdd(SessionItem.PatientConsentId, response.ConsentId);
                    if ((GlobalSessions.SessionItem(SessionItem.PatientConsentId) != null && !string.IsNullOrEmpty(Convert.ToString(GlobalSessions.SessionItem(SessionItem.PatientConsentId)))))
                    {
                        getPatientConsent(Convert.ToString(GlobalSessions.SessionItem(SessionItem.PatientConsentId)));
                    }
                    if (this.soapHandler.ResponseDecryption(response.SoapProperties, response))
                    {
                        AlertMessage(response.Result.ErrorMessage);
                    }
                    else
                    {
                        AlertMessage(response.Result.ErrorMessage);                        
                    }
                }
                else
                {
                    ExceptionHelper.HandleException(page: Page, displayMessage: response.Result.ErrorMessage, title: ExceptionHelper.ErrorTitle.Message);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ManageConsent.aspx", false);
    }

    #endregion
    
    #region Private Methods

    private void getPatientConsent(string patientConsentId)
    {
        GetPatientConsentRequest request = new GetPatientConsentRequest();
        request.MPIID = Convert.ToString(GlobalSessions.SessionItem(SessionItem.MPIID));
        request.PatientConsentId = Convert.ToInt32(patientConsentId);
        GetPatientConsentResponse response = objProxy.GetPatientConsentByConsentId(request);

        if (response.Result.IsSuccess)
        {
            
            if (this.soapHandler.ResponseDecryption(response.SoapProperties, response))
            {
                bindPageControl(response.PatientConsents.FirstOrDefault());
                CreateC32Tree(response.C32Section);
                //Bind Page controls
                
            }
            else
            {
                //ExceptionHelper.HandleException(page: Page, displayMessage: "", title: ExceptionHelper.ErrorTitle.Message);
                return;
            }
        }
        else
        {
            ExceptionHelper.HandleException(page: Page, displayMessage: response.Result.ErrorMessage, title: ExceptionHelper.ErrorTitle.Message);
        }
    }

    /// <summary>
    /// This method will fill the HTML controls
    /// </summary>
    /// <param name="patientConsent"></param>
    private void bindPageControl(PatientConsent patientConsent)
    {
        if (patientConsent.RoleId > 0)
        {
            ddlRole.SelectedValue = patientConsent.RoleId.ToString();
            ddlRole.Enabled = false;
        }

        if (patientConsent.PurposeOfUseId > 0)
        {
            ddlPurpose.SelectedValue = patientConsent.PurposeOfUseId.ToString();
            ddlPurpose.Enabled = false;
        }

        if (!string.IsNullOrWhiteSpace(patientConsent.RuleStartDate))
        {
            txtRuleStartDate.Text = Convert.ToDateTime(patientConsent.RuleStartDate).ToString("MM/dd/yyyy");
        }
        if (!string.IsNullOrWhiteSpace(patientConsent.RuleEndDate))
        {
            txtRuleEndDate.Text = Convert.ToDateTime(patientConsent.RuleEndDate).ToString("MM/dd/yyyy");
        }

        if (patientConsent.Allow)
        {
            rbtnAllow.Items[0].Selected = true;
            rbtnAllow.SelectedValue = "1";
        }
        else
        {
            rbtnAllow.Items[1].Selected = true;
            rbtnAllow.SelectedValue = "0";
        }

        if (patientConsent.Active)
        {
            rbtnActive.Items[0].Selected = true;
            rbtnActive.SelectedValue = "1";
        }
        else
        {
            rbtnActive.Items[1].Selected = true;
            rbtnActive.SelectedValue = "0";
        }

    }

    /// <summary>
    /// This method will fetch the C32 sections(Masterdata) from service
    /// </summary>
    private void getC32Modules()
    {
        
        GetC32SectionsResponse getC32SectionResponse = objProxy.GetC32Sections();
        if (getC32SectionResponse.Result.IsSuccess)
        {
            if (this.soapHandler.ResponseDecryption(getC32SectionResponse.SoapProperties, getC32SectionResponse))
            {
                CreateC32Tree(getC32SectionResponse.C32Sections);
            }
            else
            {
                //ExceptionHelper.HandleException(page: Page, displayMessage: "", title: ExceptionHelper.ErrorTitle.Message);
                return;
            }
        }
        else
        {
            ExceptionHelper.HandleException(page: Page, displayMessage: getC32SectionResponse.Result.ErrorMessage, title: ExceptionHelper.ErrorTitle.Message);
        }

    }

    /// <summary>
    /// This method would bind tree control with C32Section object
    /// </summary>
    /// <param name="list"></param>
    private void CreateC32Tree(List<MobiusServiceLibrary.C32Section> c32Section)
    {
        trvModules.Nodes.Clear();
        List<string> mandatoryLeafs = new List<string>();
        TreeNode rootNode = new TreeNode("Clinical Document");
        TreeNode node = null;
        TreeNode childnode = null;
        c32Section = c32Section.OrderBy(t => t.DisplayOrder).ToList();

        //Need to write the code for display order to sort
        c32Section = c32Section.OrderBy(t => t.DisplayOrder).ToList();

        foreach (var item in c32Section)
        {
            node = new TreeNode(item.Name, item.Id.ToString());
            node.SelectAction = TreeNodeSelectAction.None;
            // R And R2 are required and required if available section than, check box of tree control must be disabled and checked.
            if (item.Optionality == "R" || item.Optionality == "R2")
            {
                mandatoryLeafs.Add(item.Name.ToString());
            }

            if (item.ChildSections.Count > 0)
            {
                item.ChildSections = item.ChildSections.OrderBy(t => t.DisplayOrder).ToList();
                foreach (var childItem in item.ChildSections)
                {
                    childnode = new TreeNode(childItem.Name, childItem.Id.ToString());
                    if (rbtnAllow.SelectedValue=="1")
                    {
                        childnode.Checked = childItem.Allow;
                    }
                    else
                        childnode.Checked = !childItem.Allow;
                    childnode.SelectAction = TreeNodeSelectAction.None;
                    // R And R2 are required and required if available section than, check box of tree control must be disabled and checked.
                    if (childItem.Optionality == "R" || childItem.Optionality == "R2")
                    {
                        mandatoryLeafs.Add(item.Name.ToString());
                    }
                    node.ChildNodes.Add(childnode);
                }
            }
            node.Checked=rbtnAllow.SelectedValue == "1"? item.Allow: !item.Allow;

            rootNode.ChildNodes.Add(node);
            rootNode.SelectAction = TreeNodeSelectAction.None;
        }
        if (rootNode.ChildNodes.Cast<TreeNode>().ToList().Any(t => t.Checked == false))
            rootNode.Checked = false;
        else
            rootNode.Checked = true;

        trvModules.Nodes.Add(rootNode);
        trvModules.ShowCheckBoxes = TreeNodeTypes.All;
        trvModules.ShowLines = false;
        trvModules.EnableClientScript = true;
        trvModules.DataBind();
        //trvModules.s =  TreeNodeSelectAction.None;
        //Converting the collection in JSON object.
        this.jsonMandatoryLeafs = mandatoryLeafs.ToJSON();

    }


    private MobiusServiceLibrary.PatientConsentPolicy GetPatientConsent()
    {

        MobiusServiceLibrary.PatientConsentPolicy patientConsentPolicy = new MobiusServiceLibrary.PatientConsentPolicy();

        List<MobiusServiceLibrary.ModulePermission> modules = new List<MobiusServiceLibrary.ModulePermission>();
        MobiusServiceLibrary.ModulePermission modulePermission;
        MobiusServiceLibrary.Consent sectionConsent;
        TreeNodeCollection node = trvModules.Nodes;

        List<string> mandatoryLeafs = new List<string>();
       
        
        GetC32SectionsResponse getC32SectionResponse = objProxy.GetC32Sections();
       
        foreach (var item in getC32SectionResponse.C32Sections)
        {
            if (item.Optionality == "R" || item.Optionality == "R2")
            {
                mandatoryLeafs.Add(item.Id.ToString());
            }
        }
        foreach (TreeNode leaf in node[0].ChildNodes)
        {
            modulePermission = new MobiusServiceLibrary.ModulePermission();
            // C32PatientConsent.Consent = new Consent();
            modulePermission.Id = Convert.ToInt32(leaf.Value);
         //  if(modulePermission.Id.Equals(mandatoryLeafs.Any(t=>
            if (mandatoryLeafs.Contains(modulePermission.Id.ToString()))
            {
                modulePermission.Allow = true;
            }
            else
            {
                if (rbtnAllow.SelectedValue == "1")
                    modulePermission.Allow = (leaf.Checked);
                else
                    modulePermission.Allow = !(leaf.Checked);
            }
            if (leaf.ChildNodes.Count > 0)
            {
                modulePermission.Sections = new List<MobiusServiceLibrary.Consent>();
                foreach (TreeNode childLeaf in leaf.ChildNodes)
                {
                    sectionConsent = new MobiusServiceLibrary.Consent();
                    sectionConsent.Id = Convert.ToInt32(childLeaf.Value);
                    if (rbtnAllow.SelectedValue == "1")
                    sectionConsent.Allow = childLeaf.Checked;
                    else
                     sectionConsent.Allow = !(childLeaf.Checked);
                    modulePermission.Sections.Add(sectionConsent);
                }
            }

            //foreach (var item in modules)
            //{ 
            //modules.Select(t=>t.Sections.
            
            //}
            modules.Add(modulePermission);
           
        }

        patientConsentPolicy.Modules = modules;
        return patientConsentPolicy;
    }

    private bool AlertMessage(string message)
    {
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "msg", "alert('" + message + "');", true);
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool ValidatePage()
    {
        DateTime dateTime;

        if (ddlRole.SelectedIndex <= 0)
        {
            return AlertMessage(SELECT_ROLE);
        }
        if (ddlPurpose.SelectedIndex <= 0)
        {
            return AlertMessage(SELECT_PURPOSE);
        }
        if (txtRuleStartDate.Text == "")
        {
            return AlertMessage(SELECT_RULE_START_DATE);
        }
        if (txtRuleEndDate.Text == "")
        {
            return AlertMessage(SELECT_RULE_END_DATE);
        }
        if ((!DateTime.TryParse(txtRuleStartDate.Text, out dateTime)) || (!DateTime.TryParse(txtRuleEndDate.Text, out dateTime)))
        {
            return AlertMessage(SELECT_RULE_START_END_DATE);
        }
        if (Convert.ToBoolean(Session["IsNew"]) == true && Session["IsNew"] != null)
        {
            if ((Convert.ToDateTime(txtRuleStartDate.Text) < DateTime.Now.Date))
            {
                return AlertMessage(RULE_START_DATE_VALIDATION);
            }
            if ((Convert.ToDateTime(txtRuleEndDate.Text) < DateTime.Now.Date))
            {
                return AlertMessage(RULE_END_DATE_VALIDATION);
            }
        }
        if (Convert.ToDateTime(txtRuleStartDate.Text) > Convert.ToDateTime(txtRuleEndDate.Text))
        {
            return AlertMessage(RULE_END_DATE_VALIDATION_CHECK);
        }
        else
            return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void FillRoleDropDown()
    {
        GetMasterDataResponse getMasterDataResponse = new GetMasterDataResponse();
        GetMasterDataRequest getMasterDataRequest = new GetMasterDataRequest();
        //
        getMasterDataRequest.MasterCollection = MasterCollection.UserRole;
        getMasterDataRequest.dependedValue = 0;
        getMasterDataResponse = this.HISEProxy.GetMasterData(getMasterDataRequest);
        
        //Fill drop down
        FillDropDown(ddlRole, getMasterDataResponse.MasterDataCollection);
    }
    /// <summary>
    /// 
    /// </summary>
    private void FillPurposeDropDown()
    {
        GetMasterDataResponse getMasterDataResponse = new GetMasterDataResponse();
        GetMasterDataRequest getMasterDataRequest = new GetMasterDataRequest();
        //
        getMasterDataRequest.MasterCollection = MasterCollection.PurposeOfUse;
        getMasterDataRequest.dependedValue = 0;
        
        getMasterDataResponse = this.HISEProxy.GetMasterData(getMasterDataRequest);
        //// Remove the Emergency from pupose List, as whatever consent patient will set for emergency purpose, it will be over-right by Admin configuration.
        //int emergencyIndex= getMasterDataResponse.MasterDataCollection.FindIndex(t=>t.Description==PurposeOfUse.EMERGENCY.ToString());
        //getMasterDataResponse.MasterDataCollection.RemoveAt(emergencyIndex);

        //Fill drop down
        FillDropDown(ddlPurpose, getMasterDataResponse.MasterDataCollection);
    }


    #endregion
    
}
