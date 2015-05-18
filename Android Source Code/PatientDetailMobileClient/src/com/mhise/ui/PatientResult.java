package com.mhise.ui;

import java.util.ArrayList;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.TextView;

import com.mhise.constants.Constants;
import com.mhise.model.Patient;
import com.mhise.model.SearchPatientResult;
import com.mhise.model.User;
import com.mhise.util.MHISEUtil;

public class PatientResult extends BaseMenuOptionsAcitivity {

	SearchPatientResult searchpatientresult;
	EfficientAdapter adapter;

	private String userRole;
	private String email;
	private String userType;
	private static String strName;
	private ListView patientresultList;
	private User loginUser;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		Log.e("here in patient result", "here in patient result");
		super.onCreate(savedInstanceState);
		setContentView(R.layout.patientresultlist);

		try {

			Bundle bn = new Bundle();
			bn = getIntent().getExtras();
			userRole = getIntent().getStringExtra(Constants.KEY_ROLE);
			email = getIntent().getStringExtra(Constants.KEY_USER_EMAIL);
			userType = getIntent().getStringExtra(Constants.KEY_USER_TYPE);
			loginUser = (User) getIntent().getSerializableExtra("loginUser");
			Log.e("logged in user at patient result", "" + loginUser);
			// HashMap<String, Object> getobj = new HashMap<String, Object>();

			searchpatientresult = (SearchPatientResult) bn
					.getSerializable("bundleobj");
			Log.e("searchpatientresult", "" + searchpatientresult);
			// Logger.debug("PatientResult-->checking serializable obj",
			// ""+searchpatientresult.getPatient().get(0).demographics.getDOB());
		} catch (Exception e) {
			Log.e("Err", e.getMessage());
		}

		if (searchpatientresult != null) {
			int numberOfPatientFound = searchpatientresult.getPatient().size();
			ArrayList<Patient> patientlist = new ArrayList<Patient>();

			for (int p = 0; p < numberOfPatientFound; p++) {
				Patient patient = searchpatientresult.getPatient().get(p);
				patientlist.add(patient);
			}
			patientresultList = (ListView) findViewById(R.id.patientresultlist);

			patientresultList.setAdapter(new EfficientAdapter(this , patientlist));
			setListItemClickListener(patientresultList, patientlist);
		}

