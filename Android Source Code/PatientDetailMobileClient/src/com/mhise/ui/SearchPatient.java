package com.mhise.ui;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.LinkedHashMap;

import org.apache.http.impl.client.DefaultHttpClient;

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
import android.support.v4.app.DialogFragment;
import android.support.v4.app.FragmentManager;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;


import com.mhise.constants.Constants;
import com.mhise.constants.MobiusDroid;
import com.mhise.model.Assertion;
import com.mhise.model.CommunityResult;
import com.mhise.model.Demographics;
import com.mhise.model.NHINCommunity;
import com.mhise.model.SearchPatientResult;
import com.mhise.model.User;
import com.mhise.response.GetCommunities;
import com.mhise.response.PatientResultParser;
import com.mhise.util.DataConnectionManager;
import com.mhise.util.DatePickerFragment;
import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;



public class SearchPatient extends BaseMenuOptionsAcitivity implements View.OnClickListener {

	  	EditText edtFirstName ;
	    EditText edtLastName ;
	    static EditText edtDOB ;
	    EditText edtPatientID ;
	    TextView txtCommunity;
	    ImageView advancedSearch;
	    String firstname,lastname,dob,patientid,sex,community;
	    RadioGroup  rgSex;
	    
	    Button btnSearchPatient,btnCancel;
	    ImageView imgDOB;	    	
	    private String userType ;
	    private String userRole ;
	    private User user;
	    private String email ;
	    private GetCommunitiesAsyncClass getCommunitiesAsyncObj;
	    private RequestResponseHandler1 requestResponseHandler1;
	    private static int mYear;
	    private static int mMonth;
	    private static int mDay;
		protected String[] _cummunities ;
		HashMap< String, String> hmp_Community;
		
		protected ArrayList<String> _selectedcommunities ;
		protected CharSequence[] _se ;
		protected boolean[] _selections ;

		public SearchPatientResult searchPatientResult;
		private boolean clear = false;
	

