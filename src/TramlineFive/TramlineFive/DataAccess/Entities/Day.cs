using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Entities
{
    public class Day
    {
        public Day()
        {
            Stops = new HashSet<Stop>();
        }

        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }
        public string Type { get; set; }
        public ICollection<Stop> Stops { get; set; }
    }
}
