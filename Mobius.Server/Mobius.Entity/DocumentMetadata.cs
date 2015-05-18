

namespace Mobius.Entity
{
    #region Nnamespace
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    [Serializable]
    public class DocumentMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public string OriginalDocumentId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PatientId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UploadedBy
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DocumentTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Author
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string CreatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SourceCommunityId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FacilityId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Reposed
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string XacmlDocumentId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsShared
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SharedId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SourceRepositryId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DocumentSource
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string XACMLfileName
        {
            get;
            set;
        }


    }
}
