

namespace MobiusServiceLibrary
{

    #region NameSpace
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    #endregion


    [DataContract]
    public class C32Section : Component
    {
        List<Component> _ChildSections;
        [DataMember]
        public List<Component> ChildSections
        {
            get
            {
                return _ChildSections != null ? _ChildSections : _ChildSections = new List<Component>();
            }
            set
            {
                if (_ChildSections == null) _ChildSections = new List<Component>();
                _ChildSections = value;
            }
        }

    }


    [DataContract]
    public class Component
    {

        private string _Optionality;
        private string _LONICCode;
        private string _TemplateId;

        bool _Allow = false;

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Optionality { get { return string.IsNullOrEmpty(_Optionality) ? _Optionality : _Optionality.Trim(); } set { _Optionality = value; } }

        [DataMember]
        public string Repeatable { get; set; }

        [DataMember]
        public string LONICCode { get { return string.IsNullOrEmpty(_LONICCode) ? _LONICCode : _LONICCode.Trim(); } set { _LONICCode = value; } }

        [DataMember]
        public string TemplateId { get { return string.IsNullOrEmpty(_TemplateId) ? _TemplateId : _TemplateId.Trim(); } set { _TemplateId = value; } }

        [DataMember]
        public int DisplayOrder { get; set; }

        [DataMember]
        public bool Allow { get { return _Allow; } set { _Allow = value; } }




    }

}
