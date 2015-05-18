package com.mhise.response;

import java.util.ArrayList;

import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import android.util.Log;

import com.mhise.constants.Constants;
import com.mhise.model.AuthenticateUserResponse;
import com.mhise.model.MasterData;
import com.mhise.model.Result;

/** 
*@(#)GetMasterDataParser.java 
* @author R Systems
* @description This class contains the methods to parse GetMasterData Response
* 
* @since 2012-10-26
* @version 1.0 
*/
public class GetMasterDataParser {

	MasterData masterDataObj;
	
	public MasterData parseMasterDataResponse(Document dom)
	{
		masterDataObj = new MasterData();	
		masterDataObj.result = new Result();
		masterDataObj._arrDescription = new ArrayList<String>();
		masterDataObj._arrCode=new ArrayList<String>();
		try{
		NodeList nl = dom.getElementsByTagName(com.mhise.constants.Constants.TAG_GET_MASTER_DATA_RESULT);
		Node  n1 = nl.item(0);
		NodeList childListMasterData = n1.getChildNodes();
		
		for (int i=0;i<childListMasterData.getLength();i++){  
			 
			Node  subnode = childListMasterData.item(i);					
			
			if (subnode.getNodeName().equals(com.mhise.constants.Constants.TAG_GET_MASTER_DATA_COLLECTION))
			{
				
				NodeList masterdataList =subnode.getChildNodes();
				setMasterData(masterdataList,masterDataObj._arrDescription,masterDataObj._arrCode);

			}
			else if (subnode.getNodeName().equals(com.mhise.constants.Constants.TAG_RESULT) )
			{			   
				BaseParser.setResult(subnode, masterDataObj.result);		
			}

		 }
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			//Log.i("Null pointer exception while parsing", ""+e);
		}
		return masterDataObj;

	}
	
	public AuthenticateUserResponse parseAuthenticateUserResponse(Document dom)
	{
		AuthenticateUserResponse authenticateUserResponse = new AuthenticateUserResponse();
		 Result result = new Result();
		 authenticateUserResponse.setResult(result);
		 
	
		try{
		NodeList nl = dom.getElementsByTagName(com.mhise.constants.Constants.AuthenticateUserResult);
		Log.i("Node list",""+nl.getLength());
		Log.i("Node list",""+nl.item(0).getChildNodes());
		Node  n1 = nl.item(0);
		NodeList childListMasterData = n1.getChildNodes();
		
		for (int i=0;i<childListMasterData.getLength();i++){  
			 
			Node  subnode = childListMasterData.item(i);					
			
			if (subnode.getNodeName().equals(com.mhise.constants.Constants.Name))
			{
				
				authenticateUserResponse.setname(subnode.getTextContent());

			}
			else if (subnode.getNodeName().equals(com.mhise.constants.Constants.TAG_RESULT) )
			{			   
				BaseParser.setResult(subnode, result);		
			}
			else if (subnode.getNodeName().equals(com.mhise.constants.Constants.CertificateSerialNumber) )
			{			   
				authenticateUserResponse.setCertificateSerialNumber(subnode.getTextContent());		
			}
			

		 }
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Log.i("Null pointer exception while parsing", ""+e);
		}
		return authenticateUserResponse;

	}
	
	
	public Result parseForgotPWD(Document dom)
	{		
		 Result result = new Result();			 
	
		try{
		NodeList nl = dom.getElementsByTagName(com.mhise.constants.Constants.ForgotPasswordResult);
		Log.i("Node list",""+nl.getLength());
		Log.i("Node list",""+nl.item(0).getChildNodes());
		Node  n1 = nl.item(0);
		NodeList childListMasterData = n1.getChildNodes();
		
		for (int i=0;i<childListMasterData.getLength();i++){  
			 
			Node  subnode = childListMasterData.item(i);					
			
		 if (subnode.getNodeName().equals(com.mhise.constants.Constants.TAG_RESULT) )
			{			   
				BaseParser.setResult(subnode, result);		
			}
			
			

		 }
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Log.i("Null pointer exception while parsing", ""+e);
		}
		return result;

	}
	
	
	public Result parseChangePWD(Document dom)
	{		
		 Result result = new Result();			 
	
		try{
		NodeList nl = dom.getElementsByTagName(com.mhise.constants.Constants.ChangePasswordResult);
		Log.i("Node list",""+nl.getLength());
		Log.i("Node list",""+nl.item(0).getChildNodes());
		Node  n1 = nl.item(0);
		NodeList childListMasterData = n1.getChildNodes();
		
		for (int i=0;i<childListMasterData.getLength();i++){  
			 
			Node  subnode = childListMasterData.item(i);					
			
		 if (subnode.getNodeName().equals(com.mhise.constants.Constants.TAG_RESULT) )
			{			   
				BaseParser.setResult(subnode, result);		
			}
			
			

		 }
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Log.i("Null pointer exception while parsing", ""+e);
		}
		return result;

	}
	
	private void setMasterData(NodeList masterdataList,
			
			ArrayList<String> _arrPurpose,ArrayList<String> _arrCode) {
		
		for(int m=0;m<masterdataList.getLength();m++)
		{			
		Node masterDataNode =masterdataList.item(m);	
		NodeList masterdatachildList =masterDataNode.getChildNodes();
			for(int k=0;k<masterdatachildList.getLength();k++)
			{
			Node childNodeOfMasterData =masterdatachildList.item(k);
			
			if(childNodeOfMasterData.getNodeName().equals(Constants.TAG_DESCRIPTION))
			{
				_arrPurpose.add(childNodeOfMasterData.getTextContent());
				
			}
			else if(childNodeOfMasterData.getNodeName().equals(Constants.TAG_CODE))
			{
				_arrCode.add(childNodeOfMasterData.getTextContent());
			}
			}

		}	
	}
	
	
}
