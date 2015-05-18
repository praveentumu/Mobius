package com.mhise.ui;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;

import org.apache.http.impl.client.DefaultHttpClient;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.support.v4.app.FragmentActivity;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.mhise.constants.Constants;
import com.mhise.model.Result;
import com.mhise.requests.RequestBase;
import com.mhise.response.BaseParser;
import com.mhise.response.GetMasterDataParser;
import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;


public class BaseMenuOptionsAcitivity extends FragmentActivity{
	
	GetPWDChangeAsyncClass getPWDChangeAsyncClass;
	AddCertificateAsyncClass addCertificateAsyncClass;
	 AlertDialog  passwordDialog;
	 TextView error;
	   @Override
	    public boolean onCreateOptionsMenu(Menu menu) {
	    		 
	    	 new MenuInflater(this).inflate(R.menu.mobiusdroidappmenu, menu);
	    	 return super.onCreateOptionsMenu(menu);
	   
	    }
	   

	   	@Override
	  	public boolean onOptionsItemSelected(MenuItem item) {
	   		
	   	
	   	 LayoutInflater inflater = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	   		switch (item.getItemId()) {
	   		
	   		
			case R.id.menu_logout:
				
				/*SharedPreferences	appSharedPrefs=getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
				Editor edt =appSharedPrefs.edit();
				edt.putString("EmailID", "");
				edt.putString("Password","");
				edt.putBoolean("checkRemember", false);
				edt.putInt("UserTypeID",R.id.radioPatient);
				edt.commit();*/
				   Intent calllogin = new Intent(BaseMenuOptionsAcitivity.this,UserLogin.class);
				   calllogin.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
				   calllogin.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
				  // calllogin.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
				   
				  // calllogin.setFlags( Intent.FLAG_ACTIVITY_CLEAR_TOP|Intent.FLAG_ACTIVITY_SINGLE_TOP);
				   
				   startActivity(calllogin);		 
				   return super.onOptionsItemSelected(item);
				
			case R.id.menu_change_password:
				
				
		         final View changePWDlayout = inflater.inflate(R.layout.changepassword, (ViewGroup) findViewById(R.id.root));
		          AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
		          builder1.setTitle(getResources().getString(R.string.settings_password1));
		          builder1.setView(changePWDlayout); 
		          changePassword(changePWDlayout,builder1,this) ;
				  return super.onOptionsItemSelected(item);
				
			
				
			case R.id.menu_export_option:
				
				/*Implementation for exporting certificat to device sd card*/
				
		         final View exportlayout = inflater.inflate(R.layout.dialog_view, (ViewGroup) findViewById(R.id.root));
		          AlertDialog.Builder builder2 = new AlertDialog.Builder(this);
		          builder2.setTitle(getResources().getString(R.string.allowotherdevices));
		          builder2.setView(exportlayout); 
		          exportCertificate(exportlayout,builder2,this) ;
				  return super.onOptionsItemSelected(item);
				
				/*Implemntation to add certificate to server database*/
				
			
	    	
				
			
		/*	case R.id.menu_import:
				 	AlertDialog.Builder importbuilder = new AlertDialog.Builder(this);
				 	importbuilder.setTitle(getResources().getString(R.string.importHelpTitle));
				 	importbuilder.setMessage(getResources().getString(R.string.importHelpMessage));
				 	importbuilder.create().show();
				  return super.onOptionsItemSelected(item);*/

			default:
				return false;
   		
	   		}
	  }
	
	 
	   	public  void exportCertificate(final View layout , AlertDialog.Builder builder,final Context appConetxt ) {
	   		
	   		final AlertDialog  passwordDialog =builder.create();
	   		
	   		passwordDialog.show();
	  		Button btnOK = (Button) layout.findViewById(R.id.buttonok);
	  		Button btnCancel = (Button) layout.findViewById(R.id.buttonCancel);
	  		final EditText password1 = (EditText) layout.findViewById(R.id.EditText_Pwd1);
          // final EditText password2 = (EditText) layout.findViewById(R.id.EditText_Pwd2);
            final TextView error =     (TextView) layout.findViewById(R.id.TextView_PwdProblem);

	  		btnCancel.setOnClickListener(new View.OnClickListener() {
				
				@Override
				public void onClick(View v) {
					// TODO Auto-generated method stub
					//passwordDialog.dismiss();
					
					password1.setText(null);
					//password2.setText(null);
   					
   					error.setText(null);
   					
   					error.setVisibility(View.GONE);
   					passwordDialog.dismiss();
				}
			});
	  		
	  		btnOK.setOnClickListener(new View.OnClickListener() {
				
				@Override
				public void onClick(View v) {
			
				/*Implementation for exporting certificat to device sd card*/			    	 
               	 String strPassword1 = password1.getText().toString();
               	 if(strPassword1 !=null)
               	 {
               		 if(strPassword1.length()>0)
               		 {
                   		try{
                   			
                   			if(strPassword1.contains(" "))
							{
                   				error.setVisibility(View.VISIBLE);
                   				error.setText(getResources().getString(R.string.whitespaeinPassword));
							}
                   			else
                   			{
                   	   		KeyStore localTrustStore = KeyStore.getInstance("PKCS12");	
                   		 //	FileInputStream fis = appConetxt.openFileInput(Constants.defaultP12StoreName);
                   			
                   	   	SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);  
                   		 	
                   		 String strStoreName = sharedPreferences.getString(Constants.KEY_CERT_NAME, null);
             	   		File newKeystoreFile = new File(Constants.defaultP12StorePath + strStoreName);
             	   		FileInputStream fis = new FileInputStream(newKeystoreFile);
             		 	//FileInputStream fis = context.openFileInput(Constants.defaultP12StoreName);
             			String strPassword =	MHISEUtil.getStrongPassword(strStoreName);
                   		 	
                   		 //SharedPreferences sharedPreferences = appConetxt.getSharedPreferences(Constants.PREFS_NAME, Context.MODE_PRIVATE);
                   		 
                   		 Log.i("Password", ""+strPassword);
                   		//	char[] password = strPassword.toCharArray(); 	
                   	    	localTrustStore.load(fis,strPassword.toCharArray());
                   	    	
                   	    	 File  fl = new File(Constants.defaultP12StorePath + Constants.defaultP12StoreName);
                   			 OutputStream os = new FileOutputStream(fl);   
                   			 InputStream is = new FileInputStream(fl);
                   			 Log.i("password--->",""+strPassword1);
                   			 
                   			 localTrustStore.store(os,strPassword1.toCharArray());
                   			  os.close();
                   			  fis.close();
                   			  passwordDialog.dismiss();
                   			  
                   		byte[] bytes = MHISEUtil.readBytes(is);
                   			//String certificate = new String(bytes);
							
                   		
                   		String certificate = android.util.Base64.encodeToString(bytes,android.util.Base64.DEFAULT);
                   		
                   		//String certificate =new String(Base64.encode(bytes));
                   	//	SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
						String userType = sharedPreferences.getString(Constants.KEY_USER_TYPE, null);
						String userEmail = sharedPreferences.getString(Constants.KEY_USER_EMAIL, null);
						String request =	RequestBase.getAddPFXCertificaterequest(certificate, userEmail, userType);
		
					//	Log.i("Encoded certificate",""+ android.util.Base64.encodeToString(bytes,android.util.Base64.DEFAULT));
					//	Log.i("Decoded certificate",""+ android.util.Base64.decode(certificate, android.util.Base64.DEFAULT));
						
						//showDialog(Constants.GET_COMMUNITY_PROGRESS_DIALOG);
						addCertificateAsyncClass = new AddCertificateAsyncClass();
			    		addCertificateAsyncClass.execute(request);
                   		 
                   		 
                   			Toast.makeText(appConetxt,
									   appConetxt.getResources().getString(R.string.export_message),
									     Toast.LENGTH_LONG).show();
                   			}
                   	   		
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
               		 else
               		 {
               			 error.setVisibility(View.VISIBLE);
               			error.setText(getResources().getString(R.string.securityPasswordEmpty));
               		 }
               	 }
               	 else
               	 {
               		 error.setVisibility(View.VISIBLE);
             		error.setText(getResources().getString(R.string.securityPasswordEmpty));
               	 }
               }
 
			});

		        
	   	}
	   	
	   	public static String convertStreamToString(InputStream is) throws Exception {
	   	    BufferedReader reader = new BufferedReader(new InputStreamReader(is));
	   	    StringBuilder sb = new StringBuilder();
	   	    String line = null;

	   	    while ((line = reader.readLine()) != null) {
	   	        sb.append(line);
	   	    }

	   	    is.close();

	   	    return sb.toString();
	   	}
	            
		public void changePassword(final View layout , AlertDialog.Builder builder,final Context appConetxt)
		{
			passwordDialog =builder.create();
	
	   		passwordDialog.show();
	  		Button btnOK = (Button) layout.findViewById(R.id.buttonok);
	  		Button btnCancel = (Button) layout.findViewById(R.id.buttonCancel);
	  		final EditText passwordOld = (EditText) layout.findViewById(R.id.EditText_OLDPwd);
            final EditText passwordNew = (EditText) layout.findViewById(R.id.EditText_NewPwd);
            final EditText passwordConfirm = (EditText) layout.findViewById(R.id.EditText_ConfirmPwd);
            error =     (TextView) layout.findViewById(R.id.TextView_PwdProblem);
   	
			
            btnOK.setOnClickListener(new OnClickListener() {
				
				@Override
				public void onClick(View v) {
					try {
						String oldPWD =passwordOld.getText().toString();
						String newPWD =passwordNew.getText().toString();
						String confirmPWD=passwordConfirm.getText().toString();
						if(oldPWD.equals(null)||oldPWD.equals("") )
						{
							error.setVisibility(View.VISIBLE);
							error.setText(getResources().getString(R.string.old_password_empty));
						}
						else {
							if(newPWD.equals(confirmPWD)&& newPWD.length()>0)
							{
								SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
								String userType = sharedPreferences.getString(Constants.KEY_USER_TYPE, null);
								String userEmail = sharedPreferences.getString(Constants.KEY_USER_EMAIL, null);
								String request =	RequestBase.getChangePasswordRequest(userEmail ,userType,oldPWD,newPWD);
				
								//showDialog(Constants.GET_COMMUNITY_PROGRESS_DIALOG);
					    		getPWDChangeAsyncClass = new GetPWDChangeAsyncClass();
					    		getPWDChangeAsyncClass.execute(request);
								
							}
							else {
								try{
								if (newPWD.length()<1)
								{
									error.setVisibility(View.VISIBLE);
									error.setText(getResources().getString(R.string.newPasswordEmpty));
								
								}
								else{
								error.setVisibility(View.VISIBLE);
								error.setText(getResources().getString(R.string.password_not_match));
								passwordOld.setText(null);
								passwordNew.setText(null);
								passwordConfirm.setText(null);
								}
							}
								catch (NullPointerException e) {
									// TODO: handle exception
									error.setVisibility(View.VISIBLE);
									error.setText(getResources().getString(R.string.newPasswordEmpty));
							
								}
						}
						
						}
					}catch (NullPointerException e) {
						// TODO: handle exception
						Logger.debug("BaseMenuOptionsAcitivity->changePassword", ""+e);
					}
					
					
				}
			});
            
            btnCancel.setOnClickListener(new OnClickListener() {
				
       				@Override
       				public void onClick(View v) {
       					passwordConfirm.setText(null);
       					passwordNew.setText(null);
       					passwordOld.setText(null);
       					error.setText(null);
       					error.setVisibility(View.GONE);
       					passwordDialog.dismiss();
       				}
       			});
            
            
		}
	   
			
		private class GetPWDChangeAsyncClass extends AsyncTask<String, Void, String>
		 {
			 	@Override
			 	protected String doInBackground(String... params) 
			 	{       
				SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);  
			 	DefaultHttpClient httpClient = MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));
				String request = params[0];	
				String response = com.mhise.util.MHISEUtil.CallWebService(
							Constants.HTTPS_URL, XmlConstants.ACTION_CHANGE_PASSWORD, request,httpClient );
				
