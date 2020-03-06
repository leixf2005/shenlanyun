using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class Recharge
    {
        /// <summary>
        /// 
        /// </summary>
        public String kid { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public String id { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public String group { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public String accounts { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public Double time { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public Double money { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public Double? pay_money { get; set; } = null;
        /// <summary>
        /// 
        /// </summary>
        public String pay_orderid { get; set; } = "";
        /// <summary>
        /// 支付平台流水号
        /// </summary>
        public String platform_orderid { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public String payType { get; set; } = "0";
        /// <summary>
        /// 
        /// </summary>
        public String agent { get; set; } = "";

        /// <summary>
        /// 是否支付成功(0-提交中,1-成功，2-失败)
        /// </summary>
        public Int32 payStatus { get; set; } = 0;

    }
}