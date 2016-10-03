using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TramlineFive.ViewModels
{
    public class TimingsViewModel
    {
        public IEnumerable<IGrouping<string, string>> ArrivalTimings { get; set; }

        public string Title
        {
            get
            {
                return "ПРИСТИГАНИЯ";
            }
        }

        public void SetArrivals(List<string> timings)
        {
            ArrivalTimings = timings.GroupBy(t => Regex.Match(t, "-?[0-9]+").Value);
        }
    }
}
