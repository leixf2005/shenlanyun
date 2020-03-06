using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strEncode">待加密字符</param>
        /// <returns></returns>
        public static string Encode(string strEncode)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(strEncode);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strDecode">带解密字符</param>
        /// <returns></returns>
        public static string Decode(string strDecode)
        {
            byte[] bytes = Convert.FromBase64String(strDecode);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 从适用于URL的Base64编码字符串转换为普通字符串
        /// </summary>
        public static string Decode4Url(string base64String)
        {
            base64String = UrlDecode(base64String);
            string temp = base64String
                        .Replace('_', '=')
                        .Replace('.', '+')
                        .Replace('-', '/');
            return Encoding.UTF8.GetString(Convert.FromBase64String(temp));
        }

        /// <summary>
        /// 从普通字符串转换为适用于URL的Base64编码字符串
        /// </summary>
        public static string Encode4Url(string normalString)
        {
            return UrlEncode(Convert.ToBase64String(Encoding.UTF8.GetBytes(normalString)).Replace('+', '.').Replace('/', '-').Replace('=', '_'));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlToDecode"></param>
        /// <returns></returns>
        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
            {
                return urlToDecode;
            }
            return HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlToEncode"></param>
        /// <returns></returns>
        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
            {
                return urlToEncode;
            }
            return HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
        }
    }
}