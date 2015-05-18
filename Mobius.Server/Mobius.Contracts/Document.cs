using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class Document
    {
        #region Private variables
        private string _DocumentTargetCommunityId = String.Empty;
        private string _DocumentUniqueID = String.Empty;
        private string _DocumentTitle = String.Empty;
        private string _Uploadedby = String.Empty;
        private string _DocumentTargetId = String.Empty;
        private string _SourcePatientId = String.Empty;
        private string _RepositoryUniqueId = String.Empty;
        private string _DataSource = String.Empty;
        private string _Author = String.Empty;
        private string _CreatedOn = String.Empty;
        private byte[] _ByteData;
        private byte[] _XACMLBytesData;
        private int _DocumentType;
        private bool _IsShared;
        #endregion Private variables

        #region Public Property
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SourceCommunityId
        {
            get { return _DocumentTargetCommunityId; }
            set { _DocumentTargetCommunityId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DocumentUniqueId
        {
            get { return _DocumentUniqueID; }
            set { _DocumentUniqueID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DocumentTitle
        {
            get { return _DocumentTitle; }
            set { _DocumentTitle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Author
        {
            get { return _Author; }
            set { _Author = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public byte[] DocumentBytes
        {

            get { return _ByteData; }
            set { _ByteData = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public byte[] XACMLBytes
        {

            get { return _XACMLBytesData; }
            set { _XACMLBytesData = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RepositoryUniqueId
        {
            get { return _RepositoryUniqueId; }
            set { _RepositoryUniqueId = value; }
        }

        /// <summary>
        /// 
        /// </summary>       
        [DataMember]
        public string SourcePatientId
        {
            get { return _SourcePatientId; }
            set { _SourcePatientId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string DocumnetTargetID
        {
            get { return _DocumentTargetId; }
            set { _DocumentTargetId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string UploadedBy
        {
            get { return _Uploadedby; }
            set { _Uploadedby = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int DocumentType
        {
            get { return _DocumentType; }
            set { _DocumentType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsShared
        {
            get { return _IsShared; }
            set { _IsShared = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string XACMLDocumentId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool Reposed
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Location
        {
            get;
            set;
        }

        [DataMember]
        public string Community { get; set; }

        #endregion Public Property

    }
}
