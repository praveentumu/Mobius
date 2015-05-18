namespace MobiusServiceLibrary
{
    #region Namespace
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;
    #endregion

     
    [DataContract]
    public class UpdateOptInStatusRequest
    {
        /// <summary>
        /// get MPIID
        /// </summary>
        [DataMember]
        public string MPIID
        {
            get;
            set;
        }

        /// <summary>
        /// get optIn value
        /// </summary>
        [DataMember]
        public bool isOptIn
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
