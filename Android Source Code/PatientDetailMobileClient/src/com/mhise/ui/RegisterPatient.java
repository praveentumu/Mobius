package com.mhise.ui;

import java.security.PrivateKey;
import java.util.Calendar;
import java.util.HashMap;

import org.apache.http.impl.client.DefaultHttpClient;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnCancelListener;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnFocusChangeListener;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.mhise.constants.Constants;
import com.mhise.model.Address;
import com.mhise.model.City;
import com.mhise.model.Demographics;
import com.mhise.model.Patient;
import com.mhise.model.PersonName;
import com.mhise.model.RegisterationResult;
import com.mhise.model.Result;
import com.mhise.model.State;
import com.mhise.model.Telephone;
import com.mhise.requests.RequestBase;
import com.mhise.response.BaseParser;
import com.mhise.security.GenerateCSR;
import com.mhise.util.DataConnectionManager;
import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;


public class RegisterPatient extends Activity implements View.OnClickListener {

	private EditText firstName;
	private EditText middleName;
	private EditText lastName;
	private EditText socialSecurityNumber;
	private EditText dob;
	private EditText edtemail;
	private EditText motherFamilyName;
	private EditText motherGivenName;
	private EditText motherMiddleName;
	private EditText motherSuffix;
	private EditText data;
	private EditText extension;
	private EditText addressLine1;
	private EditText addressLine2;
	private EditText zip;
	private EditText password;
	private TextView txtcity;
	private TextView txtstate;
	private TextView txtcsr;
	
