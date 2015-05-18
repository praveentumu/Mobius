
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;

     
    [DataContract]
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
        [DataMember]
        public int Id { get; set; }

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
        /// Get or set of referral accomplished date
        /// </summary>
        [DataMember]
        public string ReferralAccomplishedOn { get; set; }

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
        public string ReferralOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferredToEmail { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferredByEmail { get; set; }

        /// <summary>
        /// get or set document unique id 
        /// </summary>
        [DataMember]
        public string DocumentId { get; set; }


    }
}
