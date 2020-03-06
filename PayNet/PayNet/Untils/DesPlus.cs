using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MilabaoPayNet.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class DesPlus
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="message">明文</param>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public static string Encrypt(string message, string key)
        {
            try
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(message);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);// 密匙

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(keyBytes, keyBytes), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
                cs.Dispose();
                cs = null;

                String result = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                ms.Dispose();
                ms = null;

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="message">密文</param>
        /// <param name="key">密码</param>
        /// <returns></returns>
        public static string Decrypt(string message, string key)
        {
            try
            {
                byte[] inputByteArray = Convert.FromBase64String(message);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);// 密匙

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(keyBytes, keyBytes), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
                cs.Dispose();
                cs = null;

                String result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                ms.Dispose();
                ms = null;
                return result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}