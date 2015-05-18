using System;
using System.Collections.Generic;
using System.Text;
using FirstGenesis.Mobius.Common;
using System.Xml;
using System.Xml.Serialization;


namespace FirstGenesis.Mobius.Common.DataTypes
{
    public class RECORD_MATCH_MASK
    {
        bool  _recordId;
        bool _medic;
        bool _name;
        bool _description;
        bool _category;
        bool _dateCreated;
        bool _dateModified;
        bool _dateSynchronized;
        bool _isSynchronized;
        bool _recordFormat;

        public bool RecordFormat
        {
            get { return _recordFormat; }
            set { _recordFormat = value; }
        }

        public bool DateSynchronized
        {
            get { return _dateSynchronized; }
            set { _dateSynchronized = value; }
        }

        public bool RecordId
        {
            get { return _recordId; }
            set { _recordId = value; }
        }

        public bool Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public bool Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public bool DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public bool DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }

        public bool Medic
        {
            get { return _medic; }
            set { _medic = value; }
        }
        public bool IsSynchronized
        {
            get { return _isSynchronized; }
            set { _isSynchronized = value; }
        }
    }
    public enum Criteria
    {
        MatchNotRequired = 0x00000000,
        MatchAbsolute = 0x00000001,
        MatchPattern = 0x00000002,
        MatchRange = 0x00000003,
        MatchLessThanEqualTo = 0x00000004,
        MatchGreaterThanEqualTo = 0x00000005
    }
    public class StringCriteria
    {
        string _value = "";
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        Criteria _criteria = Criteria.MatchNotRequired;

        public Criteria Criteria
        {
            get { return _criteria; }
            set { _criteria = value; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder("");
            if (_criteria == Criteria.MatchAbsolute)
                buffer.Append( " = '" + _value + "' ");
            else
                buffer.Append(" = '*" + _value + "*' ");

            return buffer.ToString();
        }
    }

    public class RecordFormatCriteria
    {
        Boolean _chcsIIFormat = true;
        Boolean _textFormat = true;
        Boolean _rtfFormat = true;

        public Boolean CHCSIIFormat
        {
            get { return _chcsIIFormat; }
            set { _chcsIIFormat = value; }
        }

        public Boolean TEXTFormat
        {
            get { return _textFormat; }
            set { _textFormat = value; }
        }

        public Boolean RTFFormat
        {
            get { return _rtfFormat; }
            set { _rtfFormat = value; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            if(_chcsIIFormat == true)
                buffer.Append("{CHCSII Format}");

            if (_textFormat == true)
            {
                if (buffer.ToString().Length > 0)
                    buffer.Append(", ");
                buffer.Append("{Text Format}");
            }

            if (_rtfFormat == true)
            {
                if (buffer.ToString().Length > 0)
                    buffer.Append(", ");

                buffer.Append("{RFT Format}");
            }

            return buffer.ToString();
        }
    }
    public class DateRange
    {
        DateTime _start;
        DateTime _end;

        public DateTime StartDate
        {
            get { return _start; }
            set { _start = value; }
        }
        public DateTime EndDate
        {
            get { return _end; }
            set { _end = value; }
        }
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder("Between ");
            buffer.Append("[" + _start.ToShortDateString() + "] and ");
            buffer.Append("[" + _end.ToShortDateString() + "]");

            return buffer.ToString();
        }
    }
    public class DateCriteria
    {
        DateRange _dateTimeRange = new DateRange();

        public DateRange Range
        {
            get { return _dateTimeRange; }
            set { _dateTimeRange = value; }
        }
        DateTime _dateTime;

        public DateTime Value
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }

        Criteria _criteria = Criteria.MatchNotRequired;

        public Criteria Criteria
        {
            get { return _criteria; }
            set { _criteria = value; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder("{ ");
            if (_criteria == Criteria.MatchAbsolute)
                buffer.Append("'" + _dateTime.ToShortDateString() + "'");
            else if (_criteria == Criteria.MatchNotRequired)
                buffer.Append("NA");
            else if (_criteria == Criteria.MatchRange)
                buffer.Append(_dateTimeRange.ToString());
            else if(_criteria == Criteria.MatchGreaterThanEqualTo)
                buffer.Append(" >= " + _dateTime.ToShortDateString());
            else if (_criteria == Criteria.MatchLessThanEqualTo)
                buffer.Append(" <= " + _dateTime.ToShortDateString());
            buffer.Append(" }");

            return buffer.ToString();
        }
    }
    public class UserCriteria
    {
        StringCriteria _userId = new StringCriteria();
        StringCriteria _firstName = new StringCriteria();
        StringCriteria _middleName = new StringCriteria();
        StringCriteria _lastName = new StringCriteria();
        FacilityCriteria _facility = new FacilityCriteria();

        public StringCriteria UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public StringCriteria FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public StringCriteria MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }
        public StringCriteria LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public FacilityCriteria Facility
        {
            get { return _facility; }
            set { _facility = value; }
        }
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{ UserId " + _userId.ToString());
            buffer.Append(" FirstName " + _firstName.ToString());
            buffer.Append(" MiddleName " + _middleName.ToString());
            buffer.Append(" LastName " + _lastName.ToString());
            buffer.Append(" Facility " + _facility.ToString());
            buffer.Append(" }");

            return buffer.ToString();
        }
    
    }
    public class FacilityCriteria
    {
        StringCriteria _name = new StringCriteria();
        StringCriteria _address = new StringCriteria();
        StringCriteria _contactNo = new StringCriteria();
        public StringCriteria Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public StringCriteria Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public StringCriteria ContactNo
        {
            get { return _contactNo; }
            set { _contactNo = value; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder( "{ Name " + _name.ToString());
            buffer.Append("Address " + _address.ToString());
            buffer.Append("ContactNo " + _contactNo.ToString());
            return buffer.ToString();
        }

    }
    public class BooleanCriteria
    {
        Boolean _value = false;
        Criteria _criteria = Criteria.MatchNotRequired;

        public Boolean Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public Criteria Criteria
        {
            get { return _criteria; }
            set { _criteria = value; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder("");
//            if (_criteria != Criteria.MatchNotRequired)
            buffer.Append(" '" + _value.ToString() + "' ");

            return buffer.ToString();
        }
    }
    
    public class RECORD_FIND_MASK
    {
        StringCriteria _recordId = new StringCriteria();
        StringCriteria _name = new StringCriteria();
        DateCriteria _syncDate = new DateCriteria();
        StringCriteria _description = new StringCriteria();
        StringCriteria _category = new StringCriteria();
        DateCriteria _creationDate = new DateCriteria();
        DateCriteria _modifiedDate = new DateCriteria();
        UserCriteria _medic = new UserCriteria();
        RecordFormatCriteria _recordFormat = new RecordFormatCriteria();
        public RecordFormatCriteria RecordFormat
        {
            get { return _recordFormat; }
            set { _recordFormat = value; }
        }

        BooleanCriteria _synchronizationCriteria = new BooleanCriteria();

        public BooleanCriteria SynchronizationCriteria
        {
          get { return _synchronizationCriteria; }
          set { _synchronizationCriteria = value; }
        }
        public StringCriteria Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public StringCriteria Category
        {
            get { return _category; }
            set { _category = value; }
        }
        public DateCriteria CreationDate
        {
            get { return _creationDate; }
            set { _creationDate = value; }
        }
        public DateCriteria ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }
        public DateCriteria SyncDate
        {
            get { return _syncDate; }
            set { _syncDate = value; }
        }
        public StringCriteria RecordId
        {
            get { return _recordId; }
            set { _recordId = value; }
        }
        public StringCriteria Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public UserCriteria Medic
        {
            get { return _medic; }
            set { _medic = value; }
        }
    }

    public class LicenseEntry
    {
        string _verificationKey;
        string _licenseId;

        public string VerificationKey
        {
            get { return _verificationKey; }
            set { _verificationKey = value; }
        }

        public string LicenseId
        {
            get { return _licenseId; }
            set { _licenseId = value; }
        }

        public override string ToString()
        {
            return _verificationKey + _licenseId;
        }
    }

    public class RECORD_INFO : MSerializable
    {
        string      _recordId;
        string      _name;
        string      _description = "description";
        string      _parentRecord;
        string      _category = "unknown";
        string      _dateCreated = "";
        string      _dateModified = "";
        string      _dateSynchronized = "";
        bool        _isSynchronized;
        USER_INFO   _medicInfo = new USER_INFO();
        string      _patientId;
        string      _recordFileName;
        string      _indexFileName;
//        string      _licenseFileName;
        RecordFormat _format;

        public RecordFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public string IndexFileName
        {
            get { return _indexFileName; }
            set { _indexFileName = value; }
        }

        public string RecordFileName
        {
            get { return _recordFileName; }
            set { _recordFileName = value; }
        }

        public string PatientId
        {
            get { return _patientId; }
            set { _patientId = value; }
        }

        public string RecordId
        {
            get { return _recordId; }
            set { _recordId = value; }
        }



        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string ParentRecord
        {
            get { return _parentRecord; }
            set { _parentRecord = value; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public string DateSynchronized
        {
            get { return _dateSynchronized; }
            set { _dateSynchronized = value; }
        }
        public string DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }
        public string DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public USER_INFO MedicInfo
        {
            get { return _medicInfo; }
            set { _medicInfo = value; }
        }
        public bool IsSynchronized
        {
            get { return _isSynchronized; }
            set { _isSynchronized = value; }
        }

        public override string ToString()
        {
            
            StringBuilder toString = new StringBuilder();

            toString.Append(base.ToString());

            toString.Append(_recordId);
            toString.Append(_name);
            toString.Append(_description);
            toString.Append(_parentRecord);
            toString.Append(_patientId);
            toString.Append(_recordId);
            toString.Append(_category);
            toString.Append(Helper.ConvertToUTCFormat(_dateCreated));
            toString.Append(Helper.ConvertToUTCFormat(_dateModified));
            toString.Append(Helper.ConvertToUTCFormat(_dateSynchronized));

            //toString.Append(_dateCreated != null ? _dateCreated.ToFileTimeUtc().ToString() : "");
            //toString.Append(_dateModified != null && _dateModified != Convert.ToDateTime("1/1/0001 12:00:00 AM") ? _dateModified.ToFileTimeUtc().ToString() : "");
            //toString.Append(_dateSynchronized != null && _dateSynchronized != Convert.ToDateTime("1/1/0001 12:00:00 AM") ? _dateSynchronized.ToFileTimeUtc().ToString() : "");

            toString.Append(_description);
            toString.Append(_medicInfo.ToString());

            return toString.ToString();
        }
    }

    public enum RecordFormat
    {
        TextFormat = 0,
        CHCSIIFormat,
        RtfFormat
    }


    [XmlInclude(typeof(RECORD))]
    public class RECORD : MSerializable
    {
        RECORD_INFO _recordInfo = new RECORD_INFO();
        string data;
        string signature;
        List<LicenseEntry> _licenseEntries = new List<LicenseEntry>();
        RecordFormat _format = RecordFormat.TextFormat;

        public List<LicenseEntry> LicenseEntries
        {
            get { return _licenseEntries; }
            set { _licenseEntries = value; }
        }

        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }
        public RECORD_INFO RecordInfo
        {
            get { return _recordInfo; }
            set { _recordInfo = value; }
        }
     
        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        public RecordFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(RecordInfo.ToString());
            toString.Append(Data);
            foreach (LicenseEntry entry in LicenseEntries)
                toString.Append(entry.ToString());

            return toString.ToString();
        }
    }
}
