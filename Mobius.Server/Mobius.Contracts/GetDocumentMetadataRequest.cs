

namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using MobiusServiceUtility;
     
    [DataContract]
    public class GetDocumentMetadataRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string patientId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<Community> communities
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Assertion Assertion
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

        [DataMember]
        public bool GetLocallyAvailable { get; set; }
    }
}
