using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PayNet
{
    /// <summary>
    /// 封装与服务器进行 Http Post 的临时类
    /// </summary>
    public static class HttpClientProxy
    {
        #region 私有属性定义

        /// <summary>
        /// 超时时间(ms)
        /// </summary>
        private const Int32 Timeout = 70000;
        /// <summary>
        /// Content-typeHTTP 标头的值。默认值为 null
        /// </summary>
        private const String ContentType = "application/octet-stream";
        /// <summary>
        /// 用于代理请求的 System.Net.IWebProxy 对象
        /// </summary>
        private const IWebProxy Proxy = null;
        /// <summary>
        /// 此 System.Net.ServicePoint 对象上允许的最大连接数
        /// </summary>
        private const int ConnectionLimit = 512;
        /// <summary>
        /// 该值指示是否与 Internet 资源建立持久性连接
        /// </summary>
        public const bool KeepAlive = false;
        /// <summary>
        /// 如果请求应自动跟随 Internet 资源的重定向响应，则为 true，否则为 false。默认值为 true
        /// </summary>
        private const bool AllowAutoRedirect = true;
        /// <summary>
        /// 若要启用 100-Continue 行为，则为 true。默认值为 true
        /// </summary>
        private const bool Expect100Continue = false;
        /// <summary>
        /// 用于请求的 HTTP 版本。默认值为 System.Net.HttpVersion.Version11
        /// </summary>
        private static Version HttpVersion
        {
            get
            {
                return System.Net.HttpVersion.Version11;
            }
        }
        /// <summary>
        /// 定义缓存策略的 System.Net.Cache.RequestCachePolicy 对象
        /// </summary>
        private static RequestCachePolicy CachePolicy
        {
            get
            {
                return new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            }
        }

        #endregion

        #region Get请求

        /// <summary>
        /// HTTP GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String GetRequestString(String url)
        {
            HttpWebRequest webRequest = null;
            Stream webResponseStream = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.ContentType = "text/html;charset=UTF-8";
                webRequest.Proxy = HttpClientProxy.Proxy;
                webRequest.KeepAlive = HttpClientProxy.KeepAlive;
                webRequest.AllowWriteStreamBuffering = false;
                webRequest.ServicePoint.UseNagleAlgorithm = false;
                webRequest.ServicePoint.Expect100Continue = HttpClientProxy.Expect100Continue;
                webRequest.ServicePoint.ConnectionLimit = HttpClientProxy.ConnectionLimit;
                webRequest.MaximumAutomaticRedirections = HttpClientProxy.ConnectionLimit;
                webRequest.AllowAutoRedirect = HttpClientProxy.AllowAutoRedirect;
                webRequest.Timeout = 30 * 1000;
                webRequest.CachePolicy = HttpClientProxy.CachePolicy;
                webRequest.ProtocolVersion = HttpClientProxy.HttpVersion;

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                webResponseStream = webResponse.GetResponseStream();
                StreamReader ms = new StreamReader(webResponseStream, Encoding.Default);
                string respBody = ms.ReadToEnd();

                ms.Close();
                ms.Dispose();
                ms = null;

                webResponse.Close();
                webResponse = null;

                return respBody;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (webResponseStream != null)
                {
                    webResponseStream.Close();
                    webResponseStream.Dispose();
                    webResponseStream = null;
                }
                if (webRequest != null)
                {
                    webRequest = null;
                }
            }
        }

        #endregion

        #region Http请求

        /// <summary>
        /// RestSharp
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static String ExecRestSharpHttp(String uriString, SortedDictionary<String, String> param)
        {
            try
            {
                Uri url = new Uri(uriString);
                RestClient client = new RestClient(url);
                client.AutomaticDecompression = true;
                client.Proxy = HttpClientProxy.Proxy;
                client.Timeout = HttpClientProxy.Timeout;
                client.MaxRedirects = HttpClientProxy.ConnectionLimit;
                client.CachePolicy = HttpClientProxy.CachePolicy;

                RestRequest webRequest = new RestRequest(Method.POST);
                webRequest.Timeout = HttpClientProxy.Timeout;
                webRequest.AddHeader("Accept", "*/*");
                webRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
                foreach (var item in param)
                {
                    webRequest.AddParameter(item.Key, item.Value);
                }

                IRestResponse response = client.Execute(webRequest);
                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    //Exception exception = response.ErrorException;
                    //webRequest = null;
                    //client = null;
                    //throw exception;

                    return null;
                }
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    //Exception exception = response.ErrorException;
                    //webRequest = null;
                    //client = null;
                    //throw exception;

                    return null;
                }

                String content = response.Content;
                webRequest = null;
                client = null;

                return content;
            }
            catch
            {
                return null;
            }
        }

        #endregion

    }
}
