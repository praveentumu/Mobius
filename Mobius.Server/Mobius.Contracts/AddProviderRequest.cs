using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class AddProviderRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CSR
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Provider Provider
        {
            get;
            set;
        }

    }
}
