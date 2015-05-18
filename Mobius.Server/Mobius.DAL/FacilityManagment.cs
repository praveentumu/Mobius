

namespace Mobius.DAL
{
    #region Namespace
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using FirstGenesis.Mobius.Server.DataAccessLayer;
    using Mobius.Entity;
    using System.Data.SqlClient;
    using Mobius.CoreLibrary;
    #endregion

    public partial class MobiusDAL
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public string GetFacilityName(int facilityId)
        {
            string facilityName = String.Empty;
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetFacilityName"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.Int32, facilityId);
                    //
                    facilityName = _dataAccessManager.ExecuteScalar(dbCommand).ToString();
                }
            }
            catch (Exception)
            {
                //TODO
            }
            return facilityName;
        }

        /// <summary>
        /// RegisterProvider to save provider's details in the table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="providerRequest">Takes Provider Request object</param>
        /// <returns>Returns Boolean value</returns>
        public Result AddProvider(Provider provider)
        {
            Int64 ProviderID = 0;
            Result result = new Result();
            string specialtie = string.Empty;
            try
            {
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("AddProvider"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "IndividualProvider", DbType.Boolean, provider.IndividualProvider);
                    _dataAccessManager.AddInParameter(dbCommand, "ProviderTypeID", DbType.Int32, Convert.ToInt32(provider.ProviderType));
                    _dataAccessManager.AddInParameter(dbCommand, "StatusID", DbType.Int16, provider.Status.GetHashCode());
                    _dataAccessManager.AddInParameter(dbCommand, "ContactNo", DbType.String, provider.ContactNumber);
                    _dataAccessManager.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, provider.CreatedOn);
                    _dataAccessManager.AddInParameter(dbCommand, "ExpiryOn", DbType.DateTime, provider.ExpiryiOn);
                    if (string.IsNullOrWhiteSpace(provider.ElectronicServiceURI))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "ElectronicServiceURI", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "ElectronicServiceURI", DbType.String, provider.ElectronicServiceURI);
                    }
                    if (string.IsNullOrWhiteSpace(provider.MedicalRecordsDeliveryEmailAddress.Trim()))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MedicalRecordsDeliveryEmailAddress", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "MedicalRecordsDeliveryEmailAddress", DbType.String, provider.MedicalRecordsDeliveryEmailAddress);
                    }
                    if (string.IsNullOrWhiteSpace(provider.StreetNumber))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "StreetNumber", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "StreetNumber", DbType.String, provider.StreetNumber);
                    }
                    if (string.IsNullOrWhiteSpace(provider.StreetName))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "StreetName", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "StreetName", DbType.String, provider.StreetName);
                    }
                    if (provider.City != null)
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "CityName", DbType.String, provider.City.CityName);
                        if (provider.City.State != null)
                        {
                            _dataAccessManager.AddInParameter(dbCommand, "StateNAme", DbType.String, provider.City.State.StateName);
                            if (provider.City.State.Country != null)
                            {
                                _dataAccessManager.AddInParameter(dbCommand, "CountryName", DbType.String, provider.City.State.Country.CountryName);
                            }
                        }
                    }
                    _dataAccessManager.AddInParameter(dbCommand, "PostalCode", DbType.String, provider.PostalCode);
                    _dataAccessManager.AddInParameter(dbCommand, "LanguageID", DbType.Int32, provider.Language.LanguageId);
                    if (string.IsNullOrWhiteSpace(provider.Identifier))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "Identifier", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "Identifier", DbType.String, provider.Identifier);
                    }
                    if (provider.IndividualProvider == true)
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "GenderID", DbType.Int32, Convert.ToInt32(provider.Gender));
                        _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, provider.Email);
                        _dataAccessManager.AddInParameter(dbCommand, "ProviderFirstName", DbType.String, provider.FirstName);
                        _dataAccessManager.AddInParameter(dbCommand, "ProviderMiddleName", DbType.String, provider.MiddleName);
                        _dataAccessManager.AddInParameter(dbCommand, "ProviderLastName", DbType.String, provider.LastName);
                    }

                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "OrganizationName", DbType.String, provider.OrganizationName);
                    }
                    if (string.IsNullOrWhiteSpace(provider.Password))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "Password", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "Password", DbType.String, provider.Password);
                    }
                    _dataAccessManager.AddInParameter(dbCommand, "PublicKey", DbType.String, provider.PublicKey);
                    _dataAccessManager.AddInParameter(dbCommand, "CertificateSerialNumber", DbType.String, provider.CertificateSerialNumber);
                    //Get Sql connection Object
                    DbProviderFactory factory = SqlClientFactory.Instance;
                    using (DbConnection connection = factory.CreateConnection())
                    {
                        connection.ConnectionString = MobiusAppSettingReader.ConnectionString;
                        connection.Open();

                        using (DbTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                ProviderID = Int64.Parse(_dataAccessManager.ExecuteScalar(dbCommand, transaction).ToString());
                                if (ProviderID > 0)
                                {
                                    if (provider.Specialty.Count > 0)
                                    {
                                        ProviderID = AddProviderSpecialty(provider.Specialty, ProviderID, transaction);
                                    }
                                }

                                transaction.Commit();
                            }
                            catch (SqlException)
                            {
                                transaction.Rollback();

                            }
                            catch (Exception ex)
                            {

                                throw new Exception(ex.Message.ToString());
                            }

                        }

                    }
                    if (ProviderID > 0)
                    {
                        result.IsSuccess = true;
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


            return result;
        }

        /// <summary>
        /// Add Pending Provider EnrollmentRequest details in the table
        /// </summary>
        /// <param name="oneTimePassword">Takes oneTimePassword</param>
        /// <param name="validHours">Takes validHours</param>
        /// <param name="pcks7">Takes pcks7 certificate</param>
        /// <param name="pendingEnrollmentRequestId">Give pendingEnrollmentRequestId after recode save in table</param>
        /// <returns>Return Result Object </returns>
        public Result AddPendingProviderEnrollmentRequest(string oneTimePassword, int validHours, string pcks7, out int pendingEnrollmentRequestId)
        {
            pendingEnrollmentRequestId = 0;
            Result result = new Result();
            try
            {
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("AddPendingProviderEnrollmentRequest"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "OTP", DbType.String, oneTimePassword);
                    _dataAccessManager.AddInParameter(dbCommand, "Expires", DbType.Int32, validHours);
                    _dataAccessManager.AddInParameter(dbCommand, "Certificate ", DbType.String, pcks7);
                    pendingEnrollmentRequestId = Int32.Parse(_dataAccessManager.ExecuteScalar(dbCommand).ToString());
                    if (pendingEnrollmentRequestId > 0)
                        result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return result;
        }

        /// <summary>
        /// Bulk insert AddProviderSpecialty in Table
        /// </summary>
        /// <param name="specialties"></param>
        /// <param name="ProviderId"></param>
        /// <returns></returns>
        private int AddProviderSpecialty(List<Specialty> specialties, Int64 ProviderId, DbTransaction dbTransaction)
        {
            DataSet dataSet = new DataSet(); ;
            DataTable dtInsertRows = new DataTable();

            dtInsertRows.TableName = "ProviderSpecialty";
            dtInsertRows.Columns.Add("ProviderId", Type.GetType("System.Int32"));
            dtInsertRows.Columns.Add("Specialty", Type.GetType("System.String"));

            foreach (Specialty specialty in specialties)
            {
                DataRow drInsertRow = dtInsertRows.NewRow();
                drInsertRow["ProviderId"] = ProviderId;
                drInsertRow["Specialty"] = specialty.SpecialtyName;
                dtInsertRows.Rows.Add(drInsertRow);
            }
            //dataSet.Tables[0].Rows.Add(dtInsertRows);
            dataSet.Tables.Add(dtInsertRows);

            DbCommand insertCommand = null;
            DbCommand updateCommand = null;
            DbCommand deleteCommand = null;

            // Establish the Insert commands.
            insertCommand = _dataAccessManager.GetStoredProcCommand("InsertSpecialty");
            _dataAccessManager.AddInParameter(insertCommand, "ProviderId", DbType.Int32, "ProviderId", DataRowVersion.Current);
            _dataAccessManager.AddInParameter(insertCommand, "Specialty", DbType.String, "Specialty", DataRowVersion.Current);

            return _dataAccessManager.UpdateBatch(dataSet, "ProviderSpecialty", insertCommand, updateCommand, deleteCommand, dbTransaction);
            // Establish the Insert commands.

        }

        /// <summary>
        /// Get PCKS Response from data behalf on  oneTimePassword
        /// </summary>
        /// <param name="oneTimePassword"></param>
        /// <returns></returns>
        public Result GetPCKSResponse(string oneTimePassword, out string pCKS7)
        {
            pCKS7 = string.Empty;
            Result result = new Result();
            try
            {
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetPCKSResponse"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "OTP", DbType.String, oneTimePassword);
                    using (DataSet dataSet = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {
                        if (dataSet.Tables.Count > 0)
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                pCKS7 = dataSet.Tables[0].Rows[0]["Certificate"].ToString();
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(pCKS7))
                        {
                            result.IsSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return result;
        }

        /// <summary>
        /// Delete one time password  through database
        /// </summary>
        /// <param name="oneTimePassword"></param>
        public void DeleteOTP(string oneTimePassword)
        {
            try
            {
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteOTP"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "OTP", DbType.String, oneTimePassword);
                    _dataAccessManager.ExecuteNonQuery(dbCommand);
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


        }


        /// <summary>
        /// Get City,State,Country via zip code
        /// </summary>
        /// <param name="ZipCode">string type</param>
        /// <param name="city"></param>
        /// <returns>result class object</returns>
        public Result GetLocalityByZipCode(string ZipCode, out City city)
        {
            city = new City();
            city.State = new State();
            city.State.Country = new Country();
            try
            {
                this.Result.IsSuccess = true;
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetLocalityByZipCode"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "ZipCode", DbType.String, ZipCode);
                    using (DataSet dataSet = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {
                        if (dataSet.Tables.Count > 0)
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                city.CityName = dataSet.Tables[0].Rows[0]["City"].ToString();
                                city.State.StateName = dataSet.Tables[0].Rows[0]["State"].ToString();
                                city.State.Country.CountryName = dataSet.Tables[0].Rows[0]["Country"].ToString();
                            }
                            else
                            {
                                this.Result.IsSuccess = false;
                                this.Result.ErrorMessage = "No data found for given ZipCode";
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return this.Result;

        }


        /// <summary>
        /// This method will update the provider record for new password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public Result UpdateProviderPassword(string emailAddress, string newPassword)
        {
            try
            {
                int returnValue = 0;
                //Modified for issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateProviderPassword"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "EmailAddress", DbType.String, emailAddress);
                    _dataAccessManager.AddInParameter(dbCommand, "Password", DbType.String, newPassword);
                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                    if (returnValue == 1)
                    {
                        this.Result.IsSuccess = true;
                    }
                    else
                    {
                        this.Result.IsSuccess = false;
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






    }
}

