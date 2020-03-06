using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// 请求结果“1”:成功 “0”:失败
        /// </summary>
        public String code { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        public String pid { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public String out_trade_no { get; set; }
        /// <summary>
        /// 易支付订单号
        /// </summary>
        public String trade_no { get; set; }
        /// <summary>
        /// 支付通道
        /// </summary>
        public String type { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public String status { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public String money { get; set; }

    }
}
