using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;

namespace Mobius.Entity
{
    public class PatientReferral
    {
        private Patient _Patient = null;
        public PatientReferral()
        {
            this.Id = 0;            
        }

        public PatientReferral(int patientReferralId)
        {
            this.Id = patientReferralId;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferralOn { get; set; }

        /// <summary>
        /// Get or set of referral accomplished date
        /// </summary>
        public string ReferralAccomplishedOn { get; set; }

        /// <summary>
        /// get or set of referral summary
        /// </summary>
        public string ReferralSummary { get; set; }

        /// <summary>
        /// get or set document unique id 
        /// </summary>
        public string DocumentId { get; set; }


        /// <summary>
        /// get or set Document Outcome unique id 
        /// </summary>
        public string OutcomeDocumentID { get; set; }

        /// <summary>
        /// get or set patient information
        /// </summary>
        public Patient Patient
        {
            get
            { return _Patient != null ? _Patient : _Patient = new Patient(); }
            set
            {
                if (_Patient == null) _Patient = new Patient();
                _Patient = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PurposeOfUseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReferredTo { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string  ReferredToEmail { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int ReferredBy { get;  set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferredByEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReferralCompleted { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public string ReferralCompletedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DispatcherSummary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AcknowledgementStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PatientAppointmentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] DocumentBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] XACMLBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CommunityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PurposeOfUse PurposeOfUse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Subject{ get; set; }
        
    }
}
