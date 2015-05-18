using System;
using System.Xml.Serialization;

namespace MobiusServiceLibrary
{

    [Serializable]
    [XmlType("WSSpecification", Namespace = "http://validation.hitsp.nist.gov/xsd")]
    public class AvailableValidations
    {
        private string descriptionField;

        private string nameField;

        private string specificationIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string specificationId
        {
            get
            {
                return this.specificationIdField;
            }
            set
            {
                this.specificationIdField = value;
            }
        }
    }
}
