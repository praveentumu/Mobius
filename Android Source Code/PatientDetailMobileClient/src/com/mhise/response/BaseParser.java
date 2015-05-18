package com.mhise.response;

import java.io.UnsupportedEncodingException;
import org.w3c.dom.DOMException;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import android.util.Log;

import com.mhise.constants.Constants;
import com.mhise.model.CSRDetails;
import com.mhise.model.City;
import com.mhise.model.Country;
import com.mhise.model.Document;
import com.mhise.model.DocumentResponse;
import com.mhise.model.PersonName;
import com.mhise.model.RegisterationResult;

import com.mhise.model.Result;
import com.mhise.model.State;
import com.mhise.model.User;
//import com.mhise.util.Logger;
import com.mhise.util.Logger;

/** 
*@(#)BaseParser.java 
* @author R Systems
* @description This class contains the methods to parse MHISE service method's Response
* 
* @since 2012-10-26
* @version 1.0 
*/
public class BaseParser {

	public static void setResult(Node subnode, Result result) {

		////Log.i("Baseparser-->inside setResult method", "true");
		try{
		NodeList  resultnodelist = subnode.getChildNodes();
		
		for(int j=0;j<resultnodelist.getLength();j++){
			
			Node resultElement =resultnodelist.item(j);
			
			if( resultElement.getNodeName().equals(com.mhise.constants.Constants.TAG_ERROR_CODE))
			{			
				Log.e("Baseparser-->Constants.TAG_ERROR_CODE" , ""+resultElement.getTextContent().toString());

				result.ErrorCode =resultElement.getTextContent().toString();
			}	
			else if( resultElement.getNodeName().equals(com.mhise.constants.Constants.TAG_IS_SUCCESS))
			{
				Log.e("Baseparser-->Constants.TAG_IS_SUCCESS" , ""+resultElement.getTextContent().toString());
				result.IsSuccess =resultElement.getTextContent().toString();				
			}
			else if( resultElement.getNodeName().equals(com.mhise.constants.Constants.TAG_ERROR_MESSAGE))
			{
				Log.e("Baseparser-->Constants.TAG_ERROR_MESSAGE" , ""+resultElement.getTextContent().toString());	//	result.ErrorMessage =resultElement.getTextContent().toString();
				result.ErrorMessage =resultElement.getTextContent().toString();
			}	
				
		}
		}
		catch (NullPointerException e) {
			// TODO: handle exception
		}
	}


	public static  RegisterationResult parseCSRResult(org.w3c.dom.Document dom)
	{
		RegisterationResult regResult = new RegisterationResult();;
		Result  result = null;
		NodeList list=null;
	
		 list = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GetPFXCertificateResult);
	
