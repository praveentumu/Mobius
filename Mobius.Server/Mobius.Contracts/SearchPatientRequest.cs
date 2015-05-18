
namespace MobiusServiceLibrary
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    using MobiusServiceLibrary;    
    using System;
    using MobiusServiceUtility;
 

    #region Search Patient Request
    [KnownType(typeof(AddressStatus))]
    [KnownType(typeof(NHINCommunity))]
     
    [DataContract]
    public class SearchPatientRequest
    {
        [DataMember]
        public Demographics Demographics
        {
            get;
            set;
        }

        [DataMember]
        public List<Community> NHINCommunities
        {
            get;
            set;
        }

        

        [DataMember]
        public Assertion Assertion
        {
            get;
            set;
        }

        [DataMember]
        public SoapProperties SoapProperties
        {
            get;
            set;
        }

    }
    #endregion
}
