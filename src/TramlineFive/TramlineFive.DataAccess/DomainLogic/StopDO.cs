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
        public string Name { get; private set; }
        public string Code { get; private set; }
        public List<string> Timings { get; private set; }

        public StopDO(Stop entity)
        {
            id = entity.ID;
            Name = entity.Name;
            Code = entity.Code;
            Timings = entity.Timings;
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
    }
}
