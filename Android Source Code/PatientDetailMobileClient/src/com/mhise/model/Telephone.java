

package com.mhise.model;

/** 
*@(#)Telephone.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Telephone
* @since 2012-08-10
* @version 1.0 
*/
@SuppressWarnings("serial")
public class Telephone  implements java.io.Serializable {
    private java.lang.String extensionnumber;

    private java.lang.Integer id;

    private java.lang.String number;

    private java.lang.Boolean status;

    private java.lang.String type;

    public Telephone() {
    }

    public Telephone(
           java.lang.String extensionnumber,
           java.lang.Integer id,
           java.lang.String number,
           java.lang.Boolean status,
           java.lang.String type) {
           this.extensionnumber = extensionnumber;
           this.id = id;
           this.number = number;
           this.status = status;
           this.type = type;
    }


    /**
     * Gets the extensionnumber value for this Telephone.
     * 
     * @return extensionnumber
     */
    public java.lang.String getExtensionnumber() {
        return extensionnumber;
    }


    /**
     * Sets the extensionnumber value for this Telephone.
     * 
     * @param extensionnumber
     */
    public void setExtensionnumber(java.lang.String extensionnumber) {
        this.extensionnumber = extensionnumber;
    }


    /**
     * Gets the id value for this Telephone.
     * 
     * @return id
     */
    public java.lang.Integer getId() {
        return id;
    }


    /**
     * Sets the id value for this Telephone.
     * 
     * @param id
     */
    public void setId(java.lang.Integer id) {
        this.id = id;
    }


    /**
     * Gets the number value for this Telephone.
     * 
     * @return number
     */
    public java.lang.String getNumber() {
        return number;
    }


    /**
     * Sets the number value for this Telephone.
     * 
     * @param number
     */
    public void setNumber(java.lang.String number) {
        this.number = number;
    }


    /**
     * Gets the status value for this Telephone.
     * 
     * @return status
     */
    public java.lang.Boolean getStatus() {
        return status;
    }


    /**
     * Sets the status value for this Telephone.
     * 
     * @param status
     */
    public void setStatus(java.lang.Boolean status) {
        this.status = status;
    }


    /**
     * Gets the type value for this Telephone.
     * 
     * @return type
     */
    public java.lang.String getType() {
        return type;
    }


    /**
     * Sets the type value for this Telephone.
     * 
     * @param type
     */
    public void setType(java.lang.String type) {
        this.type = type;
    }


  
}
