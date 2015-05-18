using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using FirstGenesis.Mobius.Common.DataTypes;

namespace FirstGenesis.Mobius.RequestResponse
{
    public class UserLicenseRequest : ModuleInfo
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetUserLicense";
        private const string version = "1.0.0";
        private PATIENT patient;

        public PATIENT Patient
        {
            get { return patient; }
            set { patient = value; }
        }

        // default constructor
        public UserLicenseRequest()
            : base(moduleName, functionName, version)
        {

        }
    }
    public class UserLicenseResponse : Response
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetUserLicense";
        private const string version = "1.0.0";
        private UserLicense license;

        public UserLicense License
        {
            get { return license; }
            set { license = value; }
        }

        private List<OfflineLicenseTemplate> offlineTemplate;

        public List<OfflineLicenseTemplate> OfflineTemplates
        {
            get { return offlineTemplate; }
            set { offlineTemplate = value; }
        }

        // default constructor
        public UserLicenseResponse()
            : base(moduleName, functionName, version)
        {

        }
    }

    public class FacilityLicenseRequest : ModuleInfo
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetFacilityLicense";
        private const string version = "1.0.0";

        private PATIENT patient; // patient info;
        private FACILITY_INFO facilityInfo = new FACILITY_INFO();

        public FacilityLicenseRequest()
            : base(moduleName, functionName, version)
        {

        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(patient.ToString());
            toString.Append(facilityInfo.ToString());
            return toString.ToString();
        }
        public PATIENT Patient
        {
            get { return patient; }
            set { patient = value; }
        }

        public FACILITY_INFO FacilityInfo
        {
            get { return facilityInfo; }
            set { facilityInfo = value; }
        }



    }
    public class FacilityLicenseResponse : Response
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetFacilityLicense";
        private const string version = "1.0.0";

        private FacilityLicense facilityLicense;
        public FacilityLicense FacilityLicense
        {
            get { return facilityLicense; }
            set { facilityLicense = value; }
        }
        private List<OfflineLicenseTemplate> offlineTemplate;

        public List<OfflineLicenseTemplate> OfflineTemplates
        {
            get { return offlineTemplate; }
            set { offlineTemplate = value; }
        }

        // default constructor
        public FacilityLicenseResponse()
            : base(moduleName, functionName, version)
        {

        }
    }

    public class RecordLicenseRequest : ModuleInfo
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetRecordLicense";
        private const string version = "1.0.0";
        private PATIENT patient; //
        private USER user; //
        private RecordLicense patientLicense;
        private RECORD record;
        public RecordLicenseRequest()
            : base(moduleName, functionName, version)
        {

        }

        public PATIENT Patient
        {
            get { return patient; }
            set { patient = value; }
        }
        public USER User
        {
            get { return user; }
            set { user = value; }
        }
        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }
        public RecordLicense PatientLicense
        {
            get { return patientLicense; }
            set { patientLicense = value; }
        }
    }
    public class RecordLicenseResponse : Response
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetRecordLicense";
        private const string version = "1.0.0";
        private RecordLicense recordLicense;
        private RECORD record;
        private UserLicense userLicense;

        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }

        public RecordLicense RecordLicense
        {
            get { return recordLicense; }
            set { recordLicense = value; }
        }

        public UserLicense UserLicense
        {
            get { return userLicense; }
            set { userLicense = value; }
        }

        // default constructor
        public RecordLicenseResponse()
            : base(moduleName, functionName, version)
        {

        }
    }

    public class FacilityRecordLicenseRequest : ModuleInfo
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetFacilityRecordLicense";
        private const string version = "1.0.0";
        
        private PATIENT patient; // patient info;
        private RecordLicense patientLicense; // patient's offline license;
        private RECORD record; // rcord for which licenses are requested
        private FACILITY_INFO facilityInfo = new FACILITY_INFO();

        private string facilitySignature; // facility signature used for verifying license request.

        public string Signature
        {
          get { return facilitySignature; }
          set { facilitySignature = value; }
        }

        public FacilityRecordLicenseRequest()
            : base(moduleName, functionName, version)
        {

        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(patient.ToString());
            toString.Append(patientLicense.ToString());
            toString.Append(record.ToString());
            toString.Append(facilityInfo.ToString());
            return toString.ToString();
        }
        //public override string ToString()
        //{

        //    string data = moduleName + functionName + version;
        //    //string data = base.Serialize();
        //    data += patient.Serialize();
        //    data += record.Serialize();
        //    data += facilityInfo.Serialize();
        //    data += patientLicense.Serialize();

        //    return data;
        //} 

        public RecordLicense PatientLicense
        {
            get { return patientLicense; }
            set { patientLicense = value; }
        }
	
        public PATIENT Patient
        {
            get { return patient; }
            set { patient = value; }
        }
       
        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }
        
        public FACILITY_INFO FacilityInfo
        {
            get { return facilityInfo; }
            set { facilityInfo = value; }
        }
        

	
    }
    public class FacilityRecordLicenseResponse : Response
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetFacilityRecordLicense";
        private const string version = "1.0.0";
        private RecordLicense   recordLicense;
        private RECORD record;

        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }

        public RecordLicense RecordLicense
        {
            get { return recordLicense; }
            set { recordLicense = value; }
        }
        private FacilityLicense facilityLicense;
        public FacilityLicense FacilityLicense
        {
            get { return facilityLicense; }
            set { facilityLicense = value; }
        }

        // default constructor
        public FacilityRecordLicenseResponse()
            : base(moduleName, functionName, version)
        {

        }
    }
   
    public class OfflineLicenseRequest : ModuleInfo
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetOfflineLicense";
        private const string version = "1.0.0";

        private USER user; // Providor info;
        private FACILITY_INFO facilityInfo;// = new FACILITY_INFO();

        public OfflineLicenseRequest()
            : base(moduleName, functionName, version)
        {}
        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());
            toString.Append(User.ToString());
            toString.Append(facilityInfo.ToString());
            return toString.ToString();
        }
        public USER User
        {
            get { return user; }
            set { user = value; }
        }
        public FACILITY_INFO FacilityInfo
        {
            get { return facilityInfo; }
            set { facilityInfo = value; }
        }
    }
    public class OfflineLicenseResponse : Response
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetOfflineLicense";
        private const string version = "1.0.0";

        private List<OfflineLicenseTemplate> offlineTemplate;

        public List<OfflineLicenseTemplate> OfflineTemplates
        {
            get { return offlineTemplate; }
            set { offlineTemplate = value; }
        }

	
        // default constructor
        public OfflineLicenseResponse()
            : base(moduleName, functionName, version)
        {

        }
    }

    public class RemoteOfflineLicenseRequest : ModuleInfo
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetRemoteOfflineLicense";
        private const string version = "1.0.0";

        private FACILITY_INFO facilityInfo;// = new FACILITY_INFO();

        public RemoteOfflineLicenseRequest()
            : base(moduleName, functionName, version)
        { }
        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString()); 
            toString.Append(facilityInfo.ToString());
            return toString.ToString();
        }
        public FACILITY_INFO FacilityInfo
        {
            get { return facilityInfo; }
            set { facilityInfo = value; }
        }
    }
    public class RemoteOfflineLicenseResponse : Response
    {
        private const string moduleName = "Licensing Module";
        private const string functionName = "GetRemoteOfflineLicense";
        private const string version = "1.0.0";

        private List<OfflineLicenseTemplate> offlineTemplates;

        public List<OfflineLicenseTemplate> OfflineTemplates
        {
            get { return offlineTemplates; }
            set { offlineTemplates = value; }
        }

        // default constructor
        public RemoteOfflineLicenseResponse()
            : base(moduleName, functionName, version)
        {

        }
    }

}