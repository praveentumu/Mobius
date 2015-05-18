

namespace MobiusServiceLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using MobiusServiceUtility;    
    
    [DataContract]
    public class GetNhinCommunityResponse
    {

        private Result _result = null;
        private List<NHINCommunity> _NHINCommunities = null;

        /// <summary>
        /// This particular member represents has array of communities.
        /// </summary>
        [DataMember]
        public List<NHINCommunity> Communities 
        {
            get { return _NHINCommunities != null ? _NHINCommunities : _NHINCommunities = new List<NHINCommunity>(); }
            set { _NHINCommunities = value; }
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
