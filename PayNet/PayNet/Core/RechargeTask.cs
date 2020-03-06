using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayNet
{
    public class RechargeTask
    {
        /// <summary>
        /// 
        /// </summary>
        public void DoTask()
        {
            List<Recharge> listRecharge = RechargeUtils.GetHistoryListByToday();
            if (listRecharge == null || listRecharge.Count == 0)
            {
                return;
            }
            Dictionary<String, String> param = new Dictionary<string, string>();
            foreach (Recharge item in listRecharge)
            {
                RechargeQueryUntils.QueryOrder(item);
            }
        }
    }
}
