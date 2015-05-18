

namespace Mobius.Entity
{

    #region NameSpace
    using System;
    using System.Collections.Generic;
    #endregion


    [Serializable]
    public partial class C32Section : Component
    {
        List<Component> _ChildSections;
        
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


    public partial class C32Section
    {
        /// <summary>
        /// 
        /// </summary>

        public int ModuleId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>

        public string ModuleName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>

        public string ModuleValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>

        public string Optionality
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Repeatable
        {
            get;
            set;
        }

    }
}
