using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public static String PrivateKey { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public static String PublicKey { get; set; }

        #region Attributes

        /// <summary>
        /// 
        /// </summary>
        public static Boolean IsDebug { get; private set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public static Boolean QuartzEnabled { get; private set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public static String QuartzCron { get; private set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public static List<PayType> PayTypes { get; private set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public static List<PayMoney> PayMoneys { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        private static XmlDocument ConfigDocument { get; set; }

        #endregion

        #region 支付配置

        /// <summary>
        /// 商户在网关系统上的商户号
        /// </summary>
        public static String mid { get; private set; }

        /// <summary>
        /// 商户安全码
        /// </summary>
        public static String securityCode { get; private set; }

        /// <summary>
        /// 通知商户Url(在网关返回信息时通知商户的地址，该地址不能带任何参数，否则异步通知会不成功)
        /// </summary>
        public static String notifyurl { get; private set; }

        /// <summary>
        /// 回调Url(在支付成功后跳转回商户的地址,该地址不能带任何参数)
        /// </summary>
        public static String returnurl { get; private set; }

        /// <summary>
        /// 支付网关地址
        /// </summary>
        public static String payurl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static String queryurl { get; private set; }

        /// <summary>
        /// 请求类型 固定值为“Payment” .
        /// </summary>
        public static String service { get; private set; } = "Payment";

        #endregion

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void InitConfig()
        {
            String privateKeyFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SkyPrivateKey.pem");
            if (System.IO.File.Exists(privateKeyFile))
            {
                PrivateKey = System.IO.File.ReadAllText(privateKeyFile);
            }

            String publicKeyFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PublicKey.pem");
            if (System.IO.File.Exists(publicKeyFile))
            {
                PublicKey = System.IO.File.ReadAllText(publicKeyFile);
            }

            if (ConfigurationManager.AppSettings.AllKeys.Contains("Debug"))
            {
                IsDebug = ConfigurationManager.AppSettings["Debug"] == "1";
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("QuartzEnabled"))
            {
                QuartzEnabled = ConfigurationManager.AppSettings["QuartzEnabled"] == "1";
            }
            if (ConfigurationManager.AppSettings.AllKeys.Contains("QuartzCron"))
            {
                QuartzCron = ConfigurationManager.AppSettings["QuartzCron"];
            }

            String filePath = String.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Setting.config");
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            ConfigDocument = new XmlDocument();
            ConfigDocument.Load(filePath);

            InitListPayType();
            InitListPayMoney();
            InitListPaySetting();

            ConfigDocument = null;
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        /// <returns></returns>
        private static void InitListPayType()
        {
            try
            {
                XmlNodeList xmlNodeList = ConfigDocument.SelectNodes("/configuration/payTypes/payType");
                if (xmlNodeList == null || xmlNodeList.Count == 0)
                {
                    return;
                }

                List<PayType> list = new List<PayType>();
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    PayType payType = new PayType();
                    payType.Key = xmlNode.Attributes["key"].Value;
                    payType.Name = xmlNode.Attributes["name"].Value;
                    payType.IsOpen = xmlNode.Attributes["isOpen"].Value == "1";
                    payType.IsDefault = xmlNode.Attributes["isDefault"].Value == "1";
                    list.Add(payType);
                }

                PayTypes = list;
                return;
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 支付金额
        /// </summary>
        /// <returns></returns>
        private static void InitListPayMoney()
        {
            try
            {
                XmlNodeList xmlNodeList = ConfigDocument.SelectNodes("/configuration/payMoneys/payMoney");
                if (xmlNodeList == null || xmlNodeList.Count == 0)
                {
                    return;
                }

                List<PayMoney> list = new List<PayMoney>();
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    PayMoney tmpPayMoney = new PayMoney();

                    string tmpValue = xmlNode.Attributes["value"].Value;
                    Double doubleValue = 0;
                    if (double.TryParse(tmpValue, out doubleValue))
                    {
                        tmpPayMoney.value = doubleValue;
                        tmpPayMoney.IsDefault = xmlNode.Attributes["isDefault"].Value == "1";
                        tmpPayMoney.payKey = xmlNode.Attributes["payKey"].Value;
                        list.Add(tmpPayMoney);
                    }
                }

                list = list.OrderBy(A => A.value).ToList();
                PayMoneys = list;
                return;
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// 支付配置
        /// </summary>
        /// <returns></returns>
        private static void InitListPaySetting()
        {
            try
            {
                XmlNodeList xmlNodeList = ConfigDocument.SelectNodes("/configuration/paySettings/add");
                if (xmlNodeList == null || xmlNodeList.Count == 0)
                {
                    return;
                }

                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    String key = xmlNode.Attributes["key"].Value.ToLower();
                    switch (key)
                    {
                        case "mid":
                            mid = xmlNode.Attributes["value"].Value;
                            break;
                        case "securitycode":
                            securityCode = xmlNode.Attributes["value"].Value;
                            break;
                        case "payurl":
                            payurl = xmlNode.Attributes["value"].Value;
                            break;
                        case "queryurl":
                            queryurl = xmlNode.Attributes["value"].Value;
                            break;
                        case "notifyurl":
                            notifyurl = xmlNode.Attributes["value"].Value;
                            break;
                        case "returnurl":
                            returnurl = xmlNode.Attributes["value"].Value;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
            }
        }

    }
}