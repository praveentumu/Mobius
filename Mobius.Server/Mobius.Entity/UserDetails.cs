using System;
using Mobius.CoreLibrary;

namespace Mobius.Entity
{
    [Serializable]
    public class UserDetails
    {
        /// <summary>
        /// 
        /// </summary>
        public UserType UserType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmailAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Certificate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SerialNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FamilyName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>

        public string GivenName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>

        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string State
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get;
            set;
        }
    }
}
