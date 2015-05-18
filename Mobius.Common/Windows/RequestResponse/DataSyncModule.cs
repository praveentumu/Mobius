using System;
using System.Collections.Generic;
using System.Text;
using FirstGenesis.Mobius.Common.DataTypes;
using FirstGenesis.Mobius.Common;
using System.Collections;
using System.Data.Common;
using System.Data;

namespace FirstGenesis.Mobius.RequestResponse
{
    public class GetTotalEncounterRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "GetTotalEncounter";
        private const string version = "1.0.0";

        private string patientId;
        private string serverUri;
        public GetTotalEncounterRequest() : base(moduleName, functionName, version) { }

        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

        public string ServerUri
        {
            get { return serverUri; }
            set { serverUri = value; }
        }
    }
    public class GetTotalEncounterResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "GetTotalEncounter";
        private const string version = "1.0.0";

        public GetTotalEncounterResponse() : base(moduleName, functionName, version) { }

        private string totalEncNum;

        public string TotalEncNum
        {
            get { return totalEncNum; }
            set { totalEncNum = value; }
        }
    }

    public class TotalEncounterRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "TotalEncounter";
        private const string version = "1.0.0";

        private string patientId;

        public TotalEncounterRequest() : base(moduleName, functionName, version) { }

        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

    }
    public class TotalEncounterResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "TotalEncounter";
        private const string version = "1.0.0";

        public TotalEncounterResponse() : base(moduleName, functionName, version) { }

        private string totalEncNum;

        public string TotalEncNum
        {
            get { return totalEncNum; }
            set { totalEncNum = value; }
        }
    }

    public class PushEncounterInfoRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PushEncounterInfo";
        private const string version = "1.0.0";
        private USER _user;
        private RECORD record;

        public PushEncounterInfoRequest() : base(moduleName, functionName, version) { }

        private FACILITY_INFO patientFacilityInfo;
        public USER user
        {
            get { return _user; }
            set { _user = value; }
        }

        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }
        //private string serverUri;

        public FACILITY_INFO FacilityInfo
        {
            get { return patientFacilityInfo; }
            set { patientFacilityInfo = value; }
        }
        private string RecordSignature;
        public string Signature
        {
            get { return RecordSignature; }
            set { RecordSignature = value; }
        }

    }
    public class PushEncounterInfoResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PushEncounterInfo";
        private const string version = "1.0.0";

        public PushEncounterInfoResponse() : base(moduleName, functionName, version) { }
    }

    public class PushEncounterRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PushEncounter";
        private const string version = "1.0.0";
        //private string patientId;
        private RECORD record;
        private string RecordSignature;

        public PushEncounterRequest() : base(moduleName, functionName, version) { }

        //public string PatientId
        //{
        //    get { return patientId; }
        //    set { patientId = value; }
        //}

        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }
        public string Signature
        {
          get { return RecordSignature; }
          set { RecordSignature = value; }
        }

        


    }
    public class PushEncounterResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PushEncounter";
        private const string version = "1.0.0";

        public PushEncounterResponse() : base(moduleName, functionName, version) { }
    }


    public class PullEncounterInfoRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullEncounterInfo";
        private const string version = "1.0.0";

        private string patientId;
        private string serverUri;

        public PullEncounterInfoRequest() : base(moduleName, functionName, version) { }

        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }

        public string ServerUri
        {
            get { return serverUri; }
            set { serverUri = value; }
        }
    }
    public class PullEncounterInfoResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullEncounterInfo";
        private const string version = "1.0.0";

        public PullEncounterInfoResponse() : base(moduleName, functionName, version) { }

        private ArrayList encounterInfo;

        public ArrayList EncounterInfo
        {
            get { return encounterInfo; }
            set { encounterInfo = value; }
        }

    }

    public class PullEncounterRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullEncounter";
        private const string version = "1.0.0";

        private string patientId;

        public PullEncounterRequest() : base(moduleName, functionName, version) { }

        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }
    }
    public class PullEncounterResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullEncounter";
        private const string version = "1.0.0";

        public PullEncounterResponse() : base(moduleName, functionName, version) { }

        private ArrayList encounterInfo;

        public ArrayList EncounterInfo
        {
            get { return encounterInfo; }
            set { encounterInfo = value; }
        }

    }

    public class SyncAcknowledgementRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "SyncAcknowledgement";
        private const string version = "1.0.0";

        private ArrayList encounterId;
        private string serverUri;
        public SyncAcknowledgementRequest() : base(moduleName, functionName, version) { }

        public ArrayList EncounterId
        {
            get { return encounterId; }
            set { encounterId = value; }
        }

        public string ServerUri
        {
            get { return serverUri; }
            set { serverUri = value; }
        }

    }
    public class SyncAcknowledgementResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "SyncAcknowledgement";
        private const string version = "1.0.0";

        public SyncAcknowledgementResponse() : base(moduleName, functionName, version) { }
    }

    public class SynchronizedAcknowledgementRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "SynchronizedAcknowledgement";
        private const string version = "1.0.0";

        private ArrayList encounterId;

        public SynchronizedAcknowledgementRequest() : base(moduleName, functionName, version) { }

        public ArrayList EncounterId
        {
            get { return encounterId; }
            set { encounterId = value; }
        }



    }
    public class SynchronizedAcknowledgementResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "SynchronizedAcknowledgement";
        private const string version = "1.0.0";

        public SynchronizedAcknowledgementResponse() : base(moduleName, functionName, version) { }
    }

    public class PullUnSyncAhaltaEncRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullUnSyncAhaltaEnc";
        private const string version = "1.0.0";

        public PullUnSyncAhaltaEncRequest() : base(moduleName, functionName, version) { }
        private string serverUri;

        public string ServerUri
        {
            get { return serverUri; }
            set { serverUri = value; }
        }

    }
    public class PullUnSyncAhaltaEncResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullUnSyncAhaltaEnc";
        private const string version = "1.0.0";

        public PullUnSyncAhaltaEncResponse() : base(moduleName, functionName, version) { }

        private DataSet unSyncAhaltaEncmyVar;

        public DataSet UnSyncAhaltaEncmyVar
        {
            get { return unSyncAhaltaEncmyVar; }
            set { unSyncAhaltaEncmyVar = value; }
        }

    }

    public class PullUnSynchronizedAhaltaEncRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullUnSynchronizedAhaltaEnc";
        private const string version = "1.0.0";

        public PullUnSynchronizedAhaltaEncRequest() : base(moduleName, functionName, version) { }

    }
    public class PullUnSynchronizedAhaltaEncResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "PullUnSynchronizedAhaltaEnc";
        private const string version = "1.0.0";

        public PullUnSynchronizedAhaltaEncResponse() : base(moduleName, functionName, version) { }

        private DataSet unSyncAhaltaEnc;

        public DataSet UnSyncAhaltaEnc
        {
            get { return unSyncAhaltaEnc; }
            set { unSyncAhaltaEnc = value; }
        }

    }

    public class UploadEncounterRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "UploadEncounterInfo";
        private const string version = "1.0.0";

        private RECORD record;
        private string serverUri;

        public UploadEncounterRequest() : base(moduleName, functionName, version) { }

        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }

        

        public string ServerUri
        {
            get { return serverUri; }
            set { serverUri = value; }
        }

    }
    public class UploadEncounterResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "UploadEncounterInfo";
        private const string version = "1.0.0";

        public UploadEncounterResponse() : base(moduleName, functionName, version) { }
    }

    public class UploadEncRequest : ModuleInfo
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "UploadEncounter";
        private const string version = "1.0.0";

        private RECORD record;

        public UploadEncRequest() : base(moduleName, functionName, version) { }

        public RECORD Record
        {
            get { return record; }
            set { record = value; }
        }

    }
    public class UploadEncResponse : Response
    {
        private const string moduleName = "DataSync Module";
        private const string functionName = "UploadEncounter";
        private const string version = "1.0.0";

        public UploadEncResponse() : base(moduleName, functionName, version) { }
    }
}
