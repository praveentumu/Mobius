

namespace MobiusServiceUtility
{
    #region namespace
    using System.Runtime.Serialization;
    using System;
    #endregion

    public class SoapProperties
    {
        #region Properties
       
        /// <summary>
        /// 
        /// </summary>
        public string Key
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string IV
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string SignedData
        {
            get;
            set;
        }

        #endregion
    }
}
