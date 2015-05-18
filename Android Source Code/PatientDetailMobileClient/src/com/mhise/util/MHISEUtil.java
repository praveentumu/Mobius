package com.mhise.util;

import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.Reader;
import java.io.StringReader;
import java.net.URL;
import java.security.KeyFactory;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.PrivateKey;
import java.security.cert.Certificate;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.security.spec.PKCS8EncodedKeySpec;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Enumeration;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Set;
import java.util.TreeMap;

import javax.net.ssl.HttpsURLConnection;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.apache.http.HeaderElement;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.conn.ssl.X509HostnameVerifier;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.params.HttpProtocolParams;
import org.apache.http.protocol.HTTP;
import org.apache.http.util.EntityUtils;
import org.w3c.dom.Document;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.ParseException;
import android.os.Environment;
import android.util.Base64;
import android.util.Log;

import com.mhise.constants.Constants;
import com.mhise.constants.MobiusDroid;
import com.mhise.model.Address;
import com.mhise.model.City;
import com.mhise.model.CommunityResult;
import com.mhise.model.NHINCommunity;
import com.mhise.model.State;
import com.mhise.model.User;
import com.mhise.security.GenerateCSR;
import com.mhise.security.MHISETrustManager;

/** 
*@(#)MHISEUtil.java 
* @author R Systems
* @description This class contains the  util methods used in application 
* 
* @since 2012-10-26
* @version 1.0 
*/

public class MHISEUtil  {
	
	
	
	 public static int getVersionCode(Context context) {
  	   PackageManager pm = context.getPackageManager();
  	   try {
  	      PackageInfo pi = pm.getPackageInfo(context.getPackageName(), 0);
  	      return pi.versionCode;
  	   } catch (NameNotFoundException ex) {}
  	   return 0;
  	}
	 
	 public static double getVersionName(Context context) {
	  	   PackageManager pm = context.getPackageManager();
	  	   try {
	  	      PackageInfo pi = pm.getPackageInfo(context.getPackageName(), 0);
	  	    
	  	  Log.e("dfsdf",pi.versionName+" "+Double.parseDouble(pi.versionName));
	  	      return Double.parseDouble(pi.versionName);
	  	   } catch (NameNotFoundException ex) {}
	  	   return 0;
	 }
	
