package com.mhise.response;

import java.util.ArrayList;

import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import com.mhise.constants.Constants;
import com.mhise.model.GetDocumentMetaDataResult;
import com.mhise.model.Result;
//import com.mhise.util.Logger;


/** 
*@(#)GetDocumentMetaDataResponse.java 
* @author R Systems
* @description This class contains the methods to parse GetDocumentMetaData Response
* 
* @since 2012-10-26
* @version 1.0 
*/
public class GetDocumentMetaDataResponse {

	GetDocumentMetaDataResult docDetailResponse;
	com.mhise.model.Result  result;
	ArrayList< com.mhise.model.Document> _arrlistDocument;
	com.mhise.model.Document document;
	
	public  GetDocumentMetaDataResult  parseXML(Document dom)
	{	
		docDetailResponse = new GetDocumentMetaDataResult();
		result = new Result();
		
		_arrlistDocument = new ArrayList<com.mhise.model.Document>();

		NodeList nl = dom.getElementsByTagName(Constants.TAG_GET_DOCUMENT_METADATRESULT);
		Node  n1 = nl.item(0);
		NodeList childListGetDocumentMetadataResultNode = n1.getChildNodes();
		
		for (int i=0;i<childListGetDocumentMetadataResultNode.getLength();i++){  
			 
			Node  subnode = childListGetDocumentMetadataResultNode.item(i);
			
			//Logger.debug("GetDocumentMetaDataResult-->childListGetDocumentMetadataResultNode Node" , ""+subnode.getNodeName());
			
			if (subnode.getNodeName().equals(Constants.TAG_DOCUMENTS))
			{
				NodeList documentsNodeList = subnode.getChildNodes(); 
				
				if(documentsNodeList.getLength() >0)
				{									
					for(int m =0; m<documentsNodeList.getLength();m++)
					{
						document = new com.mhise.model.Document();
						Node documentNode =documentsNodeList.item(m);
						setDocument(documentNode, document);
						_arrlistDocument.add(document);
						
					}
					
				}
				docDetailResponse.arrDocDetails = _arrlistDocument;

			}
			else if (subnode.getNodeName().equals(Constants.TAG_RESULT) )
			{
				    result = new com.mhise.model.Result();
					BaseParser.setResult(subnode, result);
					docDetailResponse.result =result;
			}

		 	}
		 //return com.mhise.constants.AppLicationConstants.searchpatientresult; 
	return docDetailResponse;
	}
	
	private void setDocument(Node patientnode, com.mhise.model.Document docObj)
	{
		
		NodeList  docInfoNode =patientnode.getChildNodes();
		for (int pi =0;pi<docInfoNode.getLength();pi++)
		{
			Node docInfo =docInfoNode.item(pi);

				if(docInfo.getNodeName().equals(Constants.TAG_bDOCUMENT_ID))
				{
					
					docObj.setDocumentUniqueId(docInfo.getTextContent());
		
				}
				else if(docInfo.getNodeName().equals(Constants.TAG_bDATASOURCE))
				{	
					
					docObj.setDataSource(docInfo.getTextContent());	
				}
				else if(docInfo.getNodeName().equals(Constants.TAG_bDOCUMENT_TITLE))
				{
					docObj.setDocumentTitle(docInfo.getTextContent());
				
				}
				
				else if(docInfo.getNodeName().equals(Constants.TAG_bAUTHOR))
				{
					docObj.setAuthor(docInfo.getTextContent());
				}
				else if(docInfo.getNodeName().equals(Constants.TAG_bCREATED_ON))
				{
				
					docObj.setCreatedOn(docInfo.getTextContent());
				}
			
	  }
		
	}



	
}
	

