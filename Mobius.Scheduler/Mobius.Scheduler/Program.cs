using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.EventNotification;
using Mobius.Entity;
using Mobius.Scheduler.DAL;
using System.Data;
using System.IO;


namespace Mobius.Scheduler
{
    class ScheduleActivity
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<PatientReferral> patientReferrals = GetScheduledReferralTasks();
            ProcessScheduleTasks(patientReferrals);
        }

        /// <summary>
        /// GetScheduledReferralTasks
        /// </summary>
        /// <returns>List<PatientReferral></returns>
        private static List<PatientReferral> GetScheduledReferralTasks()
        {
            Sheduler sheduler = new Sheduler();
            List<PatientReferral> patientReferrals = sheduler.GetScheduledReferralTasks();
            return patientReferrals;
        }

         /// <summary>
        /// ProcessScheduleTasks
        /// </summary>
        /// <param name="patientReferrals">List<PatientReferral></param>
        private static void ProcessScheduleTasks(List<PatientReferral> patientReferrals)
        {
            // Send Mails to those who does not close the process
            var referralAccomplishedDelay = patientReferrals.Where(t => t.ReferralCompleted == false && Convert.ToDateTime(t.ReferralOn).Date < DateTime.Now.Date);

            if (referralAccomplishedDelay.Count() > 0)
            {
                foreach (PatientReferral patientReferral in referralAccomplishedDelay)
                {
                    SendMail(EventType.ReferralOutcome, patientReferral);
                }
            }

            //Send Mails to follow up the process"today is the appointment date"
            //var todayAppointmentRefferals = patientReferrals.Where(t => t.ReferralCompleted == false && Convert.ToDateTime(t.PatientAppointmentDate).Date == DateTime.Now.Date);

            //if (todayAppointmentRefferals.Count() > 0)
            //{
            //    foreach (PatientReferral patientReferral in todayAppointmentRefferals)
            //    {
            //        SendMail(EventType.SendPatientDocument, patientReferral);                    
            //    }
            //}


            //var referralCompleter = patientReferrals.Where(t => t.ReferralCompleted == true && Convert.ToDateTime(t.ReferralAccomplishedOn).ToShortDateString() == DateTime.Now.ToShortDateString());

            //if (referralCompleter.Count() > 0)
            //{
            //    foreach (PatientReferral patientReferral in referralCompleter)
            //    {
            //        SendMail(EventType.PatientReferralCompleted, patientReferral);
            //        System.Threading.Thread.Sleep(20);
            //    }
            //}
        }


        /// <summary>
        /// SendMail
        /// </summary>
        /// <param name="eventType">eventType</param>
        /// <param name="patientReferral">patientReferral</param>
        private static void SendMail(EventType eventType, PatientReferral patientReferral)
        {
            try
            {
                EventActionData eventActionData = new EventActionData();
                eventActionData.Event = eventType;
                eventActionData.DispatcherSummary = string.IsNullOrEmpty(patientReferral.DispatcherSummary) ? " " : eventActionData.ReferralSummary = patientReferral.DispatcherSummary;

                if (eventType == EventType.PatientReferralCompleted)
                {
                    eventActionData.EmailRecipients.Add(patientReferral.ReferredByEmail);
                    eventActionData.ReferralRequestor = patientReferral.ReferredByEmail;
                }
                else if (eventType == EventType.ReferralOutcome)
                {
                    eventActionData.EmailRecipients.Add(patientReferral.ReferredToEmail);
                    eventActionData.ReferralRequestor = patientReferral.ReferredToEmail;
                }
                else if (eventType == EventType.SendPatientDocument)
                {
                    eventActionData.EmailRecipients.Add(patientReferral.ReferredToEmail);
                    eventActionData.ReferralRequestor = patientReferral.ReferredToEmail;
                }

                eventActionData.ReferredOn = Convert.ToDateTime(patientReferral.ReferralOn).ToShortDateString();
                eventActionData.PatientAppointmentDate = Convert.ToDateTime(patientReferral.PatientAppointmentDate).ToShortDateString();
                if (patientReferral.Patient.GivenName != null)
                    eventActionData.FirstName = patientReferral.Patient.GivenName[0];
                if (patientReferral.Patient.FamilyName != null)
                    eventActionData.LastName = patientReferral.Patient.FamilyName[0];

                eventActionData.Gender = patientReferral.Patient.Gender.ToString();
                eventActionData.DOB = patientReferral.Patient.DOB;
                eventActionData.ReferralDispatcher = patientReferral.ReferredToEmail;
                eventActionData.DocumentId = patientReferral.DocumentId;
                eventActionData.ReferPatientId = patientReferral.ToString();
                eventActionData.Token = eventActionData.DocumentId = patientReferral.DocumentId;
                eventActionData.PatientId = eventActionData.ReferPatientId = patientReferral.Id.ToString();
                ///Send Mail
                EventLogger eventLogger = new EventLogger();
                eventLogger.LogEvent(eventActionData, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Application", ex.Message);
            }
        }      
       
    }
}
