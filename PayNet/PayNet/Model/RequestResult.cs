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
    public class RequestResult
    {
        /// <summary>
        /// 1表示成功返回二维码数据  0表示错误
        /// </summary>
        public String state { get; set; }
        /// <summary>
        /// 返回二维码数据需要将数据生成二维码图片（需要将数据生成一个二维码图片展现）
        /// </summary>
        public String qrcode { get; set; }
        /// <summary>
        /// 云端生产的一个单号 可以通过这个单号来查询订单状态
        /// </summary>
        public String order { get; set; }
        /// <summary>
        /// 返回data数据
        /// </summary>
        public String data { get; set; }
        /// <summary>
        /// 返回付款金额
        /// </summary>
        public String money { get; set; }
        /// <summary>
        /// 订单时间(时间戳格式)  使用订单时间减去当前时间
        /// </summary>
        public String times { get; set; }
        /// <summary>
        /// 订单状态  0正在付款   1 已付款
        /// </summary>
        public String orderstatus { get; set; }
        /// <summary>
        /// 代码标识（可根据代码标识来判断是什么问题）
        /// </summary>
        public String text { get; set; }

        public String message { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public String payType { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public String orderid { get; set; }
    }
}
