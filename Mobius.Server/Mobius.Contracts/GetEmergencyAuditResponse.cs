
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using System.Collections.Generic;
    
    [DataContract]
    public class GetEmergencyAuditResponse
    {

        //[DataMember]
        //public string Description { get; set; }
        //[DataMember]
        //public int EmergencyAuditId { get; set; }
        //[DataMember]
        //public DateTime OverrideDate { get; set; }
        //[DataMember]
        //public string OverriddenBy { get; set; }
        //[DataMember]
        //public bool IsAudited { get; set; }
        //[DataMember]
        //public string DocumentId { get; set; }
        //[DataMember]
        //public string ProviderRole { get; set; }
        //[DataMember]
        //public string ProviderName { get; set; }
        //[DataMember]
        //public string ProviderEmail { get; set; }
        //[DataMember]
        //public string Reason { get; set; }


        [DataMember]
        public Result Result { get; set; }

        [DataMember]
        public List<EmergencyAccess> ListEmergencyAccess{ get; set; }

  

    }
}

