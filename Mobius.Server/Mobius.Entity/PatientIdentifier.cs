
namespace Mobius.Entity
{
    using System;
    using System.Xml.Serialization;
    using System.Runtime.Serialization;
    [XmlType("PatientIdentifier")]
    public class RemotePatientIdentifier : MobiusNHINCommunity
    {
        public string PatientId { get; set; }

    }
}
