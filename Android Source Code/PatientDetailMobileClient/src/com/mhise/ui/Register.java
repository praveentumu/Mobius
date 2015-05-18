/*package com.mhise.ui;

import android.app.ListActivity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;


public class MainList extends ListActivity {

	//DEFINING STRING ADAPTER WHICH WILL HANDLE DATA OF LISTVIEW
	ArrayAdapter<String> adapter;
	String[] mainListItem = {"Search Patient","Register Patient","Register Provider"};
	@Override
	public void onCreate(Bundle icicle) {

	super.onCreate(icicle);
    
	 //  adapter = new ArrayAdapter<String> (this,R.layout.mylist,HistoryList); 
	 
	adapter=new ArrayAdapter<String>(this, R.layout.mainlist_textview, mainListItem);
	setListAdapter(adapter);
    
	}

	@Override
	protected void onListItemClick(ListView l, View v, int position, long id) {
		String item = (String) getListAdapter().getItem(position);
		//Toast.makeText(this, item + " selected", Toast.LENGTH_LONG).show();
		
		if(item.equals(mainListItem[0]))
		{
		Intent searchPatientCall=	new Intent(this,SearchPatient.class);
		startActivity(searchPatientCall);
		}
	}
	

	   
}*/


package com.mhise.ui;

import android.app.ListActivity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.mhise.constants.Constants;



public class Register extends ListActivity {

	//DEFINING STRING ADAPTER WHICH WILL HANDLE DATA OF LISTVIEW
	ArrayAdapter<String> adapter;
	String[] mainListItem = {  "Register Patient",
			"Register Provider"};
	@Override
	public void onCreate(Bundle icicle) {

	super.onCreate(icicle);
    
	 //  adapter = new ArrayAdapter<String> (this,R.layout.mylist,HistoryList); 
	 
	adapter=new ArrayAdapter<String>(this,R.layout.mainlist_textview, mainListItem);
	setListAdapter(adapter);
	
	}


	@Override
	protected void onListItemClick(ListView l, View v, int position, long id) {

		switch (position) {
			case Constants.REGISTER_PATIENT:
				Intent registerPatient = new Intent(this, RegisterPatient.class);
				startActivity(registerPatient);
				break;
			case Constants.REGISTER_PROVIDER:
				Intent registerProvider = new Intent(this, ProviderTypeScreen.class);
				startActivity(registerProvider);
				break;
		}
		
	}

}