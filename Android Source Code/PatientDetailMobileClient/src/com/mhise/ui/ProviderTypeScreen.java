package com.mhise.ui;


import com.mhise.constants.Constants;
import android.app.ListActivity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;

public class ProviderTypeScreen extends ListActivity{

	ArrayAdapter<String> adapter;
	String[] mainListItem = {"Organizational Provider","Individual Provider"};
		
	@Override
	public void onCreate(Bundle icicle)
	{	
	super.onCreate(icicle);
	adapter=new ArrayAdapter<String>(this, R.layout.mainlist_textview, mainListItem);
	setListAdapter(adapter);	
	}
	

	@Override
	protected void onListItemClick(ListView l, View v, int position, long id) {
	

		switch (position) {
		case Constants.INDIVIDUAL_PROVIDER:
			
			Intent providerRegistrationCall=
						new Intent(ProviderTypeScreen.this,RegisterProvider.class);
			providerRegistrationCall.putExtra(Constants.PROVIDER_TYPE,position);
			startActivity(providerRegistrationCall);
			break;

		case Constants.ORGANIZATIONAL_PROVIDER:
			Intent providerRegistrationCall1= 
						new Intent(ProviderTypeScreen.this,RegisterProvider.class);
			providerRegistrationCall1.putExtra(Constants.PROVIDER_TYPE,position);
			startActivity(providerRegistrationCall1);
			break;
		}

	}

	
}




