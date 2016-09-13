using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common
{
    public static class CommonManager
    {
        public static string ToStopCode(string value)
        {
            return String.Format("{0:D4}", Int32.Parse(value));
        }
    }
}
