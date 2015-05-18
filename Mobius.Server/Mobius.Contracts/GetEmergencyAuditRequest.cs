
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;


    [DataContract]
    public class GetEmergencyAuditRequest
    {
       

        [DataMember]
        public string MPIID { get; set; }

        [DataMember]
        public int EmergencyAuditId { get; set; }


        [DataMember(EmitDefaultValue=false)]
        public EmergencyRecords EmergencyRecords { get; set; }


  

    }
}

