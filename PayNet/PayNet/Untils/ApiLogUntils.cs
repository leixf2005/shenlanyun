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
    public static class ApiLogUntils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recharge"></param>
        public static void AddLog(ApiLog log)
        {
            if (log == null)
            {
                return;
            }
            String sql = String.Format("insert into log_api(orderid, apitype, type, url, datas) values('{0}','{1}','{2}','{3}','{4}')", log.orderid, log.apitype, log.type, log.url, log.datas);
            DBUtils.ExecuteNonQuery(sql);
        }

    }
}
