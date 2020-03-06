
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace PayNet
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigUtils.InitConfig();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            QuartzScheule.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            this.PostAuthenticateRequest += (sender, e) =>
            {
                HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            };
            base.Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            QuartzScheule.Close();
        }
    }
}