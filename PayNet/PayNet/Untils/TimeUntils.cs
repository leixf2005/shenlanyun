using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class TimeUntils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static String GetTime()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static String GetTimeStamp()
        {
            return GetUTCTimestampByNow().ToString() + "000";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static long GetNow()
        {
            return GetUTCTimestampByNow();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static String GetTimeStamp(String timestamp)
        {
            return timestamp + "000";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static String GetTimeStamp(long timestamp)
        {
            return timestamp.ToString() + "000";
        }

        /// <summary>
        /// 获取UTC时间戳
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long GetUTCTimestampByNow()
        {
            return GetUTCTimestamp(DateTime.Now);
        }
        /// <summary>
        /// 获取UTC时间戳
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long GetUTCTimestamp(DateTime d)
        {
            TimeSpan ts = d.ToUniversalTime() - new DateTime(1970, 1, 1);
            long result = (long)ts.TotalSeconds;     //精确到秒
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static String ConvertTimeStampToDateTime(String timeStamp, String format)
        {
            long longTime = 0;
            if (!long.TryParse(timeStamp, out longTime))
            {
                return "";
            }
            try
            {
                DateTime dateTime = ConvertTimeStampToDateTime(longTime);
                return dateTime.ToString(format);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 转换时间戳为日期
        /// </summary>
        /// <param name="timeStamp">时间戳 单位：毫秒</param>
        /// <returns>C#时间</returns>
        public static DateTime ConvertTimeStampToDateTime(long timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(timeStamp);
            return dt;
        }

    }
}