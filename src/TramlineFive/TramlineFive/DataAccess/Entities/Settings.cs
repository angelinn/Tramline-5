using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus.DataAccess.Entities
{
    public class Settings : BaseEntity
    {
        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }
        public bool PushNotifications { get; set; }
        public bool LiveTile { get; set; }
    }
}
