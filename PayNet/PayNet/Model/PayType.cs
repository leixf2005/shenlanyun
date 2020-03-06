using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public class PayType
    {
        /// <summary>
        /// 支付编码
        /// </summary>
        public String Key { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public Boolean IsOpen { get; set; } = true;
        /// <summary>
        /// 是否默认选项
        /// </summary>
        public Boolean IsDefault { get; set; } = false;

    }
}