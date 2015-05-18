

namespace AdapterPEP
{
    #region namesapce
    using System.Runtime.Serialization;
    using PolicyEngine;
    using System.Xml.Serialization;
    #endregion
    [DataContract(Name = "AdapterComponentMpiService", Namespace = "urn:gov:hhs:fha:nhinc:adaptercomponentmpi")]
    [KnownType(typeof(CheckPolicyResponseType))]
    [KnownType(typeof(ResultType))]
    public class CheckPolicyResponseClass : CheckPolicyResponseType
    {
    }
}
