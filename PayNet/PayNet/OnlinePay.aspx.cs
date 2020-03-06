using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OnlinePay : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, string> pay_params = new Dictionary<string, string>();
        /// <summary>
        /// 列数
        /// </summary>
        private Int32 ColumnCount { get; set; } = 3;

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

            String localHost = ResponseHandler.GetRequestUrl(this.Page);
            ResponseHandler.AddCookie(this.Page, "LocalUrl", localHost);

            ResponseHandler resHandler = new ResponseHandler(Context);
            if (CommonUntils.IsFromMobile(Context.Request.UserAgent))
            {
                ColumnCount = 2;

                this.divBody.Style.Remove("width");
                this.divBody.Style.Remove("margin-top");
                this.divBody.Style.Add("width", "380px");
                this.divBody.Style.Add("margin-top", "10px");
            }

            //判断游戏账号
            String key = "accounts".ToUpper();
            String accounts = "";
            if (resHandler.upperPairs.ContainsKey(key))
            {
                accounts = resHandler.upperPairs[key];
            }
            if (String.IsNullOrWhiteSpace(accounts))
            {
                Response.Redirect("Message.aspx?message=充值账号不能为空.");
                return;
            }

            UserAccount userAccount = AccountUntils.GetInfo(accounts);
            if (userAccount == null)
            {
                Response.Redirect("Message.aspx?message=充值账号不存在, 请先注册账号.");
                return;
            }
            this.accounts.Text = accounts;

            //判断客户区组
            key = "group".ToUpper();
            String group = "";
            if (resHandler.upperPairs.ContainsKey(key))
            {
                group = resHandler.upperPairs[key];
            }
            if (String.IsNullOrWhiteSpace(group))
            {
                Response.Redirect("Message.aspx?message=客户区组不能为空.");
                return;
            }
            this.group.Text = group;

            InitOrderNO();
            InitPayTypes();
            InitPayMoneys();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitOrderNO()
        {
            this.pay_orderid.InnerText = Number.GetAPIBillNo();
        }

        /// <summary>
        /// 支付类型
        /// </summary>
        private void InitPayTypes()
        {
            List<PayType> payTypes = ConfigUtils.PayTypes;
            if (payTypes == null || payTypes.Count == 0)
            {
                return;
            }

            String defaultPayTypeValue = "";
            String scriptString = "<div style=\"text-align: left;\">";

            List<PayType> listPayType = payTypes.Where(A => A.IsOpen).ToList();
            scriptString += "<div><table id=\"tabePayType\">";
            Int32 index = 0;
            PayType firstDefaultPay = listPayType.FirstOrDefault(A => A.IsDefault);
            if (firstDefaultPay == null)
            {
                firstDefaultPay = listPayType[0];
            }
            defaultPayTypeValue = firstDefaultPay == null ? "" : firstDefaultPay.Key;
            while (true)
            {
                if (index >= listPayType.Count)
                {
                    break;
                }
                scriptString += "<tr>";
                for (int i = 0; i < ColumnCount; i++)
                {
                    scriptString += "<td style=\"text-align: left;width:120px;\">";
                    if (index < listPayType.Count)
                    {
                        scriptString += String.Format("<label for=\"rdoPay{0}\"><input type=\"radio\" id=\"rdoPay{1}\" name=\"rdoPay\" value=\"{2}\" {3} onclick=\"rdoPayClik(this);\" style=\"margin-top: -1px;\">{4}</label>", index, index, listPayType[index].Key,
                            ((firstDefaultPay != null && firstDefaultPay.Key == listPayType[index].Key) ? "checked=\"checked\"" : ""),
                            listPayType[index].Name);
                    }
                    scriptString += "</td>";
                    index++;
                }
                scriptString += "</tr>";
            }
            scriptString += "</table></div>";
            scriptString += "</div>";
            this.tablePayType.Text = scriptString;
            this.pay_bankcode.Text = defaultPayTypeValue;
        }
        /// <summary>
        /// 支付金额
        /// </summary>
        private void InitPayMoneys()
        {
            String defaultPayTypeValue = this.pay_bankcode.Text;
            String defaultMoney = "";

            List<String> payTypes = ConfigUtils.PayTypes.Where(A => A.IsOpen).Select(A => A.Key).ToList();
            String scriptString = "";
            int startIndex = 0;
            foreach (String payKey in payTypes)
            {
                List<PayMoney> payMoneys = ConfigUtils.PayMoneys.Where(A => A.payKey == "0" || A.payKey == payKey
                ).ToList();

                //
                PayMoney firstDefaultPayMoney = null;
                if (defaultPayTypeValue == payKey)
                {
                    firstDefaultPayMoney = payMoneys.FirstOrDefault(A => A.IsDefault);
                    defaultMoney = firstDefaultPayMoney == null ? "" : firstDefaultPayMoney.value.ToString();
                    scriptString += "<table id=\"table_" + payKey + "\">";
                }
                else
                {
                    firstDefaultPayMoney = payMoneys.FirstOrDefault(A => A.IsDefault);
                    if (firstDefaultPayMoney == null && payMoneys.Count > 0)
                    {
                        firstDefaultPayMoney = payMoneys[0];
                    }

                    scriptString += "<table id=\"table_" + payKey + "\" style=\"display: none;\" >";
                }

                Int32 index = 0;
                while (true)
                {   
                    if (index >= payMoneys.Count)
                    {
                        break;
                    }
                    scriptString += "<tr>";
                    for (int i = 0; i < ColumnCount; i++)
                    {
                        startIndex++;
                        scriptString += "<td style=\"text-align: left;width:120px;\">";
                        if (index < payMoneys.Count)
                        {
                            PayMoney tmpPayMoney = payMoneys[index];
                            scriptString += String.Format("<label for=\"rdoPayMoney{0}\"><input type=\"radio\" id=\"rdoPayMoney{1}\" name=\"rdoPayMoney_{2}\" {3} value=\"{4}\" onclick=\"payMoneyChange(this);\" style=\"margin-top: -1px;\">{5}元</label>", startIndex, startIndex, payKey,
                                ((firstDefaultPayMoney != null && firstDefaultPayMoney.value == tmpPayMoney.value) ? "checked=\"checked\"" : ""),
                                tmpPayMoney.value.ToString(),
                                tmpPayMoney.value.ToString());
                        }
                        scriptString += "</td>";
                        index++;
                    }
                    scriptString += "</tr>";
                }
                scriptString += "</table>";
            }

            this.tablePayMoney.Text = scriptString;
            if (!String.IsNullOrEmpty(defaultMoney))
            {
                this.pay_amount.Text = defaultMoney;
            }
        }

    }
}