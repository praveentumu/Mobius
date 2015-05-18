package com.mhise.ui;


import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckedTextView;
import android.widget.ListView;

import com.mhise.constants.MobiusDroid;
import com.mhise.constants.Constants;
import com.mhise.model.User;
import com.mhise.util.DataConnectionManager;
import com.mhise.util.Logger;
//import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;


public class SelectPurpose extends Activity{
	
		ListView lvpurposeList;
		Button btngetDocument;	
		private String patientID;
		private String documentID;	
		private String purpose;	
		private String role;	
		private String email;
		private User loginUser;
		private boolean localData;
		
		@Override
		public void onCreate(Bundle savedInstanceState) {
	        super.onCreate(savedInstanceState);
	        requestWindowFeature(Window.FEATURE_NO_TITLE);
	       
	        setContentView(R.layout.selectpurpose);
	       
	        Intent callingIntent= getIntent();
	        patientID=  callingIntent.getStringExtra(Constants.KEY_USER_ID);
	        documentID=  callingIntent.getStringExtra(Constants.KEY_DOCUMENT_ID); 
	        email = callingIntent.getStringExtra(Constants.KEY_USER_EMAIL);
	        role=  callingIntent.getStringExtra(Constants.KEY_ROLE); 
	        lvpurposeList = (ListView)findViewById(R.id.lvpurpose);
	        
	        btngetDocument = (Button)findViewById(R.id.btngetDocument);  
	        loginUser=(User)getIntent().getSerializableExtra("loginUser");
	        localData= getIntent().getBooleanExtra(("localData"),false);
	        
	        try{  	
	        	lvpurposeList.setAdapter(new ArrayAdapter<String>(this,
	            android.R.layout.simple_list_item_single_choice, MobiusDroid._arrPurpose ));
	        }
	        catch (NullPointerException e) {
				// TODO: handle exception
	        	Logger.debug("SelectPurpose-->onCreate ", ""+e);
			}
	        lvpurposeList.setItemsCanFocus(false);
	        lvpurposeList.setChoiceMode(ListView.CHOICE_MODE_SINGLE);	        
	        setListenerForList(lvpurposeList); 
	        handleButtonEvent(btngetDocument);
		}
		
		private  void handleButtonEvent(Button button)
	    {
			
			try{
				button.setOnClickListener(new View.OnClickListener() {	
			@Override
			public void onClick(View v) {
				Log.e("login User in select purpose",""+loginUser);
				 if(purpose !=null)
				    {
			    		boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
			    		
			    		if(!isDataConnectionAvailable)
				    	{
			    			Log.e("login User in select purpose"," in if");
				    		MHISEUtil.displayDialog(SelectPurpose.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
				    	}
				    	else
				    	{
				    		Log.e("login User in select purpose"," in else");
				    		Intent callDocumentView = new Intent(SelectPurpose.this,DocumentViewScreen.class);
				    		callDocumentView.putExtra(Constants.KEY_USER_ID, patientID); 
				    		callDocumentView.putExtra(Constants.KEY_DOCUMENT_ID,documentID);   
				    		callDocumentView.putExtra( Constants.KEY_PURPOSE,purpose);
				    		callDocumentView.putExtra( Constants.KEY_ROLE,role);
				    		callDocumentView.putExtra( Constants.KEY_USER_EMAIL,email);
				    		callDocumentView.putExtra("loginUser", loginUser);
				    		callDocumentView.putExtra("localData", localData);
				    		
				    		startActivity(callDocumentView);
				    		finish();
				    		
				    	}
				    }
				 else
				   {
				    MHISEUtil.displayDialog(SelectPurpose.this,
				    			getResources().getString(R.string.error_msg_purpose_not_selected),getResources().getString(R.string.error_msg_purpose_not_selected));
				   }		
				}			
				});
			}catch (NullPointerException e) {
				Logger.debug("SelectPurpose-->handleButtonEvent", "true"+e);		
		}	
	    }
	   
	
		private void setListenerForList(ListView lv)
		{
				lv.setOnItemClickListener(new OnItemClickListener() {

				@Override
				public void onItemClick(AdapterView<?> arg0, View rowView,
						int arg2, long arg3) {
					CheckedTextView ctv = (CheckedTextView)rowView;
					purpose =ctv.getText().toString();

				}
			});
		}

}
