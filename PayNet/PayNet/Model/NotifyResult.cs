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
        public String pid { get; set; }
        /// <summary>
        /// 易支付订单号
        /// </summary>
        public String trade_no { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public String out_trade_no { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public String type { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public String money { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public String trade_status { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public String sign { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public String sign_type { get; set; }
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