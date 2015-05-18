/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mobius.service;


import com.mobius.validate.Validation;
import gov.nist.hitsp.validation.GetAvailableValidationsResponse;
import gov.nist.hitsp.validation.xsd.*;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.jws.WebService;
import javax.xml.bind.JAXBException;

/**
 *
 * @author shailendra.singh
 */
@WebService(serviceName = "ValidationWebService", portName = "ValidationWebServiceSOAP11port_http", endpointInterface = "gov.nist.hitsp.validation.ValidationWebServicePortType", targetNamespace = "http://validation.hitsp.nist.gov", wsdlLocation = "WEB-INF/wsdl/NewWebServiceFromWSDL/hit-testing.nist.gov_11080/ws/services/ValidationWebService.wsdl")
public class MobiusWebService  {

    public gov.nist.hitsp.validation.xsd.WSValidationResults validateDocument(java.lang.String specificationId, java.lang.String document) {

      List<WSIndividualValidationResult> results = null;
        WSValidationResults wSValidationResults = new WSValidationResults();
        try {

            List<String> resultList = Validation.validatedocument(document, "errorsWarnings", specificationId);

            results = new ArrayList<WSIndividualValidationResult>();
            for (String st : resultList) {

                List<WSIndividualValidationResult> tempresult = Validation.unmarshallResult(st);
                results.addAll(tempresult);
            }

            wSValidationResults.issue = results;
            wSValidationResults.setValidationDate(new java.util.Date().toString());
            wSValidationResults.setValidationTime(Long.toString(new java.util.Date().getTime()));
        } catch (JAXBException ex) {
            Logger.getLogger(MobiusWebService.class.getName()).log(Level.SEVERE, null, ex);
        } catch (IOException ex) {
            Logger.getLogger(MobiusWebService.class.getName()).log(Level.SEVERE, null, ex);
        }

        return wSValidationResults;

    }


    
    public GetAvailableValidationsResponse getAvailableValidations() {
        GetAvailableValidationsResponse response = new GetAvailableValidationsResponse();

        List<WSSpecification> lts = Validation.showAvailableValidations();
        response.setReturn(lts);

        return response;
    }
    
}
