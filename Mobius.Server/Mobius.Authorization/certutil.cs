using System;
using System.ComponentModel; 
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
namespace My.IdentityModel 
{     
    /// <summary>
    /// /// X509Certificate utilities
    /// </summary>      
    public  class CertUtil
    {
        #region constants
        private const string CERT_CRL_EXTENSION = "2.5.29.31";  
        private const string CRL_CRL_EXTENSION = "2.5.29.46";
        #endregion

        /// <summary>
        /// IsCertificateInCrl method to check certificates in revcation list
        /// </summary>
        /// <param name="cert">This method would take requestor's X509Certificate as input parametr</param>
        /// <returns>Returns boolean value whether certificate is revoked or not</returns>
        public  bool IsCertificateInCrl(X509Certificate2 cert)
        {
            #region variables
            string certCrlUrl = string.Empty;
            
            #endregion
            try         
            {              
                certCrlUrl = GetBaseCrlUrl(cert);
                string[] arrCertificateURL = certCrlUrl.Split(new char[] { '\r', '\n' });
                
                if (arrCertificateURL.Length > 1)
                {
                    return IsCertificateInCrl(cert, arrCertificateURL[0]);
                }
                else
                {
                    return false;   
                }
            }             
            catch   
            {           
                return false;   
            }                     
        }        
        public  bool IsCertificateInCrl(X509Certificate2 cert, string url)  
        {            
            WebClient wc = new WebClient();   
            byte[] rgRawCrl = wc.DownloadData(url);     
            IntPtr phCertStore = IntPtr.Zero;      
            IntPtr pvContext = IntPtr.Zero;         
            GCHandle hCrlData = new GCHandle();        
            GCHandle hCryptBlob = new GCHandle();  
            try           
            {              
                hCrlData = GCHandle.Alloc(rgRawCrl, GCHandleType.Pinned);     
                WinCrypt32.CRYPTOAPI_BLOB stCryptBlob;
                stCryptBlob.cbData = rgRawCrl.Length;    
                stCryptBlob.pbData = hCrlData.AddrOfPinnedObject();   
                hCryptBlob = GCHandle.Alloc(stCryptBlob, GCHandleType.Pinned);  
                if (!WinCrypt32.CryptQueryObject(WinCrypt32.CERT_QUERY_OBJECT_BLOB, hCryptBlob.AddrOfPinnedObject(),WinCrypt32.CERT_QUERY_CONTENT_FLAG_CRL,WinCrypt32.CERT_QUERY_FORMAT_FLAG_BINARY,0,IntPtr.Zero,IntPtr.Zero,                 IntPtr.Zero,                 ref phCertStore,IntPtr.Zero, ref pvContext))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }                   
                WinCrypt32.CRL_CONTEXT stCrlContext = (WinCrypt32.CRL_CONTEXT)Marshal.PtrToStructure(pvContext, typeof(WinCrypt32.CRL_CONTEXT));
                WinCrypt32.CRL_INFO stCrlInfo = (WinCrypt32.CRL_INFO)Marshal.PtrToStructure(stCrlContext.pCrlInfo, typeof(WinCrypt32.CRL_INFO)); 
                if (IsCertificateInCrl(cert, stCrlInfo))
                {             
                    return true; 
                }          
                else    
                {         
                    url = GetDeltaCrlUrl(stCrlInfo);  
                    if (!string.IsNullOrEmpty(url))   
                    {
                        url = url.TrimEnd(new char[] { '\r', '\n' }); 
                        return IsCertificateInCrl(cert, url);   
                    }               
                }             
            }            
            finally   
            {           
                if (hCrlData.IsAllocated) hCrlData.Free();  
                if (hCryptBlob.IsAllocated) hCryptBlob.Free();   
                if (!pvContext.Equals(IntPtr.Zero))  
                {                   
                    WinCrypt32.CertFreeCRLContext(pvContext);  
                }          
            }             
            return false;   
        }          
        private  bool IsCertificateInCrl(X509Certificate2 cert, WinCrypt32.CRL_INFO stCrlInfo)
        {            
            IntPtr rgCrlEntry = stCrlInfo.rgCRLEntry;
            for (int i = 0; i < stCrlInfo.cCRLEntry; i++)   
            {               
                string serial = string.Empty;  
                WinCrypt32.CRL_ENTRY stCrlEntry = (WinCrypt32.CRL_ENTRY)Marshal.PtrToStructure(rgCrlEntry, typeof(WinCrypt32.CRL_ENTRY)); 
                IntPtr pByte = stCrlEntry.SerialNumber.pbData;
                for (int j = 0; j < stCrlEntry.SerialNumber.cbData; j++)     
                {                 
                    Byte bByte = Marshal.ReadByte(pByte);  
                    serial = bByte.ToString("X").PadLeft(2, '0') + serial;  
                    pByte = (IntPtr)((Int32)pByte + Marshal.SizeOf(typeof(Byte)));   
                }                
                if (cert.SerialNumber == serial) 
                {                  
                    return true;       
                }                 
                rgCrlEntry = (IntPtr)((Int32)rgCrlEntry + Marshal.SizeOf(typeof(WinCrypt32.CRL_ENTRY)));  
            }          
            return false;    
        }          
        private  string GetBaseCrlUrl(X509Certificate2 cert)  
        {             
            try         
            {              
                return (from X509Extension extension in cert.Extensions  where extension.Oid.Value.Equals(CERT_CRL_EXTENSION)  select GetCrlUrlFromExtension(extension)).Single();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// GetDeltaCrlUrl
        /// </summary>
        /// <param name="stCrlInfo">Takes stCrlInfo as input parameter</param>
        /// <returns>Returns URL</returns>
        private  string GetDeltaCrlUrl(WinCrypt32.CRL_INFO stCrlInfo)
        {
            IntPtr rgExtension = stCrlInfo.rgExtension;
            X509Extension deltaCrlExtension = null;
            for (int i = 0; i < stCrlInfo.cExtension; i++)
            {
                WinCrypt32.CERT_EXTENSION stCrlExt = (WinCrypt32.CERT_EXTENSION)Marshal.PtrToStructure(rgExtension, typeof(WinCrypt32.CERT_EXTENSION)); 
                if (stCrlExt.Value.pbData != IntPtr.Zero && stCrlExt.pszObjId == CRL_CRL_EXTENSION)     
                {              
                    byte[] rawData = new byte[stCrlExt.Value.cbData];  
                    Marshal.Copy(stCrlExt.Value.pbData, rawData, 0, rawData.Length);   
                    deltaCrlExtension = new X509Extension(stCrlExt.pszObjId, rawData, stCrlExt.fCritical);   
                    break;               
                } 
                rgExtension = (IntPtr)((Int32)rgExtension + Marshal.SizeOf(typeof(WinCrypt32.CERT_EXTENSION))); 
            }             
            if (deltaCrlExtension == null) 
            {             
                return null;    
            }            
            return GetCrlUrlFromExtension(deltaCrlExtension);
        }

        /// <summary>
        /// GetCrlUrlFromExtension
        /// </summary>
        /// <param name="extension">Takes extension as input parameter</param>
        /// <returns></returns> 
        private  string GetCrlUrlFromExtension(X509Extension extension)
        {
            #region variables
            StringBuilder url = new StringBuilder();
            string val = "URL";
            int urlIndex = 0;
            #endregion
            try       
            {    
                string raw=new AsnEncodedData(extension.Oid, extension.RawData).Format(true);   
                if (raw.IndexOf(val) > -1)
                {                 
                    urlIndex = raw.IndexOf(val);
                    for (int i = urlIndex+4; i <= raw.Length - 1; i++)
                    {
                        url.Append(raw[i]);
                    }
                }
                return url.ToString();
            }         
            catch     
            {            
                return null;  
            }       
        }   
    }
} 


