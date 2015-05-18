using System;
using System.Collections.Generic;
using System.Text;
using FirstGenesis.Mobius.Common.DataTypes;

namespace FirstGenesis.Mobius.RequestResponse
{   
    
    public class LoginRequest : ModuleInfo 
    {
        private const string moduleName = "Authorization Module";
        private const string functionName  = "Login";
        private const string version    = "1.0.0";

        private string userId;
        private string password;
        private string facilityId;
        private Boolean loginAsAdmin = false;

        public Boolean LoginAsAdmin
        {
            get { return loginAsAdmin; }
            set { loginAsAdmin = value; }
        }

        public LoginRequest() : base(moduleName, functionName, version)
        {
          
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string FacilityId
        {
            get { return facilityId; }
            set { facilityId = value; }
        }
    }


    public class LoginResponse : Response
    {
        private const string moduleName = "Authorization Module";
        private const string functionName = "Login";
        private const string version = "1.0.0";
        
        public LoginResponse() : base(moduleName, functionName, version)
        {
        }

        private string token;

        public string Token
        {
            get { return token; }
            set { token = value; }
        }   
    }


    public class GetUserDetailsRequest : ModuleInfo
    {
        private const string moduleName = "Authorization Module";
        private const string functionName = "GetUserDetails";
        private const string version = "1.0.0";

        private string token;

        public GetUserDetailsRequest()
            : base(moduleName, functionName, version)
        {

        }
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
    }


    public class GetUserDetailsResponse : Response
    {
        private const string moduleName = "Authorization Module";
        private const string functionName = "GetUserDetails";
        private const string version = "1.0.0";

        public GetUserDetailsResponse() : base(moduleName, functionName, version)
        {
        }

        private USER user;
        
        public USER User
        {
            get { return user; }
            set { user = value; }
        }

        private Boolean canWorkOffline = false;

        public Boolean CanWorkOffline
        {
            get { return canWorkOffline; }
            set { canWorkOffline = value; }
        }
	
    }


}
