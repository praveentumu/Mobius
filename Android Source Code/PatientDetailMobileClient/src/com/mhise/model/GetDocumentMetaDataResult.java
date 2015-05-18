package com.mhise.model;

import java.io.Serializable;
import java.util.ArrayList;

/** 
*@(#)GetDocumentMetaDataResult.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to getDocumentsMetaData response
* @since 2012-08-10
* @version 1.0 
*/
public class GetDocumentMetaDataResult implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	public  ArrayList<Document>  arrDocDetails;
	public  com.mhise.model.Result result;
	
	public void setDocumentList(ArrayList<Document> arrDocDetails)
	{
		this.arrDocDetails = arrDocDetails;
	}
	
	public void setResult(Result result)
	{
		this.result =result;
	}
	
	
	public ArrayList<Document> getDocument()
	{
		return this.arrDocDetails;
	}
	
	public Result getResult()
	{
		 return this.result;
	}
	
}
