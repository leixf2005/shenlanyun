using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAccount
    {
        public int id { get; set; }

        public String sid { get; set; }

        public String accounts { get; set; }

        public String password { get; set; }

        public int ban { get; set; }

        public String reg_ip { get; set; }

        public String reg_time { get; set; }

        public String agent { get; set; }


    }
}