
package com.mhise.constants;


import android.os.Environment;


/** 
*@(#)Constants.java 
* @author R Systems
* @description This class contains the constants used in the MobiusDroid application
* @since 2012-08-04
* @version 1.0 
*/
public interface Constants {
	
	/*Upgrade Time*/
	public static long upgradeTime =60*1000;

	/* Certificate Details */
	public  String PKCS12_FILENAME = "MobiusCerificate.p7b";
	/**
	 * The pass phrase of the PKCS12 file
	 */
	public static  String KEY_PKCS12_PASSWORD ="PASSWORD" ;
	public static  String TAG_CERTIFICATE ="PASSWORD" ;
	public static  String KEY_CERT_NAME ="CertificateName" ;
	public static  String TAG_PRIVATEKEY ="PRIVATEKEY" ;
	public static  String KEY_EMAILPWDMAP ="EMAILPWDMAP" ;
	
	public  String PREFS_NAME = "MHISE_PREF";  
	
	
	/* Keystore path and name */
	public static  String defaultP12StorePath  =Environment.getExternalStorageDirectory().toString() +"/MobiusDroid/";
	public static  String propertyFile  ="MobiusDroid.properties";
	public static  String defaultP12StoreName ="MobiusDroidStore.p12";
	

	/*Registration Type */
	public  int REGISTER_PATIENT = 0;
	public  int REGISTER_PROVIDER =1;
	

	/* ProviderType */
	public  String PROVIDER_TYPE = "ProviderType";
	public  int INDIVIDUAL_PROVIDER = 1;
	public  int ORGANIZATIONAL_PROVIDER = 0;

	/* Default values for CSR generation */
	public  String organizationalunit="Mobius";
    public  String organizationname="First Genesis";
    /* Default values for Provider registration */
	public  String Country = "US";
	public  String Status = "Active";
	public  String Language = "English";

	public  int LanguageID = 1;
	public  String TagSpeciality = "Specialty";
	public  String TagOrganizationType = "OrganizationType";
	public  String TagIndividualType = "Provider_IndividualType";

	/* Constants for requests */
	public  String PR_AddressStatus = "Primary";
	public  String OrgProv_isIndividualProvider = "false";
	public  String IndProv_isIndividualProvider = "true";
	public  String IndProv_status = "Active";
	public  String Default_CommunityID = "2.16.840.1.113883.3.1605";
	public  String DEFAULT_PURPOSE ="TREATMENT";
	public  String LOG_FILE_NAME ="MobiusDroid_Log";
	/* Default values for CSR generation */
	
	/*Url for Upgrade*/
	//public String updateURL = "http://10.0.30.88:94/";
	public String updateURL = "http://71.28.159.148:1063/";

	/* URL for Production server */
	public  String REFER_URL = "http://71.28.159.148:1061/REFERPATIENT";
	public  String URL = "http://71.28.159.148:1060/MobiusHISE.svc?wsdl";	
	public  String HTTPS_URL_SVC = "https://71.28.159.148/MobiusHISE.svc";	
	public  String HTTPS_URL ="https://71.28.159.148/MobiusHISE.svc?wsdl";
	
//	 http://71.28.159.148:1061/REFERPATIENT?DocumentID=2fd2a65c-a7a1-4679-aec7-c53bdd17c032&Serial=1363B30A00000000014A
	
	/*	
	//URL for Testing server 
 	public  String REFER_URL = "http://10.0.30.88:90/REFERPATIENT";
	public  String URL = "http://10.0.30.88:100/MobiusHISE.svc?wsdl";
	public  String HTTPS_URL_SVC = "https://10.0.30.88/MobiusHISE.svc";	
	public  String HTTPS_URL ="https://10.0.30.88/MobiusHISE.svc?wsdl";*/
		

	public  String SOAP_ACTION_FOR_GET_MASTER_DATA = "MobiusHISEService/IMobius/GetMasterData";	
	public  String SOAP_ACTION_FOR_ADD_PATIENT = "urn:MHISE/IMobius/AddPatient";

