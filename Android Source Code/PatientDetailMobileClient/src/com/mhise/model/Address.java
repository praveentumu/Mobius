
package com.mhise.model;

import java.io.Serializable;

/** 
*@(#)Address.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Address
* @since 2012-08-10
* @version 1.0 
*/

public class Address implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	private java.lang.String addressLine1;

    private java.lang.String addressLine2;

    private City city;

    private java.lang.Integer id;

    private String status;

    private java.lang.String zip;

    public Address() {
    }

    public Address(
           java.lang.String addressLine1,
           java.lang.String addressLine2,
           City city,
           java.lang.Integer id,
           String status,
           java.lang.String zip) {
           this.addressLine1 = addressLine1;
           this.addressLine2 = addressLine2;
           this.city = city;
           this.id = id;
           this.status = status;
           this.zip = zip;
    }


    /**
     * Gets the addressLine1 value for this Address.
     * 
     * @return addressLine1
     */
    public java.lang.String getAddressLine1() {
        return addressLine1;
    }


    /**
     * Sets the addressLine1 value for this Address.
     * 
     * @param addressLine1
     */
    public void setAddressLine1(java.lang.String addressLine1) {
        this.addressLine1 = addressLine1;
    }


    /**
     * Gets the addressLine2 value for this Address.
     * 
     * @return addressLine2
     */
    public java.lang.String getAddressLine2() {
        return addressLine2;
    }


    /**
     * Sets the addressLine2 value for this Address.
     * 
     * @param addressLine2
     */
    public void setAddressLine2(java.lang.String addressLine2) {
        this.addressLine2 = addressLine2;
    }


    /**
     * Gets the city value for this Address.
     * 
     * @return city
     */
    public City getCity() {
        return city;
    }


    /**
     * Sets the city value for this Address.
     * 
     * @param city
     */
    public void setCity(City city) {
        this.city = city;
    }


    /**
     * Gets the id value for this Address.
     * 
     * @return id
     */
    public java.lang.Integer getId() {
        return id;
    }


    /**
     * Sets the id value for this Address.
     * 
     * @param id
     */
    public void setId(java.lang.Integer id) {
        this.id = id;
    }


    /**
     * Gets the status value for this Address.
     * 
     * @return status
     */
    public String getStatus() {
        return status;
    }


    /**
     * Sets the status value for this Address.
     * 
     * @param status
     */
    public void setStatus(String status) {
        this.status = status;
    }


    /**
     * Gets the zip value for this Address.
     * 
     * @return zip
     */
    public java.lang.String getZip() {
        return zip;
    }


    /**
     * Sets the zip value for this Address.
     * 
     * @param zip
     */
    public void setZip(java.lang.String zip) {
        this.zip = zip;
    }



   

}
