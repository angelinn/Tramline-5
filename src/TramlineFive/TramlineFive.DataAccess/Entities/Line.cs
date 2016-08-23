using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Models;

namespace TramlineFive.DataAccess.Entities
{
    public class Line : BaseEntity
    {
        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }

        public VehicleType Type { get; set; }
        public string Number { get; set; }
        public List<Direction> Directions { get; set; }
    }
}
