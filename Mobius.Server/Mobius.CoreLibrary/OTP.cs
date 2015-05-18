using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.CoreLibrary
{
    public static class OTP
    {
        /// <summary>
        /// Generate of OTP is base 36 encoded string of random number (generated between 100 and1000) + date time (yyyymmddhhmmss).
        /// </summary>
        /// <returns></returns>
        public static string GenerateOTP()
        {
            string OTP = string.Empty;
            var random = new Random(100);
            int randomNumber = random.Next(10000);
            string dateTimeValue = DateTime.Now.ToString("yyyyMMddhhmmss");

            OTP = randomNumber.ToString() + dateTimeValue;
            OTP = StringToBase36(OTP);
            char[] arr = OTP.ToCharArray();
            Array.Reverse(arr);
            OTP = new string(arr);
            return OTP.Substring(0, 5);

        }

        /// <summary>
        ///  Generate a base 36 encoded string 
        /// </summary>
        /// <param name="randomNumber"></param>
        /// <returns></returns>
        private static string StringToBase36(string randomNumber)
        {
            System.Func<char, int> v = c => (int)((c <= '9') ? (c - '0') : (c - 'A' + 10));
            System.Func<int, char> ch = d => (char)(d + ((d < 10) ? '0' : ('A' - 10)));
            randomNumber = randomNumber.ToUpper();
            var sb = new System.Text.StringBuilder(randomNumber.Length);
            sb.Length = randomNumber.Length;
            int carry = 1;
            for (int i = randomNumber.Length - 1; i >= 0; i--)
            {
                int x = v(randomNumber[i]) + carry;
                carry = x / 36;
                sb[i] = ch(x % 36);
            }
            if (carry > 0)
                return ch(carry) + sb.ToString();
            else return sb.ToString();
        }
    }
}

/// <summary>
/// 
/// </summary>
/// <returns></returns>
