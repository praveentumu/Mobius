package com.mhise.ui;

import java.io.File;
import java.io.InputStream;
import java.io.StringReader;
import java.io.StringWriter;

import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;

import org.apache.http.impl.client.DefaultHttpClient;

import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnCancelListener;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.AssetManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import android.view.Menu;
import android.webkit.WebView;

import com.mhise.constants.Constants;
import com.mhise.constants.MobiusDroid;
import com.mhise.model.Assertion;
import com.mhise.model.DocumentResponse;
import com.mhise.model.User;
import com.mhise.requests.RequestBase;
import com.mhise.response.BaseParser;
import com.mhise.util.DataConnectionManager;
import com.mhise.util.Logger;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;


public class DocumentViewScreen extends BaseMenuOptionsAcitivity{

	private String patientID;
	private String documentID;	
	private String purpose;
	private String role;
	private String email;	
	private WebView wv;
	private User loginUser;
	private boolean localData;
	
	private GetDocumentAsync getDocumentObj;
		@Override
	    public void onCreate(Bundle savedInstanceState) {
	        super.onCreate(savedInstanceState);
	      
	        try {
	            long httpCacheSize = 10 * 1024 * 1024; // 10 MiB
	            File httpCacheDir = new File(getCacheDir(), "http");
	            Class.forName("android.net.http.HttpResponseCache")
	                .getMethod("install", File.class, long.class)
	                .invoke(null, httpCacheDir, httpCacheSize);
	        } catch (Exception httpResponseCacheNotAvailable) {
	        }
	        setContentView(R.layout.document_view);
	        wv =(WebView)findViewById(R.id.wvDocument); 
	        Log.e("here in patient view","Document");
	        
	        loginUser=(User)getIntent().getSerializableExtra("loginUser");
	        
	        Log.e("here in patient view 1","Document");
	        localData= getIntent().getBooleanExtra(("localData"),false);
	    	boolean isDataConnectionAvailable =DataConnectionManager.chkConnectionStatus(getApplicationContext());
			  if(!isDataConnectionAvailable)
	  		{
	  		MHISEUtil.displayDialog(DocumentViewScreen.this,getResources().getString(R.string.DataConnectionAvailabiltyMessage),getResources().getString(R.string.DataConnectionAvailabiltyTitle) );
	  		}
	  		else
	  		{	
	        
	        showDialog(Constants.GET_DOCUMENT_PROGRESS_DIALOG);
	        getDocumentObj = new GetDocumentAsync();
	        getDocumentObj.execute(makeRequest());
	  		}
	    }
	
	
		 @Override
		public boolean onCreateOptionsMenu(Menu menu) {
			// TODO Auto-generated method stub
			return super.onCreateOptionsMenu(menu);
		
		 }
		
		
		@Override
		public Dialog onCreateDialog(int id) {
			
			 switch (id) {	           
		 
	            case Constants.GET_DOCUMENT_PROGRESS_DIALOG:
	            		ProgressDialog dialog = new ProgressDialog(this);
	            		dialog.setMessage(getResources().getString(R.string.progressbar_msg_get_document));          		 
	            		dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
	            		//Logger.debug("DocumentViewScreen-->onCreateDialog-->Retrieving Document!", "true");
	            		dialog.setCancelable(true);
	            		 dialog.setCanceledOnTouchOutside(false);
	            	dialog.setOnCancelListener(new OnCancelListener() {
	    						
	    						@Override
	    						public void onCancel(DialogInterface dialog) {
	    							// TODO Auto-generated method stub
	    							dialog.dismiss();
	    							 getDocumentObj.cancel(true);
	    							 DocumentViewScreen.this.finish();
	    						}
	    					});
	            		
	            		
	            		return dialog;
			 	}
			return null; 
		}

		private class GetDocumentAsync extends AsyncTask<String, Void, String>
		 {				
			 @Override
			protected String doInBackground(String... params) {
		
				String request = params[0];
				SharedPreferences sharedPreferences = getSharedPreferences(Constants.PREFS_NAME,MODE_PRIVATE);  
			 	DefaultHttpClient httpClient = MHISEUtil.initializeHTTPClient(getApplicationContext(),MHISEUtil.loadKeyStore(sharedPreferences,getApplicationContext()));
				
				String response = com.mhise.util.MHISEUtil.CallWebService(
					
						Constants.HTTPS_URL,XmlConstants.ACTION_GETDOCUMENT, request,httpClient);
				return response;
			}
			 
