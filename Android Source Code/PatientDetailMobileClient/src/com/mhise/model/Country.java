

package com.mhise.model;

/** 
*@(#)Country.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Country
* @since 2012-08-10
* @version 1.0 
*/
public class Country  implements java.io.Serializable {
    private java.lang.String countryName;

    public Country() {
    }

    public Country(
           java.lang.String countryName) {
           this.countryName = countryName;
    }


    /**
     * Gets the countryName value for this Country.
     * 
     * @return countryName
     */
    public java.lang.String getCountryName() {
        return countryName;
    }


    /**
     * Sets the countryName value for this Country.
     * 
     * @param countryName
     */
    public void setCountryName(java.lang.String countryName) {
        this.countryName = countryName;
    }

  
   
}
