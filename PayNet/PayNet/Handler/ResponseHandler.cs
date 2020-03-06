using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;


namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ResponseHandler
    {
        /// <summary>
        /// 
        /// </summary>
        protected HttpContext httpContext;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> pairs = new Dictionary<string, string>();
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> upperPairs = new Dictionary<string, string>();

        /// <summary>
        /// 获取服务器通知数据方式，进行参数获取
        /// </summary>
        /// <param name="httpContext"></param>
        public ResponseHandler(HttpContext httpContext)
        {
            this.httpContext = httpContext;
            NameValueCollection collection = this.httpContext.Request.Form;
            if ((collection == null || collection.Count == 0) &&
                this.httpContext.Request.QueryString != null &&
                this.httpContext.Request.QueryString.Count > 0)
            {
                collection = this.httpContext.Request.QueryString;
            }
            if (collection == null || collection.Count == 0)
            {
                return;
            }
            List<String> keys = collection.AllKeys.ToList();
            keys.Sort();
            foreach (String key in keys)
            {
                setParameter(key, collection[key]);
            }

            upperPairs = new Dictionary<string, string>();
            foreach (var item in pairs)
            {
                upperPairs.Add(item.Key.ToUpper(), item.Value);
            }
        }
        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        private void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                pairs.Remove(parameter);
                pairs.Add(parameter, parameterValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static String GetRequestUrl(Page page)
        {
            String localHost = String.Format("{0}://{1}{2}", page.Request.Url.Scheme, page.Request.Url.Host, page.Request.Url.Port == 80 ? "" : (":" + page.Request.Url.Port));
            return localHost;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddCookie(Page page, String key, String value)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            page.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddressNew()
        {
            String ipAddress = GetIPAddress();
            Boolean flag = IsIPAddress(ipAddress);
            if (!flag)
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            String ipAddress = GetIPAddressEx();
            if (String.IsNullOrEmpty(ipAddress))
            {
                return ipAddress;
            }
            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }

        /// <summary>
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址
        /// </summary>
        private static string GetIPAddressEx()
        {
            string result = String.Empty;
            try
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (String.IsNullOrEmpty(result))
                {
                    result = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    if (String.IsNullOrEmpty(result))
                    {
                        result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                    if (String.IsNullOrEmpty(result))
                    {
                        result = HttpContext.Current.Request.UserHostAddress;
                    }
                    return result;
                }

                //可能有代理
                if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式
                {
                    return String.Empty;
                }
                if (result.IndexOf(",") != -1)
                {
                    //有“,”，估计多个代理。取第一个不是内网的IP。
                    result = result.Replace(" ", "").Replace("'", "");
                    string[] temparyip = result.Split(",;".ToCharArray());
                    for (int i = 0; i < temparyip.Length; i++)
                    {
                        if (IsIPAddress(temparyip[i])
                            && temparyip[i].Substring(0, 3) != "10."
                            && temparyip[i].Substring(0, 7) != "192.168"
                            && temparyip[i].Substring(0, 7) != "172.16.")
                        {
                            return temparyip[i];    //找到不是内网的地址
                        }
                    }
                }
                else if (IsIPAddress(result)) //代理即是IP格式 ,IsIPAddress判断是否是IP的方法,
                {
                    return result;
                }
                return String.Empty;
            }
            catch
            {
                return String.Empty;
            }
        }

        ///<summary> 
        /// 判断是否是Ip地址
        /// </summary>        
        /// <param name="str1"></param>
        /// <returns></returns>
        private static bool IsIPAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip) || ip.Length < 7 || ip.Length > 15)
            {
                return false;
            }
            string regformat = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(ip);
        }

    }
}
