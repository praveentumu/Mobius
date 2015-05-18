
namespace MobiusServiceLibrary
{

    #region NameSpace
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;
    #endregion

     
    [DataContract]
    public class DeletePatientConsentPolicyRequest
    {   
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string patientConsentId 
        { get; set; }

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
