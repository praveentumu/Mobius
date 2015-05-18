package com.mhise.model;

import java.io.Serializable;

/** 
*@(#)User.java 
* @author R Systems
* @description This class contains All the getter and setter methods related to User
* @since 2012-08-10
* @version 1.0 
*/
public class User implements Serializable{

	private   String CommunityId;
	private   String ID;
	private   String IsOptIn;
	private   String MPIID;
	private   String Name;
	private   String publicKey;
	private   String email;
	private   String Role;
	private   String UserType;
	private   Result result;
	private PersonName personName;
	
	
	public PersonName getPersonName() {
		return personName;
	}
	public void setPersonName(PersonName personName) {
		this.personName = personName;
	}
	public  String getEmail()
	{
		 return this.email;
	}
	public  void setEmail(String email)
	{
		 this.email=email;
	}
	
	
	public  String getCommunityId()
	{
		 return this.CommunityId;
	}
	public  String getId()
	{
		 return this.ID;
	}
	public  String IsOptInId()
	{
		 return this.IsOptIn;
	}
	public  String getMPIID()
	{
		 return this.MPIID;
	}
	public  String getPublicKey()
	{
		 return this.publicKey;
	}
	
	public  String getName()
	{
		 return this.Name;
	}
	public  String getRole()
	{
		 return this.Role;
	}
	public  String getUserType()
	{
		 return this.UserType;
	}
	public  Result getResult()
	{
		 return this.result;
	}
	
	/*Set */
	
	public  void setCommunityId(String CommunityId)
	{
		  this.CommunityId = CommunityId;
	}
	
	public  void setID(String ID)
	{
		  this.ID = ID;
	}
	public  void setIsOptIn(String IsOptIn)
	{
		  this.IsOptIn = IsOptIn;
	}
	public  void setMPIID(String MPIID)
	{
		  this.MPIID = MPIID;
	}
	public  void setpublicKey(String publicKey)
	{
		  this.publicKey = publicKey;
	}
	public  void setRole(String Role)
	{
		  this.Role = Role;
	}
	public void setResult(Result result)
	{
		  this.result = result;
	}
	public void set(Result result)
	{
		  this.result = result;
	}

	public void setName(String Name)
	{
		  this.Name = Name;
	}

	public void setUserType(String UserType)
	{
		  this.UserType = UserType;
	}
	
	
}
