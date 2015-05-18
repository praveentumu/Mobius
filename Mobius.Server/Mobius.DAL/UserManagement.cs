namespace Mobius.DAL
{
    #region Namespace
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using FirstGenesis.Mobius.Server.DataAccessLayer;
    using Mobius.CoreLibrary;
    using Mobius.Entity;
    #endregion
    public partial class MobiusDAL
    {
        #region Private variable
        private Result _Result = null;
        #endregion Private varible

        #region Property
        /// <summary>
        ///  
        /// </summary>
        public Result Result
        {
            get { return _Result != null ? _Result : _Result = new Result(); }
            set { _Result = value; }
        }

        #endregion Property

        #region HasProviderRegistered
        /// <summary>
        /// This method will check provider is registered or not 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Result HasProviderRegistered(string token, Provider provider)
        {
            Result result = null;
            int Count = 0;
            object obj = null;
            try
            {
                result = new Result();
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                //Modified for Issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("HasProviderRegistered"))
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@ReferralId", DbType.Int32, Convert.ToUInt32(token));
                    }
                    if (provider != null)
                    {

                        _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, provider.MedicalRecordsDeliveryEmailAddress);

                    }

                    obj = _dataAccessManager.ExecuteScalar(dbCommand);
                }

                if (obj != null)
                {
                    Count = Int32.Parse(obj.ToString());
                    if (Count > 0)
                    {
                        result.IsSuccess = true;
                    }
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for Issue id #138
            finally
            {
                provider = null;
            }
            return result;
        }
        #endregion


        #region ChangePassword
        /// <summary>
        /// Change user Password
        /// </summary>
        /// <param name="changePassword">ChangePassword object</param>
        /// <returns>changed status in either true or false</returns>
        public Result ChangePassword(ChangePassword changePassword)
        {
            try
            {
                Result result = new Result();
                if (changePassword.UserType == UserType.Provider)
                {
                    this.Result = UpdateProviderPassword(changePassword.EmailAddress, changePassword.NewPassword);
                }
                else if (changePassword.UserType == UserType.Patient)
                {
                    this.Result = UpdatePatientPassword(changePassword.EmailAddress, changePassword.NewPassword);
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for Issue Id #138
            finally
            {
                changePassword = null;
            }

            return this.Result;
        }
        #endregion

        #region Admin Methods

        #region Activate/Deactivate User
        /// <summary>
        /// update user status 
        /// </summary>
        /// <param name="changePassword">ChangePassword object</param>
        /// <returns>changed status in either true or false</returns>
        public Result UpdateUserStatus(string emailAddress, int userType, int action)
        {
            Result result = null;
            int Count = 0;
            try
            {
                result = new Result();
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateUserStatus"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@EmailAddress", DbType.String, emailAddress);
                    _dataAccessManager.AddInParameter(dbCommand, "@UserType", DbType.Int32, userType.GetHashCode());
                    _dataAccessManager.AddInParameter(dbCommand, "@Action", DbType.Int32, action);
                    Count = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if (Count > 0)
                    {
                        result.IsSuccess = true;
                    }
                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return result;
        }
        #endregion

        #region GetAdminDetails
        /// <summary>
        /// get admin information 
        /// </summary>
        /// <param name="adminDetail"></param>
        /// <param name="adminDetails"></param>
        /// <returns></returns>
        public Result GetAdminDetails(AdminDetails adminDetail, out List<AdminDetails> adminDetails)
        {

            DataSet ds = null;
            adminDetails = null;
            try
            {
                adminDetails = new List<AdminDetails>();
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetAdminDetails"))
                {
                    if (adminDetail != null)
                    {
                        if (!string.IsNullOrEmpty(adminDetail.UserName) && !string.IsNullOrEmpty(adminDetail.Password))
                        {
                            _dataAccessManager.AddInParameter(dbCommand, "@UserName", DbType.String, adminDetail.UserName);
                            _dataAccessManager.AddInParameter(dbCommand, "@Password", DbType.String, adminDetail.Password);
                        }
                    }
                    ds = _dataAccessManager.ExecuteDataSet(dbCommand);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        this.Result.IsSuccess = true;
                        adminDetail = new AdminDetails();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["ID"] != DBNull.Value)
                            {
                                adminDetail.ID = Convert.ToInt32(row["ID"]);
                            }
                            if (row["UserName"] != DBNull.Value)
                            {
                                adminDetail.UserName = row["UserName"].ToString();
                            }
                            if (row["Email"] != DBNull.Value)
                            {
                                adminDetail.Email = row["Email"].ToString();
                            }
                            if (row["Password"] != DBNull.Value)
                            {
                                adminDetail.Password = row["Password"].ToString();
                            }
                            adminDetails.Add(adminDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return this.Result;

        }
        #endregion

        #region UpdateAdminDetails
        /// <summary>
        /// Update admin basic information
        /// </summary>
        /// <param name="adminDetails"></param>
        /// <returns></returns>
        public Result UpdateAdminDetails(AdminDetails adminDetails)
        {
            int count = 0;
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;

                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateAdminDetails"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@ID", DbType.Int32, adminDetails.ID);
                    if (!string.IsNullOrEmpty(adminDetails.Email))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@Email", DbType.String, adminDetails.Email);
                    }
                    if (!string.IsNullOrEmpty(adminDetails.Password))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@Password", DbType.String, adminDetails.Password);
                    }
                    count = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if (count > 0)
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

        #region AddAdminDetails
        /// <summary>
        /// insert  Admin information into database
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Result AddAdminDetails(string email, string password = "")
        {
            int count = 0;
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;

                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("AddAdminDetails"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@Email", DbType.String, email);
                    if (!string.IsNullOrEmpty(email))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@Password", DbType.String, password);
                    }
                    count = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if (count > 0)
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

        #endregion
    }
}
