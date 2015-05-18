package com.mobius.validate;
import gov.nist.hitsp.validation.Dependency;
import gov.nist.hitsp.validation.DocumentType;
import gov.nist.hitsp.validation.ValidationResults;
import gov.nist.hitsp.validation.xsd.WSIndividualValidationResult;
import gov.nist.hitsp.validation.xsd.WSSpecification;
import gov.nist.hitsp.validation.xsd.WSValidationResults;
import gov.nist.hl7.v3.xml.util.MiscXmlUtil;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileInputStream;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;
import javax.xml.parsers.ParserConfigurationException;

import org.w3c.dom.Document;
import org.xml.sax.SAXException;


public class Validation {

	/**
	 * @param args
	 */
    public static void main(String[] args) throws JAXBException, IOException {
        // TODO Auto-generated method stub



        String content = "<name>Shailesh</name>";
        String resultDetail = "all";
        String validationType = "c32_v2_5";

        //String validationListStr= showAvailableValidations();
        List<WSSpecification> lst = showAvailableValidations();
        System.out.print("***************" + lst.size());
        List<String> resultList = validatedocument(content, resultDetail, validationType);
        ArrayList<WSIndividualValidationResult> results = new ArrayList<WSIndividualValidationResult>();
        for (String st : resultList) {


            System.out.println("****start******" + st + "*****end****");

            List<WSIndividualValidationResult> tempresult = unmarshallResult(st);
            results.addAll(tempresult);


        }
        System.out.println("#############size is##################" + results.size());
        WSValidationResults wSValidationResults = new WSValidationResults();
        wSValidationResults.issue = results;
    }

    
    
	//This method returning list of all the available validations.
    public static List<WSSpecification> showAvailableValidations() {
        String docTypesLocation = new Validation().getPropertyValue("docTypesLocation");
        List<DocumentType> documentTypes = null;

        List<WSSpecification> list = null;
        try {
            documentTypes = DocumentType
                    .parseDocumentTypes(MiscXmlUtil.fileToDom(docTypesLocation));

            list = createSpecificationList(documentTypes);

        } catch (SAXException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (ParserConfigurationException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        return list;
    }


	
	//This method validate the document based on inputs 
    public static ArrayList<String> validatedocument(String content, String resultDetail,
            String validationType) throws JAXBException {

        // TODO Auto-generated method stub
        String skeletonLocation = new Validation().getPropertyValue("skeletonLocation");
        String docTypesLocation = new Validation().getPropertyValue("docTypesLocation");
        InputStream inputStream = null;
        ArrayList<String> list = new ArrayList();
       // File file = new File("C:\\CCD_Example_Meaningful_Use_Phllip_Jones.xml");
        try {
            content = content.trim();
            

            inputStream = new ByteArrayInputStream(content.getBytes());
            //inputStream = new FileInputStream(file);
            Document doc = MiscXmlUtil.inputStreamToDom(inputStream);
            //Document doc = MiscXmlUtil.stringToDom(content);
            List<DocumentType> docTypes = DocumentType
                    .parseDocumentTypes(MiscXmlUtil.fileToDom(docTypesLocation));
            DocumentType documentType = DocumentType.findDocumentType(
                    validationType, docTypes);
            ArrayList<String> resultDetailList = processResultDetail(resultDetail);


            ValidationResults res = documentType.doFullValidation(inputStream,
                    doc, docTypes, resultDetailList, false, skeletonLocation);

            list = (ArrayList<String>) res.getSchematronErrors();

        } catch (FileNotFoundException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            System.out.println("\n" + e.getMessage());
            System.out.println("\n" + e.toString());
        } catch (SAXException e) {
            // TODO Auto-generated catch block
            list.add("Error.  Unable to parse incoming XML.  Check for valid content.  (Also check for extraneous characters at the beginning of the file.");
        } catch (ParserConfigurationException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            System.out.println("\n" + e.getMessage());
            System.out.println("\n" + e.toString());
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
            System.out.println("\n" + e.getMessage());
            System.out.println("\n" + e.toString());
        }
        return list;

    }


        
        
	//creating SpecificationList from list of DocumentType
	private static  List<WSSpecification> createSpecificationList(List<DocumentType> documentTypes) {
		
                 List<WSSpecification> specList=new ArrayList<WSSpecification>();
		for (DocumentType doctype : documentTypes) {

			//DocType type = new DocType();
			WSSpecification spec=new WSSpecification();
			
			
                        spec.setDescription(doctype.getDescription());
                        spec.setName(doctype.getDisplayName());
                        spec.setSpecificationId(doctype.getId());
			
			ArrayList<String> dependencyList=new ArrayList<String>();
			List<Dependency> dependencies=doctype.getDependencies();
			for(Dependency dependency:dependencies){
				String dep=dependency.getDocumentType();
				
				dependencyList.add(dep);
			}
			
                        specList.add(spec);
		}
                
   
return specList;
	}
	
//Getting Result Detail for given input result type.
	private static ArrayList<String> processResultDetail(String resultDetailInput) {
		ArrayList<String> resultDetail = new ArrayList();

		System.out.println(resultDetailInput);
		if ("all".equals(resultDetail)) {
			resultDetail.add("#ALL");
		} else if ("errorsWarnings".equals(resultDetailInput)) {
			resultDetail.add("errors");
			resultDetail.add("warning");
			resultDetail.add("violation");
		} else if ("errors".equals(resultDetailInput)) {
			resultDetail.add("errors");
		} else {
			resultDetail.add("#ALL");
		}
		return resultDetail;
	}
        
 
//Marshalling xml string  to List of  WSIndividualValidationResult      
    public static List<WSIndividualValidationResult> unmarshallResult(String resultString) throws JAXBException, IOException {


        String tempstr = "<RootResults>" + resultString + "</RootResults>";
        tempstr = tempstr.replaceAll("urn:gov:nist:cdaGuideValidator", "");

        InputStream inputStream = new ByteArrayInputStream(tempstr.getBytes());
        JAXBContext jaxbContext = JAXBContext.newInstance(RootResults.class);

        Unmarshaller jaxbUnmarshaller = jaxbContext.createUnmarshaller();
        RootResults rootresults = (RootResults) jaxbUnmarshaller.unmarshal(inputStream);
        List<Results> resultList = rootresults.getResults();
        List<WSIndividualValidationResult> issueList = new ArrayList<WSIndividualValidationResult>();
        for (Results results : resultList) {

            List<ValidationResult> validationresultlist = results.getValidationResult();
            if (validationresultlist != null) {
                for (ValidationResult result : validationresultlist) {
                    WSIndividualValidationResult res = new WSIndividualValidationResult();

                    Issue issue = result.getIssue();
                    res.setContext(issue.getContext());
                    res.setMessage(issue.getMessage());
                    res.setSeverity(issue.getSeverity());
                    res.setSpecification(issue.getSpecification());
                    res.setTest(issue.getTest());


                    issueList.add(res);
                }

            }
        }


        return issueList;


    }
 
    //Reading value from property file
    private String getPropertyValue(String key) {
        String value = "";
        try {

            Properties configProp = new Properties();
            InputStream in = this.getClass().getResourceAsStream("/resources/config.properties");
            configProp.load(in);
            value = (String) configProp.get(key);

        } catch (FileNotFoundException ex) {
            ex.printStackTrace();
            System.out.println("\n" + ex.getMessage());
            System.out.println("\n" + ex.toString());
        } catch (IOException ex) {
            ex.printStackTrace();
            System.out.println("\n" + ex.getMessage());
            System.out.println("\n" + ex.toString());
        }
        return value;

    }
 
 

}
