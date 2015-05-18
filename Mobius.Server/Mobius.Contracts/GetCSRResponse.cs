namespace MobiusServiceLibrary
{

    #region Namespaces
    using System;
    using Mobius.CoreLibrary;
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
    #endregion

     
    [DataContract]
    public class GetCSRResponse
    {
        private Result _result = null;

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
        public string FamilyName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string GivenName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string OrganizationName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string State
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Country
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsIndividualProvider
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string EmailAddress
        {
            get;
            set;
        }


    }
}
