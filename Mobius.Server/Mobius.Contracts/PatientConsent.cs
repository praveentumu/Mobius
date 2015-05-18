using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobiusServiceLibrary
{
  
    public class PatientConsent
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
        public PatientConsentPolicy PatientConsentPolicy { get; set; }


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
