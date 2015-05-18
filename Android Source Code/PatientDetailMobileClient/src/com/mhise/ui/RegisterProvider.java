package com.mhise.ui;

import java.security.PrivateKey;
import java.util.ArrayList;
import java.util.HashMap;

import org.apache.http.impl.client.DefaultHttpClient;

import android.app.Activity;
import android.app.AlertDialog;
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
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnFocusChangeListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.mhise.constants.Constants;
import com.mhise.model.City;
import com.mhise.model.MasterData;
import com.mhise.model.ProviderRegistration;
import com.mhise.model.RegisterationResult;
import com.mhise.model.Result;
import com.mhise.requests.RequestBase;
import com.mhise.response.BaseParser;
import com.mhise.response.GetMasterDataParser;
import com.mhise.security.GenerateCSR;

import com.mhise.util.DataConnectionManager;
import com.mhise.util.Logger;

import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;


public class RegisterProvider extends Activity implements OnClickListener{
	
	//UI Components Name for Organizational Provider 
	Button btn_register,btn_Cancel,btn_GenerateCSR;
	EditText edt_OrganizationName,edt_DeliveryEmailAddress,
				edt_ZIP,edt_AddressLisne1,edt_AddressLisne2,
				edt_ElectronicServiceURI,edt_Contact,
				edt_FirstName,edt_Middle_Name,edt_LastName,
				edt_EmailID ,edt_Password;
	TextView txt_City,txt_State,txt_Speciality,txt_CSR;
	RadioGroup rg_Gender;
	RadioButton rb_Male,rb_Female;	
	Spinner spnr_Type;
	String strProviderRegistration;
	int paramsLength =20;
	protected boolean[] _selections ;
	protected ArrayList<String> _selectedSpecalities;
	protected String[] _specialities ;
	protected String[] _specialitiesCode ;
	private static String previousZip=null;
	private int PROVIDERTYPE;
	private String[] _arType;
	private String[] _arTypeID;
	private PrivateKey privateKey;
	DefaultHttpClient httpClient ;
	private GetSpecialityClass getSpecialityClass;
	private GetTypeClass getTypeClass;
	private GetProviderRegistrationAsync getProviderRegistrationAsync;
	private GetLocalityClass getLocalityClass;
	boolean clear=false;
	GenerateCSRClass generateCSRClass;
  String organizationname;                                        
  String email;
  String organizationalunit ;
  String commonname;
  String city;
  String state;                  
  String country;
  HashMap< String,String> hmpMap ;
     
      
      
      
	  
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		
		
		PROVIDERTYPE =this.getIntent().getIntExtra(Constants.PROVIDER_TYPE, 0);

