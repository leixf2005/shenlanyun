using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace PayNet
{
    /// <summary>
    /// Json字符串帮助类
    /// </summary>
    public static class JsonProxy
    {
        /// <summary>
        /// 将对象转换成Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToJsonString(this Object obj)
        {
            try
            {
                if (obj == null)
                {
                    return null;
                }
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 将Json字符串转换成指定的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T FromJsonString<T>(this String jsonString)
        {
            try
            {
                if (String.IsNullOrEmpty(jsonString))
                {
                    return JsonConvert.DeserializeObject<T>("null");
                }
                try
                {
                    return JsonConvert.DeserializeObject<T>(jsonString);
                }
                catch(Exception ex)
                {
                    return JsonConvert.DeserializeObject<T>("null");
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JObject ConvertJObject(this String jsonString)
        {
            Newtonsoft.Json.Linq.JObject jobject = null;
            try
            {
                jobject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                return jobject;
            }
            catch (Exception ex)
            {
                return jobject;
            }
        }

    }
}
