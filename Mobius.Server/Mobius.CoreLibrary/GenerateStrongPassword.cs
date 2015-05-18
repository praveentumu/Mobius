using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.CoreLibrary
{
    public class GenerateStrongPassword
    {

        //create constant strings for each type of characters
        static string alphaCaps = "QWERTYUABCDEFGHIJKIOPASDFGHJKLZXCVBNM";
        static string alphaLow = "qwertyuabcdefghijkiopasdfghjklzxcvbnm";
        static string numerics = "1234567890";
        static string special = "@#$!";
        //create another string which is a concatenation of all above
        string allChars = alphaCaps + alphaLow + numerics + special;
        Random r = new Random();

        /// <summary>
        /// This method will accept the range greater than 4 and generate strong password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GeneratePassword(int length)
        {
            string password = "";

            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");

            // Generate four repeating random numbers are positions of
            // lower, upper, numeric and special characters
            // By filling these positions with corresponding characters,
            // we can ensure the password has at least one
            // character of those types
            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            pLower = getRandomPosition(ref posArray);
            pUpper = getRandomPosition(ref posArray);
            pNumber = getRandomPosition(ref posArray);
            pSpecial = getRandomPosition(ref posArray);


            for (int i = 0; i < length; i++)
            {
                if (i == pLower)
                    password += getRandomChar(alphaCaps);
                else if (i == pUpper)
                    password += getRandomChar(alphaLow);
                else if (i == pNumber)
                    password += getRandomChar(numerics);
                else if (i == pSpecial)
                    password += getRandomChar(special);
                else
                    password += getRandomChar(allChars);
            }
            return password;
        }

        private string getRandomChar(string fullString)
        {
            return fullString.ToCharArray()[(int)Math.Floor(r.NextDouble() * fullString.Length)].ToString();
        }

        private int getRandomPosition(ref string posArray)
        {
            int pos;
            string randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble()
                                           * posArray.Length)].ToString();
            pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }
    }

}
