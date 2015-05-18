
namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using System;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;

    /// <summary>
    /// Patient Referred Response
    /// </summary>
     
    [DataContract]
    public class PatientReferralResponse
    {
        /// <summary>
        /// Gets or Sets response result of Patient referral request 
        /// </summary>
        [DataMember]
        public Result Result { get; set; }

        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }
    }
}
