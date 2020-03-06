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
    public class ApiLog
    {
        /// <summary>
        /// 
        /// </summary>
        public long kid { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public String orderid { get; set; }

        /// <summary>
        /// API类型(1-米拉宝,2-YiJia，3-9A, 20-天空支付 )
        /// </summary>
        public int apitype { get; set; } = 20;

        /// <summary>
        /// 数据类型(1-发送请求,2-返回结果)
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String datas { get; set; }

    }
}
