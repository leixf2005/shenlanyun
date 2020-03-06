using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DBUtils
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly string MySqlConnectString = ConfigurationManager.AppSettings["ConnectString"];

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable QueryData(String sql)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySqlConnectString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    DataTable dt = new DataTable();
                    using (MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        dt.Load(reader);
                    }
                    conn.Close();
                    conn.Dispose();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Boolean ExecuteNonQuery(String sql)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySqlConnectString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    int count = cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}