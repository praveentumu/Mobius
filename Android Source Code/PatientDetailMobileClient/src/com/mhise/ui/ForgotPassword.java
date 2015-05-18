package com.mhise.ui;



import org.apache.http.impl.client.DefaultHttpClient;

import android.app.Activity;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnCancelListener;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.mhise.constants.Constants;
import com.mhise.model.Result;
import com.mhise.requests.RequestBase;
import com.mhise.response.GetMasterDataParser;
import com.mhise.util.MHISEUtil;
import com.mhise.xml.XmlConstants;

public class ForgotPassword extends Activity implements OnClickListener{

	private EditText edt_EmailID;	
	private Button btn_Submit;
	private Button btn_Cancel;
	private RadioGroup  rgUserType;
	private TextView TextView_PwdProblem;
	private ForgotPasswordAsync forgotPasswordAsync;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.forgotpassword);
		loadUI();
		handleEvents();
	
	}

	
	private void loadUI() {
		
		edt_EmailID = (EditText) findViewById(R.id.edtLoginEmail);
		btn_Submit= (Button) findViewById(R.id.btnSubmit);
		btn_Cancel = (Button) findViewById(R.id.btnCancel);	
		rgUserType=(RadioGroup) findViewById(R.id.radioUserType);
		TextView_PwdProblem=(TextView)findViewById(R.id.TextView_PwdProblem);
		
	}
	
	private void handleEvents() 
	{
		btn_Submit.setOnClickListener(this);
		btn_Cancel.setOnClickListener(this);
		
	}
	
	
	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub

		switch (v.getId()) {
			
			case R.id.btnSubmit: {
				
				String request =makeRequest();
				showDialog(Constants.FORGOTPWD_PROGRESS_DIALOG);
				forgotPasswordAsync = new ForgotPasswordAsync();
				forgotPasswordAsync.execute(request);
				break;
			}
			case R.id.btnCancel: {
				edt_EmailID.setText(null);
				TextView_PwdProblem.setText(null);
				TextView_PwdProblem.setVisibility(View.INVISIBLE);
				rgUserType.check(R.id.radioProvider);
				finish();
  	    	  break;
				
			}
			
	
		}
	}
	
	private String makeRequest()
	{
		int selectedId = rgUserType.getCheckedRadioButtonId();
		RadioButton  radioUserButton = (RadioButton) findViewById(selectedId);
		
		try{
		
			if (radioUserButton.getText().toString().equals(getResources().getString(R.string.Provider)))
				return	RequestBase.forgotPWDRequest(edt_EmailID.getText().toString(),"Provider");
			else if(radioUserButton.getText().toString().equals(getResources().getString(R.string.Patient)))
				return	RequestBase.forgotPWDRequest(edt_EmailID.getText().toString(),"Patient" );
			}
		catch (Exception e) {
			return null;
		
		}
		return null;
	}
	
	private class ForgotPasswordAsync extends AsyncTask<String, Void, String>
	 {
		 @Override
		protected String doInBackground(String... params) {
	
			String request = params[0];	
			DefaultHttpClient httpClient= new DefaultHttpClient();
			String response = com.mhise.util.MHISEUtil.CallWebService(
					Constants.URL, XmlConstants.ACTION_FORGOT_PASSWORD, request,httpClient);
			return response;
		}
		 
		 @Override
		protected void onPostExecute(String result) {
			// TODO Auto-generated method stub 
			 super.onPostExecute(result);	
			 
			 	removeDialog(Constants.FORGOTPWD_PROGRESS_DIALOG);
				org.w3c.dom.Document resultDoc = MHISEUtil.XMLfromString(result);
				if(resultDoc != null)
				{	
					
					Result  forgotPWDResult =	new GetMasterDataParser().parseForgotPWD(resultDoc);
					if(forgotPWDResult.IsSuccess.equals("true"))
					{
						
						Toast.makeText(getApplicationContext(),
								forgotPWDResult.ErrorMessage,
							     Toast.LENGTH_LONG).show();
						TextView_PwdProblem.setVisibility(View.INVISIBLE);	
						finish();
					}
					else
					{
					TextView_PwdProblem.setVisibility(View.VISIBLE);
						TextView_PwdProblem.setText(forgotPWDResult.ErrorMessage);
					}
				}
		 }
		}
	
	 @Override
		protected Dialog onCreateDialog(int id) {
		    	
		 Log.e("here in ","on create dialog");
		        switch (id) {

		            case   Constants.FORGOTPWD_PROGRESS_DIALOG:
		        	{
		        		 ProgressDialog dialog = new ProgressDialog(this);
		        		 dialog.setMessage("Processing password request..");          		 
		        		 dialog.setMax(Constants.PROGRESS_DIALOG_MAXTIME);
		        		// Logger.debug("RegisterProvider-->inside on create dialog", "true");
		        		 dialog.setCancelable(true);
		        		 dialog.setCanceledOnTouchOutside(false);
		        		 dialog.setOnCancelListener(new OnCancelListener() {
	  						
	  						@Override
	  						public void onCancel(DialogInterface dialog) {
	  							// TODO Auto-generated method stub
	  							dialog.dismiss();
	  							forgotPasswordAsync.cancel(true);
	  						
	  						}
	  					});	
				       		 
		        		 return dialog;
		        	}
		        }
				return null;
	 }
	 
	
}
