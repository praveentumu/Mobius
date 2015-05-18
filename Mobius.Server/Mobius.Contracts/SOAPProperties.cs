

namespace MobiusServiceLibrary
{
    #region namespace
    using System.Runtime.Serialization;
    using System;
    #endregion

     
    [DataContract]
    public class SoapProperties
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Key
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string IV
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SignedData
        {
            get;
            set;
        }

        #endregion
    }
}