			 @Override
			protected void onPostExecute(String result) {
				// TODO Auto-generated method stub 
				 super.onPostExecute(result);	
				 
				 	removeDialog(Constants.GET_DOCUMENT_PROGRESS_DIALOG);	
					org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
					if(resultDoc != null)
					{										
						DocumentResponse docResponse = BaseParser.parseDocument(resultDoc);
						if(docResponse.result.IsSuccess.equals("true"))
						{					
						byte[] docdata1 =	Base64.decode(docResponse.document.getDocumentBytes(), Base64.DEFAULT); 
								StringBuffer stb = new StringBuffer();
								{
								for(int i=0;i<docdata1.length;i++)
								{
								char  a =	(char)docdata1[i];
								stb.append(a);
								}
							}
						//Log.e("xml ",stb.toString());
						int index = stb.indexOf("<?xml");
						
						try {  
						  
							
							AssetManager assetManager = getAssets();
						    InputStream ims  = assetManager.open("WebViewLayout_CDA.xsl");
							StreamSource xsltSource = new StreamSource(ims); 
							StreamSource xml = new StreamSource(new StringReader(stb.substring(index).toString().trim()));
							/*if(loginUser.getUserType().equals("Provider")){
								xml = new StreamSource(new StringReader(stb.substring(3).toString().trim()));
							}else if(loginUser.getUserType().equals("Patient")){
								xml = new StreamSource(new StringReader(stb.toString().trim()));
							}*/
							//System.out.println("xslSource   "+xsltSource.toString());  
							StringWriter swResult = new StringWriter();  
							StreamResult resultDisplayDoc = new StreamResult(swResult);  
							            //System.out.println("result  "+resultDisplayDoc.toString());  
							   
							  
							   
							Transformer t = TransformerFactory.newInstance().newTransformer(xsltSource);  
							   
							t.transform(xml, resultDisplayDoc);  
							String html = swResult.toString();
							wv.loadData(html, "text/html", null);
							//System.out.println(html);  
						}catch(Exception e) {  
							e.printStackTrace();  
						}  
				        
				        
				        
						//	Logger.debug("DocumentViewScreen-->GetDocumentAsync-->onPostExecute-->appended string ", ""+stb);						
							//wv.loadDataWithBaseURL( "file:///android_asset/", stb.substring(3).toString(), "text/xml", "utf-8", null );	
						}
						else if(docResponse.result.IsSuccess.equals("false"))
						{
						Dialog dialog=	MHISEUtil.displayDialog(DocumentViewScreen.this, docResponse.result.ErrorMessage,docResponse.result.ErrorCode);	    
							dialog.setOnCancelListener(new OnCancelListener() {
								
								@Override
								public void onCancel(DialogInterface dialog) {
									// TODO Auto-generated method stub
									DocumentViewScreen.this.finish();
								}
							});
							
							 AlertDialog.Builder alertDialogBuilder=null;
								try{
								if ( !docResponse.result.ErrorMessage.equals("") ||!docResponse.result.ErrorMessage.equals(null))
								{
							
								 alertDialogBuilder = new AlertDialog.Builder(
						                DocumentViewScreen.this);
								 
								 alertDialogBuilder.setMessage(docResponse.result.ErrorMessage);
								 alertDialogBuilder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
									public void onClick(DialogInterface dialog, int which) {
										DocumentViewScreen.this.finish();       
									}
									});
								 alertDialogBuilder.setOnCancelListener(new OnCancelListener(
										 ) {
									
									@Override
									public void onCancel(DialogInterface dialog) {
										// TODO Auto-generated method stub
										DocumentViewScreen.this.finish();   
									}
								});
									// Showing Alert Message
								 alertDialogBuilder.show();								 
								}
								}catch (NullPointerException e) {						
									Logger.debug("MHISEUtil-->Unable to display dialog message", "display dialog");									
								}
						
							 

						}
						
					}
			 }
			}
	
		private  String makeRequest()
		{
			Intent callingIntent= getIntent();
		    patientID=  callingIntent.getStringExtra(Constants.KEY_USER_ID);		   
		    documentID=  callingIntent.getStringExtra(Constants.KEY_DOCUMENT_ID);  		
		    role=  callingIntent.getStringExtra(Constants.KEY_ROLE); 
		    purpose=  callingIntent.getStringExtra(Constants.KEY_PURPOSE);		    
		    email =   callingIntent.getStringExtra(Constants.KEY_USER_EMAIL);
		    Log.i("Key User Email", "-->"+email);
		    Assertion assertion = new Assertion();
		  	assertion.setAssertionMode(Constants.ASSERTION_MODE_DEFAULT);
		  	assertion.setPurposeOfUse(Constants.DEFAULT_PURPOSE);
		  	Log.e("login User in document View",""+loginUser);
		  	assertion.setUserInformation(loginUser);
		  	assertion.setNhinCommunity(MobiusDroid.homeCommunity);
		  	assertion.setHaveSignature(true);
			return RequestBase.getDocumentRequest(assertion,patientID,documentID,purpose,role,email,localData); 
		}
		
}

