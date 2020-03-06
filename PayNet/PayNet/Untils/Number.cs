using System;
using System.Collections.Generic;
using System.Text;

namespace PayNet
{
    /// <summary>
    /// 压缩ID(ID加密)
    /// </summary>
    public class Number
    {
        /// <summary>
        /// 
        /// </summary>
        private static object thisLock = new object();

        /// <summary>
        /// 生成平台单编号  年月日+时分秒+毫秒
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetAPIBillNo(string UserInfoID = "")
        {
            lock (thisLock)
            {
                //年月日时分秒
                DateTime dt = DateTime.Now;
                string v_ymd = dt.ToString("yyMMdd"); // yyyyMMdd
                string timeStr = dt.ToString("HHmmssfff"); // HHmmss
                string fffStr = CreateNumber.RandomString(1, 5);
                string billno_up = v_ymd + UserInfoID.Trim() + timeStr + fffStr;
                return billno_up;
            }
        }

    }

}
