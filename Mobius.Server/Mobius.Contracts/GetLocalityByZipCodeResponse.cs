
namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;


    [DataContract]
    public  class GetLocalityByZipCodeResponse
    {
        private Result _result ;
        private City _city;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        [DataMember]
        public City City
        {
            get { return _city != null ? _city : _city = new City(); }
            set { _city = value; }
        }

    }
}
