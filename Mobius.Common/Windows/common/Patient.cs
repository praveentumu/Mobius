using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FirstGenesis.Mobius.Common.DataTypes
{
    //public class Readiness : CollectionBase 
    //{

    //}

    public class EIC_ATTRIBUTES
    {
        public string BytesPerSector;
        public string Caption;
        public string Description;
        public string Manufacturer;
        public string MediaType;
        public string Model;
        public string SectorsPerTrack;
        public string Signature;
        public string Size;
        public string TotalCylinders;
        public string TotalHeads;
        public string TotalSectors;
        public string TracksPerCylinder;
        public string SerialId;
    }
    
    public class EIC_INFO : MSerializable
    {
        public string FriendlyName;
        public string Description;
        public string SerialId;
///        public EIC_ATTRIBUTES DeviceAttributes;
    }

    public class PATIENT_INFO : MSerializable
    {
        string patientKey;
        string nationality;
        string force;
        string sex;
        string uic;
        string religion;
        string fmp;
        string ssn;
        string grade;
        string firstName;
        string lastName;
        string mi;
        string unit;
        string dob;
        string race;
        string mos;
        string asi;
        string height;
        string bloodType;
        string weight;
        string missionId;
        string homeStation;
        string geoLocation;
        string country;
        string publicKey;
        string patientId;
        FACILITY_INFO facilityInfo = new FACILITY_INFO();

        [XmlElement(ElementName = "Facility")]
        public FACILITY_INFO FacilityInfo
        {
            get { return facilityInfo; }
            set { facilityInfo = value; }
        }


        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

        [XmlElement(ElementName = "patKey")]
        public string PatientKey
        {
            get { return patientKey; }
            set { patientKey = value; }
        }


        [XmlElement(ElementName = "FORCE")]
        public string Force
        {
            get { return force; }
            set { force = value; }
        }

        [XmlElement(ElementName = "SEX")]
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        [XmlElement(ElementName = "UIC")]
        public string Uic
        {
            get { return uic; }
            set { uic = value; }
        }

        [XmlElement(ElementName = "RELIGION")]
        public string Religion
        {
            get { return religion; }
            set { religion = value; }
        }

        [XmlElement(ElementName = "FMP")]
        public string Fmp
        {
            get { return fmp; }
            set { fmp = value; }
        }

        [XmlElement(ElementName = "SSN")]
        public string Ssn
        {
            get { return ssn; }
            set { ssn = value; }
        }

        [XmlElement(ElementName = "GRADE")]
        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        [XmlElement(ElementName = "FIRSTNAME")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [XmlElement(ElementName = "LASTNAME")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [XmlElement(ElementName = "MI")]
        public string Mi
        {
            get { return mi; }
            set { mi = value; }
        }

        [XmlElement(ElementName = "UNIT")]
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        [XmlElement(ElementName = "DOB")]
        public string Dob
        {
            get { return dob; }
            set { dob = value; }
        }

        [XmlElement(ElementName = "RACE")]
        public string Race
        {
            get { return race; }
            set { race = value; }
        }

        [XmlElement(ElementName = "MOS")]
        public string Mos
        {
            get { return mos; }
            set { mos = value; }
        }


        [XmlElement(ElementName = "ASI")]
        public string Asi
        {
            get { return asi; }
            set { asi = value; }
        }

        [XmlElement(ElementName = "HEIGHT")]
        public string Height
        {
            get { return height; }
            set { height = value; }
        }


        [XmlElement(ElementName = "WEIGHT")]
        public string Weight
        {
            get { return weight; }
            set { weight = value; }
        }


        [XmlElement(ElementName = "BLOOD_TYPE")]
        public string BloodType
        {
            get { return bloodType; }
            set { bloodType = value; }
        }


        [XmlElement(ElementName = "MISSION_ID")]
        public string MissionId
        {
            get { return missionId; }
            set { missionId = value; }
        }


        [XmlElement(ElementName = "HOME_STATION")]
        public string HomeStation
        {
            get { return homeStation; }
            set { homeStation = value; }
        }


        [XmlElement(ElementName = "GEOLOCATION")]
        public string GeoLocation
        {
            get { return geoLocation; }
            set { geoLocation = value; }
        }


        [XmlElement(ElementName = "COUNTRY")]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }


        [XmlElement(ElementName = "NATIONALITY")]
        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        [XmlElement(ElementName = "UserKey")]
        public string PublicKey
        {
            get { return publicKey; }
            set { publicKey = value; }
        }
    }

    [XmlRoot(ElementName = "Patient", Namespace="http://www.mobius.com", IsNullable = false)]
    public class PATIENT : MSerializable 
    {
        PATIENT_INFO patientInfo = new PATIENT_INFO();
        string  serverUri = "";
        string  signature = "";
     
        public PATIENT_INFO Info
        {
            get { return patientInfo; }
            set { patientInfo = value; }
        }

        public string ServerUri
        {
            get { return serverUri; }
            set { serverUri = value; }
        }

        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }
    }

    public class READINESS
    {
        
    }
}