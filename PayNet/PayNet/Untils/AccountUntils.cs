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
    public class AccountUntils
    {

        /// <summary>
        /// 获取账号信息
        /// </summary>
        public static UserAccount GetInfo(String userName)
        {
            try
            {
                String sql = String.Format("select * from accounts where accounts = '{0}'", userName);
                DataTable dataTable = DBUtils.QueryData(sql);
                List<UserAccount> userAccounts = dataTable.GetListByTableName<UserAccount>();
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
        /// 判断账号是否存在
        /// </summary>
        public static Boolean FindAccounts(String userName)
        {
            try
            {
                UserAccount userAccount = GetInfo(userName);
                return userAccount != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}