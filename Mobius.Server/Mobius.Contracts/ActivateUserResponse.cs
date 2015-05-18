
namespace MobiusServiceLibrary
{
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;

    [DataContract]
    public class ActivateUserResponse
    {
        private Result _result;


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string PKCS7Response
        {
            get;
            set;
        }
    }
}
