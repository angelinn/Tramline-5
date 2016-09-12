using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Repositories;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class StopDO
    {
        public StopDO(Stop entity)
        {
            id = entity.ID;
            name = entity.Name;
            code = entity.Code;
            timings = entity.Timings;
        }

        public static async Task<IEnumerable<StopDO>> GetFromDayId(int dayId)
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var stops = uow.Stops.Where(s => s.DayID == dayId).ToList();
                    return stops?.Select(s => new StopDO(s));
                }
            });
        }

        private int id;

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        private string code;
        public string Code
        {
            get
            {
                return code;
            }
        }

        private List<string> timings;
        public List<string> Timings
        {
            get
            {
                return timings;
            }
        }
    }
}
