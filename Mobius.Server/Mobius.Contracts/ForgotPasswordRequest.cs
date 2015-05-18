
namespace MobiusServiceLibrary
{
    using Mobius.CoreLibrary;
    using System;
    using System.Runtime.Serialization;

     
    [DataContract]
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// Set and set of email address
        /// </summary>
        [DataMember]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Set and set of email address
        /// </summary>
        [DataMember]
        public UserType UserType { get; set; }

    }
}
