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
            param.Add("service", "Order");
            param.Add("mid", ConfigUtils.mid);  //商户号
            param.Add("merchantid", recharge.id);  //订单号

            String signMD5 = CommonUntils.getSign(param);
            param.Add("sign", signMD5);

            List<String> queryParam = param.Select(A => String.Format("{0}={1}", A.Key, A.Value)).ToList();
            String queryUrl = ConfigUtils.queryurl;
            queryUrl = String.Format("{0}?{1}", queryUrl, String.Join("&", queryParam));

            FileLogUtils.TaskContent("requet begin:" + queryUrl);
            String requestResult = HttpClientProxy.ExecRestSharpHttp(ConfigUtils.queryurl, param);
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
            if (queryResult.result != "1")
            {
                queryResult.code = "0";
                return queryResult;
            }
            if (!String.IsNullOrEmpty(queryResult.code) && queryResult.code.ToUpper() != "1000")
            {
                queryResult.code = "0";
                return queryResult;
            }
            if (queryResult.mid.ToUpper() != ConfigUtils.mid.ToUpper()) //商户号不匹配
            {
                queryResult.code = "0";
                return queryResult;
            }
            if (recharge.id != queryResult.merchantid) //商户订单号不匹配
            {
                queryResult.code = "0";
                return queryResult;
            }

            Double pay_money = 0;
            if (!Double.TryParse(queryResult.amount, out pay_money))
            {
                queryResult.code = "0";
                return queryResult;
            }

            //对签名进行校验
            param = new SortedDictionary<string, string>();
            param.Add("result", queryResult.result);
            param.Add("mid", queryResult.mid);
            param.Add("merchantid", queryResult.merchantid);
            param.Add("channel", queryResult.channel);
            param.Add("code", queryResult.code);
            param.Add("amount", queryResult.amount);
            param.Add("message", queryResult.message);
            param.Add("time", queryResult.time);

            Boolean flag = CommonUntils.verifySign(param, queryResult.sign);
            if (!flag)
            {
                FileLogUtils.Debug("签名校验 失败.", requestResult, false);
                queryResult.code = "0";
                return queryResult;
            }

            Recharge newRecharge = new Recharge();
            newRecharge.id = recharge.id;
            newRecharge.pay_orderid = queryResult.merchantid;
            newRecharge.pay_money = pay_money;
            newRecharge.payStatus = 1;
            RechargeUtils.UpdateRechargeState(newRecharge);

            return queryResult;
        }

    }
}
