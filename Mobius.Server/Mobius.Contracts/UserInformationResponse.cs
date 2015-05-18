

namespace MobiusServiceLibrary
{
    #region Namespace
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using System;
    using MobiusServiceUtility;
    #endregion

     
    [DataContract]
    public class UserInformationResponse
    {
        private Result _result = null;
        private UserInformation _userInformation = null;

        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        [DataMember]
        public UserInformation UserInformation
        {
            get { return _userInformation != null ? _userInformation : _userInformation = new UserInformation(); }
            set { _userInformation = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }
    }
}
