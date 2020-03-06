using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommonUntils
    {
        /// <summary>
        /// DataTable To List<Object>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetListByTableName<T>(this DataTable dataTable) where T : new()
        {
            List<T> list = null;
            try
            {
                string tableName = typeof(T).Name;
                if (string.IsNullOrEmpty(tableName))
                {
                    return list;
                }
                if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count == 0)
                {
                    return list;
                }
                list = new List<T>();
                foreach (DataRow row in dataTable.Rows)
                {
                    T model = new T();
                    Type modelType = model.GetType();
                    PropertyInfo[] propertyInfos = modelType.GetProperties();
                    if (propertyInfos == null || propertyInfos.Length == 0)
                    {
                        continue;
                    }
                    foreach (PropertyInfo property in propertyInfos)
                    {
                        if (row.Table.Columns.Contains(property.Name))
                        {
                            object val = row[property.Name];
                            object obj = ChanageType(val, property.PropertyType);
                            property.SetValue(model, obj, null);
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                list = null;
            }
            if (list == null)
            {
                list = new List<T>();
            }
            return list;
        }

        /// <summary>
        /// Dictionary To T<Object>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DictionaryToClass<T>(Dictionary<string, string> pairs) where T : new()
        {
            T model = new T();
            try
            {
                if (pairs == null)
                {
                    return model;
                }
                List<String> pairsKeys = pairs.Keys.ToList();
                pairsKeys = pairsKeys.Select(A => A.ToUpper()).ToList();

                Type modelType = model.GetType();
                PropertyInfo[] propertys = modelType.GetProperties();
                if (propertys == null || propertys.Length == 0)
                {
                    return model;
                }
                foreach (PropertyInfo property in propertys)
                {
                    String propertyName = property.Name.ToUpper();
                    KeyValuePair<String, String> keyValuePair = pairs.FirstOrDefault(A => A.Key.ToUpper() == propertyName);
                    foreach (KeyValuePair<String, String> item in pairs)
                    {
                        if (item.Key.ToUpper() == propertyName)
                        {
                            object obj = ChanageType(item.Value, property.PropertyType);
                            property.SetValue(model, obj, null);
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns></returns>
        public static String QueryRandom(int minValue = 10000, int maxValue = 99999)
        {
            Random random = new Random();
            Int32 randomValue = random.Next(minValue, maxValue);
            return randomValue.ToString();
        }

        /// <summary>
        /// 可为空类型扩展类，转换为基础类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="convertsionType"></param>
        /// <returns></returns>
        private static object ChanageType(this object value, Type convertsionType)
        {
            if (convertsionType.IsGenericType && convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(convertsionType);
                convertsionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, convertsionType);
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }

        /// <summary>
        /// 是否来自手机端
        /// </summary>
        /// <param name="userAgant"></param>
        /// <returns></returns>
        public static Boolean IsFromMobile(String userAgant)
        {
            if (!String.IsNullOrEmpty(userAgant))
            {
                userAgant = userAgant.ToLower();
            }
            if (userAgant.Contains("android") || userAgant.Contains("ipod") || userAgant.Contains("iphone") || userAgant.Contains("ucweb"))
            {
                return true;
            }
            return false;
        }

        ///// <summary>
        ///// 字典转大写Key
        ///// </summary>
        ///// <param name="pairs"></param>
        ///// <returns></returns>
        //public static Dictionary<string, string> ToUpper(Dictionary<string, string> pairs)
        //{
        //    Dictionary<string, string> newPairs = new Dictionary<string, string>();
        //    if (pairs != null && pairs.Count > 0)
        //    {
        //        foreach (var item in pairs)
        //        {
        //            newPairs.Add(item.Key.ToUpper(), item.Value);
        //        }
        //    }
        //    return newPairs;
        //}

        #region 签名(提交到支付AP的签名)

        /// <summary>
        /// 签名
        /// </summary>
        /// <returns></returns>
        internal static string getSign(SortedDictionary<string, string> pairs)
        {
            RSAUtils rsaUtils = new RSAUtils();
            List<String> list = pairs.Select(A => String.Format("{0}={1}", A.Key, A.Value)).ToList();
            String result = String.Join("&", list);
            result += "&" + ConfigUtils.securityCode;
            result = rsaUtils.GetSign(result);
            return result;
        }

        #endregion

        #region 签名(支付AP的返回签名)

        /// <summary>
        /// 签名
        /// </summary>
        /// <returns></returns>
        internal static Boolean verifySign(SortedDictionary<string, string> pairs, String sign)
        {
            RSAUtils rsaUtils = new RSAUtils();
            List<String> list = pairs.Select(A => String.Format("{0}={1}", A.Key, A.Value)).ToList();
            String result = String.Join("&", list);
            result += "&" + ConfigUtils.securityCode;
            Boolean flag = rsaUtils.VerifySign(result, sign);
            return flag;
        }

        #endregion

    }
}