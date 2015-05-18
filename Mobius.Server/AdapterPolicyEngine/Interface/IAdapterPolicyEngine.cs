

namespace Mobius.ServiceLibrary
{
    #region Namespace
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using PolicyEngine;
    using Mobius.ServiceLibrary;
    using System.Xml;
    #endregion

   /// <summary>
    /// IAdapterPolicyEngine interface
   /// </summary>
    
    [ServiceKnownType(typeof(CheckPolicyResponseType))]
    [ServiceKnownType(typeof(ResultType))]
    //urn:gov:hhs:fha:nhinc:adapterpolicyengine
    [ServiceContract(Name = "AdapterPolicyEnginePortType"), XmlSerializerFormat(Style = OperationFormatStyle.Document,
     Use = OperationFormatUse.Literal, SupportFaults = true)]
   public interface IAdapterPolicyEngine
    {   
        /// <summary>
        /// CheckPolicy
        /// </summary>
        /// <param name="checkPolicyRequest">CheckPolicyRequestType class</param>
        /// <param name="assertion">AssertionType class</param>
        /// <returns>CheckPolicyResponse class</returns>
        /// 
        //urn:gov:hhs:fha:nhinc:adapterpolicyengine:CheckPolicy
        [OperationContract(Action = "urn:CheckPolicy"), XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal, SupportFaults = true)]
        CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequest, AssertionType assertion);

    }
}
