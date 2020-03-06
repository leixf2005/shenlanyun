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
    public static class SDK
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Result1 getPostParam(RequestParam param, String sessionCode)
        {
            Result1 result = new Result1();
            try
            {
                Decimal doubleMoney = 0;
                if (!Decimal.TryParse(param.pay_amount, out doubleMoney))
                {
                    result.status = "failed";
                    result.message = "支付金额出现异常，请稍候再试.";
                    return result;
                }
                if (doubleMoney <= 0)
                {
                    result.status = "failed";
                    result.message = "支付金额出现异常，请稍候再试.";
                    return result;
                }

                if (String.IsNullOrEmpty(sessionCode) ||
                    String.IsNullOrEmpty(param.code) ||
                    sessionCode.ToUpper() != param.code.ToUpper())
                {
                    result.status = "failed";
                    result.message = "验证码错误，请重新输入.";
                    return result;
                }
                if (String.IsNullOrWhiteSpace(param.pay_orderid) ||
                    String.IsNullOrWhiteSpace(param.pay_amount) ||
                    String.IsNullOrWhiteSpace(param.group) ||
                    String.IsNullOrWhiteSpace(param.pay_bankcode) ||
                    String.IsNullOrWhiteSpace(param.code))
                {
                    result.status = "failed";
                    result.message = "提交数据出现异常，请稍候再试.";
                    return result;
                }

                PayType payType = ConfigUtils.PayTypes.FirstOrDefault(A => A.Key == param.pay_bankcode);
                if (payType == null)
                {
                    result.status = "failed";
                    result.message = "不支持该支付类型，请重新提交.";
                    return result;
                }

                UserAccount userAccount = AccountUntils.GetInfo(param.accounts);
                if (userAccount == null)
                {
                    result.status = "failed";
                    result.message = "充值账号不存在，请重新提交.";
                    return result;
                }
                //if (String.IsNullOrEmpty(userAccount.agent))
                //{
                //    result.status = "failed";
                //    result.message = "代理不存在，无法充值.";
                //    return result;
                //}
                if (String.IsNullOrEmpty(userAccount.agent))
                {
                    userAccount.agent = "";
                }

                //写入历史订单表
                Recharge recharge = new Recharge();
                recharge.id = param.pay_orderid;
                recharge.group = param.group;
                recharge.accounts = param.accounts;
                recharge.agent = userAccount.agent;
                recharge.money = Double.Parse(doubleMoney.ToString());
                recharge.time = TimeUntils.GetNow();
                recharge.payStatus = 0;
                recharge.payType = param.pay_bankcode;

                //Post参数
                SortedDictionary<string, string> pay_params = new SortedDictionary<string, string>();
                pay_params.Add("pid", ConfigUtils.pid);
                pay_params.Add("type", recharge.payType); //平台分配商户号
                pay_params.Add("out_trade_no", recharge.id); //订单号
                pay_params.Add("notify_url", ConfigUtils.notifyurl); //服务端返回地址(POST返回数据)
                pay_params.Add("return_url", ConfigUtils.returnurl); //页面跳转返回地址(POST返回数据)
                pay_params.Add("name", "充值");
                pay_params.Add("money", doubleMoney.ToString("F2")); //支付金额 
                pay_params.Add("sitename", ConfigUtils.sitename);

                String sign = CommonUntils.getSign(pay_params);
                pay_params.Add("sign", sign);
                pay_params.Add("sign_type", "MD5");
                pay_params.Add("remark", Base64.Encode(recharge.ToJsonString()));

                String postMessage = pay_params.ToJsonString();
                postMessage = Base64.Encode(postMessage);

                result.status = "1";
                result.postUrl = "PayNet.aspx";
                result.message = postMessage;
                return result;
            }
            catch (Exception ex)
            {
                FileLogUtils.Error("getPostParam", ex.StackTrace);
                result.status = "failed";
                result.message = "服务器出现异常，请稍候再试.";
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sessionCode"></param>
        /// <returns></returns>
        public static Result checkReturntParam(Dictionary<string, string> pairs, ref NotifyResult requestParam)
        {
            Result result = new Result();
            try
            {
                requestParam = CommonUntils.DictionaryToClass<NotifyResult>(pairs);
                if (requestParam == null)
                {
                    result.message = "数据解析异常.";
                    return result;
                }
                result.data = requestParam.ToJsonString();

                if (String.IsNullOrEmpty(requestParam.pid)
                    || String.IsNullOrEmpty(requestParam.trade_no)
                    || String.IsNullOrEmpty(requestParam.out_trade_no)
                    || String.IsNullOrEmpty(requestParam.money)
                    || String.IsNullOrEmpty(requestParam.sign))
                {
                    result.message = "数据解析异常.";
                    return result;
                }

                if (requestParam.pid.ToUpper() != ConfigUtils.pid.ToUpper())
                {
                    result.message = "商户号不匹配.";
                    return result;
                }
                if (requestParam.trade_status.ToUpper() != "TRADE_SUCCESS")
                {
                    result.status = "failed";
                    result.message = "支付失败.";
                    return result;
                }

                double doubleMoney = 0;
                if (!double.TryParse(requestParam.money, out doubleMoney))
                {
                    result.status = "failed";
                    result.message = "支付金额出现异常，请稍候再试.";
                    return result;
                }
                if (doubleMoney <= 0)
                {
                    result.status = "failed";
                    result.message = "支付金额出现异常，请稍候再试.";
                    return result;
                }
                requestParam.resultMoney = doubleMoney;

                SortedDictionary<string, string> dicMap = new SortedDictionary<string, string>();
                dicMap.Add("pid", requestParam.pid);
                dicMap.Add("trade_no", requestParam.trade_no);
                dicMap.Add("out_trade_no", requestParam.out_trade_no);
                dicMap.Add("type", requestParam.type);
                dicMap.Add("name", requestParam.name);
                dicMap.Add("money", requestParam.money);
                dicMap.Add("trade_status", requestParam.trade_status);

                Boolean flag = CommonUntils.verifySign(dicMap, requestParam.sign);
                if (!flag)
                {
                    result.message = "身份校验异常.";
                    return result;
                }

                result.status = "1";
                result.message = "";
                return result;
            }
            catch(Exception ex)
            {
                FileLogUtils.Error("getPostParam", ex.StackTrace);
                result.status = "failed";
                result.message = "服务器出现异常，请稍候再试.";
                return result;
            }
        }

    }
}
