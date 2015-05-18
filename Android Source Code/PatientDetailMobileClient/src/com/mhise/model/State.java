

package com.mhise.model;

/** 
*@(#)State.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to State
* @since 2012-08-10
* @version 1.0 
*/
@SuppressWarnings("serial")
public class State  implements java.io.Serializable {
    private Country country;

    private java.lang.String stateName;

    public State() {
    }

    public State(
           Country country,
           java.lang.String stateName) {
           this.country = country;
           this.stateName = stateName;
    }


    /**
     * Gets the country value for this State.
     * 
     * @return country
     */
    public Country getCountry() {
        return country;
    }


    /**
     * Sets the country value for this State.
     * 
     * @param country
     */
    public void setCountry(Country country) {
        this.country = country;
    }


    /**
     * Gets the stateName value for this State.
     * 
     * @return stateName
     */
    public java.lang.String getStateName() {
        return stateName;
    }


    /**
     * Sets the stateName value for this State.
     * 
     * @param stateName
     */
    public void setStateName(java.lang.String stateName) {
        this.stateName = stateName;
    }



   
}
