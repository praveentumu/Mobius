

namespace MobiusServiceLibrary
{
    using System;
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;
     
    [DataContract]
    public class GetDocumentResponse
    {
        private Result _result;
        private Document _Document;

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
        public Document Document
        {
            get { return _Document != null ? _Document : _Document = new Document(); }
            set { _Document = value; }
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
