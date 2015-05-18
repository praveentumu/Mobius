
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

     
    [DataContract]
    public class GetPatientReferralRequest
    {
        /// <summary>
        /// Specify the running sequential patient referralID to get the record back
        /// </summary>
        [DataMember]
        public int patientReferralId
        {
            get;
            set;
        }

        /// <summary>
        /// Specify this field to filter records which have been referred to the specifid email ID
        /// </summary>
        [DataMember]
        public string referredToEmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Specify this field to filter records which have been referred by the specifid user email ID
        /// </summary>
        [DataMember]
        public string referredByEmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }
    }
}
