using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    [Serializable]
    public class Name
    {
        private string _given = string.Empty;
        private string _middleName = string.Empty;
        private string _prefix = string.Empty;
        private string _suffix = string.Empty;
        private string _familyName = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string GivenName
        {
            get { return _given; }
            set { _given = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FamilyName
        {
            get { return _familyName; }
            set { _familyName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }





    }
}