	/* Dialod ID */
	public  int PROGRESS_DIALOG_MAXTIME = 10000;
	public  int DATE_DIALOG_ID = 0;
	public  int COMMUNITY_DIALOG_ID = 1;
	public  int GET_COMMUNITY_PROGRESS_DIALOG = 2;
	public  int GET_PATIENT_PROGRESS_DIALOG = 3;
	public  int GET_DOCUMENT_DIALOG = 4;
	public  int GET_PURPOSE_PROGRESS_DIALOG = 5;
	public  int GET_DOCUMENT_PROGRESS_DIALOG = 6;
	public  int GET_SPECIALITY_PROGRESS_DIALOG = 7;
	public  int SPECIALITY_DIALOG_ID = 8;
	public  int GET_LOCALITY_PROGRESS_DIALOG = 9;
	public  int GET_TYPE_PROGRESS_DIALOG = 10;
	public  int GET_CERTIFICATE_PROGRESS_DIALOG = 11;
	public  int REGISTER_PATIENT_DIALOGUE = 12;
	public  int REGISTER_PROVIDER_DIALOGUE = 13;
	public  int SELECT_CERTIFICATE = 14;
	public  int PRIVATE_KEY_MISSING = 15;
	public  int PASSWORD_DIALOG = 16;
	public  int PASSWORD_DIALOG_PFX = 17;
	public  int GET_DOCUMENT_DETAILS_PROGRESS_DIALOG = 18;
	public  int GENERATE_CSR_PROGRESS_DIALOG = 19;
	public  int GENERATE_CSR_PROGRESS_DIALOG1 = 21;
	public  int INVALID_CERTIFICATE_DIALOG = 20;
	public  int LOGIN_PROGRESS_DIALOG=22;
	public  int FORGOTPWD_PROGRESS_DIALOG=23;
	public  int CERTIFICATE_MISMATCH_DIALOG=24;
	public  int UPGRADE_DIALOG=25;
	public  int FETCHING_CERTIFICATE_DIALOG=26;
	public  int ACTIVATING_USER_DIALOG=27;
	public  int Deceased_DATE_ID = 28;
	
	public int UNSPECIFIED =0;
	public int PATIENT=1;
	public int PROVIDER=2;
	/* Home Community detail */
	public static String HomeCommunityName="Home Community";


	/*	User Login*/
	public static String AuthenticateUserResult ="AuthenticateUserResult";
	public static String CertificateSerialNumber="b:CertificateSerialNumber";
	public static String EmailAddress ="b:EmailAddress ";
	public static String Name ="b:Name";
	public static String ActivateUserResult ="ActivateUserResult";

	/* Forgot Password*/
	public static String ForgotPasswordResult  ="ForgotPasswordResult";
	
	/* Change Password*/
	public static String ChangePasswordResult  ="ChangePasswordResult";
	
	/* Tag For Search Patient Result */
	public  String TAG_SEARCH_PATIENT_RESULT = "SearchPatientResult";
	public  String TAG_PATIENTS = "b:Patients";
	public  String TAG_DEMOGRAPHICS = "b:Demographics";
	
	/* Tag For NHINCommunity  Result */
	
	public  String TAG_COMMUNITY_RESULT = "GetNhinCommunityResult";
	public  String TAG_PHI_SOURCE_RESULT = "GetPHISourceResult"; 
	public  String TAG_COMMUNITIES= "b:Communities";
	public  String TAG_PatientIdentifiers= "b:PatientIdentifiers";
	public  String TAG_NHINCOMMUNITY= "c:NHINCommunity";
	public  String TAG_COMMUNITY_DESCRIPTION= "c:CommunityDescription";
	public  String TAG_bCOMMUNITY_DESCRIPTION= "b:CommunityDescription";
	public  String TAG_PHI_DESCRIPTION= "CommunityDescription";
	public  String TAG_COMMUNITY_IDENTIFIER= "c:CommunityIdentifier";
	public  String TAG_bCOMMUNITY_IDENTIFIER= "b:CommunityIdentifier";
	public  String TAG_PHI_IDENTIFIER= "CommunityIdentifier";
	
