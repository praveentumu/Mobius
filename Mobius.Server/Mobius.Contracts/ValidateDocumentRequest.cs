using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;
using Mobius.CoreLibrary;
using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class ValidateDocumentRequest
    {
        //private string specificationId;
        //private string document;
        //private NISTValidationType _ValidationType;

        [DataMember]
        public string SpecificationId
        {
            get;
            set;
        }

        [DataMember]
        public string Document
        {
            get;
            set;
        }

        [DataMember]
        public NISTValidationType ValidationType
        {
            get;
            set;
        }
    }
}
