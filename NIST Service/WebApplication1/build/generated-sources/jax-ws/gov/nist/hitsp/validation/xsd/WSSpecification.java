/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package gov.nist.hitsp.validation.xsd;

/**
 *
 * @author shailendra.singh
 */
public class WSSpecification {
    private String description;
    
    private String name;
    
    private String specificationId;

    /**
     * @return the description
     */
    public String getDescription() {
        return description;
    }

    /**
     * @param description the description to set
     */
    public void setDescription(String description) {
        this.description = description;
    }

    /**
     * @return the name
     */
    public String getName() {
        return name;
    }

    /**
     * @param name the name to set
     */
    public void setName(String name) {
        this.name = name;
    }

    /**
     * @return the specificationId
     */
    public String getSpecificationId() {
        return specificationId;
    }

    /**
     * @param specificationId the specificationId to set
     */
    public void setSpecificationId(String specificationId) {
        this.specificationId = specificationId;
    }
}