	 public static byte[] readBytes(InputStream inputStream) throws IOException {
		  // this dynamically extends to take the bytes you read
		  ByteArrayOutputStream byteBuffer = new ByteArrayOutputStream();

		  // this is storage overwritten on each iteration with bytes
		  int bufferSize = 1024;
		  byte[] buffer = new byte[bufferSize];

		  // we need to know how may bytes were read to write them to the byteBuffer
		  int len = 0;
		  while ((len = inputStream.read(buffer)) != -1) {
		    byteBuffer.write(buffer, 0, len);
		  }

		  // and then we can return your byte array.
		  return byteBuffer.toByteArray();
		}
	 
	 
	 public static boolean saveImportedCertificateToDevice(String certificate,String password, Context ctx,String certName)
	 {
		 boolean isPasswordCorrect=false;
		 
			byte[] certificatebytes =null;
		
			try{
			 certificatebytes = Base64.decode(certificate, Base64.DEFAULT);
			}
			catch (IllegalArgumentException e) {
			// TODO: handle exception
				Logger.debug("MHISEUtil-->saveImportedCertificateToDevice", ""+e);
			}
			KeyStore localTrustStore=null;
			try {
				localTrustStore = KeyStore.getInstance("PKCS12");
			} catch (KeyStoreException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}			
			
			InputStream is = new ByteArrayInputStream(certificatebytes);
			try {
				localTrustStore.load(is,password.toCharArray());
				isPasswordCorrect =true;
				
			} catch (NoSuchAlgorithmException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				return false;
			} catch (CertificateException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				return false;
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				return false;
			}
			
			OutputStream fos=null;
			try {
//<<<<<<< .mine
				//SharedPreferences sharedPreferences = ctx.getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
				//String  storeName =sharedPreferences.getString(Constants.KEY_CERT_NAME, null);
				
				File _mobiusDirectory = new File(Constants.defaultP12StorePath);
				   
				   if(!_mobiusDirectory.exists())
				   {
				      _mobiusDirectory.mkdir();
				   }
				
				File file = new File(Constants.defaultP12StorePath+certName);
				 fos = new FileOutputStream(file);
				//fos = ctx.openFileOutput(Constants.defaultP12StoreName, Context.MODE_PRIVATE);
				localTrustStore.store(fos,MHISEUtil.getStrongPassword(certName).toCharArray());
/*//=======
				//SharedPreferences sharedPreferences = ctx.getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
				//String  storeName =sharedPreferences.getString(Constants.KEY_CERT_NAME, null);
				
				
				File file = new File(Constants.defaultP12StorePath+certName);
				 fos = new FileOutputStream(file);
				//fos = ctx.openFileOutput(Constants.defaultP12StoreName, Context.MODE_PRIVATE);
				localTrustStore.store(fos,MHISEUtil.getStrongPassword(certName).toCharArray());
>>>>>>> .r4477*/
				fos.close();
				
				
				Enumeration<String> aliases =null;
				try {
					aliases = localTrustStore.aliases();
				} catch (KeyStoreException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				
				//boolean isInstalledCertificateValid = false;
				
				while (aliases.hasMoreElements()) {
					
				   String alias =aliases.nextElement();
				   java.security.cert.X509Certificate cert =null;
				try {
					cert = (X509Certificate) 
							localTrustStore.getCertificate(alias);
				} catch (KeyStoreException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

					  SharedPreferences sharedPreferences1 =ctx.getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
	  	    		  SharedPreferences.Editor editor = sharedPreferences1.edit();
	  	    		  
	  	    		  Log.i("Imported certificate serial number", ""+cert.getSerialNumber().toString(16));
	  	    		  editor.putString(Constants.KEY_SERIAL_NUMBER, ""+cert.getSerialNumber().toString(16));
	  	    		  editor.commit();
				
				}
			} catch (FileNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}     

			 catch (KeyStoreException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (NoSuchAlgorithmException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (CertificateException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
		
		 	
			
		 
		return isPasswordCorrect; 
	 }
	 
/*	
	
	public static  void  exportCertificate(FileInputStream fis ,String strPassword ,String path)
	{
		try{
	   		KeyStore localTrustStore = KeyStore.getInstance("PKCS12");		
			char[] password = strPassword.toCharArray(); 	
	    	localTrustStore.load(fis,password);	
	    	 File  fl = new File(path);
			 OutputStream os = new FileOutputStream(fl);
			 localTrustStore.store(os,password);
			  os.close();
			  fis.close();

	   		}
	   		catch ( KeyStoreException e) {
				// TODO: handle exception
			}
	   		catch ( NullPointerException e) {
				// TODO: handle exception
			} catch (FileNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (NoSuchAlgorithmException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (CertificateException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
	}
	
	*/
		 

	 public static HashMap<String,String>  	  initializeEnumTypes ()
	 {
		HashMap<String,String>  providerTpes =  new HashMap<String, String>();
	
		 providerTpes.put("1", "Hospitals");
		 providerTpes.put("2", "HIEs");
		 providerTpes.put("3", "Associations");
		 providerTpes.put("4", "IDNs");
		 providerTpes.put("5", "Labs");
		 providerTpes.put("6", "Clinics");
		 providerTpes.put("7", "Departments");	
		 providerTpes.put("8", "Pharmacies");
		 providerTpes.put("9", "Practice");
		 providerTpes.put("10", "Audiologist");
		 providerTpes.put("11", "Dental_Hygienist");
		 providerTpes.put("12", "Dentist");
		 providerTpes.put("13", "Dietitian");
		 providerTpes.put("14", "Complementary_Healthcare_worker");
		 providerTpes.put("15", "Professional_nurse");
		 providerTpes.put("16", "Optometrist");
		 providerTpes.put("17", "Pharmacist");
		 providerTpes.put("18", "Chiropractor");
		 providerTpes.put("19", "Osteopath");
		 providerTpes.put("20", "Medical_doctor");
		 providerTpes.put("21", "Medical_pathologist");
		 providerTpes.put("22", "Podiatrist");
		 providerTpes.put("23", "Psychiatrist");
		 providerTpes.put("24", "Medical_Assistant");
		 providerTpes.put("25", "Psychologist");
		 providerTpes.put("26", "Social_worker");
		 providerTpes.put("27", "Speech_therapist");
		 providerTpes.put("28", "Medical_Technician");
		 providerTpes.put("29", "Orthotist");		 
		 providerTpes.put("30", "Physiotherapist_AND_OR_occupational_therapist");
		 providerTpes.put("31", "Veterinarian");
		 providerTpes.put("32", "Paramedic_EMT");
		 providerTpes.put("33", "Philologist_translator_AND_OR_interpreter");
		 providerTpes.put("34", "clerical_occupation");
		 providerTpes.put("35", "Administrative_healthcare_staff");
		 providerTpes.put("36", "Infection_control_nurse");
		 providerTpes.put("37", "insurance_specialist_health_insurance_payor");
		 providerTpes.put("38", "Profession_allied_to_medicine_non_licensed_care_giver");
		 providerTpes.put("39", "Public_health_officer");
		 
		 return providerTpes;
 
	 }
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	/*public static boolean checkInvalidCharacters(String string)
	{
	   
		Pattern p = Pattern.compile("[^a-z ]", Pattern.CASE_INSENSITIVE);
		Matcher m = p.matcher(string);
		boolean b = m.find();

		if (b)
		   return true;
		else
			return false ;
	}*/

	public static String  makeNameInUpperCase(String name)
	{
		StringBuffer updatedName = new StringBuffer() ;
		try{
		String[] names=	name.split(Constants.STR_SEPARATOR_USERNAME);		
		Log.e("names length",""+names.length);
			for(int i=0;i<names.length;i++)
			{
				Log.e("names[i].substring(0,1)",""+names[0]);
				updatedName.append(names[i].substring(0,1).toUpperCase()+names[i].substring(1)+" ");
			}
		}
		catch (NullPointerException e) {			
			Logger.error("MHISEUtil -->makeNameInUpperCase","Exception--"+e);
		}
		catch (ArrayIndexOutOfBoundsException e) {
			e.printStackTrace();
		}
		return updatedName.toString();
	}
	
	public static String  makeNameInUpperCase(User user)
	{
		StringBuffer updatedName = new StringBuffer() ;
		
		updatedName.append(user.getPersonName().getGivenName().substring(0,1).toUpperCase()+user.getPersonName().getGivenName().substring(1)+" ");
		updatedName.append(user.getPersonName().getFamilyName().substring(0,1).toUpperCase()+user.getPersonName().getFamilyName().substring(1));
		
		return updatedName.toString();
	}
	
	public static HashMap<String,String>  extractCommunityArray(CommunityResult comResult, Context ctx)
	{		
		ArrayList<NHINCommunity> communities =comResult.getNHINCommunity();
		HashMap< String, String >  hmp = new HashMap<String, String>();
		
		try{
		for(int i=0; i< communities.size();i++)
		{	
			NHINCommunity nhincom =communities.get(i);
			
			if(nhincom.getIsHomeCommunity())
			{
				SharedPreferences sharedPreferences = ctx.getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
			    SharedPreferences.Editor editor = sharedPreferences.edit();
			    editor.putString(Constants.HOME_COMMUNITY_ID,nhincom.getCommunityIdentifier().toString().trim());
			    editor.commit();
			    Log.e("getIsHomeCommunity ","called");
				MobiusDroid.homeCommunity = nhincom;
				MobiusDroid.HomeCommunityID = nhincom.getCommunityIdentifier();
			hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			else
			{
			hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			
		}
		
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Logger.error("MHISEUtil -->extractCommunityArray","Exception--"+e);
		}
		return hmp;
	}
	
	
	
	public static ArrayList[]  extractCommunityArrayList(CommunityResult comResult, Context ctx)
	{		
		ArrayList<NHINCommunity> communities =comResult.getNHINCommunity();
		//HashMap< String, String >  hmp = new HashMap<String, String>();
		ArrayList[] arr = new ArrayList[2];
		ArrayList arr1 = new ArrayList();
		ArrayList arr2 = new ArrayList();
		try{
		for(int i=0; i< communities.size();i++)
		{	
			NHINCommunity nhincom =communities.get(i);
			
			if(nhincom.getIsHomeCommunity())
			{
				SharedPreferences sharedPreferences = ctx.getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
			    SharedPreferences.Editor editor = sharedPreferences.edit();
			    editor.putString(Constants.HOME_COMMUNITY_ID,nhincom.getCommunityIdentifier().toString().trim());
			    editor.commit();
			    
				MobiusDroid.homeCommunity = nhincom;
				MobiusDroid.HomeCommunityID = nhincom.getCommunityIdentifier();
				Log.e("nhincom.getCommunityDescription()",nhincom.getCommunityDescription());
				Log.e("nhincom.getCommunityIdentifier()",nhincom.getCommunityIdentifier());
				arr1.add(nhincom.getCommunityDescription());
				arr2.add(nhincom.getCommunityIdentifier());
			//hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			else
			{
				Log.e("nhincom.getCommunityDescription()",nhincom.getCommunityDescription());
				Log.e("nhincom.getCommunityIdentifier()",nhincom.getCommunityIdentifier());
				arr1.add(nhincom.getCommunityDescription());
				arr2.add(nhincom.getCommunityIdentifier());
			//hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			
		}
		
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Logger.error("MHISEUtil -->extractCommunityArray","Exception--"+e);
		}
		arr[0]=arr1;
		arr[1]=arr2;
		return arr;
	}
	
	public static HashMap<String,String>  extractCommunityArray(CommunityResult comResult)
	{		
		ArrayList<NHINCommunity> communities =comResult.getNHINCommunity();
		HashMap< String, String >  hmp = new HashMap<String, String>();
		
		try{
		for(int i=0; i< communities.size();i++)
		{	
			NHINCommunity nhincom =communities.get(i);
			
			if(nhincom.getIsHomeCommunity())
			{
				MobiusDroid.homeCommunity = nhincom;
				MobiusDroid.HomeCommunityID = nhincom.getCommunityIdentifier();
				hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			else
			{
			hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			
		}
		
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Logger.error("MHISEUtil -->extractCommunityArray","Exception--"+e);
		}
		return hmp;
	}
	
	
	public static ArrayList[] extractCommunityArrayList(CommunityResult comResult)
	{		
		ArrayList<NHINCommunity> communities =comResult.getNHINCommunity();
		//HashMap< String, String >  hmp = new HashMap<String, String>();
		ArrayList[] arr = new ArrayList[2];
		ArrayList arr1 = new ArrayList();
		ArrayList arr2 = new ArrayList();
		try{
		for(int i=0; i< communities.size();i++)
		{	
			NHINCommunity nhincom =communities.get(i);
			
			if(nhincom.getIsHomeCommunity())
			{
				MobiusDroid.homeCommunity = nhincom;
				MobiusDroid.HomeCommunityID = nhincom.getCommunityIdentifier();
				arr1.add(nhincom.getCommunityDescription());
				arr2.add(nhincom.getCommunityIdentifier());
				//hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			else
			{
				arr1.add(nhincom.getCommunityDescription());
				arr2.add(nhincom.getCommunityIdentifier());
			//hmp.put(nhincom.getCommunityDescription(), nhincom.getCommunityIdentifier());
			}
			
		}
		
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Logger.error("MHISEUtil -->extractCommunityArray","Exception--"+e);
		}
		arr[0]=arr1;
		arr[1]=arr2;
		return arr;
	}
	
	
    public static KeyStore loadKeyStore( SharedPreferences sharedPreferences,Context context)
    {	
   	 
   	 KeyStore localTrustStore =null ;
   	 try{
	   	 if (MobiusDroid._keyStore ==null || MobiusDroid._keyStore.equals(null))
	   	 {	
	   		localTrustStore = KeyStore.getInstance("PKCS12");	
	   		
	   		String strStoreName =	sharedPreferences.getString(Constants.KEY_CERT_NAME, null);
	   		File newKeystoreFile = new File(Constants.defaultP12StorePath + strStoreName);
	   		FileInputStream fis = new FileInputStream(newKeystoreFile);
		 	//FileInputStream fis = context.openFileInput(Constants.defaultP12StoreName);
			String strPassword =	MHISEUtil.getStrongPassword(strStoreName);
			Log.i("Password", ""+strPassword);
			char[] password = strPassword.toCharArray(); 	
	    	localTrustStore.load(fis,password);
	    	fis.close();
	    	return localTrustStore ;  	

	   	 }
	   	 else
	   	 {
	   		 Logger.debug("MHISEUtil-->1.1", "1"+MobiusDroid._keyStore);
	   		return MobiusDroid._keyStore; 
	   		
	   	 }	
		   	 
    	}catch (KeyStoreException e) {
			// TODO: handle exception
    		Logger.error("MHISEUtil -->loadKeyStore","Exception--"+e);
   		 	e.printStackTrace();
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			Logger.error("MHISEUtil -->loadKeyStore","Exception--"+e);
			e.printStackTrace();
		} catch (NoSuchAlgorithmException e) {
			// TODO Auto-generated catch block
			Logger.error("MHISEUtil -->loadKeyStore","Exception--"+e);
			e.printStackTrace();
		} catch (CertificateException e) {
			// TODO Auto-generated catch block
			Logger.error("MHISEUtil -->loadKeyStore","Exception--"+e);
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			Logger.error("MHISEUtil -->loadKeyStore","Exception--"+e);
		}
   	
   	 return localTrustStore;
    }
		
    
    public static String getStrongPassword(String emailID)
    {
    	StringBuffer strongPWD =new StringBuffer();
    	try{
    	String[] email =emailID.toUpperCase().split("@");
    	strongPWD.append(email[0]);
    	Log.i("email[0]", ""+email[0]);
    	String[] splitArrayDot = email[1].toString().split("\\.");
    	Log.i("splitArrayDot[0]", ""+splitArrayDot[0]);
    	strongPWD.append(splitArrayDot[0]);
    	}
    	catch (ArrayIndexOutOfBoundsException e) {
			// TODO: handle exception
    	
		}
    	Log.i("generated Strong Password", ""+strongPWD);
    	return strongPWD.toString();
    }
	public static DefaultHttpClient initializeHTTPClient(Context ctx ,KeyStore localTrustStore)
	{
		 DefaultHttpClient httpClient =null;
		 try{
	    	SchemeRegistry schemeRegistry = new SchemeRegistry();
	    	SSLSocketFactory sslSocketFactory = new SSLSocketFactory(localTrustStore,null,getServerKeyStore(Constants.HTTPS_URL_SVC));
	    	sslSocketFactory.setHostnameVerifier((X509HostnameVerifier )SSLSocketFactory.STRICT_HOSTNAME_VERIFIER);
	    	schemeRegistry.register(new Scheme("https", sslSocketFactory, 443));
	    	HttpParams params = new BasicHttpParams();  	
	    	ClientConnectionManager cm = 
	    	    new ThreadSafeClientConnManager(params, schemeRegistry);	
	    	 httpClient = new DefaultHttpClient(cm, params);
	    	     	 
		 }
		 catch (Exception e) {
			// TODO: handle exception
			Logger.debug("MHISEUtil-->initializeHTTPClient -->", ""+e);
		}
		return httpClient;	
	}

	public static String convertStreamToString(InputStream is) {
		
        BufferedReader reader = new BufferedReader(new InputStreamReader(is));
        StringBuilder sb = new StringBuilder();

        String line = null;
        try {
            while ((line = reader.readLine()) != null) {
                sb.append(line + "\n");
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
        	Logger.debug("MHISEUtil-->convertStreamToString -->", "finally clause");
            try {
                is.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        return sb.toString();
    }

	public static String CallWebService(String url,
			String soapAction,
			String envelope ,DefaultHttpClient httpClient)  {
		// request parameters
		HttpParams params = httpClient.getParams();
		HttpConnectionParams.setConnectionTimeout(params, 60000);
		HttpConnectionParams.setSoTimeout(params, 60000);
		// set parameter
		HttpProtocolParams.setUseExpectContinue(httpClient.getParams(), true);
		// POST the envelope
		HttpPost httppost = new HttpPost(url);		
		
		// add headers
		httppost.setHeader("action", soapAction);
		httppost.setHeader("Content-Type", "application/soap+xml;charset=UTF-8;");	
		String responseString="";
		try {
		// the entity holds the request
		HttpEntity entity = new StringEntity(envelope);
		httppost.setEntity(entity);
		
		
		// Response handler
		ResponseHandler<String> rh=new ResponseHandler<String>() {
		// invoked when client receives response
		public String handleResponse(HttpResponse response)
		throws ClientProtocolException, IOException {
				
				//Log.e("CallWebService",getResponseBody(response));
				
			// get response entity
			HttpEntity entity = response.getEntity();
			
			
			// read the response as byte array
			StringBuffer out = new StringBuffer();
			
			byte[] b = EntityUtils.toByteArray(entity);
			String str = new String(b, 0, b.length);
			Log.e("string length",""+str);
			//int index = b.length-1;
			//Log.e("b lase string",""+(char)(b[index-2])+(char)(b[index-1])+(char)(b[index]));
			// write the response byte array to a string buffer
			
			out.append(new String(b, 0, b.length));
			
			return out.toString();
			//return getResponseBody(response);
		}
		};
		try{    	   
			responseString=httpClient.execute(httppost,rh); 
		
		}
		catch (Exception e) {
			Logger.debug("MHISEUtil-->CallWebService -->", "finally clause");
		}
		}
		catch (Exception e) {
			Logger.debug("MHISEUtil-->CallWebService -->", "finally clause");
		}
	
		// close the connection
		httpClient.getConnectionManager().closeExpiredConnections();
		httpClient.getConnectionManager().shutdown();
		Log.e("Response",""+responseString);
		return responseString;
	}

	
 	public final static Document XMLfromString( String xml ) {
 		 
 		
		Document doc = null;
		DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
        try {
    		DocumentBuilder db = dbf.newDocumentBuilder();
    		
    		InputSource is = new InputSource();
    		
	        is.setCharacterStream( new StringReader( xml ) ); 
	        doc = db.parse( is ); 
	       
	        
		} catch (ParserConfigurationException e ) {
			System.out.println("XML parse error: " + e.getMessage());
			Logger.debug("MHISEUtil-->XMLfromString -->", ""+e);
			return null;
		} catch (SAXException e ) {
			System.out.println("Wrong XML file structure: " + e.getMessage());
			Logger.debug("MHISEUtil-->XMLfromString -->", ""+e);
			return null;
		} catch (IOException e) {
			System.out.println("I/O exeption: " + e.getMessage());
			Logger.debug("MHISEUtil-->XMLfromString -->", ""+e);
			return null;
		}
		
	    return doc ;
     }

	public static Dialog displayDialog(Context ctx,String message,String title)
	{
		
		 AlertDialog.Builder alertDialogBuilder=null;
		try{
		if ( !message.equals("") ||!message.equals(null))
		{
	
		 alertDialogBuilder = new AlertDialog.Builder(
                ctx);
		 
		 alertDialogBuilder.setMessage(message);
		 //alertDialogBuilder.setTitle(title);
		 alertDialogBuilder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
			public void onClick(DialogInterface dialog, int which) {
		
			}
			});
			// Showing Alert Message
		 alertDialogBuilder.show();
		 
		}
		}catch (NullPointerException e) {
			// TODO: handle exception
			
			Logger.debug("MHISEUtil-->Unable to display dialog message", "display dialog");
			
		}
		return alertDialogBuilder.create();
	}
	
	public static String convertStringToDateString(String _strDate)
	{
		if(_strDate != null)
		{
			_strDate =_strDate.substring(0,8 );
			String _year = _strDate.substring(0,4);
			String _month = _strDate.substring(4,6);
			String _day = _strDate.substring(6,8);
			return _month+"/"+_day+"/"+_year ;
		}
		else
			return "";
	}
	
	public static String makeAddressStringFromAddress(Address[] address)
	{
		StringBuffer _Address = new StringBuffer() ;
		  City city =null;
          State state =null;
          
          String _cityname = "";
          String _statename = "";
          String _zipCode ="";
		try{
		
		  city = address[0].getCity();
		  Log.e("in city",""+city);
          state =city.getState();
          
          
           _cityname = city.getCityName();
          
           _statename = state.getStateName();
          
          _zipCode =address[0].getZip();
       
		}
		catch (Exception e) {
			// TODO: handle exception
			Logger.debug("MHISEUtil-->makeAddressStringFromAddress -->", ""+e);
		}
		
          if (_cityname != null && !_cityname.equals(""))
          {
        	 _Address.append(_cityname);
        	 	if(_statename !=null && !_statename.equals(""))
        	 	{
        	 		_Address.append(", "+_statename); 
        	 			if (_zipCode != null && !_zipCode.equals("")) {
        	 				_Address.append(", "+_zipCode); 
        	 			}
        	 	}
         
          }
        return _Address.toString();
	}
	
	public static String generateCSR(String e,String cn,String ou
			,String o,String l,String state,String country)
	{
	String key = null;
	try {
		key=GenerateCSR.getInstance().generatePKCS10(e,cn,ou,
				o,l, state, country);
		}
	catch (Exception ex) {
		// TODO Auto-generated catch block
		ex.printStackTrace();
		Logger.debug("MHISEUtil-->generateCSR -->", ""+ex);
		
		}
	return key;
	}
	
/*	public static InputStream readFileFromSDCard(String Filename) {
		
			InputStream ins =null;
			File  file = new File(Environment.getExternalStorageDirectory()+"/"+"MobiusCerificate.p7b");
			if (!file.exists()) {
				throw new RuntimeException("File not found");
			}
			Log.e("Testing", "Starting to read");
			try { 
			   ins =new FileInputStream(file);
			  return ins;
			} catch (Exception e) {
				e.printStackTrace();
			}
			return ins;
		}*/
		  
	public static File addBeginEndCertificateTag(Context context,File certfile ,String filePath) 
	{
		File updatedFile =null;
		java.security.cert.Certificate[] chain = {};
		
		try{
			CertificateFactory certificateFactory =
					CertificateFactory.getInstance("X.509");
			FileInputStream ins =new FileInputStream(certfile);
			chain = certificateFactory.generateCertificates(ins).toArray(chain);
			ins.close();
			
			
			updatedFile = certfile;
		}
		catch (CertificateException e) {
			// TODO: handle exception
			Logger.debug("MHISEUtil-->addBeginEndCertificateTag", " CertificateException "+e);
		try{
			String TAG_BEGIN_CERTIFICATE = "-----BEGIN CERTIFICATE-----\n"; 
			String TAG_END_CERTIFICATE = "\n-----END CERTIFICATE-----"; 
			
			byte[] kb1 = TAG_BEGIN_CERTIFICATE.getBytes();     
			byte[] kb3 = TAG_END_CERTIFICATE.getBytes();
	      
	 
			FileInputStream fis = new FileInputStream(certfile);
	      	int kl = fis.available();
	      	byte[] kb2 = new byte[kl];
	      	fis.read(kb2);
	      	fis.close();     
	   
	      	certfile.delete();  
	        updatedFile = new File(filePath);
	      	FileOutputStream fos = new FileOutputStream(filePath);
	         	
	      	ArrayList< byte[]>  list = new ArrayList<byte[]>();
	      	list.add(kb1);
	      	list.add(kb2);
	      	list.add(kb3);
	      	concatenateByteArrays(list); 	
	      	fos.write(concatenateByteArrays(list));
	    	fos.close();
	    	
	    
		}
		catch (IOException e1) {
	
			Logger.debug("MHISEUtil-->addBeginEndCertificateTag ", "Inside catch ->try "+e1);
			return null;
		}
		
		return updatedFile;
		}
		
		catch (FileNotFoundException e) {
			Logger.debug("MHISEUtil-->addBeginEndCertificateTag ", "FileNotFoundException "+e);
		}
		catch (IOException e) {
			Logger.debug("MHISEUtil-->addBeginEndCertificateTag ", "IOException "+e);
		}
	
	     return updatedFile ;
	   }
	
	public static byte[] concatenateByteArrays(ArrayList<byte[]> blocks) {
	    ByteArrayOutputStream os = new ByteArrayOutputStream();
	    for (byte[] b : blocks) {
	        os.write(b, 0, b.length);
	    }
	    return os.toByteArray();
	}
	
	public static PrivateKey readKey(Context context) 
	      throws Exception {
		 
	    String keyFile ="privateKey.key";
	    FileInputStream fis = context.openFileInput(keyFile);	 
	      	int kl = fis.available();
	      byte[] kb = new byte[kl];
	      	fis.read(kb);
	      	fis.close();
	   		KeyFactory kf = KeyFactory.getInstance("RSA", "BC");
	      PKCS8EncodedKeySpec ks = new PKCS8EncodedKeySpec(kb);
	      PrivateKey pk = kf.generatePrivate(ks );
	   
	     
	  return pk;
	   }
	
	public static void writeKey(PrivateKey pKey,Context context
	         ) throws Exception {
	      
	      String keyFile ="privateKey.key";   
	      FileOutputStream fos = context.openFileOutput(keyFile, Context.MODE_PRIVATE);     
	      byte[] kb = pKey.getEncoded();
	      fos.write(kb);
	      fos.close();
	    
	   }
	
	/**
     *  Returns a list of the file paths containing files with extensions .p12 , .pfx , .p7b 
     *  in both internal and external( SD card) storage
     *
     */
    public static String[] listAllCertificates(){
    	
    	String state =  Environment.getExternalStorageState();
    
    	List<String> locationInter = null;
    	String[] stringArrayInt = null;
    	try {
    	        if  (Environment.MEDIA_MOUNTED.equals(state)) { 
    	        	
    	        	 locationInter = listfiles(Environment.getExternalStorageDirectory() , new ArrayList<String>());
    	        	 stringArrayInt = Arrays.copyOf(locationInter.toArray(), locationInter.toArray().length, String[].class);
    	        } 
    	

    
    	File extSdStorage=new File("/mnt/extSdCard/");
    	
    	//check whether external storage in SD card is available
    	if(extSdStorage.list() != null){
    		
    		List<String> locationExt = listfiles( extSdStorage , new ArrayList<String>());
	    	String[] stringArrayExt = Arrays.copyOf(locationExt.toArray(), locationExt.toArray().length, String[].class);
	    		    	
	    	String interExtFiles[]=new String[stringArrayInt.length + stringArrayExt.length];
	    	
	    	System.arraycopy(stringArrayInt, 0, interExtFiles, 0, stringArrayInt.length);
	    	System.arraycopy(stringArrayExt, 0, interExtFiles, stringArrayInt.length, stringArrayExt.length);
	    	
	    	  
	    	return stringArrayExt;
    	}
    	else{
    		  
    		return stringArrayInt;
    	}   
    	}
    	catch (NullPointerException e) {
			// TODO: handle exception
		Logger.debug("listAllCertificates() -->SD card unavailable","Exception"+e);
		return null ;
    	}
    	
    }
    
    /**
     *  Returns a list of the file path containing file with extensions .p12 , .pfx , .p7b 
     *
     */
    public static List<String> listfiles(File file , List<String> locationList) {

    	try{
        if(file.isDirectory())
        {
     	File list[] = file.listFiles();
         
         for (int i = 0; i < list.length; i++) {
             File temp_file = new File(file.getAbsolutePath(),list[i].getName());
             
             if(/*temp_file.getName().contains(".p7b")
            		 || */temp_file.getName().contains(".p12")
            		 	|| temp_file.getName().contains(".pfx") ){

             locationList.add(temp_file.getAbsolutePath());
             }            

             if (temp_file.isFile() || temp_file.listFiles() != null) {
                 listfiles(temp_file , locationList);
             } 
            }
        }
        }
    	catch (NullPointerException e) {
			// TODO: handle exception
    		Logger.debug("listfiles() -->","NullPointerException"+e);
    	}
		return locationList;
   }
     
    public static String[] showMobiuscertificate(Context context)
    {
        
        String path[] = listAllCertificates();
        String[] stringArray = null;
        try {
        List<String> mobiusCerts = new ArrayList<String>() ;
        Log.e("MHISEUtil-->TAG", "Verify time");
       
        
        for(int i=0;i<path.length;i++){
        	if(  ( path[i].contains(".p12") || path[i].contains(".pfx") ) )
        		{ 
        		mobiusCerts.add(path[i]);
        		}
			}
        stringArray = Arrays.copyOf(mobiusCerts.toArray(), mobiusCerts.toArray().length, String[].class); 
		Log.e("MHISEUtil-->Filtered Certificate count", ""+mobiusCerts.size());
        }
        catch (Exception e) {
			// TODO: handle exception
        	
        	Logger.debug("null pointer at showMobiuscertificate method", ""+e);
        
        	
        }
        return stringArray;
		}

	public static boolean verifyMobiusChain(String path ,Context context){
	    try {
		    CertificateFactory certificateFactory =
		                                    CertificateFactory.getInstance("X.509");
	                                    
		java.security.cert.X509Certificate[] certs={};
		File file =    addBeginEndCertificateTag(context, new File(path), path);
		FileInputStream fis = new FileInputStream(file);
	        certs = (X509Certificate[]) certificateFactory.generateCertificates(fis).toArray(certs);
	        fis.close();
	        boolean flag=false;
	        
	    for(int i=0;i< certs.length ; i++){
	        if(certs[i].getIssuerDN().toString().contains("MobiusCA"))
	        {
	        	flag=true;
	        }
	        else
	        {
	        	flag=false;
	        }
	    }    
	    return flag;
	                                     
	} 
	catch (Exception e) {
	                e.printStackTrace();
	                Logger.debug("MHISEUtil-->TAG","Exception path : "+ path);
		                }
		
		return false;
	}

	/**
	*  Returns a list of the file path containing file with extensions .p12 , .pfx , .p7b 
	*
	*/
    public static void movePKCS12(String path,Context ctx){
       
      //Move PFX or p12 store to Mobius store 
      /*  try {   
            File certificateFile=new File(path);            
            File _mobiusDirectory = new File(Constants.defaultP12StorePath);            
            if(!_mobiusDirectory.exists())
            {
                  _mobiusDirectory.mkdir();
            }            
            File newKeystoreFile = new File(Constants.defaultP12StorePath + Constants.defaultP12StoreName);            
            FileInputStream fin= new FileInputStream(certificateFile);
            FileOutputStream fout= new FileOutputStream(newKeystoreFile);            
            byte buffer[] = new byte[(int) certificateFile.length()];            
            fin.read(buffer);            
            fout.write(buffer);            
            movedPath = newKeystoreFile.getAbsolutePath();

                    
          } catch (FileNotFoundException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
          } catch (IOException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
          }
        */
        
        try{
	        KeyStore keyStore = KeyStore.getInstance("PKCS12");
	        //Get saved password
	        SharedPreferences sharedPreferences =ctx.getSharedPreferences(Constants.PREFS_NAME, Context.MODE_PRIVATE);
			String strPassword =	sharedPreferences.getString(Constants.KEY_PKCS12_PASSWORD, null);
			char[] password = strPassword.toCharArray();
					 
			//Load Selected keystore
			File input = new File(path);
			FileInputStream fin = new FileInputStream(input);
			keyStore.load(fin, password);
			
	        OutputStream fos = ctx.openFileOutput(Constants.defaultP12StoreName, Context.MODE_PRIVATE);     
			keyStore.store(fos,password);
			fos.close();
	        
        }
        catch (KeyStoreException e) {
			// TODO: handle exception
        	Logger.debug("MHISEUtil-->makePKCS12Store", "KeyStoreException: "+e);
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			Logger.debug("MHISEUtil-->makePKCS12Store", "FileNotFoundException: "+e);
			e.printStackTrace();
		} catch (NoSuchAlgorithmException e) {
			// TODO Auto-generated catch block
			Logger.debug("MHISEUtil-->makePKCS12Store", "NoSuchAlgorithmException: "+e);
			e.printStackTrace();
		} catch (CertificateException e) {
			// TODO Auto-generated catch block
			Logger.debug("MHISEUtil-->makePKCS12Store", "CertificateException: "+e);
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			Logger.debug("MHISEUtil-->makePKCS12Store", "IOException: "+e);
			e.printStackTrace();
		}

      }

    public static boolean verifyP12StorePassword(String keyStorePath , String password ,String serialNumber,Context ctx)
	{
    	boolean isInstalledCertificateValid = false;
    	KeyStore trustStore =null;
    	FileInputStream fin =null;		
		try {
			trustStore = KeyStore.getInstance("PKCS12");
		} catch (KeyStoreException e2) {
			// TODO Auto-generated catch block
			e2.printStackTrace();
		}
		
				
			File file = new File(keyStorePath);    	
    		if(file.exists())
    		{    		
			
				try {
					fin = new FileInputStream(file);
				} catch (FileNotFoundException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				
				try {
					trustStore.load(fin, password.toCharArray());
					fin.close();
				} catch (NoSuchAlgorithmException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					MHISEUtil.displayDialog(ctx, "Invalid Password", null);
				} catch (CertificateException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					MHISEUtil.displayDialog(ctx, "Invalid Password", null);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
					MHISEUtil.displayDialog(ctx, "Invalid Password", null);
				}
			
				Enumeration<String> aliases =null;
				try {
					aliases = trustStore.aliases();
				} catch (KeyStoreException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			try{
				
				while (aliases.hasMoreElements()) {	
				   String alias =aliases.nextElement();
				   java.security.cert.X509Certificate cert =null;
					try {
						cert = (X509Certificate) 
								trustStore.getCertificate(alias);
					} catch (KeyStoreException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					} 
				  if (cert.getSerialNumber().toString().equals(serialNumber))
				  {
					 // isInstalledCertificateValid = true; 
					 SharedPreferences sharedPreferences = ctx.getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
  	    		    SharedPreferences.Editor editor = sharedPreferences.edit();

  	    		    editor.putString(Constants.KEY_SERIAL_NUMBER, ""+cert.getSerialNumber().toString(16));
  	    		    editor.commit();
					  
					  
					  return true ;
				  }
				}
			}
			catch (NullPointerException e) {
				// TODO: handle exception
				Logger.debug("password invalid", ""+e);
			}
    		}
				
				
				
    	
    			
    	
		return isInstalledCertificateValid;
	}
    
    
    
    
    public static KeyStore getServerKeyStore(String url){
    	 KeyStore ks = null ;
    	 try{
	        MHISETrustManager.allowAllSSL();	  
	        HttpsURLConnection connection = (HttpsURLConnection) (new URL(url))
	        .openConnection();
	       
	        connection.connect();
	          
		    Certificate[] certs = connection.getServerCertificates();
		    ks = KeyStore.getInstance(KeyStore.getDefaultType());
		    ks.load(null, null);
		    ks.setCertificateEntry("servercert", certs[0]);  
		    Log.i("MHISEUtil-->getServerKeyStore", certs[0].getPublicKey().toString());	    
    	 	}
    	 catch (Exception e) {	        
	      
    		 Logger.debug("MHISEUtil-->getServerKeyStore","Exception"+e);
	        e.printStackTrace();
    	 }
	   return ks;
    	}
    
    
    public static String getResponseBody(HttpResponse response) {
    	 
    	String response_text = null;
    	 
    	HttpEntity entity = null;
    	 
    	try {
    	 
    	entity = response.getEntity();
    	 
    	response_text = _getResponseBody(entity);
    	 
    	} catch (ParseException e) {
    	 
    	e.printStackTrace();
    	 
    	} catch (IOException e) {
    	 
    	if (entity != null) {
    	 
    	try {
    	 
    	entity.consumeContent();
    	 
    	} catch (IOException e1) {
    	 
    	}
    	 
    	}
    	 
    	}
    	 
    	return response_text;
    	 
    	}
    
    
    public static String _getResponseBody(final HttpEntity entity) throws IOException, ParseException {
    	 
    	if (entity == null) { throw new IllegalArgumentException("HTTP entity may not be null"); }
    	 
    	InputStream instream = entity.getContent();
    	 
    	if (instream == null) { return ""; }
    	 
    	if (entity.getContentLength() > Integer.MAX_VALUE) { throw new IllegalArgumentException(
    	 
    	"HTTP entity too large to be buffered in memory"); }
    	 
    	String charset = getContentCharSet(entity);
    	 
    	if (charset == null) {
    	 
    	charset = HTTP.DEFAULT_CONTENT_CHARSET;
    	 
    	}
    	 
    	Reader reader = new InputStreamReader(instream, charset);
    	 
    	StringBuilder buffer = new StringBuilder();
    	 
    	try {
    	 
    	char[] tmp = new char[1024];
    	 
    	int l;
    	 
    	while ((l = reader.read(tmp)) != -1) {
    	 
    	buffer.append(tmp, 0, l);
    	 
    	}
    	 
    	} finally {
    	 
    	reader.close();
    	 
    	}
    	 
    	return buffer.toString();
    	 
    	}
    
    
    public static String getContentCharSet(final HttpEntity entity) throws ParseException {
    	 
    	if (entity == null) { throw new IllegalArgumentException("HTTP entity may not be null"); }
    	 
    	String charset = null;
    	 
    	if (entity.getContentType() != null) {
    	 
    	HeaderElement values[] = entity.getContentType().getElements();
    	 
    	if (values.length > 0) {
    	 
    	NameValuePair param = values[0].getParameterByName("charset");
    	 
    	if (param != null) {
    	 
    	charset = param.getValue();
    	 
    	}
    	 
    	}
    	 
    	}
    	 
    	return charset;
    	 
    	}

}
