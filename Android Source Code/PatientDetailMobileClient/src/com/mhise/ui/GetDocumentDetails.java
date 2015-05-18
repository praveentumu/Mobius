package com.mhise.ui;

import java.util.ArrayList;
import java.util.HashMap;
import org.apache.http.impl.client.DefaultHttpClient;
import android.R.color;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnCancelListener;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import com.mhise.constants.Constants;
import com.mhise.constants.MobiusDroid;
import com.mhise.model.Assertion;
import com.mhise.model.CommunityResult;
import com.mhise.model.GetDocumentMetaDataResult;
import com.mhise.model.MasterData;
import com.mhise.model.User;
import com.mhise.requests.GetDocumentMetaDataRequest;
import com.mhise.requests.RequestBase;
import com.mhise.response.GetCommunities;
import com.mhise.response.GetDocumentMetaDataResponse;
import com.mhise.response.GetMasterDataParser;

import com.mhise.util.DataConnectionManager;
import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;

public class GetDocumentDetails extends BaseMenuOptionsAcitivity{
	
	ListView docDetailList;
	Button btnGet;
	TextView txtCommunity;
	private String patientID; 
	private String role;
	private String userType;
	private String email;	
	private String patientname;
	String TAG_PURPOSE_OF_USE ="PurposeOfUse";
	protected boolean[] _selections ;
	protected ArrayList<String> _selectedcommunities ;
	protected String[] _cummunities ;
	
	HashMap< String, String> hmp_Community;
	public static GetDocumentMetaDataResult responseObj;
	private EfficientAdapter adapter ;
	private GetpurposeAsyncClass getpurposeAsyncObj;
	private GetCommunitiesAsyncClass getCommunitiesAsyncObj;
	private RequestResponseHandler1 requestResponseHandler1;
	private User loginUser;
	private boolean locallyDocumentsBollean  = true;
	CheckBox locallyAvailableDocuments;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
			super.onCreate(savedInstanceState);
			
			setContentView(R.layout.document_details);
			patientID = getIntent().getStringExtra(Constants.KEY_USER_ID);
			patientname = getIntent().getStringExtra(Constants.KEY_NAME);
			role = getIntent().getStringExtra(Constants.KEY_ROLE);	
			userType = getIntent().getStringExtra(Constants.KEY_USER_TYPE);	
			email = getIntent().getStringExtra(Constants.KEY_USER_EMAIL);
	
			loginUser=(User)getIntent().getSerializableExtra("loginUser");
			
			docDetailList =(ListView)findViewById(R.id.lvDocDetailList);

			adapter = new EfficientAdapter(this);
			
			docDetailList.setAdapter( adapter);
		
			setListItemClickListener(docDetailList);
			
			TextView patientName =(TextView)findViewById(R.id.txtPatient);
			
			//patientName.setText("Patient: "+MHISEUtil.makeNameInUpperCase(patientname));
			if (loginUser.getUserType().equalsIgnoreCase("Patient")){
				patientName.setText("Patient: "+MHISEUtil.makeNameInUpperCase(loginUser));
			}else if(loginUser.getUserType().equalsIgnoreCase("Provider")){
				patientName.setText("Patient: "+patientname);
			}
			txtCommunity = (TextView)findViewById(R.id.txt2community);
			
			txtCommunity.setOnClickListener(new View.OnClickListener() {
				
				@Override
				public void onClick(View v) {
					// TODO Auto-generated method stub
					 showDialog(Constants.COMMUNITY_DIALOG_ID);
				}
			});
			Log.e("in document details 1"," in document details");
			locallyAvailableDocuments=(CheckBox) findViewById(R.id.locallyDocumentCheckBox);
			
