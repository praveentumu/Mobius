

namespace AdapterPEP
{
    #region Namespace
    using System.ServiceModel;
    using System.ServiceModel.Web;        
    using System.Xml; 
    #endregion

//   /// <summary>
//    /// IAdapterPolicyEngine interface
//   /// </summary>
    
//    //[ServiceKnownType(typeof(CheckPolicyResponseType))]
//    //[ServiceKnownType(typeof(ResultType))]
//    ////urn:gov:hhs:fha:nhinc:adapterpolicyengine
//    [ServiceContract(Name = "AdapterPEPPortType"), XmlSerializerFormat(Style = OperationFormatStyle.Document,
//     Use = OperationFormatUse.Literal, SupportFaults = true)]
//    public interface IAdapterPEPPortType
//    {   
//        ///// <summary>
//        ///// CheckPolicy
//        ///// </summary>
//        ///// <param name="checkPolicyRequest">CheckPolicyRequestType class</param>
//        ///// <param name="assertion">AssertionType class</param>
//        ///// <returns>CheckPolicyResponse class</returns>
//        ///// 
//        ////urn:gov:hhs:fha:nhinc:adapterpolicyengine:CheckPolicy
//        [OperationContract(Action = "urn:CheckPolicy"), XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal, SupportFaults = true)]
//        //CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequest, AssertionType assertion);
////        [System.Xml.Serialization.XmlElementAttribute(Namespace="urn:gov:hhs:fha:nhinc:common:nhinccommonadapter")]
//         CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequestType, AssertionType assertion);

//    }


    [ServiceContract(Name = "AdapterPEPPortType"), XmlSerializerFormat(Style = OperationFormatStyle.Document,
    Use = OperationFormatUse.Literal, SupportFaults = true)]
    public interface IAdapterPEPPortType
    {


        [OperationContract(Action = "urn:CheckPolicy"), XmlSerializerFormat(Style = OperationFormatStyle.Document,
            Use = OperationFormatUse.Literal, SupportFaults = true)]  
        CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequestType, AssertionType assertion);
    }

}
