

namespace MobiusServiceLibrary
{
    using System;
    using System.Collections.Generic;
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;    
     
    [DataContract]
    public class GetDocumentMetadataResponse
    {
        private Result _result;
        private List<Document> _Documents;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
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
        public List<Document> Documents
        {
            get { return _Documents != null ? _Documents : _Documents = new List<Document>(); }
            set { _Documents = value; }
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
