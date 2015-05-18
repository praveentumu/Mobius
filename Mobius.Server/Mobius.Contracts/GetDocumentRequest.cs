

using System.Xml.Serialization;

namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

    [XmlType("DocumentRequest")]
    [Serializable]
    [DataContract]
    public class GetDocumentRequest
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
        /// Load data from local server
        /// </summary>
        [DataMember]
        public bool LocalData 
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
        public string documentId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string purpose
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string subjectRole
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
        public string subjectEmailID
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore()] 
        [DataMember]
        public EmergencyAccess EmergencyAccess
        {
            get;
            set;
        }

    }
}
