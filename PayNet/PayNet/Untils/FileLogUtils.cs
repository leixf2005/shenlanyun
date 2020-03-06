using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class FileLogUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textContent"></param>
        /// <param name="isOutput"></param>
        public static void Debug(String functionName, string textContent, Boolean isOutput)
        {
            try
            {
                if (!ConfigUtils.IsDebug)
                {
                    return;
                }
                String filePath = String.Format(@"{0}logs\Debug_{1}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
                String newContent = String.Format("[Debug]{0} API {1} {2}:{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), functionName, (isOutput ? "output" : "input"), textContent);
                Save(filePath, newContent);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="textContent"></param>
        /// <param name="isOutput"></param>
        public static void Info(String functionName, string textContent)
        {
            try
            {
                if (!ConfigUtils.IsDebug)
                {
                    return;
                }
                String filePath = String.Format(@"{0}logs\Info_{1}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
                String newContent = String.Format("[Info]{0} {1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), functionName, textContent);
                Save(filePath, newContent);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="textContent"></param>
        /// <param name="isOutput"></param>
        public static void Error(String functionName, string textContent)
        {
            try
            {
                if (!ConfigUtils.IsDebug)
                {
                    return;
                }
                String filePath = String.Format(@"{0}logs\Error_{1}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
                String newContent = String.Format("[Error]{0} API {1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), functionName, textContent);
                Save(filePath, newContent);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="textContent"></param>
        /// <param name="isOutput"></param>
        public static void Task(string textContent)
        {
            try
            {
                if (!ConfigUtils.IsDebug)
                {
                    return;
                }
                String filePath = String.Format(@"{0}logs\Task_{1}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
                String newContent = String.Format("[Task]{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),  textContent);
                Save(filePath, newContent);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="textContent"></param>
        /// <param name="isOutput"></param>
        public static void TaskContent(string textContent)
        {
            try
            {
                if (!ConfigUtils.IsDebug)
                {
                    return;
                }
                String filePath = String.Format(@"{0}logs\TaskContent_{1}.log", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));
                String newContent = String.Format("[TaskContent]{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), textContent);
                Save(filePath, newContent);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 保存文本文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="textContent"></param>
        /// <param name="isAppend">是否追加[否-覆盖文本，是-追加文本]</param>
        /// <returns></returns>
        private static Boolean Save(String path, string textContent, Boolean isAppend = true)
        {
            try
            {
                Int32 endIndex = path.LastIndexOf(@"\");
                if (endIndex == -1)
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + path;
                }
                endIndex = path.LastIndexOf(@"\");
                if (endIndex == -1)
                {
                    return false;
                }
                String direPath = path.Substring(0, endIndex);
                if (!Directory.Exists(direPath))
                {
                    Directory.CreateDirectory(direPath);
                }

                FileStream fs;
                StreamWriter sw;
                if (File.Exists(path)) //文件已存在，则追加
                {
                    if (isAppend)
                    {
                        fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    }
                    else
                    {
                        fs = new FileStream(path, FileMode.Open, FileAccess.Write);
                    }
                }
                else
                {
                    fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                }

                sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine(textContent);

                sw.Close();
                sw.Dispose();
                sw = null;

                fs.Close();
                fs.Dispose();
                fs = null;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}