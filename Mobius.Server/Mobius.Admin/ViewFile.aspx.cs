using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mobius.CoreLibrary;
using Mobius.BAL;
using Mobius.Entity;
using System.IO;
using System.Xml;
using System.Text;


using FirstGenesis.UI;

public partial class ViewConfigFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string filePath = MobiusAppSettingReader.ResourceFilePath + @"\Configuration.xml";
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                HttpContext.Current.Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment;filename=ConfigurationSetting.xml");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "text/plain";
                Response.TransmitFile(file.FullName);
                Response.End();
            }
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