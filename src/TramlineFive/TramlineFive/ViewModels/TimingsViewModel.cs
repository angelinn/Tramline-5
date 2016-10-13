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
                return $"ПРИСТИГАНИЯ - {Name}";
            }
        }

        public string Name { get; private set; }

        public void Update(string name, List<string> timings)
        {
            Name = name;
            ArrivalTimings = timings.GroupBy(t => Regex.Match(t, "-?[0-9]+").Value);
        }
    }
}
