
package gov.nist.hitsp.validation.xsd;

import java.util.ArrayList;
import java.util.List;
import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for WSValidationResults complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="WSValidationResults">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="issue" type="{http://validation.hitsp.nist.gov/xsd}WSIndividualValidationResult" maxOccurs="unbounded" minOccurs="0"/>
 *         &lt;element name="validationDate" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="validationTest" type="{http://www.w3.org/2001/XMLSchema}boolean" minOccurs="0"/>
 *         &lt;element name="validationTime" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "WSValidationResults", propOrder = {
    "issue",
    "validationDate",
    "validationTest",
    "validationTime"
})
public class WSValidationResults {

    @XmlElement(nillable = true)
    protected List<WSIndividualValidationResult> issue;
    @XmlElementRef(name = "validationDate", namespace = "http://validation.hitsp.nist.gov/xsd", type = JAXBElement.class, required = false)
    protected JAXBElement<String> validationDate;
    protected Boolean validationTest;
    @XmlElementRef(name = "validationTime", namespace = "http://validation.hitsp.nist.gov/xsd", type = JAXBElement.class, required = false)
    protected JAXBElement<String> validationTime;

    /**
     * Gets the value of the issue property.
     * 
     * <p>
     * This accessor method returns a reference to the live list,
     * not a snapshot. Therefore any modification you make to the
     * returned list will be present inside the JAXB object.
     * This is why there is not a <CODE>set</CODE> method for the issue property.
     * 
     * <p>
     * For example, to add a new item, do as follows:
     * <pre>
     *    getIssue().add(newItem);
     * </pre>
     * 
     * 
     * <p>
     * Objects of the following type(s) are allowed in the list
     * {@link WSIndividualValidationResult }
     * 
     * 
     */
    public List<WSIndividualValidationResult> getIssue() {
        if (issue == null) {
            issue = new ArrayList<WSIndividualValidationResult>();
        }
        return this.issue;
    }

    /**
     * Gets the value of the validationDate property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getValidationDate() {
        return validationDate;
    }

    /**
     * Sets the value of the validationDate property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setValidationDate(JAXBElement<String> value) {
        this.validationDate = value;
    }

    /**
     * Gets the value of the validationTest property.
     * 
     * @return
     *     possible object is
     *     {@link Boolean }
     *     
     */
    public Boolean isValidationTest() {
        return validationTest;
    }

    /**
     * Sets the value of the validationTest property.
     * 
     * @param value
     *     allowed object is
     *     {@link Boolean }
     *     
     */
    public void setValidationTest(Boolean value) {
        this.validationTest = value;
    }

    /**
     * Gets the value of the validationTime property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getValidationTime() {
        return validationTime;
    }

    /**
     * Sets the value of the validationTime property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setValidationTime(JAXBElement<String> value) {
        this.validationTime = value;
    }

}
