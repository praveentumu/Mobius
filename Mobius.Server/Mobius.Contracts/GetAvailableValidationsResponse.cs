
namespace MobiusServiceLibrary
{
    #region Namespace
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Mobius.CoreLibrary;
    
    
    #endregion Namespace

    [DataContract]
    public class GetAvailableValidationsResponse
    {
        private Result _result;
        List<AvailableValidations> _availableValidations=null;
        
        /// <summary>
        /// Contains Result object
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        [DataMember]
        public List<AvailableValidations> AvailableValidations
        {
            get { return _availableValidations != null ? _availableValidations : _availableValidations = new List<AvailableValidations>(); }
            set { _availableValidations = value; }
        }


    }
}
