package com.mhise.model;

import java.util.ArrayList;


/** 
*@(#)ProviderRegistration.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to ProviderRegistration
* @since 2012-08-10
* @version 1.0 
*/
public class ProviderRegistration {

	//Organizational provider parameter
	String cSR ;
	String cityName;
	String country;
	String stateName;
	String contactNumber;
	String electronicServiceURI;
	String medicalRecordsDeliveryEmailAddress;
	String organizationName;
	String postalCode;
	String status;
	String addressLine1;
	String addressLine2;
	ArrayList<String> specialtyNameAndID;
	String providerType;
	String language;
	int ProviderCode;
	String password;
	//Individual provider parameter
	String firstName;
	String email;
	String gender;
	String lastName;
	String middleName;
	
	
    
    
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

	
	
	public void setProviderCode(int ProviderCode)
	{
		this.ProviderCode= ProviderCode  ;
	}	
	public int getProviderCode()
	{
		return this.ProviderCode  ;
	}
	
	public void setCSR(String cSR)
	{
		this.cSR= cSR  ;
	}	
	public String getCSR()
	{
		return this.cSR  ;
	}
	
	public void setCityName(String cityName)
	{
		this.cityName= cityName  ;
	}
	public String getCityName()
	{
		return this.cityName  ;
	}
	
	public void setCountry(String country)
	{
		this.country= country  ;
	}	
	public String getCountry()
	{
		return this.country  ;
	}
	
	public void setStateName(String stateName)
	{
		this.stateName= stateName  ;
	}	
	public String getStateName()
	{
		return this.stateName  ;
	}
	
	public void setContactNumber(String contactNumber)
	{
		this.contactNumber= contactNumber  ;
	}
	public String getContactNumber()
	{
		return this.contactNumber  ;
	}
	
	public void setElectronicServiceURI(String electronicServiceURI)
	{
		this.electronicServiceURI= electronicServiceURI  ;
	}	
	public String getElectronicServiceURI()
	{
		return this.electronicServiceURI  ;
	}
	
	public void setMedicalRecordsDeliveryEmailAddress(String medicalRecordsDeliveryEmailAddress)
	{
		this.medicalRecordsDeliveryEmailAddress= medicalRecordsDeliveryEmailAddress  ;
	}
	public String getMedicalRecordsDeliveryEmailAddress()
	{
		return this.medicalRecordsDeliveryEmailAddress  ;
	}
	
	public void setOrganizationName(String organizationName)
	{
		this.organizationName= organizationName  ;
	}
	public String getOrganizationName()
	{
		return this.organizationName  ;
	}
	
	public void setPostalCode(String postalCode)
	{
		this.postalCode= postalCode  ;
	}	
	public String getPostalCode()
	{	
		return this.postalCode  ;
	}
	
	public void setStatus(String status)
	{
		this.status= status  ;
	}	
	public String getStatus()
	{
		return this.status  ;
	}
	
	public void setAddressLine1(String addressLine1)
	{
		this.addressLine1= addressLine1  ;
	}
	public String getAddressLine1()
	{
		return this.addressLine1  ;
	}	
	
	public void setAddressLine2(String addressLine2)
	{
		this.addressLine2= addressLine2  ;
	}
	public String getAddressLine2()
	{
		return this.addressLine2  ;
	}
	public ArrayList<String> getspecialtyNameAndID()
	{
		return this.specialtyNameAndID  ;
	}
	public void setspecialtyNameAndID(ArrayList<String> specialtyName)
	{
		this.specialtyNameAndID= specialtyName  ;
	}
	
	public String getProviderType()
	{
		return this.providerType  ;
	}
	public void setProviderType(String providerType)
	{
		this.providerType= providerType  ;
	}
	
	public String getLanguage()
	{
		return this.language  ;
	}
	public void setLanguage(String language)
	{
		this.language= language  ;
	}
	
	public String getFirstName()
	{
		return this.firstName  ;
	}
	public void setFirstName(String firstName)
	{
		this.firstName= firstName  ;
	}
	
	public String getLastName()
	{
		return this.lastName  ;
	}
	public void setLastName(String lastName)
	{
		this.lastName= lastName  ;
	}
	
	public String getMiddleName()
	{
		return this.middleName  ;
	}
	public void setMiddleName(String middleName)
	{
		this.middleName= middleName  ;
	}
	
	public String getEmail()
	{
		return this.email  ;
	}
	public void setEmail(String email)
	{
		this.email= email  ;
	}
	
	public String getGender()
	{
		return this.gender  ;
	}
	public void setGender(String gender)
	{
		this.gender= gender  ;
	}
	
	
}
