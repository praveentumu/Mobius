
package com.mhise.model;

/** 
*@(#)Patient.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Patient
* @since 2012-08-10
* @version 1.0 
*/
@SuppressWarnings("serial")
public class Patient  implements java.io.Serializable {

	public Address[] address;

	public java.lang.String CSR;

	public java.lang.String communityId;

	public Demographics demographics;

	public java.lang.String emailAddress;

	public java.lang.String facilityID;

	public java.lang.String patientId;

	public java.lang.String publicKey;

    public java.lang.String serialNumber;
    
    public java.lang.String password;

    public Telephone[] telephone;

    public Patient() {
    }

    public Patient(
           Address[] address,
           java.lang.String CSR,
           java.lang.String communityId,
           Demographics demographics,
           java.lang.String emailAddress,
           java.lang.String facilityID,
           java.lang.String patientId,
           java.lang.String publicKey,
           java.lang.String serialNumber,
           java.lang.String password,
           Telephone[] telephone) {
           this.address = address;
           this.CSR = CSR;
           this.communityId = communityId;
           this.demographics = demographics;
           this.emailAddress = emailAddress;
           this.facilityID = facilityID;
           this.patientId = patientId;
           this.publicKey = publicKey;
           this.serialNumber = serialNumber;
           this.telephone = telephone;
           this.password=password;
           
    }


    /**
     * Gets the address value for this Patient.
     * 
     * @return address
     */
    public Address[] getAddress() {
        return address;
    }


    /**
     * Sets the address value for this Patient.
     * 
     * @param address
     */
    public void setAddress(Address[] address) {
        this.address = address;
    }


    /**
     * Gets the CSR value for this Patient.
     * 
     * @return CSR
     */
    public java.lang.String getCSR() {
        return CSR;
    }


    /**
     * Sets the CSR value for this Patient.
     * 
     * @param CSR
     */
    public void setCSR(java.lang.String CSR) {
        this.CSR = CSR;
    }


    /**
     * Gets the communityId value for this Patient.
     * 
     * @return communityId
     */
    public java.lang.String getCommunityId() {
        return communityId;
    }


    /**
     * Sets the communityId value for this Patient.
     * 
     * @param communityId
     */
    public void setCommunityId(java.lang.String communityId) {
        this.communityId = communityId;
    }


    /**
     * Gets the demographics value for this Patient.
     * 
     * @return demographics
     */
    public Demographics getDemographics() {
        return demographics;
    }


    /**
     * Sets the demographics value for this Patient.
     * 
     * @param demographics
     */
    public void setDemographics(Demographics demographics) {
        this.demographics = demographics;
    }


    /**
     * Gets the emailAddress value for this Patient.
     * 
     * @return emailAddress
     */
    public java.lang.String getEmailAddress() {
        return emailAddress;
    }


    /**
     * Sets the emailAddress value for this Patient.
     * 
     * @param emailAddress
     */
    public void setEmailAddress(java.lang.String emailAddress) {
        this.emailAddress = emailAddress;
    }

    
    
    
    /**
     * Gets the Password value for this Patient.
     * 
     * @return address
     */
    public java.lang.String  getPassword() {
        return password;
    }


    /**
     * Sets the Password value for this Patient.
     * 
     * @param address
     */
    public void setPassword(java.lang.String Password) {
        this.password = Password;
    }

    
    

    /**
     * Gets the facilityID value for this Patient.
     * 
     * @return facilityID
     */
    public java.lang.String getFacilityID() {
        return facilityID;
    }


    /**
     * Sets the facilityID value for this Patient.
     * 
     * @param facilityID
     */
    public void setFacilityID(java.lang.String facilityID) {
        this.facilityID = facilityID;
    }


    /**
     * Gets the patientId value for this Patient.
     * 
     * @return patientId
     */
    public java.lang.String getPatientId() {
        return patientId;
    }


    /**
     * Sets the patientId value for this Patient.
     * 
     * @param patientId
     */
    public void setPatientId(java.lang.String patientId) {
        this.patientId = patientId;
    }


    /**
     * Gets the publicKey value for this Patient.
     * 
     * @return publicKey
     */
    public java.lang.String getPublicKey() {
        return publicKey;
    }


    /**
     * Sets the publicKey value for this Patient.
     * 
     * @param publicKey
     */
    public void setPublicKey(java.lang.String publicKey) {
        this.publicKey = publicKey;
    }


    /**
     * Gets the serialNumber value for this Patient.
     * 
     * @return serialNumber
     */
    public java.lang.String getSerialNumber() {
        return serialNumber;
    }


    /**
     * Sets the serialNumber value for this Patient.
     * 
     * @param serialNumber
     */
    public void setSerialNumber(java.lang.String serialNumber) {
        this.serialNumber = serialNumber;
    }


    /**
     * Gets the telephone value for this Patient.
     * 
     * @return telephone
     */
    public Telephone[] getTelephone() {
        return telephone;
    }


    /**
     * Sets the telephone value for this Patient.
     * 
     * @param telephone
     */
    public void setTelephone(Telephone[] telephone) {
        this.telephone = telephone;
    }

  
 
}
