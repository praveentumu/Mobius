/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mobius.validate;

import java.util.List;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlRootElement;

/**
 *
 * @author shailendra.singh
 */
@XmlAccessorType(XmlAccessType.FIELD) 
@XmlRootElement(name = "RootResults")
public class RootResults {
  private List<Results> results;  

    /**
     * @return the results
     */
    public List<Results> getResults() {
        return results;
    }

    /**
     * @param results the results to set
     */
    public void setResults(List<Results> results) {
        this.results = results;
    }
}
