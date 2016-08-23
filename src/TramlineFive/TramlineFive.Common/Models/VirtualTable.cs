using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common.Models
{
    public class VirtualTable
    {
        public IEnumerable<Arrival> Arrivals { get; set; }
        public IEnumerable<FormUrlEncodedContent> OtherTransportTypes { get; set; }
    }
}
