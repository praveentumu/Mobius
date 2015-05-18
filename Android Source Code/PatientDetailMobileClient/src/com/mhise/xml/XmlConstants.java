package com.mhise.xml;

/** 
*@(#)XmlConstants.java 
* @author R Systems
* @description this class contains all the Soap request namespaces and action tags used in Application
* @since 2012-10-26
* @version 1.0 
*/

public class XmlConstants {
	
	//XML request tags
	public static final String XMLNS="xmlns:";
	public static final String NS_SOAP="soap";	
	public static final String NS_URN="urn";
	public static final String NS_WSA="wsa";
	public static final String NS_MOB="mob";
	public static final String NS_ARR="arr";
	  
	public static final String VALUE_SOAP="http://www.w3.org/2003/05/soap-envelope";
	public static final String VALUE_URN="urn:MHISE";
	public static final String VALUE_WSA="http://www.w3.org/2005/08/addressing";
	public static final String VALUE_MOB="http://schemas.datacontract.org/2004/07/MobiusServiceLibrary";
	 public static final String VALUE_ARR="http://schemas.microsoft.com/2003/10/Serialization/Arrays";
	
	public static final String ACTION_ADD_PROVIDER="urn:MHISE/IMobius/AddProvider";
    public static final String ACTION_ADD_PATIENT="urn:MHISE/IMobius/AddPatient";
	public static final String ACTION_GETLOCALITYBYZIPCODE="urn:MHISE/IMobius/GetLocalityByZipCode";
	public static final String ACTION_GETMASTERDATA="urn:MHISE/IMobius/GetMasterData";
	public static final String ACTION_GET_APPLICATION_VERSION="urn:MHISE/IMobius/GetApplicationVersion";
	
	public static final String ACTION_GETUSERINFORMATION="urn:MHISE/IMobiusSecured/GetUserInformation";
	public static final String ACTION_GET_COMMUNITIES="urn:MHISE/IMobiusSecured/GetNhinCommunity";
	public static final String ACTION_GET_PHI_SOURCE="urn:MHISE/IMobiusSecured/GetPHISource";
	public static final String ACTION_FOR_SEARCH_PATIENT = "urn:MHISE/IMobiusSecured/SearchPatient";
	public static final String ACTION_FOR_GET_DOCUMENT_METADATA =														
														"urn:MHISE/IMobiusSecured/GetDocumentMetadata";
	
	public static final String ACTION_GETDOCUMENT="urn:MHISE/IMobiusSecured/GetDocument";
	
	public static final String ACTION_LOGIN="urn:MHISE/IMobius/AuthenticateUser";
	public static final String ACTION_FORGOT_PASSWORD="urn:MHISE/IMobius/ForgotPassword";
	public static final String ACTION_CHANGE_PASSWORD="urn:MHISE/IMobiusSecured/ChangePassword";
	public static final String ACTION_ADD_PFX_CERTIFICATE="urn:MHISE/IMobiusSecured/AddPFXCertificate";
	public static final String ACTION_GET_USERCERTIFICATE="urn:MHISE/IMobius/GetPFXCertificate";
	public static final String ACTION_ACTIVATE_USER="urn:MHISE/IMobius/ActivateUser";
	public static final String ACTION_GET_CSR_DETAILS="urn:MHISE/IMobius/GetCSRDetails";

	
}
