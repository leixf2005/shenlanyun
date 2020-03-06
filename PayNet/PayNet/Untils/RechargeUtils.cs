using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class RechargeUtils
    {
        /// <summary>
        /// 
        /// </summary>
        private static Object LockObj = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private static Recharge GetInfo(String id)
        {
            try
            {
                String sql = String.Format("select agent from recharge where id = '{0}' ", id);
                DataTable dataTable = DBUtils.QueryData(sql);
                List<Recharge> userAccounts = dataTable.GetListByTableName<Recharge>();
                if (userAccounts != null && userAccounts.Count > 0)
                {
                    return userAccounts[0];
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static Recharge GetHistoryInfo(String id)
        {
            try
            {
                String sql = String.Format("select * from recharge_history where id = '{0}' ", id);
                DataTable dataTable = DBUtils.QueryData(sql);
                List<Recharge> userAccounts = dataTable.GetListByTableName<Recharge>();
                if (userAccounts != null && userAccounts.Count > 0)
                {
                    return userAccounts[0];
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static List<Recharge> GetHistoryListByToday()
        {
            try
            {
                String updated_at = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:30:00");
                String sql = String.Format("select a.* from recharge_history a left join recharge b on a.id = b.id where a.payState = 0 and a.pay_querycount < 4 and b.id is null and a.updated_at >= '{0}' ", updated_at);
                DataTable dataTable = DBUtils.QueryData(sql);
                List<Recharge> userAccounts = dataTable.GetListByTableName<Recharge>();

                if (userAccounts != null && userAccounts.Count > 0)
                {
                    List<String> ids = userAccounts.Select(A => A.kid).ToList();
                    sql = "update recharge_history set pay_querycount = (pay_querycount + 1) where kid in (" + String.Join(",", ids) + ")";
                    DBUtils.ExecuteNonQuery(sql);
                }

                return userAccounts;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 新增历史订单信息
        /// </summary>
        /// <param name="recharge"></param>
        public static void AddHistoryRecharge(Recharge recharge)
        {
            if (recharge == null || String.IsNullOrWhiteSpace(recharge.id))
            {
                return;
            }

            String sql = String.Format("delete recharge_history where id = '{0}'", recharge.id);
            DBUtils.ExecuteNonQuery(sql);

            sql = String.Format("insert into recharge_history(id,`group`,accounts,time,money,agent,payState) values('{0}','{1}','{2}','{3}','{4}','{5}',{6})", recharge.id, recharge.group, recharge.accounts, recharge.time, recharge.money, recharge.agent, recharge.payStatus);
            DBUtils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recharge"></param>
        public static void UpdtHistoryRecharge(Recharge recharge)
        {
            if (recharge == null || String.IsNullOrWhiteSpace(recharge.id))
            {
                return;
            }
            String sql = String.Format("update recharge_history set pay_money = '{0}', pay_orderid = '{1}' where id = '{2}' ", recharge.pay_money, recharge.pay_orderid, recharge.id);
            DBUtils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 新增成功的订单信息
        /// </summary>
        /// <param name="recharge"></param>
        private static void AddRecharge(Recharge recharge)
        {
            if (recharge == null || String.IsNullOrWhiteSpace(recharge.id))
            {
                return;
            }
            Recharge recharge1 = GetInfo(recharge.id);

            String sql = "";
            if (recharge1 != null)
            {
                sql = String.Format("update recharge set `group` = '{0}', accounts='{1}', time='{2}', money = '{3}', agent = '{4}' where id = '{5}' ", recharge.group, recharge.accounts, recharge.time, recharge.money, recharge.agent, recharge.id);
                DBUtils.ExecuteNonQuery(sql);

                FileLogUtils.Info("AddRecharge", "历史订单已处理:" + recharge.ToJsonString());
            }
            else
            {
                sql = String.Format("insert into recharge(id,`group`,accounts,time,money,agent) values('{0}','{1}','{2}','{3}','{4}','{5}')", recharge.id, recharge.group, recharge.accounts, recharge.time, recharge.money, recharge.agent);
                DBUtils.ExecuteNonQuery(sql);

                FileLogUtils.Info("AddRecharge", "新订单生成成功:" + recharge.ToJsonString());
            }
        }

        /// <summary>
        /// 更新历史订单信息(主要用于更新支付状态)
        /// </summary>
        /// <param name="recharge"></param>
        public static void UpdateRechargeState(Recharge recharge)
        {
            lock (LockObj)
            {
                if (recharge == null || String.IsNullOrWhiteSpace(recharge.id))
                {
                    FileLogUtils.Info("UpdateRechargeState", "订单参数异常:" + recharge.ToJsonString());
                    return;
                }
                Recharge recharge1 = GetHistoryInfo(recharge.id);
                if (recharge1 == null)
                {
                    FileLogUtils.Info("UpdateRechargeState", "历史订单不存在:" + recharge.ToJsonString());
                    return;
                }

                String sql = "";
                if (String.IsNullOrEmpty(recharge1.pay_orderid) && recharge1.pay_orderid != recharge.pay_orderid)
                {
                    sql = String.Format("update recharge_history set pay_orderid = '{0}' where id = '{1}' ", recharge.pay_orderid, recharge1.id);
                    DBUtils.ExecuteNonQuery(sql);
                    FileLogUtils.Info("UpdateRechargeState", "支付平台流水号已记录:" + recharge.ToJsonString());
                }
                if (recharge1.payStatus == 1)
                {
                    FileLogUtils.Info("UpdateRechargeState", "历史订单已处理:" + recharge.ToJsonString());
                    return;
                }
                sql = String.Format("update recharge_history set payState = {0}, pay_money={1}, pay_orderid='{2}' where id = '{3}' ", recharge.payStatus, recharge.pay_money, recharge.pay_orderid, recharge1.id);
                DBUtils.ExecuteNonQuery(sql);
                FileLogUtils.Info("UpdateRechargeState", "历史订单状态已变更:" + recharge.ToJsonString());

                if (recharge.payStatus == 1)
                {
                    recharge1.payStatus = 1;
                    recharge1.money = recharge.pay_money.Value;
                    recharge1.pay_orderid = recharge.pay_orderid;
                    AddRecharge(recharge1);
                }
            }
        }


    }
}