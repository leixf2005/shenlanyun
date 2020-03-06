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
    public static class DesPlusEx
    {
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
                byte[] l_value = HexToBin(message);
                if (l_value.Length == 0)
                {
                    return "";
                }
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.IV = GetKey(key);
                    des.Key = GetKey(key);
                    des.Mode = CipherMode.ECB;
                    MemoryStream ms = new MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(l_value, 0, l_value.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = Encoding.Default.GetString(ms.ToArray());
                    ms.Close();
                    return str;
                }
            }
            catch
            {
                return "";
            }
        }
        static byte[] HexToBin(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)//如果长度不是双数 则报错
            {
                return new byte[0];
            }
            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return buffer;
        }

        static byte[] GetKey(string key)
        {
            byte[] l_bits = new byte[8];
            for (int i = 0; i < (key.Length >= l_bits.Length ? l_bits.Length : key.Length); i++)
            {
                l_bits[i] = (byte)key[i];
            }
            return l_bits;
        }

        static string BinToHex(byte[] data)
        {
            if (data.Length == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString().ToUpper();
        }

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
                byte[] l_value = Encoding.Default.GetBytes(message);
                if (l_value.Length == 0)
                {
                    return "";
                }
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.IV = GetKey(key);
                    des.Key = GetKey(key);
                    des.Mode = CipherMode.ECB;
                    MemoryStream ms = new MemoryStream();
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(l_value, 0, l_value.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                    string str = BinToHex(ms.ToArray()).ToLower();
                    ms.Close();
                    return str;
                }
            }
            catch
            {
                return "";
            }
        }

    }
}