package com.mhise.requests;



import java.util.ArrayList;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

import android.util.Log;

import com.mhise.constants.Constants;
import com.mhise.model.Address;
import com.mhise.model.Assertion;
import com.mhise.model.City;
import com.mhise.model.Demographics;
import com.mhise.model.Patient;
import com.mhise.model.ProviderRegistration;
import com.mhise.model.Telephone;
import com.mhise.xml.XMLHolder;
import com.mhise.xml.XMLRequest;
import com.mhise.xml.XmlConstants;



/** 
*@(#)RequestBase.java 
* @author R Systems
* @description This class contains the methods to create requests  for MHISE service methods except Search patient 
* and GetDocument Meta Data response
* 
* @since 2012-10-26
* @version 1.0 
*/
public class RequestBase {

	
	public static String loginRequest(String email,String password,String userType )
	{
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		    
		 XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_LOGIN);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			//     <urn:authenticateUserRequest>
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":AuthenticateUser");
			Element getMasterDataElement =holder.el;	
		
			holder =xmlRequest.addElementToParent(holder.doc,getMasterDataElement ,XmlConstants.NS_URN+":authenticateUserRequest");
			Element authenticateUserRequest =holder.el;	
		
			
			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":EmailAddress",email);
			
			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":Password",password);

			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":UserType",userType);
		
			Log.i("login request", ""+xmlRequest.getStringFromDocument(holder.doc));
	
		
		       return   xmlRequest.getStringFromDocument(holder.doc);
		
	}
	
	        
	
	public static String getActivateUserRequest(String email,String userType,String CSR )
	{
			/*	
			 * 
			action="urn:MHISE/IMobius/ActivateUser"
			 */
			XMLRequest xmlRequest = new XMLRequest();
			Document	doc =xmlRequest.InitializeDoc();
		    
			XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_ACTIVATE_USER);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
		
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":ActivateUser");
			Element getMasterDataElement =holder.el;	
		
			holder =xmlRequest.addElementToParent(holder.doc,getMasterDataElement ,XmlConstants.NS_URN+":activateUserRequest");
			Element authenticateUserRequest =holder.el;	
		
			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":CSR",CSR);
			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":EmailAddress",email);

			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":UserType",userType);
		
			Log.i("Get User certificate  request", ""+xmlRequest.getStringFromDocument(holder.doc));
	
		
		       return   xmlRequest.getStringFromDocument(holder.doc);
		
	}
	
	
	public static String getCSRDetailRequest(String email,String userType )
	{
			/*
			 * <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:urn="urn:MHISE" xmlns:mob="http://schemas.datacontract.org/2004/07/MobiusServiceLibrary">
				   <soap:Header/>
				   <soap:Body>
				      <urn:GetCSRDetails>
				         <!--Optional:-->
				         <urn:getCSRRequest>
				            <!--Optional:-->
				            <mob:EmailAddress>?</mob:EmailAddress>
				            <!--Optional:-->
				            <mob:UserType>?</mob:UserType>
				         </urn:getCSRRequest>
				      </urn:GetCSRDetails>
				   </soap:Body>
				</soap:Envelope>
				Content-Type: application/soap+xml;charset=UTF-8;
				action="urn:MHISE/IMobius/GetCSRDetails"
			 */
			XMLRequest xmlRequest = new XMLRequest();
			Document	doc =xmlRequest.InitializeDoc();
		    
			XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GET_CSR_DETAILS);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			//     <urn:authenticateUserRequest>
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":GetCSRDetails");
			Element getMasterDataElement =holder.el;	
			holder =xmlRequest.addElementToParent(holder.doc,getMasterDataElement ,XmlConstants.NS_URN+":getCSRRequest");
			Element authenticateUserRequest =holder.el;	
			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":EmailAddress",email);
			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":UserType",userType);
			Log.i("Get User certificate  request", ""+xmlRequest.getStringFromDocument(holder.doc));

		       return   xmlRequest.getStringFromDocument(holder.doc);
		
	}
	
	
	public static String getUserCertificateRequest(String email,String userType )
	{
			/*	<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:urn="urn:MHISE" xmlns:mob="http://schemas.datacontract.org/2004/07/MobiusServiceLibrary">
		   <soap:Header/>
		   <soap:Body>
		      <urn:GetPFXCertificate>
		         <urn:getPFXCertificateRequest>
		            <mob:EmailAddress>?</mob:EmailAddress>
		            <mob:UserType>?</mob:UserType>
		         </urn:getPFXCertificateRequest>
		      </urn:GetPFXCertificate>
		   		</soap:Body>
			</soap:Envelope>
			 */
			XMLRequest xmlRequest = new XMLRequest();
			Document	doc =xmlRequest.InitializeDoc();
		    
			XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GET_USERCERTIFICATE);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			//     <urn:authenticateUserRequest>
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":GetPFXCertificate");
			Element getMasterDataElement =holder.el;	
		
			holder =xmlRequest.addElementToParent(holder.doc,getMasterDataElement ,XmlConstants.NS_URN+":getPFXCertificateRequest");
			Element authenticateUserRequest =holder.el;	
		
			
			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":EmailAddress",email);

			holder =xmlRequest.addElementToParent(holder.doc,authenticateUserRequest ,XmlConstants.NS_MOB+":UserType",userType);
		
			Log.i("Get User certificate  request", ""+xmlRequest.getStringFromDocument(holder.doc));
	
		
		       return   xmlRequest.getStringFromDocument(holder.doc);
		
	}

	public static String getVersionRequest()
	{
		/*	<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:urn="urn:MHISE">
		   <soap:Header/>
		   <soap:Body>
		      <urn:GetApplicationVersion/>
		   </soap:Body>
			</soap:Envelope>*/
		
			XMLRequest xmlRequest = new XMLRequest();
			Document	doc =xmlRequest.InitializeDoc();
		    
			XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GET_APPLICATION_VERSION);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":GetApplicationVersion");
	
		       return   xmlRequest.getStringFromDocument(holder.doc);
		
	}
	
	
	public static String getAddPFXCertificaterequest(String certificate,String emailAddress,String userType)
	{
	/*	<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:urn="urn:MHISE" xmlns:mob="http://schemas.datacontract.org/2004/07/MobiusServiceLibrary">
		   <soap:Header/>
		   <soap:Body>
		      <urn:AddPFXCertificate>
		         <urn:addPFXCertificateRequest>
		            <mob:Certificate><![CDATA[?</mob:Certificate>
		            
		            <mob:EmailAddress>?</mob:EmailAddress>
		            <mob:UserType>?</mob:UserType>
		         </urn:addPFXCertificateRequest>
		      </urn:AddPFXCertificate>
		   </soap:Body>
		</soap:Envelope>*/
		
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		    
		 XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);		
			Element rootelement=holder.el;
		
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_ADD_PFX_CERTIFICATE);	
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element paramsElement =holder.el ;
		
			holder =xmlRequest.addElementToParent(holder.doc, paramsElement,XmlConstants.NS_URN+":AddPFXCertificate");
			Element ChangePasswordElement =holder.el ;
			
			holder =xmlRequest.addElementToParent(holder.doc, ChangePasswordElement,XmlConstants.NS_URN+":addPFXCertificateRequest");
			Element changePasswordRequestElement =holder.el ;
			
			
			holder =xmlRequest.addElementToParent(holder.doc,changePasswordRequestElement ,XmlConstants.NS_MOB+":Certificate",certificate);
			holder =xmlRequest.addElementToParent(holder.doc,changePasswordRequestElement ,XmlConstants.NS_MOB+":EmailAddress",emailAddress);
			holder =xmlRequest.addElementToParent(holder.doc,changePasswordRequestElement ,XmlConstants.NS_MOB+":UserType",userType);

			return xmlRequest.getStringFromDocument(holder.doc);
		
		
		
		
	}
	
	public static String forgotPWDRequest(String email,String userType)
	{
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		    
		 XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_FORGOT_PASSWORD);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":ForgotPassword");
			Element getMasterDataElement =holder.el;	
		
			holder =xmlRequest.addElementToParent(holder.doc,getMasterDataElement ,XmlConstants.NS_URN+":forgotPasswordRequest");
			Element forgotPasswordRequest =holder.el;
			
			holder =xmlRequest.addElementToParent(holder.doc,forgotPasswordRequest ,XmlConstants.NS_MOB+":EmailAddress",email);
			
			

			holder =xmlRequest.addElementToParent(holder.doc,forgotPasswordRequest ,XmlConstants.NS_MOB+":UserType",String.valueOf(userType));
		
			Log.i("login request", ""+xmlRequest.getStringFromDocument(holder.doc));
	
		
		       return   xmlRequest.getStringFromDocument(holder.doc);
		
	}
	
	
	
	public static String getChangePasswordRequest(String emailID,String userType,String oldPWD,String newPWD)
	{
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		    
		 XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);		
			Element rootelement=holder.el;
		
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_CHANGE_PASSWORD);	
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element paramsElement =holder.el ;
		
			holder =xmlRequest.addElementToParent(holder.doc, paramsElement,XmlConstants.NS_URN+":ChangePassword");
			Element ChangePasswordElement =holder.el ;
			
			holder =xmlRequest.addElementToParent(holder.doc, ChangePasswordElement,XmlConstants.NS_URN+":changePasswordRequest");
			Element changePasswordRequestElement =holder.el ;
			
			
			holder =xmlRequest.addElementToParent(holder.doc,changePasswordRequestElement ,XmlConstants.NS_MOB+":EmailAddress",emailID);
			holder =xmlRequest.addElementToParent(holder.doc,changePasswordRequestElement ,XmlConstants.NS_MOB+":UserType",userType);
			holder =xmlRequest.addElementToParent(holder.doc,changePasswordRequestElement ,XmlConstants.NS_MOB+":NewPassword",newPWD);
			holder =xmlRequest.addElementToParent(holder.doc,changePasswordRequestElement ,XmlConstants.NS_MOB+":OldPassword",oldPWD);
		
			
			return xmlRequest.getStringFromDocument(holder.doc);
	}
	
	
	public static String getNhinCommReq(){
		
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();

		XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);
		Element rootelement=holder.el;
		
		//holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header");
		holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
		Element  headerElement =holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GET_COMMUNITIES);	
		
		holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
		Element paramsElement =holder.el ;
		holder =xmlRequest.addElementToParent(holder.doc, paramsElement,XmlConstants.NS_URN+":GetNhinCommunity");
		
		return xmlRequest.getStringFromDocument(holder.doc);	
	
	}

	
	public static String getPHISourceRequest(String AssigningCommunityId,String PatientId){
	
		/*<soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:urn="urn:MHISE" xmlns:mob="http://schemas.datacontract.org/2004/07/MobiusServiceLibrary">
		   <soap:Header/>
		   <soap:Body>
		      <urn:GetPHISource>
		         <urn:patientCorrelationRequest>
		            <mob:AssigningCommunityId>?</mob:AssigningCommunityId>
		            <mob:PatientId>?</mob:PatientId>
		         </urn:patientCorrelationRequest>
		      </urn:GetPHISource>
		   </soap:Body>
		</soap:Envelope>
		Content-Type: application/soap+xml;charset=UTF-8
		action=urn:MHISE/IMobiusSecured/GetPHISource
		*/
		Log.e("getPHISourceRequest","getPHISourceRequest");
		
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		 Log.e("getPHISourceRequest 1","getPHISourceRequest");
		 
		XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
		  holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);
		Element rootelement=holder.el;
		
		Log.e("getPHISourceRequest2 ","getPHISourceRequest");
		
		//holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header");
		holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
		Element  headerElement =holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GET_PHI_SOURCE);	
	
		
		holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
		Element paramsElement =holder.el ;
		holder =xmlRequest.addElementToParent(holder.doc, paramsElement,XmlConstants.NS_URN+":GetPHISource");
		Element getPHISourceNode =holder.el;
		holder =xmlRequest.addElementToParent(holder.doc, getPHISourceNode,XmlConstants.NS_URN+":patientCorrelationRequest");
		Element patientCorrelationRequestNode =holder.el;
		
		Log.e("getPHISourceRequest 4","getPHISourceRequest"+AssigningCommunityId);	
		holder =xmlRequest.addElementToParent(holder.doc,patientCorrelationRequestNode ,XmlConstants.NS_MOB+":AssigningCommunityId",AssigningCommunityId);
		holder =xmlRequest.addElementToParent(holder.doc,patientCorrelationRequestNode ,XmlConstants.NS_MOB+":PatientId",PatientId);
		Log.e("getPHISourceRequest 5","getPHISourceRequest"+PatientId);	
		Log.e("xml request",xmlRequest.getStringFromDocument(holder.doc));
		return xmlRequest.getStringFromDocument(holder.doc);	
	
	}
	
	
	public static String getUserInformation(){
		
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		    
		 XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GETUSERINFORMATION);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":GetUserInformation");
		
		       return   xmlRequest.getStringFromDocument(holder.doc);
	
	}
		

	public static String getMasterDataRequest(String masterCollection)
	{
		XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		    
		 XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);		
			Element rootelement=holder.el;
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GETMASTERDATA);	
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":GetMasterData");
			Element getMasterDataElement =holder.el;	
		
			holder =xmlRequest.addElementToParent(holder.doc,getMasterDataElement ,XmlConstants.NS_URN+":getMasterDataRequest");
			Element getMasterDataRequestElement =holder.el;	
		
			holder =xmlRequest.addElementToParent(holder.doc,getMasterDataRequestElement ,XmlConstants.NS_MOB+":MasterCollection",masterCollection);
			
		       return   xmlRequest.getStringFromDocument(holder.doc);
	}
	

	public static String getDocumentRequest(Assertion assertion, String patientID,String documentID,String purpose,String subject ,String email,boolean locallyAvailableDocuments)
	{
		
		 XMLRequest xmlRequest = new XMLRequest();
		 Document	doc =xmlRequest.InitializeDoc();
		 
		 XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		
		 	holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
			holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);		
			Element rootelement=holder.el;

			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
			Element  headerElement =holder.el;
			holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GETDOCUMENT);	
			
			
			holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
			Element bodyElement=holder.el;
			
			holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":GetDocument");
			Element getDocumentElement =holder.el;	
			
			holder =xmlRequest.addElementToParent(holder.doc,getDocumentElement ,XmlConstants.NS_URN+":getDocumentRequest");
			Element getDocumentRequestElement=holder.el;			
			
			holder=xmlRequest.addElementToParent(holder.doc, getDocumentRequestElement , XmlConstants.NS_MOB+":Assertion");
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
				holder=xmlRequest.addElementToParent(holder.doc, getDocumentRequestElement , XmlConstants.NS_MOB+":LocalData","1");
			}else holder=xmlRequest.addElementToParent(holder.doc, getDocumentRequestElement , XmlConstants.NS_MOB+":LocalData","0");
			
			holder =xmlRequest.addElementToParent(holder.doc,getDocumentRequestElement ,XmlConstants.NS_MOB+":documentId",documentID);
			holder =xmlRequest.addElementToParent(holder.doc,getDocumentRequestElement ,XmlConstants.NS_MOB+":patientId",patientID);
			holder =xmlRequest.addElementToParent(holder.doc,getDocumentRequestElement ,XmlConstants.NS_MOB+":purpose",purpose);
			holder =xmlRequest.addElementToParent(holder.doc,getDocumentRequestElement ,XmlConstants.NS_MOB+":subjectEmailID",email);
			holder =xmlRequest.addElementToParent(holder.doc,getDocumentRequestElement ,XmlConstants.NS_MOB+":subjectRole",subject);
			
			
			Log.e("xmlRequest",""+xmlRequest.getStringFromDocument(holder.doc));
		        return   xmlRequest.getStringFromDocument(holder.doc);
	}

	
	public static String getProviderRequest(ProviderRegistration provider)
    {
		String requestEnvelope =null;
		if (provider.getProviderCode() ==Constants.ORGANIZATIONAL_PROVIDER)
		{    
            XMLRequest xmlRequest = new XMLRequest();
            Document doc =xmlRequest.InitializeDoc();
            XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
            holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
            holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
            
            Element rootelement=holder.el;
            holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header", XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);
            
            Element headerElement=holder.el;
            holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_ADD_PROVIDER);
            
            holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
            Element bodyElement=holder.el;

            holder=xmlRequest.addElementToParent(holder.doc, bodyElement , XmlConstants.NS_URN+":AddProvider");
            Element addProviderElement=holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, addProviderElement , XmlConstants.NS_URN+":Provider");
            Element urnProviderElement=holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, urnProviderElement , XmlConstants.NS_MOB+":CSR", provider.getCSR());
                                    
            holder=xmlRequest.addElementToParent(holder.doc, urnProviderElement , XmlConstants.NS_MOB+":Provider");
            Element mobProviderElement=holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":City");
            Element cityElement = holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, cityElement , XmlConstants.NS_MOB+":CityName" , provider.getCityName());        
            holder=xmlRequest.addElementToParent(holder.doc, cityElement , XmlConstants.NS_MOB+":State" /*, provider.getStateName()*/); // no value given
            Element stateElement = holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, stateElement , XmlConstants.NS_MOB+":Country");
            Element countryElement = holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, countryElement , XmlConstants.NS_MOB+":CountryName" , provider.getCountry());
            
            holder=xmlRequest.addElementToParent(holder.doc, stateElement , XmlConstants.NS_MOB+":StateName" , provider.getStateName());
            
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":ContactNumber" , provider.getContactNumber());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":ElectronicServiceURI" , provider.getElectronicServiceURI());
                   
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":IndividualProvider" , Constants.OrgProv_isIndividualProvider);
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Language" );
            
            Element languageElement= holder.el;       
            holder=xmlRequest.addElementToParent(holder.doc, languageElement , XmlConstants.NS_MOB+":LanguageId" , Constants.LanguageID+"");
            
            //  <mob:Password>?</mob:Password>
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":MedicalRecordsDeliveryEmailAddress" , provider.getMedicalRecordsDeliveryEmailAddress());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":OrganizationName" , provider.getOrganizationName());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Password" , 	provider.getPassword());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":PostalCode" , provider.getPostalCode());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":ProviderType" , provider.getProviderType());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Status" , provider.getStatus());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":StreetName" , provider.getAddressLine1()); // street name
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":StreetNumber" , provider.getAddressLine2()); // street number
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Specialty");
            
            Element specialityElement = holder.el;
            
            ArrayList<String> _specialities =provider.getspecialtyNameAndID();
            
            if (_specialities !=null){   
                  
                   for(int i=0;i<_specialities.size();i++ )
                  {
                        String[] codeDesc = _specialities.get(i).split(Constants.STR_SEPARATOR_SPECIALITIES);
                  
                         holder=xmlRequest.addElementToParent(holder.doc, specialityElement , XmlConstants.NS_MOB+":Specialty" );
                        Element childSpecialityElement = holder.el;
                        
                         holder=xmlRequest.addElementToParent(holder.doc, childSpecialityElement , XmlConstants.NS_MOB+":SpecialtyID" , codeDesc[0] );
                        holder=xmlRequest.addElementToParent(holder.doc, childSpecialityElement , XmlConstants.NS_MOB+":SpecialtyName" , codeDesc[1] );
                        
                  }
            
            }
                 
            return xmlRequest.getStringFromDocument(holder.doc);

		}
		else if (provider.getProviderCode() ==Constants.INDIVIDUAL_PROVIDER)
		{
			
            
            XMLRequest xmlRequest = new XMLRequest();
      Document doc =xmlRequest.InitializeDoc();

      		XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
            holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
            holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
            
            Element rootelement=holder.el;
            holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header", XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);
            
            Element headerElement=holder.el;
            holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_ADD_PROVIDER);
            
            holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
            Element bodyElement=holder.el;

            holder=xmlRequest.addElementToParent(holder.doc, bodyElement , XmlConstants.NS_URN+":AddProvider");
            Element addProviderElement=holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, addProviderElement , XmlConstants.NS_URN+":Provider");
            Element urnProviderElement=holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, urnProviderElement , XmlConstants.NS_MOB+":CSR", provider.getCSR());
                                    
            holder=xmlRequest.addElementToParent(holder.doc, urnProviderElement , XmlConstants.NS_MOB+":Provider");
            Element mobProviderElement=holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":City");
            Element cityElement = holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, cityElement , XmlConstants.NS_MOB+":CityName" , provider.getCityName());        
            holder=xmlRequest.addElementToParent(holder.doc, cityElement , XmlConstants.NS_MOB+":State" /*, provider.getStateName()*/); // no value given
            Element stateElement = holder.el;
            
            holder=xmlRequest.addElementToParent(holder.doc, stateElement , XmlConstants.NS_MOB+":Country");
            Element countryElement = holder.el;
            
            //verify country
            holder=xmlRequest.addElementToParent(holder.doc, countryElement , XmlConstants.NS_MOB+":CountryName" , Constants.Country); 
            holder=xmlRequest.addElementToParent(holder.doc, stateElement , XmlConstants.NS_MOB+":StateName" , provider.getStateName());
            
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":ContactNumber" , provider.getContactNumber());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":ElectronicServiceURI" , provider.getElectronicServiceURI());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Email" , provider.getEmail());
            
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":FirstName" , provider.getFirstName());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Gender" , provider.getGender());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":IndividualProvider" , Constants.IndProv_isIndividualProvider);
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Language" );
            
            Element languageElement= holder.el;       
            holder=xmlRequest.addElementToParent(holder.doc, languageElement , XmlConstants.NS_MOB+":LanguageId" , Integer.toString(Constants.LanguageID));
            
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":LastName", provider.getLastName());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":MedicalRecordsDeliveryEmailAddress" , provider.getMedicalRecordsDeliveryEmailAddress());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":MiddleName",	provider.getMiddleName());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Password" , 	provider.getPassword());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":PostalCode" ,	 provider.getPostalCode());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":ProviderType" , provider.getProviderType());
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Status" , Constants.IndProv_status);
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":StreetName" , provider.getAddressLine1()); // street name
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":StreetNumber" , provider.getAddressLine2()); // street number
            holder=xmlRequest.addElementToParent(holder.doc, mobProviderElement , XmlConstants.NS_MOB+":Specialty");
            
            Element specialityElement = holder.el;
            
            ArrayList<String> _specialities =provider.getspecialtyNameAndID();
            
            if (_specialities !=null){   
                  
                   for(int i=0;i<_specialities.size();i++ )
                  {
                        String[] codeDesc = _specialities.get(i).split(Constants.STR_SEPARATOR_SPECIALITIES);
                  
                         holder=xmlRequest.addElementToParent(holder.doc, specialityElement , XmlConstants.NS_MOB+":Specialty" );
                        Element childSpecialityElement = holder.el;
                        
                         holder=xmlRequest.addElementToParent(holder.doc, childSpecialityElement , XmlConstants.NS_MOB+":SpecialtyID" , codeDesc[0] );
                        holder=xmlRequest.addElementToParent(holder.doc, childSpecialityElement , XmlConstants.NS_MOB+":SpecialtyName" , codeDesc[1] );
                        
                  }
            
            }
                  
            return xmlRequest.getStringFromDocument(holder.doc);
            

		}
		else
		{
			requestEnvelope =null;
			Log.i("Provider type -->","Nothing");
		}
         
    	return requestEnvelope;
    }
	
	
    
	public static String getLocalityByZipRequest(String ZIP)
    {
		XMLRequest xmlRequest = new XMLRequest();
		Document	doc =xmlRequest.InitializeDoc();
		 
		XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
		holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);

		
		Element rootelement=holder.el;
		
		//<soap:Header xmlns:wsa="http://www.w3.org/2005/08/addressing"><wsa:Action>urn:MHISE/IMobius/GetLocalityByZipCode</wsa:Action></soap:Header>
		holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Header",XmlConstants.XMLNS+XmlConstants.NS_WSA,XmlConstants.VALUE_WSA);
		Element  headerElement =holder.el;
		holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action",XmlConstants.ACTION_GETLOCALITYBYZIPCODE);
		
		holder=xmlRequest.addElementToParent(holder.doc, rootelement , XmlConstants.NS_SOAP+":Body");
		Element bodyElement=holder.el;
		
		holder =xmlRequest.addElementToParent(holder.doc,bodyElement ,XmlConstants.NS_URN+":GetLocalityByZipCode");
		Element getLocalityByZipCodeElement=holder.el;
		
		holder =xmlRequest.addElementToParent(holder.doc,getLocalityByZipCodeElement ,XmlConstants.NS_URN+":zipCode",ZIP);
    	return    xmlRequest.getStringFromDocument(holder.doc);
	
    }
	

	public static String getPatientRegistrationRequest(Patient patient)
	{
        Demographics demographisDetail =patient.getDemographics();
        Address[]  arraddressDetails =patient.getAddress();
        Address addressDetails =arraddressDetails[0];
        City cityDetails =addressDetails.getCity();
        Telephone[] arrTelephoneDetails =patient.getTelephone();
       Telephone TelephoneDetails=arrTelephoneDetails[0];	     
	   
         XMLRequest xmlRequest = new XMLRequest();
         Document doc =xmlRequest.InitializeDoc();

         XMLHolder holder = xmlRequest.addRoot(doc, XmlConstants.NS_SOAP+":Envelope", XmlConstants.XMLNS+XmlConstants.NS_SOAP, XmlConstants.VALUE_SOAP, ""); 
               holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_URN, XmlConstants.VALUE_URN);
               holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_MOB, XmlConstants.VALUE_MOB);
               holder = xmlRequest.addAttribute(doc, holder.el, XmlConstants.XMLNS+XmlConstants.NS_ARR, XmlConstants.VALUE_ARR);

               Element rootElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, rootElement , XmlConstants.NS_SOAP+":Header", XmlConstants.XMLNS+XmlConstants.NS_WSA, XmlConstants.VALUE_WSA);
               
               Element headerElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, headerElement , XmlConstants.NS_WSA+":Action", XmlConstants.ACTION_ADD_PATIENT);
               
               holder=xmlRequest.addElementToParent(holder.doc, rootElement , XmlConstants.NS_SOAP+":Body");
               
               Element bodyElement = holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, bodyElement , XmlConstants.NS_URN+":AddPatient");
               
               Element addPatientElement = holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, addPatientElement , XmlConstants.NS_URN+":registerPatientRequest");
               
               Element registerPatientRequestElement = holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, registerPatientRequestElement , XmlConstants.NS_MOB+":Patient");
               
               Element patientElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":CSR" , patient.getCSR());
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":CommunityId" ,Constants.Default_CommunityID);
                
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":DOB" , demographisDetail.getDOB());
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":EmailAddress" , patient.getEmailAddress());
               
              
               //        <mob:Password>?</mob:Password>
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":FamilyName");
               
               Element familyNameElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, familyNameElement , XmlConstants.NS_ARR+":string" , demographisDetail.getFamilyName());
               
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":Gender" , demographisDetail.getGender());
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":GivenName");
               
               Element givenNameElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, givenNameElement , XmlConstants.NS_ARR+":string" , demographisDetail.getGivenName());
               
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":MiddleName");
               
               Element middleNameElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, middleNameElement , XmlConstants.NS_ARR+":string" , demographisDetail.getMiddleName());
               
               
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":MothersMaidenName");
               Element mothersMaidenName=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, mothersMaidenName , XmlConstants.NS_MOB+":FamilyName" , demographisDetail.getMotherMaiden().getFamilyName());
               holder=xmlRequest.addElementToParent(holder.doc, mothersMaidenName , XmlConstants.NS_MOB+":GivenName" , demographisDetail.getMotherMaiden().getGivenName());
               holder=xmlRequest.addElementToParent(holder.doc, mothersMaidenName , XmlConstants.NS_MOB+":MiddleName" , demographisDetail.getMotherMaiden().getMiddleName());
               holder=xmlRequest.addElementToParent(holder.doc, mothersMaidenName , XmlConstants.NS_MOB+":Prefix" );
               holder=xmlRequest.addElementToParent(holder.doc, mothersMaidenName , XmlConstants.NS_MOB+":Suffix",demographisDetail.getMotherMaiden().getSuffix());
               
               
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":Password" , patient.getPassword());
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":PatientAddress");
               
               Element patientAddressElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, patientAddressElement , XmlConstants.NS_MOB+":Address");
               
               Element addressElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, addressElement , XmlConstants.NS_MOB+":AddressLine1",addressDetails.getAddressLine1());
               holder=xmlRequest.addElementToParent(holder.doc, addressElement , XmlConstants.NS_MOB+":AddressLine2",addressDetails.getAddressLine2());
               //verify
               holder=xmlRequest.addElementToParent(holder.doc, addressElement , XmlConstants.NS_MOB+":AddressStatus",Constants.PR_AddressStatus);
               holder=xmlRequest.addElementToParent(holder.doc, addressElement , XmlConstants.NS_MOB+":City");
               
               Element cityElement=holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, cityElement , XmlConstants.NS_MOB+":CityName",cityDetails.getCityName());
               holder=xmlRequest.addElementToParent(holder.doc, cityElement , XmlConstants.NS_MOB+":State");
               
               Element stateElement = holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, stateElement , XmlConstants.NS_MOB+":Country");
               
               Element countryElement= holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, countryElement , XmlConstants.NS_MOB+":CountryName", Constants.Country);           
               
               holder=xmlRequest.addElementToParent(holder.doc, stateElement , XmlConstants.NS_MOB+":StateName", cityDetails.getState().getStateName());
               holder=xmlRequest.addElementToParent(holder.doc, addressElement , XmlConstants.NS_MOB+":Zip", addressDetails.getZip());
               
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":Prefix" );
               Element prefix= holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, prefix , XmlConstants.NS_ARR+":string" );
               
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":SSN", demographisDetail.getSSN());
               
   
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":Suffix" );
               Element suffix= holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, suffix , XmlConstants.NS_ARR+":string" );

               
               holder=xmlRequest.addElementToParent(holder.doc, patientElement , XmlConstants.NS_MOB+":Telephones");
               Element telephone =holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, telephone , XmlConstants.NS_MOB+":Telephone");
              
              
            
               
               Element telephoneElement= holder.el;
               holder=xmlRequest.addElementToParent(holder.doc, telephoneElement , XmlConstants.NS_MOB+":Extensionnumber",TelephoneDetails.getExtensionnumber());
               holder=xmlRequest.addElementToParent(holder.doc, telephoneElement , XmlConstants.NS_MOB+":Number",TelephoneDetails.getNumber());
               holder=xmlRequest.addElementToParent(holder.doc, telephoneElement , XmlConstants.NS_MOB+":Type",TelephoneDetails.getType());
               
               Log.e("xmlRequest",""+xmlRequest.getStringFromDocument(holder.doc));
               return xmlRequest.getStringFromDocument(holder.doc);

	}

}
