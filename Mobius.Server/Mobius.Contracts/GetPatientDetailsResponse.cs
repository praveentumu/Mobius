

namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;

     
    [DataContract]
    public class GetPatientDetailsResponse
    {
        private Patient _patient = null;
        private Result _result;

        [DataMember]
        public Patient Patient
        {
            get
            {
                return _patient != null ? _patient : _patient = new Patient();
            }
            set
            {
                _patient = value;
            }
        }

        
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

    }
}
