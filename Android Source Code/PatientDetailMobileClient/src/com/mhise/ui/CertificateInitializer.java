package com.mhise.ui;

import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.PrivateKey;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.util.Enumeration;
import org.apache.http.impl.client.DefaultHttpClient;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnCancelListener;
import android.content.DialogInterface.OnClickListener;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.TextView;
import com.mhise.constants.Constants;
import com.mhise.model.User;
import com.mhise.response.BaseParser;
import com.mhise.util.DataConnectionManager;
import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;

 
public class CertificateInitializer extends Activity 
			//implements    KeyChainAliasCallback 
{
	
	private String []loc =null;
	public static String selectedPath ;
	private  static File  fileToInstall ;
	
	/*Certificate Types*/
	//private static  final String certTypeP7B ="p7b";
	private static  final String certTypePFX ="pfx";
	private static  final String certTypeP12 ="p12";
	
	/*Dialog  ID*/
	private static  final short  GET_USER_INFORMATION_PROGRESS_DIALOG =0;
 	
	private GetUserType getUsertypeObj;
 	Context context;
	SharedPreferences prefs;
	PrivateKey pKey;
	public static String TAG_BEGIN_CERTIFICATE = "-----BEGIN CERTIFICATE-----\n"; 
	public static String TAG_END_CERTIFICATE = "\n-----END CERTIFICATE-----"; 
    /** Called when the activity is first created. */
	
   @Override
   	public void onCreate(Bundle savedInstanceState) {
  
    	super.onCreate(savedInstanceState);
    	context = getApplicationContext();
    	Intent intent =getIntent();
    	
    	try{
    		String certificate=intent.getStringExtra(Constants.TAG_CERTIFICATE);
    		if(certificate != null)
    		{
    			if(certificate.length()>0)
    			{
    				pKey =(PrivateKey) intent.getSerializableExtra(Constants.TAG_PRIVATEKEY);
    				StringBuffer certificateBuffer = new StringBuffer();
    	    		certificateBuffer.append(TAG_BEGIN_CERTIFICATE);
    	    		certificateBuffer.append(certificate);
    	    		certificateBuffer.append(TAG_END_CERTIFICATE);
    	    		
    	    		SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME, Context.MODE_PRIVATE);
    				
    				String strCertificateName=sharedPreferences.getString(Constants.KEY_CERT_NAME, null);
    				String strPassword =MHISEUtil.getStrongPassword(strCertificateName);
    	    		installP7BCertificate(certificateBuffer.toString(),pKey,strPassword,strCertificateName);

    			}
    			
    		}
    		else
    		{
    			   //Logger.debug("called via intent", "true");
    		    	showDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
    		    	handleCertificateInstallation();
    			
    		}
    	}
    	catch (NullPointerException e) {
			// TODO: handle exception
    		Logger.debug("CertificateInitializer -->onCreate -->", ""+e);
		}
    	
    	 
 
    	
    }
        
   	private  KeyStore  makePKCS12Store( File file)
   	{	 
  	 try{
  		 
		 File _mobiusDirectory = new File(Constants.defaultP12StorePath);
		   
		   if(!_mobiusDirectory.exists())
		   {
		       _mobiusDirectory.mkdir();
		   }
		   
		   PrivateKey pKey =MHISEUtil.readKey(getApplicationContext());
		   KeyStore keyStore = KeyStore.getInstance("PKCS12");
		 //  Logger.debug("keyStore", "true"+keyStore);
			
		   	showDialog(Constants.PASSWORD_DIALOG);	
			SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME, Context.MODE_PRIVATE);
			String strPassword =	sharedPreferences.getString(Constants.KEY_PKCS12_PASSWORD, null);
			char[] password = strPassword.toCharArray();
			//Logger.debug("CertificateInitializer -> makePKCS12Store", "Stored Password: "+strPassword);
			keyStore.load(null, password); 
			
			CertificateFactory certificateFactory =
			CertificateFactory.getInstance("X.509");
			java.security.cert.Certificate[] chain = {};
			String filePath = Constants.defaultP12StorePath+Constants.defaultP12StoreName;
			File updatedFile =	MHISEUtil.addBeginEndCertificateTag(this ,file,filePath);
			FileInputStream ins1 =new FileInputStream(updatedFile);			
			chain = certificateFactory.generateCertificates(ins1).toArray(chain);
			ins1.close();
			keyStore.setKeyEntry("pkcs12", pKey,  password ,chain  ); 
			
			//Store KeyStore in external storage
			 File  fl = new File(Constants.defaultP12StorePath + Constants.defaultP12StoreName);
			 OutputStream os = new FileOutputStream(fl);
			 keyStore.store(os,password);
			  os.close();
			 
			 // Store keyStore in internal Storage
			OutputStream fos = getApplicationContext().openFileOutput(Constants.defaultP12StoreName, Context.MODE_PRIVATE);     
			keyStore.store(fos,password);
			fos.close();
    
    	   
  }
      catch (Exception e) {
		// TODO: handle exception
		Logger.debug("CertificateInitializer-> makePKCS12Store ", ""+e);
      }
  	return null;
   }
     
   	public static boolean isInstalled(Context context)
    {
   	 
   		SharedPreferences sharedPreferences = context.getSharedPreferences(Constants.PREFS_NAME, Context.MODE_PRIVATE);
   		Log.e("sharedPreferences",""+sharedPreferences);
   		String certName =	sharedPreferences.getString(Constants.KEY_CERT_NAME, null);
   		Log.e("certName",""+certName);
   		File newKeystoreFile = new File(Constants.defaultP12StorePath+certName);
	       if(  newKeystoreFile.exists())	        
	    	   return true ;	        
	       else
	    	 return false;  
	    
			/*fis = context.openFileInput(newKeystoreFile);
			if (fis.available()>0)
			   {
					fis.close();
			 		return true ;
			 		
			 	}
			 	else
			 	{   fis.close();
			 		return false;
			 	}*/
	
    }
 
   	public void  handleCertificateInstallation()
     {
    	boolean isInstalled = isInstalled(this);
    	 	if(isInstalled)
	        {
    	 		//Logger.debug("CertificateInitializer->handleCertificateInstallation","isInstalled"+isInstalled);
    	 		// check User type
				removeDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
	
				boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
				  if(!isDataConnectionAvailable)
		  		{
		  		MHISEUtil.displayDialog(CertificateInitializer.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
		  		}
		  		else
		  		{	
		  			showDialog(GET_USER_INFORMATION_PROGRESS_DIALOG);
		  			String[] params ={com.mhise.requests.RequestBase.getUserInformation()};
		  			getUsertypeObj = new GetUserType();
		  			getUsertypeObj.execute(params);
		  		}

	        }
	        else
	        {
	        	//Logger.debug("CertificateInitializer->handleCertificateInstallation","isInstalled"+isInstalled);	      
	        	
	        	loc = MHISEUtil.showMobiuscertificate(getApplicationContext()); 
	        	if (loc != null)
	        	showDialog(Constants.SELECT_CERTIFICATE);
	        	else
	        	{
	        				removeDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
	            		  	Logger.debug("CertificateInitializer->SELECT_CERTIFICATE-->No certificate available ", "true");
		      	    	  	Intent callRegisterScreen = new Intent(CertificateInitializer.this,Register.class);
		      	    	  	startActivity(callRegisterScreen);
		      	    	  	CertificateInitializer.this.finish();
  
	        	}
	        	//Logger.debug("loc ",""+loc);
	        	
	        }
    	 
     }
    
   @Override
   	public Dialog onCreateDialog(int id) {
	        switch (id) {
	        
            case Constants.PASSWORD_DIALOG:
            	
            LayoutInflater inflater = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);
              final View layout = inflater.inflate(R.layout.dialog_view, (ViewGroup) findViewById(R.id.root));
              AlertDialog.Builder builder = new AlertDialog.Builder(this);
              
              builder.setTitle(getResources().getString(R.string.settings_password1));
              builder.setView(layout);
        
               Dialog password_dialog  =  builder.setPositiveButton(android.R.string.ok, new DialogInterface.OnClickListener() {
                     public void onClick(DialogInterface dialog, int which) {
                    	 final EditText password1 = (EditText) layout.findViewById(R.id.EditText_Pwd1);
                     //    final EditText password2 = (EditText) layout.findViewById(R.id.EditText_Pwd2);
                         final TextView error = (TextView) layout.findViewById(R.id.TextView_PwdProblem);
                    	 
                    	 String strPassword1 = password1.getText().toString();
                    	// String strPassword2 = password2.getText().toString();
                      //  if (strPassword1.equals(strPassword2)) {
                        	if (strPassword1.length()>0)
                        	{
     		
                        		SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
            	    		    SharedPreferences.Editor editor = sharedPreferences.edit();
            	    		    editor.putString(Constants.KEY_PKCS12_PASSWORD, strPassword1);
            	    		    editor.commit();
                        		makePKCS12Store(fileToInstall);
        
                        		boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
                      		  if(!isDataConnectionAvailable)
                        		{
                        		MHISEUtil.displayDialog(CertificateInitializer.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
                        		}
                        		else
                        		{	
                        			showDialog(GET_USER_INFORMATION_PROGRESS_DIALOG);
                        		String[] params ={com.mhise.requests.RequestBase.getUserInformation()};
                        		getUsertypeObj = new GetUserType();
                        		getUsertypeObj.execute(params);
                        		}
                        	}
                        	else
                        	{	//Logger.debug("CertificateInitializer->PASSWORD_DIALOG-->Entered Password is wrong:", ""+strPassword1);
                        		error.setText(getResources().getString(R.string.settings_password1));
                        	}
                        }
                   /*     else
                        {  
                        	//Logger.debug("CertificateInitializer->PASSWORD_DIALOG-->Password not Confirmed!", ""+strPassword1);
                        	error.setText("Password not Confirmed!");
                        	password1.setText(null);
                        	//password2.setText(null);
                        }*/
                        
                     }
                  ).create();
               password_dialog.setOnCancelListener(new OnCancelListener() {
						
						@Override
						public void onCancel(DialogInterface dialog) {
							// TODO Auto-generated method stub
							dialog.dismiss();
							CertificateInitializer.this.finish();
						}
					});
                  
          return builder.create();

            case Constants.INVALID_CERTIFICATE_DIALOG:
            {
            	       
                AlertDialog.Builder builder1 = new AlertDialog.Builder(this);   
                builder1.setTitle(getResources().getString(R.string.error_Invalid_Certificate_Title));
                builder1.setMessage(getResources().getString(R.string.error_Invalid_Certificate_Msg));
              builder1.setPositiveButton(getResources().getString(R.string.Register), new OnClickListener() {
				
				public void onClick(DialogInterface dialog, int which) {
					// TODO Auto-generated method stub
					startActivity(new Intent(CertificateInitializer.this,Register.class));
					CertificateInitializer.this.finish();
				}
              });
              builder1.setNegativeButton(getResources().getString(R.string.Cancel), new OnClickListener() {
  				
  				public void onClick(DialogInterface dialog, int which) {
  					// TODO Auto-generated method stub
  					CertificateInitializer.this.finish();
  				}
  			});
                Dialog dialog = builder1.create();
                dialog.setOnCancelListener(new OnCancelListener() {
					
					@Override
					public void onCancel(DialogInterface dialog) {
						// TODO Auto-generated method stub
						dialog.dismiss();
						 CertificateInitializer.this.finish();
						 try {
							FileInputStream fis = context.openFileInput(Constants.defaultP12StoreName);
							
							fis.mark(fis.available());
							fis.reset();
							fis.close();
							 
						} catch (FileNotFoundException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
							Logger.debug("CertificateInitializer->INVALID_CERTIFICATE_DIALOG-->:", ""+e);
							
						}
						catch (IOException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
							Logger.debug("CertificateInitializer->INVALID_CERTIFICATE_DIALOG-->", ""+e);
						}
						 
					}
				});
              
               return dialog;
              
            }
          
            case Constants.PASSWORD_DIALOG_PFX:
            	LayoutInflater inflater1 = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                  final View layout1 = inflater1.inflate(R.layout.dialogforpfx, /*(ViewGroup) findViewById(R.id.root)*/null);
                  AlertDialog.Builder builder_pfx = new AlertDialog.Builder(this);
                  
                  builder_pfx.setTitle(getResources().getString(R.string.settings_password1));
                  builder_pfx.setView(layout1);
            
                  
                  
                  Dialog password_dialog1  =  builder_pfx.setPositiveButton(android.R.string.ok, new DialogInterface.OnClickListener() {
                         public void onClick(DialogInterface dialog, int which) {
                        	 final EditText password1 = (EditText) layout1.findViewById(R.id.EditText_Pwd1);
                          //   final TextView error = (TextView) layout1.findViewById(R.id.TextView_PwdProblem);              
                        	 String strPassword1 = password1.getText().toString();
                       
                        	 
                            	if (MHISEUtil.verifyP12StorePassword(selectedPath, strPassword1,getIntent().getStringExtra("CertificateSerialNumber"),CertificateInitializer.this))
                            	{       
                            		removeDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
                            		SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
                	    		    SharedPreferences.Editor editor = sharedPreferences.edit();
                	    		    editor.putString(Constants.KEY_PKCS12_PASSWORD, strPassword1);
                	    		    editor.commit();
                	    		    MHISEUtil.movePKCS12(selectedPath ,getApplicationContext());
                	    		    
                	    			boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
                	    			  if(!isDataConnectionAvailable)
                	    	  			{
                	    				  MHISEUtil.displayDialog(CertificateInitializer.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
                	    	  			}
                	    	  		else
                	    	  		{	 
                	    	  		removeDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
                	    	  		showDialog(GET_USER_INFORMATION_PROGRESS_DIALOG);                	    		    
                	    	  		String[] params ={com.mhise.requests.RequestBase.getUserInformation()};
                	    		    getUsertypeObj = new GetUserType();
                	    		    getUsertypeObj.execute(params);
                	    	  		}
                            	}
                            	else
                            	{	
                            		removeDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
                            		showDialog(Constants.CERTIFICATE_MISMATCH_DIALOG);
                            	
                            	}
                            
                            
                         }
                      }).create();
                   	password_dialog1.setOnCancelListener(new OnCancelListener() {
    						
    						@Override
    						public void onCancel(DialogInterface dialog) {
    							// TODO Auto-generated method stub
    							dialog.dismiss();
    							//Logger.debug("Certificate Initiliazer--> Password dialog for pfx","Dialog Canceled");
    							CertificateInitializer.this.finish();
    						}
    					});
                      
              return password_dialog1;
          
	        
	            case Constants.GET_CERTIFICATE_PROGRESS_DIALOG:
	            	
	            	
	            	 ProgressDialog dialog = new ProgressDialog(this);
	            	 
            		 dialog.setMessage(getResources().getString(R.string.error_check_cert));          		 
            		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
            		 
            		 dialog.setCancelable(true);
            		 dialog.setCanceledOnTouchOutside(false);
            	 dialog.setOnCancelListener(new OnCancelListener() {
   						
   						@Override
   						public void onCancel(DialogInterface dialog) {
   							// TODO Auto-generated method stub
   							dialog.dismiss();
   							try{
   							 getUsertypeObj.cancel(true);
   							}
   							catch (NullPointerException e) {
								// TODO: handle exception
   								Logger.debug("GET_CERTIFICATE_PROGRESS_DIALOG", ""+e);
							}
   							 CertificateInitializer.this.finish();
   						}
   					});
            		 return dialog;	
            	
	            case Constants.SELECT_CERTIFICATE:
	            {
	            	
	            	  AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
	            	 
	            	if (loc.length>0)
	            	{	            	  
	            	    builder1.setTitle(getResources().getString(R.string.select_cert))
	            	           .setItems(loc, new DialogInterface.OnClickListener() {
	            	              public void onClick(DialogInterface dialog, int which) {
	            	            	selectedPath =loc[which];   
	            	            	removeDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
	            	    		   installSelectedCertificate(selectedPath);
	            	          
	            	           }
	            	    });
	            	    builder1.setPositiveButton(getResources().getString(R.string.Register), new OnClickListener() {
						
							@Override
							public void onClick(DialogInterface dialog, int which) {
								// TODO Auto-generated method stub
								removeDialog(Constants.GET_CERTIFICATE_PROGRESS_DIALOG);
								startActivity(new Intent(CertificateInitializer.this,Register.class));
								CertificateInitializer.this.finish();
							}
						});
	            	  Dialog dlg = builder1.create();
	            	  dlg.setOnCancelListener(new OnCancelListener() {
						
						@Override
						public void onCancel(DialogInterface dialog) {
							// TODO Auto-generated method stub
								dialog.dismiss();
								CertificateInitializer.this.finish();
						}
					});
	            	  return dlg;
	            	}
	            	else  		
	            	{
	            		//Call  Registeration screen
	            		//Logger.debug("CertificateInitializer->SELECT_CERTIFICATE-->No certificate available ", "true");
	      	    	  	Intent callRegisterScreen = new Intent(CertificateInitializer.this,Register.class);
	      	    	  	startActivity(callRegisterScreen);
	      	    	  	CertificateInitializer.this.finish();
	      	    	  	return null;
	            	}
	            	  
	            	
	            	
	            }
	            
	            case Constants.CERTIFICATE_MISMATCH_DIALOG:
	            {

	     		
	     		 AlertDialog.Builder builder1 = new AlertDialog.Builder(this);   
	              builder1.setTitle(getResources().getString(R.string.error_Invalid_Certificate_Mismatch_Title));
	              builder1.setMessage(getResources().getString(R.string.error_Invalid_Certificate_Mismatch_msg));
	              builder1.setPositiveButton(getResources().getString(android.R.string.ok), new OnClickListener() {
					
					public void onClick(DialogInterface dialog, int which) {
						// TODO Auto-generated method stub
						Intent userLogin = new Intent(CertificateInitializer.this,UserLogin.class);
						startActivity(userLogin);
						finish();
					}
	              });
	            
	                Dialog dialog_pwd_mismatch = builder1.create();
	                dialog_pwd_mismatch.setOnCancelListener(new OnCancelListener() {
						
						@Override
						public void onCancel(DialogInterface dialog) {
							// TODO Auto-generated method stub
							dialog.dismiss();
							 CertificateInitializer.this.finish();
 
						}
					});
	              
	               return dialog_pwd_mismatch;
	            }
	            
	            case Constants.PRIVATE_KEY_MISSING :
	            {
	           	  AlertDialog alertDialog = new AlertDialog.Builder(
			                CertificateInitializer.this).create();
					alertDialog.setMessage(getResources().getString(R.string.error_PrivateKey));
					alertDialog.setTitle(getResources().getString(R.string.error_Invalid_Certificate_Title));
					alertDialog.setButton(getResources().getString(R.string.Register), new DialogInterface.OnClickListener() {
						public void onClick(DialogInterface dialog, int which) {
					    	 
							Intent callRegisterScreen = new Intent(CertificateInitializer.this,Register.class);
					    	  startActivity(callRegisterScreen);
					    	  CertificateInitializer.this.finish();
							
						}
						});

	            	  alertDialog.setOnCancelListener(new OnCancelListener() {
						
						@Override
						public void onCancel(DialogInterface dialog) {
							// TODO Auto-generated method stub
							dialog.dismiss();
							 CertificateInitializer.this.finish();
						}
					});
	            	  
					return alertDialog;
	            }
	       
            	
    case GET_USER_INFORMATION_PROGRESS_DIALOG:

   	 ProgressDialog userInfoDialog = new ProgressDialog(this);
   	userInfoDialog.setMax(15000);
   	userInfoDialog.setMessage(getResources().getString(R.string.progressbar_msg_User_Info));          		 
   	userInfoDialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);	 
 
   	userInfoDialog.setCancelable(true);
   	userInfoDialog.setCanceledOnTouchOutside(false);
   	  	userInfoDialog.setOnCancelListener(new OnCancelListener() {
				
				@Override
				public void onCancel(DialogInterface dialog) {
					// TODO Auto-generated method stub
					dialog.dismiss();
					try{
					 getUsertypeObj.cancel(true);
					}
					catch (NullPointerException e) {
					// TODO: handle exception
						Logger.debug("GET_USER_INFORMATION_PROGRESS_DIALOG", ""+e);
				}
					 CertificateInitializer.this.finish();
				}
			});
		 return userInfoDialog;	
    default:
		 return null;
}		 
            			 
	  }
		
	private void installSelectedCertificate(String selectedPath)
	{
		String _certificateType ;
		
		try {
			_certificateType =selectedPath.substring(selectedPath.length()-3, selectedPath.length());
			//Logger.debug("CertificateInitializer->installSelectedCertificate -- Selected_CertificateType", ""+_certificateType);
			//Certificate implementation changed on 29Nov
		/*	if (_certificateType.equalsIgnoreCase(certTypeP7B))
			{
				installP7BCertificate(selectedPath);
			}
			else*/
				if(_certificateType.equalsIgnoreCase(certTypePFX)||_certificateType.equalsIgnoreCase(certTypeP12))
			{
				showDialog(Constants.PASSWORD_DIALOG_PFX);
			}
			}
		catch (NullPointerException e) {
			// TODO: handle exception
			Logger.debug("CertificateInitializer->installSelectedCertificate  Selected path is invalid", ""+e);
		}
	}
	  
/*	public void installP7BCertificate(String path)
	{
	
		 fileToInstall = new File(path);
	    
	  String keyFile ="privateKey.key";
	  FileInputStream fis =null;
	      try {
	    	  fis = this.openFileInput(keyFile);
	      } catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
	      }
	      try {
			if(fis.available()>0)
			{
				fis.close();
				showDialog(Constants.PASSWORD_DIALOG);
			}
			else
			{
				
				showDialog(Constants.PRIVATE_KEY_MISSING);			
			}
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	      catch (NullPointerException e) {
	    	  // TODO: handle exception
	    	showDialog(Constants.PRIVATE_KEY_MISSING);		
					
	      }
			}	*/		
	public   void installP7BCertificate(String certificate,PrivateKey pKey,String strPassword,String certName)
	{
	
		char[] password = strPassword.toCharArray();
		File _mobiusDirectory = new File(Constants.defaultP12StorePath);
		   
		   if(!_mobiusDirectory.exists())
		   {
		      _mobiusDirectory.mkdir();
		   }
		 try{
			 KeyStore keyStore = KeyStore.getInstance("PKCS12");
			
			 keyStore.load(null, password); 
			
			CertificateFactory certificateFactory =
			CertificateFactory.getInstance("X.509");
			java.security.cert.Certificate[] chain = {};
			InputStream ins1 = new ByteArrayInputStream(certificate.getBytes());
			chain = certificateFactory.generateCertificates(ins1).toArray(chain);
			ins1.close();
			keyStore.setKeyEntry("pkcs12", pKey,  password ,chain  ); 

			// Store keyStore in internal Storage
			/*OutputStream fos = getApplicationContext().openFileOutput(Constants.defaultP12StoreName, Context.MODE_PRIVATE);     
			keyStore.store(fos,password);
			fos.close();*/
			
	        File newKeystoreFile = new File(Constants.defaultP12StorePath + certName);            
            FileOutputStream fout= new FileOutputStream(newKeystoreFile); 
            keyStore.store(fout,password);
            fout.close();
            
			Enumeration<String> aliases =null;
			try {
				aliases = keyStore.aliases();
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
				    		keyStore.getCertificate(alias);
			} catch (KeyStoreException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

				  SharedPreferences sharedPreferences1 =getApplicationContext().getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
  	    		  SharedPreferences.Editor editor = sharedPreferences1.edit();
  	    		  editor.putString(Constants.KEY_SERIAL_NUMBER, ""+cert.getSerialNumber().toString(16));
  	    		  editor.commit();
				  //isInstalledCertificateValid = true; 
			  
			}

			boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
			  if(!isDataConnectionAvailable)
	  		{
	  		MHISEUtil.displayDialog(CertificateInitializer.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
	  		}
	  		else
	  		{	
	  			showDialog(GET_USER_INFORMATION_PROGRESS_DIALOG);
	  			String[] params ={com.mhise.requests.RequestBase.getUserInformation()};
	  			getUsertypeObj = new GetUserType();
	  			getUsertypeObj.execute(params);
	  		}
		
		 }
		 catch (Exception e) {
			// TODO: handle exception
			 
			 Logger.debug("Exception installP7BCertificate", ""+e);
		}

			}

	private class GetUserType extends AsyncTask<String, Void, String>
	 {
		 @Override
		protected String doInBackground(String... params) {
			 
			SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);                     
			DefaultHttpClient httpClient= MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));
			
			String request = params[0];	
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.HTTPS_URL, XmlConstants.ACTION_GETUSERINFORMATION, request,httpClient );
			return response;
	}		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 	super.onPostExecute(result);	
			 		
			 	Logger.debug("CertificateInitializer->User type response",""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
					User user;
					try{
						removeDialog(GET_USER_INFORMATION_PROGRESS_DIALOG); 
						user =  BaseParser.parseUserInformationType(resultDoc);
				
						SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
					    SharedPreferences.Editor editor = sharedPreferences.edit();
					    //editor.putString(Constants.HOME_COMMUNITY_ID,user.getCommunityId().toString().trim());
					    editor.putString(Constants.HOME_COMMUNITY_ID,"2.16.840.1.113883.3.1605");
					   
					    editor.putString(Constants.KEY_USER_EMAIL,user.getEmail().toString().trim());
					    editor.putString(Constants.KEY_USER_TYPE,user.getUserType().toString().trim());
					    editor.commit();	
						
					if (user.getUserType().equalsIgnoreCase("Patient"))
					{

					//	Logger.debug("CertificateInitializer->GetUserType->onPostExecute--PATIENT_ID","->"+user.getId());
												
						CertificateInitializer.this.finish();
						
	                	Intent intent = new Intent(CertificateInitializer.this,GetDocumentDetails.class);
	                	Log.i("Patient name---",""+MHISEUtil.makeNameInUpperCase(user.getName()))	  ;              		      
	                	intent.putExtra(Constants.KEY_NAME, MHISEUtil.makeNameInUpperCase(user.getName()));	             	                	
						intent.putExtra(Constants.KEY_USER_ID , user.getMPIID());
						intent.putExtra(Constants.KEY_ROLE, user.getRole()); 
						intent.putExtra(Constants.KEY_USER_TYPE, user.getUserType()); 
						intent.putExtra(Constants.KEY_USER_EMAIL, user.getEmail());
						intent.putExtra("loginUser",user);
						startActivity(intent);
						    
					 }
					 else if(user.getUserType().equalsIgnoreCase("Provider"))
					 {
					            
	                	Intent intent = new Intent(CertificateInitializer.this,SearchPatient.class);
	                	intent.putExtra(Constants.KEY_ROLE, user.getRole()); 
	                	intent.putExtra(Constants.KEY_USER_TYPE, user.getUserType()); 
	                	intent.putExtra(Constants.KEY_USER_EMAIL, user.getEmail());
	                	intent.putExtra("User",user);
	                	startActivity(intent);
	                	
	                	CertificateInitializer.this.finish();
					 }
					 else if(user.getUserType().equalsIgnoreCase("Unspecified"))
					 {
						 AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(CertificateInitializer.this );
							 alertDialogBuilder.setMessage(getResources().getString(R.string.error_unspecified_user_msg));
							 alertDialogBuilder.setTitle(getResources().getString(R.string.error_unspecified_user_title));
							 alertDialogBuilder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog, int which) {
									dialog.cancel();
									CertificateInitializer.this.finish();
								}
								});
							 alertDialogBuilder.setOnCancelListener(new OnCancelListener() {
									
									@Override
									public void onCancel(DialogInterface dialog) {
										// TODO Auto-generated method stub
										dialog.cancel();
										CertificateInitializer.this.finish();
										
									}
								});
								// Showing Alert Message
							 alertDialogBuilder.show();
        
						
					 }
					 else
					 {
						 CertificateInitializer.this.finish();
					 }

					}
					catch (NullPointerException e) {
						// TODO: handle exception
						Logger.debug("CertificateInitializer->CertificateInitializer->GetUserType->onPostExecute--", "No user response"+e);
						showDialog(Constants.INVALID_CERTIFICATE_DIALOG);	
					}
				}
		 }
		}
	


	
}