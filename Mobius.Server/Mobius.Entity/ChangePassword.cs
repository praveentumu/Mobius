
namespace Mobius.Entity
{
    using Mobius.CoreLibrary;
    using System;
    
    [Serializable]
    public class ChangePassword : ForgotPassword
    {
        /// <summary>
        /// Get and set of Old Password
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Get and set of New Password
        /// </summary>
        public string NewPassword { get; set; }

    }
}