		adjustUI();	
		initializeUIObjects();
		addEventToUIControls();
		fetchSpecialityList();
		fetchTypes();
		hmpMap = MHISEUtil.initializeEnumTypes();
	}
	
	
	
		
	private  void adjustUI()
	{
		if(PROVIDERTYPE == Constants.INDIVIDUAL_PROVIDER)
		{
		
			setContentView(R.layout.individual_provider_registration);
		}
		else
		{
			setContentView(R.layout.organizationalprovider_registration);
		}
	}

	
	private void initializeUIObjects()
	{
		
	
		switch(PROVIDERTYPE){
		
			case Constants.ORGANIZATIONAL_PROVIDER:
			{
				btn_Cancel = (Button)findViewById(R.id.btncancel);
				btn_register=(Button)findViewById(R.id.btnregisterprovider);
				btn_GenerateCSR=(Button)findViewById(R.id.btnGenerateCSR);	
				edt_AddressLisne1=(EditText)findViewById(R.id.edtaddressline1);
				edt_AddressLisne2=(EditText)findViewById(R.id.edtaddressline2);
				edt_DeliveryEmailAddress=(EditText)findViewById(R.id.edtdeliveryemailid);
				edt_Password =(EditText)findViewById(R.id.edtPassword);
				edt_ZIP=(EditText)findViewById(R.id.edtzip);
				edt_ElectronicServiceURI=(EditText)findViewById(R.id.edtElectronicServiceURI);
				edt_Contact=(EditText)findViewById(R.id.edtContact);
				txt_City=(TextView)findViewById(R.id.txtCity);
				txt_State=(TextView)findViewById(R.id.txtSate);
				txt_Speciality=(TextView)findViewById(R.id.txtSpeciality);
				txt_CSR=(TextView)findViewById(R.id.txtCSR);

				spnr_Type=(Spinner)findViewById(R.id.spnrType);
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
				edt_OrganizationName=(EditText)findViewById(R.id.edtOrgName);
				break;
			}
			case Constants.INDIVIDUAL_PROVIDER:
			{
		
	
				btn_Cancel = (Button)findViewById(R.id.btncancel);
				btn_register=(Button)findViewById(R.id.btnregisterprovider);
				btn_GenerateCSR=(Button)findViewById(R.id.btnGenerateCSR);	
				edt_AddressLisne1=(EditText)findViewById(R.id.edtaddressline1);
				edt_AddressLisne2=(EditText)findViewById(R.id.edtaddressline2);
				edt_Password =(EditText)findViewById(R.id.edtPassword);
				edt_DeliveryEmailAddress=(EditText)findViewById(R.id.edtdeliveryemailid);
				edt_ZIP=(EditText)findViewById(R.id.edtzip);
				edt_ElectronicServiceURI=(EditText)findViewById(R.id.edtElectronicServiceURI);
				edt_Contact=(EditText)findViewById(R.id.edtContact);
				txt_City=(TextView)findViewById(R.id.txtCity);
				txt_State=(TextView)findViewById(R.id.txtSate);
				txt_Speciality=(TextView)findViewById(R.id.txtSpeciality);
				txt_CSR=(TextView)findViewById(R.id.txtCSR);
				
				edt_FirstName=(EditText)findViewById(R.id.edtfirstname);
				edt_LastName=(EditText)findViewById(R.id.edtlastname);
				edt_Middle_Name=(EditText)findViewById(R.id.edtMiddleName);
				edt_EmailID=(EditText)findViewById(R.id.edtEmailID);
				spnr_Type=(Spinner)findViewById(R.id.spnrType);
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
				//spnr_Type.requestFocus();
				rg_Gender =(RadioGroup)findViewById(R.id.radioSex);
				rb_Female=(RadioButton)findViewById(R.id.radiofemale);
				rb_Male=(RadioButton)findViewById(R.id.radiomale);
				
			}
		
		}
	}
	
	private void addEventToUIControls()
	{
		btn_Cancel.setOnClickListener(this);
		btn_GenerateCSR.setOnClickListener(this);
		btn_register.setOnClickListener(this);
		txt_CSR.setOnClickListener(this);
		txt_Speciality.setOnClickListener(this);
		setLocalityInfo(edt_ZIP);		
	
	}		
	
	private void setLocalityInfo(final EditText edt)
	{
		edt.setOnFocusChangeListener(new OnFocusChangeListener() {
			
			@Override
			public void onFocusChange(View v, boolean hasFocus) {
				// TODO Auto-generated method stub
				EditText edt =(EditText)v;
				  boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
				 // Logger.debug("RegisterProvider-->previous zip",""+previousZip);
				//  Logger.debug("RegisterProvider-->edittext value",""+edt.getText().toString().trim());
				  if(edt.getText().length()>0 && hasFocus ==false )
			    	{	
					  if(previousZip!=null )
					  {
						  if(!previousZip.equals(edt.getText().toString().trim()))
						  {	
							  previousZip =edt.getText().toString().trim();
							  if(!isDataConnectionAvailable)
					    		{
					    		MHISEUtil.displayDialog(RegisterProvider.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage) ,getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
					    		}
					    		else
					    		{	 	
					    		showDialog(Constants.GET_LOCALITY_PROGRESS_DIALOG);							
					    		String request = com.mhise.requests.RequestBase.getLocalityByZipRequest(edt.getText().toString().trim());
							//	Logger.debug("RegisterProvider-->request Creates for Locality -->","Request:"+request);
								getLocalityClass = new GetLocalityClass();
								getLocalityClass.execute(request);				
					    		}
						  }
						  
					  }		    	
					  else
					  {
						  previousZip=edt.getText().toString().trim();
						  if(!isDataConnectionAvailable)
				    		{
				    		MHISEUtil.displayDialog(RegisterProvider.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage) ,getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
				    		}
				    		else
				    		{	
				    			
				    		showDialog(Constants.GET_LOCALITY_PROGRESS_DIALOG);
				    		String request = com.mhise.requests.RequestBase.getLocalityByZipRequest(edt.getText().toString().trim());
							//Logger.debug("RegisterProvider-->request Creates for Locality -->","request"+request);
							getLocalityClass = new GetLocalityClass();
							getLocalityClass.execute(request);				
				    		}
					  }	    				  	
			    	}
			}
	
			
		});
		
		
		
	}
	
	private class GetSpecialityClass extends AsyncTask<String, Void, String>
	 {		
		 @Override
		protected String doInBackground(String... params) {
	
			String request = params[0];		
			//Logger.debug("RegisterProvider-->request for specilaity--->", ""+request);
			httpClient= new DefaultHttpClient();
		//	Logger.debug("RegisterProvider-->SOAP_ACTION_FOR_GET_MASTER_DATA--->", ""+XmlConstants.ACTION_GETMASTERDATA);
		//	Logger.debug("RegisterProvider-->Constants.URL--->", ""+Constants.URL);
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL, XmlConstants.ACTION_GETMASTERDATA, request,httpClient);
		//	Logger.debug("RegisterProvider-->Response--->", ""+response);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);	  	
			 	removeDialog(Constants.GET_SPECIALITY_PROGRESS_DIALOG);
			 	Logger.debug("RegisterProvider-->web service response",""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
					try{
					MasterData masterData =	new GetMasterDataParser().parseMasterDataResponse(resultDoc);
					//Logger.debug("RegisterProvider-->is  purpose class launched ?", ""+masterData.result.IsSuccess);
					if(masterData.result.IsSuccess.equals("true"))
					{	
						///Logger.debug("RegisterProvider-->array list size", ""+masterData._arrDescription.size());
						//Logger.debug("RegisterProvider-->is  purpose class launched ?11", ""+masterData.result.IsSuccess);
						_specialities = new String[masterData._arrDescription.size()];
						_specialities = masterData._arrDescription.toArray(_specialities);
					//	Logger.debug("RegisterProvider-->array size", ""+_specialities.length );
						_specialitiesCode = new String[masterData._arrCode.size()];
						_specialitiesCode= masterData._arrCode.toArray(_specialitiesCode);
					//	Logger.debug("RegisterProvider-->array size", ""+_specialitiesCode.length );
						_selections = new boolean[_specialities.length];
						
					}
					else if(masterData.result.IsSuccess.equals("false"))
					{
						MHISEUtil.displayDialog(RegisterProvider.this, masterData.result.ErrorMessage,masterData.result.ErrorCode);	    
					}
					}
					catch (NullPointerException e) {
						// TODO: handle exception
						Logger.debug("RegisterProvider-->GetSpecialityClass", "NullPointerException"+e);
					}
				}
		 }
	}
	private class GetTypeClass extends AsyncTask<String, Void, String>
	 {
		 @Override
		protected String doInBackground(String... params) {
	
			String request = params[0];	
			httpClient= new DefaultHttpClient();
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL, Constants.SOAP_ACTION_FOR_GET_MASTER_DATA, request,httpClient);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);	
			  	
			 	removeDialog(Constants.GET_TYPE_PROGRESS_DIALOG);
			 	Logger.debug("RegisterProvider-->web service response",""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
		
					MasterData masterData =	new GetMasterDataParser().parseMasterDataResponse(resultDoc);
					try{
					if(masterData.result.IsSuccess.equals("true"))
					{	
						_arType = new String[masterData._arrDescription.size()];
						_arType = masterData._arrDescription.toArray(_arType);
						
						//Logger.debug("RegisterProvider-->array size", ""+_arType.length );
						//Set Code 
						_arTypeID= new String[masterData._arrCode.size()];
						_arTypeID= masterData._arrCode.toArray(_arTypeID);
						
						//Logger.debug("RegisterProvider-->array size", ""+_arTypeID.length );	
						@SuppressWarnings({ "rawtypes", "unchecked" })
						ArrayAdapter adapter = new ArrayAdapter(RegisterProvider.this,
								R.layout.spinnertextview , _arType);
						adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
						//adapter.setDropDownViewResource(R.layout.spinneritem);
								spnr_Type.setAdapter(adapter);
								
								
					}
					else if(masterData.result.IsSuccess.equals("false"))
					{
						MHISEUtil.displayDialog(RegisterProvider.this, masterData.result.ErrorMessage,masterData.result.ErrorCode);	    
					}
					}
					catch (NullPointerException e) {
						Logger.debug("RegisterProvider-->GetTypeClass", "NullPointerException"+e);
					}
				}
		 }
		}
	
	
	
	private void resetForm()
	{
		clear = true;
		 for( int i = 0; i < _selections.length; i++ )
			{
			  _selections[i]=false;
			}
		edt_AddressLisne1.setText("");
		
		edt_AddressLisne2.setText("");
		edt_DeliveryEmailAddress.setText("");
		edt_ZIP.setText("");
		edt_ElectronicServiceURI.setText("");
		edt_Contact.setText("");
		txt_City.setText("");
		txt_State.setText("");
		txt_Speciality.setText("");
		txt_CSR.setText("");
		edt_Password.setText(null);
		spnr_Type.setSelection(0);
	    privateKey = null;
	    previousZip=null;
		if(PROVIDERTYPE==Constants.ORGANIZATIONAL_PROVIDER)
			edt_OrganizationName.setText("");
		else if(PROVIDERTYPE==Constants.INDIVIDUAL_PROVIDER)
		{
			edt_FirstName.setText("");
			edt_Middle_Name.setText("");
			edt_LastName.setText("");
			edt_EmailID.setText("");
			rg_Gender.check(R.id.radiomale);

		}
		
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

		    	}
			    	catch (Exception e) {
			    		Logger.debug(" GenerateCSRClass" ,"Exception------>"+e);
			    		// TODO: handle exception
					}
				return csr;
		    		}
		 
		 @SuppressWarnings("static-access")
		@Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub
			super.onPostExecute(result);
			txt_CSR.setText(result);
			removeDialog(Constants.GENERATE_CSR_PROGRESS_DIALOG1);
			privateKey =  gCSR.privateKey ;	
			Log.i("Private Key", "private key:"+privateKey);
		}
	}
	

	@Override
	public void onClick(View v) {
		
		switch(v.getId())
		{
			case R.id.btncancel:
			{
				resetForm();
				break;
			}
			case R.id.btnGenerateCSR:
			{
				
				if(PROVIDERTYPE==Constants.ORGANIZATIONAL_PROVIDER)
				{
                    organizationname =edt_OrganizationName.getText().toString().trim();                                        
                    email=edt_DeliveryEmailAddress.getText().toString().trim();
                    organizationalunit =Constants.organizationalunit;
                    commonname=edt_OrganizationName.getText().toString().trim();
                    city=txt_City.getText().toString().trim();
                    state= txt_State.getText().toString().trim();                  
                    country =Constants.Country;

	
						if(organizationname.equals("")||organizationname.equals(null) || email.equals(null) ||email.equals("") ||city.equals(null) ||city.equals("") || state.equals(null) ||state.equals("") || country.equals(null)||country.equals(""))
						{
						
							
							MHISEUtil.displayDialog(RegisterProvider.this,
									getResources().getString(R.string.error_CSR_msg),getResources().getString(R.string.error_CSR_title));
						}
						else
						{
							showDialog(Constants.GENERATE_CSR_PROGRESS_DIALOG1);
							generateCSRClass = new GenerateCSRClass();
							generateCSRClass.execute();
					  
						}
				}
					else if(PROVIDERTYPE==Constants.INDIVIDUAL_PROVIDER)
                    {
                    String firstname =edt_FirstName.getText().toString().trim();
                    String lastname=edt_LastName.getText().toString().trim();
                     email=edt_DeliveryEmailAddress.getText().toString().trim();
//                  String commonname=Constants.commomname;
                    commonname=firstname+lastname;
                   organizationalunit =Constants.organizationalunit;
                   organizationname=Constants.organizationname;
                     city=txt_City.getText().toString().trim();
                     state= txt_State.getText().toString().trim();
//                  String country =strCountry;
                    country =Constants.Country;
//                  made a change in following line by checking firstname and lastname separately
                    if(firstname.equals(null) ||firstname.equals("") || lastname.equals(null) ||lastname.equals("") ||email.equals(null)||  email.equals("")||city.equals(null) ||city.equals("") || state.equals("") ||state.equals(null) || country.equals(null)||country.equals("")) 
                          

							{
                    	MHISEUtil.displayDialog(RegisterProvider.this,
								getResources().getString(R.string.error_CSR_msg),getResources().getString(R.string.error_CSR_title));
	
						
							}
							else
							{
								//GenerateCSR gCSR = new GenerateCSR();
						    /*	try {
									String csr =gCSR.generatePKCS10(email,commonname,organizationalunit,organizationname,city,state,country);
								  	privateKey =  GenerateCSR.privateKey ;	
								  	//privateKey.getEncoded();								  
							    	//Logger.debug(TAG ,"Private key------>"+privateKey.toString().trim());
							    	txt_CSR.setText(csr);
							    	//MHISEUtil.writeKey(privateKey,getApplicationContext());
							    	csr =null;
							    	Logger.debug(TAG ,"Start reading------>");
							    	Logger.debug(TAG ,"Private key------>"+privateKey);
							    	Logger.debug(TAG ,"CSR------>"+csr);
							    	//MHISEUtil.readKey(getApplicationContext());
							    	
							     *using shared preferences
							    	SharedPreferences myPrefs = this.getSharedPreferences("myPrefs", MODE_PRIVATE);
							        SharedPreferences.Editor prefsEditor = myPrefs.edit();
							        prefsEditor.putString("key", privateKey.toString().trim());						      
							        prefsEditor.commit();							    		
							    	SharedPreferences shp = getSharedPreferences("myPrefs", MODE_PRIVATE);
							    	Logger.debug("RegisterProvider-->private key as string", shp.getString("key", null));
							        
							        
						    	} catch (Exception e) {
									// TODO Auto-generated catch block
									e.printStackTrace();
								}*/
								//To generate a Dialog
								showDialog(Constants.GENERATE_CSR_PROGRESS_DIALOG1);
								generateCSRClass = new GenerateCSRClass();
								generateCSRClass.execute();
								
							}			
					}
					break;
			
			}
			case R.id.btnregisterprovider:
			{
				makeRequest();
				break;
			}
			case R.id.txtSpeciality:
			{
				 showDialog(Constants.SPECIALITY_DIALOG_ID);;
				break;
			}
		}
		
	}	


	private void makeRequest()
	{	
		if(PROVIDERTYPE==Constants.ORGANIZATIONAL_PROVIDER)
		{		
		ProviderRegistration  provider = new ProviderRegistration();
		provider.setProviderCode(Constants.ORGANIZATIONAL_PROVIDER);
		provider.setOrganizationName(edt_OrganizationName.getText().toString().trim());
		provider.setAddressLine1(edt_AddressLisne1.getText().toString().trim());
		provider.setAddressLine2(edt_AddressLisne2.getText().toString().trim());
		provider.setCityName(txt_City.getText().toString().trim());
		provider.setStateName(txt_State.getText().toString().trim());
		provider.setContactNumber(edt_Contact.getText().toString().trim());
		provider.setCountry(Constants.Country);
		provider.setPassword(edt_Password.getText().toString().trim());
		provider.setCSR(txt_CSR.getText().toString().trim());	
		provider.setElectronicServiceURI(edt_ElectronicServiceURI.getText().toString().trim());
		provider.setLanguage(Constants.Language);
		provider.setMedicalRecordsDeliveryEmailAddress(edt_DeliveryEmailAddress.getText().toString().trim());
		provider.setPostalCode(edt_ZIP.getText().toString().trim());
		provider.setStateName(txt_State.getText().toString().trim());
		provider.setStatus(Constants.Status);
		try{
		provider.setProviderType( spnr_Type.getSelectedItem().toString().trim());
		provider.setspecialtyNameAndID(_selectedSpecalities);

	
		
		boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
		  if(!isDataConnectionAvailable)
  		{
  		MHISEUtil.displayDialog(RegisterProvider.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
  		}
  		else
  		{			strProviderRegistration = RequestBase.getProviderRequest(provider);	
  			showDialog(Constants.REGISTER_PROVIDER_DIALOGUE);
		new GetProviderRegistrationAsync().execute(strProviderRegistration);
		
  		}
		
		}
		catch (NullPointerException e) {	
		Logger.debug("RegisterProvider-->RegisterProvider --> makeRequest","NullPointerException");		}
		
		}
		else if(PROVIDERTYPE==Constants.INDIVIDUAL_PROVIDER)
		{
			ProviderRegistration  provider = new ProviderRegistration();		
			
			provider.setProviderCode(Constants.INDIVIDUAL_PROVIDER);
			provider.setAddressLine1(edt_AddressLisne1.getText().toString().trim());
			provider.setAddressLine2(edt_AddressLisne2.getText().toString().trim());
			provider.setCityName(txt_City.getText().toString().trim());
			provider.setStateName(txt_State.getText().toString().trim());
			provider.setContactNumber(edt_Contact.getText().toString().trim());
			provider.setCountry(Constants.Country);
			provider.setPassword(edt_Password.getText().toString().trim());
			provider.setCSR(txt_CSR.getText().toString().trim());	
			provider.setElectronicServiceURI(edt_ElectronicServiceURI.getText().toString().trim());
			provider.setEmail(edt_EmailID.getText().toString().trim());
			provider.setFirstName(edt_FirstName.getText().toString().trim());
			int selectedId = rg_Gender.getCheckedRadioButtonId();
		    RadioButton  radioSexButton = (RadioButton) findViewById(selectedId);
			provider.setGender(radioSexButton.getText().toString().trim());
			provider.setLanguage(Constants.Language);
			provider.setLastName(edt_LastName.getText().toString().trim());
			provider.setMedicalRecordsDeliveryEmailAddress(edt_DeliveryEmailAddress.getText().toString().trim());
			provider.setMiddleName(edt_Middle_Name.getText().toString().trim());
			provider.setPostalCode(edt_ZIP.getText().toString().trim());
			provider.setStateName(txt_State.getText().toString().trim());
			provider.setStatus(Constants.Status);
			try{

			Log.i("Selected type value", ""+spnr_Type.getSelectedItem()+spnr_Type.getSelectedItemPosition());
			Log.i("Selected type ID", ""+_arTypeID[spnr_Type.getSelectedItemPosition()]);
			Log.i("Selected type  Enum value", ""+hmpMap.get(_arTypeID[spnr_Type.getSelectedItemPosition()]));
			
			provider.setProviderType(hmpMap.get(_arTypeID[spnr_Type.getSelectedItemPosition()]));//spnr_Type.getSelectedItem().toString().trim());
			provider.setspecialtyNameAndID(_selectedSpecalities);
			
			
			
			  boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
			 if(!isDataConnectionAvailable)
	    		{
	    		MHISEUtil.displayDialog(RegisterProvider.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
	    		}
	    		else
	    		{
			
			
			strProviderRegistration = RequestBase.getProviderRequest(provider);	
			
			
			showDialog(Constants.REGISTER_PROVIDER_DIALOGUE);
			getProviderRegistrationAsync = new GetProviderRegistrationAsync();
			getProviderRegistrationAsync.execute(strProviderRegistration);
	    		}
			}
			catch (NullPointerException e) {	
			Logger.debug("RegisterProvider-->RegisterProvider --> makeRequest","NullPointerException");		}
		}
	}
	
		private class GetLocalityClass extends AsyncTask<String, Void, String>
		{		
		 @Override
		 protected String doInBackground(String... params) {
	
			String request = params[0];
			
			Logger.debug("RegisterProvider-->request comes in async class", ""+request);
			httpClient= new DefaultHttpClient();
			String response = com.mhise.util.MHISEUtil.CallWebService(
				
					Constants.URL,XmlConstants.ACTION_GETLOCALITYBYZIPCODE, request,httpClient);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);		
			 	removeDialog(Constants.GET_LOCALITY_PROGRESS_DIALOG);
			 	Logger.debug("RegisterProvider-->web service response",""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
		
					City city  =	BaseParser.parseGetLocalityByZipresult(resultDoc);
					//Logger.debug("RegisterProvider-->is  purpose class launched ?", ""+city.getResult().IsSuccess);
					if(city.getResult().IsSuccess.equals("true"))
					{	
					try
					{
						txt_City.setText(city.getCityName());
						txt_State.setText(city.getState().getStateName());			
						//strCountry=city.getState().getCountry().getCountryName();
					}
					catch (NullPointerException e) {
						// TODO: handle exception
						Logger.debug("null pointer exception at post execute of Locality async class", ""+e);
					}	
					}
					else if(city.getResult().IsSuccess.equals("false"))
					{
						//Logger.debug("RegisterProvider-->before show dialog", ""+city.getResult().ErrorMessage);
						MHISEUtil.displayDialog(RegisterProvider.this, city.getResult().ErrorMessage,city.getResult().ErrorCode);	    
						edt_ZIP.setText(null);
						txt_City.setText(null);
						txt_State.setText(null);
					//	strCountry =null;
					}
					
				}
		 }	
		}

	private class GetProviderRegistrationAsync extends AsyncTask<String, Void, String>
	 {		
		 @Override
		protected String doInBackground(String... params) {
	
			String request = params[0];	
			//Logger.debug("RegisterProvider-->request comes in async class", ""+request);
			httpClient= new DefaultHttpClient();
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL,XmlConstants.ACTION_ADD_PROVIDER, request,httpClient);
			//Logger.debug("RegisterProvider-->response for Provider registration","response"+response);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);		
			removeDialog(Constants.REGISTER_PROVIDER_DIALOGUE);
			
			try {
			 	Logger.debug("RegisterProvider-->web service response",""+result);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
					/*		Result result1  =	BaseParser.parseAddProviderResult(resultDoc);
					
					if(result1!=null)
					{
					if(result1.IsSuccess.equals("false"))
						MHISEUtil.displayDialog(RegisterProvider.this, result1.ErrorMessage,result1.ErrorCode);	
					else
					{
						try {
							MHISEUtil.writeKey(privateKey,getApplicationContext());
							MHISEUtil.displayDialog(RegisterProvider.this, result1.ErrorMessage,result1.ErrorCode);
						} catch (Exception e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
					
					}
					
				}	*/
					RegisterationResult regResult  =	BaseParser.parseAddProviderResult(resultDoc,Constants.PROVIDER);
					Result result1 = regResult.getResult();
					if(result1.IsSuccess.equals("false"))
						MHISEUtil.displayDialog(RegisterProvider.this, result1.ErrorMessage,result1.ErrorCode);	
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
							
							Intent certificateIntent = new Intent(RegisterProvider.this,CertificateInitializer.class);
							certificateIntent.putExtra(Constants.TAG_CERTIFICATE,certificate);
							certificateIntent.putExtra(Constants.TAG_PRIVATEKEY, privateKey);
					
							//certificateIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
							//	certificateIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
							//	certificateIntent.setFlags( Intent.FLAG_ACTIVITY_CLEAR_TOP|Intent.FLAG_ACTIVITY_SINGLE_TOP);
							
							startActivity(certificateIntent);
							finish();
							
						}
						catch (NullPointerException e) {
							Logger.debug("GetProviderRegistrationAsync", "-->"+e);
						}
					
					}

					}
			}
			catch (Exception e) {

				Logger.debug("GetProviderRegistrationAsync - on post execute -->", "-->"+e);
				
			}
		 }	
		}
	
	private void fetchTypes()
	{
		   if(_arType == null ||_arType.length<1)
		   {	
			   boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
		    	
		    	if(!isDataConnectionAvailable)
		    	{
		    		MHISEUtil.displayDialog(this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle)  );
		    	}
		    	else
		    	{	String request=null;
					showDialog(Constants.GET_TYPE_PROGRESS_DIALOG);
					if(PROVIDERTYPE ==Constants.ORGANIZATIONAL_PROVIDER)
					request = RequestBase.getMasterDataRequest(Constants.TagOrganizationType) ;
					else if(PROVIDERTYPE ==Constants.INDIVIDUAL_PROVIDER)
					request = RequestBase.getMasterDataRequest(Constants.TagIndividualType) ;
					
					showDialog(Constants.GET_TYPE_PROGRESS_DIALOG);
					getTypeClass = new GetTypeClass();
					getTypeClass.execute(request);				
		    	}
	  
		   }
	}
	
	private void fetchSpecialityList() {		
		  
		   if(_specialities == null ||_specialities.length<1)
		   {	
			   boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
		    	
		    	if(!isDataConnectionAvailable)
		    	{
		    		MHISEUtil.displayDialog(this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle)  );
		    	}
		    	else
		    	{	 	
					showDialog(Constants.GET_SPECIALITY_PROGRESS_DIALOG);			
					String request = RequestBase.getMasterDataRequest(Constants.TagSpeciality) ;
					//Logger.debug("RegisterProvider-->request Creates for Speciality",""+request);
					getSpecialityClass = new GetSpecialityClass();
					getSpecialityClass.execute(request);				
		    	}
	  
		   }
		  	
	   }
	
    @Override
	protected Dialog onCreateDialog(int id) {
	    	
	        switch (id) {
  
	            	
	            case   Constants.GET_SPECIALITY_PROGRESS_DIALOG:
	        	{
	        		 ProgressDialog dialog = new ProgressDialog(this);
	        		 dialog.setMessage("Loading Speciality List..");          		 
	        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
	        		// Logger.debug("RegisterProvider-->inside on create dialog", "true");
	        		 dialog.setCancelable(true);
	        		 dialog.setCanceledOnTouchOutside(false);
	        		 dialog.setOnCancelListener(new OnCancelListener() {
  						
  						@Override
  						public void onCancel(DialogInterface dialog) {
  							// TODO Auto-generated method stub
  							dialog.dismiss();
  							getSpecialityClass.cancel(true);
  						
  						}
  					});	
			       		 
	        		 return dialog;
	        } 
	            case   Constants.GET_LOCALITY_PROGRESS_DIALOG:
	        	{
	        		ProgressDialog dialog = new ProgressDialog(this);
	       		 	dialog.setMessage("Fetching Locality Details.");          		 
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
	        	
	          	case Constants.GENERATE_CSR_PROGRESS_DIALOG1:{
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
	        	
	        	
	            case   Constants.GET_TYPE_PROGRESS_DIALOG:
	        	{
	        		 ProgressDialog dialog = new ProgressDialog(this);
	        		 dialog.setMessage("Fetching Types");          		 
	        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
	        		// Logger.debug("RegisterProvider-->inside on create dialog", "true");
	        		 dialog.setCancelable(true);
	        		 dialog.setCanceledOnTouchOutside(false);
	        	 dialog.setOnCancelListener(new OnCancelListener() {
  						
  						@Override
  						public void onCancel(DialogInterface dialog) {
  							// TODO Auto-generated method stub
  							dialog.dismiss();
  							getTypeClass.cancel(true);
  						
  						}
  					});	
			       		 
	        		 return dialog;
	        }
	        	
	     
	            case   Constants.SPECIALITY_DIALOG_ID:
            	{
            		clear = false;
            		return new AlertDialog.Builder( this )
            	        .setTitle( getResources().getString(R.string.select_Speciality) )
            	        .setMultiChoiceItems
            	        ( _specialities, _selections, new DialogSelectionClickHandler())
            	        .setPositiveButton( "OK", new DialogButtonClickHandler() )
            	        .create();
            	}  
	   	
	            case   Constants.REGISTER_PROVIDER_DIALOGUE:
            	{
            		ProgressDialog dialog = new ProgressDialog(this);
	        		 dialog.setMessage("Registering Provider..");          		 
	        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
	        		// Logger.debug("RegisterProvider-->inside on create dialog Register Provider", "true");
	        		 dialog.setCancelable(true);
	        		 dialog.setCanceledOnTouchOutside(false);
	        		 dialog.setOnCancelListener(new OnCancelListener() {
  						
  						@Override
  						public void onCancel(DialogInterface dialog) {
  							// TODO Auto-generated method stub
  							dialog.dismiss();
  							getProviderRegistrationAsync.cancel(true);
  						
  						}
  					});	
			       		 	
	        		 
	        		 return dialog;
            	}  
	        	
	        }
	       return null;
	    }

    
    @Override
    protected void onPrepareDialog(int id, Dialog dialog) {
        switch (id) {
           
            case Constants.SPECIALITY_DIALOG_ID:
            {
            	if(clear){
           		 final AlertDialog alert = (AlertDialog)dialog;
           		 final ListView list = alert.getListView();
           		 for(int i = 0 ; i < list.getCount(); i++){
           	         list.setItemChecked(i, false);  
           	     }
	            	
            	}
           	
            }        
        }
  
    } 
    public class DialogButtonClickHandler implements DialogInterface.OnClickListener
	{
		public void onClick( DialogInterface dialog, int clicked )
		{
			switch( clicked )
			{
				case DialogInterface.BUTTON_POSITIVE:
					setSelectedSpeciality();
					break;
			}
		}
	}
	
	private ArrayList<String> setSelectedSpeciality() {
		// TODO Auto-generated method stub
		_selectedSpecalities = new ArrayList<String>();
		
		StringBuffer strSpeciality = new StringBuffer();
		
		if(_specialities!=null)
		{
			for( int i = 0; i < _specialities.length; i++ )
			{	
				if(_selections[i] == true)
				{
					//Logger.debug("RegisterProvider-->_selections---->", "->"+_specialities[i]);
					strSpeciality.append(_specialities[i]+",");
					_selectedSpecalities.add(_specialitiesCode[i]+"@"+_specialities[i].toString().trim());		
				}
			}
			if(strSpeciality.length()>0)
				strSpeciality.deleteCharAt(strSpeciality.length()-1);
		}
		txt_Speciality.setText(strSpeciality);
		if(_selectedSpecalities.size() == 0)
		return null;
		
		return _selectedSpecalities;
	}
	
	public class DialogSelectionClickHandler implements DialogInterface.OnMultiChoiceClickListener
	{
		public void onClick( DialogInterface dialog, int clicked, boolean selected )
		{
			Logger.debug( "ME", _specialities[ clicked ] + " selected: " + selected );
		}
	}
	
	@Override
	protected void onDestroy() {
	
		super.onDestroy();
		previousZip=null;
	}
	 @Override
	protected void onStop() {
		// TODO Auto-generated method stub
		super.onStop();
		previousZip =null;
	}
	 
	
} 

