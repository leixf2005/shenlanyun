using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class sdkController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public Newtonsoft.Json.Linq.JObject getPostParam([FromBody]RequestParam login)
        {
            FileLogUtils.Debug("getPostParam", login.ToJsonString(), false);

            if (login == null)
            {
                login = new RequestParam();
            }
            Newtonsoft.Json.Linq.JObject jobject = null;
            try
            {
                String sessionCode = "";
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["CheckCode"] != null)
                {
                    sessionCode = HttpContext.Current.Session["CheckCode"].ToString();
                }

                login.uip = ResponseHandler.GetIPAddress();
                Result1 result = SDK.getPostParam(login, sessionCode);
                if (result.status == "1")
                {
                    String newSessionId = MD5Untils.GetMd5(TimeUntils.GetNow() + CommonUntils.CreateRandomCode(5)).ToUpper();
                    HttpContext.Current.Session[newSessionId] = result.message;
                    result.message = newSessionId;
                }

                String jsonString = result.ToJsonString();
                FileLogUtils.Debug("getPostParam", jsonString, true);
                jobject = jsonString.ConvertJObject();
            }
            catch (Exception ex)
            {
                FileLogUtils.Error("getPostParam", ex.Message);
            }
            return jobject;
        }

    }
}