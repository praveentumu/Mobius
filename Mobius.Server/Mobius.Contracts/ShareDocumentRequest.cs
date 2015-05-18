
using System.Xml.Serialization;
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

    [XmlType("ShareDocumentRequest")]
    [Serializable]
    [DataContract]
    public class ShareDocumentRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public byte[] docByteData
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
        public byte[] XACMLbyteData
        {
            get;
            set;
        }

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
        public string RemotePatientId
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RemoteCommunityId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string subject
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string homeCommunityId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string sourceRepositryId
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

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string OriginalDocumentID { get; set; }





    }
}
