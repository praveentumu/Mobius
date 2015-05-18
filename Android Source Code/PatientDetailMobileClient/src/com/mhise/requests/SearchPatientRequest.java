package com.mhise.requests;

import java.util.HashMap;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

import android.util.Log;

import com.mhise.model.Assertion;
import com.mhise.model.Demographics;
import com.mhise.xml.XMLHolder;
import com.mhise.xml.XMLRequest;
import com.mhise.xml.XmlConstants;

/** 
*@(#) SearchPatientRequest.java 
* @author R Systems
* @description This class contains the methods to create request for Search Patient
* @since 2012-10-26
* @version 1.0 
*/
public class SearchPatientRequest {
	
	
	public String makeSearchPatientRequest1(Assertion assertion,Demographics ds ,String[] _communities,HashMap<String, String> hmp_Community)
	{
		
		XMLRequest xmlRequest = new XMLRequest();
    	Document doc =xmlRequest.InitializeDoc();

    	XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
		
		try{
			Element rootelement=holder.el;
		
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_FOR_SEARCH_PATIENT);	
		
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			
			Element bodyElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, bodyElement , XmlConstants.NS_URN+":SearchPatient");
			
			Element searchPatientElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientElement , XmlConstants.NS_URN+":searchPatientRequest");
			
			
			Element searchPatientRequestElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientRequestElement , XmlConstants.NS_MOB+":Assertion");
			
			
			Element assertionElement = holder.el;
			/*holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":Address");
			Element assertionAddressElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":AddressLine1");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":AddressLine2");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":AddressStatus");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":City");
			Element assertionAddressCityElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityElement , XmlConstants.NS_MOB+":CityName");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityElement , XmlConstants.NS_MOB+":State");
			Element assertionAddressCityStateElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityStateElement , XmlConstants.NS_MOB+":Country");
			Element assertionAddressCityStateCountryElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityStateCountryElement , XmlConstants.NS_MOB+":CountryName");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityStateElement , XmlConstants.NS_MOB+":StateName");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":Id");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":Zip");*/
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":AssertionMode",assertion.getAssertionMode());
			//holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":ExplanationNonClaimantSignature");
			
			
			/*holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":HomeCommunityId");
			Element assertionHomeCommunityElement = holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":CommunityDescription",assertion.getNhinCommunity().getCommunityDescription());
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":CommunityIdentifier",assertion.getNhinCommunity().getCommunityIdentifier());
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":CommunityName",assertion.getNhinCommunity().getCommunityName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":IsHomeCommunity");
			
			Log.e("assertion start","assertion person start"+assertion.getUserInformation());
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":MessageId");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":PatientId");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":PersonName");
			
			Element assertionNameElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":FamilyName",assertion.getUserInformation().getPersonName().getFamilyName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":GivenName",assertion.getUserInformation().getPersonName().getGivenName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":MiddleName");
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":Prefix");
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":Suffix");*/
			
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":PurposeOfUse",assertion.getPurposeOfUse());
			/*holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":RelatesToList");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":SSN");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":SamlAuthnStatement");
			Log.e("assertion start","assertion SamlAuthnStatement start");
			
			Element assertionSamlAuthnStatement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":AuthContextClassRef");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":AuthInstant");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":SessionIndex");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":SubjectLocalityAddress");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":SubjectLocalityDNSName");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":SamlSignature");
			Element assertionSamlSignature = holder.el;
			
			Log.e("assertion start","assertion SamlSignature start");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignature , XmlConstants.NS_MOB+":KeyInfo");
			Element assertionSamlSignatureKeyInfo = holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignatureKeyInfo , XmlConstants.NS_MOB+":RSAKeyValueExponent");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignatureKeyInfo , XmlConstants.NS_MOB+":RSAKeyValueModulus");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignature , XmlConstants.NS_MOB+":signatureValue");*/
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":UserInformation");
			Element assertionUserInformation = holder.el;
			
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":DateOfBirth");
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":HomeCommunity");
			
			Element assertionUserInformationCommunity = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityDescription",assertion.getNhinCommunity().getCommunityDescription());
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityIdentifier",assertion.getNhinCommunity().getCommunityIdentifier());
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityName",assertion.getNhinCommunity().getCommunityName());
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":IsHomeCommunity");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":Name");
			
			Log.e("assertion start","assertion userinformation start");
			Element assertionUserInformationName = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":FamilyName",assertion.getUserInformation().getPersonName().getFamilyName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":GivenName",assertion.getUserInformation().getPersonName().getGivenName());
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":MiddleName");
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":Prefix");
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":Suffix");
			String userRole = assertion.getUserInformation().getRole().trim().replace(" ", "_");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":Role",userRole);
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":UserName");
			
			Log.e("assertion start","assertion Demographics start");
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientRequestElement , XmlConstants.NS_MOB+":Demographics");
			
			Element demographicsElement = holder.el;
		
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":FamilyName" , ds.getFamilyName());
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":GivenName" , ds.getGivenName());
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":DOB" , ds.getDOB());
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":Gender" , ds.getGender());
			
			
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientRequestElement , XmlConstants.NS_MOB+":NHINCommunities");
			
			Element nhinCommunityElement = holder.el;
			
			for(int i=0;i<_communities.length;i++ )
			 {
				holder=xmlRequest.addElementToParent(holder.doc, nhinCommunityElement , XmlConstants.NS_MOB+":Community");
				Element communityElement = holder.el;
				
				if(hmp_Community.containsKey(_communities[i]))
				{
					holder=xmlRequest.addElementToParent(holder.doc, communityElement , XmlConstants.NS_MOB+":CommunityIdentifier" ,hmp_Community.get(_communities[i]));
				}
				/*if (_communities[i].equalsIgnoreCase(Constants.HomeCommunityName))
				{
					holder=xmlRequest.addElementToParent(holder.doc, communityElement , XmlConstants.NS_MOB+":CommunityIdentifier" ,MobiusDroid.HomeCommunityID);
				}*/
				//else
				//	holder=xmlRequest.addElementToParent(holder.doc, communityElement , XmlConstants.NS_MOB+":CommunityIdentifier" ,_communities[i]);	
				
			 }
			Log.e("xmlRequest",""+xmlRequest.getStringFromDocument(holder.doc));
		}catch(Exception ex){
			Log.e("exception in create search request",""+ex.getMessage());
		}
		return xmlRequest.getStringFromDocument(holder.doc);
	}

	public String makeSearchPatientRequest(Assertion assertion,Demographics ds ,String[] _communities,HashMap<String, String> hmp_Community)
	{
		
		XMLRequest xmlRequest = new XMLRequest();
    	Document doc =xmlRequest.InitializeDoc();

    	XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_ARR, XmlConstants.VALUE_ARR);
		
		try{
			Element rootelement=holder.el;
		
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_FOR_SEARCH_PATIENT);	
		
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			
			Element bodyElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, bodyElement , XmlConstants.NS_URN+":SearchPatient");
			
			Element searchPatientElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientElement , XmlConstants.NS_URN+":searchPatientRequest");
			
			
			Element searchPatientRequestElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientRequestElement , XmlConstants.NS_MOB+":Assertion");
			
			
			Element assertionElement = holder.el;
			/*holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":Address");
			Element assertionAddressElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":AddressLine1");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":AddressLine2");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":AddressStatus");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":City");
			Element assertionAddressCityElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityElement , XmlConstants.NS_MOB+":CityName");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityElement , XmlConstants.NS_MOB+":State");
			Element assertionAddressCityStateElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityStateElement , XmlConstants.NS_MOB+":Country");
			Element assertionAddressCityStateCountryElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityStateCountryElement , XmlConstants.NS_MOB+":CountryName");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressCityStateElement , XmlConstants.NS_MOB+":StateName");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":Id");
			holder=xmlRequest.addElementToParent(holder.doc, assertionAddressElement , XmlConstants.NS_MOB+":Zip");*/
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":AssertionMode",assertion.getAssertionMode());
			//holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":ExplanationNonClaimantSignature");
			
			
			/*holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":HomeCommunityId");
			Element assertionHomeCommunityElement = holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":CommunityDescription",assertion.getNhinCommunity().getCommunityDescription());
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":CommunityIdentifier",assertion.getNhinCommunity().getCommunityIdentifier());
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":CommunityName",assertion.getNhinCommunity().getCommunityName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionHomeCommunityElement , XmlConstants.NS_MOB+":IsHomeCommunity");
			
			Log.e("assertion start","assertion person start"+assertion.getUserInformation());
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":MessageId");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":PatientId");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":PersonName");
			
			Element assertionNameElement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":FamilyName",assertion.getUserInformation().getPersonName().getFamilyName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":GivenName",assertion.getUserInformation().getPersonName().getGivenName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":MiddleName");
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":Prefix");
			holder=xmlRequest.addElementToParent(holder.doc, assertionNameElement , XmlConstants.NS_MOB+":Suffix");*/
			
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":PurposeOfUse",assertion.getPurposeOfUse());
			/*holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":RelatesToList");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":SSN");
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":SamlAuthnStatement");
			Log.e("assertion start","assertion SamlAuthnStatement start");
			
			Element assertionSamlAuthnStatement = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":AuthContextClassRef");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":AuthInstant");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":SessionIndex");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":SubjectLocalityAddress");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlAuthnStatement , XmlConstants.NS_MOB+":SubjectLocalityDNSName");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":SamlSignature");
			Element assertionSamlSignature = holder.el;
			
			Log.e("assertion start","assertion SamlSignature start");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignature , XmlConstants.NS_MOB+":KeyInfo");
			Element assertionSamlSignatureKeyInfo = holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignatureKeyInfo , XmlConstants.NS_MOB+":RSAKeyValueExponent");
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignatureKeyInfo , XmlConstants.NS_MOB+":RSAKeyValueModulus");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionSamlSignature , XmlConstants.NS_MOB+":signatureValue");*/
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":UserInformation");
			Element assertionUserInformation = holder.el;
			
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":DateOfBirth");
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":HomeCommunity");
			
			Element assertionUserInformationCommunity = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityDescription",assertion.getNhinCommunity().getCommunityDescription());
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityIdentifier",assertion.getNhinCommunity().getCommunityIdentifier());
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityName",assertion.getNhinCommunity().getCommunityName());
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":IsHomeCommunity");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":Name");
			
			Log.e("assertion start","assertion userinformation start");
			Element assertionUserInformationName = holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":FamilyName",assertion.getUserInformation().getPersonName().getFamilyName());
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":GivenName",assertion.getUserInformation().getPersonName().getGivenName());
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":MiddleName");
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":Prefix");
			//holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":Suffix");
			String userRole = assertion.getUserInformation().getRole().trim().replace(" ", "_");
			
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":Role",userRole);
			holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":UserName");
			
			Log.e("assertion start","assertion Demographics start");
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientRequestElement , XmlConstants.NS_MOB+":Demographics");
			
			Element demographicsElement = holder.el;
		
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":FamilyName" , ds.getFamilyName());
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":GivenName" , ds.getGivenName());
			if(ds.isAdvanceSearch()){
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":MiddleName" , ds.getMiddleName());
			
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":Prefix" , ds.getPrefix());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":Suffix" , ds.getSuffix());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":BirthPlaceCity" , ds.getBirthPlaceCity());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":BirthPlaceCountry" , ds.getBirthPlaceCountry());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":BirthPlaceState" , ds.getBirthPlaceState());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":BirthPlaceStreet" , ds.getBirthPlaceStreet());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":BirthPlaceZip" , ds.getBirthPlaceZip());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":City");
				Element city = holder.el;
				if(!"".equals(ds.getCity())){
					String[] arrcity = ds.getCity().split("\n");
					for(int i=0;i<arrcity.length;i++){
						holder=xmlRequest.addElementToParent(holder.doc, city , XmlConstants.NS_ARR+":string" , arrcity[i]);
					}
				}
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":ContractNumbers");
				
				Element telephones = holder.el;
				if(!"".equals(ds.getTelephone())){
					String[] telenums = ds.getTelephone().split("\n");
					for(int i=0;i<telenums.length;i++){
						holder=xmlRequest.addElementToParent(holder.doc, telephones , XmlConstants.NS_ARR+":string" , telenums[i]);
					}
				}
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":Country");
				
				Element country = holder.el;
				if(!"".equals(ds.getCountry())){
					String[] arrCountry = ds.getCountry().split("\n");
					for(int i=0;i<arrCountry.length;i++){
						holder=xmlRequest.addElementToParent(holder.doc, country , XmlConstants.NS_ARR+":string" , arrCountry[i]);
					}
				}
			}
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":DOB" , ds.getDOB());
			if(ds.isAdvanceSearch()){
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":DeceasedDate" , ds.getDeceasedDate());
			}
			holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":Gender" , ds.getGender());
			
			if(ds.isAdvanceSearch()){
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":MothersMaidenName");
				
				Element mothersmaiden = holder.el;
				holder=xmlRequest.addElementToParent(holder.doc, mothersmaiden , XmlConstants.NS_MOB+":FamilyName" , ds.getMotherMaiden().getFamilyName());
				holder=xmlRequest.addElementToParent(holder.doc, mothersmaiden , XmlConstants.NS_MOB+":GivenName" , ds.getMotherMaiden().getGivenName());
				holder=xmlRequest.addElementToParent(holder.doc, mothersmaiden , XmlConstants.NS_MOB+":MiddleName" , ds.getMotherMaiden().getMiddleName());
				holder=xmlRequest.addElementToParent(holder.doc, mothersmaiden , XmlConstants.NS_MOB+":Prefix" , ds.getMotherMaiden().getPrefix());
				holder=xmlRequest.addElementToParent(holder.doc, mothersmaiden , XmlConstants.NS_MOB+":Suffix" , ds.getMotherMaiden().getSuffix());
				
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":SSN",ds.getSSN());
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":State");
				
				Element state = holder.el;
				if(!"".equals(ds.getState())){
					String[] arrState = ds.getState().split("\n");
					for(int i=0;i<arrState.length;i++){
						holder=xmlRequest.addElementToParent(holder.doc, state , XmlConstants.NS_ARR+":string" , arrState[i]);
					}
				}
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":Street");
				
				Element street = holder.el;
				if(!"".equals(ds.getStreet())){
					String[] arrAddress = ds.getStreet().split("\n");
					for(int i=0;i<arrAddress.length;i++){
						holder=xmlRequest.addElementToParent(holder.doc, street , XmlConstants.NS_ARR+":string" , arrAddress[i].trim());
					}
				}
				
				holder=xmlRequest.addElementToParent(holder.doc, demographicsElement , XmlConstants.NS_MOB+":Zip");
				Element zip = holder.el;
				if(!"".equals(ds.getZip())){
					String[] arrZip = ds.getZip().split("\n");
					for(int i=0;i<arrZip.length;i++){
						holder=xmlRequest.addElementToParent(holder.doc, zip , XmlConstants.NS_ARR+":string" , arrZip[i]);
					}
				}
			}
			holder=xmlRequest.addElementToParent(holder.doc, searchPatientRequestElement , XmlConstants.NS_MOB+":NHINCommunities");
			
			Element nhinCommunityElement = holder.el;
			
			for(int i=0;i<_communities.length;i++ )
			 {
				holder=xmlRequest.addElementToParent(holder.doc, nhinCommunityElement , XmlConstants.NS_MOB+":Community");
				Element communityElement = holder.el;
				
				if(hmp_Community.containsKey(_communities[i]))
				{
					holder=xmlRequest.addElementToParent(holder.doc, communityElement , XmlConstants.NS_MOB+":CommunityIdentifier" ,hmp_Community.get(_communities[i]));
				}
				/*if (_communities[i].equalsIgnoreCase(Constants.HomeCommunityName))
				{
					holder=xmlRequest.addElementToParent(holder.doc, communityElement , XmlConstants.NS_MOB+":CommunityIdentifier" ,MobiusDroid.HomeCommunityID);
				}*/
				//else
				//	holder=xmlRequest.addElementToParent(holder.doc, communityElement , XmlConstants.NS_MOB+":CommunityIdentifier" ,_communities[i]);	
				
			 }
			Log.e("xmlRequest",""+xmlRequest.getStringFromDocument(holder.doc));
		}catch(Exception ex){
			Log.e("exception in create search request",""+ex.getMessage());
		}
		return xmlRequest.getStringFromDocument(holder.doc);
	}	
}

