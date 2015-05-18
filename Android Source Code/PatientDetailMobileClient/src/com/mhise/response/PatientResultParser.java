package com.mhise.response;

import java.util.ArrayList;

import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.mhise.constants.Constants;
import com.mhise.model.Address;
import com.mhise.model.City;
import com.mhise.model.Demographics;
import com.mhise.model.Patient;
import com.mhise.model.SearchPatientResult;
import com.mhise.model.State;


/** 
*@(#)PatientResultParser.java 
* @author R Systems
* @description This class contains the methods to parse Patient result 
* 
* @since 2012-10-26
* @version 1.0 
*/


public class PatientResultParser {

	
	com.mhise.model.Result  result;
	Patient patientOBJ ;

	
	public   SearchPatientResult parseXML(Document dom)
	{	
		SearchPatientResult searchpatientresult = new SearchPatientResult();
		ArrayList<Patient>  arrPatient = new ArrayList<Patient>();
		searchpatientresult.setPatient( arrPatient);	
		NodeList nl = dom.getElementsByTagName(Constants.TAG_SEARCH_PATIENT_RESULT);
		Node  n1 = nl.item(0);
		NodeList childListofSearchPatientResultNode = n1.getChildNodes();
		
		for (int i=0;i<childListofSearchPatientResultNode.getLength();i++){  
			 
			Node  subnode = childListofSearchPatientResultNode.item(i);
			
			//Logger.debug("PatientResultParser-->SearchPatientResult Node" , ""+subnode.getNodeName());
			
			if (subnode.getNodeName().equals(Constants.TAG_PATIENTS))
			{
				NodeList patientNodeList = subnode.getChildNodes(); 
				 
				//Logger.debug("PatientResultParser-->TAG_PATIENTS Node" , "1");
				
				if(patientNodeList.getLength() >0)
				{
					//Logger.debug("PatientResultParser-->patientNodeList Node" , "="+patientNodeList.getLength());
					
					for(int m =0; m<patientNodeList.getLength();m++)
					{
						patientOBJ = new Patient();
						Node patientNode =patientNodeList.item(m);
						setPatient(patientNode, patientOBJ);
						arrPatient.add(patientOBJ);						
					}					
				}

			}
			else if (subnode.getNodeName().equals(Constants.TAG_RESULT) )
			{
				    result = new com.mhise.model.Result();
				    BaseParser.setResult(subnode, result);
				    searchpatientresult.setResult(result);
				   // com.mhise.constants.AppLicationConstants.searchpatientresult.result =result;
			}

		 	}
		 return searchpatientresult; 
	}
	
