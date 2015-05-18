using System;
using System.Data;
using System.Data.Common;
using FirstGenesis.Mobius.Server.DataAccessLayer;
using Mobius.CoreLibrary;
using Mobius.Entity;
using System.Security.Cryptography;
using System.Collections.Generic;
namespace Mobius.DAL
{
    public partial class MobiusDAL : IMobiusDAL
    {

        #region Private variable
       // private const string _strAlpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //private const string _strNumeric = "0123456789!@#$%^&*_+-|";
       // private const string _strAlphaNumeric = _strAlpha + _strNumeric;
        #endregion Private variable

        /// <summary>
        /// This method will token string if user is authorized 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="passwordHash"></param>
        /// <param name="facilityID"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        //public string LoginUser(string userID, string passwordHash, int facilityID, out ErrorCode errorCode)
        //{
        //    DbCommand command;
        //    CryptoLib.Crypto cryptoWraper = new CryptoLib.Crypto();
        //    string userGuid = string.Empty;
        //    string hashPass = string.Empty;
        //    string tokenString = string.Empty;
        //    string password = string.Empty;
        //    errorCode = ErrorCode.ErrorSuccess;
        //    int err = 0;
        //    int active = 0;
        //    try
        //    {
        //        DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
        //        command = dataAccessManager.GetStoredProcCommand("CheckAdminLogin");
        //        dataAccessManager.AddInParameter(command, "UserID", DbType.String, userID.ToString());
        //        dataAccessManager.AddInParameter(command, "FacilityID", DbType.Int32, facilityID);
        //        dataAccessManager.AddOutParameter(command, "Active", DbType.Int32, active);
        //        DataSet dataSet = (DataSet)dataAccessManager.ExecuteDataSet(command);

        //        active = Convert.ToInt32(command.Parameters["Active"].Value);

        //        if (dataSet != null)
        //        {
        //            if (dataSet.Tables[0].Rows.Count > 0)
        //            {
        //                password = dataSet.Tables[0].Rows[0]["PassHash"].ToString();
        //                userGuid = dataSet.Tables[0].Rows[0]["userGuid"].ToString();

        //                cryptoWraper.Hash(password, out hashPass, out err);
        //                if (hashPass.ToLower().ToString() == passwordHash.ToLower().ToString())
        //                {
        //                    tokenString = AddToken(userGuid).ToString();
        //                }
        //                else
        //                {
        //                    errorCode = ErrorCode.InvaidLoginCredential;
        //                }
        //            }
        //            else
        //            {
        //                tokenString = string.Empty;
        //                if (active > 0)
        //                {
        //                    errorCode = ErrorCode.UserIdNotActive;
        //                }
        //                else
        //                {
        //                    errorCode = ErrorCode.InvaidLoginCredential;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            tokenString = string.Empty;
        //            if (active > 0)
        //            {
        //                errorCode = ErrorCode.UserIdNotActive;
        //            }
        //            else
        //            {
        //                errorCode = ErrorCode.InvaidLoginCredential;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        errorCode = ErrorCode.UnknownException;
        //    }
        //    return tokenString;
        //}

