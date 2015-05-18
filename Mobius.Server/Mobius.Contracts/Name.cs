
using System;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;
using System.Collections.Generic;
namespace MobiusServiceLibrary
{

    [DataContract]
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
        [DataMember(EmitDefaultValue = false)]
        public string GivenName
        {
            get { return _given; }
            set { _given = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FamilyName
        {
            get { return _familyName; }
            set { _familyName = value; }
        }





    }
}
