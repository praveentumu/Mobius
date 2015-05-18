using System;
using Mobius.CoreLibrary;
using System.Runtime.Serialization;
using MobiusServiceUtility;

namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class UpdatePatientResponse
    {
        private Result _result = null;
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
