namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;

     
    [DataContract]
    public class UploadDocumentResponse
    {
        private Result _result = null;

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

        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }
    }
}
