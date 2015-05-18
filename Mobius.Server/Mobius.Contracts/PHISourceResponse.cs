

namespace MobiusServiceLibrary
{
    using Mobius.CoreLibrary;
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;

     
    [DataContract]
    public class PHISourceResponse
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
        public SoapProperties SoapProperties
        {
            get;
            set;
        }

        [DataMember]
        public List<PatientIdentifier> PatientIdentifiers
        {
            get;
            set;
        }


       
    }
}
