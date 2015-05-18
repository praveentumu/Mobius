package com.mhise.model;

import java.io.Serializable;

/** 
*@(#)RegisterationResult.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to Registeration Response
* @since 2012-08-10
* @version 1.0 
*/
public class RegisterationResult implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private String certificate;
	
	private com.mhise.model.Result result;
	
	public void setCertificate(String  certificate)
	{
		this.certificate =certificate;
	}
	
	public String getCertificate()
	{
		 return this.certificate;
	}
	
	public void setResult(Result result)
	{
		this.result =result;
	}
	
	public Result getResult()
	{
		 return this.result;
	}
}
