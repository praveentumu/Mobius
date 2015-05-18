#region Namespace
using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using MobiusServiceLibrary;
using CERTENROLLLib;
using System.ServiceModel;
using Mobius.CoreLibrary;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI;
using System.Linq;
using System.Text.RegularExpressions;
using Mobius.Entity;
using C32Utility;
using MobiusServiceUtility;
using System.ComponentModel;
using CERTCLIENTLib;
#endregion

namespace FirstGenesis.UI.Base
{
    public partial class BaseClass : System.Web.UI.Page
    {
        #region Variables
        private const string AUTHENTICATION_FAILED = "Authentication failed";
        protected const string INVALID_RESPONSE_DATA = "Invalid response data";
        private const string INVALID_CERTIFICATE = "Invalid Certificate";
        private const string X509_CERTIFICATE_NOT_FOUND = "Cannot find the X.509 certificate using the following search criteria";
        private const string UNABLE_TO_CONNECT_SERVER = "There was a problem connecting to server";
        private const string INVALID_USER = "InvalidUser.";
        private const string SERIALNUMBER = "SerialNumber.";
        private const string CERTIFICATE_NOT_FOUND = @"User has not enabled this site for application access. Please use <<Allow Login>> option from login screen to enable the same.";
        private const string Session_expired = "Your session has expired.";
        private string emailAddress = string.Empty;
        protected MobiusSecuredClient objProxy = null;
        X509Certificate2Collection certCollection = null;
        public bool isCertificateExpired = false;
        #endregion

        #region Properties
        protected string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        #endregion

        #region Events
        /// <summary>
        /// Base Class Constructor
        /// </summary>
        public BaseClass()
        {
            objProxy = new MobiusSecuredClient();
        }
        /// <summary>
        ///  OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            bool IsSessionexpired = false;
            bool IsCertificateMissing = false;

