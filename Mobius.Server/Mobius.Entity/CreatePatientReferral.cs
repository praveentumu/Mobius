

namespace Mobius.Entity
{
    using System;
    using Mobius.CoreLibrary;

    [Serializable]
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
        public PurposeOfUse PurposeOfUse { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ReferredByEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferredToEmail { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int ReferredBy { get; set; }

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
        public string Subject { get; set; }

        /// <summary>
        /// get or set document unique id 
        /// </summary>
        public string DocumentId { get; set; }

        public MobiusAssertion Assertion { get; set; }


        public string OriginalDocumentID { get; set; }
    }
}
