using System;
using System.Runtime.Serialization;
using MobiusServiceUtility;

namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class UpdatePatientRequest
    {
        [DataMember]
        public Patient Patient
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
