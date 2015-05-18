using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Mobius.CoreLibrary;
using FirstGenesis.Mobius.Logging;
using Mobius.BAL;

namespace AdapterPEP
{
    
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class AdapterPEP : IAdapterPEPPortType
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

        
        public CheckPolicyResponseType CheckPolicy(CheckPolicyRequestType checkPolicyRequestType, AssertionType assertion)
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
                    logMessage("Object of PEP.AssertionType is Null");
                else
                {
                    try
                    {
                        logMessage("PEP.AssertionType SerializeObject : ");
                        logMessage("PEP.AssertionType" + XmlSerializerHelper.SerializeObject(assertion));
                    }
                    catch (Exception ex)
                    {

                    }
                }

                checkPolicyResponseType = this.MobiusBAL.CheckPolicyPEP(checkPolicyRequestType, assertion);
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
                logger.WriteLog(LogSeverity.DEBUG, "AdapterPEP", message);
            }
            catch (Exception ex)
            {
            }

        }

    }
}
