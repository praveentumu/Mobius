using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Resources;
using System.Reflection;

namespace FirstGenesis.Mobius.Common
{
    public enum ErrorCode
    {
        ErrorSuccess = 0x00000000,
        FileNotFound = 0x00000001,
        RecordNotFound = 0x00000002,
        LicenseNotFound = 0x00000003,
        DeviceNotConfigured = 0x00000004,
        InvalidParam    = 0x00000005,
        NotSupported = 0x00000006,
        UserNotFound = 0x00000007,
        UserNotLoggedIn = 0x00000008,
        DeviceAlreadyConfigured = 0x00000009,
        SoftwareNotConfigured = 0x00000010,
        AccessDenied = 0x00000011,
        InvalidSignature = 0x00000012,
        InvalidPassword = 0x00000013,
        ErrorUnknown = 0x00000014,
        DeviceNotAnEIC = 0x00000015,
        RepositoryNotUpdated = 0x00000016,
        EICNotPluggedIn = 0x00000017,
        InvalidServerResponse = 0x00000018,
        ErrorReadRegistry = 0x00000019,
        ErrorWriteRegistry = 0x00000020,
        ErrorOpenRegistry = 0x00000021,
        PathNotFound = 0x00000022,
        InvaidLoginCredential = 0x000000023,
        FailDbInstance = 0x000000024,
        AlreadyExist = 0x000000025,
        FailToCreateToken = 0x000000026,
        UnknownException = 0x000000027,
        ProblemRemovingToken = 0x000000028,
        NoMatchFoundDB = 0x000000029,
        FailToRegister = 0x000000030,
        FailToDelete = 0x000000031,
        FailToGenrateKey = 0x000000032,
        FailToImport = 0x000000033,
        FailToExport = 0x000000034,
        FailToGetDefaultPassword = 0x000000035,
        LicenseHasExpired = 0x000000036,
        FacilityNotAssociated = 0x000000037,
        InvalidRecordSignature = 0x000000038,
        InvalidLicenseSignature = 0x000000039,
        FailToGenerateIdnFile = 0x000000040,        
        EICConfiguredToDifferentUser = 0x000000042,
        DiffrentEICConfiguredToThisUser = 0x000000043,
        FailToSetPermissionAndCategory = 0x000000044,
        UserIdAlreadyExsist = 0x000000045,
        SSNAlreadyExsist = 0x000000046,
        UserIdNotActive = 0x000000047,
        NotEnoughSpace = 0x000000048,
        FailToPublish = 0x000000049,
        AlreadyPublish = 0x000000050,
        FacilityInfoUpdationFail = 0x000000051,
        InvalidEicHandle = 0x000000052
   
    }

    internal class ErrorMsg
    {
        ErrorCode _code;
        string _description;

        public ErrorCode Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }

    public class ErrorManager
    {
        static Hashtable _messages = new Hashtable();
        static ResourceManager _mgr = new ResourceManager("FirstGenesis.Mobius.Common.Properties.Resources", Assembly.GetExecutingAssembly());
        static ErrorCode _errorCode;

        public static string LastErrorDescription
        {
            get 
            {
                try
                {
                    string msgId = _messages[(object)LastError].ToString();
                    return _mgr.GetString(msgId);
                }
                catch
                {
                    return "Unknown error -- No description available for this error, Please contact your system administrator.";
                }
            }
        }

        public static ErrorCode LastError
        {
            get { return ErrorManager._errorCode; }
            set { ErrorManager._errorCode = value; }
        }

        public static void SetLastError(ErrorCode code)
        {
            _errorCode = code;
        }

        static ErrorManager()
        {
            InitializeErrorMessages();
        }

        //public static string GetLastErrorDescription()
        //{
        //    try
        //    {
        //        string msgId = _messages[(object)LastError].ToString();
        //        return _mgr.GetString(msgId);
        //    }
        //    catch
        //    {
        //        return "Unknown error -- No description available for this error, Please contact your system administrator.";
        //    }

        //}

        public static string GetDescription(ErrorCode code)
        {
            try
            {
                string msgId = _messages[(object)code].ToString();

                return _mgr.GetString(msgId);
            }
            catch
            {
                return "Unknown error -- No description available for this error, Please contact your system administrator.";
            }
        }

        /// <summary>
        /// Fills messages(hashmap) with error lists;
        /// </summary>
        /// <returns></returns>
        static void InitializeErrorMessages()
        {
            Type ErrorCodeType = typeof(ErrorCode);
            FieldInfo[] fieldInfo = ErrorCodeType.GetFields(BindingFlags.Public | BindingFlags.Static);
            for(int i=0;i<fieldInfo.Length;i++)
            {
                ErrorCode val = (ErrorCode)fieldInfo[i].GetValue(null);
                string name = fieldInfo[i].Name;
                _messages.Add(val, name);
            }
        }
    }
}