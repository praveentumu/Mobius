using System.Runtime.Serialization;
using Mobius.CoreLibrary;
namespace MobiusServiceLibrary
{
    [DataContract]
    public class AddProviderResponse
    {
        private Result _result = null;

        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        [DataMember]
        public string PKCS7Response
        {
            get;
            set;
        }
    }
}
