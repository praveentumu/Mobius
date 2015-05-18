
package gov.nist.hitsp.validation.xsd;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlElementDecl;
import javax.xml.bind.annotation.XmlRegistry;
import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the gov.nist.hitsp.validation.xsd package. 
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

    private final static QName _WSIndividualValidationResultMessage_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "message");
    private final static QName _WSIndividualValidationResultTest_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "test");
    private final static QName _WSIndividualValidationResultSpecification_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "specification");
    private final static QName _WSIndividualValidationResultContext_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "context");
    private final static QName _WSIndividualValidationResultSeverity_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "severity");
    private final static QName _WSSpecificationSpecificationId_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "specificationId");
    private final static QName _WSSpecificationDescription_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "description");
    private final static QName _WSSpecificationName_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "name");
    private final static QName _WSValidationResultsValidationTime_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "validationTime");
    private final static QName _WSValidationResultsValidationDate_QNAME = new QName("http://validation.hitsp.nist.gov/xsd", "validationDate");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: gov.nist.hitsp.validation.xsd
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link WSValidationResults }
     * 
     */
    public WSValidationResults createWSValidationResults() {
        return new WSValidationResults();
    }

    /**
     * Create an instance of {@link WSIndividualValidationResult }
     * 
     */
    public WSIndividualValidationResult createWSIndividualValidationResult() {
        return new WSIndividualValidationResult();
    }

    /**
     * Create an instance of {@link WSSpecification }
     * 
     */
    public WSSpecification createWSSpecification() {
        return new WSSpecification();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "message", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultMessage(String value) {
        return new JAXBElement<String>(_WSIndividualValidationResultMessage_QNAME, String.class, WSIndividualValidationResult.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "test", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultTest(String value) {
        return new JAXBElement<String>(_WSIndividualValidationResultTest_QNAME, String.class, WSIndividualValidationResult.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "specification", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultSpecification(String value) {
        return new JAXBElement<String>(_WSIndividualValidationResultSpecification_QNAME, String.class, WSIndividualValidationResult.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "context", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultContext(String value) {
        return new JAXBElement<String>(_WSIndividualValidationResultContext_QNAME, String.class, WSIndividualValidationResult.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "severity", scope = WSIndividualValidationResult.class)
    public JAXBElement<String> createWSIndividualValidationResultSeverity(String value) {
        return new JAXBElement<String>(_WSIndividualValidationResultSeverity_QNAME, String.class, WSIndividualValidationResult.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "specificationId", scope = WSSpecification.class)
    public JAXBElement<String> createWSSpecificationSpecificationId(String value) {
        return new JAXBElement<String>(_WSSpecificationSpecificationId_QNAME, String.class, WSSpecification.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "description", scope = WSSpecification.class)
    public JAXBElement<String> createWSSpecificationDescription(String value) {
        return new JAXBElement<String>(_WSSpecificationDescription_QNAME, String.class, WSSpecification.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "name", scope = WSSpecification.class)
    public JAXBElement<String> createWSSpecificationName(String value) {
        return new JAXBElement<String>(_WSSpecificationName_QNAME, String.class, WSSpecification.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "validationTime", scope = WSValidationResults.class)
    public JAXBElement<String> createWSValidationResultsValidationTime(String value) {
        return new JAXBElement<String>(_WSValidationResultsValidationTime_QNAME, String.class, WSValidationResults.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://validation.hitsp.nist.gov/xsd", name = "validationDate", scope = WSValidationResults.class)
    public JAXBElement<String> createWSValidationResultsValidationDate(String value) {
        return new JAXBElement<String>(_WSValidationResultsValidationDate_QNAME, String.class, WSValidationResults.class, value);
    }

}
