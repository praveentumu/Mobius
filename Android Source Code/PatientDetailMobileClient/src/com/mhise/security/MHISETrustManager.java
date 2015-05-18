package com.mhise.security;

import java.security.KeyManagementException;

import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;

import javax.net.ssl.HostnameVerifier;
import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSession;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

/** 
*@(#)MHISETrustManager.java 
* @author R Systems
* @description This class contains the methods to authenticate server
* 
* @since 2012-10-26
* @version 1.0 
*/

public class MHISETrustManager implements X509TrustManager {

   
        private static TrustManager[] trustManagers;
        
        public static void allowAllSSL() {
        	
            HttpsURLConnection.setDefaultHostnameVerifier(new HostnameVerifier() {
            public boolean verify(String hostname, SSLSession session) {
            return true;
              }
            });
            
            SSLContext context = null;
            
            if (trustManagers == null) {
               trustManagers = new TrustManager[] { new MHISETrustManager() };
            }
            
            try {
                            context = SSLContext.getInstance("TLS");
                            context.init(null, trustManagers, new SecureRandom());
            		} catch (NoSuchAlgorithmException e) {
                            e.printStackTrace();
            		} catch (KeyManagementException e) {
                            e.printStackTrace();
            	}
            
            HttpsURLConnection.setDefaultSSLSocketFactory(context.getSocketFactory());
                    }

        @Override
        public void checkClientTrusted(X509Certificate[] chain, String authType)
                                        throws CertificateException {
                        // TODO Auto-generated method stub
                        
        }

        @Override
        public void checkServerTrusted(X509Certificate[] chain, String authType)
                                        throws CertificateException {
                        // TODO Auto-generated method stub
                        
        }

        @Override
        public X509Certificate[] getAcceptedIssuers() {
                        // TODO Auto-generated method stub
                        return null;
        }

                                
                                

                                

}
