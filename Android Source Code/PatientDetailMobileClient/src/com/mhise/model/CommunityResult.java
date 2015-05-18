package com.mhise.model;

import java.util.ArrayList;


/** 
*@(#)CommunityResult.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to getCommunity method response
* @since 2012-08-10
* @version 1.0 
*/
public class CommunityResult {

	
	private ArrayList<NHINCommunity>  arrCommunity ;
	private com.mhise.model.Result result;
	
	public void setNHINCommunity(ArrayList<NHINCommunity> arrCommunity)
	{
		this.arrCommunity = arrCommunity;
	}
	
	public void setResult(Result result)
	{
		this.result =result;
	}
	
	
	public ArrayList<NHINCommunity> getNHINCommunity()
	{
		return this.arrCommunity;
	}
	
	public Result getResult()
	{
		 return this.result;
	}
}