using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Component
    {

        private string _Optionality;
        private string _LONICCode;
        private string _TemplateId;

        bool _Allow = false;

        /// <summary>
        /// Get and set of Identity of component 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get and Set of Name of component section
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get and set of Optionality of section 
        /// </summary>
        public string Optionality { get { return string.IsNullOrEmpty(_Optionality) ? _Optionality : _Optionality.Trim(); } set { _Optionality = value; } }

        /// <summary>
        /// Get and set of Repeatable of section 
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        /// Get and set of LONICCode of section 
        /// </summary>
        public string LONICCode { get { return string.IsNullOrEmpty(_LONICCode) ? _LONICCode : _LONICCode.Trim(); } set { _LONICCode = value; } }

        /// <summary>
        /// Get and set of TemplateId of section 
        /// </summary>
        public string TemplateId { get { return string.IsNullOrEmpty(_TemplateId) ? _TemplateId : _TemplateId.Trim(); } set { _TemplateId = value; } }

        /// <summary>
        /// Get and set of display order of 
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Get and set of allow 
        /// </summary>
        public bool Allow { get { return _Allow; } set { _Allow = value; } }

    }

}
