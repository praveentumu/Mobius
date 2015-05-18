using System;
using System.Web.UI;
using WebText = System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using FirstGenesis.UI.Base;
using Ediable_Repeater;
using System.Web.UI.WebControls;
using MobiusServiceLibrary;
using Mobius.CoreLibrary;
using System.Web.Services;
using FirstGenesis.UI;
using MobiusServiceUtility;
using System.Linq;


public partial class UpdatePatientDetails : BaseClass
{
    #region Variables
    MobiusClient HISEProxy = new MobiusClient();
    string MPIID = string.Empty;
    SoapHandler soapHandler;
    private const string FIRSTNAME_LASTNAME = "Please provide both first and last name";
    private const string FIRST_NAME = "Please provide first name";
    private const string LAST_NAME = "Please provide last name";
    private const string ZIP_CODE = "Please provide valid zip code";
    private const string POSTAL_CODE = "Please provide postalcode.";
    private const string INVALID_CSR = "Invalid CSR request input(s).";
    private const string TELEPHONE_EXTENSION_NUMBER = "Please provide telephone number";
    private const string TELEPHONE_NUMBER = "Please provide telephone number";
    private const string EXTENSION_NUMBER = "Please provide extension number";
    private const string UPDATE_PATIENT_FAILED = "Patient Update failed";
    private const string TeleStatus_Active = "Active";
    private const string TeleStatus_InActive = "InActive";
    
    #endregion

    #region Properties
    

    private int EditIndexName
    {
        get { return (int)ViewState["EditIndexName"]; }
        set { ViewState["EditIndexName"] = value; }
    }

    private int EditIndexTelephone
    {
        get { return (int)ViewState["EditIndexTelephone"]; }

        set { ViewState["EditIndexTelephone"] = value; }
    }

    private int EditIndexAddress
    {
        get { return (int)ViewState["EditIndexAddress"]; }
        set { ViewState["EditIndexAddress"] = value; }
    }

    #endregion

    #region Methods
    // Function call on page load.
    /// <summary>
    /// Function called at page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
   
