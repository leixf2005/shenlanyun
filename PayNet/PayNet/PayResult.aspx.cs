using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PayResult : System.Web.UI.Page
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            String writeString = "SUCCESS";
            if (IsPostBack)
            {
                Response.Write(writeString);
                Response.End();
                return;
            }

            ResponseHandler resHandler = new ResponseHandler(Context);
            FileLogUtils.Debug("PayResult.aspx", resHandler.pairs.ToJsonString(), false);

            if (resHandler.pairs.Count == 0)
            {
                Response.Write(writeString);
                return;
            }

            NotifyResult requestParam = new NotifyResult();
            Result sdkResult = SDK.checkReturntParam(resHandler.pairs, ref requestParam);

            FileLogUtils.Info("PayResult.aspx", sdkResult.ToJsonString());

            ApiLog log = new ApiLog();
            //log.apitype = 7;
            log.type = 2;
            log.url = "PayResult.aspx";
            log.datas = resHandler.pairs.ToJsonString();
            log.orderid = requestParam.merchantid;
            ApiLogUntils.AddLog(log);

            if (sdkResult.status != "1")
            {
                Response.Redirect(String.Format("message.html?m={0}", Uri.EscapeDataString("支付失败. 订单号:" + requestParam.merchantid)));
                return;
            }

            Recharge recharge = new Recharge();
            recharge.id = requestParam.merchantid;
            recharge.pay_orderid = requestParam.systemid;
            recharge.pay_money = requestParam.resultMoney;
            if (recharge != null && !String.IsNullOrEmpty(recharge.id))
            {
                recharge.payStatus = requestParam.status == "1" ? 1 : 2;
                RechargeUtils.UpdateRechargeState(recharge);
            }
            Response.Redirect(String.Format("message.html?m={0}", Uri.EscapeDataString("支付成功. 订单号:" + requestParam.merchantid)));
        }

    }
}

