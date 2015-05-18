
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using System.Xml.Serialization;

    [XmlType("DocumentRequest")]
    [Serializable]
    [DataContract]
    public class EmergencyAccess
    {


        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime OverrideDate { get; set; }

        [DataMember]
        public string OverriddenBy { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public OverrideReason OverrideReason { get; set; }



        [DataMember]
        public int EmergencyAuditId { get; set; }
        [DataMember]
        public bool IsAudited { get; set; }
        [DataMember]
        public string DocumentId { get; set; }
        [DataMember]
        public string ProviderRole { get; set; }
        [DataMember]
        public string ProviderName { get; set; }
        [DataMember]
        public bool Status { get; set; }


    }
}

