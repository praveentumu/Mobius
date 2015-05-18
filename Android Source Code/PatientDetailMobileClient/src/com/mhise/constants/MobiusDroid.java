package com.mhise.constants;

import java.security.KeyStore;
import java.security.PrivateKey;
import java.util.HashMap;

import com.mhise.model.NHINCommunity;

import android.app.Application;



/** 
*@(#)MobiusDroid.java 
* @author R Systems
* @description This class contains the Application constants used in the MobiusDroid application
* @since 2012-08-10
* @version 1.0 
*/

public class MobiusDroid extends Application{

	/* Objects in Application Context */

	public static String[] _arrComunities ;
	public static String[] _arrPHIComunities ;
	public static HashMap<String, String> hmp_CommunityID;
	public static HashMap<String, String> hmp_PHICommunityID;
	public static String[] _arrPurpose;
	public static String[] _arrSpeciality;
	public static String[] _arrSpecialityCode;
    public static  PrivateKey _pKey; 
    public static String _aliasName ;
    public static KeyStore _keyStore =null ;
	public static String HomeCommunityID=null;
	public static NHINCommunity homeCommunity = null;
	

/*  private String userID ;
    private String userType ;
    private String userRole ;
    
    
    public void setUserID(String userID)
    {
    	this.userID = userID;
    }
    public void setUserType(String userType)
    {
    	this.userType = userType;
    }
    public void setUserRole(String useerRole)
    {
    	this.userRole = useerRole;
    }
    
    public String getUserID()
    {
    	return this.userID;
    }
    public String getUserRole()
    {
    	return this.userRole;
    }
    public String getUserType()
    {
    	return this.userType;
    }
  */
    
}
