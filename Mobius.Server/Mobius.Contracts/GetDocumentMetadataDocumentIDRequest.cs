
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

     
    [DataContract]
    public class GetDocumentMetadataDocumentIDRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string documentID
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
    }
}
