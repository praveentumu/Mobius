

namespace Mobius.Entity
{
    #region NameSpace
    using System;
    using System.Xml.Serialization;
    #endregion

    [Serializable]
    [XmlType("PatientConsent")]
    public class MobiusPatientConsent
    {


        /// <summary>
        /// 
        /// </summary>
        public int PatientConsentID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Role
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RuleStartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RuleEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Allow
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Active
        {
            get;
            set;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public bool Status
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 
        /// </summary>
        public string Permission
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int RoleId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int PurposeOfUseId
        {
            get;
            set;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public int C32SectionId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 
        /// </summary>
        public MobiusPatientConsentPolicy PatientConsentPolicy { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string MPIID
        {
            get;
            set;
        }

    

    }
}
