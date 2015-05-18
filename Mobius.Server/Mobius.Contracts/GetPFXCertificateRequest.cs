
namespace MobiusServiceLibrary
{
    #region Namespaces
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    #endregion

     
    [DataContract]
    public class GetPFXCertificateRequest
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

        
    }
}
