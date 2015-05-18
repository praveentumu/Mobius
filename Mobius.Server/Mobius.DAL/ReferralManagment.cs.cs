namespace Mobius.DAL
{
    #region Namespace
    using System;
    using System.Collections.Generic;
    using Mobius.Entity;
    using Mobius.CoreLibrary;
    using System.Data.Common;
    using FirstGenesis.Mobius.Server.DataAccessLayer;
    using System.Data;
    #endregion

    public partial class MobiusDAL : IMobiusDAL
    {
        #region CreatePatientReferral
        /// <summary>
        /// This method will create new patient referral
        /// </summary>
        /// <param name="patientReferred"></param>
        /// <param name="referPatientId"></param>
        /// <returns></returns>
        public Result CreatePatientReferral(PatientReferral patientReferred, out int referPatientId)
        {            
            referPatientId = 0;
            try
            {
                int suceessValue = 0;
                DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
                //Modified for Issue id #138
                using (DbCommand dbCommand = dataAccessManager.GetStoredProcCommand("SavePatientReferral"))
                {
                    dataAccessManager.AddInParameter(dbCommand, "@PatientReferredId", DbType.String, patientReferred.Id);
                    dataAccessManager.AddInParameter(dbCommand, "@ReferredByEmail", DbType.String, patientReferred.ReferredByEmail);
                    dataAccessManager.AddInParameter(dbCommand, "@ReferredToEmail", DbType.String, patientReferred.ReferredToEmail);
                    dataAccessManager.AddInParameter(dbCommand, "@PatientMPIId", DbType.String, patientReferred.Patient.LocalMPIID);
                    if (patientReferred.PurposeOfUse.GetHashCode() == 0)
                        dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32, DBNull.Value);
                    else
                        dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32, patientReferred.PurposeOfUse.GetHashCode());

                    if (string.IsNullOrWhiteSpace(patientReferred.PatientAppointmentDate))
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralDate", DbType.DateTime, DBNull.Value);
                    else
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralDate", DbType.DateTime, Convert.ToDateTime(patientReferred.PatientAppointmentDate));

                    if (Convert.ToDateTime(patientReferred.ReferralAccomplishedOn) == DateTime.MinValue)
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralAccomplishmentDate", DbType.DateTime, DBNull.Value);
                    else
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralAccomplishmentDate", DbType.DateTime, Convert.ToDateTime(patientReferred.ReferralAccomplishedOn));

                    dataAccessManager.AddInParameter(dbCommand, "@ReferralSummary", DbType.String, patientReferred.ReferralSummary);
                    dataAccessManager.AddInParameter(dbCommand, "@DocumentId", DbType.String, patientReferred.DocumentId);
                    dataAccessManager.AddInParameter(dbCommand, "@DispatcherSummary", DbType.String, patientReferred.DispatcherSummary);

                    dataAccessManager.AddInParameter(dbCommand, "@ReferralCompleted", DbType.Boolean, patientReferred.ReferralCompleted);

                    dataAccessManager.AddOutParameter(dbCommand, "@ReferPatientId", DbType.Int32, 0);
                    dataAccessManager.AddInParameter(dbCommand, "@OutcomeDocumentID", DbType.String, patientReferred.OutcomeDocumentID); ;



                    suceessValue = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if ((suceessValue == 1) || (suceessValue == 2))
                    {
                        referPatientId = (int)dbCommand.Parameters["@ReferPatientId"].Value;
                        this.Result.IsSuccess = true;
                    }
                    else
                    {
                        this.Result.SetError(ErrorCode.PatientReferral_Failed);
                    }
                    dataAccessManager = null;
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
                patientReferred = null;
            }
            return this.Result;
        }
        #endregion

        #region AcknowledgePatientReferral
        /// <summary>
        /// This method will save patient referral acknowledgment
        /// </summary>
        /// <param name="patientReferred"></param>
        /// <param name="referPatientId"></param>
        /// <returns></returns>
        public Result AcknowledgePatientReferral(PatientReferral patientReferred, out int referPatientId)
        {            
            referPatientId = 0;
            try
            {
                int suceessValue = 0;
                DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
                //Modified for Issue id #138
                using (DbCommand dbCommand = dataAccessManager.GetStoredProcCommand("SavePatientReferral"))
                {
                    dataAccessManager.AddInParameter(dbCommand, "@PatientReferredId", DbType.String, patientReferred.Id);
                    dataAccessManager.AddInParameter(dbCommand, "@ReferredByEmail", DbType.String, patientReferred.ReferredByEmail);
                    dataAccessManager.AddInParameter(dbCommand, "@ReferredToEmail", DbType.String, patientReferred.ReferredToEmail);
                    dataAccessManager.AddInParameter(dbCommand, "@PatientMPIId", DbType.String, patientReferred.Patient.LocalMPIID);
                    if (patientReferred.PurposeOfUse.GetHashCode() == 0)
                        dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32, DBNull.Value);
                    else
                        dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32, patientReferred.PurposeOfUse.GetHashCode());

                    if (string.IsNullOrWhiteSpace(patientReferred.PatientAppointmentDate))
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralDate", DbType.DateTime, DBNull.Value);
                    else
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralDate", DbType.DateTime, Convert.ToDateTime(patientReferred.PatientAppointmentDate));

                    if (Convert.ToDateTime(patientReferred.ReferralAccomplishedOn) == DateTime.MinValue)
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralAccomplishmentDate", DbType.DateTime, DBNull.Value);
                    else
                        dataAccessManager.AddInParameter(dbCommand, "@ReferralAccomplishmentDate", DbType.DateTime, Convert.ToDateTime(patientReferred.ReferralAccomplishedOn));

                    dataAccessManager.AddInParameter(dbCommand, "@ReferralSummary", DbType.String, patientReferred.ReferralSummary);
                    dataAccessManager.AddInParameter(dbCommand, "@DocumentId", DbType.String, patientReferred.DocumentId);
                    dataAccessManager.AddInParameter(dbCommand, "@DispatcherSummary", DbType.String, patientReferred.DispatcherSummary);

                    dataAccessManager.AddInParameter(dbCommand, "@ReferralCompleted", DbType.Boolean, patientReferred.ReferralCompleted);
                    dataAccessManager.AddInParameter(dbCommand, "@ReferralAcknowledgement", DbType.Boolean, patientReferred.AcknowledgementStatus);
                    dataAccessManager.AddOutParameter(dbCommand, "@ReferPatientId", DbType.Int32, 0);
                    dataAccessManager.AddInParameter(dbCommand, "@OutcomeDocumentID", DbType.String, patientReferred.OutcomeDocumentID); ;



                    suceessValue = _dataAccessManager.ExecuteNonQuery(dbCommand);
                    if ((suceessValue == 1) || (suceessValue == 2))
                    {
                        referPatientId = (int)dbCommand.Parameters["@ReferPatientId"].Value;
                        this.Result.IsSuccess = true;
                    }
                    else
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.PatientReferral_Failed);
                    }
                    //Added for Issue id #138
                    dataAccessManager = null;
                }
            }
            catch (Exception ex)
            {
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for Issue id #138
            finally
            {
                patientReferred = null;
            }
            return this.Result;
        }
        #endregion

        #region GetPatientReferralDetails
        /// <summary>
        /// This method will return patient referral details based on patientReferralId and email address
        /// </summary>
        /// <param name="patientReferralId"></param>
        /// <param name="emailAddress"></param>
        /// <param name="patientReferrals"></param>
        /// <returns></returns>
        public Result GetPatientReferralDetails(int patientReferralId, string referredToEmailAddress, string referredByEmailAddress, out  List<PatientReferral> patientReferrals)
        {            
            IDataReader reader = null;
            patientReferrals = new List<PatientReferral>();
            PatientReferral patientReferral = null;

            try
            {
                //Modified for Issue id #138
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientReferral"))
                {

                    if (patientReferralId == 0)
                        _dataAccessManager.AddInParameter(dbCommand, "@PatientReferralId", DbType.Int32, DBNull.Value);
                    else
                        _dataAccessManager.AddInParameter(dbCommand, "@PatientReferralId", DbType.Int32, patientReferralId);

                    if (string.IsNullOrEmpty(referredToEmailAddress))
                        _dataAccessManager.AddInParameter(dbCommand, "@ReferredToEmailAddress", DbType.String, DBNull.Value);
                    else
                        _dataAccessManager.AddInParameter(dbCommand, "@ReferredToEmailAddress", DbType.String, referredToEmailAddress.Trim());

                    if (string.IsNullOrEmpty(referredByEmailAddress))
                        _dataAccessManager.AddInParameter(dbCommand, "@ReferredByEmailAddress", DbType.String, DBNull.Value);
                    else
                        _dataAccessManager.AddInParameter(dbCommand, "@ReferredByEmailAddress", DbType.String, referredByEmailAddress.Trim());


                    reader = _dataAccessManager.ExecuteReader(dbCommand);
                }
                while (reader.Read())
                {
                    if (reader["Id"] != DBNull.Value)
                        patientReferral = new PatientReferral(Convert.ToInt32(reader["Id"]));
                    else
                        patientReferral = new PatientReferral();

                    if (reader["DocumentID"] != DBNull.Value)
                        patientReferral.DocumentId = Convert.ToString(reader["DocumentID"]);

                    if (reader["ReferredByEmail"] != DBNull.Value)
                        patientReferral.ReferredByEmail = Convert.ToString(reader["ReferredByEmail"]);

                    if (reader["ReferredToEmail"] != DBNull.Value)
                        patientReferral.ReferredToEmail = Convert.ToString(reader["ReferredToEmail"]);

                    if (reader["ReferredOn"] != DBNull.Value)
                        patientReferral.ReferralOn = Convert.ToString(reader["ReferredOn"]);

                    if (reader["PatientAppointmentDate"] != DBNull.Value)
                        patientReferral.PatientAppointmentDate = Convert.ToString(reader["PatientAppointmentDate"]);

                    if (reader["ReferralAccomplishmentDate"] != DBNull.Value)
                        patientReferral.ReferralAccomplishedOn = Convert.ToString(reader["ReferralAccomplishmentDate"]);

                    if (reader["LastName"] != DBNull.Value)
                    {
                        patientReferral.Patient.FamilyName = new List<string>();
                        patientReferral.Patient.FamilyName.Add(Convert.ToString(reader["LastName"]));
                    }

                    if (reader["FirstName"] != DBNull.Value)
                    {
                        patientReferral.Patient.GivenName = new List<string>();
                        patientReferral.Patient.GivenName.Add(Convert.ToString(reader["FirstName"]));
                    }

                    if (reader["DOB"] != DBNull.Value)
                        patientReferral.Patient.DOB = Convert.ToString(reader["DOB"]);

                    if (reader["ReferralSummary"] != DBNull.Value)
                        patientReferral.ReferralSummary = Convert.ToString(reader["ReferralSummary"]);

                    if (reader["DispatcherSummary"] != DBNull.Value)
                        patientReferral.DispatcherSummary = Convert.ToString(reader["DispatcherSummary"]);

                    if (reader["ReferralCompleted"] != DBNull.Value)
                        patientReferral.ReferralCompleted = Convert.ToBoolean(reader["ReferralCompleted"]);

                    if (reader["PurposeOfUseId"] != DBNull.Value)
                        patientReferral.PurposeOfUseId = Convert.ToInt32(reader["PurposeOfUseId"]);

                    if (reader["AcknowledgementStatus"] != DBNull.Value)
                        patientReferral.AcknowledgementStatus = Convert.ToBoolean(reader["AcknowledgementStatus"]);

                    if (reader["MPIID"] != DBNull.Value)
                        patientReferral.Patient.LocalMPIID = Convert.ToString(reader["MPIID"]);

                    if (reader["Gender"] != DBNull.Value)
                        patientReferral.Patient.Gender = (Gender)Enum.Parse(typeof(Gender), Convert.ToString(reader["Gender"]), true); // (Gender)(reader["Gender"]);

                    if (reader["ReferralCompletedOn"] != DBNull.Value)
                        patientReferral.ReferralCompletedOn = Convert.ToString(reader["ReferralCompletedOn"]);

                    if (reader["OutcomeDocumentID"] != DBNull.Value)
                        patientReferral.OutcomeDocumentID = Convert.ToString(reader["OutcomeDocumentID"]);
                    

                    patientReferrals.Add(patientReferral);
                }
                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return this.Result;
        }
        #endregion
    }
}
