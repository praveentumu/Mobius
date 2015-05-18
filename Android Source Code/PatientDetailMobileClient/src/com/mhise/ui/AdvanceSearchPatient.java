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
import com.mhise.model.PersonName;
import com.mhise.model.SearchPatientResult;
import com.mhise.model.User;
import com.mhise.response.GetCommunities;
import com.mhise.response.PatientResultParser;
import com.mhise.ui.SearchPatient.DatePickerFragment1;
import com.mhise.util.DataConnectionManager;
import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;

public class AdvanceSearchPatient extends BaseMenuOptionsAcitivity implements View.OnClickListener{

  	EditText edtFirstName ;
    EditText edtLastName ;
    EditText edtMiddleName ;
    EditText edtPrefix ;
    EditText edtSuffix ;
    EditText edtMothersPrefix ;
    EditText edtMothersSuffix ;
    EditText edtMothersFamilyName ;
    EditText edtMothersMiddleName ;
    EditText edtMothersGivenName ;
    EditText city;
    EditText street;
    EditText state;
    EditText zip;
    EditText country;
    EditText ssn;
    EditText birthCity;
    EditText birthStreet;
    EditText birthState;
    EditText birthZip;
    EditText birthCountry;
    EditText telephone;
    
    static EditText edtDOB ;
    static EditText edtDeceasedDate ;
    EditText edtPatientID ;
    TextView txtCommunity;	    
    String firstname,lastname,dob,patientid,sex,community;
    RadioGroup  rgSex;
    static int clickedCalender;
    Button btnSearchPatient,btnCancel;
    ImageView imgDOB;
    ImageView imgDeceasedDate;
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

    setContentView(R.layout.advance_search_patient);
    
    //Get Data from intent
    userRole =getIntent().getStringExtra(Constants.KEY_ROLE);
    userType=getIntent().getStringExtra(Constants.KEY_USER_TYPE);
    email=getIntent().getStringExtra(Constants.KEY_USER_EMAIL);
    user=(User)getIntent().getSerializableExtra("User");
    hmp_Community=(HashMap<String, String>)getIntent().getSerializableExtra("hmpCommunity");
    _cummunities=(String[])getIntent().getSerializableExtra("Cummunities");
    _selections=(boolean[])getIntent().getSerializableExtra("selections");
    if(_selections!=null){
    	for( int i = 0; i < _selections.length; i++ )
		{
		  _selections[i]=false;
		}
    }
    Log.e("_cummunities length",""+_cummunities);
    
    //Initialize form components
	btnSearchPatient = (Button)findViewById(R.id.btnsearchpatient);
	btnCancel = (Button)findViewById(R.id.btncancel);	
	edtFirstName=(EditText)findViewById(R.id.edtfirstname);
	edtMiddleName = (EditText)findViewById(R.id.Middle_Name);
	edtLastName=(EditText)findViewById(R.id.edtlastname);

	edtPrefix = (EditText)findViewById(R.id.Prefix);
	edtSuffix = (EditText)findViewById(R.id.Suffix);
	edtMothersPrefix = (EditText)findViewById(R.id.Mothers_Prefix);
	edtMothersSuffix = (EditText)findViewById(R.id.Mothers_Suffix);
	edtMothersFamilyName = (EditText)findViewById(R.id.Mothers_Family_Name);
	edtMothersMiddleName = (EditText)findViewById(R.id.Mothers_Middle_Name);
	edtMothersGivenName = (EditText)findViewById(R.id.Mothers_Given_Name);
	city = (EditText)findViewById(R.id.City);
    street = (EditText)findViewById(R.id.Street);
    state = (EditText)findViewById(R.id.State);
    zip = (EditText)findViewById(R.id.Zip);
    country = (EditText)findViewById(R.id.Country);
    ssn = (EditText)findViewById(R.id.SSN);
    birthCity = (EditText)findViewById(R.id.BirthPlaceCity);
    birthStreet = (EditText)findViewById(R.id.BirthPlaceStreet);
    birthState = (EditText)findViewById(R.id.BirthPlaceState);
    birthZip = (EditText)findViewById(R.id.BirthPlaceZip);
    birthCountry = (EditText)findViewById(R.id.BirthPlaceCountry);
    telephone = (EditText)findViewById(R.id.TelephoneNums);
	
