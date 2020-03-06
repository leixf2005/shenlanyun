
using Quartz;
using Quartz.Impl;
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
    public static class QuartzScheule
    {
        //调度器工厂
        static ISchedulerFactory factory;

        //调度器
        static IScheduler scheduler;

        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            if (!ConfigUtils.QuartzEnabled)
            {
                return;
            }
            if (String.IsNullOrEmpty(ConfigUtils.QuartzCron))
            {
                return;
            }

            QuartzJob.ExecuteJob();

            //1、创建一个调度器
            factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();
            scheduler.Start();

            //2、创建一个任务
            IJobDetail job = JobBuilder.Create<QuartzJob>().WithIdentity("job1", "group1").Build();

            //3、创建一个触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithCronSchedule(ConfigUtils.QuartzCron)
                .Build();

            //4、将任务与触发器添加到调度器中
            scheduler.ScheduleJob(job, trigger);

            //5、开始执行
            scheduler.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Close()
        {
            if (scheduler != null)
            {
                scheduler.Shutdown(true);
            }
        }
    }
}