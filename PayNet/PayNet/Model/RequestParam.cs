using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestParam
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public String pay_orderid { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public String pay_amount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public String pay_bankcode { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public String code { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public String accounts { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public String group { get; set; }

        /// <summary>
        /// 代理号
        /// </summary>
        public String agent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String uip { get; set; }

        /// <summary>
        /// 中转数据
        /// </summary>
        public String postMessage { get; set; }


        #region 支付结果通知

        /// <summary>
        /// 网站单号/支付宝备注	账号ID/或 201633225454
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// 支付宝/微信/QQ钱包交易单号
        /// </summary>
        public String ddh { get; set; }
        /// <summary>
        /// 支付类型 1支付宝，2QQ钱包，3微信
        /// </summary>
        public String lb { get; set; }
        /// <summary>
        /// 支付金额	   10.00
        /// </summary>
        public String money { get; set; }
        /// <summary>
        /// 交易状态 1 为成功
        /// </summary>
        public String status { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public String key { get; set; }
        /// <summary>
        /// 支付成功日期
        /// </summary>
        public String paytime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Double resultMoney { get; set; }

        #endregion

    }
}