using System;
using System.Xml;
using System.Xml.Xsl;
using FirstGenesis.UI;
using System.IO;
using System.Text;

public partial class ViewDoDDocument : System.Web.UI.Page
{
    string strDocumentId = string.Empty;
    string strSource = string.Empty;
    string strLoadPath = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //if (Request.QueryString["strDocumentId"] != null)
            //    strDocumentId = Convert.ToString(Request.QueryString["strDocumentId"]);

            //if (Request.QueryString["strSource"] != null)
            //    strSource = Convert.ToString(Request.QueryString["strSource"]);

            //if (strSource.ToUpper() == "DOD")
            //    strLoadPath = "\\RetrivedDocuments\\";
            //else
            //    strLoadPath = "\\Upload\\";

            MemoryStream ms = new MemoryStream((byte[])GlobalSessions.SessionItem(SessionItem.XMLDOC));
            XmlDocument docXML = new XmlDocument();
            docXML.Load(ms);
            
            XslTransform docXSL = new XslTransform();
            docXSL.Load(Server.MapPath("") + "\\" + "RetrivedDocuments" + "\\" + "WebViewLayout_CDA.xsl");
            xmlViewC32Document.Document = docXML;
            xmlViewC32Document.Transform = docXSL;

        }
        catch (Exception ex)
        {
            if (GlobalSessions.SessionItem(SessionItem.XMLDOC) != null)
            {  
                Response.Write(Encoding.ASCII.GetString((byte[])GlobalSessions.SessionItem(SessionItem.XMLDOC)));
            }
            else
            {
                ExceptionHelper.HandleException(page: Page, ex: ex, IsPopup: true);
            }
        }
    }
}
