using System;
using System.Collections.Generic;
using Mobius.CoreLibrary;

namespace MobiusServiceLibrary
{
    public class Provider
    {
        List<Specialty> _specialties;

        /// <summary>
        /// 
        /// </summary>
        public int ProviderId
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProviderType ProviderType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Status Status
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContactNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ElectronicServiceURI
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string MedicalRecordsDeliveryEmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string StreetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string StreetName
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
        public City City
        {
            get;
            set;
        }

        public Language Language
        {
            get;
            set;
        }

        public string Identifier
        {
            get;
            set;
        }

        public List<Specialty> Specialty
        {
            get { return _specialties != null ? _specialties : _specialties = new List<Specialty>(); }
            set
            {
                if (_specialties == null) _specialties = new List<Specialty>();
                _specialties = value;
            }
        }

        public Boolean IndividualProvider
        { get; set; }



        public Gender Gender
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }
        public string MiddleName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }

        public string OrganizationName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

    }
}
