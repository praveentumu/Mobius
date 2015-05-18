
namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;

    [DataContract]
    public class GetMasterDataRequest
    {
        [DataMember]
        public MasterCollection MasterCollection
        {
            get;
            set;
        }
        [DataMember]
        public int dependedValue
        {
            get;
            set;
        }




    }
}
