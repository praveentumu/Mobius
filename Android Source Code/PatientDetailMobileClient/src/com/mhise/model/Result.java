package com.mhise.model;

import java.io.Serializable;


/** 
*@(#)Result.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Result response
* @since 2012-08-10
* @version 1.0 
*/
public class Result implements Serializable{


	/**
	 * 
	 */
	private static final long serialVersionUID = -821818204042920523L;
	public	String ErrorCode;
	public  String ErrorMessage;		
	public String IsSuccess;
	
	public void setErrorCode(String ErrorCode)
	{
		this.ErrorCode = ErrorCode;
	}
	
	public void setErrorMessage(String ErrorMessage)
	{
		this.ErrorMessage = ErrorMessage;
	}
	
	public void setIsSuccess(String IsSuccess)
	{
		this.IsSuccess = IsSuccess;
	}
	
	
	public String getErrorCode()
	{
		return this.ErrorCode;
	}
	public String getErrorMessage()
	{
		return this.ErrorMessage;
	}
	public String getIsSuccess()
	{
		return this.IsSuccess;
	}
}
