using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.IO;
using Mobius.CoreLibrary;
using FirstGenesis.UI.Base;
using System.Xml;
using Mobius.Entity;
using MobiusServiceLibrary;
using FirstGenesis.UI;
using MobiusServiceUtility;
using System.Collections.Generic;
using System.Linq;
public partial class ViewShareDocument : BaseClass
{
    private DataTable docInfo = new DataTable();
    SoapHandler soapHandler;
    SoapProperties soapProperties = new SoapProperties();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));
            if (!Page.IsPostBack)
            {
                if (HttpContext.Current.Request.QueryString["Token"] != null)
                {
                    if (!(string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Token"].ToString())))
                    {
                        GetDocumentResponse getDocumentResponse = new GetDocumentResponse();
                        GetDocumentRequest getDocumentRequest = new GetDocumentRequest();
                        getDocumentRequest.patientId = HttpContext.Current.Request.QueryString["patientId"].ToString(); ;
                        getDocumentRequest.documentId = HttpContext.Current.Request.QueryString["Token"].ToString(); ;
                        getDocumentRequest.purpose = "TREATMENT"; ;
                        getDocumentRequest.subjectRole =  GlobalSessions.SessionItem(SessionItem.UserRole).ToString() ;
                        getDocumentRequest.subjectEmailID = this.EmailAddress;

                        List<NHINCommunity> nhinCommunitiesSession = (List<NHINCommunity>)GlobalSessions.SessionItem(SessionItem.CommunityList);

                        if (nhinCommunitiesSession == null)
                            nhinCommunitiesSession = GetNhinCommunities();

                        NHINCommunity homeNHINCommunity = nhinCommunitiesSession.Where(t => t.IsHomeCommunity).FirstOrDefault();
                        
                        AssertionHelper assertion = new AssertionHelper();
                        getDocumentRequest.Assertion = assertion.CreateAssertion(AssertionMode.Default, AssertionAction.DocumentRetrieve,
                        PurposeOfUse.TREATMENT, base.GetUserInformation(homeNHINCommunity), homeNHINCommunity);

                        soapHandler.RequestEncryption(getDocumentRequest, out soapProperties);
                        getDocumentRequest.SoapProperties = soapProperties;
                        getDocumentResponse = this.objProxy.GetDocument(getDocumentRequest);
                        if (getDocumentResponse.Result.IsSuccess)
                        {
                            if (soapHandler.ResponseDecryption(getDocumentResponse.SoapProperties, getDocumentResponse))
                            {
                                Document docData = getDocumentResponse.Document;
                                if (docData.DocumentBytes != null)
                                {
                                    // Code for showing doc byte data in a page 
                                    //MemoryStream ms = new MemoryStream(docData.DocumentBytes);
                                    //XmlDocument xdoc = new XmlDocument();
                                    //xdoc.Load(ms);
                                    ////Session["XMLDOC"] = xdoc;
                                    GlobalSessions.SessionAdd(SessionItem.XMLDOC, docData.DocumentBytes);
                                    Response.Redirect("ViewDoDDocument.aspx", false);
                                    //ScriptManager.RegisterStartupScript(this, typeof(ViewShareDocument), "ViewDocument", "window.open('ViewDoDDocument.aspx','','scrollbars=yes,menubar=no,height=600,width=900,resizable=yes,toolbar=no,location=no,status=no');", true);
                                }
                                else if (docData.DocumentBytes == null)
                                {
                                    htnErrorMessage.Value = getDocumentResponse.Result.ErrorMessage;
                                }
                            }
                            else
                            {
                                lblErrorMsg.Text = INVALID_RESPONSE_DATA;
                                return;
                            }
                        }
                        else
                        {
                            htnErrorMessage.Value = getDocumentResponse.Result.ErrorMessage;
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
    private List<NHINCommunity> GetNhinCommunities()
    {
        SoapHandler soapHandler = new SoapHandler();
        GetNhinCommunityResponse nhinCommunityResponse = objProxy.GetNhinCommunity();
        if (nhinCommunityResponse.Result.IsSuccess)
        {
            if (soapHandler.ResponseDecryption(nhinCommunityResponse.SoapProperties, nhinCommunityResponse))
            {
                if (nhinCommunityResponse.Communities != null)
                {
                    List<NHINCommunity> nhinCommunities = new List<NHINCommunity>(nhinCommunityResponse.Communities);
                    GlobalSessions.SessionAdd(SessionItem.CommunityList, nhinCommunities);
                    return nhinCommunities;
                }
            }
        }
        return new List<NHINCommunity>();
    }
}