using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;

namespace Mobius.Entity
{
    [Serializable]
    public class Provider
    {
        private List<Specialty> _specialties;
        private string _CertificateCreationDate;
        private string _CertificateExpirationDate;
        public int ProviderId
        { get; set; }

        public ProviderType ProviderType
        {
            get;
            set;
        }
        public Status Status
        {
            get;
            set;
        }

        public string ContactNumber
        {
            get;
            set;
        }

        public string ElectronicServiceURI
        {
            get;
            set;
        }

        public string MedicalRecordsDeliveryEmailAddress
        {
            get;
            set;
        }

        public string StreetNumber
        {
            get;
            set;
        }

        public string StreetName
        {
            get;
            set;
        }
        public string PostalCode
        {
            get;
            set;
        }
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
        public string CreatedOn
        {
            get { return _CertificateCreationDate; }
            set { _CertificateCreationDate = value; }
        }
        public string ExpiryiOn
        {
            get { return _CertificateExpirationDate; }
            set { _CertificateExpirationDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
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

        public string CSR
        {
            get;
            set;
        }
        public string PublicKey
        {
            get;
            set;
        }
        public string CertificateSerialNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get;
            set;
        }
    }











}

