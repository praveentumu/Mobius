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
    public class EmergencyAudit
    {

        public int OverrideReasonId { get; set; }
        public string Description { get; set; }
        public int EmergencyAuditId { get; set; }
        public DateTime OverrideDate { get; set; }
        public string OverriddenBy { get; set; }
        public bool IsAudited { get; set; }
        public string DocumentId { get; set; }
        public string MPIId { get; set; }
        public string ProviderRole { get; set; }
        public string PatientName { get; set; }
        public string ProviderName { get; set; }
        public DateTime PatientDOB { get; set; }
        public string PatientGender { get; set; }
        public OverrideReason OverrideReason { get; set; }
    }
}
