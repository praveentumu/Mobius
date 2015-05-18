
namespace Mobius.Entity
{    
    using Mobius.CoreLibrary;
    using System;

    [Serializable]
    public class ForgotPassword
    {
        /// <summary>
        /// Set and set of email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Set and set of email address
        /// </summary>
        public UserType UserType { get; set; }

    }
}