	private void setPatient(Node patientnode, Patient patient)
	{
		Demographics demographics = new Demographics();
	
		NodeList  patientInfoNode =patientnode.getChildNodes();
		for (int pi =0;pi<patientInfoNode.getLength();pi++)
			{
				Node patientInfo =patientInfoNode.item(pi);
				//Logger.debug("PatientResultParser-->patientInfo--->" , ""+patientInfo.getNodeName());
			
				if(patientInfo.getNodeName().equals(Constants.TAG_PATIENT_ADDRESS))
				{
					com.mhise.model.Address address;
					//Logger.debug("PatientResultParser-->TAG_ADDRESS" , ""+patientInfo.getNodeName());
					//Logger.debug("PatientResultParser-->address tag" , ""+patientInfo.getChildNodes().item(0).getChildNodes());
		
					NodeList _addressElement =patientInfo.getChildNodes();
		
					Address[] tempAddress = new Address[_addressElement.getLength()];
					//Address 
					for(int a=0;a<_addressElement.getLength();a++)
					{
						
					Node addressNode =	_addressElement.item(a);
					address = new com.mhise.model.Address();
					setAddress(addressNode,address);
					//Logger.debug("PatientResultParser-->address"+address, "a"+a);
					tempAddress[a]=address;
		
					}
					patient.setAddress(tempAddress);
				}
				else if(patientInfo.getNodeName().equals(Constants.TAG_DOB))
				{
					//Logger.debug("PatientResultParser-->TAG_DOB" , ""+patientInfo.getTextContent());
					String _strDOB =patientInfo.getTextContent();
					String _year =_strDOB.substring(0, 4);
					String _month =_strDOB.substring(4, 6);
					String _day =_strDOB.substring(6, 8);
					demographics.setDOB(_month+"/"+_day+"/"+_year);
					
				}
				else if(patientInfo.getNodeName().equals(Constants.TAG_GENDER))
				{
					//Logger.debug("PatientResultParser-->TAG_GENDER" , ""+patientInfo.getTextContent());
					String gender =patientInfo.getTextContent();
					if(gender.equals("Male"))
					{
					demographics.setGender("Male");
					//Logger.debug("PatientResultParser-->TAG_GENDER" , ""+patientInfo.getTextContent());
					}
					if(gender.equals("Female"))
					{
					demographics.setGender("Female");
					//Logger.debug("PatientResultParser-->TAG_GENDER" , ""+patientInfo.getTextContent());
					}					
				}
				
				else if(patientInfo.getNodeName().equals(Constants.TAG_PATIENT_ID))
				{
					//Logger.debug("PatientResultParser-->TAG_PATIENT_ID" , ""+patientInfo.getTextContent());
					patient.patientId =patientInfo.getTextContent();
				}
				else if(patientInfo.getNodeName().equals(Constants.TAG_FIRSTNAME))
				{
					//Logger.debug("PatientResultParser-->TAG_FIRSTNAME" , ""+patientInfo.getTextContent());
					//Logger.debug("PatientResultParser-->patientInfo.getChildNodes().item(0).getNodeName()" , 	""+patientInfo.getChildNodes().item(0).getNodeName());
					//Logger.debug("PatientResultParser-->patientInfo.getChildNodes().item(0).getNodevalue()" , 	""+patientInfo.getChildNodes().item(0).getTextContent());
					
					demographics.setGivenName(patientInfo.getChildNodes().item(0).getTextContent());
					
				}
				else if(patientInfo.getNodeName().equals(Constants.TAG_LASTNAME))
				{
					//Logger.debug("PatientResultParser-->TAG_LASTNAME" , ""+patientInfo.getTextContent());
					demographics.setFamilyName(patientInfo.getChildNodes().item(0).getTextContent());
				}
				else if(patientInfo.getNodeName().equals(Constants.TAG_COMMUNITY))
				{
					//Logger.debug("PatientResultParser-->TAG_LASTNAME" , ""+patientInfo.getTextContent());
					demographics.setCommunityDescription(patientInfo.getChildNodes().item(0).getTextContent());
				}
			
			}
		patient.demographics = demographics;
	}

	private void setAddress(Node addressNode ,com.mhise.model.Address address)
	{
		NodeList addressElement = addressNode.getChildNodes();
		for (int ae = 0; ae < addressElement.getLength(); ae++) {
			Node addressElementNode = addressElement.item(ae);
			if (addressElementNode.getNodeName().equals(Constants.TAG_AddressLine1)) {
				address.setAddressLine1(addressElementNode.getTextContent());
			}else if (addressElementNode.getNodeName().equals(Constants.TAG_AddressLine2)) {
				address.setAddressLine2(addressElementNode.getTextContent());
			}else if (addressElementNode.getNodeName().equals(Constants.TAG_AddressStatus)) {
				address.setStatus(addressElementNode.getTextContent());
			}else if (addressElementNode.getNodeName().equals(Constants.TAG_CITY)) {
				City city = new City();
				NodeList cityElement = addressElementNode.getChildNodes();
				for (int c = 0; c < cityElement.getLength(); c++) {

					Node cityElementNode = cityElement.item(c);
					if (cityElementNode.getNodeName().equals(
							Constants.TAG_STATE)) {
						State state = new State();
						// Logger.debug("PatientResultParser-->state name",
						// "state:"+cityElementNode.getNodeName());
						NodeList stateElement = cityElementNode.getChildNodes();
						for (int s = 0; s < cityElement.getLength(); s++) {
							Node stateNode = stateElement.item(s);
							// Logger.debug("PatientResultParser-->stateNode.getNodeName()",
							// ""+stateNode.getNodeName());
							if (stateNode.getNodeName().equals(
									Constants.TAG_STATENAME)) {
								// Logger.debug("PatientResultParser-->stateNode. get text content )",
								// ""+stateNode.getTextContent());

								state.setStateName(stateNode.getTextContent());
							}
						}
						city.setState(state);
					}

					if (cityElementNode.getNodeName().equals(
							Constants.TAG_CITYNAME)) {
						// Logger.debug("PatientResultParser-->city name",
						// "city:"+cityElementNode.getTextContent());
						city.setCityName(cityElementNode.getTextContent());
					}
				}
				address.setCity(city);
			}else if (addressElementNode.getNodeName().equals(Constants.TAG_ZIP)) {
				// Logger.debug("PatientResultParser-->TAG_ZIP",
				// ""+addressElementNode.getTextContent());
				address.setZip(addressElementNode.getTextContent());
			}
		}

	}
	
	
	
}
