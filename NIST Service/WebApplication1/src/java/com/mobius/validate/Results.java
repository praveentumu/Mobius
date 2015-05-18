/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mobius.validate;

import java.util.List;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;

/**
 *
 * @author shailendra.singh
 */
@XmlAccessorType(XmlAccessType.FIELD) 
@XmlRootElement(name = "Results")


public class Results {
    private List<ValidationResult> validationResult;

    /**
     * @return the validationResult
     */
    public List<ValidationResult> getValidationResult() {
        return validationResult;
    }

    /**
     * @param validationResult the validationResult to set
     */
    public void setValidationResult(List<ValidationResult> validationResult) {
        this.validationResult = validationResult;
    }
}
