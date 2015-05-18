using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mobius.CoreLibrary;
using Mobius.Entity;
using System.IO;
using System.Xml;
using System.Text;
using FirstGenesis.UI;
using System.Xml.Linq;

public partial class ViewConfigFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (GlobalSessions.SessionItem(SessionItem.XACMLBytes) != null)
                {
                    byte[] XACMLBytes = (byte[])GlobalSessions.SessionItem(SessionItem.XACMLBytes);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/xml";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Policy.xml");
                    HttpContext.Current.Response.OutputStream.Write(XACMLBytes, 0, XACMLBytes.Length);
                    HttpContext.Current.Response.End();
                }
                else
                {
                    Response.Write("Could not locate the file.");
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(page: Page, ex: ex);
            }
        }
    }
}