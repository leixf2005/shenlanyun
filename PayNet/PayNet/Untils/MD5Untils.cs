using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class MD5Untils
    {
        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static String GetMd5(string message)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] byteData = System.Text.Encoding.UTF8.GetBytes(message);
                byteData = md5.ComputeHash(byteData);
                string OutString = "";
                for (int i = 0; i < byteData.Length; i++)
                {
                    OutString += byteData[i].ToString("x2");
                }
                OutString = OutString.Replace("-", "");
                return OutString;
            }
            catch
            {
                return "";
            }
        }

    }
}