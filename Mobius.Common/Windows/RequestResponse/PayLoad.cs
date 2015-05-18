using System;
using System.Collections.Generic;
using System.Text;
using FirstGenesis.Mobius.Common;
using FirstGenesis.Mobius.Common.DataTypes;

using System.Xml.Serialization;


namespace FirstGenesis.Mobius.RequestResponse
{
    public class ModuleInfo : MSerializable
    {
        private string module;
        private string version;
        private string function;

        public string Function
        {
            get { return function; }
            set { function = value; }
        }
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
        public string Module
        {
            get { return module; }
            set { module = value; }
        }
	
        public ModuleInfo(string Module, string Function, string Version)
        {
            module = Module;
            version = Version;
            function = Function;
        }
        public ModuleInfo()
        {

        }
    }

    public class ErrorInfo
    {
        ErrorCode errorCode;
        string description;

        public ErrorInfo()
        {
            errorCode = 0;
            description = "No error";
        }

        public ErrorCode Code
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

    }

    public class Response : ModuleInfo
    {
        ErrorInfo errorInfo = new ErrorInfo();

        public ErrorInfo ErrorInfo
        {
            get { return errorInfo; }
            set { errorInfo = value; }
        }
        Boolean result = false;

        public Boolean Result
        {
            get { return result; }
            set { result = value; }
        }

        public Response(string Module, string Function, string Version)
            : base(Module, Function, Version)
        {

        }
    }

    [XmlRoot(ElementName = "MobiusPayLoad", Namespace = @"http://www.mobius.com", IsNullable = false)]
    public class Payload : MSerializable
    {
        private string version;
        private string requestId;
        private string signature;
        private string xmlPayLoad;
        private ModuleInfo requestedModule = new ModuleInfo();
        ErrorInfo errorInfo = new ErrorInfo();
        public Payload()
        {
            requestId = Guid.NewGuid().ToString();
            version = "1.0.0";
        }
        bool result = false;

        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
        public ErrorInfo ErrorInfo
        {
            get { return errorInfo; }
            set { errorInfo = value; }
        }
        public ModuleInfo RequestedModule
        {
            get { return requestedModule; }
            set { requestedModule = value; }
        }
        private LoginRequest loginInfo = null;

        public LoginRequest LoginInfo
        {
            get { return loginInfo; }
            set { loginInfo = value; }
        }
        private string userToken = null;

        public string UserToken
        {
            get { return userToken; }
            set { userToken = value; }
        }

        public string PayLoad
        {
            get { return xmlPayLoad; }
            set { xmlPayLoad = value; }
        }

        public string RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public void SetPayLoad(ModuleInfo payLoad)
        {
            requestedModule.Module = payLoad.Module;
            requestedModule.Version = payLoad.Version;
            requestedModule.Function = payLoad.Function;

            xmlPayLoad = payLoad.Serialize();
            UTF8Encoding encoding = new UTF8Encoding();
            xmlPayLoad = System.Convert.ToBase64String(encoding.GetBytes(xmlPayLoad));
        }

        public string GetPayLoad()
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] array = System.Convert.FromBase64String(PayLoad);
            string xmlLoginRequest = encoding.GetString(array);
            return xmlLoginRequest;
        }

        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }
    }
}