	edtPatientID=(EditText)findViewById(R.id.edtpatientid);
	edtDOB=(EditText)findViewById(R.id.edtdob);
	edtDeceasedDate = (EditText)findViewById(R.id.edtDeceasedDate);
	txtCommunity=(TextView)findViewById(R.id.edtCommunity);
	txtCommunity.setSingleLine();
	 
	//Initialize Communities

	imgDOB =(ImageView)findViewById(R.id.imgDOB);
	imgDeceasedDate =(ImageView)findViewById(R.id.imgDeceasedDate);
	rgSex =(RadioGroup)findViewById(R.id.radioSex);
	
	//set Listener for button
	btnSearchPatient.setOnClickListener(this);
	btnCancel.setOnClickListener(this);
    imgDOB.setOnClickListener(this);
    imgDeceasedDate.setOnClickListener(this);
    txtCommunity.setOnClickListener(this);
	}






@Override
protected void onResume() {
// TODO Auto-generated method stub
super.onResume();
	Context ctx = getApplicationContext();
	
	Log.e("_cummunities",""+_cummunities);
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
		/*Get Saved Communities List*/
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
				
				Intent callResultscreen = new Intent(AdvanceSearchPatient.this,PatientResult.class);
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
				com.mhise.util.MHISEUtil.displayDialog(AdvanceSearchPatient.this, 
						patientResult.getResult().ErrorMessage,patientResult.getResult().ErrorCode);
				else
					com.mhise.util.MHISEUtil.displayDialog(AdvanceSearchPatient.this, 
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
	case R.id.btncancel :
	{
		
		ResetValues();
		break;
	}
	
	case R.id.imgDOB:
	{
		clickedCalender=Constants.DATE_DIALOG_ID;
		DatePickerFragment1 newFragment = new DatePickerFragment1();
	    String tag ="datePicker";
	    FragmentManager fm = getSupportFragmentManager();
	    newFragment.show(fm, tag);
		//showDialog(Constants.DATE_DIALOG_ID);
		  
		break;
	}
	case R.id.imgDeceasedDate:
	{
		clickedCalender=Constants.Deceased_DATE_ID;
		DatePickerFragment1 newFragment = new DatePickerFragment1();
	    String tag ="datePicker";
	    FragmentManager fm = getSupportFragmentManager();
	    newFragment.show(fm, tag);
		//showDialog(Constants.Deceased_DATE_ID);
		  
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
	  	patientDemographics.setAdvanceSearch(true);
	  	Log.e("edtFirstName",""+edtFirstName.getText().toString());
	  	Log.e("edtMiddleName",""+edtMiddleName.getText().toString());
	  	Log.e("edtLastName",""+edtLastName.getText().toString());
	  	patientDemographics.setGivenName(edtFirstName.getText().toString().trim());
	  	patientDemographics.setFamilyName(edtLastName.getText().toString().trim());
	  	patientDemographics.setMiddleName(edtMiddleName.getText().toString().trim());
	  	patientDemographics.setPrefix(edtPrefix.getText().toString().trim());
	  	patientDemographics.setSuffix(edtSuffix.getText().toString().trim());
	  	patientDemographics.setTelephone(telephone.getText().toString().trim());
	  	PersonName mothersmaiden = new PersonName();
	  	mothersmaiden.setFamilyName(edtMothersFamilyName.getText().toString().trim());
	  	mothersmaiden.setGivenName(edtMothersGivenName.getText().toString().trim());
	  	mothersmaiden.setMiddleName(edtMothersMiddleName.getText().toString().trim());
	  	mothersmaiden.setPrefix(edtMothersPrefix.getText().toString().trim());
	  	mothersmaiden.setSuffix(edtMothersSuffix.getText().toString().trim());
	  	patientDemographics.setMotherMaiden(mothersmaiden);
	  	int selectedId = rgSex.getCheckedRadioButtonId();
	    RadioButton  radioSexButton = (RadioButton) findViewById(selectedId);		
	    patientDemographics.setGender(radioSexButton.getText().toString().trim());
	  	patientDemographics.setDOB(edtDOB.getText().toString().trim());
	  	patientDemographics.setDeceasedDate(edtDeceasedDate.getText().toString().trim());
	  	patientDemographics.setState(state.getText().toString().trim());
	  	patientDemographics.setStreet(street.getText().toString().trim());
	  	patientDemographics.setCity(city.getText().toString().trim());
	  	patientDemographics.setCountry(country.getText().toString().trim());
	  	patientDemographics.setSSN(ssn.getText().toString().trim());
	  	patientDemographics.setZip(zip.getText().toString().trim());
	  	patientDemographics.setBirthPlaceCity(birthCity.getText().toString().trim());
	  	patientDemographics.setBirthPlaceCountry(birthCountry.getText().toString().trim());
	  	patientDemographics.setBirthPlaceState(birthState.getText().toString().trim());
	  	patientDemographics.setBirthPlaceStreet(birthStreet.getText().toString().trim());
	  	patientDemographics.setBirthPlaceZip(birthZip.getText().toString().trim());
	   
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
                clickedCalender=Constants.DATE_DIALOG_ID;
                return new DatePickerDialog(this,
                            mDateSetListener,
                            mYear, mMonth, mDay);
                
            case Constants.Deceased_DATE_ID:
                final Calendar cd = Calendar.getInstance();
                 mYear = cd.get(Calendar.YEAR);
                 mMonth = cd.get(Calendar.MONTH);
                 mDay = cd.get(Calendar.DAY_OF_MONTH);
                 clickedCalender=Constants.Deceased_DATE_ID;
                 return new DatePickerDialog(this,
                             mDateSetListener,
                             mYear, mMonth, mDay);
                
                
            case   Constants.COMMUNITY_DIALOG_ID:
            	{
            		clear = false;
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
            	clickedCalender=Constants.DATE_DIALOG_ID;
                ((DatePickerDialog) dialog).updateDate(mYear, mMonth, mDay);
                break;
            }
            case Constants.Deceased_DATE_ID:
            {
            	clickedCalender=Constants.Deceased_DATE_ID;
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
           	
            }        
        }
  
    } 
    
    private static void updateDisplay(int id) {
    	mMonth =mMonth+1;
    	String _mMonth;
    	String _mDay ;
    	
    	Log.e("date changed","id"+id);
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
	    	
	    	switch (id) {
		    	case Constants.DATE_DIALOG_ID:
		    	{
		    		Log.e("date changed","edtDOB"+id);
		    		 edtDOB.setText(
		    	        		
		    		          new StringBuffer()
		    		                    // Month is 0 based so add 1
		    		                    .append(_mMonth).append("/")
		    		                    .append(_mDay).append("/")
		    		                    .append(mYear)
		    		                    );
		    		 break;
		    	}
		    	case Constants.Deceased_DATE_ID:
		    	{
		    		Log.e("date changed","edtDeceasedDate"+id);
		    		edtDeceasedDate.setText(
	    	        		
		    		          new StringBuffer()
		    		                    // Month is 0 based so add 1
		    		                    .append(_mMonth).append("/")
		    		                    .append(_mDay).append("/")
		    		                    .append(mYear)
		    		                    );
		    		break;
		    	}
    		}
       
    	}

    private DatePickerDialog.OnDateSetListener mDateSetListener =
            new DatePickerDialog.OnDateSetListener() {

                public void onDateSet(DatePicker view, int year, int monthOfYear,
                        int dayOfMonth) {
                    mYear = year;
                    mMonth = monthOfYear;
                    mDay = dayOfMonth;
                    updateDisplay(clickedCalender);
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
	  edtFirstName.setText(null);
	  edtLastName.setText(null);
	  edtPatientID.setText(null);
	  edtDOB.setText(null);
	  edtDeceasedDate.setText(null);
	  txtCommunity.setText(null);
	
	  rgSex.check(R.id.radiomale);
	 
	  edtMiddleName.setText(null);
	  edtPrefix.setText(null);
	  edtSuffix.setText(null);
	  edtMothersPrefix.setText(null);
	  edtMothersSuffix.setText(null);
	  edtMothersFamilyName.setText(null);
	  edtMothersMiddleName.setText(null);
	  edtMothersGivenName.setText(null);
	  city.setText(null);
	  street.setText(null);
	  state.setText(null);
	  zip.setText(null);
	  country.setText(null);
	  ssn.setText(null);
	  birthCity.setText(null);
	  birthStreet.setText(null);
	  birthState.setText(null);
	  birthZip.setText(null);
	  birthCountry.setText(null);
	  telephone.setText(null);
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
			 	Log.e("GetCommunitiesAsyncClass ","called");
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
						//_cummunities =MHISEUtil.extractCommunityArray(comResult,getApplicationContext()).keySet().toArray(_cummunities);
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
            updateDisplay(clickedCalender);
		}
}


}