        /// <summary>
        /// AuthenticateUser
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <param name="serailNumber"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Result AuthenticateUser(string emailAddress, string password, int userType, out string serailNumber, out string Name)
        {
            serailNumber = string.Empty;
            Name = string.Empty;
            bool active = false;
            try
            {
                DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
                using (DbCommand command = dataAccessManager.GetStoredProcCommand("AuthenticateUser"))
                {
                    dataAccessManager.AddInParameter(command, "EmailAddress", DbType.String, emailAddress);
                    dataAccessManager.AddInParameter(command, "Password", DbType.String, password);
                    dataAccessManager.AddInParameter(command, "UserType", DbType.String, userType);
                    using (DataSet ds = _dataAccessManager.ExecuteDataSet(command))
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                serailNumber = dr["SerialNumber"].ToString();
                                Name = dr["Name"].ToString();
                                if (dr["Active"] != DBNull.Value)
                                {
                                    active = Convert.ToBoolean(dr["Active"]);
                                }
                            }
                            this.Result.IsSuccess = true;
                            // To check user activated or not 
                            if (!active)
                            {
                                this.Result.IsSuccess = false;
                                this.Result.SetError(ErrorCode.Inactive_Account);
                            }


                        }
                        else
                        {
                            this.Result.SetError(ErrorCode.LoginFail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }


        /// <summary>
        /// This method return the referral permission 
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="subject"></param>
        /// <param name="purposeOfUse"></param>
        /// <returns></returns>
        public Result GetReferralPermission(string patientId, int subject, int purposeOfUse)
        {
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetReferralPermission"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, patientId);
                    _dataAccessManager.AddInParameter(dbCommand, "@Subject", DbType.Int32, subject);
                    _dataAccessManager.AddInParameter(dbCommand, "@Code", DbType.String, purposeOfUse);
                    using (DataSet dataSet = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {
                        if (dataSet.Tables.Count > 0)
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                this.Result.IsSuccess = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this.Result;
        }

        /// <summary>
        /// This method will verify if the logged in user has acess permissions to share the document.
        /// </summary>
        /// <param name="documentRequest"></param>
        /// <param name="patientConsentId"></param>
        /// <returns></returns>
        public Result HasAccessPermission(DocumentRequest documentRequest, out int patientConsentId)
        {
            bool bAccessPermission = false;
            patientConsentId = 0;
            int errorResponse = 0;
            try
            {
                    using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("HasAccessPermission"))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, documentRequest.patientId);
                        _dataAccessManager.AddInParameter(dbCommand, "@Subject", DbType.String, documentRequest.subjectRole);
                        _dataAccessManager.AddInParameter(dbCommand, "@Code", DbType.String, documentRequest.purpose);
                        using (DataSet dataSet = _dataAccessManager.ExecuteDataSet(dbCommand))
                        {
                            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                            {
                                errorResponse = (int)dataSet.Tables[0].Rows[0]["Value"];
                                switch (errorResponse)
                                {
                                    case 1:
                                        if (Convert.ToInt32(dataSet.Tables[0].Rows[0]["PatientConsentID"]) != 0)
                                        {
                                            patientConsentId = Convert.ToInt32(dataSet.Tables[0].Rows[0][1]);
                                        }
                                        bAccessPermission = true;
                                        break;
                                    case 2:
                                        patientConsentId = -2;
                                        bAccessPermission = false;
                                        break;

                                    case 3:
                                        patientConsentId = -3;
                                        bAccessPermission = false;
                                        break;

                                    case 4:
                                        patientConsentId = -4;
                                        bAccessPermission = false;
                                        break;

                                    case 5:
                                        patientConsentId = -5;
                                        bAccessPermission = false;
                                        break;

                                    case 6:
                                        patientConsentId = -6;
                                        bAccessPermission = false;
                                        break;

                                    case 7:
                                        patientConsentId = -7;
                                        bAccessPermission = false;
                                        break;
                                    case 8:
                                        patientConsentId = -8;
                                        bAccessPermission = false;
                                        break;
                                }
                            }
                        }
                    }
                    this.Result.IsSuccess = bAccessPermission;
                    if (!bAccessPermission)
                    {
                        //if (documentRequest.purpose==PurposeOfUse.EMERGENCY.ToString())   //case of emergency access
                        //{
                        //    this.Result = CheckEmergencyAudit(documentRequest.patientId, documentRequest.documentId, documentRequest.subjectEmailID);
                        //    if (this.Result.IsSuccess)
                        //        return this.Result;
                        //}
                        if (patientConsentId < 1)
                        {
                            if (patientConsentId == -2)
                            {
                                this.Result.SetError(ErrorCode.Patient_Consent_Missing, string.Format("Consent not mapped for patient {0}. Fail to share.", documentRequest.patientId).ToString());
                                return this.Result;
                            }
                            else
                            {
                                this.Result.SetError(ErrorCode.Patient_Consent_Deviated);
                                return this.Result;
                            }
                        }
                    }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this.Result;
        }

     

        /// <summary>
        /// This method will return the type of user
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public UserType GetTypeOfUser(string serialNumber)
        {
            UserType userType = new UserType();
            int i = 0;
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetTypeOfUser"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@SerialNumber", DbType.String, serialNumber);
                    i = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    userType = i != 0 ? UserType.Patient : UserType.Unspecified;
                }
            }
            catch (Exception)
            {
                throw new Exception(Helper.GetErrorMessage(ErrorCode.UnknownException));
            }
            return userType;
        }