				return response;	
			 	}	

				 @Override
				protected void onPostExecute(String result) {
					 
				super.onPostExecute(result);	

				try{
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{
						Result  changePWDResult =	new GetMasterDataParser().parseChangePWD(resultDoc);
						
						if (changePWDResult.IsSuccess.equals("true"))
						{
							passwordDialog.dismiss();
							Toast.makeText(getApplicationContext(),
								   changePWDResult.ErrorMessage,
								     Toast.LENGTH_LONG).show();
							error.setVisibility(View.INVISIBLE);
						}
						else
						{
							error.setVisibility(View.VISIBLE);
							 error.setText(changePWDResult.ErrorMessage);
						}
						//MHISEUtil.displayDialog(BaseMenuOptionsAcitivity.this, changePWDResult.ErrorMessage, "");	
					}
					 		
				}
				catch (Exception e) {
					// TODO: handle exception										
				 	Logger.debug("BaseMenuOptionsActivity-->onPostExecute Response",""+e);
				}
				 }
		 }
	    
	    
		private class AddCertificateAsyncClass extends AsyncTask<String, Void, String>
		 {
			 	@Override
			 	protected String doInBackground(String... params) 
			 	{       
			 		SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);  
			
			 	DefaultHttpClient httpClient = MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));
				String request = params[0];	
				String response = com.mhise.util.MHISEUtil.CallWebService(
							Constants.HTTPS_URL, XmlConstants.ACTION_ADD_PFX_CERTIFICATE, request,httpClient );
				
				return response;	
			 	}	

				 @Override
				protected void onPostExecute(String result) {
					 
				super.onPostExecute(result);	

				try{
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{
						Log.e("BaseMenuOptionsActivity ->AddCertificateAsyncClass-->onPostExecute ", ""+result);
						
						NodeList list = resultDoc.getElementsByTagName(com.mhise.constants.Constants.TAG_RESULT);
						Result result1 =null;
						for(int i=0;i<list.getLength();i++)
						{
						Node 	nodeResponse = list.item(i);
						Log.i("list item",""+list.item(i).getNodeName());
						Log.i("list item",""+list.item(i).getTextContent());
						
						if(nodeResponse.getNodeName().equals(Constants.TAG_RESULT))
						{
							 result1= new com.mhise.model.Result();
							BaseParser.setResult(nodeResponse, result1);
							
						}
						}

						if (result1.IsSuccess.equals("true"))
						{
						/*	Toast.makeText(getApplicationContext(),
								   result1.ErrorMessage,
								     Toast.LENGTH_LONG).show();
							error.setVisibility(View.GONE);*/
						}
						else
						{
							error.setVisibility(View.VISIBLE);
							 error.setText(result1.ErrorMessage);
						}
						//MHISEUtil.displayDialog(BaseMenuOptionsAcitivity.this, changePWDResult.ErrorMessage, "");	
				
					}
					 		
				}
				catch (Exception e) {
					// TODO: handle exception										
				 	Logger.debug("BaseMenuOptionsActivity-->onPostExecute Response",""+e);
				}
				 }
		 }
	     
	    
	    
	    
	    
	    
}