    @Override
    	public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.search_patient);
        
        //Get Data from intent
        userRole =getIntent().getStringExtra(Constants.KEY_ROLE);
        userType=getIntent().getStringExtra(Constants.KEY_USER_TYPE);
        email=getIntent().getStringExtra(Constants.KEY_USER_EMAIL);
        user=(User)getIntent().getSerializableExtra("User");
        
        //Initialize form components
    	btnSearchPatient = (Button)findViewById(R.id.btnsearchpatient);
    	btnCancel = (Button)findViewById(R.id.btncancel);	
    	edtFirstName=(EditText)findViewById(R.id.edtfirstname);
    	edtLastName=(EditText)findViewById(R.id.edtlastname);
    	edtPatientID=(EditText)findViewById(R.id.edtpatientid);
    	edtDOB=(EditText)findViewById(R.id.edtdob);
    	txtCommunity=(TextView)findViewById(R.id.edtCommunity);
    	advancedSearch=(ImageView)findViewById(R.id.advanceSearchText);
    	txtCommunity.setSingleLine();
   	 
    	//Initialize Communities

    	imgDOB =(ImageView)findViewById(R.id.imgDOB);
    	rgSex =(RadioGroup)findViewById(R.id.radioSex);
    	
    	//set Listener for button
    	btnSearchPatient.setOnClickListener(this);
    	btnCancel.setOnClickListener(this);
        imgDOB.setOnClickListener(this);
        
        txtCommunity.setOnClickListener(this);
        advancedSearch.setOnClickListener(this);

    	}
    
    
    
    @Override
    protected void onResume() {
    // TODO Auto-generated method stub
    super.onResume();
  	Context ctx = getApplicationContext();
	if(_cummunities ==null)
	{
		boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(ctx);
    	if(!isDataConnectionAvailable)
    	{
    		MHISEUtil.displayDialog(this,getResources().getString(R.string.DataConnectionAvailabiltyMessage), getResources().getString(R.string.DataConnectionAvailabiltyTitle));
    	}
    	else
    	{
    		showDialog(Constants.GET_COMMUNITY_PROGRESS_DIALOG);
    		String[] params ={com.mhise.requests.RequestBase.getNhinCommReq()};
    		getCommunitiesAsyncObj = new GetCommunitiesAsyncClass();
    		getCommunitiesAsyncObj.execute(params);   	
    	}
	}


    }
    
    @Override
   	protected void onSaveInstanceState(Bundle outState) {
   		// TODO Auto-generated method stub
    	 Log.e("onSaveInstanceState","onSaveInstanceState");
   		super.onSaveInstanceState(outState);
   	try{
   		outState.putStringArray("Selected_Communities",_cummunities);
   		outState.putSerializable("HashMap_Communities",hmp_Community);
   		outState.putBooleanArray("SelectionArray", _selections);
   		outState.putString("CommunityText", txtCommunity.getText().toString());
   	}
   	catch (NullPointerException e) {
		// TODO: handle exception
   		Logger.debug("SearchPatient --> onSaveInstanceState","Exception"+e);
	}
   		
   	}
   	 
   	 @SuppressWarnings("unchecked")
   	@Override
   	protected void onRestoreInstanceState(Bundle savedInstanceState) {
   		//Get Saved Communities List
   		 Log.e("onRestoreInstanceState","onRestoreInstanceState");
   		super.onRestoreInstanceState(savedInstanceState);
  		try {
  		 hmp_Community= (HashMap<String,String>) savedInstanceState.getSerializable("HashMap_Communities");
   		_cummunities = savedInstanceState.getStringArray("Selected_Communities");
   		_selections = savedInstanceState.getBooleanArray("SelectionArray");   		
   		txtCommunity.setText(savedInstanceState.getString("CommunityText"));
 	}
    	catch (NullPointerException e) {
 		// TODO: handle exception
    		Logger.debug("SearchPatient --> onRestoreInstanceState","Exception"+e);
 	}		
   	}
  
   private  class RequestResponseHandler1 extends AsyncTask<String, Void, String> {

	
		String PatientSearchRequest;
		
		@Override
		protected String doInBackground(String... params) {
			
			this.PatientSearchRequest=params[0];
			SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);  
		 	DefaultHttpClient httpClient = MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));
			String response = com.mhise.util.MHISEUtil.CallWebService(					
					Constants.HTTPS_URL, XmlConstants.ACTION_FOR_SEARCH_PATIENT, PatientSearchRequest,httpClient);
			//Log.i("SearchPatient-->Response", "-->"+response);
			return response;
		}
		@Override
		protected void onPostExecute(String result) {
				// TODO Auto-generated method stub
			
			org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
			if(resultDoc != null)
			{
				try{
					SearchPatientResult  patientResult =	new PatientResultParser().parseXML(resultDoc); 
				if(patientResult.getResult().IsSuccess.equals("true"))
				{
					
			
	                LinkedHashMap<String, Object> obj = new LinkedHashMap<String, Object>();
	                obj.put("SearchPatientResult", obj);
					
					Intent callResultscreen = new Intent(SearchPatient.this,PatientResult.class);
					   Bundle b = new Bundle();
		                b.putSerializable("bundleobj", patientResult);
		                callResultscreen.putExtras(b);  	
		                callResultscreen.putExtra(Constants.KEY_ROLE, userRole);
		                callResultscreen.putExtra(Constants.KEY_USER_EMAIL, email); 
		                callResultscreen.putExtra(Constants.KEY_USER_TYPE, userType);
		                callResultscreen.putExtra("loginUser", user);
					startActivity(callResultscreen);
					removeDialog(Constants.GET_PATIENT_PROGRESS_DIALOG);
				}
		
				else if(patientResult.getResult().IsSuccess.equals("false"))
				{
					removeDialog(Constants.GET_PATIENT_PROGRESS_DIALOG);
					if(	patientResult.getResult().ErrorMessage !=null)
					com.mhise.util.MHISEUtil.displayDialog(SearchPatient.this, 
							patientResult.getResult().ErrorMessage,patientResult.getResult().ErrorCode);
					else
						com.mhise.util.MHISEUtil.displayDialog(SearchPatient.this, 
						getResources().getString(R.string.No_MSG),getResources().getString(R.string.No_MSG_Title)	);	
				}
				}
				catch (NullPointerException e) {
					Logger.debug("SearchPatient-->RequestResponseHandler1 Null poiter exception  at Search  Patient response parsing", "Exception"+e);
				}
			
			}
			
		}
	}
    
	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		
		Log.e("v.getId()",""+v.getId());
		Logger.debug("SearchPatient-->onClick ", "called");
		switch(v.getId())
		{
		case R.id.btnsearchpatient:
		{
			
			boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
	    	
	    	if(!isDataConnectionAvailable)
	    	{
	    		MHISEUtil.displayDialog(this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle)  );
	    	}
	    	else
	    	{
			
	    		String patientRequest = initializeRequestString();		
				if(patientRequest!=null)
				{	showDialog(Constants.GET_PATIENT_PROGRESS_DIALOG);
					requestResponseHandler1 = new RequestResponseHandler1();
					requestResponseHandler1.execute( new String[]{patientRequest});
				}
			
			 
	    	}
			break;
		}
		case R.id.advanceSearchText:
		{
			Intent advanceSearchIntent = new Intent(SearchPatient.this,AdvanceSearchPatient.class);
		    advanceSearchIntent.putExtra(Constants.KEY_ROLE, user.getRole()); 
		    advanceSearchIntent.putExtra(Constants.KEY_USER_TYPE, user.getUserType()); 
		    advanceSearchIntent.putExtra(Constants.KEY_USER_EMAIL, user.getEmail());
		    advanceSearchIntent.putExtra("User",user);
		    advanceSearchIntent.putExtra("hmpCommunity",hmp_Community);
		    advanceSearchIntent.putExtra("Cummunities",_cummunities);
		    advanceSearchIntent.putExtra("selections",_selections);
		    startActivity(advanceSearchIntent);
		}
		case R.id.btncancel :
		{
			
			ResetValues();
			break;
		}
		
		case R.id.imgDOB:
		{
			DatePickerFragment1 newFragment = new DatePickerFragment1();
		    String tag ="datePicker";
		    FragmentManager fm = getSupportFragmentManager();
		    newFragment.show(fm, tag);
			//showDialog(Constants.DATE_DIALOG_ID);
			  
			break;
		}
		case R.id.edtCommunity:
		{
			
			
			 showDialog(Constants.COMMUNITY_DIALOG_ID);
		}
		}
	}
	
	  private String initializeRequestString() {  
		  	Assertion assertion = new Assertion();
		  	assertion.setAssertionMode(Constants.ASSERTION_MODE_DEFAULT);
		  	assertion.setPurposeOfUse(Constants.DEFAULT_PURPOSE);
		  	assertion.setUserInformation(user);
		  	assertion.setNhinCommunity(MobiusDroid.homeCommunity);
		  	assertion.setHaveSignature(true);
		  	
		  	Demographics patientDemographics = new Demographics();
		  	patientDemographics.setGivenName(edtFirstName.getText().toString().trim());
		  	patientDemographics.setFamilyName(edtLastName.getText().toString().trim());	
		  	int selectedId = rgSex.getCheckedRadioButtonId();
		    RadioButton  radioSexButton = (RadioButton) findViewById(selectedId);		
		    patientDemographics.setGender(radioSexButton.getText().toString().trim());
		  	patientDemographics.setDOB(edtDOB.getText().toString().trim());
		   
		  	String request =null;
		  	try {
			  	String[] _arrComunities =txtCommunity.getText().toString().split(",");
			  	Log.i("SearchPatient-->Final Array of cummunties length" , ""+MobiusDroid._arrComunities.length);
			  	
			  	com.mhise.requests.SearchPatientRequest searchRequest =
			  			new com.mhise.requests.SearchPatientRequest(); 
			  	Log.i("SearchPatient-->selected community",""+_arrComunities);	  	
			  	 request =searchRequest.makeSearchPatientRequest
			  			(assertion, patientDemographics, _arrComunities,hmp_Community);	
		  	}
		  	catch (NullPointerException e) {
		  		Logger.debug("SearchPatient-->initializeRequestString","NullPointerException"+e);
			}
		 
		  	return request;
	}

	  @Override
	public Dialog onCreateDialog(int id) {
	        switch (id) {
	           
	            case Constants.DATE_DIALOG_ID:
	               final Calendar c = Calendar.getInstance();
	                mYear = c.get(Calendar.YEAR);
	                mMonth = c.get(Calendar.MONTH);
	                mDay = c.get(Calendar.DAY_OF_MONTH);              	
	                return new DatePickerDialog(this,
	                            mDateSetListener,
	                            mYear, mMonth, mDay);
	                
	                
	            case   Constants.COMMUNITY_DIALOG_ID:
	            	{
	            		clear = false;
	            		//Log.e("_selections","_selections"+_selections.length);
	            		return new AlertDialog.Builder( this )
	            	        .setTitle( getResources().getString(R.string.selectCommunity) )
	            	        .setMultiChoiceItems
	            	        ( _cummunities, _selections, new DialogSelectionClickHandler() )
	            	        .setPositiveButton( "OK", new DialogButtonClickHandler() )
	            	        .create();
	            	}           	
	            case   Constants.GET_COMMUNITY_PROGRESS_DIALOG:
            		{
            		
            		 ProgressDialog dialog = new ProgressDialog(this);
            		 dialog.setMessage("Loading Communities...");          		 
            		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
            	
            		 dialog.setCancelable(true);
            		 dialog.setCanceledOnTouchOutside(false);
            		 dialog.setOnCancelListener(new OnCancelListener() {
   						
   						@Override
   						public void onCancel(DialogInterface dialog) {
   							// TODO Auto-generated method stub
   							dialog.dismiss();
   							getCommunitiesAsyncObj.cancel(true);
   						
   						}
   					});
            		 return dialog;
            		}
	            case   Constants.GET_PATIENT_PROGRESS_DIALOG:
            		{
            		 ProgressDialog dialog = new ProgressDialog(this);
            		 dialog.setMessage("Searching Patient...");
            		 //dialog.setIndeterminate(true);
            		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
            		 
            		 dialog.setCancelable(true);
            		 dialog.setCanceledOnTouchOutside(false);
            		dialog.setOnCancelListener(new OnCancelListener() {
						
						@Override
						public void onCancel(DialogInterface dialog) {
							// TODO Auto-generated method stub
						requestResponseHandler1.cancel(true);	
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
	           
	            case Constants.DATE_DIALOG_ID:
	            {
	                ((DatePickerDialog) dialog).updateDate(mYear, mMonth, mDay);
	                break;
	            }
	            case Constants.COMMUNITY_DIALOG_ID:
	            {	
	            	if(clear){
	            		 final AlertDialog alert = (AlertDialog)dialog;
	            		 final ListView list = alert.getListView();
	            		 for(int i = 0 ; i < list.getCount(); i++){
	            	         list.setItemChecked(i, false);  
	            	     }
		            	
	            	}
	            	
	            	//final AlertDialog alert = (AlertDialog)dialog;
	                //final ListView list = alert.getListView();
	            }        
	        }
      
	    } 
	    
	    private static void updateDisplay() {
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
		    	
	        edtDOB.setText(
	        		
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
	         /*   private static String pad(int c) {
	                if (c >= 10)
	                    return String.valueOf(c);
	                else
	                    return "0" + String.valueOf(c);
	            }*/
	            
	private void ResetValues() {
		  clear = true;
		  Log.e("reset values","reset values");
		  edtFirstName.setText(null);
		  edtLastName.setText(null);
		  edtPatientID.setText(null);
		  edtDOB.setText(null);
		  txtCommunity.setText(null);
		  rgSex.check(R.id.radiomale);
		 for( int i = 0; i < _selections.length; i++ )
			{
			  _selections[i]=false;
			}
		  
		  
	}


	 private class GetCommunitiesAsyncClass extends AsyncTask<String, Void, String>
	 {
		 	@Override
		 	protected String doInBackground(String... params) 
		 	{       
			SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);  
		 	DefaultHttpClient httpClient = MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));
			String request = params[0];	
			String response = com.mhise.util.MHISEUtil.CallWebService(
						Constants.HTTPS_URL, XmlConstants.ACTION_GET_COMMUNITIES, request,httpClient );
			
			return response;	
	}	

			 @Override
			protected void onPostExecute(String result) {
				 
			super.onPostExecute(result);	
			
			try{
			removeDialog(Constants.GET_COMMUNITY_PROGRESS_DIALOG);
				 	Logger.debug("SearchPatient-->Response",""+result);
				 	
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{
						CommunityResult comResult   =GetCommunities.parseXML(resultDoc); 
						if (comResult.getResult().IsSuccess.equalsIgnoreCase("true"))
						{
							hmp_Community = MHISEUtil.extractCommunityArray(comResult,getApplicationContext());
							MobiusDroid.hmp_CommunityID=hmp_Community;
							MobiusDroid._arrComunities = new String[hmp_Community.size()];
							_cummunities = new String[hmp_Community.size()];
							MobiusDroid._arrComunities= MHISEUtil.extractCommunityArray(comResult,getApplicationContext()).keySet().toArray(MobiusDroid._arrComunities);
							
							//_cummunities =MHISEUtil.extractCommunityArray1(comResult,getApplicationContext()).keySet().toArray(_cummunities);
							
							ArrayList<String> arr = MHISEUtil.extractCommunityArrayList(comResult,getApplicationContext())[0];
							_cummunities =arr.toArray(_cummunities);
							_selections =  new boolean[ _cummunities.length ];
					
						}
					}	
			 
			}
			catch (Exception e) {
				// TODO: handle exception
				Log.i("SearchPatient-->onPostExecute Response",""+e);
			 	Logger.debug("SearchPatient-->onPostExecute Response",""+e);
			}
			 }
	 }

	
	 
		public class DialogSelectionClickHandler implements DialogInterface.OnMultiChoiceClickListener
		{
			public void onClick( DialogInterface dialog, int clicked, boolean selected )
			{
				
				_selections[clicked] =selected;
			}
		}
		
	 
	public class DialogButtonClickHandler implements DialogInterface.OnClickListener
	{
		public void onClick( DialogInterface dialog, int clicked )
		{
			switch( clicked )
			{
				case DialogInterface.BUTTON_POSITIVE:
					
					setSelectedCommunity();
					break;
			}
		}
	}
	
	protected ArrayList<String> setSelectedCommunity()
	{
		_selectedcommunities = new ArrayList<String>();
		
		Log.e("in set selected","in set selected");
		StringBuffer _community = new StringBuffer();
		if(_cummunities!=null)
		{
			for( int i = 0; i < _cummunities.length; i++ )
			{	
				if(_selections[i] == true)
				{
					_community.append(_cummunities[i]+",");
					_selectedcommunities.add( _cummunities[i].toString());
					
				}
			}
			if(_community.length()>0)
			_community.deleteCharAt(_community.length()-1);
		}
		txtCommunity.setText(_community);
		if(_selectedcommunities.size() == 0)
		return null;
		
		return _selectedcommunities;
	}
	
	public void advanceSearch(View view) {
		Log.e("ssss","dddd");
	    Intent advanceSearchIntent = new Intent(SearchPatient.this,AdvanceSearchPatient.class);
	    advanceSearchIntent.putExtra(Constants.KEY_ROLE, user.getRole()); 
	    advanceSearchIntent.putExtra(Constants.KEY_USER_TYPE, user.getUserType()); 
	    advanceSearchIntent.putExtra(Constants.KEY_USER_EMAIL, user.getEmail());
	    advanceSearchIntent.putExtra("User",user);
	    startActivity(advanceSearchIntent);
	 }
	
	
	
	public static class DatePickerFragment1 extends DialogFragment implements DatePickerDialog.OnDateSetListener {
		
	@Override
		public Dialog onCreateDialog(Bundle savedInstanceState) {
		// Use the current date as the default date in the picker
			final Calendar c = Calendar.getInstance();
			mYear = c.get(Calendar.YEAR);
			mMonth = c.get(Calendar.MONTH);
			mDay = c.get(Calendar.DAY_OF_MONTH);
			
			// Create a new instance of DatePickerDialog and return it
			return new DatePickerDialog(getActivity(), this, mYear, mMonth, mDay);
		}

		public void onDateSet(DatePicker view, int year, int month, int day) {
		    mYear = year;
            mMonth = month;
            mDay = day;
            updateDisplay();
		}
	}
}
