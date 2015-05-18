using System.Collections.Generic;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;
using MobiusServiceLibrary;
using System;
using Mobius.CoreLibrary;
using MobiusServiceUtility;

namespace MobiusServiceLibrary
{

    #region Search Patient Response

    [KnownType(typeof(MobiusServiceLibrary.Patient))]

    [DataContract]
    public class SearchPatientResponse
    {
        private List<MobiusServiceLibrary.Patient> _patients = null;
        private Result _result;

        /// <summary>
        /// 
        /// </summary>

        [DataMember]
        public List<MobiusServiceLibrary.Patient> Patients
        {
            get
            {
                return _patients != null ? _patients : _patients = new List<MobiusServiceLibrary.Patient>();
            }
            set
            {
                _patients = value;
            }
        }

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
        public SoapProperties SoapProperties
        {
            get;
            set;
        }

    }

    #endregion


}
