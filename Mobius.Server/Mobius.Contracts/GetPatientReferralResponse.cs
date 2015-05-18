namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using System.Collections.Generic;
    using System;
    using MobiusServiceUtility;

     
    [DataContract]
   public class GetPatientReferralResponse
    {

        [DataMember]
        public Result Result { get; set; }


        [DataMember]
        public List<PatientReferral> PatientReferrals { get; set; }

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
