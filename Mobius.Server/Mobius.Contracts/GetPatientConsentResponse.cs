using System.Collections.Generic;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;
using MobiusServiceUtility;
namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class GetPatientConsentResponse
    {
        /// <summary>
        /// Gets or Sets response result of Get Patient Consent response
        /// </summary>
        [DataMember]
        public Result Result { get; set; }

        [DataMember]
        public List<PatientConsent> PatientConsents
        {
            get;
            set;
        }

        [DataMember]
        public List<C32Section> C32Section
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
