namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using System;
    using MobiusServiceUtility;

    /// <summary>
    /// Patient referred request
    /// </summary>
     
    [DataContract]
    public class CreateReferralRequest
    {

        /// <summary>
        /// get set of Patient referral 
        /// </summary>
        [DataMember]
        public CreatePatientReferral PatientReferred { get; set; }

        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }

    }
}