            X509Store store = null;
            string message = string.Empty;
            try
            {
                string SerialNumber = string.Empty;

                if (GlobalSessions.SessionItem(SessionItem.SerialNumber) == null)
                {

                    switch (Page.GetType().Name.ToUpper())
                    {
                        case "CHANGEPASSWORD_ASPX":
                            IsSessionexpired = true;
                            message = Session_expired;
                            Response.End();
                            break;
                    }
                    return;
                }

                if (GlobalSessions.SessionItem(SessionItem.SerialNumber) != null)
                {
                    SerialNumber = GlobalSessions.SessionItem(SessionItem.SerialNumber).ToString();
                    store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadWrite);
                    certCollection = store.Certificates.Find(X509FindType.FindBySerialNumber, SerialNumber, true);
                    if (certCollection != null && certCollection.Count > 0)
                    {
                        objProxy.ClientCredentials.ClientCertificate.Certificate = certCollection[0];
                        GlobalSessions.SessionAdd(SessionItem.ValidTill, certCollection[0].NotAfter);
                    }
                    else
                    {
                        //Added an additional check to verify for certificates lying in store is 'Invalid Certificate'. 
                        //Earlier it was throwing CERTIFICATE_NOT_FOUND error, even if certificate was present, but was in Invalid state.
                        //Updated the last parameter in following call from True to False (to pick certificates irrespective of it's validity). 
                        certCollection = store.Certificates.Find(X509FindType.FindBySerialNumber, SerialNumber, false);
                        if (certCollection != null && certCollection.Count > 0)
                        {
                            if (IsExpired(certCollection[0]))
                            {
                                isCertificateExpired = true;
                                GlobalSessions.SessionAdd(SessionItem.RenewalNotificationSent, true);
                                message = Helper.GetErrorMessage(ErrorCode.Account_Expired);
                                Response.End();
                            }
                        }
                    }

                    if (!Page.IsPostBack)
                    {

                        SoapHandler soapHandler = new SoapHandler(Convert.ToString(GlobalSessions.SessionItem(SessionItem.SerialNumber)));

                        UserInformationResponse userInfo = new UserInformationResponse();
                        if (certCollection != null && certCollection.Count > 0)
                        {
                            userInfo = objProxy.GetUserInformation();
                        }
                        if (userInfo.Result.IsSuccess)
                        {
                            if (soapHandler.ResponseDecryption(userInfo.SoapProperties, userInfo))
                            {
                                if (userInfo == null)
                                {
                                    IsCertificateMissing = true;
                                    message = AUTHENTICATION_FAILED;
                                    Response.End();

                                }
                                //Load community and store into session 
                                GetNhinCommunity();

                                if (GlobalSessions.SessionItem(SessionItem.UserInformation) == null)
                                {
                                    GlobalSessions.SessionAdd(SessionItem.UserInformation, userInfo.UserInformation);

                                    if (userInfo.UserInformation.UserType == UserType.Patient)
                                    {
                                        if (userInfo.UserInformation.Name == null || string.IsNullOrWhiteSpace(userInfo.UserInformation.MPIID))
                                        {
                                            IsCertificateMissing = true;
                                            message = AUTHENTICATION_FAILED;
                                            Response.End();
                                        }

                                        

                                        GlobalSessions.SessionAdd(SessionItem.UserType, userInfo.UserInformation.UserType);

                                        if (userInfo.UserInformation.Name != null)
                                        {
                                            string name = "";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.Prefix))
                                                name = userInfo.UserInformation.Name.Prefix + " ";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.GivenName))
                                                name = name + userInfo.UserInformation.Name.GivenName + " ";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.MiddleName))
                                                name = name + userInfo.UserInformation.Name.MiddleName + " ";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.FamilyName))
                                                name = name + userInfo.UserInformation.Name.FamilyName + " ";

                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.Suffix))
                                                name = name + userInfo.UserInformation.Name.Suffix + " ";

                                            GlobalSessions.SessionAdd(SessionItem.UserName, name);
                                        }
                                        if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.MPIID))
                                        {
                                            GlobalSessions.SessionAdd(SessionItem.MPIID, userInfo.UserInformation.MPIID);
                                        }
                                        if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Role))
                                        {
                                            GlobalSessions.SessionAdd(SessionItem.UserRole, userInfo.UserInformation.Role);
                                        }
                                        if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.EmailAddress))
                                        {
                                            GlobalSessions.SessionAdd(SessionItem.UserEmailAddress, userInfo.UserInformation.EmailAddress);
                                        }
                                        GlobalSessions.SessionAdd(SessionItem.IsOptIn, userInfo.UserInformation.IsOptIn);
                                    }
                                    else if (userInfo.UserInformation.UserType == UserType.Provider)
                                    {
                                        if (userInfo.UserInformation.Name == null || string.IsNullOrWhiteSpace(userInfo.UserInformation.Role))
                                        {
                                            IsCertificateMissing = true;
                                            message = AUTHENTICATION_FAILED;
                                            Response.End();
                                        }

                                        GlobalSessions.SessionAdd(SessionItem.UserType, userInfo.UserInformation.UserType);

                                        if (userInfo.UserInformation.Name != null)
                                        {
                                            string name = "";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.Prefix))
                                                name = userInfo.UserInformation.Name.Prefix + " ";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.GivenName))
                                                name = name + userInfo.UserInformation.Name.GivenName + " ";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.MiddleName))
                                                name = name + userInfo.UserInformation.Name.MiddleName + " ";
                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.FamilyName))
                                                name = name + userInfo.UserInformation.Name.FamilyName + " ";

                                            if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Name.Suffix))
                                                name = name + userInfo.UserInformation.Name.Suffix + " ";

                                            GlobalSessions.SessionAdd(SessionItem.UserName, name.Trim());
                                        }

                                        if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.Role))
                                        {
                                            GlobalSessions.SessionAdd(SessionItem.UserRole, userInfo.UserInformation.Role);
                                        }

                                        if (!string.IsNullOrWhiteSpace(userInfo.UserInformation.EmailAddress))
                                        {
                                            GlobalSessions.SessionAdd(SessionItem.UserEmailAddress, userInfo.UserInformation.EmailAddress);
                                        }
                                    }
                                    else if (userInfo.UserInformation.UserType == UserType.Unspecified)
                                    {
                                        // Nothing  to do
                                    }
                                }
                            }
                            else
                            {
                                IsCertificateMissing = true;
                                message = INVALID_RESPONSE_DATA;
                                Response.End();
                            }
                        }
                        else
                        {
                            IsCertificateMissing = true;
                            message = CERTIFICATE_NOT_FOUND;
                            Response.End();
                        }
                    }
                    if ((GlobalSessions.SessionItem(SessionItem.UserEmailAddress) != null) && (GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString() != string.Empty))
                    {
                        this.EmailAddress = (string)GlobalSessions.SessionItem(SessionItem.UserEmailAddress);
                    }
                }
            }
            catch (FaultException fe)
            {
                if (fe.Code.Name.Equals(INVALID_CERTIFICATE, StringComparison.OrdinalIgnoreCase) || fe.Code.Name.Equals(INVALID_USER, StringComparison.OrdinalIgnoreCase))
                {
                    promptErrorMessage(fe.Message.ToString());

                }
                IsCertificateMissing = true;
            }

            catch (CommunicationException cm)
            {
                message = UNABLE_TO_CONNECT_SERVER;
                promptErrorMessage(message);
                IsCertificateMissing = true;
            }
            catch (System.Threading.ThreadAbortException)
            {
                if (IsSessionexpired || IsCertificateMissing)
                {
                    promptErrorMessage(message);
                }
                if (isCertificateExpired)
                {
                    promptExpiredCertificateErrorMessage();
                }
                // used to handle expired certificate
                //if (isCertificateExpired)
                //    promptExpiredCertificateErrorMessage(message);

            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains(X509_CERTIFICATE_NOT_FOUND))
                {
                    promptErrorMessage(CERTIFICATE_NOT_FOUND);
                }

            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
        }

        protected void OpenWindow()
        {
            string strScript = "<script>";
            strScript += "window.opener.location='ContactUs.aspx?MODE=REFRESH';";
            //strScript += "window.close();";
            strScript += "</script>";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "ContactUs", strScript);
        }

        private void GetNhinCommunity()
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
                    }
                }
            }
        }

        /// <summary>
        /// prompt Error Message
        /// </summary>
        /// <param name="message"></param>

        private void promptErrorMessage(string message)
        {
            Response.Write(" <script language='javascript' type='text/javascript'>alert('" + message + "');  top.location.href = 'login.aspx' </script>");
        }
        #endregion

        private void promptExpiredCertificateErrorMessage()
        {
            string EmailAddress = GlobalSessions.SessionItem(SessionItem.UserEmailAddress).ToString();
            Response.Write(" <script language='javascript' type='text/javascript'> top.location.href = 'login.aspx?IsExpired=true&Email=" + EmailAddress + "'</script>");
        }


        #region PublicMethod

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listbox"></param>
        /// <param name="masterData"></param>
        /// <param name="IncludeSelect"></param>
        protected void FillListBox(ListBox listbox, List<MobiusServiceLibrary.MasterData> masterData)
        {
            listbox.DataSource = masterData;
            listbox.DataValueField = "Code";
            listbox.DataTextField = "Description";
            listbox.DataBind();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DropDownList"></param>
        /// <param name="masterData"></param>
        /// <param name="IncludeSelect"></param>
        protected void FillDropDown(DropDownList DropDownList, List<MobiusServiceLibrary.MasterData> masterData, bool IncludeSelect = true)
        {

            DropDownList.DataSource = masterData;
            DropDownList.DataValueField = "Code";
            DropDownList.DataTextField = "Description";
            DropDownList.DataBind();
            if (IncludeSelect)
            {
                DropDownList.Items.Insert(0, new ListItem("--Select--", "0"));
            }

        }

        /// <summary>
        /// Create Generate CSR request
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="commonName"></param>
        /// <param name="organizationalUnit"></param>
        /// <param name="organizationName"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="country"></param>
        /// <returns></returns>


        /// <summary>
        /// This method will check attached C32 Document is belongs to respective patient or not
        /// </summary>
        /// <param name="c32DocumentBytes"></param>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Result ValidateDocumentForPatient(byte[] c32DocumentBytes, MobiusServiceLibrary.Patient patient)
        {
            Result result = new Result();
            try
            {
                string patientGender = string.Empty;
                DateTime patientDOB = new DateTime();
                //byte[] c32DocumentBytes = ;

                //Load & validate XML of C32 Document 
                CDAHelper CDAHelper = new C32Utility.CDAHelper(c32DocumentBytes);

                //get Patient Gender
                patientGender = CDAHelper.PatientGender;
                if (patientGender == "M")
                {
                    patientGender = "Male";
                }
                else if (patientGender == "F")
                {
                    patientGender = "Female";
                }
                else
                {
                    patientGender = "Unspecified";
                }

                //Get Patient DBO
                if (!string.IsNullOrEmpty(CDAHelper.PatientDOB))
                    patientDOB = DateTime.ParseExact(CDAHelper.PatientDOB, "yyyyMMdd", new System.Globalization.DateTimeFormatInfo());

                //Validate Patient Gender and DBO
                if (!patient.Gender.ToString().Equals(patientGender, StringComparison.OrdinalIgnoreCase) || Convert.ToDateTime(patient.DOB) != patientDOB)
                {
                    result.SetError(ErrorCode.UnknownException, "Document content being uploaded, doesn't matches with the patient basic details (given name, family name, gender, date of birth). Please verify.");
                    return result;
                }

                //Validate Patient Name =>FirstName and LastName
                //if (CDAHelper.PatientName.GivenName.Count(t => patient.GivenName.Any(a => a.ToString() == t.ToString())) >= 1
                //    &&
                //    CDAHelper.PatientName.FamilyName.Count(t => patient.FamilyName.Any(a => a.ToString() == t.ToString())) >= 1)

                if(CDAHelper.PatientName != null && CDAHelper.PatientName.GivenName != null && CDAHelper.PatientName.GivenName.Count > 0 &&
                            CDAHelper.PatientName.FamilyName != null && CDAHelper.PatientName.FamilyName.Count > 0)
                {
                    result.IsSuccess = true;

                }
                else
                {
                    result.SetError(ErrorCode.UnknownException, "Document content being uploaded, doesn't matches with the patient basic details (given name, family name, gender, date of birth). Please verify.");
                }
            }
            catch (Exception ex)
            {
                result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return result;
        }


        public MobiusServiceLibrary.User GetUserInformation(NHINCommunity homeNHINCommunity)
        {
            MobiusServiceLibrary.User user = new MobiusServiceLibrary.User();
            UserInformation userInfo = GlobalSessions.SessionItem(SessionItem.UserInformation) as MobiusServiceLibrary.UserInformation;
            user.HomeCommunity = homeNHINCommunity;
            user.Name = new MobiusServiceLibrary.Name();
            user.Name = userInfo.Name;// CreatePersonName(userInfo.Name);
            UserRole role;


            role = (UserRole)EnumHelper.GetEnumFromDescription<DescriptionAttribute>(typeof(UserRole), userInfo.Role);

            user.Role = role;
            user.UserName = userInfo.EmailAddress;
            return user;
        }


        private MobiusServiceLibrary.Name CreatePersonName(string nameParam)
        {
            MobiusServiceLibrary.Name personNameType = new MobiusServiceLibrary.Name();
            if (!string.IsNullOrEmpty(nameParam))
            {
                var name = nameParam.Split(' ');
                if (name.Count() >= 2)
                {
                    personNameType.FamilyName = name[1];
                    if (name.Count() == 3)
                        personNameType.FamilyName = name[2];
                    personNameType.GivenName = name[0];
                }
                else if (name.Count() == 1)
                {
                    personNameType.GivenName = name[0];
                }
            }
            return personNameType;
        }

        #endregion



    }


}
