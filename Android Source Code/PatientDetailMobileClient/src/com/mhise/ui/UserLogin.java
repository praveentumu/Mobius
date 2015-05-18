package com.mhise.ui;



import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;

import java.math.BigInteger;
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
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.CompoundButton.OnCheckedChangeListener;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.mhise.constants.Constants;
import com.mhise.model.AuthenticateUserResponse;
import com.mhise.model.CSRDetails;
import com.mhise.model.RegisterationResult;
import com.mhise.requests.RequestBase;
import com.mhise.response.BaseParser;
import com.mhise.response.GetMasterDataParser;
import com.mhise.security.GenerateCSR;

import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;


public class UserLogin extends Activity implements OnClickListener {

	private EditText edt_EmailID;
	private EditText edt_Password;
	private TextView txt_ForgotPWD ;
	private TextView txt_import;
	private TextView txt_ActivateUser;
	private Button btn_Login;
	private Button btn_Register;
	private RadioGroup  rgUserType;
	CheckBox  chkRemember;
	private AlertDialog importcertificateDialog;
	private AlertDialog activateUserDialog;
	private UserLoginAsync userLoginAsync;
	private GetCSRDetailsAsync getCSRDetailsAsync;
	private FetchingCertificateAsync fetchingCertificateAsync;
	private double versionInstalled;
	SharedPreferences appSharedPrefs ;
	CheckUserActivationAsync checkUserActivationAsync;
	GenerateCSR	 gCSR ;
	private TextView errorInImport;
	private TextView errorActivate;
	private RadioGroup rgUserType1;
	private RadioGroup rgActivateUserType;
	String certPWD,emailID1;
	String activateUserPWD,ActivateUserEmailID;
	ActivateUserAsync activateUserAsync;
	 String   email;
	@Override
	protected void onCreate(Bundle savedInstanceState) {

		super.onCreate(savedInstanceState);
		appSharedPrefs=getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
		setContentView(R.layout.loginscreen);
		loadUI();
		upgrade();
		
	}
	
	
  @Override
  	protected void onResume() {
	// TODO Auto-generated method stub
	super.onResume();
	handleEvents();

}
	
