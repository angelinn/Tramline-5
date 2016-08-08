using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Entities
{
    public class Day : BaseEntity
    {
        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }
        public string Type { get; set; }
        public List<Stop> Stops { get; set; }

        public int? DirectionID { get; set; }
        public Direction Direction { get; set; }
    }
}
