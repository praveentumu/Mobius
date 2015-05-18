

namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;

     
    [DataContract]
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
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferralOn { get; set; }

        /// <summary>
        /// Get or set of referral accomplished date
        /// </summary>
        [DataMember]
        public string ReferralAccomplishedOn { get; set; }

        /// <summary>
        /// get or set of referral summary
        /// </summary>
        [DataMember]
        public string ReferralSummary { get; set; }

        /// <summary>
        /// get or set document unique id 
        /// </summary>
        [DataMember]
        public string DocumentId { get; set; }


        /// <summary>
        /// get or set Document Outcome unique id 
        /// </summary>
        [DataMember]
        public string OutcomeDocumentID { get; set; }

        /// <summary>
        /// get or set Referral completed dated
        /// </summary>
        [DataMember]
        public string ReferralCompletedOn { get; set; }

        /// <summary>
        /// get or set patient information
        /// </summary>
        [DataMember]
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
        [DataMember]
        public int PurposeOfUseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ReferredTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferredToEmail { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ReferredBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferredByEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool ReferralCompleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DispatcherSummary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool AcknowledgementStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string PatientAppointmentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public byte[] DocumentBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public byte[] XACMLBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CommunityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public PurposeOfUse PurposeOfUse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

    }
}
