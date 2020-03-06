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
    public partial class PayNotify : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            String writeString = "OK";
            if (IsPostBack)
            {
                Response.Write(writeString);
                Response.End();
                return;
            }

            ResponseHandler resHandler = new ResponseHandler(Context);
            FileLogUtils.Debug("PayNotify.aspx", resHandler.pairs.ToJsonString(), false);

            if (resHandler.pairs.Count == 0)
            {
                Response.Write(writeString);
                return;
            }

            NotifyResult requestParam = new NotifyResult();
            Result sdkResult = SDK.checkReturntParam(resHandler.pairs, ref requestParam);

            FileLogUtils.Info("PayNotify.aspx", sdkResult.ToJsonString());

            ApiLog log = new ApiLog();
            log.type = 2;
            log.url = "PayNotify.aspx";
            log.datas = resHandler.pairs.ToJsonString();
            log.orderid = requestParam.out_trade_no;
            ApiLogUntils.AddLog(log);

            if (sdkResult.status != "1")
            {
                Response.Write(writeString);
                Response.End();
                return;
            }

            Recharge recharge = new Recharge();
            recharge.id = requestParam.out_trade_no;
            recharge.pay_orderid = requestParam.trade_no;
            recharge.pay_money = requestParam.resultMoney;
            if (recharge != null && !String.IsNullOrEmpty(recharge.id))
            {
                recharge.payStatus = 1;
                RechargeUtils.UpdateRechargeState(recharge);
            }
            Response.Write(writeString);
            Response.End();
        }
    }
}