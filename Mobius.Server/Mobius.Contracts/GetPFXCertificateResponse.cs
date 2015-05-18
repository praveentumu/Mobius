

namespace MobiusServiceLibrary
{

    #region Namespaces
    using System;
    using Mobius.CoreLibrary;
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
    #endregion

     
    [DataContract]
    public class GetPFXCertificateResponse
    {
        private Result _result = null;

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
        public string PFXCertificate
        {
            get;
            set;
        }

        
    }
}
