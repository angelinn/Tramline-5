using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common
{
    public static class SumcParser
    {
        public static string ParseStopTitle(string stopTitle)
        {
            if (stopTitle == null)
                return String.Empty;

            string parsed = WebUtility.HtmlDecode(stopTitle);
            return parsed.Substring(parsed.IndexOf(".") + 1);
        }
   } 
}
