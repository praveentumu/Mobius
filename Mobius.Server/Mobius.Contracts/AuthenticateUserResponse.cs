// -----------------------------------------------------------------------
// <copyright file="AuthenticateUserResponse.cs" company="R Systems">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MobiusServiceLibrary
{
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using Mobius.CoreLibrary;

    [DataContract]
    public class AuthenticateUserResponse
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


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CertificateSerialNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Name
        {
            get;
            set;
        }
    }
}
