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
    public static class RechargeQueryUntils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recharge"></param>
        /// <returns></returns>
        public static QueryResult QueryOrder(Recharge recharge)
        {
            QueryResult queryResult = new QueryResult();
            queryResult.code = "0";
            if (recharge == null || String.IsNullOrEmpty(recharge.id))
            {
                return queryResult;
            }

            SortedDictionary<string, string> param = new SortedDictionary<string, string>();
            param.Add("act", "order");
            param.Add("pid", ConfigUtils.pid);  //商户号
            param.Add("key", ConfigUtils.key);
            param.Add("out_trade_no", recharge.id);  //订单号

            List<String> queryParam = param.Select(A => String.Format("{0}={1}", A.Key, A.Value)).ToList();
            String queryUrl = ConfigUtils.queryurl;
            queryUrl = String.Format("{0}?{1}", queryUrl, String.Join("&", queryParam));

            FileLogUtils.TaskContent("requet begin:" + queryUrl);
            String requestResult = HttpClientProxy.GetRequestString(queryUrl);
            FileLogUtils.TaskContent("requet end:" + requestResult);
            if (String.IsNullOrEmpty(requestResult))
            {
                queryResult.code = "0";
                return queryResult;
            }

            ApiLog log = new ApiLog();
            log.orderid = recharge.id;
            log.type = 3;
            log.url = queryUrl;
            log.datas = requestResult;
            ApiLogUntils.AddLog(log);

            queryResult = JsonProxy.FromJsonString<QueryResult>(requestResult);
            if (queryResult == null)
            {
                queryResult = new QueryResult();
            }
            if (queryResult.code != "1" || queryResult.status != "1")
            {
                queryResult.code = "0";
                return queryResult;
            }
            if (queryResult.pid.ToUpper() != ConfigUtils.pid.ToUpper()) //商户号不匹配
            {
                queryResult.code = "0";
                return queryResult;
            }
            if (recharge.id != queryResult.out_trade_no) //商户订单号不匹配
            {
                queryResult.code = "0";
                return queryResult;
            }

            Double pay_money = 0;
            if (!Double.TryParse(queryResult.money, out pay_money))
            {
                queryResult.code = "0";
                return queryResult;
            }

            Recharge newRecharge = new Recharge();
            newRecharge.id = recharge.id;
            newRecharge.pay_orderid = queryResult.trade_no;
            newRecharge.pay_money = pay_money;
            newRecharge.payStatus = 1;
            RechargeUtils.UpdateRechargeState(newRecharge);

            return queryResult;
        }

    }
}
