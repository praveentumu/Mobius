
namespace Mobius.Entity
{
    using System;

    [Serializable]
    public class PatientReferralCompleted
    {
        private Patient _Patient = null;

        public PatientReferralCompleted()
        {
            this.Id = 0;
        }

        public PatientReferralCompleted(int patientReferralId)
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

        public string DispatcherSummary { get; set; }

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
        public string ReferredByEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferredToEmail { get; set; }


        /// <summary>
        /// get or set document unique id 
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// get or set Document Outcome unique id 
        /// </summary>
        public string OutcomeDocumentID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReferralCompleted { get; set; }

        /// <summary>
        /// Get or set of referral accomplished date
        /// </summary>
        public string ReferralAccomplishedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferralOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AcknowledgementStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PatientAppointmentDate { get; set; }

        /// <summary>
        /// Gets and sets the document name 
        /// </summary>
        public string DocumentName { get; set; }

        
        public byte[] XACMLBytes
        { get; set; }

        
        public byte[] DocumentBytes
        { get; set; }

        public MobiusAssertion Assertion { get; set; }
    }

}