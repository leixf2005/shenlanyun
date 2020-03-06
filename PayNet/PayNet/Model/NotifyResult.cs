using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class NotifyResult
    {
        /// <summary>
        /// 商户编号
        /// </summary>
        public String mid { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String merchantid { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public String amount { get; set; }
        /// <summary>
        /// 系统订单号
        /// </summary>
        public String systemid { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public String time { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public String status { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public String sign { get; set; }
        /// <summary>
        /// 扩展返回
        /// </summary>
        public String remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Double resultMoney { get; set; }
    }
}