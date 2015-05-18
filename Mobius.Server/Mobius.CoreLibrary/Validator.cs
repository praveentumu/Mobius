using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mobius.CoreLibrary
{
    public static  class Validator
    {  
        /// <summary>
        /// Method to validate email address(s)
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string inputValue, out string message)
        {
           // string regExemail = @"^[a-z_0-9][a-z_0-9\-\.\']+@[a-z_0-9-\.]+\.[a-z]{2,4}$";
            string regExemail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            message = "";
            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                Match matchInput = System.Text.RegularExpressions.Regex.Match(inputValue, regExemail, RegexOptions.IgnoreCase);
                if (matchInput.Success)
                {                    
                    return true;
                }
                else
                {
                    message = "Please enter a valid email address.";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method to validate name
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ValidateName(string inputValue, out string message)
        {
            //string validString = @"^([a-zA-Z0-9\s@_.']{1,255})$";
            string validString = @"^([a-zA-Z0-9\s.']{1,255})$";
            message = "";
            Match matchInput = System.Text.RegularExpressions.Regex.Match(inputValue, validString);
            if (matchInput.Success)
            {                
                return true;                
            }
            else
            {
                // Following characters are not valid <>?#*/\\|:&'"
                message = "Invalid characters found in name.";
                return false;
            }            
        }

        /// <summary>
        /// Method to validate Password
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string inputValue, out string message)
        {
            string validString = @"(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";
            message = "";
            Match matchInput = System.Text.RegularExpressions.Regex.Match(inputValue, validString);
            if (matchInput.Success)
            {
                return true;
            }
            else
            {
                message = "Please provide a strong password.";
                return false;
            }
        }

        /// <summary>
        ///  Method to validate phone
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ValidatePhone(string inputValue, out string message)
        {   
            bool result;
            message = "";
            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                //Regex objAlphaPattern = new Regex(@"^[0-9]\d{2}(-\d{3})?-\d{4}$");
                //http://msdn.microsoft.com/en-us/library/ff650303.aspx
                //Regex objAlphaPattern = new Regex(@"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$");
                Regex objAlphaPattern = new Regex(@"^?[\+]?[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$");
                result = objAlphaPattern.IsMatch(inputValue);
                if (result)
                {
                    return true;
                }
                else
                {
                    //message = "Please provide phone number in the following format: ###-#### or ###-###-####";
                    message = "Please provide phone number in the following format: ###-###-#### or ########## or #-###-###-#### or (###) ###-####";

                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method to validate SSN
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ValidateSSN(string inputValue, out string message)
        {
            bool result;
            message = "";
            if(!string.IsNullOrWhiteSpace(inputValue))
            {
                Regex objPattern=new Regex(@"^\d{3}\-?\d{2}\-?\d{4}$");
                result = objPattern.IsMatch(inputValue);
                if (result)
                {
                    return true;
                }
                else
                {
                    message = "Please provide SSN in the following format: ###-##-#### or #########";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ValidateURL(string inputValue, out string message)
        {   
            bool result;
            message = "";
            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                Regex objAlphaPattern = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");
                result = objAlphaPattern.IsMatch(inputValue);
                if (result)
                {                 
                    return true;
                }
                else
                {
                    message = "Please provide valid URL";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method to validate Postal Code
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ValidatePostalCode(string inputValue, out string message)
        {   
            bool result;
            message = "";
            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                Regex objAlphaPattern = new Regex(@"^\d{5}(-\d{4})?$");
                result = objAlphaPattern.IsMatch(inputValue);
                if (result)
                {            
                    return true;
                }
                else
                {
                    message = "Please provide valid Postal Code";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///  Method to validate address
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="validData"></param>
        /// <returns></returns>
        public static string ValidateAddress(string inputValue, out string validData)
        {
            validData = "";
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < inputValue.Length; i++) 
            {                
                if ((inputValue[i] >= '0' && inputValue[i] <= '9') || (inputValue[i] >= 'A' && inputValue[i] <= 'z' || (inputValue[i] == '.' || inputValue[i] == '/'|| inputValue[i] == ',' || inputValue[i] == '-' || Convert.ToString(inputValue[i]).Contains(" "))))
                    sb.Append(inputValue[i]); 
            } 
            
           validData= sb.ToString();
           validData= validData.Replace("^", "");
           return validData =validData.Replace("`", "");
        
        }

        /// <summary>
        /// Method to validate address
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static bool ValidateAddress(string inputValue)
        {            
            bool valid = false;           

            for (int i = 0; i < inputValue.Length; i++)
            {
                if ((inputValue[i] >= '0' && inputValue[i] <= '9') || (inputValue[i] >= 'A' && inputValue[i] <= 'z' || (inputValue[i] == '.' || inputValue[i] == '/' || inputValue[i] == ',' || inputValue[i] == '-' || Convert.ToString(inputValue[i]).Contains(" "))))
                {
                    valid = true;
                }
                else
                {
                    return valid = false;
                }
                    
            }
            return valid;

        }

        /// <summary>
        /// Method to validate numeric
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static bool ValidateNumeric(string inputValue)
        {
            bool result;
            if(!string.IsNullOrWhiteSpace(inputValue))
            {
                //Regex objAlphaPattern = new Regex(@"^(0|[1-9][0-9]+)$");
                Regex objAlphaPattern = new Regex(@"^([0-9]+)$");
                result = objAlphaPattern.IsMatch(inputValue);
                if (inputValue.Length > 6)
                {
                    return false;
                }
                if (result)
                {            
                    return true;
                }
                else
                {
                    return false;
                }
            }
             return true;
        }
        
    }
}
