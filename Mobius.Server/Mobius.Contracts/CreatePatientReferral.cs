

namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;


    [DataContract]
    public class CreatePatientReferral
    {
        private Patient _Patient = null;

        public CreatePatientReferral()
        {
            this.Id = 0;
        }

        public CreatePatientReferral(int patientReferralId)
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
        public PurposeOfUse PurposeOfUse { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferredByEmail { get; set; }

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
        public string Subject { get; set; }

        /// <summary>
        /// get or set document unique id 
        /// </summary>
        [DataMember]
        public string DocumentId { get; set; }

        [DataMember]
        public Assertion Assertion { get; set; }

        [DataMember]
        public string OriginalDocumentID { get; set; }

    }
}
