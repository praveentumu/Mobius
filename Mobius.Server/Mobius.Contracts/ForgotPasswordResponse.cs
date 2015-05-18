
namespace MobiusServiceLibrary
{
    #region NameSpace
    using System.Runtime.Serialization;
    using System;
    using Mobius.CoreLibrary;
    using Mobius.CoreLibrary;
    #endregion

     
    [DataContract]
    public class ForgotPasswordResponse
    {
        private Result _result;


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }
    }
}
