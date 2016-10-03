using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Repositories;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class DayDO
    {
        public string Type { get; set; }

        public IEnumerable<StopDO> Stops { get; set; }

        public DayDO(Day entity)
        {
            id = entity.ID;
            Type = entity.Type;
            Stops = entity.Stops?.Select(s => new StopDO(s));
        }

        public static async Task<IEnumerable<DayDO>> GetByDirectionId(int dirId)
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var l = uow.Days.Where(d => d.DirectionID == dirId).ToList();
                    return l?.Select(e => new DayDO(e));
                }
            });
        }

        public async Task LoadStops()
        {
            Stops = await StopDO.GetFromDayId(id);
        }

        private int id;
        
    }
}
