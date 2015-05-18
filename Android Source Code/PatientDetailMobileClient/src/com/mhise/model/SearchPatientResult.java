package com.mhise.model;

import java.io.Serializable;
import java.util.ArrayList;


/** 
*@(#)SearchPatientResult.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to SearchPatient Response
* @since 2012-08-10
* @version 1.0 
*/
public class SearchPatientResult implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private ArrayList<Patient>  arrPatient ;
	private com.mhise.model.Result result;
	
	public void setPatient(ArrayList<Patient> arrPatient)
	{
		this.arrPatient = arrPatient;
	}
	
	public void setResult(Result result)
	{
		this.result =result;
	}
	
	
	public ArrayList<Patient> getPatient()
	{
		return this.arrPatient;
	}
	
	public Result getResult()
	{
		 return this.result;
	}
}
