
namespace Mobius.ServiceLibrary
{
    
    #region Namespace
    using System;
    using System.ServiceModel;
    using FirstGenesis.Mobius.Logging;
    using Mobius.BAL;
    using Mobius.CoreLibrary;
    using PatientDiscovery;
    #endregion


    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    /// <summary>
    /// partial class MobiusHISE for contract IAdapterComponentMpi
    /// </summary>  
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any, Namespace = "urn:gov:hhs:fha:nhinc:adaptercomponentmpi")]
    public class AdapterComponentMPI : IAdapterComponentMpi
    {
        /// <summary>
        /// TODO- Remove logging code before going live.  
        /// </summary>
        
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

        #region FindCandidates
        /// <summary>
        /// Get Community_PRPA_IN201306UV02ResponseType call array object
        /// </summary>
        /// <param name="reqType">object of RespondingGateway_PRPA_IN201305UV02RequestType class</param>
        /// <returns>object of PRPA_IN201306UV02ResponseType class</returns>
        //[WebMethod]
        public PRPA_IN201306UV02ResponseType FindCandidates(RespondingGateway_PRPA_IN201305UV02RequestType RespondingGateway_PRPA_IN201305UV02Request)
        {
            PRPA_IN201306UV02ResponseType PRPA_IN201306UV02ResponseType = new PRPA_IN201306UV02ResponseType();
            Community_PRPA_IN201306UV02ResponseType Community_PRPA_IN201306UV02ResponseType = new Community_PRPA_IN201306UV02ResponseType();
            try
            {
                logMessage("====Call Started===============");
                if (RespondingGateway_PRPA_IN201305UV02Request == null)
                {
                    logMessage("Object of RespondingGateway_PRPA_IN201305UV02Request is Null");
                }
                else
                {
                    try
                    {
                        logMessage("RespondingGateway_PRPA_IN201305UV02Request SerializeObject : ");
                        logMessage("RespondingGateway_PRPA_IN201305UV02Request" + XmlSerializerHelper.SerializeObject(RespondingGateway_PRPA_IN201305UV02Request));
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    
                }
                
                Community_PRPA_IN201306UV02ResponseType = this.MobiusBAL.FindCandidates(RespondingGateway_PRPA_IN201305UV02Request);
                logMessage("Call to FindCandidates method");
                PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02 = Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02;
                logMessage("get response from FindCandidates method");
                if (Community_PRPA_IN201306UV02ResponseType == null)
                    logMessage("Community_PRPA_IN201306UV02ResponseType object is null");
                else
                {
                    try
                    {
                        logMessage("Community_PRPA_IN201306UV02ResponseType SerializeObject : ");                        
                        logMessage("Community_PRPA_IN201306UV02ResponseType" + XmlSerializerHelper.SerializeObject(PRPA_IN201306UV02ResponseType));
                    }
                    catch (Exception ex)
                    {
                       
                    }

                }
            }
            catch (Exception ex)
            {
                logMessage("====Error ===============");
                logMessage("Error message --" + ex.Message);
                Community_PRPA_IN201306UV02ResponseType = new Community_PRPA_IN201306UV02ResponseType();
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02 = new PRPA_IN201306UV02();
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess = new PRPA_IN201306UV02MFMI_MT700711UV01ControlActProcess();
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject = new PRPA_IN201306UV02MFMI_MT700711UV01Subject1[1] { new PRPA_IN201306UV02MFMI_MT700711UV01Subject1() };
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent = new PRPA_IN201306UV02MFMI_MT700711UV01RegistrationEvent();
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1 = new PRPA_IN201306UV02MFMI_MT700711UV01Subject2();
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient = new PRPA_MT201310UV02Patient();
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.id = new II[1] { new II() };
                Community_PRPA_IN201306UV02ResponseType.PRPA_IN201306UV02.controlActProcess.subject[0].registrationEvent.subject1.patient.Item = new PRPA_MT201310UV02Person();

            }
            logMessage("====Call End=============== ");
            return PRPA_IN201306UV02ResponseType;
        }

        private void logMessage(string message)
        {
            
            try
            {
                Logger logger;
                logger = Logger.GetInstance();
                logger.WriteLog(LogSeverity.DEBUG ,"AdapterComponentMPI", message);                
            }
            catch(Exception ex)
            {
                            

            }

        }

        #endregion
    }
}
