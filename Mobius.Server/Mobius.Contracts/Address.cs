using System;
using Mobius.CoreLibrary;
using System.Runtime.Serialization;


namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class Address
    {
        
        public Address()
        {
            this.Id = 0;
        }
        
        public Address(int id)
        {
            this.Id = id;
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public string AddressLine2 { get; set; }
        [DataMember]
        public AddressStatus AddressStatus { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public City City
        {
            get;
            set;
        }
        [DataMember]
        public ActionType Action
        {
            get;
            set;
        }
    }
}
