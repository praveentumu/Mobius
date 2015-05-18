
namespace MobiusServiceLibrary
{
    #region Namespaces
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;
    #endregion

     
    [DataContract]
    public class AddPFXCertificateRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public UserType UserType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string EmailAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Certificate
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
    }
}
