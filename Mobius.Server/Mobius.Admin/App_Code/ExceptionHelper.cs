using System;
using System.Web;
using System.Web.UI;

namespace FirstGenesis.UI
{
    public class ExceptionHelper
    {
        public enum ErrorTitle
        {
            Error,
            Warning,
            Message,
            HelpText
        }

        public static void HandleException(Page page, Exception ex = null, string displayMessage = null, bool IsPopup = false, ErrorTitle title = ErrorTitle.Error)
        {
            try
            {
                if (string.IsNullOrEmpty(displayMessage))
                    GlobalSessions.SessionAdd(SessionItem.ErrorMessage, ex.Message);
                else
                    GlobalSessions.SessionAdd(SessionItem.ErrorMessage, displayMessage);

                if (IsPopup)
                {
                    HttpContext.Current.Response.Redirect("Error.aspx", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(page, typeof(ExceptionHelper), "ExceptionHelper", "onError('" + title.ToString() + "');", true);
                }

            }
            catch (System.Threading.ThreadAbortException tEx)
            {
                tEx = null;
                //if (IsPopup)
                //    HttpContext.Current.Response.Redirect("Error.aspx", false);
            }
            catch (Exception exException)
            {
                GlobalSessions.SessionAdd(SessionItem.ErrorMessage, exException.Message);
                if (IsPopup)
                    HttpContext.Current.Response.Redirect("Error.aspx", false);
            }
        }
    }
}
