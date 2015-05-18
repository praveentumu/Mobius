
namespace MobiusServiceLibrary
{
    #region NameSpace
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

    #endregion

     
    [DataContract]
    public class UpdatePatientConsentPolicyRequest
    {
      
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int PatientConsentID
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RuleStartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RuleEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool Allow
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Permission
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int RoleId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int PurposeOfUseId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public PatientConsentPolicy PatientConsentPolicy { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string MPIID
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

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
         public bool Active{ get; set; }
    }
}
