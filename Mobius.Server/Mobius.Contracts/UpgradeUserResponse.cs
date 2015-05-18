using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobiusServiceLibrary
{
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
    [DataContract]
    public class UpgradeUserResponse
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
        public string NewSerialNumber
        {
            get;
            set;
        }

        [DataMember]
        public string PKCS7Response
        {
            get;
            set;
        }
    }
}
