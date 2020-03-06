using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MilabaoPayNet.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class PaySetting
    {
        /// <summary>
        /// API网关地址
        /// </summary>
        public String refer_url { get; set; }
        /// <summary>
        /// 平台分配商户号
        /// </summary>
        public String pay_memberid { get; set; }
        /// <summary>
        /// 商户密钥
        /// </summary>
        public String apikey { get; set; }
        /// <summary>
        /// 服务端返回地址.(POST返回数据)
        /// </summary>
        public String pay_notifyurl { get; set; }
        /// <summary>
        /// 页面跳转返回地址(POST返回数据)
        /// </summary>
        public String pay_callbackurl { get; set; }

        /// <summary>
        /// 上送订单号唯一, 字符长度20
        /// </summary>
        public String pay_orderid { get; set; }
        /// <summary>
        /// 提交时间, 时间格式：2016-12-26 18:18:18
        /// </summary>
        public String pay_applydate { get; set; }
        /// <summary>
        /// 银行编码
        /// </summary>
        public String pay_bankcode { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public String pay_amount { get; set; }
        /// <summary>
        /// MD5签名
        /// </summary>
        public String pay_md5sign { get; set; }
        /// <summary>
        /// 附加字段
        /// </summary>
        public String pay_attach { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public String pay_productname { get; set; }
        /// <summary>
        /// 商户品数量
        /// </summary>
        public String pay_productnum { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public String pay_productdesc { get; set; }
        /// <summary>
        /// 商户链接地址
        /// </summary>
        public String pay_producturl { get; set; }

    }
}