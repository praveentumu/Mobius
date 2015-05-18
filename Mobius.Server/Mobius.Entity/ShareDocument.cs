using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Mobius.CoreLibrary;

namespace Mobius.Entity
{
    [XmlType("ShareDocumentRequest")]
    [Serializable]
    public class ShareDocument
    {
        /// <summary>
        /// 
        /// </summary>

        public byte[] docByteData
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>

        public MobiusAssertion Assertion
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>

        public byte[] XACMLbyteData
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>

        public string patientId
        {
            get;
            set;
        }



        /// <summary>
        /// 
        /// </summary>

        public string RemotePatientId
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>

        public string RemoteCommunityId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>

        public string subject
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>

        public string homeCommunityId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>

        public string sourceRepositryId
        {
            get;
            set;
        }


        /// <summary>
        /// get or set document original id 
        /// </summary>

        public string OriginalDocumentID { get; set; }

        /// <summary>
        /// 
        /// </summary>



    }
}
