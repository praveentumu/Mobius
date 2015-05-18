package com.mhise.util;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.net.ConnectivityManager;

/** 
*@(#)DataConnectionManager.java 
* @author R Systems
* @description This class contains the methods for Data Connection check
* @since 2012-10-26
* @version 1.0 
*/

public class DataConnectionManager {
	
	static Context ctx;
	
	public static boolean chkConnectionStatus(Context c)
	  {
		ctx = c;
		ConnectivityManager connMgr = (ConnectivityManager)
		c.getSystemService(Context.CONNECTIVITY_SERVICE);
		
		final android.net.NetworkInfo wifi =
					connMgr.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
		final android.net.NetworkInfo mobile_network =
					connMgr.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);
			if( wifi.isAvailable() ){	
				return true;
			}
			else if(mobile_network.isAvailable())
			{
				return true;
			}
			else 
			{
				return false ;
			}
	  }
	
	public static void displayAlert()
	{
		
	new AlertDialog.Builder(ctx).setMessage("Please Check Your Internet Connection and Try Again") 
	.setTitle("Network Error") 
	.setCancelable(true) 
	.setNeutralButton(android.R.string.ok, 
		new DialogInterface.OnClickListener() { 
		public void onClick(DialogInterface dialog, int whichButton){
			dialog.cancel();
			} 
		}) 
		.show(); 
	}
	
	
}
