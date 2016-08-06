using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Entities
{
    public class Line : BaseEntity
    {
        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }
        
        public string Name { get; set; }
        public List<Direction> Directions { get; set; }

        public override string ToString()
        {
            string[] split = Name.Split('/');
            string type = String.Empty;

            switch(split[0])
            {
                case "tramway":
                    type = "Трамвай";
                    break;
                case "autobus":
                    type = "Автобус";
                    break;
                case "trolleybus":
                    type = "Тролей";
                    break;
                    
            }

            return $"{type} №{split[1]}";
        }
    }
}