        try
        {
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

    #endregion

    #region CancelRegister

    protected void CancelRegister(object sender, EventArgs e)
    {
        try
        {
            LoadData();

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region OnAddName

    protected void OnAddName(object sender, EventArgs e)
    {
        // Get the textboxes using the button as the starting point
        WebText.TextBox firstName = ((System.Web.UI.Control)sender).Parent.FindControl("NewFirstName") as WebText.TextBox;
        WebText.TextBox middleName = ((System.Web.UI.Control)sender).Parent.FindControl("NewMiddleName") as WebText.TextBox;
        string middlename = !string.IsNullOrEmpty(middleName.Text.ToString()) ? middleName.Text.ToString() : string.Empty;
        WebText.TextBox lastName = ((System.Web.UI.Control)sender).Parent.FindControl("NewLastName") as WebText.TextBox;

        WebText.TextBox PrefixName = ((System.Web.UI.Control)sender).Parent.FindControl("txtPrefixName") as WebText.TextBox;
        WebText.TextBox SuffixName = ((System.Web.UI.Control)sender).Parent.FindControl("txtSuffixName") as WebText.TextBox;


        // No point in adding anything if empty
        if (!string.IsNullOrEmpty(firstName.Text) && !string.IsNullOrEmpty(lastName.Text))
        {
            // Add a new name and rebind the repeater
            Data data = new Data();
            BindNameDetails(data);
            Names name = new Names();
            name.IDName = 0;
            name.FirstName = firstName.Text;
            name.MiddleName = middlename;
            name.LastName = lastName.Text;
            name.PrefixName = PrefixName.Text;
            name.SuffixName= SuffixName.Text;
            name.Action = ActionType.Insert;
            if (name != null)
            {
                data.Names.Add(name);
            }

            rptrName.DataSource = data.Names.FindAll(t => t.Action != ActionType.Delete);
            rptrName.DataBind();
        }
        else
        {
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(firstName.Text) && string.IsNullOrWhiteSpace(lastName.Text))
            {
                message = FIRSTNAME_LASTNAME;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(firstName.Text))
                {
                    message = FIRST_NAME;
                }
                else
                {
                    message = LAST_NAME;
                }

            }
            var script = "alert('" + message + "');";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
        }
    }

    #endregion

    #region Event rptrName_ItemDataBound
    protected void rptrName_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item ||
             e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.ItemIndex == EditIndexName)
                {
                    // Find the placeholder
                    PlaceHolder p = e.Item.FindControl("firstNameEditPlaceholder") as PlaceHolder;

                    // Create textBox and assign the current value of the data item
                    TextBox t = new TextBox();
                    t.ID = "firstNameEdit";
                    t.Text = ((Names)e.Item.DataItem).FirstName;
                    // Add the textbox to the placeholder
                    p.Controls.Add(t);
                    // Get the existing label and hide it
                    Label l = e.Item.FindControl("firstName") as Label;
                    l.Visible = false;



                    p = e.Item.FindControl("middleNameEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "middleNameEdit";
                    t.Text = ((Names)e.Item.DataItem).MiddleName;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("middleName") as Label;
                    l.Visible = false;



                    p = e.Item.FindControl("lastNameEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "lastNameEdit";
                    t.Text = ((Names)e.Item.DataItem).LastName.ToString();
                    p.Controls.Add(t);
                    l = e.Item.FindControl("lastName") as Label;
                    l.Visible = false;

                    p = e.Item.FindControl("IDNameEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "NameIDEdit";
                    t.Text = ((Names)e.Item.DataItem).IDName.ToString();
                    p.Controls.Add(t);
                    l = e.Item.FindControl("NameID") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("SuffixNamePlaceHolder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "suffixNameEdit";
                    t.Text = ((Names)e.Item.DataItem).SuffixName;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("suffixName") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("PrefixNameEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "prefixNameEdit";
                    t.Text = ((Names)e.Item.DataItem).PrefixName;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("prefixName") as Label;
                    l.Visible = false;




                    // Make hidden fields visible
                    HtmlInputHidden h = e.Item.FindControl("firstNameHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("hdnNameID") as HtmlInputHidden;
                    h.Visible = true;
                    //middle 
                    h = e.Item.FindControl("middleNameHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("lastNameHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("PrefixNameHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("SuffixNameHidden") as HtmlInputHidden;
                    h.Visible = true;
                    // Remove the edit button from display
                    ImageButton b = e.Item.FindControl("Edit") as ImageButton;
                    b.Visible = false;

                    // Re use the delete button
                    b = e.Item.FindControl("Delete") as ImageButton;
                    b.CommandName = "save";
                    b.OnClientClick = "OnSave(this)";
                    b.ImageUrl = "~/images/base_floppydisk_32.png";
                    b.ToolTip = "Save";
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region Event RptrName_ItemCommad

    protected void rptrName_ItemCommand(object sender, RepeaterCommandEventArgs e)
    {
        Data data = new Data();
        BindNameDetails(data);
        try
        {
            if (e.Item.ItemIndex != -1)
            {
                if (data.Names.Count > e.Item.ItemIndex)
                {
                    int CurrentItemIndex = 0;
                    HtmlInputHidden hdnIdValue = e.Item.FindControl("hdnNameID") as HtmlInputHidden;

                    if (hdnIdValue.Value == "0" || string.IsNullOrEmpty(hdnIdValue.Value))
                    {
                        CurrentItemIndex = data.Names.IndexOf(data.Names.FindAll(item => item.Action != ActionType.Delete)[e.Item.ItemIndex]);
                    }
                    else
                    {
                        CurrentItemIndex = data.Names.IndexOf(data.Names.First(addItem => addItem.IDName.ToString() == hdnIdValue.Value));
                    }

                    if (e.CommandName == "delete")
                    {
                        if (data.Names[CurrentItemIndex] != null)
                            data.Names[CurrentItemIndex].Action = ActionType.Delete;
                        EditIndexName = -1;
                    }
                    else if (e.CommandName == "edit")
                    {
                        EditIndexTelephone = -1;
                        EditIndexName = e.Item.ItemIndex;
                    }
                    else if (e.CommandName == "save")
                    {
                        HtmlInputHidden t = e.Item.FindControl("firstNameHidden") as HtmlInputHidden;
                        data.Names[CurrentItemIndex].FirstName = t.Value;

                        t = e.Item.FindControl("middleNameHidden") as HtmlInputHidden;
                        data.Names[CurrentItemIndex].MiddleName = t.Value;


                        t = e.Item.FindControl("lastNameHidden") as HtmlInputHidden;
                        data.Names[CurrentItemIndex].LastName = t.Value;

                        t = e.Item.FindControl("PrefixNameHidden") as HtmlInputHidden;
                        data.Names[CurrentItemIndex].PrefixName = t.Value;

                        t = e.Item.FindControl("SuffixNameHidden") as HtmlInputHidden;
                        data.Names[CurrentItemIndex].SuffixName = t.Value;

                        if (data.Names[CurrentItemIndex].Action != ActionType.Insert)
                            data.Names[CurrentItemIndex].Action = ActionType.Update;
                        EditIndexName = -1;
                    }

                    rptrName.DataSource = data.Names.FindAll(t => t.Action != ActionType.Delete);
                    rptrName.DataBind();
                }
                else
                    ExceptionHelper.HandleException(page: Page, displayMessage: "Record Not Found");
            }
            else
            {
                rptrName.DataSource = data.Names.FindAll(t => t.Action != ActionType.Delete);
                rptrName.DataBind();
            }
        }

        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region OnAddTelephone

    protected void OnAddTelephone(object sender, EventArgs e)
    {
        Data data = new Data();
        // Get the textboxes using the button as the starting point
        WebText.DropDownList NewTelephoneType = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneType") as WebText.DropDownList;
        WebText.TextBox NewTelephoneExtension = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneExtension") as WebText.TextBox;
        WebText.TextBox NewTelephoneData = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneData") as WebText.TextBox;
        WebText.DropDownList NewTelephoneStatus = ((System.Web.UI.Control)sender).Parent.FindControl("NewTelephoneStatus") as WebText.DropDownList;

        //  No point in adding anything if empty
        if (!string.IsNullOrEmpty(NewTelephoneData.Text))
        {
            // Add a new name and rebind the repeater

            BindTelephoneDetails(data);
            MobiusServiceLibrary.Telephone tele = new MobiusServiceLibrary.Telephone();
            tele.Id = 0;
            tele.Type = NewTelephoneType.SelectedItem.Text;
            tele.Extensionnumber = NewTelephoneExtension.Text;
            tele.Number = NewTelephoneData.Text;
            tele.Status = NewTelephoneStatus.SelectedItem.Text == TeleStatus_Active ? true : false; ;
            tele.Action = ActionType.Insert;
            if (tele != null)
            {
                data.Telephone.Add(tele);
            }
            
        }
        else
        {
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(NewTelephoneData.Text) )
            {
                message = TELEPHONE_EXTENSION_NUMBER;
            }           
            var script = "alert('" + message + "');";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
        }
        rptrTelephone.DataSource = data.Telephone.FindAll(t => t.Action != ActionType.Delete);
        rptrTelephone.DataBind();
    }

    #endregion

    #region event rptrTelephone_ItemDataBound

    protected void rptrTelephone_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //to handle
                if (e.Item.ItemIndex == EditIndexTelephone)
                {
                    // Find the placeholder
                    PlaceHolder p = e.Item.FindControl("TelephoneTypeEditPlaceholder") as PlaceHolder;

                    // Create textBox and assign the current value of the data item
                    DropDownList d = new DropDownList();
                    d.ID = "TelephoneTypeEdit";
                    d.DataSource = GetTypes();
                    d.SelectedIndex = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Type == "Work" ? 0 : ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Type == "Home" ? 1 : 2 ;
                    d.DataBind();
                    d.Width = 150;
                    // Add the textbox to the placeholder
                    p.Controls.Add(d);
                    // Get the existing label and hide it
                    Label l = e.Item.FindControl("TelephoneType") as Label;
                    l.Visible = false;

                   

                    p = e.Item.FindControl("TelephoneExtensionEditPlaceholder") as PlaceHolder;
                    TextBox t = new TextBox();
                    t.ID = "TelephoneExtensionEdit";
                    t.Text = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Extensionnumber;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("TelephoneExtension") as Label;
                    l.Visible = false;

                    p = e.Item.FindControl("TelephoneIDEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "TelephoneIDEdit";
                    t.Text = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Id.ToString();
                    p.Controls.Add(t);
                    l = e.Item.FindControl("TelephoneID") as Label;
                    l.Visible = false;



                    p = e.Item.FindControl("TelephoneDataEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "TelephoneDataEdit";
                    t.Text = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Number;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("TelephoneData") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("TelephoneStatusEditPlaceholder") as PlaceHolder;
                    d = new DropDownList();
                    d.ID = "TelephoneStatusEdit";
                    d.DataSource = TelephoneStatus();
                    d.SelectedIndex = ((MobiusServiceLibrary.Telephone)e.Item.DataItem).Status == true ? 0 : 1;
                    d.DataBind();
                    d.Width = 150;
                    // Add the textbox to the placeholder   hdnTelephoneID
                    p.Controls.Add(d);
                    // Get the existing label and hide it
                    l = e.Item.FindControl("TelephoneStatus") as Label;
                    l.Visible = false;


                    // Make hidden fields visible
                    HtmlInputHidden h = e.Item.FindControl("TelephoneTypeHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("hdnTelephoneID") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("TelephoneExtensionHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("TelephoneDataHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("TelephoneStatusHidden") as HtmlInputHidden;
                    h.Visible = true;
                    // Remove the edit button from display
                    ImageButton b = e.Item.FindControl("EditT") as ImageButton;
                    b.Visible = false;

                    // Re use the delete button
                    b = e.Item.FindControl("DeleteT") as ImageButton;
                    b.CommandName = "saveT";
                    b.OnClientClick = "OnSaveTelephone(this)";
                    b.ImageUrl = "~/images/base_floppydisk_32.png";
                    b.ToolTip = "Save";
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList NewTelephoneType = e.Item.FindControl("NewTelephoneType") as DropDownList;
                if (NewTelephoneType != null)
                {
                    NewTelephoneType.DataSource = GetTypes();
                    NewTelephoneType.DataBind();
                }
                DropDownList NewTelephoneStatus = e.Item.FindControl("NewTelephoneStatus") as DropDownList;
                if (NewTelephoneStatus != null)
                {
                    NewTelephoneStatus.DataSource = TelephoneStatus();
                    NewTelephoneStatus.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region Event rptrTelephone_ItemCommand
    protected void rptrTelephone_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        Data data = new Data();
        try
        {
            BindTelephoneDetails(data);
            if (e.Item.ItemIndex != -1)
            {
                if (data.Telephone.Count > e.Item.ItemIndex)
                {

                    int CurrentItemIndex = 0;
                    HtmlInputHidden hdnIdValue = e.Item.FindControl("hdnTelephoneID") as HtmlInputHidden;

                    if (hdnIdValue.Value == "0" || string.IsNullOrEmpty(hdnIdValue.Value))
                    {
                        CurrentItemIndex = data.Telephone.IndexOf(data.Telephone.FindAll(item => item.Action != ActionType.Delete)[e.Item.ItemIndex]);
                    }
                    else
                    {
                        CurrentItemIndex = data.Telephone.IndexOf(data.Telephone.First(addItem => addItem.Id.ToString() == hdnIdValue.Value));
                    }

                    if (e.CommandName == "deleteT")
                    {
                        if (data.Telephone[CurrentItemIndex] != null)
                            data.Telephone[CurrentItemIndex].Action = ActionType.Delete;
                        EditIndexTelephone = -1;
                    }
                    else if (e.CommandName == "editT")
                    {
                        EditIndexTelephone = e.Item.ItemIndex;
                    }
                    else if (e.CommandName == "saveT")
                    {

                        HtmlInputHidden t = e.Item.FindControl("TelephoneTypeHidden") as HtmlInputHidden;
                        data.Telephone[CurrentItemIndex].Type = t.Value;

                        t = e.Item.FindControl("TelephoneExtensionHidden") as HtmlInputHidden;
                        data.Telephone[CurrentItemIndex].Extensionnumber = t.Value;

                        t = e.Item.FindControl("TelephoneDataHidden") as HtmlInputHidden;
                        data.Telephone[CurrentItemIndex].Number = t.Value;

                        t = e.Item.FindControl("TelephoneStatusHidden") as HtmlInputHidden;

                        data.Telephone[CurrentItemIndex].Status = t.Value == "Active" ? true : false;

                        if (data.Telephone[CurrentItemIndex].Action != ActionType.Insert)
                            data.Telephone[CurrentItemIndex].Action = ActionType.Update;

                        EditIndexTelephone = -1;
                    }
                    rptrTelephone.DataSource = null;
                    rptrTelephone.DataSource = data.Telephone.FindAll(t => t.Action != ActionType.Delete); ;
                    rptrTelephone.DataBind();
                }
                else
                {
                    ExceptionHelper.HandleException(page: Page, displayMessage: "Record Not Found");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region OnAddAddress

    protected void OnAddAddress(object sender, EventArgs e)
    {
        string message = string.Empty;
        //GetLocalityByZipCodeResponse getLocalityByZipCodeResponse = new GetLocalityByZipCodeResponse() ;
        // Get the textboxes using the button as the starting point
        try
        {
            Data data = new Data();

            WebText.TextBox NewAddressLine1 = ((System.Web.UI.Control)sender).Parent.FindControl("NewAddressLine1") as WebText.TextBox;
            WebText.TextBox NewAddressLine2 = ((System.Web.UI.Control)sender).Parent.FindControl("NewAddressLine2") as WebText.TextBox;
            WebText.TextBox NewCountry = ((System.Web.UI.Control)sender).Parent.FindControl("NewCountry") as WebText.TextBox;
            WebText.TextBox NewState = ((System.Web.UI.Control)sender).Parent.FindControl("NewState") as WebText.TextBox;
            WebText.TextBox NewCity = ((System.Web.UI.Control)sender).Parent.FindControl("NewCity") as WebText.TextBox;
            WebText.TextBox NewZip = ((System.Web.UI.Control)sender).Parent.FindControl("NewZip") as WebText.TextBox;
            WebText.DropDownList NewStatus = ((System.Web.UI.Control)sender).Parent.FindControl("NewStatus") as WebText.DropDownList;

            //  No point in adding anything if empty
            if (!string.IsNullOrEmpty(NewZip.Text))
            {
                if (NewZip.Text.Length < 5)
                {
                    message = ZIP_CODE;
                    var script = "alert('" + message + "');";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
                    return;
                }

                // Add a new name and rebind the repeater

                BindAddressDetails(data);
                MobiusServiceLibrary.Address address = new MobiusServiceLibrary.Address();
                address.Id = 0;
                address.AddressLine1 = NewAddressLine1.Text;
                address.AddressLine2 = NewAddressLine2.Text;
                address.Zip = NewZip.Text;
                
                address.City = new City();
                address.City.CityName = NewCity.Text;

                address.City.State = new State();
                address.City.State.StateName = NewState.Text;

                address.City.State.Country = new Country();
                address.City.State.Country.CountryName = NewCountry.Text;
             
               
                address.Action = ActionType.Insert;
                //}            
                //address.Status = NewStatus.SelectedItem.Text;
                address.AddressStatus = (AddressStatus)Enum.Parse(typeof(AddressStatus), NewStatus.SelectedValue, true);

                if (address != null)
                {
                    data.Address.Add(address);
                }
              
            }
            else
            {

                if (string.IsNullOrWhiteSpace(NewZip.Text))
                {
                    message = POSTAL_CODE;
                }

                var script = "alert('" + message + "');";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "ShowError", script, true);
            }

            rptr_Address.DataSource = data.Address.FindAll(t => t.Action != ActionType.Delete); ;
            rptr_Address.DataBind();

        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region Event rptr_Address_ItemDataBound
    protected void rptr_Address_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //to handle
                if (e.Item.ItemIndex == EditIndexAddress)
                {
                    Data data = new Data();
                    MobiusServiceLibrary.Address adres = new MobiusServiceLibrary.Address();
                    adres = data.Address[e.Item.ItemIndex];

                    PlaceHolder p = e.Item.FindControl("AddressLine1EditPlaceholder") as PlaceHolder;

                    TextBox t = new TextBox();
                    t.ID = "AddressLine1Edit";
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).AddressLine1;
                    t.Width = Unit.Pixel(400);
                    // Add the textbox to the placeholder
                    p.Controls.Add(t);
                    // Get the existing label and hide it
                    Label l = e.Item.FindControl("AddressLine1") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("AddressLine2EditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "AddressLine2Edit";
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).AddressLine2;
                    p.Controls.Add(t);
                    l = e.Item.FindControl("AddressLine2") as Label;
                    l.Visible = false;

                    p = e.Item.FindControl("AddressIdEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "AddressIdEdit";
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).Id.ToString();
                    p.Controls.Add(t);
                    l = e.Item.FindControl("AddressID") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("CountryEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "CountryEdit";
                    t.Width = Unit.Pixel(50); ;
                    t.Enabled = false;
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).City.State.Country.CountryName;

                    p.Controls.Add(t);
                    l = e.Item.FindControl("Country") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("StateEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "StateEdit";
                    t.Width = Unit.Pixel(50); 
                    t.Enabled = false;
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).City.State.StateName;

                    p.Controls.Add(t);
                    l = e.Item.FindControl("State") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("CityEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "CityEdit";
                    t.Width = 120;
                    t.Enabled = false;
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).City.CityName;

                    p.Controls.Add(t);
                    l = e.Item.FindControl("City") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("ZipEditPlaceholder") as PlaceHolder;
                    t = new TextBox();
                    t.ID = "ZipEdit";
                    t.Text = ((MobiusServiceLibrary.Address)e.Item.DataItem).Zip;
                    t.Width =Unit.Pixel(50);
                    t.Attributes.Add("onchange", "GetLocalityByZipCode(this);");
                    p.Controls.Add(t);
                    l = e.Item.FindControl("Zip") as Label;
                    l.Visible = false;


                    p = e.Item.FindControl("StatusEditPlaceholder") as PlaceHolder;
                    DropDownList d = new DropDownList();
                    d.ID = "StatusEdit";
                    d.Width = Unit.Pixel(100); 
                    List<ListItem> status = new List<ListItem>();
                    status.Add(new ListItem("Primary", "1"));
                    status.Add(new ListItem("Secondary ", "2"));
                    status.Add(new ListItem("Inactive", "3"));
                    d.DataSource = status;
                    d.DataBind();
                    //d.SelectedIndex = 0;
                    p.Controls.Add(d);
                    l = e.Item.FindControl("Status") as Label;
                    l.Visible = false;

                    // Make hidden fields visible
                    HtmlInputHidden h = e.Item.FindControl("AddressLine1eHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("AddressLine2eHidden") as HtmlInputHidden;
                    h.Visible = true;
                    h = e.Item.FindControl("AddressIdHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("CountryHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("StateHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("CityHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("ZipHidden") as HtmlInputHidden;
                    h.Visible = true;

                    h = e.Item.FindControl("StatusHidden") as HtmlInputHidden;
                    h.Visible = true;

                    // Remove the edit button from display
                    ImageButton b = e.Item.FindControl("EditA") as ImageButton;

                    b.Visible = false;

                    // Re use the delete button
                    b = e.Item.FindControl("DeleteA") as ImageButton;
                    b.CommandName = "saveA";
                    b.OnClientClick = "OnSaveAddress(this)";
                    b.ImageUrl = "~/images/base_floppydisk_32.png";
                    b.ToolTip = "Save";
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList NewStatus = e.Item.FindControl("NewStatus") as DropDownList;
                if (NewStatus != null)
                {
                    NewStatus.DataSource = Enum.GetValues(typeof(AddressStatus));
                    NewStatus.DataBind();
                    NewStatus.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region Event rptr_Address_ItemCommand
    protected void rptr_Address_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Data data = new Data();
        try
        {
            BindAddressDetails(data);
            if (e.Item.ItemIndex != -1)
            {
                if (data.Address.Count > e.Item.ItemIndex)
                {
                    int CurrentItemIndex = 0;
                    HtmlInputHidden hdnIdValue = e.Item.FindControl("AddressIdHidden") as HtmlInputHidden;

                    if (hdnIdValue.Value == "0" || string.IsNullOrEmpty(hdnIdValue.Value))
                    {
                        CurrentItemIndex = data.Address.IndexOf(data.Address.FindAll(item => item.Action != ActionType.Delete)[e.Item.ItemIndex]);
                    }
                    else
                    {
                        CurrentItemIndex = data.Address.IndexOf(data.Address.First(addItem => addItem.Id.ToString() == hdnIdValue.Value));
                    }

                    if (e.CommandName == "deleteA")
                    {
                        if (data.Address[CurrentItemIndex] != null)
                            data.Address[CurrentItemIndex].Action = ActionType.Delete;
                        EditIndexAddress = -1;
                    }
                    else if (e.CommandName == "editA")
                    {
                        EditIndexAddress = e.Item.ItemIndex;
                    }
                    else if (e.CommandName == "saveA")
                    {

                        HtmlInputHidden t = e.Item.FindControl("AddressLine1eHidden") as HtmlInputHidden;
                        data.Address[CurrentItemIndex].AddressLine1 = t.Value;

                        t = e.Item.FindControl("AddressLine2eHidden") as HtmlInputHidden;
                        data.Address[CurrentItemIndex].AddressLine2 = t.Value;

                        t = e.Item.FindControl("CountryHidden") as HtmlInputHidden;
                        data.Address[CurrentItemIndex].City.State.Country.CountryName = t.Value;

                        t = e.Item.FindControl("StateHidden") as HtmlInputHidden;
                        data.Address[CurrentItemIndex].City.State.StateName = t.Value;

                        t = e.Item.FindControl("CityHidden") as HtmlInputHidden;
                        data.Address[CurrentItemIndex].City.CityName = t.Value;

                        t = e.Item.FindControl("ZipHidden") as HtmlInputHidden;
                        data.Address[CurrentItemIndex].Zip = t.Value;

                        t = e.Item.FindControl("StatusHidden") as HtmlInputHidden;
                        data.Address[CurrentItemIndex].AddressStatus = (AddressStatus)Enum.Parse(typeof(AddressStatus), t.Value, true);

                        if (data.Address[CurrentItemIndex].Action != ActionType.Insert)
                            data.Address[CurrentItemIndex].Action = ActionType.Update;
                        EditIndexAddress = -1;
                    }


                    rptr_Address.DataSource = data.Address.FindAll(t => t.Action != ActionType.Delete);
                    rptr_Address.DataBind();
                }
                else
                    ExceptionHelper.HandleException(page: Page, displayMessage: "Record Not Found");
            }
        }
        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page, ex: ex);
        }
    }

    #endregion

    #region BindRepeaterItems

    protected void BindRepeaterItems()
    {
        Data data = new Data();
        rptr_Address.DataSource = data.Address.FindAll(t => t.Action != ActionType.Delete);
        rptr_Address.DataBind();
    }

    #endregion

    #region BtnUpdate_Click

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Data data = new Data();
        UpdatePatientResponse patientResponse = null;
        UpdatePatientRequest patientRequest = null;
        SoapHandler soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
        SoapProperties soapProperties = new SoapProperties();
        patientRequest = new UpdatePatientRequest();
        patientRequest.Patient = new MobiusServiceLibrary.Patient();
        MobiusServiceLibrary.Patient patient = new MobiusServiceLibrary.Patient();
        try
        {

            patientRequest.Patient.EmailAddress = txtUserEmail.Text;
            patientRequest.Patient.CommunityId = MobiusAppSettingReader.LocalHomeCommunityID;
            patientRequest.Patient.PatientId = hdnPatientID.Value;

            patient.SSN = txtSSN.Text;
            patient.DOB = txtDOB.Text;
            //patient.Gender = ddlSex.SelectedItem.Text == "Male" ? "M" : "F";
            patient.Gender = (Mobius.CoreLibrary.Gender)Enum.Parse(typeof(Mobius.CoreLibrary.Gender), ddlSex.SelectedValue, true);
            patient.LocalMPIID = hdnMPIID.Value;
            patient.EmailAddress = patientRequest.Patient.EmailAddress;
            patient.CommunityId = patientRequest.Patient.CommunityId;
            patient.PatientId = patientRequest.Patient.PatientId;
            patientRequest.Patient = patient;
            // ends

            // Mother Name
            patientRequest.Patient.MothersMaidenName = new Name();
            patientRequest.Patient.MothersMaidenName.Prefix = txtMotherPrefix.Text;
            patientRequest.Patient.MothersMaidenName.GivenName = txtMotherGivenName.Text;
            patientRequest.Patient.MothersMaidenName.MiddleName = txtMotherMiddleName.Text;
            patientRequest.Patient.MothersMaidenName.FamilyName = txtMotherFamilyName.Text;
            patientRequest.Patient.MothersMaidenName.Suffix = txtMotherSuffix.Text;

            // Telephone
          
            patientRequest.Patient.Telephones = data.Telephone;

            // Demographics
            // MobiusServiceLibrary.Patient patient = new MobiusServiceLibrary.Patient();

            data.Names.ForEach(x =>
            {
                if(!string.IsNullOrEmpty(x.FirstName))
                    patientRequest.Patient.GivenName.Add(x.FirstName);
                else
                    patientRequest.Patient.GivenName.Add(string.Empty);

                if (patientRequest.Patient.MiddleName == null)
                    patientRequest.Patient.MiddleName = new List<string>();

                if (!string.IsNullOrEmpty(x.MiddleName))
                    patientRequest.Patient.MiddleName.Add(x.MiddleName);
                else
                    patientRequest.Patient.MiddleName.Add(string.Empty);

                if (patientRequest.Patient.Prefix == null)
                    patientRequest.Patient.Prefix = new List<string>();

                if (!string.IsNullOrEmpty(x.PrefixName))
                    patientRequest.Patient.Prefix.Add(x.PrefixName);
                else
                    patientRequest.Patient.Prefix.Add(string.Empty);

                if (patientRequest.Patient.Suffix == null)
                    patientRequest.Patient.Suffix = new List<string>();

                if (!string.IsNullOrEmpty(x.SuffixName))
                 patientRequest.Patient.Suffix.Add(x.SuffixName);
                else
                    patientRequest.Patient.Suffix.Add(string.Empty);

                if (patientRequest.Patient.FamilyName == null)
                    patientRequest.Patient.FamilyName = new List<string>();


                if (!string.IsNullOrEmpty(x.LastName))
                    patientRequest.Patient.FamilyName.Add(x.LastName);
                else
                    patientRequest.Patient.FamilyName.Add(string.Empty);

                if (x.IDName >= 0)
                    patientRequest.Patient.IDNames.Add(x.IDName);
                else
                    patientRequest.Patient.IDNames.Add(0);

                if (patientRequest.Patient.Action == null)
                    patientRequest.Patient.Action=new List<ActionType>();

                if (x.Action != null)
                    patientRequest.Patient.Action.Add(x.Action);
                else
                    patientRequest.Patient.Action.Add(ActionType.NoChange);
             });
  
           
            patientRequest.Patient.PatientAddress = data.Address; 

            //Birth Place Address
            patientRequest.Patient.BirthPlaceAddress = txtBirthPlacestreetAddress.Text;
            patientRequest.Patient.BirthPlaceCity = txtBirthPlaceCity.Text;
            patientRequest.Patient.BirthPlaceState = txtBirthPlaceState.Text;
            patientRequest.Patient.BirthPlaceCountry = txtBirthPlaceCountry.Text;
            patientRequest.Patient.BirthPlaceZip = txtBirthPlaceZip.Text;


            //  objProxy = new MobiusSecuredClient();
            // Request Encryption
            soapHandler.RequestEncryption(patientRequest, out soapProperties);
            patientRequest.SoapProperties = soapProperties;
            patientResponse = objProxy.UpdatePatient(patientRequest);

            if (soapHandler.ResponseDecryption(patientResponse.SoapProperties, patientResponse))
            {
                lblErrorMsg.Text = patientResponse.Result.ErrorMessage;
                lblErrorMsg.Visible = true;
                GetPatientDetailsResponse patientNewResponse = null;
                if (patientResponse.Result.IsSuccess)
                {
                    GetPatientDetailsRequest getPatientDetailsRequest = new GetPatientDetailsRequest();
                    getPatientDetailsRequest.MPIID = Convert.ToString(GlobalSessions.SessionItem(SessionItem.MPIID));
                    patientNewResponse = objProxy.GetPatientDetails(getPatientDetailsRequest);
                    EditPatient(patientNewResponse.Patient);
                    //deletedAddress = null;
                    //deletedNames = null;
                    //deletedTelephone = null;
                }
            }
            else
            {
                lblErrorMsg.Text = INVALID_RESPONSE_DATA;
            }
        }

        catch (Exception ex)
        {
            ExceptionHelper.HandleException(page: Page,ex:ex, displayMessage: UPDATE_PATIENT_FAILED);
        }

    }
    #endregion

    #region GetLocality
    [WebMethod]
    public static string GetLocality(string Zipcode)
    {   //13056
        string cityname = string.Empty;
        string statename = string.Empty;
        string CountryName = string.Empty;
        GetLocalityByZipCodeResponse getLocalityByZipCodeResponse = new GetLocalityByZipCodeResponse();
        MobiusClient HISEProxy = new MobiusClient();
        getLocalityByZipCodeResponse = HISEProxy.GetLocalityByZipCode(Zipcode);

        if (getLocalityByZipCodeResponse.Result.IsSuccess)
        {
            cityname = getLocalityByZipCodeResponse.City.CityName;
            statename = getLocalityByZipCodeResponse.City.State.StateName;
            CountryName = getLocalityByZipCodeResponse.City.State.Country.CountryName;
        }

        return cityname + "," + statename + "," + CountryName;
    }

    #endregion

    #region Private Methods

    private void BindGenderList()
    {
        ddlSex.Items.Clear();
        foreach (Mobius.CoreLibrary.Gender Gender in Enum.GetValues(typeof(Mobius.CoreLibrary.Gender)))
        {
            string name = Enum.GetName(typeof(Mobius.CoreLibrary.Gender), Gender);
            ddlSex.Items.Add(name);
        }

    }

    private void EditPatient(Patient patient)
    {
        BindGenderList();
        ddlSex.SelectedIndex = ((Mobius.CoreLibrary.Gender)Enum.Parse(typeof(Mobius.CoreLibrary.Gender), patient.Gender.ToString(), true)).GetHashCode();
        txtSSN.Attributes["value"] = patient.SSN;
        //txtSSN.Enabled = false;
        //ddlSex.DataSource = BindGenderList();
        //ddlSex.DataBind();
        //ddlSex.SelectedItem.Text = patient.Gender.ToString();
        // ddlSex.SelectedIndex = patient.Gender.ToString().ToUpper().Equals("Male".ToUpper()) ? 0 :  patient.Gender.ToString().ToUpper().Equals("Female".ToUpper())? 1 : 2;
        txtDOB.Text = patient.DOB;

        txtUserEmail.Text = patient.EmailAddress;
        hdnPatientID.Value = patient.PatientId;
        
        //Bind Mother Name
        if (patient.MothersMaidenName != null && !string.IsNullOrEmpty(patient.MothersMaidenName.Prefix))
            txtMotherPrefix.Text = patient.MothersMaidenName.Prefix;
        if (patient.MothersMaidenName != null && !string.IsNullOrEmpty(patient.MothersMaidenName.GivenName))
            txtMotherGivenName.Text = patient.MothersMaidenName.GivenName;
        if (patient.MothersMaidenName != null && !string.IsNullOrEmpty(patient.MothersMaidenName.MiddleName))
            txtMotherMiddleName.Text = patient.MothersMaidenName.MiddleName;
        if (patient.MothersMaidenName != null && !string.IsNullOrEmpty(patient.MothersMaidenName.FamilyName))
            txtMotherFamilyName.Text = patient.MothersMaidenName.FamilyName;
        if (patient.MothersMaidenName != null && !string.IsNullOrEmpty(patient.MothersMaidenName.Suffix))
            txtMotherSuffix.Text = patient.MothersMaidenName.Suffix;

        //Bind Birth Address

        if(patient!=null && !string.IsNullOrEmpty(patient.BirthPlaceCity))
            txtBirthPlaceCity.Text=patient.BirthPlaceCity;
        if (patient != null && !string.IsNullOrEmpty(patient.BirthPlaceCountry))
            txtBirthPlaceCountry.Text = patient.BirthPlaceCountry;
        if (patient != null && !string.IsNullOrEmpty(patient.BirthPlaceState))
            txtBirthPlaceState.Text = patient.BirthPlaceState;
        if (patient != null && !string.IsNullOrEmpty(patient.BirthPlaceZip))
            txtBirthPlaceZip.Text = patient.BirthPlaceZip;
        if (patient != null && !string.IsNullOrEmpty(patient.BirthPlaceAddress))
            txtBirthPlacestreetAddress.Text = patient.BirthPlaceAddress;

        //CHECK null HANDLING FOR FOLLOWING PIECE OF CODE.

        hdnMPIID.Value = patient.LocalMPIID;
        Data data = new Data();
        data.Names = new List<Names>();
        Names name = null;
        int i = 0;
        foreach (string givenName in patient.GivenName)
        {
            name = new Names();
            name.IDName = patient.IDNames.ToArray()[i];
            name.FirstName = givenName;
            name.MiddleName = patient.MiddleName.ToArray()[i];
            name.LastName = patient.FamilyName.ToArray()[i];
            name.SuffixName = patient.Suffix != null ? patient.Suffix.ToArray()[i] : string.Empty;
            name.PrefixName = patient.Prefix != null ? patient.Prefix.ToArray()[i] : string.Empty;
            name.Action = ActionType.NoChange;
            data.Names.Add(name);
            i++;
        }
        EditIndexName = -1;
        rptrName.DataSource = data.Names.FindAll(t => t.Action != ActionType.Delete); ;
        rptrName.DataBind();


        EditIndexTelephone = -1;
        data.Telephone = patient.Telephones;
        data.Telephone.ForEach(t => t.Action = ActionType.NoChange);
        rptrTelephone.DataSource = data.Telephone.FindAll(t => t.Action != ActionType.Delete);
        rptrTelephone.DataBind();


        EditIndexAddress = -1;
        data.Address = patient.PatientAddress;
        data.Address.ForEach(t => t.Action = ActionType.NoChange);
        rptr_Address.DataSource = data.Address.FindAll(t => t.Action != ActionType.Delete); ;
        rptr_Address.DataBind();
    }

    /// <summary>
    /// This method initializes all repeater and other controls.
    /// </summary>
    private void LoadData()
    {
        Data data = new Data();
        data.Names.Clear();
        rptrName.DataSource = data.Names.FindAll(t => t.Action != ActionType.Delete);
        rptrName.DataBind();

        data.Address.Clear();
        rptr_Address.DataSource = data.Address.FindAll(t => t.Action != ActionType.Delete); ;
        rptr_Address.DataBind();

        data.Telephone.Clear();
        rptrTelephone.DataSource = data.Telephone.FindAll(t => t.Action != ActionType.Delete); ;
        rptrTelephone.DataBind();

        MPIID = Convert.ToString(GlobalSessions.SessionItem(SessionItem.MPIID));
        if (!string.IsNullOrEmpty(MPIID))
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            SoapProperties soapProperties = new SoapProperties();


            GetPatientDetailsResponse patientResponse = null;
            GetPatientDetailsRequest getPatientDetailsRequest = new GetPatientDetailsRequest();
            getPatientDetailsRequest.MPIID = MPIID;

            soapHandler.RequestEncryption(getPatientDetailsRequest, out soapProperties);
            getPatientDetailsRequest.SoapProperties = soapProperties;
            patientResponse = objProxy.GetPatientDetails(getPatientDetailsRequest);

            if (soapHandler.ResponseDecryption(patientResponse.SoapProperties, patientResponse))
            {
                if (patientResponse.Result.IsSuccess)
                {
                    EditPatient(patientResponse.Patient);
                }
                else
                {
                    lblErrorMsg.Text = patientResponse.Result.ErrorMessage;
                    lblErrorMsg.Visible = true;
                }
            }
            else
            {
                lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                lblErrorMsg.Visible = true;
            }

        }

        else
        {
            EditIndexName = -1;
            EditIndexTelephone = -1;
            EditIndexAddress = -1;
            btnUpdate.Enabled = false;

            data = new Data();
            rptrName.DataSource = data.Names;
            rptrName.DataBind();

            rptrTelephone.DataSource = data.Telephone;
            rptrTelephone.DataBind();

            rptr_Address.DataSource = data.Address;
            rptr_Address.DataBind();

            BindGenderList();
            //ddlSex.DataBind();

            //********************************************************************PT
            //ddlSex.DataSource = Enum.GetValues(typeof(Gender));
            // ddlSex.DataBind();
            ddlSex.SelectedIndex = 0;
            //********************************************************************ENDS

        }
    }

    private void BindAddressDetails( Data data) 
    {
        
        if (EditIndexName != -1)
        {
            EditIndexName = -1;
            rptrName.DataSource = data.Names.FindAll(t => t.Action != ActionType.Delete); ;
            rptrName.DataBind();

        }
        else if (EditIndexTelephone != -1)
        {
            EditIndexTelephone = -1;
            rptrTelephone.DataSource = data.Telephone.FindAll(t => t.Action != ActionType.Delete); ;
            rptrTelephone.DataBind();

        }

    }

    private void BindNameDetails(Data data)
    {
        if (EditIndexAddress != -1)
        {
            EditIndexAddress = -1;
            rptr_Address.DataSource = data.Address.FindAll(t => t.Action != ActionType.Delete); ;
            rptr_Address.DataBind();

        }
        else if (EditIndexTelephone != -1)
        {
            EditIndexTelephone = -1;
            rptrTelephone.DataSource = data.Telephone.FindAll(t => t.Action != ActionType.Delete); ;
            rptrTelephone.DataBind();

        }
    }

    private void BindTelephoneDetails(Data data)
    {
        if (EditIndexAddress != -1)
        {
            EditIndexAddress = -1;
            rptr_Address.DataSource = data.Address.FindAll(t => t.Action != ActionType.Delete); ;
            rptr_Address.DataBind();

        }
        else if (EditIndexName != -1)
        {
            EditIndexName = -1;
            rptrName.DataSource = data.Names.FindAll(t => t.Action != ActionType.Delete); ;
            rptrName.DataBind();

        }
    }

    private List<ListItem> GetTypes()
    {
        List<ListItem> types = new List<ListItem>();
        types.Add(new ListItem("Work", "1"));
        types.Add(new ListItem("Home", "2"));
        types.Add(new ListItem("Mobile", "3"));
        return types;
    }

    private List<ListItem> TelephoneStatus()
    {
        List<ListItem> telePhoneStatus = new List<ListItem>();
        telePhoneStatus.Add(new ListItem(TeleStatus_Active, "1"));
        telePhoneStatus.Add(new ListItem(TeleStatus_InActive, "0"));
        return telePhoneStatus;
    }

    public string GetTelephoneStatus(string val)
    {
        return val.ToString().Equals("True") ? TeleStatus_Active : TeleStatus_InActive;
    }
    #endregion

    #endregion

    
}
