using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayNet
{
    /// <summary>
    /// 
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 
        /// </summary>
        public String status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Object data { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class Result1 : Result
    {
        /// <summary>
        /// 
        /// </summary>
        public String postUrl { get; set; }

    }
}