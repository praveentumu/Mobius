using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MobiusServiceUtility;

namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class GetPatientConsentRequest
    {
        [DataMember]
        public string MPIID
        {
            get;
            set;
        }

        [DataMember]
        public int PatientConsentId
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
