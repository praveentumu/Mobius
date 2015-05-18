/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mobius.validate;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author shailendra.singh
 */
@XmlAccessorType(XmlAccessType.FIELD) 
@XmlRootElement(name = "validationResult") 
public class ValidationResult {
    private Issue issue;

    /**
     * @return the issue
     */
    public Issue getIssue() {
        return issue;
    }

    /**
     * @param issue the issue to set
     */
    public void setIssue(Issue issue) {
        this.issue = issue;
    }
}
