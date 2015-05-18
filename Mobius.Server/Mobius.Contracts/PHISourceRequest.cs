

namespace MobiusServiceLibrary
{
    using Mobius.CoreLibrary;
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;
    
     
    [DataContract]
    public class PHISourceRequest 
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }
        /// <summary>
        /// Get and set PatientId
        /// </summary>
        [DataMember]
        public string PatientId
        {
            get;
            set;
        }

        /// <summary>
        /// Get/set community Id
        /// </summary>
        [DataMember]
        public string AssigningCommunityId
        {
            get;
            set;
        }
    }
}
