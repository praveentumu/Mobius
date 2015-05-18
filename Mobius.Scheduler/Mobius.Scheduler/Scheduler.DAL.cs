using System;
using System.Collections.Generic;
using System.Data;
using Mobius.Entity;
using System.Data.Common;
using FirstGenesis.Mobius.Server.DataAccessLayer;
using Mobius.CoreLibrary;

namespace Mobius.Scheduler.DAL
{
    public class Sheduler
    {

        /// <summary>
        /// Calls the method which gets you the Accomplished dates
        /// </summary>
        /// <returns></returns>
        public List<PatientReferral> GetScheduledReferralTasks()
        {
            List<PatientReferral> patientReferrals = null;
            DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
            DbCommand dbCommand;
            DataSet patientReferralsdataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetAccomplishedRefferrals");
                patientReferralsdataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
                patientReferrals = GetReferralTasks(patientReferralsdataSet);
            }
            catch (Exception ex)
            {
                //TODO
            }
            return patientReferrals;
        }

        private List<PatientReferral> GetReferralTasks(DataSet patientReferralsdataSet)
        {
            List<PatientReferral> patientReferralList = new List<PatientReferral>();
            PatientReferral patientReferral = new PatientReferral();
            Patient patient = new Patient();
            /* 
             * Traverse the dataset 
             * Map it into the class 
             * then add the class into the list.
            */

            DataRowCollection drc = patientReferralsdataSet.Tables[0].Rows;
            foreach (DataRow dr in drc)
            {
                patientReferral = new PatientReferral();
                if (dr["Id"] != DBNull.Value)
                    patientReferral.Id = Convert.ToInt32(dr["Id"]);

                if (dr["DocumentId"] != DBNull.Value)
                    patientReferral.DocumentId = Convert.ToString(dr["DocumentId"]);

                if ((dr["ReferredByEmail"]) != DBNull.Value)
                    patientReferral.ReferredByEmail = Convert.ToString(dr["ReferredByEmail"]);

                if (dr["DispatcherSummary"] != DBNull.Value)
                    patientReferral.DispatcherSummary = Convert.ToString(dr["DispatcherSummary"]);

                if (dr["AcknowledgementStatus"] != DBNull.Value)
                    patientReferral.AcknowledgementStatus = Convert.ToBoolean(dr["AcknowledgementStatus"]);

                if (dr["PurposeOfUseId"] != DBNull.Value)
                    patientReferral.PurposeOfUseId = Convert.ToInt32(dr["PurposeOfUseId"]);

                if (dr["ReferralAccomplishmentDate"] != DBNull.Value)
                    patientReferral.ReferralAccomplishedOn = Convert.ToString(dr["ReferralAccomplishmentDate"]);

                if (dr["ReferralCompleted"] != DBNull.Value)
                    patientReferral.ReferralCompleted = Convert.ToBoolean(dr["ReferralCompleted"]);

                if (dr["ReferredOn"] != DBNull.Value)
                    patientReferral.ReferralOn = Convert.ToString(dr["ReferredOn"]);

                if (dr["PatientAppointmentDate"] != DBNull.Value)
                    patientReferral.PatientAppointmentDate = Convert.ToString(dr["PatientAppointmentDate"]);


                if (dr["ReferralCompleted"] != DBNull.Value)
                    patientReferral.ReferralCompleted = Convert.ToBoolean(dr["ReferralCompleted"]);

                if (dr["ReferredToEmail"] != DBNull.Value)
                    patientReferral.ReferredToEmail = Convert.ToString(dr["ReferredToEmail"]);

                // for demographichs

                if (dr["FirstName"] != DBNull.Value)
                {
                    patient.GivenName = new List<string>();
                    patient.GivenName.Add(Convert.ToString(dr["FirstName"]));
                }

                if (dr["LastName"] != DBNull.Value)
                {
                    patient.FamilyName = new List<string>();
                    patient.FamilyName.Add(Convert.ToString(dr["LastName"]));
                }

                if (dr["DOB"] != DBNull.Value)
                    patient.DOB = Convert.ToString(dr["DOB"]);

                if (dr["Gender"] != DBNull.Value)
                    patient.Gender = (Gender)Enum.Parse(typeof(Gender), Convert.ToString(dr["Gender"]), true);



                //
                patientReferral.Patient = patient;

                patientReferralList.Add(patientReferral);
            }

            return patientReferralList;
        }
    }
}
