
namespace MobiusServiceLibrary
{

    #region NameSpace
    using Mobius.CoreLibrary;
    using System.Runtime.Serialization;
    using System;
    using System.Collections.Generic;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;
    #endregion

     
    [DataContract]
    public class GetC32SectionsResponse
    {
        private Result _result = null;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<C32Section> C32Sections
        {
            get;
            set;
        }

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
        public SoapProperties SoapProperties
        {
            get;
            set;
        }
    }
}
