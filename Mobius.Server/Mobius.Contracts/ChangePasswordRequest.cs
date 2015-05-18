namespace MobiusServiceLibrary
{
    #region Namespaces
    using System;
    using Mobius.CoreLibrary;
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;
    #endregion

     
    [DataContract]
    public class ChangePasswordRequest: ForgotPasswordRequest
    {
        /// <summary>
        /// Get and set of Old Password
        /// </summary>
        [DataMember]
        public string OldPassword { get; set; }

        /// <summary>
        /// Get and set of New Password
        /// </summary>
        [DataMember]
        public string NewPassword { get; set; }

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

