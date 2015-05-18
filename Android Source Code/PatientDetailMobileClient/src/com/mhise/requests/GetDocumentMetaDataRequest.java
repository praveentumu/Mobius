package com.mhise.requests;

import java.util.ArrayList;
import java.util.HashMap;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import android.util.Log;

import com.mhise.model.Assertion;
import com.mhise.util.Logger;
import com.mhise.xml.XMLHolder;
import com.mhise.xml.XMLRequest;
import com.mhise.xml.XmlConstants;




/** 
*@(#)GetDocumentMetaDataRequest.java 
* @author R Systems
* @description This class contains the methods to create request for GetDocumentMetaData
* @since 2012-10-26
* @version 1.0 
*/

public class GetDocumentMetaDataRequest {
	
	public String getDocumentDetailsRequest(Assertion assertion, String patientID ,ArrayList<String> _communities ,HashMap<String,String> hmp_Community,boolean locallyAvailableDocuments)
	{
		XMLRequest xmlRequest = new XMLRequest();
    	Document doc =xmlRequest.InitializeDoc();
    	
		XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
		
		Element rootElement=holder.el;		
		holder=xmlRequest.addElementToParent(holder.doc, rootElement , XmlConstants.NS_SOAP+":Header" , XmlConstants.XMLNS + XmlConstants.NS_WSA , XmlConstants.VALUE_WSA);
		
		Element headerElement = holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action" , XmlConstants.ACTION_FOR_GET_DOCUMENT_METADATA);
		
		holder=xmlRequest.addElementToParent(holder.doc, rootElement , XmlConstants.NS_SOAP+":Body");
		
		Element bodyElement = holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, bodyElement , XmlConstants.NS_URN+":GetDocumentMetadata");
		
		Element getDocumentMetadataElement= holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, getDocumentMetadataElement , XmlConstants.NS_URN+":getDocumentMetadataRequest");
		
		Element getDocumentMetadataRequestElement = holder.el;
		
		
		holder=xmlRequest.addElementToParent(holder.doc, getDocumentMetadataRequestElement , XmlConstants.NS_MOB+":Assertion");
		
		Element assertionElement = holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":AssertionMode",assertion.getAssertionMode());
		holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":PurposeOfUse",assertion.getPurposeOfUse());
		holder=xmlRequest.addElementToParent(holder.doc, assertionElement , XmlConstants.NS_MOB+":UserInformation");
		Element assertionUserInformation = holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":HomeCommunity");
		Element assertionUserInformationCommunity = holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityDescription",assertion.getNhinCommunity().getCommunityDescription());
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityIdentifier",assertion.getNhinCommunity().getCommunityIdentifier());
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationCommunity , XmlConstants.NS_MOB+":CommunityName",assertion.getNhinCommunity().getCommunityName());
		
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":Name");
		
		Log.e("assertion start","assertion userinformation start");
		Element assertionUserInformationName = holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":FamilyName",assertion.getUserInformation().getPersonName().getFamilyName());
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformationName , XmlConstants.NS_MOB+":GivenName",assertion.getUserInformation().getPersonName().getGivenName());
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":Role",assertion.getUserInformation().getRole().trim().replace(" ", "_"));
		holder=xmlRequest.addElementToParent(holder.doc, assertionUserInformation , XmlConstants.NS_MOB+":UserName");
		
		if(locallyAvailableDocuments){
			holder=xmlRequest.addElementToParent(holder.doc, getDocumentMetadataRequestElement , XmlConstants.NS_MOB+":GetLocallyAvailable","1");
		}else holder=xmlRequest.addElementToParent(holder.doc, getDocumentMetadataRequestElement , XmlConstants.NS_MOB+":GetLocallyAvailable","0");
		
		holder=xmlRequest.addElementToParent(holder.doc, getDocumentMetadataRequestElement , XmlConstants.NS_MOB+":communities");
		
		Element communityElement = holder.el;
		
		if (_communities !=null){	 
			 for(int i=0;i<_communities.size();i++ )
			 {
				 Log.i("setCommunities", "_communities.size()"+_communities.size());
				 
				 holder=xmlRequest.addElementToParent(holder.doc, communityElement , XmlConstants.NS_MOB+":Community");
				 
				 Element communitySubElement = holder.el;
		 
				 try{
					if(hmp_Community.containsKey(_communities.get(i).toString()))
					{
						holder=xmlRequest.addElementToParent(holder.doc, communitySubElement , XmlConstants.NS_MOB+":CommunityIdentifier" ,hmp_Community.get(_communities.get(i)));
					}
				 }
				 catch (NullPointerException e) {
					// TODO: handle exception
					 Logger.debug("GetDocumentMetaDataRequest", ""+e);
				 }
				 
				
			 }

		}

		holder=xmlRequest.addElementToParent(holder.doc, getDocumentMetadataRequestElement , XmlConstants.NS_MOB+":patientId" , patientID);
		
		Log.e("document xml",""+xmlRequest.getStringFromDocument(holder.doc));
		return xmlRequest.getStringFromDocument(holder.doc);		  
	}


 
		
}
	
