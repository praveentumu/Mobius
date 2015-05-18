
package com.mhise.model;

import java.io.Serializable;

/** 
*@(#)AuthenticateUserResponse.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to User Login response
* @since 2012-08-10
* @version 1.0 
*/
public class AuthenticateUserResponse implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;

	private  String CertificateSerialNumber ;
	private  String name ;
	private  Result result ;
	
	
	
	  /**
     * Gets the CertificateSerialNumber value for this CertificateSerialNumber.
     * 
     * @return CertificateSerialNumber
     */
    public java.lang.String getCertificateSerialNumber() {
        return CertificateSerialNumber;
    }


    /**
     * Sets the CertificateSerialNumber value for this CertificateSerialNumber.
     * 
     * @param CertificateSerialNumber
     */
    public void setCertificateSerialNumber(java.lang.String CertificateSerialNumber) {
        this.CertificateSerialNumber = CertificateSerialNumber;
    }
    
   
    /**
     * Gets the name value for this name.
     * 
     * @return name
     */
    public java.lang.String getname() {
        return name;
    }


    /**
     * Sets the name value for this name.
     * 
     * @param name
     */
    public void setname(java.lang.String name) {
        this.name = name;
    }
    
    
    /**
     * Gets the Result value for this Result.
     * 
     * @return Result
     */
    public Result getResult() {
        return result;
    }


    /**
     * Sets the name value for this name.
     * 
     * @param name
     */
    public void setResult(Result result) {
        this.result = result;
    }
    
	
	
}
