namespace MobiusServiceLibrary
{
    #region Namespace
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;
    #endregion

     
    [DataContract]
    public class UpdateOptInStatusResponse
    {
        private Result _result;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }

    }
}
