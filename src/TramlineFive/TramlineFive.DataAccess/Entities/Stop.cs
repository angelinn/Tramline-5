using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.Entities
{
    public class Stop : BaseEntity
    {
        public int GetId()
        {
            return ID;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Index { get; set; }

        public string TimingsAsString
        {
            get
            {
                return (Timings == null) ? String.Empty : String.Join(",", Timings);
            }
            set
            {
                Timings = value.Split(',').ToList();
            }
        }

        public int? DayID { get; set; }
        public Day Day { get; set; }
        
        public List<Favourite> Favourites { get; set; }

        [NotMapped]
        public List<string> Timings { get; set; }
    }
}
