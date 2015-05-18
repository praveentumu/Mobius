

package com.mhise.util;

import java.io.Serializable;

/** @(#)Log.java 1.0
 * @author R Systems
 * @description this class is used to log the debug statements.
 * @since  2012-11-07
 * @version 1.0
 */
class Log implements Serializable {
	private static final long serialVersionUID = 7526472295622776148L;
	@SuppressWarnings("unused")
	private long timeStamp;
	private String msg;
	private byte level;

	public Log() {
	}

	public Log(byte level, String msg) {
		this.timeStamp = System.currentTimeMillis();
		this.msg = msg;
		this.level = level;
	}

	public String toString() {
		StringBuffer buffer = new StringBuffer();
		buffer.append("[");
		if (this.level == Logger.Level.FATAL) {
			buffer.append("FATEL] :");
		} else if (this.level == Logger.Level.ERROR) {
			buffer.append("ERROR] :");
		} else if (this.level == Logger.Level.WARN) {
			buffer.append("WARN] :");
		} else if (this.level == Logger.Level.INFO) {
			buffer.append("INFO] :");
		} else {
			buffer.append("DEBUG] :");
		}

		buffer.append(" " + msg + " :");
		buffer.append(" Thread: " + Thread.currentThread().getName());
		// buffer.append(" Date: "+timeStamp);
		return buffer.toString();
	}
}
