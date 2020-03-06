using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "sdk",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}