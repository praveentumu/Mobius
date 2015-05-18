using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Mobius.CoreLibrary;

namespace Mobius.Entity
{
    [XmlType("DocumentRequest")]
    [Serializable]
    public class DocumentRequest
    {
        public string patientId { get; set; }
        public string documentId { get; set; }
        public string purpose { get; set; }
        public string subjectRole { get; set; }
        public string subjectEmailID { get; set; }
        public MobiusAssertion Assertion { get; set; }
        public bool LocalData { get; set; }

        public OverrideReason OverrideReason { get; set; }
        public string Description { get; set; }
        public bool EmergencyOverrideStatus { get; set; }
        public string FilePathLocation { get; set; }
        public string Name { get; set; }
        public string PatientEmailId { get; set; }

    }
}
