
namespace MobiusServiceLibrary
{
    using System;
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
   
     
    [DataContract]
    public class PatientIdentifier : NHINCommunity
    {
        [DataMember]
        public string PatientId { get; set; }

    }
}