		try{
		NodeList subNodeList =list.item(0).getChildNodes();
	
		////Log.i("Node name","----"+subNodeList.item(0).getNodeName());
		////Log.i("Node name","----"+subNodeList.item(1).getNodeName());
		for(int i=0;i<subNodeList.getLength();i++)
		{
		Node 	nodeResponse = subNodeList.item(i);
		////Log.i("Node name","----"+nodeResponse.getNodeName());
		 if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
			{
			 	
			 	result = new com.mhise.model.Result();
				BaseParser.setResult(nodeResponse, result);
				regResult.setResult(result);
				Log.e("result-->","-->"+nodeResponse.getTextContent());
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_PFXCertificate))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 	regResult.setCertificate(nodeResponse.getTextContent());
			 	Log.e("certificate-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}

		}
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			////Log.i("BaseParser->RegisterationResult", ""+e);
		}
		return regResult;
	}
	
	
	public static  CSRDetails parseCSRDetails(org.w3c.dom.Document dom)
	{
		CSRDetails cSRDetails = new CSRDetails();;
		Result  result = null;
		NodeList list=null;
	
		 list = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GetCSRDetailsResult);
	
		try{
		NodeList subNodeList =list.item(0).getChildNodes();
	
		////Log.i("Node name","----"+subNodeList.item(0).getNodeName());
		////Log.i("Node name","----"+subNodeList.item(1).getNodeName());
		for(int i=0;i<subNodeList.getLength();i++)
		{
		Node 	nodeResponse = subNodeList.item(i);
		////Log.i("Node name","----"+nodeResponse.getNodeName());
		 if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
			{
			 	
			 	result = new com.mhise.model.Result();
				BaseParser.setResult(nodeResponse, result);
				cSRDetails.setResult(result);
				Log.e("result-->","-->"+nodeResponse.getTextContent());
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_CITY))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 cSRDetails.setCity(nodeResponse.getTextContent());
			 	Log.e("City-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_STATE))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 cSRDetails.setState(nodeResponse.getTextContent());
			 	Log.e("state-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_COUNTRY))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 cSRDetails.setCountry(nodeResponse.getTextContent());
			 	Log.e("Country-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_FIRSTNAME))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 cSRDetails.setGivenName(nodeResponse.getTextContent());
			 	Log.e("Given name-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_LASTNAME))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 	cSRDetails.setFamilyName(nodeResponse.getTextContent());
			 	Log.e("Last name-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_bEmailAddress))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 	cSRDetails.setEmailAddress(nodeResponse.getTextContent());
			 	Log.e("setEmailAddress-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_IsIndividualProvider))
			{
			 	//Log.i("Certificate Node", "parsing node");
			 	cSRDetails.setIsIndividualProvider(nodeResponse.getTextContent());
			 	Log.e("Individual Provider-->","222-->"+cSRDetails.getIsIndividualProvider());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_OrganizationName))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 	cSRDetails.setOrgName(nodeResponse.getTextContent());
			 	Log.e("OrganizationName-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}

		}
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			////Log.i("BaseParser->RegisterationResult", ""+e);
		}
		return cSRDetails;
	}
	
	
	
	public static  RegisterationResult parseGetCertificateResult(org.w3c.dom.Document dom)
	{
		RegisterationResult regResult = new RegisterationResult();;
		Result  result = null;
		NodeList list=null;
	
		 list = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GetPFXCertificateResult);
	
		try{
		NodeList subNodeList =list.item(0).getChildNodes();
	
		////Log.i("Node name","----"+subNodeList.item(0).getNodeName());
		////Log.i("Node name","----"+subNodeList.item(1).getNodeName());
		for(int i=0;i<subNodeList.getLength();i++)
		{
		Node 	nodeResponse = subNodeList.item(i);
		////Log.i("Node name","----"+nodeResponse.getNodeName());
		 if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
			{
			 	
			 	result = new com.mhise.model.Result();
				BaseParser.setResult(nodeResponse, result);
				regResult.setResult(result);
				Log.e("result-->","-->"+nodeResponse.getTextContent());
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_PFXCertificate))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 	regResult.setCertificate(nodeResponse.getTextContent());
			 	Log.e("certificate-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}

		}
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			////Log.i("BaseParser->RegisterationResult", ""+e);
		}
		return regResult;
	}
	
	
	
	public static DocumentResponse parseDocument(org.w3c.dom.Document dom)
	{
		DocumentResponse responseOBJ = new DocumentResponse() ;
		Result result;
		try{
		NodeList nl0 = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GET_DOCUMENT_RESULT);
		NodeList nl=   nl0.item(0).getChildNodes();
		    
		for(int i=0;i<nl.getLength();i++)
		{
			Node subnode =nl.item(i);
			////Log.i("Baseparser-->Node name ", ""+subnode.getNodeName());
			if(subnode.getNodeName().equals(Constants.TAG_GET_DOCUMENT))
			{
				NodeList nodelist = subnode.getChildNodes();
				Document documentOBJ = new Document();
				for(int d=0;d<nodelist.getLength();d++)
				{
					Node docNode =nodelist.item(d);
					{
						if(docNode.getNodeName().equals(Constants.TAG_bGET_DOCUMENT_BYTES))
						{
							byte[] arByte =null;
							try {
								////Log.i("Baseparser-->pure text cotent", docNode.getTextContent());
								arByte =docNode.getTextContent().getBytes("UTF-8");
							} catch (UnsupportedEncodingException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
								Log.i("Baseparser-->Node name ", ""+docNode.getNodeName());
							} catch (DOMException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
								Log.i("Baseparser-->Node name ", ""+docNode.getNodeName());
							}
							Log.i("arByte  ", ""+arByte.length);
							documentOBJ.setDocumentBytes(arByte);
						}	
						responseOBJ.document =documentOBJ;
					}
					Log.i("Baseparser-->Node  ", "parsing suuccessfull");
				}
				 
			 }
			 else if (subnode.getNodeName().equals(Constants.TAG_RESULT))
			 {
				    result = new com.mhise.model.Result();
					BaseParser.setResult(subnode, result);
					responseOBJ.result =result;
					////Log.i("Baseparser-->is result set ", "result set true");
			 }
		}
		
		
	}
	catch (NullPointerException e) {
		// TODO: handle exception
	}
		return responseOBJ;
	}

	public static City parseGetLocalityByZipresult(org.w3c.dom.Document dom)
	{
		City city = new City() ;
		Result result;
		NodeList list_getLocalityByZipCode0 = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GET_LOCALITY_BY_ZIP_CODE);
		Node root =list_getLocalityByZipCode0.item(0);
		NodeList list_getLocalityByZipCode =root.getChildNodes();
		for(int index=0;index<list_getLocalityByZipCode.getLength();index++)
		{
			Node nodeResponse =list_getLocalityByZipCode.item(index);
			if(nodeResponse.getNodeName().equals(Constants.TAG_LOCALITY_CITY));
			{
				////Log.i("Baseparser-->parseGetLocalityByZipresult","nodeResponse.getNodeName()->"+nodeResponse.getNodeName());
				NodeList  nlCityChild =nodeResponse.getChildNodes();
				for(int c=0;c<nlCityChild.getLength();c++)
				{
					Node nCityChild =nlCityChild.item(c);
					if(nCityChild.getNodeName().equals(Constants.TAG_LOCALITY_CITYNAME))
					{
						city.setCityName(nCityChild.getTextContent());
						////Log.i("Baseparser-->parseGetLocalityByZipresult","nCityChild.getTextContent()->"+nCityChild.getTextContent());
					}
					else if(nCityChild.getNodeName().equals(Constants.TAG_LOCALITY_STATE))
					{
						State state = new State();
						NodeList nlState =nCityChild.getChildNodes();
						////Log.i("Baseparser-->parseGetLocalityByZipresult","nCityChild.getNodeName()->"+nCityChild.getNodeName());
						for(int s=0;s<nlState.getLength();s++)
						{
							Node nState =nlState.item(s);
							if(nState.getNodeName().equals(Constants.TAG_LOCALITY_COUNTRY))
							{
								NodeList nlCountry =nState.getChildNodes();
								for(int nc=0;nc<nlCountry.getLength();nc++)
								{
									Node nCountry =nlCountry.item(nc);
									if(nCountry.getNodeName().equals(Constants.TAG_LOCALITY_COUNTRYNAME))
									{
										Country country = new Country();
										country.setCountryName(nCountry.getTextContent());
										state.setCountry(country);
									}
								}
							}
							else if(nState.getNodeName().equals(Constants.TAG_LOCALITY_STATENAME) )
							{
								state.setStateName(nState.getTextContent());
							}
						}
						city.setState(state);
					}
				}
				
			}
			 if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
			{
			    result = new com.mhise.model.Result();
				BaseParser.setResult(nodeResponse, result);
				city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		}
		
		
	return city;
	}

	
	public static String parseApplicationVersionResponse(org.w3c.dom.Document dom)
	{try {
		NodeList list = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GET_APPLICATION_VERSION_RESULT);
		Log.i("Node name","getNodeName----"+list.item(0).getNodeName());
		Log.i("Node name","getTextContent----"+list.item(0).getTextContent());
		return list.item(0).getTextContent();
	}
	catch (NullPointerException e) {
		// TODO: handle exception
		Logger.debug("BaseParser ->parseApplicationVersionResponse", ""+e);
		Log.i("BaseParser ->parseApplicationVersionResponse", ""+e);
		return null;
	}
	}
	
	
	
	public static  RegisterationResult parseActivateUserResponse(org.w3c.dom.Document dom)
	{
		RegisterationResult regResult = new RegisterationResult();;
		Result  result = null;
		NodeList list=null;
	
		try{
		 list = dom.getElementsByTagName(com.mhise.constants.Constants.ActivateUserResult);
		NodeList subNodeList =list.item(0).getChildNodes();
	
	Log.i("Node name","----"+subNodeList.item(0).getNodeName());
		////Log.i("Node name","----"+subNodeList.item(1).getNodeName());
		for(int i=0;i<subNodeList.getLength();i++)
		{
		Node 	nodeResponse = subNodeList.item(i);
		////Log.i("Node name","----"+nodeResponse.getNodeName());
		 if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
			{
			 	
			 	result = new com.mhise.model.Result();
				BaseParser.setResult(nodeResponse, result);
				regResult.setResult(result);
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_PKCS7Response))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 	regResult.setCertificate(nodeResponse.getTextContent());
			 	Log.e("certificate-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		}
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			////Log.i("BaseParser->RegisterationResult", ""+e);
		}
		return regResult;
	}
	
	
	public static  RegisterationResult parseAddProviderResult(org.w3c.dom.Document dom, int type)
	{
		RegisterationResult regResult = new RegisterationResult();;
		Result  result = null;
		NodeList list=null;
		if(type ==Constants.PATIENT)
		 list = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_ADD_PATIENT_RESULT);
		if(type == Constants.PROVIDER)
			list = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_ADD_PROVIDER_RESULT);
		try{
		NodeList subNodeList =list.item(0).getChildNodes();
	
		////Log.i("Node name","----"+subNodeList.item(0).getNodeName());
		////Log.i("Node name","----"+subNodeList.item(1).getNodeName());
		for(int i=0;i<subNodeList.getLength();i++)
		{
		Node 	nodeResponse = subNodeList.item(i);
		////Log.i("Node name","----"+nodeResponse.getNodeName());
		 if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
			{
			 	
			 	result = new com.mhise.model.Result();
				BaseParser.setResult(nodeResponse, result);
				regResult.setResult(result);
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_PKCS7Response))
			{
			 	////Log.i("Certificate Node", "parsing node");
			 	regResult.setCertificate(nodeResponse.getTextContent());
			 	Log.e("certificate-->","-->"+nodeResponse.getTextContent());
				
				//city.setResult(result);
				////Log.i("Baseparser-->is result set ", "result set true");
			}
		}
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			////Log.i("BaseParser->RegisterationResult", ""+e);
		}
		return regResult;
	}
		
	public static User parseUserInformationType(org.w3c.dom.Document dom)
	{
		Result  result ;
		User user = new User();
		
		try{
		
		NodeList list = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GET_USER_INFORMATION_RESULT);
		Node nodeResult =list.item(0);
		 
		NodeList userInfoNodeList = nodeResult.getChildNodes();
		for(int k =0; k<userInfoNodeList.getLength();k++)
		{
		Node nodeResponse =userInfoNodeList.item(k);
			
		////Log.i("Baseparser-->Start parsing", "RESULT");
		////Log.i("Baseparser-->Start parsing", "RESULT"+nodeResponse.getNodeName());
		 if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
			{
			 	////Log.i("Baseparser-->RESULT", "RESULT");
			 	result = new com.mhise.model.Result();
				BaseParser.setResult(nodeResponse, result);
				user.setResult(result);
				////Log.i("Baseparser-->is result set ? ", "true");
			}
		
		 else if(nodeResponse.getNodeName().equals(Constants.TAG_GET_USER_INFORMATION))
			{
			 NodeList userinfoNodeList =nodeResponse.getChildNodes();
			 
			 for(int i=0;i<userinfoNodeList.getLength();i++)
			 {
			 Node  userInfo = userinfoNodeList.item(i);
			 
			 if(userInfo.getNodeName().equals(Constants.TAG_CommunityId))
			 {
				 user.setCommunityId(userInfo.getTextContent());
			 }
			 
			 
			 if(userInfo.getNodeName().equals(Constants.TAG_bEmailAddress))
			 {
				 user.setEmail(userInfo.getTextContent());
			 }
			 
			 else if(userInfo.getNodeName().equals(Constants.TAG_ID))
			 {
				 user.setID(userInfo.getTextContent()); 
			 }
			 else if(userInfo.getNodeName().equals(Constants.TAG_IsOptIn))
			 {
				 user.setIsOptIn(userInfo.getTextContent());
			 }
			 else if(userInfo.getNodeName().equals(Constants.TAG_bMPIID))
			 {
				 user.setMPIID(userInfo.getTextContent());
			 }
			 else  if(userInfo.getNodeName().equals(Constants.TAG_bName))
			 { 
				 PersonName name = new PersonName();
				 NodeList nameNodeList = userInfo.getChildNodes();
				 for(int j=0;j<nameNodeList.getLength();j++){
					 Node fullName = nameNodeList.item(j);
					 if(fullName.getNodeName().equals(Constants.bTAG_FamilyName)){
						 name.setFamilyName(fullName.getTextContent());
					 }else if(fullName.getNodeName().equals(Constants.bTAG_GivenName)){
						 name.setGivenName(fullName.getTextContent());
					 }
				 } 
				 user.setPersonName(name);	 
			 }
			 else  if(userInfo.getNodeName().equals(Constants.TAG_PublicKey))
			 {
				 user.setpublicKey(userInfo.getTextContent());
			 }
			 else  if(userInfo.getNodeName().equals(Constants.TAG_bRole))
			 {
				 user.setRole(userInfo.getTextContent());
			 }
			 else  if(userInfo.getNodeName().equals(Constants.TAG_bUserType))
			 {
				 user.setUserType(userInfo.getTextContent());
			 }
			 
			 }
			 
			}
		}
		}
		catch (NullPointerException e) {
			// TODO: handle exception
		}
		return user;
	}
	
}
