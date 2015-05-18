

namespace MobiusServiceLibrary
{
    #region Namespace
    using System.Runtime.Serialization;
    #endregion
    [DataContract]
    public class AuthenticateUserRequest
    {

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
        public string Password
        {
            get;
            set;
        }

         /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Mobius.CoreLibrary.UserType UserType
        {
            get;
            set;
        }

       
    }
}
