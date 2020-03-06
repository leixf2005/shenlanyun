//using CSharp_easy_RSA_PEM;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;

//namespace PayNet
//{
//    /// <summary>
//    /// RSA签名
//    /// </summary>
//    public class RSAUtils
//    {
//        /// <summary>
//        /// RSA签名
//        /// </summary>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        public string GetSign(string data)
//        {
//            RSACryptoServiceProvider privateRSAkey = Crypto.DecodeRsaPrivateKey(ConfigUtils.PrivateKey);

//            string importantMessage = data;
//            byte[] importantMessageBytes = Encoding.UTF8.GetBytes(importantMessage);

//            byte[] bytes = privateRSAkey.SignData(importantMessageBytes, typeof(SHA1));
//            string signature = Convert.ToBase64String(bytes);

//            return signature;
//        }

//        /// <summary>
//        /// 校验签名
//        /// </summary>
//        /// <param name="data"></param>
//        /// <param name="signature"></param>
//        /// <returns></returns>
//        public bool VerifySign(string data, string signature)
//        {
//            string importantMessage = data;
//            byte[] importantMessageBytes = Encoding.UTF8.GetBytes(importantMessage);
//            byte[] signatureBytes = Convert.FromBase64String(signature);

//            RSACryptoServiceProvider publicX509key = Crypto.DecodeX509PublicKey(ConfigUtils.PublicKey);
//            bool isSignatureOkay = publicX509key.VerifyData(importantMessageBytes, typeof(SHA1),signatureBytes);

//            return isSignatureOkay;
//        }

//    }
//}