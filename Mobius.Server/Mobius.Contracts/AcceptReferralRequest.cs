
namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using System;
    using MobiusServiceUtility;

    /// <summary>
    /// Accept referred request
    /// </summary>
     
    [DataContract]
    public class AcceptReferralRequest
    {

        /// <summary>
        /// get set of Patient referral 
        /// </summary>
        [DataMember]
        public AcceptReferral AcceptPatientReferred { get; set; }

        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }

    }


}
