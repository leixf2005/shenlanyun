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
        public String result { get; set; }
        /// <summary>
        /// 商户编号
        /// </summary>
        public String mid { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String merchantid { get; set; }
        /// <summary>
        /// 支付通道
        /// </summary>
        public String channel { get; set; }
        /// <summary>
        /// 订单状态 
        /// “0000” : 处理中
        /// “1000” : 支付完成
        /// “2000” : 失败
        /// “3000” : 异常
        /// </summary>
        public String code { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public String amount { get; set; }
        /// <summary>
        /// 订单状态
        /// <para>“ Processing ” : 处理中
        /// “Success” : 支付完成
        /// “ Failed ” : 失败
        /// “ Abnormal” : 异常
        /// </para>
        /// </summary>
        public String message { get; set; }
        /// <summary>
        /// 订单支付时间
        /// </summary>
        public String time { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public String sign { get; set; }

    }
}
