using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class PayMoney
    {
        /// <summary>
        /// 支付金额
        /// </summary>
        public Double value { get; set; }
        /// <summary>
        /// 是否默认选项
        /// </summary>
        public Boolean IsDefault { get; set; } = false;
        /// <summary>
        /// 支付类型
        /// </summary>
        public String payKey { get; set; }
    }
}