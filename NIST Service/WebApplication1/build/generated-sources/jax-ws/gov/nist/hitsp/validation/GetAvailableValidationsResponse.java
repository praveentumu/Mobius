
package gov.nist.hitsp.validation;


import java.util.List;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;
import gov.nist.hitsp.validation.xsd.WSSpecification;
import java.util.ArrayList;


/**
 * <p>Java class for anonymous complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType>
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="return" type="{http://validation.hitsp.nist.gov/xsd}WSSpecification" maxOccurs="unbounded" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "_return"
})
@XmlRootElement(name = "getAvailableValidationsResponse")
public class GetAvailableValidationsResponse {

  @XmlElement(name = "return", nillable = true)
    private List<WSSpecification> _return;

    /**
     * @return the _return
     */
      public List<WSSpecification> getReturn() {
        if (_return == null) {
            _return = new ArrayList<WSSpecification>();
        }
        return this._return;
    }

    /**
     * @param _return the _return to set
     */
    public void setReturn(List<WSSpecification> _return) {
        this._return = _return;
    }

}
