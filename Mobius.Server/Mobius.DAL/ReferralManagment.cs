using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.Entity;
using Mobius.CoreLibrary;
using System.Data.Common;
using FirstGenesis.Mobius.Server.DataAccessLayer;
using System.Data;

namespace Mobius.DAL
{
    public partial class MobiusDAL : IMobiusDAL
    {
        public Result CreatePatientReferral(PatientReferral patientReferred, out int referPatientId)
        {
            DbCommand dbCommand;
            referPatientId = 0;
            try
            {
                int suceessValue = 0; 
                DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
                dbCommand = dataAccessManager.GetStoredProcCommand("SavePatientReferral");
                dataAccessManager.AddInParameter(dbCommand, "@PatientReferredId", DbType.String, patientReferred.Id);
                dataAccessManager.AddInParameter(dbCommand, "@ReferredByEmail", DbType.String , patientReferred.ReferredByEmail);
                dataAccessManager.AddInParameter(dbCommand, "@ReferredToEmail", DbType.String , patientReferred.ReferredToEmail);
                dataAccessManager.AddInParameter(dbCommand, "@PatientMPIId", DbType.String , patientReferred.Patient.LocalMPIID);
                if(patientReferred.PurposeOfUseId == 0)
                    dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32 , DBNull.Value);
                else
                    dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32, patientReferred.PurposeOfUseId);

                if (Convert.ToDateTime(patientReferred.ReferralOn) == DateTime.MinValue)
                    dataAccessManager.AddInParameter(dbCommand, "@ReferralDate", DbType.DateTime, DBNull.Value);
                else
                    dataAccessManager.AddInParameter(dbCommand, "@ReferralDate", DbType.DateTime ,Convert.ToDateTime(patientReferred.ReferralOn));

                if(Convert.ToDateTime(patientReferred.ReferralAccomplishedOn) == DateTime.MinValue)
                    dataAccessManager.AddInParameter(dbCommand, "@ReferralAccomplishmentDate", DbType.DateTime, DBNull.Value);
                else
                    dataAccessManager.AddInParameter(dbCommand, "@ReferralAccomplishmentDate", DbType.DateTime , Convert.ToDateTime(patientReferred.ReferralAccomplishedOn));

                dataAccessManager.AddInParameter(dbCommand, "@ReferralSummary", DbType.String , patientReferred.ReferralSummary);
                dataAccessManager.AddInParameter(dbCommand, "@DocumentId", DbType.String , patientReferred.DocumentId);                
                dataAccessManager.AddInParameter(dbCommand, "@DispatcherSummary", DbType.String, patientReferred.DispatcherSummary);

                dataAccessManager.AddInParameter(dbCommand, "@ReferralCompleted", DbType.Boolean, patientReferred.ReferralCompleted);
                dataAccessManager.AddInParameter(dbCommand, "@ReferralAcknowledgement", DbType.Boolean, patientReferred.AcknowledgementStatus);
                dataAccessManager.AddOutParameter(dbCommand, "@ReferPatientId", DbType.Int32, 0);
                
                

                suceessValue = _dataAccessManager.ExecuteNonQuery(dbCommand);
                if (suceessValue == 1)
                {
                    referPatientId = (int)dbCommand.Parameters["@ReferPatientId"].Value;
                    this.Result.IsSuccess = true;
                }
                else
                {
                    this.Result.ErrorCode = ErrorCode.PatientReferral_Failed;
                    this.Result.ErrorMessage = "Patient referral not created.";
                }
            }
            catch (Exception ex)
            {

                this.Result.ErrorCode = ErrorCode.PatientReferral_Failed;
                this.Result.ErrorMessage = ex.Message;
            }
            return this.Result;
        }

        public Result GetPatientReferralDetails(int patientReferralId, string emailAddress, out  List<PatientReferral> patientReferrals)
        {
            DbCommand dbCommand;
            IDataReader reader = null;
            patientReferrals = new List<PatientReferral>();
            PatientReferral patientReferral = null;

            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientReferral");

                if (patientReferralId == 0)
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientReferralId", DbType.Int32, DBNull.Value);
                else
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientReferralId", DbType.Int32, patientReferralId);

                if (string.IsNullOrEmpty(emailAddress.Trim()))
                    _dataAccessManager.AddInParameter(dbCommand, "@EmailAddress", DbType.String, DBNull.Value);
                else
                    _dataAccessManager.AddInParameter(dbCommand, "@EmailAddress", DbType.String, emailAddress);

                reader = _dataAccessManager.ExecuteReader(dbCommand);
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

                    if (reader["ReferralDate"] != DBNull.Value)
                        patientReferral.ReferralOn = Convert.ToString(reader["ReferralDate"]);

                    if (reader["ReferredOn"] != DBNull.Value)
                        patientReferral.ReferredDate = Convert.ToString(reader["ReferredOn"]);
                    

                    if (reader["ReferralAccomplishmentDate"] != DBNull.Value)
                        patientReferral.ReferralAccomplishedOn = Convert.ToString(reader["ReferralAccomplishmentDate"]);

                    if (reader["FirstName"] != DBNull.Value)
                        patientReferral.Patient.GivenName = Convert.ToString(reader["FirstName"]);

                    if (reader["LastName"] != DBNull.Value)
                        patientReferral.Patient.GivenName = Convert.ToString(reader["FirstName"]);

                    if (reader["ReferralSummary"] != DBNull.Value)
                        patientReferral.Patient.FamilyName = Convert.ToString(reader["LastName"]);

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
                        patientReferral.Patient.LocalMPIID  = Convert.ToString(reader["MPIID"]);

                    if (reader["Gender"] != DBNull.Value)
                        patientReferral.Patient.Gender = Convert.ToString(reader["Gender"]);

                    

                    patientReferrals.Add(patientReferral);
                }
                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {

                this.Result.ErrorCode = ErrorCode.PatientReferral_Failed;
                this.Result.ErrorMessage = ex.Message;
            }
            return this.Result;
        }
    }
}
