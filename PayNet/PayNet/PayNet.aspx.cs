using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PayNet
{
    public partial class PayNet : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            ResponseHandler resHandler = new ResponseHandler(Context);
            RequestParam param = CommonUntils.DictionaryToClass<RequestParam>(resHandler.pairs);
            if (param == null)
            {
                param = new RequestParam();
            }
            if (String.IsNullOrEmpty(param.postMessage))
            {
                Response.Redirect("message.html?m=提交数据异常，请稍候再试.");
                return;
            }

            String postMessage = "";
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[param.postMessage] != null)
            {
                postMessage = HttpContext.Current.Session[param.postMessage].ToString();
            }
            if (String.IsNullOrEmpty(postMessage))
            {
                Response.Redirect("message.html?m=提交数据异常，请稍候再试.");
                return;
            }
            postMessage = Base64.Decode(postMessage);
            if (String.IsNullOrEmpty(postMessage))
            {
                Response.Redirect("message.html?m=提交数据异常，请稍候再试.");
                return;
            }
            Dictionary<string, string> pay_params = postMessage.FromJsonString<Dictionary<string, string>>();
            if (pay_params == null || pay_params.Count == 0)
            {
                Response.Redirect("message.html?m=提交数据异常，请稍候再试.");
                return;
            }

            String dicKey = "remark";
            String mark = "";
            if (pay_params.ContainsKey(dicKey))
            {
                mark = pay_params[dicKey];
                mark = Base64.Decode(mark);
            }
            Recharge recharge = mark.FromJsonString<Recharge>();
            if (recharge != null)
            {
                recharge.payStatus = 0;
                RechargeUtils.AddHistoryRecharge(recharge);
            }
            pay_params.Remove(dicKey);
            
            NameValueCollection data = new NameValueCollection();
            foreach (var item in pay_params)
            {
                data.Add(item.Key, item.Value);
            }

            String apiUrl = ConfigUtils.payurl;
            String joinPostParam = String.Join("&", pay_params.Select(A => String.Format("{0}={1}", A.Key, A.Value)).ToList());
            FileLogUtils.Debug("RedirectAndPOST  Url", String.Format("{0}?{1}", apiUrl, joinPostParam), true);

            ApiLog log = new ApiLog();
            log.orderid = recharge.id;
            log.type = 1;
            log.url = apiUrl;
            log.datas = pay_params.ToJsonString();
            ApiLogUntils.AddLog(log);

            HttpHelper.RedirectAndPOST(this.Page, apiUrl, data);
        }

    }
}