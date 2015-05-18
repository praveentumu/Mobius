
namespace Mobius.Entity
{
    using System;

    [Serializable]
    public class AcceptReferral
    {
        private Patient _Patient = null;

        public AcceptReferral()
        {
            this.Id = 0;
        }

        public AcceptReferral(int patientReferralId)
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
        /// 
        /// </summary>
        public bool AcknowledgementStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PatientAppointmentDate { get; set; }

        /// <summary>
        /// Get or set of referral accomplished date
        /// </summary>
        public string ReferralAccomplishedOn { get; set; }

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
        public string ReferralOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferredToEmail { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ReferredByEmail { get; set; }


        /// <summary>
        /// get or set document unique id 
        /// </summary>
        public string DocumentId { get; set; }
    }
}
