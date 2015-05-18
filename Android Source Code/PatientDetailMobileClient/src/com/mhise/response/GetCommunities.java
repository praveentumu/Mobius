package com.mhise.response;

import java.util.ArrayList;

import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import android.util.Log;

import com.mhise.constants.Constants;
import com.mhise.model.CommunityResult;
import com.mhise.model.NHINCommunity;
import com.mhise.model.Result;

/** 
*@(#)GetCommunities.java 
* @author R Systems
* @description This class contains the methods to get Community from  get Community response
* 
* @since 2012-10-26
* @version 1.0 
*/

public class GetCommunities  {
	
	
	public static CommunityResult   parseXML(Document dom)
	{	
		
		CommunityResult commResult = new CommunityResult();
      
		NodeList nl = dom.getElementsByTagName(Constants.TAG_COMMUNITY_RESULT);
		Node  n1 = nl.item(0);
		ArrayList<NHINCommunity> arrCommunity = new ArrayList<NHINCommunity>();
		NodeList childResultNode = n1.getChildNodes();
		
		
		for (int i=0;i<childResultNode.getLength();i++){  
			
			Node  subnode = childResultNode.item(i);
		
			
			if (subnode.getNodeName().equals(Constants.TAG_COMMUNITIES))
			{ 
				 NodeList commList =subnode.getChildNodes();
				 
				 for(int m =0; m<commList.getLength();m++)
				 {
				     NHINCommunity  community = new NHINCommunity();
					 Node communityNode = commList.item(m); 
				
					 NodeList  comInfoNode= communityNode.getChildNodes();
					 {
					
						 for(int k =0; k<comInfoNode.getLength();k++)
						 {
							 Node communityDetalisNode = comInfoNode.item(k);	

							// Node  communityDetalisNode = communityNode.getChildNodes().item(k);
	
							 if (communityDetalisNode.getNodeName().equals(Constants.TAG_bCOMMUNITY_IDENTIFIER))
							 {
							
								 community.setCommunityIdentifier(communityDetalisNode.getTextContent());
							 }
							 else if (communityDetalisNode.getNodeName().equals(Constants.TAG_bCOMMUNITY_NAME))
							{
								
								 community.setCommunityName(communityDetalisNode.getTextContent());
							}
							 else if (communityDetalisNode.getNodeName().equals(Constants.TAG_bIS_HOME_COMMUNITY))
							{
								 community.setIsHomeCommunity(Boolean.valueOf(communityDetalisNode.getTextContent()));
							}
							 else if (communityDetalisNode.getNodeName().equals(Constants.TAG_bCOMMUNITY_DESCRIPTION))
								{
									 community.setCommunityDescription(communityDetalisNode.getTextContent());
								}
					
						 }
					 }
					arrCommunity.add(community); 
				 }	
			}
			else if (subnode.getNodeName().equals(Constants.TAG_RESULT) )
			{
				   Result result = new com.mhise.model.Result();
					BaseParser.setResult(subnode, result);
					commResult.setResult(result);
			}

		
		}
		commResult.setNHINCommunity(arrCommunity);
		
	
		return commResult;
	} 	
		  

	
	public static CommunityResult   parsePHISourceXML(Document dom)
	{			
		CommunityResult commResult = new CommunityResult();      
		NodeList nl = dom.getElementsByTagName(Constants.TAG_PHI_SOURCE_RESULT);
		Node  n1 = nl.item(0);
		ArrayList<NHINCommunity> arrCommunity = new ArrayList<NHINCommunity>();
		NodeList childResultNode = n1.getChildNodes();
		
		
		for (int i=0;i<childResultNode.getLength();i++){  
			
			Node  subnode = childResultNode.item(i);
		
			
			if (subnode.getNodeName().equals(Constants.TAG_PatientIdentifiers))
			{ 
				 NodeList commList =subnode.getChildNodes();
				 
				 for(int m =0; m<commList.getLength();m++)
				 {
				     NHINCommunity  community = new NHINCommunity();
					 Node communityNode = commList.item(m); 
				
					 
					 NodeList  comInfoNode= communityNode.getChildNodes();
					 {
						 
					
						 for(int k =0; k<comInfoNode.getLength();k++)
						 {
							 Node communityDetalisNode = comInfoNode.item(k);	
							
							// Node  communityDetalisNode = communityNode.getChildNodes().item(k);
	
							 if (communityDetalisNode.getNodeName().equals(Constants.TAG_bCOMMUNITY_IDENTIFIER))
							 {							
								 community.setCommunityIdentifier(communityDetalisNode.getTextContent());
							 }
							 else if (communityDetalisNode.getNodeName().equals(Constants.TAG_bCOMMUNITY_NAME))
							{								
								 community.setCommunityName(communityDetalisNode.getTextContent());
							}
							 else if (communityDetalisNode.getNodeName().equals(Constants.TAG_bIS_HOME_COMMUNITY))
							{
								 community.setIsHomeCommunity(Boolean.valueOf(communityDetalisNode.getTextContent()));
							}
							 else if (communityDetalisNode.getNodeName().equals(Constants.TAG_bCOMMUNITY_DESCRIPTION))
								{
									 community.setCommunityDescription(communityDetalisNode.getTextContent());
								}
					
						 }
					 }
					arrCommunity.add(community); 
				 }	
			}
			else if (subnode.getNodeName().equals(Constants.TAG_RESULT) )
			{
				   Result result = new com.mhise.model.Result();
					BaseParser.setResult(subnode, result);
					commResult.setResult(result);
			}

		
		}
		commResult.setNHINCommunity(arrCommunity);
		
	
		return commResult;
	} 	
		
	
}
