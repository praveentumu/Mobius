using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using System.Data;
using System.ComponentModel;

/// <summary>
/// Summary description for Utility
/// </summary>
public static class Utility
{
    

    /// <summary>
    /// This function encrypt the data with TripleDES Crypto provider. 
    /// </summary>
    /// <param name="toEncrypt">Encrypt able string</param>
    /// <returns>Encrypted string</returns>
    public static string Encrypt(string toEncrypt)
    {
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        try
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            keyArray = UTF8Encoding.UTF8.GetBytes(key);

            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;

            //ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;

            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();

            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        finally
        {
            //Release resources held by TripleDes Encrypt or
            tdes.Clear();
        }
    }

    /// <summary>
    /// This function Decrypts the encrypted string with TripleDES Crypto provider.
    /// </summary>
    /// <param name="cipherString">Encrypted String</param>
    /// <returns>Decrypted String</returns>
    public static string Decrypt(string cipherString)
    {
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        try
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            keyArray = UTF8Encoding.UTF8.GetBytes(key);

            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encrypt or                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        finally
        {
            tdes.Clear();
        }
    }


    /// <summary>
    /// Extension method to Convert Generic List To DataTable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static DataTable ToDataTable<T>(this IList<T> list)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }
        object[] values = new object[props.Count];
        foreach (T item in list)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = props[i].GetValue(item) ?? DBNull.Value;
            table.Rows.Add(values);
        }
        return table;
    }
}