	private void handleEvents() 
	{
		if(appSharedPrefs.contains("EmailID"))
		{
			edt_EmailID.setText(appSharedPrefs.getString("EmailID", ""));
		}
		if(appSharedPrefs.contains("Password"))
			{
			edt_Password.setText(appSharedPrefs.getString("Password", ""));
			}
		if(appSharedPrefs.contains("checkRemember"))
		{
		chkRemember.setChecked(appSharedPrefs.getBoolean("checkRemember", false));
		}	
		if(appSharedPrefs.contains("UserTypeID"))
		{
		rgUserType.check(appSharedPrefs.getInt("UserTypeID", R.id.radioProvider));
		}
		
		chkRemember.setOnCheckedChangeListener(new OnCheckedChangeListener() {
			
			
			@Override
			public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
				// TODO Auto-generated method stub
				if(isChecked)
				{
					rememberDetails();
				}
				else
				{
					Editor edt =appSharedPrefs.edit();
					edt.putString("EmailID", "");
					edt.putString("Password","");
					edt.putBoolean("checkRemember", false);
					edt.putInt("UserTypeID",R.id.radioProvider);
					edt.commit();
				}
			}
		});
		
		
		btn_Register.setOnClickListener(this);
		btn_Login.setOnClickListener(this);
		txt_ForgotPWD.setOnClickListener(this);
		txt_ForgotPWD.setOnClickListener(this);
		txt_import.setOnClickListener(this);
		txt_ActivateUser.setOnClickListener(this);
	}
	
	private void rememberDetails()
	{
		Editor edt =appSharedPrefs.edit();
		edt.putString("EmailID", edt_EmailID.getText().toString());
		edt.putString("Password",edt_Password.getText().toString());
		edt.putInt("UserTypeID",rgUserType.getCheckedRadioButtonId());
		edt.putBoolean("checkRemember", true);
		edt.commit();
		
	}
	
	private void loadUI() {

		edt_EmailID = (EditText) findViewById(R.id.edtLoginEmail);
		edt_Password = (EditText) findViewById(R.id.edtLoginPWD);
		chkRemember =(CheckBox)findViewById(R.id.checkRemember);
		btn_Login = (Button) findViewById(R.id.btnLogin);
		btn_Register = (Button) findViewById(R.id.btnRegister);
		txt_import =(TextView)findViewById(R.id.txtImport);
		txt_ForgotPWD = (TextView) findViewById(R.id.txtForgotPWD);
		txt_ActivateUser =(TextView) findViewById(R.id.TextView_ActivateUser);
		rgUserType=(RadioGroup) findViewById(R.id.radioUserType);

	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		
		
		switch (v.getId()) {
			
			case R.id.btnLogin: {
				if (chkRemember.isChecked())
				{
					rememberDetails();
				}
				else 
				{
					Editor edt =appSharedPrefs.edit();
						edt.putString("EmailID", "");
						edt.putString("Password","");
						edt.putBoolean("checkRemember", false);
						edt.putInt("UserTypeID",R.id.radioProvider);
						edt.commit();
				}
				
				String request =makeRequest();
				showDialog(Constants.LOGIN_PROGRESS_DIALOG);
				userLoginAsync = new UserLoginAsync();
				userLoginAsync.execute(request);
				break;
			}
			case R.id.btnRegister: {
			
			 	Intent callRegisterScreen = new Intent(UserLogin.this,Register.class);
				startActivity(callRegisterScreen);
				//UserLogin.this.finish();
				
			/*	int selectedId = rgUserType.getCheckedRadioButtonId();
			 * RadioButton  radioUserButton = (RadioButton) findViewById(selectedId);
			 * if (radioUserButton.getText().toString().equals(getResources().getString(R.string.Provider)))
				{
					Intent registerProvider = new Intent(this, ProviderTypeScreen.class);
					startActivity(registerProvider);
				}	
				else if(radioUserButton.getText().toString().equals(getResources().getString(R.string.Patient)))
				{
					Intent registerPatient = new Intent(this, RegisterPatient.class);
					startActivity(registerPatient);
				}
				*/
  	    	  break;
				
			}
			case R.id.txtForgotPWD: {
				Intent callForgotPassword = new Intent(UserLogin.this,ForgotPassword.class);				
				startActivity(callForgotPassword);
			
  	    	  break;
				
			}
			case R.id.txtImport:
			{
				/*Previous Implementation for Showing steps to download*/
				
				/*AlertDialog.Builder importbuilder = new AlertDialog.Builder(this);
			 	importbuilder.setTitle(getResources().getString(R.string.importHelpTitle));
			 	importbuilder.setMessage(getResources().getString(R.string.importHelpMessage));
			 	importbuilder.create().show();*/				
				/*New Implementation to fetch Certificate*/


			   	 LayoutInflater inflater = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		         final View importLayout = inflater.inflate(R.layout.importcertificate, (ViewGroup) findViewById(R.id.root));
		         AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
		         builder1.setTitle(getResources().getString(R.string.CertificateImport));
		         builder1.setView(importLayout); 
		         importCertificate(importLayout,builder1,this) ;

				break;
							
			}
			case R.id.TextView_ActivateUser:
			{
			
				LayoutInflater inflater = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		         final View activateUserLayout = inflater.inflate(R.layout.activateuser, (ViewGroup) findViewById(R.id.root));
		         AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
		         builder1.setTitle(getResources().getString(R.string.activateuser));
		         builder1.setView(activateUserLayout); 
		         activateUser(activateUserLayout,builder1,this) ;
			}
	
		}
	}

	public void activateUser(final View layout , AlertDialog.Builder builder,final Context appConetxt)
	{
		activateUserDialog =builder.create();
		activateUserDialog.show();
		
  		Button btnActivate = (Button) layout.findViewById(R.id.btnActivate);
  		Button btnCancel = (Button) layout.findViewById(R.id.btnCancel);
  		final EditText emailID = (EditText) layout.findViewById(R.id.edtActiveEmail);
        final EditText password = (EditText) layout.findViewById(R.id.edtActivatePWD);
        errorActivate =    ( TextView) layout.findViewById(R.id.TextView_ActivatePwdProblem);
        rgActivateUserType=(RadioGroup) layout.findViewById(R.id.radioActivateUserType);
        
        btnActivate.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				try {
					
					AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(
							UserLogin.this);

						alertDialogBuilder.setTitle("Confirmation");
			 
						// set dialog message
						alertDialogBuilder
							.setMessage("This action will deactivate all previous devices and activate this device only for account access. Do you want to continue?")
							.setCancelable(false)
							.setPositiveButton("Yes",new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog,int id) {
									// if this button is clicked, close
									// current activity
									errorActivate.setVisibility(View.GONE);
									ActivateUserEmailID =emailID.getText().toString();
									activateUserPWD =password.getText().toString();					
									int selectedId = rgActivateUserType.getCheckedRadioButtonId();
									String loginRequest =null;
									RadioButton  radioUserButton = (RadioButton) findViewById(selectedId);
									if (radioUserButton.getText().toString().equals(getResources().getString(R.string.Provider)))
										loginRequest =RequestBase.loginRequest(ActivateUserEmailID,activateUserPWD,Constants.strPROVIDER );
									else if(radioUserButton.getText().toString().equals(getResources().getString(R.string.Patient)))
									loginRequest =RequestBase.loginRequest(ActivateUserEmailID,activateUserPWD,Constants.strPATIENT );
					                showDialog(Constants.ACTIVATING_USER_DIALOG);
									checkUserActivationAsync = new CheckUserActivationAsync();
									checkUserActivationAsync.execute(loginRequest);
								}
							  })
							.setNegativeButton("No",new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog,int id) {
									// if this button is clicked, just close
									// the dialog box and do nothing
									dialog.cancel();
								}
							});
			 
							// create alert dialog
							AlertDialog alertDialog = alertDialogBuilder.create();
			 
							// show it
							alertDialog.show();
					
				
	
				}
				catch (Exception e) {
					// TODO: handle exception
					e.printStackTrace();
				}
			}
		});
        
        btnCancel.setOnClickListener(new OnClickListener() {
			
   				@Override
   				public void onClick(View v) {
   					emailID.setText(null);		
   					password.setText(null);
   					errorActivate.setText(null);
   					errorActivate.setVisibility(View.GONE);
   					rgActivateUserType.check(R.id.radioProvider);
   					activateUserDialog.dismiss();
   				}
   			});
        
	}
	private class GetCSRDetailsAsync extends AsyncTask<String, Void, String>
	{
		protected String doInBackground(String... params) {
			
			String request = params[0];	
			DefaultHttpClient httpClient= new DefaultHttpClient();
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL, XmlConstants.ACTION_GET_CSR_DETAILS, request,httpClient);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);	
			 CSRDetails csrDetails=null;
			 String csr =null;
			 	Logger.debug("User Login-->GetUserDetailsAsync-->",""+result);
			 	Log.i("GetCSR Detail response -->Response", ""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
				 csrDetails=	BaseParser.parseCSRDetails(resultDoc);
				}
				
				int selectedId = rgActivateUserType.getCheckedRadioButtonId();
				RadioButton  radioUserButton = (RadioButton) findViewById(selectedId);
				gCSR = new GenerateCSR();
				if (radioUserButton.getText().toString().equals(getResources().getString(R.string.Patient)))
				{
						String   	commonname=csrDetails.getGivenName()+csrDetails.getFamilyName();
		                 String  	organizationalunit =Constants.organizationalunit;
		                 String  	organizationname=Constants.organizationname;
		                 String    	city=csrDetails.getCity();
		                 String   	 state= csrDetails.getState();
		                 String  	country =csrDetails.getCountry();
		                email=csrDetails.getEmailAddress();
		                
		                 String request;
		            	 
//		                 made a change in following line by checking firstname and lastname separately
		                   if(csrDetails.getGivenName().equals(null) ||csrDetails.getGivenName().equals("") || csrDetails.getFamilyName().equals(null) ||csrDetails.getFamilyName().equals("") ||email.equals(null)||  email.equals("")||city.equals(null) ||city.equals("") || state.equals("") ||state.equals(null) || country.equals(null)||country.equals("")) 	                    
									{
		                    	MHISEUtil.displayDialog(UserLogin.this,
										getResources().getString(R.string.error_CSR_msg),getResources().getString(R.string.error_CSR_title));							
									}
									else
									{ 
										
									 	 try{
											 csr=gCSR.generatePKCS10(email,commonname,organizationalunit,organizationname,city,state,country);					 								  
											 Log.i("CSR", ""+csr);		
											  request =RequestBase.getActivateUserRequest(email, Constants.strPATIENT, csr);
											  activateUserAsync = new ActivateUserAsync();
											  activateUserAsync.execute(request);
									 	 	}
									    	catch (Exception e) {
									    		removeDialog(Constants.ACTIVATING_USER_DIALOG);
									    		Logger.debug(" GenerateCSRClass" ,"Exception------>"+e);
									    		// TODO: handle exception
											}
									
									}
					
					/* firstname =firstName.getText().toString().trim();
	                 lastname=lastName.getText().toString().trim();
	                 commonname=firstname+lastname;
	                 email=edtemail.getText().toString().trim();
	                 organizationalunit =Constants.organizationalunit;
	                 city=txtcity.getText().toString().trim();
	                 state= txtstate.getText().toString().trim();                      
	                 country =Constants.Country;		
					
				
						if(commonname.equals("") || commonname.equals(null) ||email.equals(null) || email.equals("") ||city.equals(null) ||city.equals("") || state.equals(null) || state.equals("") ||country.equals(null)||country.equals(""))
						{
							MHISEUtil.displayDialog(RegisterPatient.this,getResources().getString(R.string.error_CSR_msg),getResources().getString(R.string.error_CSR_msg));
						
						}
						else
						{	
							
						showDialog(Constants.GENERATE_CSR_PROGRESS_DIALOG);
						generateCSRClass =		new GenerateCSRClass();
						generateCSRClass.execute();
							
						}
					*/
					
					
				}
				else{

				
				try{
					if(csrDetails.getIsIndividualProvider().equals("true"))
					{
	                 String   	commonname=csrDetails.getGivenName()+csrDetails.getFamilyName();
	                 String  	organizationalunit =Constants.organizationalunit;
	                 String  	organizationname=Constants.organizationname;
	                 String    	city=csrDetails.getCity();
	                 String   	 state= csrDetails.getState();
	                 String  	country =csrDetails.getCountry();
	                   email=csrDetails.getEmailAddress();
	                 String request;
	            	 
	            	 
//	                 made a change in following line by checking firstname and lastname separately
	                   if(csrDetails.getGivenName().equals(null) ||csrDetails.getGivenName().equals("") || csrDetails.getFamilyName().equals(null) ||csrDetails.getFamilyName().equals("") ||email.equals(null)||  email.equals("")||city.equals(null) ||city.equals("") || state.equals("") ||state.equals(null) || country.equals(null)||country.equals("")) 	                    
								{
	                    	MHISEUtil.displayDialog(UserLogin.this,
									getResources().getString(R.string.error_CSR_msg),getResources().getString(R.string.error_CSR_title));							
								}
								else
								{ 
									
								 	 try{
										 csr=gCSR.generatePKCS10(email,commonname,organizationalunit,organizationname,city,state,country);					 								  
										 Log.i("CSR", ""+csr);		
										  request =RequestBase.getActivateUserRequest(email, Constants.strPROVIDER, csr);
										  activateUserAsync = new ActivateUserAsync();
										  activateUserAsync.execute(request);
								 	 	}
								    	catch (Exception e) {
								    		removeDialog(Constants.ACTIVATING_USER_DIALOG);
								    		Logger.debug(" GenerateCSRClass" ,"Exception------>"+e);
								    		// TODO: handle exception
										}
								
								}			
						
					}
					else
					{
	                  String  commonname =csrDetails.getOrgName(); 
	                  String  email=csrDetails.getEmailAddress();
	                  String  organizationalunit =Constants.organizationalunit;
		              String  city=csrDetails.getCity();
		              String  state= csrDetails.getState();
		              String  country =csrDetails.getCountry();	               
	                  String   organizationname =csrDetails.getOrgName();
	                  
	
					if(commonname.equals("")||commonname.equals(null) || email.equals(null) ||email.equals("") ||city.equals(null) ||city.equals("") || state.equals(null) ||state.equals("") || country.equals(null)||country.equals(""))
						{
							MHISEUtil.displayDialog(UserLogin.this,
										getResources().getString(R.string.error_CSR_msg),getResources().getString(R.string.error_CSR_title));
							}
							else
							{
								 
								 
							 	 try {
									 csr=gCSR.generatePKCS10(email,commonname,organizationalunit,organizationname,city,state,country);					 								  
									 Log.i("CSR", ""+csr);
									String request =RequestBase.getActivateUserRequest(email, Constants.strPROVIDER, csr);
									  new ActivateUserAsync().execute(request);
						    	}
							    	catch (Exception e) {
							    		removeDialog(Constants.ACTIVATING_USER_DIALOG);
							    		Logger.debug(" GenerateCSRClass" ,"Exception------>"+e);
							    		// TODO: handle exception
									}
						  
							}
					}
				
					
				}
				catch (NullPointerException e) {
					// TODO: handle exception
					Logger.debug("GetCSRDetailsAsync", ""+e);
					removeDialog(Constants.ACTIVATING_USER_DIALOG);
				}
				}	
		 }
		
	}
	
	
	public void importCertificate(final View layout , AlertDialog.Builder builder,final Context appConetxt)
	{		
		importcertificateDialog =builder.create();
		importcertificateDialog.show();
  		Button btnOK = (Button) layout.findViewById(R.id.btnImportCertificate);
  		Button btnCancel = (Button) layout.findViewById(R.id.btnCancel);
  		final EditText emailID = (EditText) layout.findViewById(R.id.edtEmail);
        final EditText passwordNew = (EditText) layout.findViewById(R.id.edtCERTPWD);
        
        errorInImport = ( TextView) layout.findViewById(R.id.TextView_Problem);
        rgUserType1=(RadioGroup) layout.findViewById(R.id.radioUserType1);
	
        btnOK.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				try {
					emailID1 =emailID.getText().toString();
					certPWD =passwordNew.getText().toString();
					
					int selectedId = rgUserType1.getCheckedRadioButtonId();
					RadioButton  radioUserButton = (RadioButton) findViewById(selectedId);
					String request =null;
					if (radioUserButton.getText().toString().equals(getResources().getString(R.string.Provider)))
						request=	RequestBase.getUserCertificateRequest(emailID1,"Provider" );
					else if(radioUserButton.getText().toString().equals(getResources().getString(R.string.Patient)))
						request=	RequestBase.getUserCertificateRequest(emailID1,"Patient");
					
					if(certPWD.length()<1)
					{
						if(emailID1.length()>0)
						{
							errorInImport.setVisibility(View.VISIBLE);
							errorInImport.setText(getResources().getString(R.string.securityPasswordEmpty));
						}
						else
						{
							if(certPWD.contains(" "))
							{
								errorInImport.setVisibility(View.VISIBLE);
								errorInImport.setText(getResources().getString(R.string.whitespaeinPassword));
							}
							else
							{
							errorInImport.setVisibility(View.GONE);
							showDialog(Constants.FETCHING_CERTIFICATE_DIALOG);
							fetchingCertificateAsync = new FetchingCertificateAsync();
							fetchingCertificateAsync.execute(request);
							}
						}
					}
					else 
					{
						/*if(certPWD.contains(" "))
						{
							errorInImport.setVisibility(View.VISIBLE);
							errorInImport.setText(getResources().getString(R.string.whitespaeinPassword));
						}
						else
						{*/
						errorInImport.setVisibility(View.GONE);
						showDialog(Constants.FETCHING_CERTIFICATE_DIALOG);
						fetchingCertificateAsync = new FetchingCertificateAsync();
						fetchingCertificateAsync.execute(request);
						//}
					}

				
			}
				catch (Exception e) {
					// TODO: handle exception
					e.printStackTrace();
				}
			}
		});
        
        btnCancel.setOnClickListener(new OnClickListener() {
			
   				@Override
   				public void onClick(View v) {
   					emailID.setText(null);		
   					passwordNew.setText(null);
   					errorInImport.setText(null);
   					errorInImport.setVisibility(View.GONE);
   					rgUserType1.check(R.id.radioProvider);
   					importcertificateDialog.dismiss();
   				}
   			});
        
        
	}
   
	
	public  void upgrade()
	{
		String requestEnvelope =	RequestBase.getVersionRequest();   		
		new ApplicationVersionAsync().execute(requestEnvelope);
			
	 /*	 Calendar cal = Calendar.getInstance();
	    cal.add(Calendar.SECOND, 10);
	    Intent intent = new Intent(UserLogin.this, Service_class.class);
	    PendingIntent pintent = PendingIntent.getService(UserLogin.this, 0, intent,
	            0);
	    AlarmManager alarm = (AlarmManager) getSystemService(Context.ALARM_SERVICE);
	    alarm.setRepeating(AlarmManager.RTC_WAKEUP, cal.getTimeInMillis(),
	    		Constants.upgradeTime, pintent);
	    Intent startUpgradeService =new Intent(getBaseContext(), Service_class.class);
	 //   startUpgradeService.putExtra("Version", version);
        startService(startUpgradeService);
		*/

	}
	
	private String makeRequest()
	{
		int selectedId = rgUserType.getCheckedRadioButtonId();
		RadioButton  radioUserButton = (RadioButton) findViewById(selectedId);
		
		try{
		
			if (radioUserButton.getText().toString().equals(getResources().getString(R.string.Provider)))
				return	RequestBase.loginRequest(edt_EmailID.getText().toString(),edt_Password.getText().toString(),Constants.strPROVIDER );
			else if(radioUserButton.getText().toString().equals(getResources().getString(R.string.Patient)))
				return	RequestBase.loginRequest(edt_EmailID.getText().toString(),edt_Password.getText().toString(),Constants.strPATIENT);
			}
		catch (Exception e) {
			return null;
		
		}
		return null;
	}

	
	private class FetchingCertificateAsync extends AsyncTask<String, Void, String>
	 {
		 @Override
		protected String doInBackground(String... params) {
	
			String request = params[0];	
			DefaultHttpClient httpClient= new DefaultHttpClient();
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL, XmlConstants.ACTION_GET_USERCERTIFICATE, request,httpClient);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);	
			  	
			 	
			 	Logger.debug("User Login-->FetchingCertificateAsync-->",""+result);
			 
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
					RegisterationResult  registrationResult =BaseParser.parseGetCertificateResult(resultDoc);
					try {
					if(registrationResult.getResult().IsSuccess.equals("true"))
					{
						removeDialog(Constants.FETCHING_CERTIFICATE_DIALOG);
					
						if(registrationResult.getCertificate().length()>0)
						{
						 boolean isImportSuccess=	MHISEUtil.saveImportedCertificateToDevice(registrationResult.getCertificate(),certPWD,getApplicationContext(),emailID1+".p12");
						
						 if(isImportSuccess)
						 {
							 importcertificateDialog.dismiss();
							 
							 SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
							    SharedPreferences.Editor editor = sharedPreferences.edit();							   
							    editor.putString(Constants.KEY_CERT_NAME,emailID1+".p12");	
							   // editor.putString(Constants.KEY_PKCS12_PASSWORD,certPWD);
							    editor.commit();
							    
								Toast.makeText(getApplicationContext(),
										getResources().getString(R.string.ImportSuccessfull) ,
									     Toast.LENGTH_LONG).show();
						 }
						 else
						 {
							 errorInImport.setVisibility(View.VISIBLE);
							 errorInImport.setText(getResources().getString(R.string.WrongCertificatePassword));
						 }
						}
						else
						{
							removeDialog(Constants.FETCHING_CERTIFICATE_DIALOG);
							errorInImport.setVisibility(View.VISIBLE);
							errorInImport.setText(registrationResult.getResult().ErrorMessage);

						}
						
					}
					else
					{
						removeDialog(Constants.FETCHING_CERTIFICATE_DIALOG);
						errorInImport.setVisibility(View.VISIBLE);
						errorInImport.setText(registrationResult.getResult().ErrorMessage);
					}
						
					}
					catch (NullPointerException e) {
						// TODO: handle exception
						Logger.debug("UserLogin-->FetchingCertificateAsync",""+e);
					}
					
					
					/*	
				 		removeDialog(Constants.LOGIN_PROGRESS_DIALOG);
						Intent callCert = new Intent(UserLogin.this,CertificateInitializer.class);
						callCert.putExtra("CertificateSerialNumber",authenticateUserResponse.getCertificateSerialNumber() );
						startActivity(callCert);
						UserLogin.this.finish();*/

				}
		 }
		}
	
	
	
	private class UserLoginAsync extends AsyncTask<String, Void, String>
	 {
		 @Override
		protected String doInBackground(String... params) {
			String request = params[0];	
			DefaultHttpClient httpClient= new DefaultHttpClient();
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL, XmlConstants.ACTION_LOGIN, request,httpClient);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);	
			  	
			 	
			 	Logger.debug("UserLoginAsync-->web service response",""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
					AuthenticateUserResponse authenticateUserResponse =	new GetMasterDataParser().parseAuthenticateUserResponse(resultDoc);
					try{
					if(authenticateUserResponse.getResult().IsSuccess.equals("true"))
					{	
						SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);     
						Editor editor =sharedPreferences.edit();
						editor.putString(Constants.KEY_CERT_NAME, edt_EmailID.getText().toString()+".p12");
						editor.commit();
						Log.e("user login","user login 1");
						boolean isInstalled =CertificateInitializer.isInstalled(UserLogin.this);
			    	 	if(isInstalled)
				        {			   
						/*SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);     
						Editor editor =sharedPreferences.edit();
						editor.putString(Constants.KEY_CERT_NAME, edt_EmailID.getText().toString()+".p12");
						editor.commit();*/
						KeyStore  keyStore =MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext());
						Enumeration<String> aliases =null;
						try {
							aliases = keyStore.aliases();
						} catch (KeyStoreException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
						
						boolean isInstalledCertificateValid = false;
						
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

							 Log.i("installed certificate serial number", cert.getSerialNumber().toString(16));
							 Log.i("login response serial number ", ""+authenticateUserResponse.getCertificateSerialNumber());
							BigInteger big = new BigInteger(authenticateUserResponse.getCertificateSerialNumber(),16) ;
							
						//	Log.i("Login user serial number", ""+big);  
						  if (cert.getSerialNumber().toString().equals(big.toString()))
						  {
							  SharedPreferences sharedPreferences1 =getApplicationContext().getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
			  	    		    SharedPreferences.Editor editor1 = sharedPreferences1.edit();
	
			  	    		    editor1.putString(Constants.KEY_SERIAL_NUMBER, ""+authenticateUserResponse.getCertificateSerialNumber());
			  	    		    editor1.commit();
							  isInstalledCertificateValid = true; 
						  }
						}

						if (isInstalledCertificateValid)
						
						{
							removeDialog(Constants.LOGIN_PROGRESS_DIALOG);
							Intent callCert = new Intent(UserLogin.this,CertificateInitializer.class);
							callCert.putExtra("CertificateSerialNumber",new BigInteger(authenticateUserResponse.getCertificateSerialNumber(),16).toString());
							startActivity(callCert);
							//UserLogin.this.finish();
						}
						else
						{
							removeDialog(Constants.LOGIN_PROGRESS_DIALOG);
							MHISEUtil.displayDialog(UserLogin.this,getResources().getString(R.string.error_Invalid_Certificate_Mismatch_msg),getResources().getString(R.string.error_Invalid_Certificate_Mismatch_Title));
						}
				      }
			    	 	else
			    	 	{
			    	 		removeDialog(Constants.LOGIN_PROGRESS_DIALOG);
							//Intent callCert = new Intent(UserLogin.this,CertificateInitializer.class);
							//callCert.putExtra("CertificateSerialNumber",new BigInteger(authenticateUserResponse.getCertificateSerialNumber(),16).toString() );
							//startActivity(callCert);
			    	 		Log.e("get reso",getResources().getString(R.string.error_Invalid_Certificate_Mismatch_Title));
			    	 		MHISEUtil.displayDialog(UserLogin.this,getResources().getString(R.string.error_Invalid_Certificate_Mismatch_msg),getResources().getString(R.string.error_Invalid_Certificate_Mismatch_Title));
							//UserLogin.this.finish();	
			    	 	}
						
			    	 	/*	
				 		removeDialog(Constants.LOGIN_PROGRESS_DIALOG);
						Intent callCert = new Intent(UserLogin.this,CertificateInitializer.class);
						callCert.putExtra("CertificateSerialNumber",authenticateUserResponse.getCertificateSerialNumber() );
						startActivity(callCert);
						UserLogin.this.finish();
						*/
						
						
					}
					else if(authenticateUserResponse.getResult().IsSuccess.equals("false"))
					{
						removeDialog(Constants.LOGIN_PROGRESS_DIALOG);
						MHISEUtil.displayDialog(UserLogin.this, authenticateUserResponse.getResult().ErrorMessage,authenticateUserResponse.getResult().ErrorCode);	    
					
					}
					}
					catch (NullPointerException e) {
						removeDialog(Constants.LOGIN_PROGRESS_DIALOG);
						Logger.debug("RegisterProvider-->GetTypeClass", "NullPointerException"+e);
					}
				}
		 }
		}
	
	
	 @Override
	protected Dialog onCreateDialog(int id) {
		    	
		        switch (id) {
      	
		            case   Constants.LOGIN_PROGRESS_DIALOG:
		        	{
		        		 ProgressDialog dialog = new ProgressDialog(this);
		        		 dialog.setMessage("Logging in ..");          		 
		        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
		        		 //Logger.debug("RegisterProvider-->inside on create dialog", "true");
		        		 dialog.setCancelable(true);
		        		 dialog.setCanceledOnTouchOutside(false);
		        		 dialog.setOnCancelListener(new OnCancelListener() {
	  						
	  						@Override
	  						public void onCancel(DialogInterface dialog) {
	  							// TODO Auto-generated method stub
	  							dialog.dismiss();
	  							userLoginAsync.cancel(true);
	  						
	  						}
	  					});	
				       		 
		        		 return dialog;
		        		 
		        	}
		            case   Constants.ACTIVATING_USER_DIALOG:
		        	{
		        		 ProgressDialog dialog = new ProgressDialog(this);
		        		 dialog.setMessage(getResources().getString(R.string.AccountActivate));          		 
		        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
		        		 //Logger.debug("RegisterProvider-->inside on create dialog", "true");
		        		 dialog.setCancelable(true);
		        		 dialog.setCanceledOnTouchOutside(false);
		        		 dialog.setOnCancelListener(new OnCancelListener() {
	  						
	  						@Override
	  						public void onCancel(DialogInterface dialog) {
	  							// TODO Auto-generated method stub
	  							
	  							activateUserDialog.dismiss();
	  							try{
	  								checkUserActivationAsync.cancel(true);
	  								getCSRDetailsAsync.cancel(true);
	  								activateUserAsync.cancel(true);
	  								
	  								}
	  							catch (NullPointerException e) {
									
								}
	  							//userLoginAsync.cancel(true);
	  						
	  						}
	  					});	
				       		 
		        		 return dialog;
		        		 
		        	}
		        	
		            case   Constants.FETCHING_CERTIFICATE_DIALOG:
		        	{
		        		 ProgressDialog dialog = new ProgressDialog(this);
		        		 dialog.setMessage("In Progress..");          		 
		        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
		        		 //Logger.debug("RegisterProvider-->inside on create dialog", "true");
		        		 dialog.setCancelable(true);
		        		 dialog.setCanceledOnTouchOutside(false);
		        		 dialog.setOnCancelListener(new OnCancelListener() {
	  						
	  						@Override
	  						public void onCancel(DialogInterface dialog) {
	  							// TODO Auto-generated method stub
	  							dialog.dismiss();
	  							fetchingCertificateAsync.cancel(true);
	  						
	  						}
	  					});	
				       		 
		        		 return dialog;
		        		 
		        	}
		        	
		            case  Constants.UPGRADE_DIALOG:
		        	{
		        		AlertDialog.Builder importbuilder = new AlertDialog.Builder(this);
					 	importbuilder.setTitle(getResources().getString(R.string.appUpgradeTitle));
					 	importbuilder.setMessage(getResources().getString(R.string.appUpgradeMessage));
					 	importbuilder.setPositiveButton("Download apk",new DialogInterface.OnClickListener() {
					
							@Override
							public void onClick(DialogInterface dialog, int which) {
								
								/*	Intent intent = new Intent(Intent.ACTION_VIEW);
									intent.setDataAndType( Uri.parse("http://10.0.30.88:94/"), "application/vnd.android.package-archive");
									startActivity(intent);  
								*/
								
								
								 Intent updateIntent = new Intent(Intent.ACTION_VIEW,
						     		       Uri.parse(Constants.updateURL));
						     	   updateIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
						     		startActivity(updateIntent); 
								
							}
						});
					
					 	importbuilder.create().show();

		        	}
		        	
		        }
				return null;
	 }
	
		private class CheckUserActivationAsync extends AsyncTask<String, Void, String>
		 {
			 @Override
			protected String doInBackground(String... params) {
		
				String request = params[0];	
				DefaultHttpClient httpClient= new DefaultHttpClient();
				String response = com.mhise.util.MHISEUtil.CallWebService(
						Constants.URL, XmlConstants.ACTION_LOGIN, request,httpClient);
				return response;
			}
			 
			 @Override
			protected void onPostExecute(String result) {
				// TODO Auto-generated method stub 
				 super.onPostExecute(result);	
				  	
				 	
				 	Logger.debug("RegisterProvider-->web service response",""+result);
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{
						AuthenticateUserResponse authenticateUserResponse =	new GetMasterDataParser().parseAuthenticateUserResponse(resultDoc);
						try{
						if(authenticateUserResponse.getResult().IsSuccess.equals("true"))
						{	
							removeDialog(Constants.ACTIVATING_USER_DIALOG);
							errorActivate.setVisibility(View.VISIBLE);
							errorActivate.setText("This Account is already activated.");
							
						}
						else if(authenticateUserResponse.getResult().IsSuccess.equals("false"))
						{
							if(authenticateUserResponse.getResult().ErrorCode.equals("Inactive_Account") )
							{
								if(ActivateUserEmailID.length()>0)
								{
									if(activateUserPWD.length()>0)
									{
										int selectedId = rgActivateUserType.getCheckedRadioButtonId();
										RadioButton  radioUserButton = (RadioButton) findViewById(selectedId);
										String request =null;
										if (radioUserButton.getText().toString().equals(getResources().getString(R.string.Provider)))
											request=	RequestBase.getCSRDetailRequest(ActivateUserEmailID,"Provider" );
										else if(radioUserButton.getText().toString().equals(getResources().getString(R.string.Patient)))
											request=	RequestBase.getCSRDetailRequest(ActivateUserEmailID,"Patient");					
										getCSRDetailsAsync=new	GetCSRDetailsAsync();
										
										getCSRDetailsAsync.execute(request);
									}
									else
									{
										removeDialog(Constants.ACTIVATING_USER_DIALOG);
										errorActivate.setVisibility(View.VISIBLE);
										errorActivate.setText(getResources().getString(R.string.newPasswordEmpty));
									}
							
									
								}
								else
								{
									removeDialog(Constants.ACTIVATING_USER_DIALOG);
									errorActivate.setVisibility(View.VISIBLE);
									errorActivate.setText(getResources().getString(R.string.emptyEmpty));
								
								}
								
								
							}
							else
							{
								removeDialog(Constants.ACTIVATING_USER_DIALOG);
								errorActivate.setVisibility(View.VISIBLE);
								errorActivate.setText(authenticateUserResponse.getResult().ErrorMessage);
							}
								    
						
						}
						}
						catch (NullPointerException e) {
							removeDialog(Constants.ACTIVATING_USER_DIALOG);
							Logger.debug("RegisterProvider-->GetTypeClass", "NullPointerException"+e);
						}
					}
			 }
			}
	 	
		private class ActivateUserAsync extends AsyncTask<String, Void, String>
		{
			protected String doInBackground(String... params) {
				
				String request = params[0];	
				DefaultHttpClient httpClient= new DefaultHttpClient();
				String response = com.mhise.util.MHISEUtil.CallWebService(
						Constants.URL, XmlConstants.ACTION_ACTIVATE_USER, request,httpClient);
				return response;
			}
			 
			 @Override
			protected void onPostExecute(String result) {
				// TODO Auto-generated method stub 
				 super.onPostExecute(result);	
				  	
				 	Logger.debug("User Login-->GetUserDetailsAsync-->",""+result);
				 	Log.i("ActivateUserAsync",""+result);
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{
						RegisterationResult activateUser =BaseParser.parseActivateUserResponse(resultDoc);						
						if(activateUser.getResult().IsSuccess.equals("true"))
						{
							if(activateUser.getCertificate().length()>0)
			    			{
						
								
			    				@SuppressWarnings("static-access")
								PrivateKey pKey =gCSR.privateKey;
			    				StringBuffer certificateBuffer = new StringBuffer();
			    	    		certificateBuffer.append(CertificateInitializer.TAG_BEGIN_CERTIFICATE);
			    	    		certificateBuffer.append(activateUser.getCertificate());
			    	    		certificateBuffer.append(CertificateInitializer.TAG_END_CERTIFICATE);
			    	    		
			    	    		
			    	    		
//<<<<<<< .mine
			    	    		
			    	    		
			    				installActivateResponseCertificate(certificateBuffer.toString(),pKey, MHISEUtil.getStrongPassword(ActivateUserEmailID),ActivateUserEmailID);
//=======
			    	    		
			    	    		
		/*	    				installActivateResponseCertificate(certificateBuffer.toString(),pKey, MHISEUtil.getStrongPassword(ActivateUserEmailID));
//>>>>>>> .r4471
*/			    				
			    				SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
							    SharedPreferences.Editor editor = sharedPreferences.edit();
//<<<<<<< .mine
							   // editor.putString(Constants.KEY_PKCS12_PASSWORD,activateUserPWD);
							    editor.putString(Constants.KEY_CERT_NAME,ActivateUserEmailID+".p12");
//=======
			/*				   // editor.putString(Constants.KEY_PKCS12_PASSWORD,activateUserPWD);
							    editor.putString(Constants.KEY_CERT_NAME,ActivateUserEmailID);
>>>>>>> .r4471*/
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
			    	    		
/*<<<<<<< .mine
			    	    		
			    	    		
			    				
=======
			*/    	    		
			    	    	/*	installActivateResponseCertificate(certificateBuffer.toString(),pKey, MHISEUtil.getStrongPassword(ActivateUserEmailID),ActivateUserEmailID);
			    			//	installActivateResponseCertificate(certificateBuffer.toString(),pKey, MHISEUtil.getStrongPassword(ActivateUserEmailID));

			    				
			    				SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
							    SharedPreferences.Editor editor = sharedPreferences.edit();

							   // editor.putString(Constants.KEY_PKCS12_PASSWORD,activateUserPWD);
							    editor.putString(Constants.KEY_CERT_NAME,ActivateUserEmailID+".p12");*/

							    editor.commit();
							    
								Toast.makeText(getApplicationContext(),
										"Your request has been sent to system administrator for activation." ,
									     Toast.LENGTH_LONG).show();
								
								removeDialog(Constants.ACTIVATING_USER_DIALOG);
								activateUserDialog.dismiss();
								
			    			}
						}
						else
						{
							errorActivate.setText(activateUser.getResult().ErrorMessage);
						}	
					}
			 }
			
		}
	 
	private void installActivateResponseCertificate(String certificate,PrivateKey pKey,String strPassword,String email)
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
		/*	OutputStream fos = getApplicationContext().openFileOutput(Constants.defaultP12StoreName, Context.MODE_PRIVATE);     
			keyStore.store(fos,password);
			fos.close();*/
			
			//Store keystore in external storage
		     File newKeystoreFile = new File(Constants.defaultP12StorePath + email+".p12");            
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
  	    		 Log.i("Activated certificate serial number", ""+cert.getSerialNumber().toString(16));
  	    		  editor.putString(Constants.KEY_SERIAL_NUMBER, ""+cert.getSerialNumber().toString(16));
  	    		  editor.commit();
				  //isInstalledCertificateValid = true; 
			  
			}

		 }
		 catch (Exception e) {
			// TODO: handle exception
			 
			 Logger.debug("Exception installP7BCertificate", ""+e);
		}
	}
	 
	private class ApplicationVersionAsync extends AsyncTask<String, Void, String>
	 		{
			 @Override
			protected String doInBackground(String... params) {
		
				String request = params[0];	
				DefaultHttpClient httpClient= new DefaultHttpClient();
				String response = com.mhise.util.MHISEUtil.CallWebService(
						Constants.URL, XmlConstants.ACTION_GET_APPLICATION_VERSION, request,httpClient);
				return response;
			}
			 
			 @Override
			protected void onPostExecute(String result) {
				// TODO Auto-generated method stub 
				 super.onPostExecute(result);	
				 Log.i("Login application version-->web service response",""+result);
				 	org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);	
				 	String version =	BaseParser.parseApplicationVersionResponse(resultDoc);
				try{
				 	versionInstalled = MHISEUtil.getVersionName(getApplicationContext());
				 	
				 	Log.e("versionInstalled",""+versionInstalled);
				 	Log.e("version",""+version);
				 	
				 	if (versionInstalled <Double.parseDouble(version))
				 	{
				 		//Show available Upgrade option
				 		showDialog(Constants.UPGRADE_DIALOG);
				 	}
				}
				catch (NullPointerException e) {
					// TODO: handle exception
					Logger.debug("UserLogin->ApplicationVersionAsync->onPostExecute",""+e);
					Log.i("UserLogin->ApplicationVersionAsync->onPostExecute",""+e);
				}
				 	
				 	/* 	 Calendar cal = Calendar.getInstance();
					    cal.add(Calendar.SECOND, 10);
					    Intent intent = new Intent(UserLogin.this, Service_class.class);
					    PendingIntent pintent = PendingIntent.getService(UserLogin.this, 0, intent,
					            0);
					    AlarmManager alarm = (AlarmManager) getSystemService(Context.ALARM_SERVICE);
					    alarm.setRepeating(AlarmManager.RTC_WAKEUP, cal.getTimeInMillis(),
					    		Constants.upgradeTime, pintent);
					    Intent startUpgradeService =new Intent(getBaseContext(), Service_class.class);
					    startUpgradeService.putExtra("Version", version);
			            startService(startUpgradeService);*/
					
			 }
			}
		
	 		
	 	
	 
	 
	 
}