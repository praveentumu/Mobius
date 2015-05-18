
namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

     
    [DataContract]
    public class GetPatientDetailsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string MPIID
        {
            get;
            set;
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
