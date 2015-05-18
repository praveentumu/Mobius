using System;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;
namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class Telephone
    {
        public Telephone()
        {
            this.Id = 0;
        }

        public Telephone(int id)
        {
            this.Id = id;
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string Extensionnumber { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public ActionType Action
        {
            get;
            set;
        }
    }
}
