namespace MobiusServiceLibrary
{

    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

     
    [DataContract]
    public class GetPatientDetailsbyDocumentIdRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DocumentID
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
