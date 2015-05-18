package com.mhise.security;

import java.io.IOException;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;
import java.security.PrivateKey;
import java.security.PublicKey;
import java.security.SecureRandom;
import java.security.SignatureException;
import java.security.cert.CertificateEncodingException;
import java.security.cert.X509Certificate;


import javax.security.auth.x500.X500Principal;

import org.bouncycastle.asn1.DEROctetString;
import org.bouncycastle.asn1.DERSet;
import org.bouncycastle.asn1.cms.Attribute;
import org.bouncycastle.asn1.pkcs.PKCSObjectIdentifiers;
import org.bouncycastle.asn1.x509.ExtendedKeyUsage;
import org.bouncycastle.asn1.x509.KeyPurposeId;
import org.bouncycastle.asn1.x509.X509Extension;
import org.bouncycastle.asn1.x509.X509Extensions;
import org.bouncycastle.asn1.x509.X509Name;
import org.bouncycastle.jce.PKCS10CertificationRequest;
import org.bouncycastle.util.encoders.Base64;
import org.bouncycastle.x509.X509V3CertificateGenerator;
import org.bouncycastle.x509.extension.AuthorityKeyIdentifierStructure;
import org.bouncycastle.x509.extension.SubjectKeyIdentifierStructure;

import com.mhise.util.Logger;

import android.util.Log;


/** 
*@(#)GenerateCSR.java 
* @author R Systems
* @description This class contains the methods to  generate PKCS10 certificate signing request
* 
* @since 2012-10-26
* @version 1.0 
*/
@SuppressWarnings({ "deprecation" })
public class GenerateCSR {
	public static PublicKey publicKey = null;
	public static PrivateKey privateKey = null;
	public static KeyPairGenerator keyGen = null;
	private static GenerateCSR gcsr = null;

	public GenerateCSR() {
		try {
			keyGen = KeyPairGenerator.getInstance("RSA");
			
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
		keyGen.initialize(2048, new SecureRandom());
		KeyPair keypair = keyGen.generateKeyPair();
		publicKey = keypair.getPublic();
		privateKey = keypair.getPrivate();
		
		
		
		Log.i("private key in generate csr ", ""+privateKey);
	}

	public static GenerateCSR getInstance() {
		if (gcsr == null)
			gcsr = new GenerateCSR();
		return gcsr;
	}

	/**
	 * 
	 * @param CN
	 *            Common Name, is X.509 speak for the name that distinguishes
	 *            the Certificate best, and ties it to your Organization
	 * @param OU
	 *            Organizational unit
	 * @param O
	 *            Organization NAME
	 * @param L
	 *            Location
	 * @param S
	 *            State
	 * @param C
	 *            Country
	 * @return
	 * @throws Exception
	 */
	public  String generatePKCS10(String E,String CN, String OU, String O,
			String L, String S, String C) throws Exception {

		String sigAlg = "SHA1withRSA";
		 X509Name dn = new X509Name("E="+E+", CN=" + CN + ", O=" + O
					+ ", OU=" + OU + ", L=" + L + ", ST=" + S + ", C=" + C);

         ExtendedKeyUsage clientAuthentication = new ExtendedKeyUsage(KeyPurposeId.id_kp_clientAuth);        
         //To make Client authentication specific
         Attribute attribute = new Attribute(
              PKCSObjectIdentifiers.pkcs_12,
              new DERSet(clientAuthentication));
		PKCS10CertificationRequest csr = new PKCS10CertificationRequest(sigAlg,
				dn, publicKey, new DERSet(attribute), privateKey);
		byte[] outBytes = csr.getEncoded();	
		return new String(Base64.encode(outBytes));

	}

	public static X509Certificate v3certificateGenerator(String E,String CN, String OU, String O,
                  String L, String S, String C){
            String sigAlg = "SHA1withRSA";
            X509Certificate cert=null;
            X509V3CertificateGenerator certGen = new X509V3CertificateGenerator();
            try {
                  X500Principal              subjectName = new X500Principal("E="+E+"CN=" + CN + ", O=" + O
                              + ", OU=" + OU + ", L=" + L + ", ST=" + S + ", C=" + C);
                  X509Certificate caCert =X509Certificate.class.newInstance();
                  certGen.setIssuerDN(caCert.getSubjectX500Principal());
                  certGen.setSubjectDN(subjectName);
                  certGen.setPublicKey(publicKey);
                  certGen.setSignatureAlgorithm(sigAlg);

                  ExtendedKeyUsage extendedKeyUsage = new ExtendedKeyUsage(KeyPurposeId.id_kp_clientAuth);

                  X509Extension extension = new X509Extension(false, new DEROctetString(extendedKeyUsage));
                  
                  
                  certGen.addExtension(X509Extensions.AuthorityKeyIdentifier, false,
                                          new AuthorityKeyIdentifierStructure(extension));
                  certGen.addExtension(X509Extensions.SubjectKeyIdentifier, false,
                                          new SubjectKeyIdentifierStructure(publicKey));
                 cert = certGen.generate(privateKey, "BC");
                  return cert;
                  
            } catch (CertificateEncodingException e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            } catch (IllegalArgumentException e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            } catch (IllegalStateException e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            } catch (NoSuchProviderException e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            } catch (NoSuchAlgorithmException e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            } catch (SignatureException e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            } catch (InstantiationException e) {
                  // TODO Auto-generated catch block
                  e.printStackTrace();
            } catch (IllegalAccessException e) {
            	Logger.debug("v3certificateGenerator", ""+e);
            	// TODO Auto-generated catch block
                  e.printStackTrace();
            } catch (IOException e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            }
            catch (Exception e) {
                  // TODO Auto-generated catch block
            	Logger.debug("v3certificateGenerator", ""+e);
                  e.printStackTrace();
            }
            return null;
      }

	
}
