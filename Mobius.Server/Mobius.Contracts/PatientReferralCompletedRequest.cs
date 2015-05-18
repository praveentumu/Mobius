namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using System;
    using MobiusServiceUtility;

    /// <summary>
    /// Accept referred request
    /// </summary>
     
    [DataContract]
    public class PatientReferralCompletedRequest
    {

        /// <summary>
        /// get set of Patient referral 
        /// </summary>
        [DataMember]
        public PatientReferralCompleted ReferralCompleted { get; set; }


        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }

    }


}

