using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Entities
{
    public class Direction : BaseEntity
    {
        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public List<Day> Days { get; set; }

        public int? LineID { get; set; }
        public Line Line { get; set; }
    }
}
