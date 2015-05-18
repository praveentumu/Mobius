using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C32Utility.Interface;
using Mobius.Entity;
using Mobius.CoreLibrary;

namespace C32Utility
{
    public class NISTValidation : IValidationService
    {
        #region Private variable
        private string _ValidationServiceURL = string.Empty;
        private Result _Result = null;
        #endregion Private variable

        #region Property
        /// <summary>
        /// This method returns list of validations available
        /// </summary>
        /// <returns></returns>
        public string ValidationServiceURL
        {
            get
            {
                if (MobiusAppSettingReader.UseOnlineService)
                {
                    _ValidationServiceURL = MobiusAppSettingReader.OnlineNISTValidationServiceURL;
                }
                else
                {
                    _ValidationServiceURL = MobiusAppSettingReader.MobiusNISTValidationServiceURL;
                }
                return _ValidationServiceURL;
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public Result Result
        {
            get { return _Result != null ? _Result : _Result = new Result(); }
            set { _Result = value; }
        }

        #endregion Property


        public Result getAvailableValidations(out List<MobiusAvailableValidations> availableValidations)
        {
            ValidationWebService DocValidationService;
            String sValidations = string.Empty;
            availableValidations = null;

            try
            {
                this.Result.IsSuccess = false;
                //Set the validation service url as per the configuration.
                DocValidationService = new ValidationWebService(this.ValidationServiceURL);

                //Set the validation service forcefully to local service, in case the object fails to initialize in normal process.
                if (DocValidationService == null)
                {
                    DocValidationService = new ValidationWebService(MobiusAppSettingReader.MobiusNISTValidationServiceURL);
                }

                List<WSSpecification> ValidationResult = DocValidationService.getAvailableValidations().ToList();
                sValidations = XmlSerializerHelper.SerializeObject(ValidationResult);
                if (!string.IsNullOrEmpty(sValidations))
                {
                    availableValidations = (List<MobiusAvailableValidations>)XmlSerializerHelper.DeserializeObject(sValidations, typeof(List<MobiusAvailableValidations>));
                    this.Result.IsSuccess = true;
                }
            }

            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }

            return this.Result;
        }


        /// <summary>
        /// This method actually validates the document and return the collection of issues of type specified by caller.
        /// </summary>
        /// <param name="specificationId"></param>
        /// <param name="document"></param>
        /// <param name="NISTValidationType"></param>
        /// <returns></returns>
        public Result validateDocument(string specificationId, string document, NISTValidationType NISTValidationType, out MobiusValidationResults validationResults)
        {
            ValidationWebService DocValidationService;
            validationResults = null;

            try
            {
                this.Result.IsSuccess = false;
                //Set the validation service url as per the configuration.
                DocValidationService = new ValidationWebService(this.ValidationServiceURL);

                //Set the validation service forcefully to local service, in case the object fails to initialize in normal process.
                if (DocValidationService == null)
                {
                    DocValidationService = new ValidationWebService(MobiusAppSettingReader.MobiusNISTValidationServiceURL);
                }
                WSValidationResults ValidationResult = DocValidationService.validateDocument(specificationId, document);
                string sValidationResult = XmlSerializerHelper.SerializeObject(ValidationResult);
                if (!string.IsNullOrEmpty(sValidationResult))
                {
                    validationResults = (MobiusValidationResults)XmlSerializerHelper.DeserializeObject(sValidationResult, typeof(MobiusValidationResults));
                    this.Result.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

    }

}
