using System;
using System.Xml;
using System.Xml.Xsl;
using System.Web.Security;
using System.Data;
using FirstGenesis.UI;
public partial class ViewC32Document : System.Web.UI.Page
{
    //AdminWebService.Service objProxy = new AdminWebService.Service();
    MobiusClient objproxy = new MobiusClient();
    //ClientConnect.ErrorCode errorCode;

    private string strDocumentId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["DocumentId"] != null)
        {
            strDocumentId = Request.QueryString["DocumentId"].ToString();
        }
        if (!string.IsNullOrEmpty(strDocumentId))
        {
            if (ValidateUserForDocumentView())
            {
                XmlDocument docXML = new XmlDocument();
                docXML.Load(Server.MapPath("") + "\\Shared\\" + strDocumentId + ".xml");
                XslTransform docXSL = new XslTransform();
                docXSL.Load(Server.MapPath("") + "\\Shared\\WebViewLayout_CDA.xsl");
                xmlViewC32Document.Document = docXML;
                xmlViewC32Document.Transform = docXSL;
                DataSet dsAttachment=null;
                //dsAttachment = objProxy.GetSharedAttachment(strDocumentId, Convert.ToString(Session["UserEmail"]), out errorCode);
                if (dsAttachment.Tables.Count > 0)
                {
                    if (dsAttachment.Tables[0].Rows.Count > 0)
                    {
                        tblAttachments.Visible = true;

                        lnkViewAttachments.Attributes.Add("onclick", "javascript:OpenAttachmentWindow('" + strDocumentId + "');");
                    }
                    else
                    {
                        tblAttachments.Visible = false;
                    }
                }
                else
                {
                    tblAttachments.Visible = false;
                }
            }
            else
            {                
                if (GlobalSessions.SessionItem(SessionItem.Token) != null)                
                {             
                    GlobalSessions.SessionAdd(SessionItem.Token, null);        
                }
                Session.Abandon();
                Request.Cookies.Clear();
                string cookieName = FormsAuthentication.FormsCookieName;
                Response.Cookies[cookieName].Expires = DateTime.Now;
                Response.Redirect("Default.aspx?ErrorMsg=You are not authorized to view this document, please contact system administrator if this was not intended.&MODE=" + Utility.Encrypt("ViewDocument") + "&DocumentId=" + strDocumentId);
            }
        }
    }

    private bool ValidateUserForDocumentView()
    {
        //return objProxy.ValidateUserForDocumentView(Session["UserEmail"].ToString(), strDocumentId, out errorCode);
        return false;
    }
}
