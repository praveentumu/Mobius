using System;
using System.Collections.Generic;
using System.Text;
using FirstGenesis.Mobius.Common.DataTypes;
using FirstGenesis.Mobius.Common;
using System.Data.Common;
using System.Data;

namespace FirstGenesis.Mobius.RequestResponse
{
 
    public class GetPatientRequest : ModuleInfo
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "SearchPatient";
        private const string version = "1.0.0";
        
        private string userId = "";
        private string firstName = "";
        private string middleName = "";
        private string lastName = "";
        private string email = "";
        private string dob = "";
        private string ssn = "";
        private int userTypeId = 0;
        private int facilityId;
        private string eicGuid = "";
        public GetPatientRequest() : base(moduleName, functionName, version) { }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string MiddleName
        {
            get { return middleName; }
            set { middleName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Dob
        {
            get { return dob; }
            set { dob = value; }
        }
        public string SSN
        {
            get { return ssn; }
            set { ssn = value; }
        }
        public int UserTypeId
        {
            get { return userTypeId; }
            set { userTypeId = value; }
        }
        public int FacilityId
        {
            get { return facilityId; }
            set { facilityId = value; }
        }
        public string EicGuid
        {
            get { return eicGuid; }
            set { eicGuid = value; }
        }
        

    }
    public class GetPatientResponse : Response
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "SearchPatient";
        private const string version = "1.0.0";

        private List<PATIENT_INFO> patientInfoList;

        public GetPatientResponse() : base(moduleName, functionName, version) { }

        public List<PATIENT_INFO> PatientInfoList
        {
            get { return patientInfoList; }
            set { patientInfoList = value; }
        }
    }

    public class GetPatientInfoRequest : ModuleInfo
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "GetPatientInfo";
        private const string version = "1.0.0";
        
        private string patientId;        

        public GetPatientInfoRequest() : base(moduleName, functionName, version) { }
                
        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

        
    }
    public class GetPatientInfoResponse : Response
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "GetPatientInfo";
        private const string version = "1.0.0";

        private string patientIdnFile;

        public GetPatientInfoResponse() : base(moduleName, functionName, version) { }

        public string PatientIdnFile
        {
            get { return patientIdnFile; }
            set { patientIdnFile = value; }
        }
    }


    public class GetPatient911InfoRequest : ModuleInfo
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "GetPatient911Info";
        private const string version = "1.0.0";

        private string patientId;

        public GetPatient911InfoRequest() : base(moduleName, functionName, version) { }

        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }


    }
    public class GetPatient911InfoResponse : Response
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "GetPatient911Info";
        private const string version = "1.0.0";

        private List<FILE_INFO> fileInfo;

        public GetPatient911InfoResponse() : base(moduleName, functionName, version) { }

        public List<FILE_INFO> FileInfo
        {
            get { return fileInfo; }
            set { fileInfo = value; }
        }
    }
    
    public class RegisterEicRequest : ModuleInfo
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "RegisterEic";
        private const string version = "1.0.0";
        string  patientId;
        Boolean _registerDevice;
        EIC_INFO eicInfo = new EIC_INFO();
        

        public RegisterEicRequest() : base(moduleName, functionName, version) { }
        
        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

        public EIC_INFO EicInfo
        {
            get { return eicInfo; }
            set { eicInfo = value; }
        }

        public Boolean RegisterDevice
        {
            get { return _registerDevice; }
            set { _registerDevice = value; }
        }       
    }
    public class RegisterEicResponse : Response
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "RegisterEic";
        private const string version = "1.0.0";
        private List<FILE_INFO> fileInfo;
        private string serverPublicKey;
        
        public RegisterEicResponse() : base(moduleName, functionName, version) { }

        public string ServerPublicKey
        {
            get { return serverPublicKey; }
            set { serverPublicKey = value; }
        }
	
        public List<FILE_INFO> FileInfo
        {
            get { return fileInfo; }
            set { fileInfo = value; }
        }
    }

    public class UnRegisterEicRequest : ModuleInfo
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "UnRegisterEic";
        private const string version = "1.0.0";
        string patientId;
        EIC_INFO eicInfo = new EIC_INFO();
        public UnRegisterEicRequest() : base(moduleName, functionName, version) { }
        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

    }
    public class UnRegisterEicResponse : Response
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "UnRegisterEic";
        private const string version = "1.0.0";
        
        public UnRegisterEicResponse() : base(moduleName, functionName, version) { }        

    }
    public class CheckRegisterEicRequest : ModuleInfo
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "UnRegisterEic";
        private const string version = "1.0.0";
        string eicSerialId;
        public CheckRegisterEicRequest() : base(moduleName, functionName, version) { }
        public string EicSerialId
        {
            get { return eicSerialId; }
            set { eicSerialId = value; }
        }

    }
    public class CheckRegisterEicResponse : Response
    {
        private const string moduleName = "Admin Module";
        private const string functionName = "UnRegisterEic";
        private const string version = "1.0.0";
        bool isAlreadyRegister;

        public CheckRegisterEicResponse() : base(moduleName, functionName, version) { } 

        public bool IsAlreadyRegister
        {
            get { return isAlreadyRegister; }
            set { isAlreadyRegister = value; }
        }
	

    }



 


}
