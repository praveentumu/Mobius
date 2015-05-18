using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using FirstGenesis.Mobius.Common;
namespace FirstGenesis.Mobius.Common.DataTypes
{
    public enum LicenseType
    {
        PatientLicense  = 0x00000000,
        OffLineLicense  = 0x00000001,
        UserLicense     = 0x00000002,
        FacilityLicense = 0x00000003
    }

    public class OfflineLicenseTemplate //: MSerializable
    {
        private string xmlOfflineLicense;
        private string facilityId;
        private string publicKey;

        public string XmlOfflineLicense
        {
            get { return xmlOfflineLicense; }
            set { xmlOfflineLicense = value; }
        }        

        public string FacilityId
        {
            get { return facilityId; }
            set { facilityId = value; }
        }        

        public string PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }
        public override string ToString()
        {
            string hashData = base.ToString();
            hashData += xmlOfflineLicense;
            hashData += facilityId;
            hashData += publicKey;
            return hashData;
        }
    }

    public class PASSWORD_ENTRY : MSerializable
    {
        string _password;
        string _signingKey;

        public string SigningKey
        {
            get { return _signingKey; }
            set { _signingKey = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public override string ToString()
        {
            string hashData = base.ToString();
            hashData += _password;
            hashData += _signingKey;
            return hashData;
        }
    }

    public class RecordLicense : License
    {
        string          _recordId; // record to which this license is bound to;
        PASSWORD_ENTRY  _passwordEntry = new PASSWORD_ENTRY();
        string          _verificationKey = "";
        string          _signature = "";
        LicenseType     _type;

        public LicenseType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string RecordId
        {
            get
            {
                return _recordId;
            }
            set
            {
                _recordId = value;
            }
        }
        public PASSWORD_ENTRY PasswordEntry
        {
            get { return _passwordEntry; }
            set { _passwordEntry = value; }
        }
        public string   VerificationKey
        {
            get { return _verificationKey; }
            set { _verificationKey = value; }
        }
        public string   Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString()); // string hashData = (new License(this)).Serialize();
            toString.Append(PasswordEntry.ToString());
            toString.Append(VerificationKey); ;

            return toString.ToString();
        }
    }

    public class License : MSerializable
    {
        string _licenseId;
        string _name;
        string _description;
        string _creationDate;
        USER_INFO _creatorInfo = new USER_INFO();
        FACILITY_INFO _facilityInfo = new FACILITY_INFO();

        public string LicenseId
        {
            get
            {
                return _licenseId;
            }
            set
            {
                _licenseId = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public string CreationDate
        {
            get
            {
                return _creationDate;
            }
            set
            {
                _creationDate = value;
            }
        }
        public USER_INFO CreatorInfo
        {
            get { return _creatorInfo; }
            set { _creatorInfo = value; }
        }
        public FACILITY_INFO FacilityInfo
        {
            get { return _facilityInfo; }
            set { _facilityInfo = value; }
        }

        public License()
        {

        }

        /// <summary>
        /// Copy constructor provided.
        /// </summary>
        /// <param name="license"></param>
        public License(License license)
        {
            _licenseId = license.LicenseId;
            _name = license.Name;
            _description = license.Description;
            _creationDate = license.CreationDate;
            _creatorInfo = license.CreatorInfo;
            _facilityInfo = license.FacilityInfo;
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            
            toString.Append(base.ToString());
            toString.Append(_licenseId);
            toString.Append(_name);
            toString.Append(_description);
            toString.Append(Helper.ConvertToUTCFormat(_creationDate));
            toString.Append(_creatorInfo.ToString());
            toString.Append(_facilityInfo.ToString());

            return toString.ToString();
        }
        

    }

    public class UserLicense : License
    {
        DateTime            _expiresOn;
        USER_INFO           _owner = new USER_INFO();	// License to which this user belongs to:
        List<Permission>    _permissions = new List<Permission>();
        string              _signature = "";
        List<string>        _categories = new List<string>();
        FACILITY_INFO _IssuedBy = new FACILITY_INFO();
        FACILITY_INFO _IssuedTo = new FACILITY_INFO();

        public FACILITY_INFO IssuedByFacility
        {
            get { return _IssuedBy; }
            set { _IssuedBy = value; }
        }
        public FACILITY_INFO IssuedToFacility
        {
            get { return _IssuedTo; }
            set { _IssuedTo = value; }
        }

        public List<string> PermittedCategories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        public DateTime ExpiresOn
        {
            get { return _expiresOn; }
            set { _expiresOn = value; }
        }
        public List<Permission> Permissions
        {
            get { return _permissions; }
            set { _permissions = value; }
        }
        public USER_INFO Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        public string Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(_expiresOn.ToString());
            toString.Append(_owner.ToString());

            foreach(Permission permission in _permissions)
                toString.Append (permission.ToString());

            foreach (string category in _categories)
                toString.Append(category);

            return toString.ToString();
        }
    }

    public class FacilityLicense : License
    {
        DateTime _expiresOn;
        List<Permission> _permissions = new List<Permission>();
        List<string> _permittedCategories = new List<string>();
        string _signature = "";

        public List<string> PermittedCategories
        {
            get { return _permittedCategories; }
            set { _permittedCategories = value; }
        }

        public DateTime ExpiresOn
        {
            get { return _expiresOn; }
            set { _expiresOn = value; }
        }
        public List<Permission> Permissions
        {
            get { return _permissions; }
            set { _permissions = value; }
        }
        
        public string Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(_expiresOn.ToString());

            foreach (Permission permission in _permissions)
                toString.Append(permission.ToString());// Serialize();


            foreach (string category in _permittedCategories)
                toString.Append(category);

            return toString.ToString();
        }
    }
}
