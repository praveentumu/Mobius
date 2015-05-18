using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PatientDiscovery;
using Mobius.BAL;
using System.IO;
using Mobius.CoreLibrary;
using FirstGenesis.Mobius.Logging;
using PolicyEngine;


namespace Mobius.ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    /// <summary>
    /// partial class MobiusHISE for contract IAdapterComponentMpi
    /// </summary>  
    /// "urn:gov:hhs:fha:nhinc:adapterpolicyengine"
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    /// <summary>
    /// partial class MobiusHISE for contract IAdapterPolicyEngine
    /// </summary>
    public partial class AdapterPolicyEngine : IAdapterPolicyEngine
    {
        
        /// TODO- Remove logging code before going live.               
        #region Properties
        private MobiusBAL _MobiusBAL = null;

        /// <summary>
        /// Gets the MobiusBAL class called MobiusBAL
        /// </summary>
        private MobiusBAL MobiusBAL
        {
            get
            {
                return _MobiusBAL != null ? _MobiusBAL : _MobiusBAL = new MobiusBAL();
            }
        }



        #endregion

        #region CheckPolicy
        /// <summary>
        /// CheckPolicy
        /// </summary>
        /// <param name="checkPolicyRequestType">object of CheckPolicyRequestType class</param>
        /// <param name="assertion">object of assertion class</param>
        /// <returns>return object of CheckPolicyResponse</returns>
        public CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequestType, PolicyEngine.AssertionType assertion)
        {
            CheckPolicyResponseType checkPolicyResponseType = new CheckPolicyResponseType();            
            ResultType resultType = new ResultType();
            try
            {
                logMessage("====Call Started===============");
                if (checkPolicyRequestType == null)
                    logMessage("Object of checkPolicyRequestType is Null");
                else
                {
                    try
                    {
                        logMessage("checkPolicyRequestType SerializeObject : ");
                        logMessage("checkPolicyRequestType" + XmlSerializerHelper.SerializeObject(checkPolicyRequestType));
                    }
                    catch (Exception ex)
                    {
                      
                    }
                }

                if (assertion == null)
                    logMessage("Object of PolicyEngine.AssertionType is Null");
                else
                {
                    try
                    {
                        logMessage("PolicyEngine.AssertionType SerializeObject : ");
                        logMessage("PolicyEngine.AssertionType" + XmlSerializerHelper.SerializeObject(assertion));
                    }
                    catch (Exception ex)
                    {
                       
                    }
                }              
                
                checkPolicyResponseType = this.MobiusBAL.CheckPolicy(checkPolicyRequestType, assertion);
                logMessage("Call to CheckPolicy method");
               
                logMessage("get response from FindCandidates method");

                if (checkPolicyResponseType == null)
                    logMessage("checkPolicyResponseType object is null");
                else
                {
                    try
                    {
                        logMessage("checkPolicyResponseType SerializeObject : ");
                        logMessage("checkPolicyResponseType" + XmlSerializerHelper.SerializeObject(checkPolicyResponseType));
                    }
                    catch (Exception ex)
                    {
                       
                    }
                }

            }
            catch (Exception ex)
            {
                logMessage("====Error===========");
                logMessage(ex.Message);
                checkPolicyResponseType.response = new ResultType[1] { new ResultType() };
                resultType.Decision = (DecisionType)Enum.Parse(typeof(DecisionType), DecisionType.Deny.ToString(), true);
                resultType.Status = new StatusType();
                resultType.Status.StatusMessage = ex.Message.ToString();
                checkPolicyResponseType.response[0] = resultType;

            }
            logMessage("====Call End===============");
            return checkPolicyResponseType;
        }


        private void logMessage(string message)
        {
            
            try
            {
                Logger logger = Logger.GetInstance();
                logger.WriteLog(LogSeverity.DEBUG, "AdapterPolicyEngine", message);
            }
            catch (Exception ex)
            {
            }

        }

        #endregion
    }
}
