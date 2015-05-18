using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace FirstGenesis.Mobius.Common.DataTypes
{    
    /// <summary>
    /// 
    /// </summary>
    //public class PUBLISH_FACILITY_INFO : MSerializable
    //{ 
    //    string _facilityId;
    //    string _name;
    //    string _city;
    //    string _address;
    //    string _address2;
    //    string _state;
    //    string _zip;
    //    string _description;
    //    string _email;
    //    string _logo;
    //    FacilityType _type;
    //    string  _serverUri;
    //    string _publicKey;

    //    public string FacilityId
    //    {
    //        get { return _facilityId; }
    //        set { _facilityId = value; }
    //    }

    //    public string Name
    //    {
    //        get { return _name; }
    //        set { _name = value; }
    //    }

    //    public string City
    //    {
    //        get { return _city; }
    //        set { _city = value; }
    //    }
	
    //    public string Address
    //    {
    //        get { return _address; }
    //        set { _address = value; }
    //    }

    //    public string Address2
    //    {
    //        get { return _address2; }
    //        set { _address2 = value; }
    //    }

    //    public string State
    //    {
    //        get { return _state; }
    //        set { _state = value; }
    //    }

    //    public string Zip
    //    {
    //        get { return _zip; }
    //        set { _zip = value; }
    //    }

    //    public string Description
    //    {
    //        get { return _description; }
    //        set { _description = value; }
    //    }

    //    public string Email
    //    {
    //        get { return _email; }
    //        set { _email = value; }
    //    }
	
    //    public string PublicKey
    //    {
    //        get { return _publicKey; }
    //        set { _publicKey = value; }
    //    }

    //    public string Logo
    //    {
    //        get { return _logo; }
    //        set { _logo = value; }
    //    }
	
    //    public FacilityType Type
    //    {
    //        get { return _type; }
    //        set { _type = value; }
    //    }

    //    public string ServerUri
    //    {
    //        get { return _serverUri; }
    //        set { _serverUri = value; }
    //    }

    //    public static bool operator ==(PUBLISH_FACILITY_INFO a, PUBLISH_FACILITY_INFO b)
    //    {
    //        // If both are null, or both are same instance, return true.
    //        if (System.Object.ReferenceEquals(a, b))
    //        {
    //            return true;
    //        }

    //        // If one is null, but not both, return false.
    //        if (((object)a == null) || ((object)b == null))
    //            return false;

    //        if (a.FacilityId == null || b.FacilityId == null)
    //            return false;

    //        return a.FacilityId.ToUpper() == b.FacilityId.ToUpper();
    //    }
    //    public static bool operator !=(PUBLISH_FACILITY_INFO a, PUBLISH_FACILITY_INFO b)
    //    {
    //        return !(a == b);
    //    }
    //    public override bool Equals(System.Object obj)
    //    {
    //        // If parameter is null return false.
    //        if (obj == null)
    //        {
    //            return false;
    //        }

    //        // If parameter cannot be cast to Point return false.
    //        PUBLISH_FACILITY_INFO p = obj as PUBLISH_FACILITY_INFO;

    //        if ((System.Object)p == null)
    //        {
    //            return false;
    //        }

    //        // Return true if the fields match:
    //        if (FacilityId == null || p.FacilityId == null)
    //            return false;

    //        return (FacilityId.ToUpper()  == p.FacilityId.ToUpper());
    //    }
    //    public bool Equals(PUBLISH_FACILITY_INFO p)
    //    {
    //        // If parameter is null return false:
    //        if ((object)p == null)
    //        {
    //            return false;
    //        }

    //        // Return true if the fields match:
    //        if (FacilityId == null || p.FacilityId == null)
    //            return false;

    //        return (FacilityId.ToUpper()  == p.FacilityId.ToUpper());
    //    }

    //    public override string ToString()
    //    {
    //        StringBuilder toString = new StringBuilder();
    //        toString.Append(base.ToString());
    //        toString.Append(_facilityId);
    //        toString.Append(_name);
    //        toString.Append(_city);
    //        toString.Append(_address);
    //        toString.Append(_address2);
    //        toString.Append(_state);
    //        toString.Append(_zip);
    //        toString.Append(_description);
    //        toString.Append(_email);
    //        toString.Append(_logo);
    //        toString.Append(_type.ToString());
    //        toString.Append(_serverUri);
    //        toString.Append(_publicKey);

    //        return toString.ToString();
    //    }

    //}

    public class PUBLISHING_SERVER
    {
        public string Name;
        public string Uri;
        public Boolean RequiresAuthentication;
        public string Signature;
    }
}