	public  String TAG_COMMUNITY_NAME= "c:CommunityName";
	public  String TAG_bCOMMUNITY_NAME= "b:CommunityName";
	public  String TAG_PHI_NAME= "CommunityName";
	public  String TAG_IS_HOME_COMMUNITY = "c:IsHomeCommunity";
	public  String TAG_bIS_HOME_COMMUNITY = "b:IsHomeCommunity";
	public  String TAG_IS_HOME_PHI = "IsHomeCommunity";
	
	/*Key Constants for Intent */
	public  String KEY_USER_ID= "PATIENT_ID";
	public  String KEY_ROLE= "ROLE";
	public  String KEY_USER_TYPE= "USER_TYPE";
	public  String KEY_DOCUMENT_ID= "DOCUMENT_ID";
	public  String KEY_PURPOSE= "PURPOSE";
	public  String KEY_NAME= "NAME";
	public  String KEY_USER_EMAIL= "EMAIL";
	public  String HOME_COMMUNITY_ID = "HomeCommunityID";
	public  String KEY_SERIAL_NUMBER ="SERIAL_NUMBER" ;
	
	/*GetUserInformationResult*/
	public  String TAG_GET_USER_INFORMATION_RESULT= "GetUserInformationResult";
	public  String TAG_GET_USER_INFORMATION="b:UserInformation";
	public  String TAG_CommunityId="c:CommunityId";
	public  String TAG_EmailAddress="c:EmailAddress";
	public  String TAG_bEmailAddress="b:EmailAddress";
	public  String bTAG_FamilyName="b:FamilyName";
	public  String bTAG_GivenName="b:GivenName";
	public  String TAG_OrganizationName="b:OrganizationName";
	public  String TAG_IsIndividualProvider="b:IsIndividualProvider";
	public  String TAG_ID="c:Id";
	public  String TAG_IsOptIn="c:IsOptIn";
	public  String TAG_MPIID="c:MPIID";
	public  String TAG_bMPIID="b:MPIID";
	public  String TAG_Name="c:Name";
	public  String TAG_bName="b:Name";
	public  String TAG_PublicKey="c:PublicKey";
	public  String TAG_Role="c:Role";
	public  String TAG_bRole="b:Role";
	public  String TAG_UserType="c:UserType";
	public  String TAG_bUserType="b:UserType";
    

	/* Tag For Result Class */
	public  String TAG_RESULT = "b:Result";
	public  String TAG_ERROR_CODE = "c:_errorCode";
	public  String TAG_bERROR_CODE = "b:_errorCode";
	public  String TAG_ERROR_MESSAGE = "c:_errorMessage";
	public  String TAG_bERROR_MESSAGE = "b:_errorMessage";
	public  String TAG_IS_SUCCESS = "c:_IsSuccess";
	public  String TAG_bIS_SUCCESS = "b:_IsSuccess";

	/* Tag For Patient Class */
	public  String TAG_ADD_PATIENT_RESULT = "AddPatientResult";
	public  String strPATIENT = "Patient";
	public  String strPROVIDER = "Provider";
	public  String TAG_ADD_PROVIDER_RESULT = "AddProviderResult";
	public  String TAG_PATIENT = "b:Patients";
	public  String TAG_ADDRESS = "b:Address";
	public  String TAG_PATIENT_ADDRESS = "b:PatientAddress";
	public  String TAG_PATIENT_ID = "b:LocalMPIID";
	public  String TAG_DOB = "b:DOB";
	public  String TAG_GENDER = "b:Gender";
	public  String TAG_FIRSTNAME = "b:GivenName";
	public  String TAG_LASTNAME = "b:FamilyName";
	public  String TAG_COMMUNITY = "b:CommunityId";
	public String TAG_PKCS7Response ="b:PKCS7Response";
	public String TAG_GetPFXCertificateResult ="GetPFXCertificateResult";
	