			locallyAvailableDocuments.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					if(locallyAvailableDocuments.isChecked())locallyDocumentsBollean=true;
					else locallyDocumentsBollean=false;
				}
			});
			
			btnGet =(Button)findViewById(R.id.btnGet);
			Log.e("in document details 2"," in document details");
			btnGet.setOnClickListener(new View.OnClickListener() {
			
			@Override
				public void onClick(View v) {
				// TODO Auto-generated method stub
				
				Context ctx = getApplicationContext();
		    	
		    	boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(ctx);
		    	
		    	if(!isDataConnectionAvailable)
		    	{
		    		MHISEUtil.displayDialog(GetDocumentDetails.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage) ,getResources().getString(R.string.DataConnectionAvailabiltyMessage) );
		    	}
		    	else
		    	{
		    		Assertion assertion = new Assertion();
				  	assertion.setAssertionMode(Constants.ASSERTION_MODE_DEFAULT);
				  	assertion.setPurposeOfUse(Constants.DEFAULT_PURPOSE);
				  	Log.e("login User",""+loginUser);
				  	assertion.setUserInformation(loginUser);
				  	assertion.setNhinCommunity(MobiusDroid.homeCommunity);
				  	assertion.setHaveSignature(true);
		    		
		    		/*if (_selectedcommunities.size()>0)
		    		{*/
		    			//Logger.debug("GetDocumentDetailsonList item selection","true");	
				  	Log.e("on click", "getDocumentDetailsRequest");
				  	
		    			String requestEnvelope =new GetDocumentMetaDataRequest().getDocumentDetailsRequest(assertion, patientID, _selectedcommunities,hmp_Community,locallyDocumentsBollean);
		    			showDialog(Constants.GET_DOCUMENT_DETAILS_PROGRESS_DIALOG);
		    			requestResponseHandler1 = new RequestResponseHandler1();
		    			requestResponseHandler1.execute(requestEnvelope);  	
		    		/*}
		    		else
		    		{
		    			MHISEUtil.displayDialog(GetDocumentDetails.this, getResources().getString(R.string.error_select_community_empty),getResources().getString(R.string.error_select_community_empty_title));
		    		}*/
		    		
		    	}
				
			}
			});
			
			
	}	
	
    @Override
    protected void onResume() {
    // TODO Auto-generated method stub
    super.onResume();
    try{
    	Log.e("dipti in resume","here in resume");
		_cummunities = MobiusDroid._arrPHIComunities;
		if(_cummunities ==null)
		{ 
			//Logger.debug("GetDocumentDetailsOnCreate -->onCreate", "Loading communities");
			
			boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
			  if(!isDataConnectionAvailable)
	    		{
	    		MHISEUtil.displayDialog(GetDocumentDetails
	    				.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
	    		}
	    		else
	    		{	
			
			showDialog(Constants.GET_COMMUNITY_PROGRESS_DIALOG);
			SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
			String params=null;
				try{
					Log.i("assigning community id ",""+sharedPreferences.getString(Constants.HOME_COMMUNITY_ID, null));
					 params =com.mhise.requests.RequestBase.getPHISourceRequest(sharedPreferences.getString(Constants.HOME_COMMUNITY_ID, null), patientID);
					}
				catch (NullPointerException e) {
				Logger.debug("GetDocumentDetails-->on resume", ""+e);
			}
    		getCommunitiesAsyncObj = new GetCommunitiesAsyncClass();
    		getCommunitiesAsyncObj.execute(params);
    		
    		_cummunities = MobiusDroid._arrPHIComunities;
    		Log.e("_cummunities hmp_Community _selections","fgdfgd");
    		
    		hmp_Community= MobiusDroid.hmp_PHICommunityID;
    		
    		Log.e("_cummunities hmp_Community _selections 1","fgdfgd");
    		_selections = new boolean[_cummunities.length];
    		
    		Log.e("_cummunities hmp_Community _selections","");
	    }
		}
		else
		{
			_selections = new boolean[_cummunities.length];
			hmp_Community= MobiusDroid.hmp_PHICommunityID;
		}
		
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Logger.error("GetDocumentDetails onCreate", ""+e);
		}

    }
	
/*=======
    @Override
    protected void onResume() {
    // TODO Auto-generated method stub
    super.onResume();
    try{
		_cummunities = MobiusDroid._arrPHIComunities;
		if(_cummunities ==null)
		{ 
			//Logger.debug("GetDocumentDetailsOnCreate -->onCreate", "Loading communities");
			
			boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
			  if(!isDataConnectionAvailable)
	    		{
	    		MHISEUtil.displayDialog(GetDocumentDetails
	    				.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
	    		}
	    		else
	    		{	
			
			showDialog(Constants.GET_COMMUNITY_PROGRESS_DIALOG);
    		String[] params ={com.mhise.requests.RequestBase.getPHISourceRequest(MobiusDroid.HomeCommunityID, patientID)};
    		getCommunitiesAsyncObj = new GetCommunitiesAsyncClass();
    		getCommunitiesAsyncObj.execute(params); 
    		_cummunities = MobiusDroid._arrPHIComunities;
    		hmp_Community= MobiusDroid.hmp_PHICommunityID;
    		_selections = new boolean[_cummunities.length];
	    		}
		}
		else
		{
			_selections = new boolean[_cummunities.length];
			hmp_Community= MobiusDroid.hmp_PHICommunityID;
		}
		
		}
		catch (NullPointerException e) {
			// TODO: handle exception
			Logger.error("GetDocumentDetails onCreate", ""+e);
		}

    }
	
>>>>>>> .r4471*/
	private class RequestResponseHandler1 extends AsyncTask<String, Void, String> {
	
			String	getDocumentMetaDataRequest;
			
			@Override
			protected String doInBackground(String... params) {
				
				
				this.getDocumentMetaDataRequest=params[0];	
				SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
			 	DefaultHttpClient httpClient = MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));
				String response = com.mhise.util.MHISEUtil.CallWebService(
						Constants.HTTPS_URL, XmlConstants.ACTION_FOR_GET_DOCUMENT_METADATA, getDocumentMetaDataRequest,httpClient);
				return response;
			}
			@Override
			protected void onPostExecute(String result) {
					// TODO Auto-generated method stub
				
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{
					removeDialog(Constants.GET_DOCUMENT_DETAILS_PROGRESS_DIALOG);
					responseObj =	new GetDocumentMetaDataResponse().parseXML(resultDoc);

					try{
					if(responseObj.result.IsSuccess.equals("true"))
					{
						if( responseObj.arrDocDetails != null )
							if(responseObj.arrDocDetails.size()>0)
								adapter.notifyDataSetChanged();
							else
								MHISEUtil.displayDialog(GetDocumentDetails.this,getResources().getString(R.string.error_No_Documents_available_msg),getResources().getString(R.string.error_No_Documents_available_title));	
					}
					else if(responseObj.result.IsSuccess.equals("false"))
					{
						MHISEUtil.displayDialog(GetDocumentDetails.this, responseObj.result.ErrorMessage,responseObj.result.ErrorCode);	
						responseObj =null;
						adapter.notifyDataSetChanged();
					}
					}catch (NullPointerException e) {
						// TODO: handle exception
						Logger.debug("GetDocumentDetails-->RequestResponseHandler1", ""+e);
					}
					catch (ArrayIndexOutOfBoundsException e) {
						// TODO: handle exception
						Logger.debug("GetDocumentDetails-->RequestResponseHandler1", ""+e);
					}
					
				}
				
	
			}
		}	
	
	 private class GetpurposeAsyncClass extends AsyncTask<String, Void, String>
	 {
			String documentID;
		 @Override
		protected String doInBackground(String... params) {
	
			String request = params[0];
			documentID=params[1];
			
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL, XmlConstants.ACTION_GETMASTERDATA, request, new DefaultHttpClient());
			return response;
		}
		 
		 @Override
		 
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);				 
			 removeDialog(Constants.GET_PURPOSE_PROGRESS_DIALOG);				 	
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{	
					MasterData masterData =	new GetMasterDataParser().parseMasterDataResponse(resultDoc);
					//Logger.debug("GetDocumentDetailsis  purpose class launched ?", ""+masterData.result.IsSuccess);
					if(masterData.result.IsSuccess.equals("true"))
					{	
						//Logger.debug("GetDocumentDetails-->array list size", ""+masterData._arrDescription.size());
						//Logger.debug("GetDocumentDetails-->is  purpose class launched ?11", ""+masterData.result.IsSuccess);
						MobiusDroid._arrPurpose = new String[masterData._arrDescription.size()];
						MobiusDroid._arrPurpose = masterData._arrDescription.toArray(MobiusDroid._arrPurpose);
						//Logger.debug("GetDocumentDetails-->array size", ""+MobiusDroid._arrPurpose.length );
						Intent callPurpose = new Intent(GetDocumentDetails.this,SelectPurpose.class); 
						callPurpose.putExtra(Constants.KEY_USER_ID, patientID); 
						//Logger.debug("GetDocumentDetails-->KEY_USER_ID", ""+patientID );
						callPurpose.putExtra(Constants.KEY_ROLE, role); 
						//Logger.debug("GetDocumentDetails-->KEY_ROLE", ""+role );
						callPurpose.putExtra(Constants.KEY_DOCUMENT_ID,documentID);  
						callPurpose.putExtra(Constants.KEY_USER_EMAIL,email);
						callPurpose.putExtra("loginUser", loginUser);
						callPurpose.putExtra("localData", locallyDocumentsBollean);
						//Logger.debug("GetDocumentDetails-->array size", ""+MobiusDroid._arrPurpose.length );
						
						//Logger.debug("GetDocumentDetails-->is  purpose class launched ?22", ""+masterData.result.IsSuccess);
						startActivity(callPurpose);
						
					}
					else if(masterData.result.IsSuccess.equals("false"))
					{
						MHISEUtil.displayDialog(GetDocumentDetails.this, masterData.result.ErrorMessage,masterData.result.ErrorCode);	    
					}
					
				}
		 }

		}

	 
	 @Override
	protected void onSaveInstanceState(Bundle outState) {
		// TODO Auto-generated method stub
		super.onSaveInstanceState(outState);
		//Logger.debug("Saved Instance state called","patiendID"+patientID);
		outState.putSerializable("DocumentDetailList", responseObj);
		outState.putStringArray("Selected_Communities",_cummunities);
		outState.putBooleanArray("SelectionArray", _selections);
		outState.putString("PatientID", patientID);
		outState.putString("Role", role);
		outState.putString("EMAIL", email);
		outState.putString("CommunityText", txtCommunity.getText().toString());
		outState.putSerializable("_selectedcommunities",_selectedcommunities);
   		outState.putSerializable("HashMap_Communities",hmp_Community);
		
	}
	 
	 @SuppressWarnings("unchecked")
	@Override
	protected void onRestoreInstanceState(Bundle savedInstanceState) {
		/*Get Saved Document Detail List*/
		super.onRestoreInstanceState(savedInstanceState);
		responseObj = (GetDocumentMetaDataResult)savedInstanceState.getSerializable("DocumentDetailList");
		adapter.notifyDataSetChanged();
		
		/*Get Saved Community Details*/
		_cummunities = savedInstanceState.getStringArray("Selected_Communities");
		_selections=savedInstanceState.getBooleanArray("SelectionArray");
		patientID=savedInstanceState.getString("PatientID");
		role=savedInstanceState.getString("Role");
		email=savedInstanceState.getString("EMAIL");
		hmp_Community= (HashMap<String, String>) savedInstanceState.getSerializable("HashMap_Communities");
		txtCommunity.setText(savedInstanceState.getString("CommunityText"));
		try{
		_selectedcommunities =(ArrayList<String>)savedInstanceState.getSerializable("_selectedcommunities");
		}
		catch (ClassCastException e) {
			// TODO: handle exception
			Logger.debug("Get document detail onRestore instance state", "e"+e);
		}
		
		
	}
	 
	private  class EfficientAdapter extends BaseAdapter {
	       
		private LayoutInflater mInflater;

	        public EfficientAdapter(Context context) {
	            // Cache the LayoutInflate to avoid asking for a new one each time.
	            mInflater = LayoutInflater.from(context);   
	        }

	        /**
	         * The number of items in the list is determined by the number of speeches
	         * in our array.
	         *
	         * 
	         */
	        public int getCount() {         
	       
	        	if(isDataAvailable())
	        	{
	        		return  responseObj.arrDocDetails.size();
	        	}
	        	else
	        	{
	        		return 0;
	        	}

	        }

	        /**
	         * Since the data comes from an array, just returning the index is
	         * sufficent to get at the data. If we were using a more complex data
	         * structure, we would return whatever object represents one row in the
	         * list.
	         *
	         * @see android.widget.ListAdapter#getItem(int)
	         */
	        public Object getItem(int position) {
	            return position;
	        }

	        /**
	         * Use the array index as a unique id.
	         *
	         * @see android.widget.ListAdapter#getItemId(int)
	         */
	        public long getItemId(int position) {
	            return position;
	        }
	        ViewHolder holder;
	        public View getView(final int position, View convertView, ViewGroup parent) {
	        
	        	 
	        	 
	        	 
	        		//Logger.debug("position", ""+position);
		            if (convertView == null)
		            {

		                convertView = mInflater.inflate(R.layout.docdetailslistitem, null);
		                holder = new ViewHolder();
		                holder.textDocName = (TextView) convertView.findViewById(R.id.txtDocName);
		                holder.textAuthor = (TextView) convertView.findViewById(R.id.txtAutorValue);
		                holder.textDataSource=(TextView)convertView.findViewById(R.id.textDataSourceValue);
		                holder.textCreatedDate=(TextView)convertView.findViewById(R.id.textDate);
		                holder.imgRefer = (ImageView)convertView.findViewById(R.id.imgRefer);
		                holder.textDocID =(TextView)convertView.findViewById(R.id.DocID);
		                
		                if(userType.equalsIgnoreCase("Patient"))
		                	holder.imgRefer.setVisibility(View.GONE);
		                	
		                else
		                	holder.imgRefer.setVisibility(View.VISIBLE);	
		                
		                convertView.setTag(holder);
		                
                
		                if (position%2 ==0)
		                	convertView.setBackgroundColor(Color.LTGRAY);
			   	        else
			   	        {	   	        
			   	        	convertView.setBackgroundColor(color.holo_blue_light);
			   	        }
		                
		            } 
		            else 
		            {            	
		                holder = (ViewHolder) convertView.getTag();
		                
		                if (position%2 ==0)
		                	convertView.setBackgroundColor(Color.LTGRAY);
			   	        else
			   	        {
			   	        	convertView.setBackgroundColor(color.holo_blue_light);
			   	        }
		                
		            }
		            holder.imgRefer.setOnClickListener(new View.OnClickListener() {
						
						@Override
						public void onClick(View v) {
							// TODO Auto-generated method stub
							Log.e("position",""+position);
							Log.e("DocumentUniqueId()",""+responseObj.arrDocDetails.get(position).getDocumentUniqueId());
							SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
							String serialNo=  sharedPreferences.getString(Constants.KEY_SERIAL_NUMBER, null);
							Intent httpIntent = new Intent(Intent.ACTION_VIEW);
				  			httpIntent.setData(Uri.parse(Constants.REFER_URL+"?Serial="+serialNo+"&DocumentID="+responseObj.arrDocDetails.get(position).getDocumentUniqueId()));
				  			startActivity(httpIntent);
							//callRefer(responseObj.arrDocDetails.get(position).getDocumentUniqueId());
						}
					});
		            if(responseObj !=null)
		            {
		            	if(responseObj.arrDocDetails!=null )
		            	{
		            		if(responseObj.arrDocDetails.size()>0)
		            		{
		            			holder.textDocName.setText(responseObj.arrDocDetails.get(position).getDocumentTitle()) ;
		            			holder.textAuthor.setText(responseObj.arrDocDetails.get(position).getAuthor()) ;
		            			holder.textDataSource.setText(responseObj.arrDocDetails.get(position).getDataSource()) ;
		            			holder.textCreatedDate.setText(
		            					MHISEUtil.convertStringToDateString(responseObj.arrDocDetails.get(position).getCreatedOn()));
		            			holder.textDocID.setText(responseObj.arrDocDetails.get(position).getDocumentUniqueId());
		            		}
		            	 } 
		            }
		           
	            return convertView;
	       
	    }
	        
	        
	   

			public void finish() {
				// TODO Auto-generated method stub
				
			}
	   
	}

	private  void callRefer(String docID)
	{
		Intent callRefer = new Intent(GetDocumentDetails.this,ReferPatient.class);
		callRefer.putExtra(Constants.KEY_DOCUMENT_ID, docID);
		startActivity(callRefer);
	 
	}
	
	 class ViewHolder {
	        TextView textDocName;
	        TextView textDocID;
	        TextView textCreatedDate;
	        TextView textDataSource;
	        TextView textAuthor;
	        ImageView imgRefer;
	    }
	   	
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
	        if (keyCode == KeyEvent.KEYCODE_BACK) {
	        	adapter.finish();
	            this.finish();
	            return true;
	        }
	        return super.onKeyDown(keyCode, event);
	    }
	
    @Override
	public Dialog onCreateDialog(int id) {
    	
        switch (id) {
                
                
            case   Constants.COMMUNITY_DIALOG_ID:
            	{
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
    		 
    		// Logger.debug("GetDocumentDetails-->inside on create dialog", "true");
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
            	
            case   Constants.GET_PURPOSE_PROGRESS_DIALOG:
        	{
        		 ProgressDialog dialog = new ProgressDialog(this);
        		 dialog.setMessage(getResources().getString(R.string.progressbar_msg_purpose));          		 
        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
        		// Logger.debug("GetDocumentDetails-->GET_PURPOSE_PROGRESS_DIALOG", "true");
        		 dialog.setCancelable(true);
        		 dialog.setCanceledOnTouchOutside(false);
        		 dialog.setOnCancelListener(new OnCancelListener() {
						
						@Override
						public void onCancel(DialogInterface dialog) {
							// TODO Auto-generated method stub
							dialog.dismiss();
							 getpurposeAsyncObj.cancel(true);
							
						}
					});
        		 return dialog;
        } 
            case   Constants.GET_DOCUMENT_DETAILS_PROGRESS_DIALOG:
        	{
        		 ProgressDialog dialog = new ProgressDialog(this);
        		 dialog.setMessage(getResources().getString(R.string.progressbar_msg_doc_Details));          		 
        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
        		// Logger.debug("GetDocumentDetails-->GET_DOCUMENT_DETAILS_PROGRESS_DIALOG", "true");
        		 dialog.setCancelable(true);
        		 dialog.setCanceledOnTouchOutside(false);
        		 dialog.setOnCancelListener(new OnCancelListener() {
						
						@Override
						public void onCancel(DialogInterface dialog) {
							// TODO Auto-generated method stub
							dialog.dismiss();
							 requestResponseHandler1.cancel(true);
							
						}
					});
        		 return dialog;
        } 
     
   	
        	
        }
       return null;
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

	private ArrayList<String> setSelectedCommunity() {
		// TODO Auto-generated method stub
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
		//Logger.debug("Selected Community",""+_selectedcommunities.size());
		return _selectedcommunities;
		
	}
	
	public class DialogSelectionClickHandler implements DialogInterface.OnMultiChoiceClickListener
		{
			public void onClick( DialogInterface dialog, int clicked, boolean selected )
			{
				//Logger.debug( "ME", _cummunities[ clicked ] + " selected: " + selected );
			}
		}
		
	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		
		_selections =null;
		//_selectedcommunities.clear();
		responseObj =null;
		
		super.onDestroy();
	}
	
	private void setListItemClickListener(final ListView listView)
	{
	
	/*	View v =getLayoutInflater().inflate(R.layout.docdetailslistitem, null);
		
		Button  btnRefer =(Button) v.findViewById(R.id.Refer);
		btnRefer.setOnClickListener(new View.OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				Intent callRefer = new Intent(GetDocumentDetails.this,ReferPatient.class);
				callRefer.putExtra(Constants.KEY_DOCUMENT_ID, responseObj.arrDocDetails.get(listView.getSelectedItemPosition()).getDocumentUniqueId());
				startActivity(callRefer);
			}
		});*/
		
		listView.setOnItemClickListener(new OnItemClickListener() {

	
			@Override
			public void onItemClick(AdapterView<?> arg0, View arg1, int position,
					long arg3) {
				// TODO Auto-generated method stub
				if(isDataAvailable())
				{
					//Logger.debug("GetDocumentDetailsData avialable in list but fetching purpose","inside if of setListItemClickListener");
					
					if (userType.equalsIgnoreCase("Patient"))
					{
						Intent callDocumentView = new Intent(GetDocumentDetails.this,DocumentViewScreen.class);
						callDocumentView.putExtra(Constants.KEY_USER_ID, patientID);
						Log.e("here for list position",""+position);
						callDocumentView.putExtra(Constants.KEY_DOCUMENT_ID,responseObj.arrDocDetails.get(position).getDocumentUniqueId());
						//Logger.debug("GetDocumentDetails-->setListItemClickListener-->KEY_DOCUMENT_ID", "KEY_DOCUMENT_ID"+responseObj.arrDocDetails.get(position).getDocumentUniqueId());
						//Logger.debug("GetDocumentDetails-->setListItemClickListener--> document id set", "true"+responseObj.arrDocDetails.get(position).getDocumentUniqueId());//callDocumentView.putExtra(Constants.KEY_DOCUMENT_ID,documentID);   
						callDocumentView.putExtra(Constants.KEY_PURPOSE,Constants.KEY_PURPOSE);
						callDocumentView.putExtra(Constants.KEY_ROLE,role);
						callDocumentView.putExtra(Constants.KEY_USER_EMAIL,email);
						callDocumentView.putExtra("loginUser", loginUser);
						//Logger.debug("GetDocumentDetails-->setListItemClickListener-->call document view screen", "true");
						startActivity(callDocumentView);
						
					}
					else if (userType.equalsIgnoreCase("Provider"))
					{
						fetchPurposeList(position);
					}
				}
				else
				{
					MHISEUtil.displayDialog(GetDocumentDetails.this, getResources().getString(R.string.error_No_Documents_available_msg),getResources().getString(R.string.error_No_Documents_available_title));
				}
			}	
		});
	}
	
	private void fetchPurposeList(int position) {
			   
		Log.e("login User for select purpose",""+loginUser);
		   if(MobiusDroid._arrPurpose == null ||MobiusDroid._arrPurpose.length<1)
		   {
			   Log.e("login User for select purpose"," in if");
			   boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
		    	
		    	if(!isDataConnectionAvailable)
		    	{
		    		MHISEUtil.displayDialog(this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle)  );
		    	}
		    	else
		    	{	
			    	//Logger.debug("GetDocumentDetails-->Inside fetchPurposeList","if ->else condition");	
					showDialog(Constants.GET_PURPOSE_PROGRESS_DIALOG);
					String request = RequestBase.getMasterDataRequest(TAG_PURPOSE_OF_USE) ;

					String[] params=new String[]{request,responseObj.arrDocDetails.get(position).getDocumentUniqueId()};
					getpurposeAsyncObj = new GetpurposeAsyncClass();
					
					getpurposeAsyncObj.execute(params);
				
		    	}
	  
		   }
	   		else
	   		{
	   			Log.e("login User for select purpose"," in else");
			   Intent callPurpose = new Intent(GetDocumentDetails.this,SelectPurpose.class); 
			   	
			   callPurpose.putExtra(Constants.KEY_USER_ID, patientID);
			 
				//Logger.debug("GetDocumentDetails-->setListItemClickListener-->KEY_DOCUMENT_ID", "KEY_DOCUMENT_ID"+responseObj.arrDocDetails.get(position).getDocumentUniqueId());
				//Logger.debug("GetDocumentDetails-->setListItemClickListener--> document id set", "true"+responseObj.arrDocDetails.get(position).getDocumentUniqueId());//callDocumentView.putExtra(Constants.KEY_DOCUMENT_ID,documentID);   
				callPurpose.putExtra(Constants.KEY_PURPOSE,Constants.KEY_PURPOSE);
				callPurpose.putExtra(Constants.KEY_ROLE,role);
				callPurpose.putExtra(Constants.KEY_USER_EMAIL,email);
				callPurpose.putExtra(Constants.KEY_DOCUMENT_ID,responseObj.arrDocDetails.get(position).getDocumentUniqueId());
				
				callPurpose.putExtra("loginUser", loginUser);
				callPurpose.putExtra("localData", locallyDocumentsBollean);
				
			   startActivity(callPurpose);
	   		}
	   }
	
	private static boolean isDataAvailable()
	{
		boolean isListViewFilled =false ;
		if (responseObj !=null )
    	{
    		if(responseObj.arrDocDetails !=null)
    		{
    			if( responseObj.arrDocDetails.size() > 0)
    			{ 
    				return true;
    			}		
    			
    		}
    	
    	}	
    	return isListViewFilled;
    }
	
	

	 private class GetCommunitiesAsyncClass extends AsyncTask<String, Void, String>
	 {
		 	@Override
		 	protected String doInBackground(String... params) 
		 	{
		 		SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);		 		
				DefaultHttpClient httpClient= MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));           
					
				String request = params[0];	
			String response = com.mhise.util.MHISEUtil.CallWebService(
						Constants.HTTPS_URL, XmlConstants.ACTION_GET_PHI_SOURCE, request,httpClient );
			
			return response;	
	}	

			 @Override
			protected void onPostExecute(String result) {
				 
			super.onPostExecute(result);	
			removeDialog(Constants.GET_COMMUNITY_PROGRESS_DIALOG);
				 	
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{
						CommunityResult comResult   =GetCommunities.parsePHISourceXML(resultDoc); 
						if (comResult.getResult().IsSuccess.equalsIgnoreCase("true"))
						{
							hmp_Community = MHISEUtil.extractCommunityArray(comResult);
							
							Log.e("hmp_Community size dipti",""+hmp_Community.size());
							MobiusDroid.hmp_PHICommunityID =hmp_Community;
							
							MobiusDroid._arrComunities = new String[hmp_Community.size()];
							
							_cummunities = new String[hmp_Community.size()];
							
							MobiusDroid._arrPHIComunities= MHISEUtil.extractCommunityArray(comResult).keySet().toArray(MobiusDroid._arrComunities);
							
							_cummunities =MHISEUtil.extractCommunityArray(comResult).keySet().toArray(_cummunities);
							
							_selections =  new boolean[ _cummunities.length ];
							
						}
					}
					Log.e("hmp_Community size dipti8","gdfgd");
			 }
			 
	 }

	
	@Override
	public void onBackPressed() {
		// TODO Auto-generated method stub
		super.onBackPressed();
		this.finish();
		//System.exit(0);
	}
}
