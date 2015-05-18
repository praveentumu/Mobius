using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class ValidateDocumentResponse
    {
        private Result _result;
        private ValidationResults _ValidationResults;

        /// <summary>
        /// Contains Result object
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        
        public ValidationResults ValidationResult
        {
            get { return _ValidationResults != null ? _ValidationResults : _ValidationResults = new ValidationResults(); }
            set { _ValidationResults = value; }
        }
    }
}