        /// <summary>
        /// This method will return user information 
        /// </summary>
        /// <param name="certificateSerialNumber"></param>
        /// <returns></returns>
        public UserInfo GetUserInformation(string certificateSerialNumber)
        {
            UserInfo userInfo = new UserInfo();

            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetUserInformation"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@SerialNumber", DbType.String, certificateSerialNumber);
                    using (DataSet dsuserinfo = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {
                        if (dsuserinfo.Tables.Count > 0 && dsuserinfo.Tables[0].Rows.Count > 0)
                        {
                            if (dsuserinfo.Tables[0].Rows[0]["UserType"].ToString() == "0")
                            {
                                Result.IsSuccess = false;
                                userInfo = null;

                            }
                            else
                            {
                                DataRow row = dsuserinfo.Tables[0].Rows[0];

                                if (row["IsOptIn"] != DBNull.Value)
                                    userInfo.IsOptIn = Convert.ToBoolean(dsuserinfo.Tables[0].Rows[0]["IsOptIn"]);

                                if (row["MPIID"] != DBNull.Value)
                                    userInfo.MPIID = row["MPIID"].ToString();

                                //if (row["Name"] != DBNull.Value)
                                //    userInfo.Name = row["Name"].ToString();

                                if (row["UserType"] != DBNull.Value)
                                    userInfo.UserType = (UserType)row["UserType"];

                                if (row["PublicKey"] != DBNull.Value)
                                    userInfo.PublicKey = row["PublicKey"].ToString();

                                if (row["CommunityId"] != DBNull.Value)
                                    userInfo.CommunityId = row["CommunityId"].ToString();

                                if (row["Role"] != DBNull.Value)
                                    userInfo.Role = row["Role"].ToString();

                                if (row["Email"] != DBNull.Value)
                                    userInfo.EmailAddress = row["Email"].ToString();

                                if (row["UserRoleCode"] != DBNull.Value)
                                    userInfo.UserRoleCode = row["UserRoleCode"].ToString();

                                userInfo.Name = new Name();

                                if (row["Prefix"] != DBNull.Value)
                                    userInfo.Name.Prefix = row["Prefix"].ToString();

                                if (row["GivenName"] != DBNull.Value)
                                    userInfo.Name.GivenName = row["GivenName"].ToString();

                                if (row["MiddleName"] != DBNull.Value)
                                    userInfo.Name.MiddleName = row["MiddleName"].ToString();

                                if (row["FamilyName"] != DBNull.Value)
                                    userInfo.Name.FamilyName = row["FamilyName"].ToString();

                                if (row["Suffix"] != DBNull.Value)
                                    userInfo.Name.Suffix = row["Suffix"].ToString();


                                if (userInfo.UserType == UserType.Provider)
                                {
                                    if (row["IndividualProvider"] != DBNull.Value && !Convert.ToBoolean(row["IndividualProvider"]) && row["OrganizationName"] != DBNull.Value)
                                    {
                                        userInfo.Name.GivenName = row["OrganizationName"].ToString();
                                    }
                                }
                                Result.IsSuccess = true;
                            }
                        }
                        else
                        {
                            Result.IsSuccess = false;
                            userInfo = null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception(Helper.GetErrorMessage(ErrorCode.UnknownException));
            }

            return userInfo;
        }

        /// <summary>
        /// This method will check patient OptIn status
        /// </summary>
        /// <param name="patientId">Patient ID</param>
        /// <param name="status">Return OptIn status</param>
        /// <returns></returns>
        public Result GetOptInStatus(string patientId, out bool status)
        {
            status = false;
            try
            {
                this.Result.IsSuccess = true;
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetOptInStatus"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, patientId);
                    object value = _dataAccessManager.ExecuteScalar(dbCommand);
                    if (value != null)
                    {
                        status = (bool)value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }


        #region EmergencyAccess

        /// <summary>
        /// This method will return the type of user
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public Result SaveEmergencyAudit(DocumentRequest doc)
        {
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("SaveEmergencyAudit"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@MPIID", DbType.String, doc.patientId);
                    _dataAccessManager.AddInParameter(dbCommand, "@DocumentId", DbType.String, doc.documentId);
                    _dataAccessManager.AddInParameter(dbCommand, "@OverrideDate", DbType.String, DateTime.Now);
                    _dataAccessManager.AddInParameter(dbCommand, "@AccessEndDate", DbType.String, DateTime.Now.AddHours(MobiusAppSettingReader.EmergencyOverriddenTime));
                    _dataAccessManager.AddInParameter(dbCommand, "@OverriddenBy", DbType.String, doc.subjectEmailID);
                    _dataAccessManager.AddInParameter(dbCommand, "@OverrideReasonId", DbType.String, Convert.ToInt32(doc.OverrideReason));
                    _dataAccessManager.AddInParameter(dbCommand, "@Description", DbType.String, doc.Description);
                    _dataAccessManager.AddInParameter(dbCommand, "@IsAudited", DbType.String, false);
                    _dataAccessManager.AddInParameter(dbCommand, "@UserRole", DbType.String, doc.subjectRole);

                    int value = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if (value > 0)
                    {   
                        this.Result.IsSuccess = true;
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.Error_in_emergency_access);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Error_in_emergency_access, ex.Message);
            }
            return this.Result;
        }







        /// <summary>
        ///  To get All the instance of Emergency Audit
        /// </summary>
        /// <param name="isShowAll"></param>
        /// <param name="lstEmergencyAudit"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public Result GetAllEmergencyAudit(EmergencyRecords emergencyRecords, out List<EmergencyAudit> lstEmergencyAudit, string patientId)
        {
            lstEmergencyAudit = new List<EmergencyAudit>();
            DbCommand dbCommand = null;
            DataSet ds = new DataSet();
            try
            {
                EmergencyAudit emergencyAudit = null;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetAllEmergencyAudit");
                _dataAccessManager.AddInParameter(dbCommand, "EmergencyRecords", DbType.Int32,Convert.ToInt32(emergencyRecords));
                _dataAccessManager.AddInParameter(dbCommand, "PatientId", DbType.String, patientId);
                ds = _dataAccessManager.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        emergencyAudit = new EmergencyAudit();
                        if (dr["Id"] != DBNull.Value)
                            emergencyAudit.EmergencyAuditId = Convert.ToInt32(dr["Id"]);
                        if (dr["PatientName"] != DBNull.Value)
                            emergencyAudit.PatientName = Convert.ToString(dr["PatientName"]);
                        if (dr["MPIId"] != DBNull.Value)
                            emergencyAudit.MPIId = Convert.ToString(dr["MPIId"]);
                        if (dr["DocumentId"] != DBNull.Value)
                            emergencyAudit.DocumentId = Convert.ToString(dr["DocumentId"]);
                        if (dr["OverrideDate"] != DBNull.Value)
                        {
                            emergencyAudit.OverrideDate = Convert.ToDateTime(dr["OverrideDate"]);
                        }
                        if (dr["OverriddenBy"] != DBNull.Value)
                            emergencyAudit.OverriddenBy = Convert.ToString(dr["OverriddenBy"]);
                        if (dr["ProviderName"] != DBNull.Value)
                            emergencyAudit.ProviderName = Convert.ToString(dr["ProviderName"]);

                        if (dr["Reason"] != DBNull.Value)
                        {
                            OverrideReason purposeOfUse;
                            Enum.TryParse(Convert.ToString(dr["Reason"]), out purposeOfUse);
                            emergencyAudit.OverrideReason = purposeOfUse;
                        }
                        if (dr["IsAudited"] != DBNull.Value)
                            emergencyAudit.IsAudited = Convert.ToBoolean(dr["IsAudited"]);
                        lstEmergencyAudit.Add(emergencyAudit);
                    }
                }
                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return this.Result;
        }

        /// <summary>
        /// Get the emergency Details by id
        /// </summary>
        /// <param name="auditID"></param>
        /// <param name="emergencyAudit"></param>
        /// <returns></returns>
        public Result GetEmergencyDetailById(int auditID, out EmergencyAudit emergencyAudit)
        {
            DbCommand dbCommand = null;
            DataSet ds = new DataSet();
            emergencyAudit = null;
            try
            {

                dbCommand = _dataAccessManager.GetStoredProcCommand("GetEmergencyAuditById");
                _dataAccessManager.AddInParameter(dbCommand, "@AuditID", DbType.Int32, auditID);
                ds = _dataAccessManager.ExecuteDataSet(dbCommand);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        emergencyAudit = new EmergencyAudit();
                        if (dr["PatientName"] != DBNull.Value)
                            emergencyAudit.PatientName = Convert.ToString(dr["PatientName"]);
                        if (dr["MPIId"] != DBNull.Value)
                            emergencyAudit.MPIId = Convert.ToString(dr["MPIId"]);
                        if (dr["DocumentId"] != DBNull.Value)
                            emergencyAudit.DocumentId = Convert.ToString(dr["DocumentId"]);
                        if (dr["OverrideDate"] != DBNull.Value)
                        {
                            emergencyAudit.OverrideDate = Convert.ToDateTime(dr["OverrideDate"]);
                        }
                        if (dr["OverriddenBy"] != DBNull.Value)
                            emergencyAudit.OverriddenBy = Convert.ToString(dr["OverriddenBy"]);
                        if (dr["Reason"] != DBNull.Value)
                        {
                            OverrideReason purposeOfUse;
                            Enum.TryParse(Convert.ToString(dr["Reason"]), out purposeOfUse);
                            emergencyAudit.OverrideReason = purposeOfUse;
                        }


                        if (dr["IsAudited"] != DBNull.Value)
                            emergencyAudit.IsAudited = Convert.ToBoolean(dr["IsAudited"]);
                        if (dr["Gender"] != DBNull.Value)
                            emergencyAudit.PatientGender = Convert.ToString(dr["Gender"]);
                        if (dr["DOB"] != DBNull.Value)
                            emergencyAudit.PatientDOB = Convert.ToDateTime(dr["DOB"]);
                        if (dr["UserRole"] != DBNull.Value)
                            emergencyAudit.ProviderRole = Convert.ToString(dr["UserRole"]);
                        if (dr["ProviderName"] != DBNull.Value)
                            emergencyAudit.ProviderName = Convert.ToString(dr["ProviderName"]);
                        if (dr["Description"] != DBNull.Value)
                            emergencyAudit.Description = Convert.ToString(dr["Description"]);
                    }
                }
                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return this.Result;
        }

