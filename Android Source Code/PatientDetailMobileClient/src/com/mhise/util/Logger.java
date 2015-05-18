

package com.mhise.util;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.OutputStream;
import java.util.Vector;

import com.mhise.constants.Constants;

import android.os.Environment;
import android.util.Log;

/** @(#)Logger.java 
 * @author R Systems
 * @description this class helps in finding the logs throughout the project debugging
 * @since 2012-11-21
 * @version 1.0
 */

@SuppressWarnings("unchecked")
public class Logger implements Constants {

	public interface Level {
		byte FATAL = 0;
		byte ERROR = 1;
		byte WARN = 2;
		byte INFO = 3;
		byte DEBUG = 4;
		byte VERBOSE = 5;
	}

	private static final byte LEVEL = Level.DEBUG;
	private static final boolean consoleLoggingEnable = false;
	private static final boolean deviceLoggingEnable = false;
	private static final int logSize = 1000;

	private static Vector<com.mhise.util.Log> logVector;

	static {
		String state = Environment.getExternalStorageState();
	    if (Environment.MEDIA_MOUNTED.equals(state))
	    {
	    	File path = android.os.Environment.getExternalStorageDirectory ();
	    	File file = new File(path, Constants.LOG_FILE_NAME);
	    	try {
	            // Make sure the Pictures directory exists.
	            path.mkdirs();
	            
	            InputStream fis = new FileInputStream(file);
				ObjectInputStream in = new ObjectInputStream(fis);
				logVector = new Vector<com.mhise.util.Log>();
				logVector = (Vector<com.mhise.util.Log>) in.readObject();
				in.close();
				fis.close();
	    	}catch (Exception e) {
				// TODO: handle exception
			}	    	
	    }
	    	
		if (logVector == null) {
			logVector = new Vector<com.mhise.util.Log>();
			commit();
		}
	}

	private Logger() {
	}

	public static Vector<com.mhise.util.Log> getLogs() {
		return logVector;
	}

	public static void resetLogs() {
		logVector.removeAllElements();
	}

	private static boolean save(com.mhise.util.Log log) {
		logVector.addElement(log);
		if (logVector.size() > logSize) {
			logVector.removeElementAt(0);
		}
		
		commit();
		return true;
	}
	
	private static boolean commit()
	{
		String state = Environment.getExternalStorageState();
	    if (Environment.MEDIA_MOUNTED.equals(state)) {
	    	File path = android.os.Environment.getExternalStorageDirectory ();
	    	File file = new File(path, Constants.LOG_FILE_NAME);
	    	try {
	            path.mkdirs();
	            OutputStream os = new FileOutputStream(file);
	            ObjectOutputStream out = new ObjectOutputStream(os);
	            out.writeObject(logVector);
	            out.close();
	            out.flush();
	            os.close();

	    	}catch (Exception e) {
			}
	    }
		
		return true;
	}

	private static void log(byte logLevel, String tag, String msg) {
		
		 if(logLevel > LEVEL ) return;
		  
		 com.mhise.util.Log log = new com.mhise.util.Log(logLevel,msg); 
		 if(deviceLoggingEnable) 
		 { 
			 save(log);
		 }
		  
		 if(consoleLoggingEnable) 
		 { 
			 switch(logLevel)
			 {
			 	case Level.VERBOSE:
			 		Log.v(tag, msg);
			 		break;
			 		
			 	case Level.DEBUG:
			 		Log.d(tag, msg);
			 		break;
			 		
			 	case Level.INFO:
			 		Log.i(tag, msg);
			 		break;
			 		
			 	case Level.WARN:
			 		Log.w(tag, msg);
			 		break;
			 		
			 	case Level.ERROR:
			 		Log.e(tag, msg);
			 		break;
			 		
			 	case Level.FATAL:
			 		Log.e(tag, msg);
			 		break;			 		
			 }
		 }
	}

	public static void debug(String tag, String msg) {
		com.mhise.util.Logger.log(Level.DEBUG, tag, msg);
	}

	public static void info(String tag, String msg) {
		com.mhise.util.Logger.log(Level.INFO, tag, msg);
	}

	public static void warn(String tag, String msg) {
		com.mhise.util.Logger.log(Level.WARN, tag, msg);
	}

	public static void error(String tag, String msg) {
		com.mhise.util.Logger.log(Level.ERROR, tag, msg);
	}

	public static void fatal(String tag, String msg) {
		com.mhise.util.Logger.log(Level.FATAL, tag, msg);
	}
}
