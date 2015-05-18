using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Mobius.Entity
{
    [Serializable]
    [XmlType("WSValidationResults",Namespace="http://validation.hitsp.nist.gov/xsd")]
    public class MobiusValidationResults
    {
    
        private List<MobiusIndividualValidationResult> issueField;

        private string validationDateField;

        private bool validationTestField;

        private bool validationTestFieldSpecified;

        private string validationTimeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("issue", IsNullable = true)]
        public List<MobiusIndividualValidationResult> issue
        {
            get
            {
                return this.issueField != null ? this.issueField : this.issueField = new List<MobiusIndividualValidationResult>();
            }
            set
            {
                this.issueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string validationDate
        {
            get
            {
                return this.validationDateField;
            }
            set
            {
                this.validationDateField = value;
            }
        }

        /// <remarks/>
        public bool validationTest
        {
            get
            {
                return this.validationTestField;
            }
            set
            {
                this.validationTestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validationTestSpecified
        {
            get
            {
                return this.validationTestFieldSpecified;
            }
            set
            {
                this.validationTestFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string validationTime
        {
            get
            {
                return this.validationTimeField;
            }
            set
            {
                this.validationTimeField = value;
            }
        }
    }


    [Serializable]
    [XmlType("WSIndividualValidationResult")]
    public class MobiusIndividualValidationResult
    {
        private string contextField;

        private string messageField;

        private string severityField = string.Empty;

        private string specificationField;

        private string testField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string context
        {
            get
            {
                return this.contextField;
            }
            set
            {
                this.contextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string severity
        {
            get
            {
                return this.severityField;
            }
            set
            {
                this.severityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string specification
        {
            get
            {
                return this.specificationField;
            }
            set
            {
                this.specificationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }
    }
}
