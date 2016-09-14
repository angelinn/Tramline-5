using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common.Managers
{
    public static class ParseManager
    {
        public static string ToStopCode(string value)
        {
            return String.Format("{0:D4}", Int32.Parse(value));
        }

        public static string ParseStopTitle(string stopTitle)
        {
            if (stopTitle == null)
                return String.Empty;

            string parsed = WebUtility.HtmlDecode(stopTitle);
            return parsed.Substring(parsed.IndexOf(".") + 1).Trim();
        }

        public static string ParseSumcVehicleType(char type)
        {
            switch (type)
            {
                case '0':
                    return "Трамвай";
                case '1':
                    return "Автобус";
                case '2':
                    return "Тролей";
                default:
                    return String.Empty;
            }
        }
    }
}
