using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common.Models
{
    public class Arrival
    {
        public string Type { get; set; }
        public int VehicleNumber { get; set; }
        public string[] Timings { get; set; }
        public string Direction { get; set; }
        public string StopTitle { get; set; }
    }
}
