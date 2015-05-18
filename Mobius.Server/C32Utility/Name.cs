using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C32Utility
{

    public class Name
    {
        private List<string> _GivenName = null;
        private List<string> _FamilyName = null;
        private List<string> _MiddleName = null;
        private List<string> _Prefix = null;
        private List<string> _Suffix = null;

        /// <summary>
        /// Suffix
        /// </summary>
        public List<string> Suffix
        {
            get
            {
                return _Suffix != null ? _Suffix : _Suffix = new List<string>();
            }
            set
            {
                if (_Suffix == null) _Suffix = new List<string>();
                _Suffix = value;
            }
        }


        /// <summary>
        /// Prefix
        /// </summary>
        public List<string> Prefix
        {
            get
            {
                return _Prefix != null ? _Prefix : _Prefix = new List<string>();
            }
            set
            {
                if (_Prefix == null) _Prefix = new List<string>();
                _Prefix = value;
            }
        }


        /// <summary>
        /// Middle Name
        /// </summary>
        public List<string> MiddleName
        {
            get
            {
                return _MiddleName != null ? _MiddleName : _MiddleName = new List<string>();
            }
            set
            {
                if (_MiddleName == null) _MiddleName = new List<string>();
                _MiddleName = value;
            }
        }



        /// <summary>
        /// Family Name
        /// </summary>
        public List<string> FamilyName
        {
            get
            {
                return _FamilyName != null ? _FamilyName : _FamilyName = new List<string>();
            }
            set
            {
                if (_FamilyName == null) _FamilyName = new List<string>();
                _FamilyName = value;
            }
        }


        /// <summary>
        /// Given Name
        /// </summary>
        public List<string> GivenName
        {
            get
            {
                return _GivenName != null ? _GivenName : _GivenName = new List<string>();
            }
            set
            {
                if (GivenName == null) _GivenName = new List<string>();
                _GivenName = value;
            }
        }



    }
}
