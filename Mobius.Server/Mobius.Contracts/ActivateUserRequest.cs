
namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;

    [DataContract]
   public class ActivateUserRequest
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
        public string CSR
        {
            get;
            set;
        }
    }
}
