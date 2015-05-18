using System;
using System.Web;
using System.Data;
using Mobius.DAL;
using System.Collections.Generic;
using Mobius.Entity;
using FirstGenesis.UI;
namespace Mobius.Token
{
    public class TokenAuthenticate : IHttpModule
    {

        public DataSet DocumentInformation { get; set; }


        #region IHttpModule Members

        void IHttpModule.Dispose()
        {

        }

        void IHttpModule.Init(HttpApplication context)
        {
            try
            {
                context.AuthenticateRequest += new EventHandler(context_TokenAuthenticate);
            }
            catch (Exception)
            {

            }
        }

        void context_TokenAuthenticate(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToUpper().Equals("~/PATIENTDOCUMENTS"))
            {
                if (HttpContext.Current.Request.QueryString.Count != 0)
                {
                    if (HttpContext.Current.Request.QueryString["token"] != null)
                    {
                        HttpContext.Current.Response.Redirect("Login.aspx?token=" + HttpContext.Current.Request.QueryString["token"].ToString()
                            + "&patientId=" + HttpContext.Current.Request.QueryString["patientId"].ToString()
                            + "&Serial=" + HttpContext.Current.Request.QueryString["Serial"].ToString(), true);
                    }
                }
            }
            else if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToUpper().Equals("~/ACKNOWLEDGEREFERRAL"))
            {
                if (HttpContext.Current.Request.QueryString.Count != 0)
                {
                    if (HttpContext.Current.Request.QueryString["ReferPatientID"] != null)
                    {
                        MobiusDAL mobiusDAL = new DAL.MobiusDAL();

                        if (mobiusDAL.HasProviderRegistered(HttpContext.Current.Request.QueryString["ReferPatientID"].ToString(), null).IsSuccess)
                        {
                            HttpContext.Current.Response.Redirect("Login.aspx?DocumentID=" + HttpContext.Current.Request.QueryString["DocumentID"].ToString()
                                + "&PatientReferralID=" + HttpContext.Current.Request.QueryString["ReferPatientID"].ToString()
                                + "&Serial=" + HttpContext.Current.Request.QueryString["Serial"].ToString(), true);
                        }
                        else
                        {

                            HttpContext.Current.Response.Redirect("RegisterProvider.aspx?DocumentID=" + HttpContext.Current.Request.QueryString["DocumentID"].ToString()
                                + "&PatientReferralID=" + HttpContext.Current.Request.QueryString["ReferPatientID"].ToString(), true);
                        }


                    }
                }
            }
            else if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToUpper().Equals("~/REFERPATIENT"))
            {
                if (HttpContext.Current.Request.QueryString.Count != 0)
                {
                    if (HttpContext.Current.Request.QueryString["Serial"] != null && HttpContext.Current.Request.QueryString["DocumentID"] != null)
                    {
                        HttpContext.Current.Response.Redirect("Login.aspx?DocumentID=" + HttpContext.Current.Request.QueryString["DocumentID"].ToString()
                                                               + "&Serial=" + HttpContext.Current.Request.QueryString["Serial"].ToString(), true);

                    }

                }
            }

        #endregion
        }
    }
}
