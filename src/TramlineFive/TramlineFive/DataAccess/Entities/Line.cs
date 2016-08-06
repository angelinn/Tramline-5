using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Entities
{
    public class Line : BaseEntity
    {
        public Line()
        {
            Directions = new HashSet<Direction>();
        }

        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }

        public string Name { get; set; }
        public ICollection<Direction> Directions { get; set; }
    }
}
