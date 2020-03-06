
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class QuartzJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void IJob.Execute(IJobExecutionContext context)
        {
            Task.Factory.StartNew(() =>
            {
                QuartzJob.ExecuteJob();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ExecuteJob()
        {
            FileLogUtils.Task("QuartzJob.ExecuteJob");

            RechargeTask rechargeTask = new RechargeTask();
            rechargeTask.DoTask();
        }

    }
}