	/* Tag For CSR Detail Class */
	public String TAG_GetCSRDetailsResult ="GetCSRDetailsResult";
	
	public String TAG_ ="b:PFXCertificate";
	public String TAG_PFXCertificate ="b:PFXCertificate";
	
	/* Address tag */
	public  String TAG_AddressLine1 = "b:AddressLine1";
	public  String TAG_AddressLine2 = "b:AddressLine2";
	public  String TAG_AddressStatus = "b:AddressStatus";
	public  String TAG_CITY = "b:City";
	public  String TAG_STATE = "b:State";
	public  String TAG_COUNTRY = "b:Country";
	public  String TAG_ZIP = "b:Zip";
	public  String TAG_CITYNAME = "b:CityName";
	public  String TAG_STATENAME = "b:StateName";

	/* GetDocumentDetails Tag */
	public  String TAG_GET_DOCUMENT_METADATRESULT = "GetDocumentMetadataResult";
	public  String TAG_DOCUMENTS = "b:Documents";
	public  String TAG_DOCUMENT = "c:Document";
	public  String TAG_AUTHOR = "c:Author";
	public  String TAG_bAUTHOR = "b:Author";
	public  String TAG_CREATED_ON = "c:CreatedOn";
	public  String TAG_bCREATED_ON = "b:CreatedOn";
	public  String TAG_DATASOURCE = "c:DataSource";
	public  String TAG_bDATASOURCE = "b:DataSource";
	public  String TAG_DOCUMENT_TITLE = "c:DocumentTitle";
	public  String TAG_bDOCUMENT_TITLE = "b:DocumentTitle";
	public  String TAG_DOCUMENT_ID = "c:DocumentUniqueId";
	public  String TAG_bDOCUMENT_ID = "b:DocumentUniqueId";
	
	/* Get Application version tags */
	public  String TAG_GET_APPLICATION_VERSION_RESULT = "GetApplicationVersionResult";

	/* Get Master Data Result tags */
	public  String TAG_GET_MASTER_DATA_RESULT = "GetMasterDataResult";
	public  String TAG_GET_MASTER_DATA_COLLECTION = "b:MasterDataCollection";
	public  String TAG_MASTER_DATA = "b:MasterData";
	public  String TAG_CODE = "b:Code";
	public  String TAG_DESCRIPTION = "b:Description";

	/* Get Document result tags */
	public  String TAG_GET_DOCUMENT_RESULT = "GetDocumentResult";
	public  String TAG_GET_DOCUMENT_BYTES = "c:DocumentBytes";//"c:_ByteData";
	public  String TAG_bGET_DOCUMENT_BYTES = "b:DocumentBytes";//"c:_ByteData";
	public  String TAG_GET_DOCUMENT = "b:Document";

	/* Get Locality by ZIP code tag */
	public  String TAG_GET_LOCALITY_BY_ZIP_CODE = "GetLocalityByZipCodeResult";
	public  String TAG_LOCALITY_CITYNAME = "b:CityName";
	public  String TAG_LOCALITY_STATE = "b:State";
	public  String TAG_LOCALITY_CITY = "b:City";
	public  String TAG_LOCALITY_COUNTRY = "b:Country";
	public  String TAG_LOCALITY_COUNTRYNAME = "b:CountryName";
	public  String TAG_LOCALITY_STATENAME = "b:StateName";
	public  String TAG_AddPFXCertificateResult = "AddPFXCertificateResult";

	/* String separator */
	public  String STR_SEPARATOR_SPECIALITIES = "@";
	public  String STR_SEPARATOR_USERNAME = "   ";
	
	/* Assertion Mode */
	public String ASSERTION_MODE_DEFAULT = "Default";
	public String ASSERTION_MODE_CUSTOM = "Custom";
}
