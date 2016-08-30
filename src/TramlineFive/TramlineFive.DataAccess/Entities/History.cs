using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Entities
{
    public class History : BaseEntity
    {
        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }
        public DateTime? TimeStamp { get; set; }

        public int? StopID { get; set; }
        public Stop Stop { get; set; }
    }
}
