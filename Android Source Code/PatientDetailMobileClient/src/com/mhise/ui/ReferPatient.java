package com.mhise.ui;


import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import com.mhise.constants.Constants;
import com.mhise.util.DataConnectionManager;
import com.mhise.util.MHISEUtil;

public class ReferPatient extends BaseMenuOptionsAcitivity {

	WebView wv;
	private String docID =null;
	private String serialNo =null;

	@Override
	
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
			setContentView(R.layout.document_view);
		
		     /*wv =(WebView)findViewById(R.id.wvDocument); 
		     
		     WebSettings webSettings = wv.getSettings();

		        webSettings.setJavaScriptEnabled(true);

		        webSettings.setBuiltInZoomControls(true);

		        wv.requestFocusFromTouch();

		       // wv.setWebViewClient(new WebViewClient());
		     
		     
		    wv.setWebViewClient(new WebViewClient() {
			        @Override
			        public boolean shouldOverrideUrlLoading(WebView view, String url) {
			           return super.shouldOverrideUrlLoading(view, url);
			        }
			    });
			
		    wv.setWebChromeClient(new WebChromeClient());*/
	        //wv.setWebViewClient(yourWebClient);
	    	boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
			  if(!isDataConnectionAvailable)
	  		{
	  		MHISEUtil.displayDialog(ReferPatient.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
	  		}
	  		else
	  		{		
	  			SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE);
	  			serialNo=  sharedPreferences.getString(Constants.KEY_SERIAL_NUMBER, null);
	  			docID =getIntent().getStringExtra(Constants.KEY_DOCUMENT_ID);
	  			Log.i("request",Constants.REFER_URL+"?Serial="+serialNo+"&DocumentID="+docID);
	  			//wv.loadUrl(Constants.REFER_URL+"?Serial="+serialNo+"&DocumentID="+docID );
	  			Intent httpIntent = new Intent(Intent.ACTION_VIEW);
	  			httpIntent.setData(Uri.parse(Constants.REFER_URL+"?Serial="+serialNo+"&DocumentID="+docID));
	  			startActivity(httpIntent);
	    
	  		}
	}
	
	@Override
	public void onBackPressed() {
		super.onBackPressed();
		setContentView(R.layout.document_view);
	}

}
