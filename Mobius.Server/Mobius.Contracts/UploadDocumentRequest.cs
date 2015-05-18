namespace MobiusServiceLibrary
{
    using System;
    using System.Runtime.Serialization;
    using MobiusServiceUtility;

     
    [DataContract]
    public class UploadDocumentRequest
    {
        [DataMember]
        public string CommunityId { get; set; }

        [DataMember]
        public string DocumentId { get; set; }

        [DataMember]
        public byte[] DocumentBytes { get; set; }

        [DataMember]
        public byte[] XACMLBytes { get; set; }

        [DataMember]
        public string PatientId { get; set; }

        [DataMember]
        public string UploadedBy { get; set; }

        [DataMember]
        public string RepositoryId { get; set; }

        [DataMember]
        public bool SubmitOnGateway { get; set; }


        /// <summary>
        /// 
        /// </summary>
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
}
