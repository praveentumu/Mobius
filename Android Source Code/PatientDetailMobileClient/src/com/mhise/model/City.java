

package com.mhise.model;

/** 
*@(#)City.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to City
* @since 2012-08-10
* @version 1.0 
*/
public class City  implements java.io.Serializable {
   
	private static final long serialVersionUID = 1L;

	private java.lang.String cityName;

	private com.mhise.model.Result result;
	
    private State state;

    public City() {
    }

    public City(
           java.lang.String cityName,
           State state) {
           this.cityName = cityName;
           this.state = state;
    }

    public Result getResult() {
        return result;
    }
    
    public void setResult(Result result) {
        this.result = result;
    }

    /**
     * Gets the cityName value for this City.
     * 
     * @return cityName
     */
    public java.lang.String getCityName() {
        return cityName;
    }


    /**
     * Sets the cityName value for this City.
     * 
     * @param cityName
     */
    public void setCityName(java.lang.String cityName) {
        this.cityName = cityName;
    }


    /**
     * Gets the state value for this City.
     * 
     * @return state
     */
    public State getState() {
        return state;
    }


    /**
     * Sets the state value for this City.
     * 
     * @param state
     */
    public void setState(State state) {
        this.state = state;
    }

   

    
}
