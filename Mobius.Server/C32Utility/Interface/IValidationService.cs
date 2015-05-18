using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C32Utility;
using Mobius.Entity;
using Mobius.CoreLibrary;

namespace C32Utility.Interface
{
    interface IValidationService
    {
        Result getAvailableValidations(out List<MobiusAvailableValidations> availableValidations);
        Result validateDocument(string specificationId, string document, NISTValidationType NISTValidationType, out MobiusValidationResults validationResults);
    }
}