	private RadioGroup rg_Gender;
	private RadioButton rb_Male ,rb_Female;	
	private Spinner spnr_Type;
	private Button btnRegister,btnCancle,btnGenerateCsr;
	private ImageView imgDOB;
	private PrivateKey privateKey;
	private int mYear;
	private int mMonth;
	private int mDay;
	private static String previousZip=null;
	private GetLocalityClass getLocalityClass;
	private PatientRegistrationAsync patientRegistrationAsync;
	
//	DefaultHttpClient httpClient = new DefaultHttpClient(); 
	 
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		
		loadUI();
		setLocalityInfo(zip);
	}
	
	private class PatientRegistrationAsync extends AsyncTask<String, Void, String>
	 {		
		 @Override
		protected String doInBackground(String... params) {
	
			String request = params[0];

		Logger.debug("RegisterPatient-->request comes in async class", ""+request);
//			DefaultHttpClient httpClient = new DefaultHttpClient(); 
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL,Constants.SOAP_ACTION_FOR_ADD_PATIENT, request, new DefaultHttpClient());
			
			return response;
		}
		 
		 @Override 
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);		
			 Logger.debug("RegisterPatient-->response for Provider registration","response"+result);
			 
			 removeDialog(Constants.REGISTER_PATIENT_DIALOGUE);
			// writeToFile(result, "PatientRegistrationRespnose.txt");
		 			 //	Logger.debug("RegisterPatient-->web service response",""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				
				if(resultDoc != null)
				{
					RegisterationResult regResult  =	BaseParser.parseAddProviderResult(resultDoc,Constants.PATIENT);
					Result result1 =regResult.getResult();
				if(result1!=null)
				{
					if(result1.IsSuccess.equals("false"))
						MHISEUtil.displayDialog(RegisterPatient.this, result1.ErrorMessage,result1.ErrorCode);	
					else
					{	
						try{
							
							   Toast.makeText(getApplicationContext(),
									   result1.ErrorMessage,
									     Toast.LENGTH_LONG).show();
							
							SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
					
						    SharedPreferences.Editor editor = sharedPreferences.edit();						  
						    editor.putString(Constants.KEY_CERT_NAME,email+".p12");				
						    editor.commit();
						    
							String certificate =regResult.getCertificate();
							Intent certificateIntent = new Intent(RegisterPatient.this,CertificateInitializer.class);
							certificateIntent.putExtra(Constants.TAG_CERTIFICATE,certificate);
							certificateIntent.putExtra(Constants.TAG_PRIVATEKEY, privateKey);
							
					
							//certificateIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
							//certificateIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
							//certificateIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
							//certificateIntent.setFlags( Intent.FLAG_ACTIVITY_CLEAR_TOP|Intent.FLAG_ACTIVITY_SINGLE_TOP);
							
							startActivity(certificateIntent);
							finish();
							
						}
						catch (NullPointerException e) {
							// TODO: handle exception
							Logger.debug("PatientRegistrationAsync", "-->"+e);
						}
						
					}
				
				}	
					
					}
		 }	
		}
	
	
	final int MIN_TEXT_LENGTH=2;
	private String organizationname;
	String firstname;// =firstName.getText().toString().trim();
    String lastname;//=lastName.getText().toString().trim();
    String commonname;//=firstname+lastname;
    String email;//=edtemail.getText().toString().trim();
    String organizationalunit;// =Constants.organizationalunit;
    String city;//=txtcity.getText().toString().trim();
    String state;//= txtstate.getText().toString().trim();                      
    String country;// =Constants.Country;
    GenerateCSRClass generateCSRClass;
	
	
	@Override
	public void onClick(View v) {
		
		switch(v.getId())
		{
			case R.id.btnregisterprovider:
				
				boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
				  if(!isDataConnectionAvailable)
		    		{
		    		MHISEUtil.displayDialog(RegisterPatient.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
		    		}
		    		else
		    		{	
				
											String request =makeRequest();
											patientRegistrationAsync=new PatientRegistrationAsync();
											patientRegistrationAsync.execute(request);
		    		}
			                            break;
			case R.id.btnGenerateCSR:
			{
							
				 firstname =firstName.getText().toString().trim();
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
					break;
				}
			case R.id.btncancel:
										ResetValues();
										break;
										
			case R.id.imgDOB:
										showDialog(Constants.DATE_DIALOG_ID);
										break;
		}
		
	}

	
	
	private void loadUI()
	{
		setContentView(R.layout.patient_registration);
		firstName=(EditText) findViewById(R.id.edtfirstname);
		middleName=(EditText) findViewById(R.id.edtmiddlename);
		lastName=(EditText) findViewById(R.id.edtlastname);
		socialSecurityNumber=(EditText) findViewById(R.id.edtssn);
		dob=(EditText) findViewById(R.id.edtdob);
		edtemail=(EditText) findViewById(R.id.edtuseremail);
		motherFamilyName=(EditText) findViewById(R.id.Mothers_Family_Name);
		motherGivenName=(EditText) findViewById(R.id.Mothers_Given_Name);
		motherMiddleName=(EditText) findViewById(R.id.Mothers_Middle_Name);
		motherSuffix=(EditText) findViewById(R.id.Mothers_Suffix);
		password = (EditText) findViewById(R.id.edtPassword);
		data=(EditText) findViewById(R.id.edtData);
		extension=(EditText) findViewById(R.id.edtExtension);
		addressLine1=(EditText) findViewById(R.id.edtAddressLine1);
		addressLine2=(EditText) findViewById(R.id.edtAddressLine2);
		zip=(EditText) findViewById(R.id.edtzip);
		txtcity=(TextView) findViewById(R.id.txtCity);
		txtstate=(TextView) findViewById(R.id.txtSate);
		txtcsr=(TextView) findViewById(R.id.txtCSR);
		btnRegister=(Button) findViewById(R.id.btnregisterprovider);
		btnCancle=(Button) findViewById(R.id.btncancel);

		btnGenerateCsr=(Button) findViewById(R.id.btnGenerateCSR);
		imgDOB =(ImageView)findViewById(R.id.imgDOB);
		spnr_Type =(Spinner)findViewById(R.id.phone_spinner);
		
		spnr_Type.setFocusableInTouchMode(true);
		spnr_Type.setOnFocusChangeListener(new OnFocusChangeListener() {

	        @Override
	        public void onFocusChange(View arg0, boolean hasFocus) {
	            // TODO Auto-generated method stub
	            if(hasFocus){
	            	spnr_Type.performClick();
	            }
	        }
	    });
		
		rg_Gender =(RadioGroup)findViewById(R.id.radioSex);
		rb_Female=(RadioButton)findViewById(R.id.radiofemale);
		rb_Male=(RadioButton)findViewById(R.id.radiomale);
		
		imgDOB.setOnClickListener(this);
		btnRegister.setOnClickListener(this);
		btnCancle.setOnClickListener(this);
		btnGenerateCsr.setOnClickListener(this);

	}
	
	@Override
	protected Dialog onCreateDialog(int id) {  
		
		switch (id) 
        {
    
        	case Constants.DATE_DIALOG_ID:
							           final Calendar c = Calendar.getInstance();
							            mYear = c.get(Calendar.YEAR);
							            mMonth = c.get(Calendar.MONTH);
							            mDay = c.get(Calendar.DAY_OF_MONTH);              	
							            return new DatePickerDialog(this,
							                        mDateSetListener,
							                        mYear, mMonth, mDay);
        	case Constants.REGISTER_PATIENT_DIALOGUE:{
						        		ProgressDialog dialog = new ProgressDialog(this);
						       		 	dialog.setMessage("Registering Patient");          		 
						       		 	dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
						       		 	//Logger.debug("RegisterPatient-->inside on create dialog Register Patient", "true");
						       		 	dialog.setCancelable(true);
						       		 dialog.setCanceledOnTouchOutside(false);
						       		 	dialog.setOnCancelListener(new OnCancelListener() {
 			    						
 			    						@Override
 			    						public void onCancel(DialogInterface dialog) {
 			    							// TODO Auto-generated method stub
 			    							dialog.dismiss();
 			    							patientRegistrationAsync.cancel(true); 			    						
 			    						}
 			    					});	
						       		 	return dialog;
        	}
        	
        	
          	case Constants.GENERATE_CSR_PROGRESS_DIALOG:{
        		ProgressDialog dialog = new ProgressDialog(this);
       		 	dialog.setMessage("Generating Certificate Signing Request..");          		 
       		 	dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
       		 	
       		 	dialog.setCancelable(true);
       		 dialog.setCanceledOnTouchOutside(false);
       		 	dialog.setOnCancelListener(new OnCancelListener() {
					
					@Override
					public void onCancel(DialogInterface dialog) {
						// TODO Auto-generated method stub
						dialog.dismiss();
						generateCSRClass.cancel(true); 			    						
					}
				});	
       		 	return dialog;
}
        	
            
        	case Constants.GET_LOCALITY_PROGRESS_DIALOG:{
        								ProgressDialog dialog = new ProgressDialog(this);
        								dialog.setMessage("Fetching Locality Details");          		 
        								dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
        							//	Logger.debug("RegisterPatient-->inside on create dialog Register Patient", "true");
        								dialog.setCancelable(true);
        								 dialog.setCanceledOnTouchOutside(false);
        								 dialog.setOnCancelListener(new OnCancelListener() {
        			    						
        			    						@Override
        			    						public void onCancel(DialogInterface dialog) {
        			    							// TODO Auto-generated method stub
        			    							dialog.dismiss();
        			    							try {
        			    							getLocalityClass.cancel(true);
        			    							}
        			    							catch (Exception e) {
														// TODO: handle exception
													}
        			    						}
        			    					});
        								return dialog;
        	}        	        	
        	
        }
		return null;
	}

	@Override
	protected void onPrepareDialog(int id, Dialog dialog) {
		switch (id) 
		{
		        case Constants.DATE_DIALOG_ID:
		        {
		            ((DatePickerDialog) dialog).updateDate(mYear, mMonth, mDay);
		            break;
		        }
		}
	}

	private void updateDisplay() {
	    	mMonth =mMonth+1;
	    	String _mMonth;
	    	String _mDay ;
	    	
	    	if(mMonth<10)
	    		{
	    			_mMonth = "0"+String.valueOf(mMonth);
	    		}
		    	else
		    	{
		    		_mMonth = String.valueOf(mMonth);
		    	}
		    	if(mDay <10)
		    	{
		    		_mDay = "0"+String.valueOf(mDay);
		    	}
		    	else
		    	{
		    		_mDay = String.valueOf(mDay);
		    	}
		    	
		    	dob.setText(
	        		
	          new StringBuffer()
	                    // Month is 0 based so add 1
	                    .append(_mMonth).append("/")
	                    .append(_mDay).append("/")
	                    .append(mYear)
	                    );
	    	}

	private DatePickerDialog.OnDateSetListener mDateSetListener =
	            new DatePickerDialog.OnDateSetListener() {

	                public void onDateSet(DatePicker view, int year, int monthOfYear,
	                        int dayOfMonth) {
	                    mYear = year;
	                    mMonth = monthOfYear;
	                    mDay = dayOfMonth;
	                    updateDisplay();
	                }
	            };

	
	private void ResetValues() {
		
		firstName.setText("");
		middleName.setText("");
		lastName.setText("");		
		socialSecurityNumber.setText("");
		dob.setText("");
		edtemail.setText("");
		motherFamilyName.setText("");
		data.setText("");
		extension.setText("");
		addressLine1.setText("");
		addressLine2.setText("");
		zip.setText("");
		txtcity.setText("");
		txtstate.setText("");
		txtcsr.setText("");
		rb_Female.setSelected(false);
		password.setText(null);
		rb_Male.setSelected(true);
		spnr_Type.setSelection(0);
		privateKey =null;
		previousZip=null;
		
		rg_Gender.check(R.id.radiomale);
		
	}


	
	private String makeRequest()
	{	
		showDialog(Constants.REGISTER_PATIENT_DIALOGUE);
		Patient  patient= new Patient();
		
		Demographics patientDemographics = new Demographics();
		patientDemographics.setFamilyName(lastName.getText().toString().trim());
		patientDemographics.setGivenName(firstName.getText().toString().trim());
		patientDemographics.setMiddleName(middleName.getText().toString().trim());
		patientDemographics.setDOB(dob.getText().toString().trim());
		int selectedId = rg_Gender.getCheckedRadioButtonId();
	    RadioButton  radioSexButton = (RadioButton) findViewById(selectedId);
		patientDemographics.setGender(radioSexButton.getText().toString().trim());
		patientDemographics.setSSN(socialSecurityNumber.getText().toString().trim());
		PersonName mothersMaiden = new PersonName();
		mothersMaiden.setFamilyName(motherFamilyName.getText().toString().trim());
		mothersMaiden.setGivenName(motherGivenName.getText().toString().trim());
		mothersMaiden.setMiddleName(motherMiddleName.getText().toString().trim());
		mothersMaiden.setSuffix(motherSuffix.getText().toString().trim());
		patientDemographics.setMotherMaiden(mothersMaiden);	
		patient.setDemographics(patientDemographics);	
		Telephone[] arrpatientTelephoneDetails= new Telephone[1];
		Telephone patientTelephoneDetails = new Telephone();
		arrpatientTelephoneDetails[0] =patientTelephoneDetails ;
		patientTelephoneDetails.setNumber(data.getText().toString().trim());
		patientTelephoneDetails.setExtensionnumber(extension.getText().toString().trim());
		patientTelephoneDetails.setType(spnr_Type.getSelectedItem().toString().trim());
		patient.setTelephone(arrpatientTelephoneDetails);	
		patient.setPassword(password.getText().toString().trim());
		Address[] arrAddress= new Address[1];
		Address address = new Address();
		arrAddress[0]=address;
		address.setZip(zip.getText().toString().trim());
		address.setAddressLine1(addressLine1.getText().toString().trim());
		address.setAddressLine2(addressLine2.getText().toString().trim());
		City city = new City();
		city.setCityName(txtcity.getText().toString().trim());
		State state = new State();
		state.setStateName(txtstate.getText().toString().trim());
		city.setState(state);
		address.setCity(city);
		patient.setAddress(arrAddress);
		
		patient.setCSR(txtcsr.getText().toString().trim());
		patient.setEmailAddress(edtemail.getText().toString().trim());

		
		String request = RequestBase.getPatientRegistrationRequest(patient);
		Logger.debug("RegisterPatient-->request generation for add patient", "patient"+request);
		
		return request;
		
	}
		
	private class GenerateCSRClass extends AsyncTask<String, Void, String>
	 {		
		GenerateCSR gCSR;
		 @Override
		protected String doInBackground(String... params) {
			 	
			 	 gCSR = new GenerateCSR();
			 	 String csr =null;
			 	 try { 
			 		 
			 		 
					 csr=gCSR.generatePKCS10(email,commonname,organizationalunit,organizationname,city,state,country);					 								  
			    	//Logger.debug(TAG ,"CSR------>"+csr);
			    
		    	}
			    	catch (Exception e) {
						// TODO: handle exception
			    		Logger.debug("Patient ->GenerateCSRClass" ,"Exception------>"+e);
					}
				return csr;
		    		}
		 
		 @SuppressWarnings("static-access")
		@Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub
			super.onPostExecute(result);
			txtcsr.setText(result);
			removeDialog(Constants.GENERATE_CSR_PROGRESS_DIALOG);
			privateKey =  gCSR.privateKey ;	
			

		}
	}
			
		

	
	private void setLocalityInfo(final EditText edt)
	{
		edt.setOnFocusChangeListener(new OnFocusChangeListener() {
			
			@Override
			public void onFocusChange(View v, boolean hasFocus) {
				// TODO Auto-generated method stub
				EditText edt =(EditText)v;
				  boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
				//  Logger.debug("RegisterPatient-->previous zip",""+previousZip);
				//  Logger.debug("RegisterPatient-->edittext value","==="+edt.getText().toString().trim());
				  if(edt.getText().length()>0 && hasFocus ==false )
			    	{	
					  //Logger.debug("RegisterPatient-->1previous zip",""+previousZip);
					  if(previousZip!=null)
					  {
						  if(!edt.getText().toString().trim().equals(previousZip))
						  {	
							//  Logger.debug("RegisterPatient-->2previous zip",""+previousZip);
							  previousZip =edt.getText().toString().trim();
							 
							  if(!isDataConnectionAvailable)
					    		{
					    		MHISEUtil.displayDialog(RegisterPatient.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
					    		}
					    		else
					    		{	 	
					    		showDialog(Constants.GET_LOCALITY_PROGRESS_DIALOG);
					    		String request = com.mhise.requests.RequestBase.getLocalityByZipRequest(edt.getText().toString().trim());
					    		//Logger.debug("RegisterPatient-->request",""+request);
					    		getLocalityClass = new GetLocalityClass();
								 getLocalityClass.execute(request);					
					    		}
						  }
						  
					  }
			    	
					  else
					  {
						 // Logger.debug("RegisterPatient-->else previous zip",""+previousZip);
						  previousZip=edt.getText().toString().trim();
						  if(!isDataConnectionAvailable)
				    		{
							 // Logger.debug("RegisterPatient-->1else previous zip",""+previousZip);
				    		MHISEUtil.displayDialog(RegisterPatient.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage) ,getResources().getString(R.string.DataConnectionAvailabiltyTitle));
				    		}
				    		else
				    		{	
				    			//Logger.debug("RegisterPatient-->2else previous zip",""+previousZip);
				    		showDialog(Constants.GET_LOCALITY_PROGRESS_DIALOG);	
				    		String request = com.mhise.requests.RequestBase.getLocalityByZipRequest(edt.getText().toString().trim());
				    		//Logger.debug("RegisterPatient-->request",""+request);
							new GetLocalityClass().execute(request);					

				    		}
					  }
				    	
					  	
			    	}
			}		
		});	
		
	}
			
	private class GetLocalityClass extends AsyncTask<String, Void, String>
		 {		
			 @Override
			protected String doInBackground(String... params) {
		
				String request = params[0];

				//Logger.debug("RegisterPatient-->request comes in async class", ""+request);
				DefaultHttpClient httpClient = new DefaultHttpClient(); 
				String response = com.mhise.util.MHISEUtil.CallWebService(
						Constants.URL, XmlConstants.ACTION_GETLOCALITYBYZIPCODE, request,httpClient);
				return response;
			}
			 
			 @Override
			protected void onPostExecute(String result) {
				// TODO Auto-generated method stub 
				 super.onPostExecute(result);		
				 	removeDialog(Constants.GET_LOCALITY_PROGRESS_DIALOG);
				 	Logger.debug("RegisterPatient-->web service response",""+result);
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{
			
						City city  =	BaseParser.parseGetLocalityByZipresult(resultDoc);
						//Logger.debug("RegisterPatient-->is  purpose class launched ?", ""+city.getResult().IsSuccess);
						if(city.getResult().IsSuccess.equals("true"))
						{	
						try
						{
							RegisterPatient.this.txtcity.setText(city.getCityName());
							RegisterPatient.this.txtstate.setText(city.getState().getStateName());		
							//strCountry=city.getState().getCountry().getCountryName();
						}
						catch (NullPointerException e) {
							// TODO: handle exception
							
							Logger.debug("null pointer exception at post execute of Locality async class", ""+e);
						}	
						}
						else if(city.getResult().IsSuccess.equals("false"))
						{
							
							MHISEUtil.displayDialog(RegisterPatient.this, city.getResult().ErrorMessage,city.getResult().ErrorCode);	    
							RegisterPatient.this.txtcity.setText(null);
							RegisterPatient.this.txtstate.setText(null);	
							RegisterPatient.this.zip.setText(null);
						}
						
					}
			 }	
			}
	
	 @Override
	protected void onStop() {
		// TODO Auto-generated method stub
		super.onStop();
		previousZip =null;
	}

	 @Override
		protected void onDestroy() {
		
			super.onDestroy();
			previousZip=null;
		}
}