		/*
		 * patientresultList.setOnItemClickListener(new OnItemClickListener() {
		 * 
		 * @Override public void onItemClick(AdapterView<?> arg0, View arg1, int
		 * position, long arg3) { // TODO Auto-generated method stub Intent
		 * callGetDocumentDetails = new
		 * Intent(PatientResult.this,GetDocumentDetails.class); String
		 * strGivenName
		 * =patientlist.get(position).getDemographics().getGivenName(); String
		 * strFamilyName
		 * =patientlist.get(position).getDemographics().getFamilyName(); strName
		 * =
		 * strGivenName.substring(0,1).toUpperCase()+strGivenName.substring(1)+" "
		 * +
		 * strFamilyName.substring(0,1).toUpperCase()+strFamilyName.substring(1)
		 * ; callGetDocumentDetails.putExtra(Constants.KEY_NAME, strName);
		 * 
		 * callGetDocumentDetails.putExtra(Constants.KEY_USER_ID,
		 * searchpatientresult.getPatient().get(position).getPatientId());
		 * callGetDocumentDetails.putExtra(Constants.KEY_ROLE,userRole);
		 * callGetDocumentDetails.putExtra(Constants.KEY_USER_EMAIL,email);
		 * callGetDocumentDetails
		 * .putExtra(Constants.KEY_NAME,searchpatientresult
		 * .getPatient().get(position
		 * ).getDemographics().getGivenName()+" "+searchpatientresult
		 * .getPatient().get(position).getDemographics().getFamilyName());
		 * callGetDocumentDetails.putExtra(Constants.KEY_USER_TYPE,userType);
		 * callGetDocumentDetails.putExtra("loginUser", loginUser);
		 * startActivity(callGetDocumentDetails); }
		 * 
		 * });
		 */

	}

	private void setListItemClickListener(final ListView listView , final ArrayList<Patient> patientlist) {
		listView.setOnItemClickListener(new OnItemClickListener() {
			public void onItemClick(AdapterView<?> arg0, View view, int position, long arg3){
				// TODO Auto-generated method stub
				Intent callGetDocumentDetails = new Intent(PatientResult.this,
						GetDocumentDetails.class);
				String strGivenName = patientlist.get(position)
						.getDemographics().getGivenName();
				String strFamilyName = patientlist.get(position)
						.getDemographics().getFamilyName();
				strName = strGivenName.substring(0, 1).toUpperCase()
						+ strGivenName.substring(1) + " "
						+ strFamilyName.substring(0, 1).toUpperCase()
						+ strFamilyName.substring(1);
				callGetDocumentDetails.putExtra(Constants.KEY_NAME, strName);

				callGetDocumentDetails.putExtra(Constants.KEY_USER_ID,
						searchpatientresult.getPatient().get(position)
								.getPatientId());
				callGetDocumentDetails.putExtra(Constants.KEY_ROLE, userRole);
				callGetDocumentDetails
						.putExtra(Constants.KEY_USER_EMAIL, email);
				callGetDocumentDetails.putExtra(
						Constants.KEY_NAME,
						searchpatientresult.getPatient().get(position)
								.getDemographics().getGivenName()
								+ " "
								+ searchpatientresult.getPatient()
										.get(position).getDemographics()
										.getFamilyName());
				callGetDocumentDetails.putExtra(Constants.KEY_USER_TYPE,
						userType);
				callGetDocumentDetails.putExtra("loginUser", loginUser);
				//Log.e("here in setListItemClickListener", "uhgjhh");
				startActivity(callGetDocumentDetails);
			}
		});
	}

	/*
	 * @Override public boolean onCreateOptionsMenu(Menu menu) {
	 * 
	 * new MenuInflater(this).inflate(R.menu.export, menu); return
	 * super.onCreateOptionsMenu(menu);
	 * 
	 * }
	 */

	/*
	 * @Override public boolean onOptionsItemSelected(MenuItem item) {
	 * 
	 * SharedPreferences sharedPreferences =
	 * getSharedPreferences(Constants.PREFS_NAME,Context.MODE_PRIVATE); try{
	 * KeyStore localTrustStore = KeyStore.getInstance("PKCS12");
	 * FileInputStream fis =
	 * getApplicationContext().openFileInput(Constants.defaultP12StoreName);
	 * String strPassword =
	 * sharedPreferences.getString(Constants.KEY_PKCS12_PASSWORD, null); char[]
	 * password = strPassword.toCharArray(); localTrustStore.load(fis,password);
	 * 
	 * File fl = new File(Constants.defaultP12StorePath +
	 * Constants.defaultP12StoreName); OutputStream os = new
	 * FileOutputStream(fl); localTrustStore.store(os,password); os.close();
	 * fis.close();
	 * MHISEUtil.displayDialog(this,"Certificate has been successfully exported to "
	 * + "[External Storage \\MobiusDroid folder.]" ,
	 * "Certificate Export Successfull"); } catch ( KeyStoreException e) { //
	 * TODO: handle exception } catch ( NullPointerException e) { // TODO:
	 * handle exception } catch (FileNotFoundException e) { // TODO
	 * Auto-generated catch block e.printStackTrace(); } catch
	 * (NoSuchAlgorithmException e) { // TODO Auto-generated catch block
	 * e.printStackTrace(); } catch (CertificateException e) { // TODO
	 * Auto-generated catch block e.printStackTrace(); } catch (IOException e) {
	 * // TODO Auto-generated catch block e.printStackTrace(); }
	 * 
	 * 
	 * return super.onOptionsItemSelected(item); }
	 */

	private static class EfficientAdapter extends BaseAdapter {
		private LayoutInflater mInflater;
		private ArrayList<Patient> patientlist = new ArrayList<Patient>();

		public EfficientAdapter(Context context, ArrayList<Patient> patientList) {
			// Cache the LayoutInflate to avoid asking for a new one each time.
			mInflater = LayoutInflater.from(context);
			this.patientlist = patientList;
		}

		/**
		 * The number of items in the list is determined by the number of
		 * speeches in our array.
		 * 
		 * @see android.widget.ListAdapter#getCount()
		 */
		public int getCount() {
			return patientlist.size();
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

		public View getView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder;
			if (convertView == null) {
				convertView = mInflater.inflate(
						R.layout.patient_result_list_item, null);
				holder = new ViewHolder();
				holder.textFirstName = (TextView) convertView
						.findViewById(R.id.txtFirstName);
				holder.textDOB = (TextView) convertView
						.findViewById(R.id.textDOB);
				holder.textSex = (TextView) convertView
						.findViewById(R.id.textSex);
				holder.textAddress = (TextView) convertView
						.findViewById(R.id.textAddress);
				holder.textCommunity = (TextView) convertView
						.findViewById(R.id.textCommunity);
				convertView.setTag(holder);
			} else {
				holder = (ViewHolder) convertView.getTag();
			}

			String strGivenName = patientlist.get(position).getDemographics()
					.getGivenName();
			String strFamilyName = patientlist.get(position).getDemographics()
					.getFamilyName();
			strName = strGivenName.substring(0, 1).toUpperCase()
					+ strGivenName.substring(1) + " "
					+ strFamilyName.substring(0, 1).toUpperCase()
					+ strFamilyName.substring(1);
			// name.substring(0,1).toUpperCase() + name.substring(1)
			holder.textFirstName.setText(strName);
			holder.textDOB.setText(patientlist.get(position).getDemographics()
					.getDOB());
			holder.textSex.setText(patientlist.get(position).getDemographics()
					.getGender());
			com.mhise.model.Address[] address = patientlist.get(position)
					.getAddress();
			holder.textAddress.setText(MHISEUtil
					.makeAddressStringFromAddress(address));
			holder.textCommunity.setText(patientlist.get(position)
					.getDemographics().getCommunityDescription());
			return convertView;

		}

	}

	static class ViewHolder {
		TextView textFirstName;
		TextView textDOB;
		TextView textSex;
		TextView textAddress;
		TextView textCommunity;
	}

	/*
	 * @Override protected void onDestroy() {
	 * 
	 * super.onDestroy(); patientlist.clear(); }
	 */
	@Override
	public void onBackPressed() {
		super.onBackPressed();
		// patientlist.clear();
	}

	@Override
	protected void onStop() {
		// TODO Auto-generated method stub
		super.onStop();
		// patientlist.clear();
	}
}
