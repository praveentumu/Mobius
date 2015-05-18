
package gov.nist.hitsp.validation;

import gov.nist.hitsp.validation.xsd.WSIndividualValidationResult;
import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlElementDecl;
import javax.xml.bind.annotation.XmlRegistry;
import javax.xml.namespace.QName;
import gov.nist.hitsp.validation.xsd.WSValidationResults;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the gov.nist.hitsp.validation package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {

    private final static QName _ValidateDocumentDocument_QNAME = new QName("http://validation.hitsp.nist.gov", "document");
    private final static QName _ValidateDocumentSpecificationId_QNAME = new QName("http://validation.hitsp.nist.gov", "specificationId");
    private final static QName _ValidateDocumentDescription_QNAME = new QName("http://validation.hitsp.nist.gov", "description");
    private final static QName _ValidateDocumentName_QNAME = new QName("http://validation.hitsp.nist.gov", "name");
    private final static QName _ValidateDocumentResponseReturn_QNAME = new QName("http://validation.hitsp.nist.gov", "return");
    
    public  final static QName _ValidateDocumentResponsecontext_QNAME = new QName("http://validation.hitsp.nist.gov", "context");
    public final static QName _ValidateDocumentResponsemessage_QNAME = new QName("http://validation.hitsp.nist.gov", "message");
    public final static QName _ValidateDocumentResponseseverity_QNAME = new QName("http://validation.hitsp.nist.gov", "severity");
    
    public final static QName _ValidateDocumentResponsetest_QNAME = new QName("http://validation.hitsp.nist.gov", "test");
    
    public final static QName _Specification_QNAME = new QName("http://validation.hitsp.nist.gov", "Specification");
    
    
        
        
      /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    
    
    
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "Specification", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultSpecification(String value) {
        return new JAXBElement<String>(_Specification_QNAME, String.class, WSIndividualValidationResult.class, value);
    }
    
    
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "test", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResulttest(String value) {
        return new JAXBElement<String>(_ValidateDocumentResponsetest_QNAME, String.class, WSIndividualValidationResult.class, value);
    }
    
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "context", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultContext(String value) {
        return new JAXBElement<String>(_ValidateDocumentResponsecontext_QNAME, String.class, WSIndividualValidationResult.class, value);
    }
    
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "message", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultmessage(String value) {
        return new JAXBElement<String>(_ValidateDocumentResponsemessage_QNAME, String.class, WSIndividualValidationResult.class, value);
    }
    
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "severity", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultseverity(String value) {
        return new JAXBElement<String>(_ValidateDocumentResponseseverity_QNAME, String.class, WSIndividualValidationResult.class, value);
    }
    
    
        
        
    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: gov.nist.hitsp.validation
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link ValidateDocumentResponse }
     * 
     */
    public ValidateDocumentResponse createValidateDocumentResponse() {
        return new ValidateDocumentResponse();
    }

    /**
     * Create an instance of {@link ValidateDocument }
     * 
     */
    public ValidateDocument createValidateDocument() {
        return new ValidateDocument();
    }

    /**
     * Create an instance of {@link GetAvailableValidationsResponse }
     * 
     */
    public GetAvailableValidationsResponse createGetAvailableValidationsResponse() {
        return new GetAvailableValidationsResponse();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "document", scope = ValidateDocument.class)
    public JAXBElement<String> createValidateDocumentDocument(String value) {
        return new JAXBElement<String>(_ValidateDocumentDocument_QNAME, String.class, ValidateDocument.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "specificationId", scope = ValidateDocument.class)
    public JAXBElement<String> createValidateDocumentSpecificationId(String value) {
        return new JAXBElement<String>(_ValidateDocumentSpecificationId_QNAME, String.class, ValidateDocument.class, value);
    }
    
    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "description", scope = ValidateDocument.class)
    public JAXBElement<String> createValidateDocumentDescription(String value) {
        return new JAXBElement<String>(_ValidateDocumentDescription_QNAME, String.class, ValidateDocument.class, value);
    }
    
    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "name", scope = ValidateDocument.class)
    public JAXBElement<String> createValidateDocumentName(String value) {
        return new JAXBElement<String>(_ValidateDocumentName_QNAME, String.class, ValidateDocument.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link WSValidationResults }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov", name = "return", scope = ValidateDocumentResponse.class)
    public JAXBElement<WSValidationResults> createValidateDocumentResponseReturn(WSValidationResults value) {
        return new JAXBElement<WSValidationResults>(_ValidateDocumentResponseReturn_QNAME, WSValidationResults.class, ValidateDocumentResponse.class, value);
    }

}
