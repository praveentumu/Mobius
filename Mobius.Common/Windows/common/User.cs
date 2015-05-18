using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace FirstGenesis.Mobius.Common.DataTypes
{
    public enum FacilityType
    {
        Military        = 0,
        OMF             = 1,
        Unknown         = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class FACILITY_INFO : MSerializable
    {
        string _name="";
        string _address="";
        string _contactNo="";
        string _facilityId="";
        FacilityType _type;
        string  _serverUri="";
        string _publicKey="";

        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        public FacilityType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string ContactNo
        {
            get { return _contactNo; }
            set { _contactNo = value; }
        }

        public string FacilityId
        {
            get { return _facilityId; }
            set { _facilityId = value; }
        }

        public string ServerUri
        {
            get { return _serverUri; }
            set { _serverUri = value; }
        }
        public static bool operator == (FACILITY_INFO a, FACILITY_INFO b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            if (a.FacilityId == null || b.FacilityId == null)
                return false;

            return a.FacilityId.ToUpper() == b.FacilityId.ToUpper();
        }
        public static bool operator !=(FACILITY_INFO a, FACILITY_INFO b)
        {
            return !(a == b);
        }
        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            FACILITY_INFO p = obj as FACILITY_INFO ;

            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            if (FacilityId == null || p.FacilityId == null)
                return false;

            return (FacilityId.ToUpper()  == p.FacilityId.ToUpper());
        }
        public bool Equals(FACILITY_INFO p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            if (FacilityId == null || p.FacilityId == null)
                return false;

            return (FacilityId.ToUpper()  == p.FacilityId.ToUpper());
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(_name);
            toString.Append(_address);
            toString.Append(_contactNo);
            toString.Append(_facilityId);
            toString.Append(_type.ToString());
            toString.Append(_serverUri);
            toString.Append(_publicKey);

            return toString.ToString();
        }

    }

    public class USER_INFO : MSerializable
    {
        string _userId;
        string _firstName;
        string _middleName;
        string _lastName;
        FACILITY_INFO _facility = new FACILITY_INFO();
        ROLE_INFO _role = new ROLE_INFO();

        public ROLE_INFO Role
        {
            get { return _role; }
            set { _role = value; }
        }
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public FACILITY_INFO FacilityInfo
        {
            get { return _facility; }
            set { _facility = value; }
        }

        public static bool operator == (USER_INFO a, USER_INFO b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
                return false;

            if (a.UserId == null || b.UserId == null)
                return false;

            return a.UserId.ToUpper() == b.UserId.ToUpper() && a.FacilityInfo == b.FacilityInfo;
        }
        public static bool operator !=(USER_INFO a, USER_INFO b)
        {
            if (a.UserId == null || b.UserId == null)
                return true;

            return !(a.UserId.ToUpper() == b.UserId.ToUpper() && a.FacilityInfo == b.FacilityInfo);

        }
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            USER_INFO p = obj as USER_INFO ;

            if ((System.Object)p == null)
            {
                return false;
            }

            if (UserId == null || p.UserId == null)
                return false;

            // Return true if the fields match:
            return ( UserId.ToUpper() == p.UserId.ToUpper() && FacilityInfo == p.FacilityInfo);
        }
        public bool Equals(USER_INFO p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            if (UserId == null || p.UserId == null)
                return false;

            // Return true if the fields match:
            return (UserId.ToUpper() == p.UserId.ToUpper() && FacilityInfo == p.FacilityInfo);
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(_userId);
            toString.Append(_firstName);
            toString.Append(_middleName);
            toString.Append(_lastName);
            toString.Append(_facility.ToString());
            toString.Append(_role.ToString());

            return toString.ToString();
        }
    }

    public class USER : MSerializable 
    {
        USER_INFO   _info = new USER_INFO();
        private List<UserLicense> _userLicenses = new List<UserLicense>();
        public List<UserLicense> UserLicenses
        {
            get { return _userLicenses; }
        }

        string      _privateKey;
        string      _password;
        string      _publicKey;

        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }
        [XmlIgnore]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [XmlIgnore]
        public Boolean CanWorkOffline;

        // User's private key protected using user's password;
        public string PrivateKey
        {
            get { return _privateKey; }
            set { _privateKey = value; }
        }
        public USER_INFO Info
        {
            get { return _info; }
            set { _info = value; }
        }
        public static bool operator == (USER a, USER b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.Info == b.Info;

        }
        public static bool operator !=(USER a, USER b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            USER p = obj as USER ;

            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Info == p.Info);
        }
        public bool Equals(USER p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Info == p.Info);
        }


    }
}