        /// <summary>
        /// To update the Emergency override instances
        /// </summary>
        /// <param name="lstAuditID"></param>
        /// <param name="isAuditStatus"></param>
        /// <returns></returns>
        public Result UpdateOverrideDetails(List<int> lstAuditID, bool isAuditStatus)
        {

            DbCommand dbCommand = null;
            try
            {
                if (lstAuditID.Count > 0)
                {
                    string IdList = string.Join(",", lstAuditID);
                    dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateEmergencyAudit");
                    _dataAccessManager.AddInParameter(dbCommand, "@IdList", DbType.String, IdList);
                    _dataAccessManager.AddInParameter(dbCommand, "@IsAuditStatus", DbType.Boolean, isAuditStatus);
                    _dataAccessManager.ExecuteNonQuery(dbCommand);
                    this.Result.IsSuccess = true;
                }
                else
                {
                    this.Result.IsSuccess = false;
                    return this.Result;
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.Error_in_updating_audit_status, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }

            }
            return this.Result;

        }



        #endregion



        #region GetSerialNumber
        /// <summary>
        /// GetSerialNumber
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="userType"></param>
        /// <param name="serailNumber"></param>
        /// <returns></returns>
        public Result GetSerialNumber(string emailAddress, int userType, out string serailNumber)
        {
            serailNumber = string.Empty;
            try
            {
                DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
                using (DbCommand command = dataAccessManager.GetStoredProcCommand("GetSerialNumber"))
                {
                    dataAccessManager.AddInParameter(command, "EmailAddress", DbType.String, emailAddress);
                    dataAccessManager.AddInParameter(command, "UserType", DbType.String, userType);

                    if (_dataAccessManager.ExecuteScalar(command) != null)
                    {
                        serailNumber = _dataAccessManager.ExecuteScalar(command).ToString();
                    }

                    if (!string.IsNullOrEmpty(serailNumber))
                    {
                        this.Result.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }
        #endregion

        #region Helper Methods

        //private string AddToken(string userGUID)
        //{
        //    string tokenString = GetTokenString().ToString();
        //    int returnValue;
        //    try
        //    {

        //        using (DbCommand command = DataAccessManager.GetInstance.GetStoredProcCommand("AddToken"))
        //        {
        //            DataAccessManager.GetInstance.AddInParameter(command, "Token", DbType.String, tokenString);
        //            DataAccessManager.GetInstance.AddInParameter(command, "UserGUID", DbType.String, userGUID);
        //            returnValue = Convert.ToInt32(DataAccessManager.GetInstance.ExecuteNonQuery(command));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        tokenString = "";
        //    }
        //    return tokenString;
        //}

        //private string GetTokenString()
        //{
        //    string randomData = String.Empty;
        //    try
        //    {
        //        int position = 0;
        //        byte[] data = new byte[50];
        //        int characterSetLength = _strAlphaNumeric.Length;
        //        RandomNumberGenerator random = RandomNumberGenerator.Create();
        //        random.GetBytes(data);
        //        for (int index = 0; (index < 50); index++)
        //        {
        //            position = data[index];
        //            position = (position % characterSetLength);
        //            randomData = (randomData + _strAlphaNumeric.Substring(position, 1));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        randomData = "";
        //    }
        //    return randomData;
        //}

        /// <summary>
        /// to check whether A provider has done emergency access in last EmergencyExpireHours 
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="documentID"></param>
        /// <param name="subjectEmailID"></param>
        /// <returns></returns>
        public Result CheckEmergencyAudit(DocumentRequest documentRequest)
        {
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("CheckEmergencyAudit"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, documentRequest.patientId);
                    _dataAccessManager.AddInParameter(dbCommand, "@ProviderEmail", DbType.String, documentRequest.subjectEmailID);
                    _dataAccessManager.AddInParameter(dbCommand, "@DocumentId", DbType.String, documentRequest.documentId);
                    using (DataSet dataSet = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {
                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            this.Result.IsSuccess = (bool)dataSet.Tables[0].Rows[0][0];
                        }
                        else
                        {
                            this.Result.IsSuccess = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this.Result;
        }

        #endregion Helper Methods
    }
}
