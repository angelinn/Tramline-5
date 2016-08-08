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
        public DayDO(Day entity)
        {
            id = entity.ID;
            type = entity.Type;
            stops = entity.Stops?.Select(s => new StopDO(s));
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
            stops = await StopDO.GetFromDayId(id);
        }

        private int id;

        private string type;
        public string Type
        {
            get
            {
                return type;
            }
        }

        private IEnumerable<StopDO> stops;
        public IEnumerable<StopDO> Stops
        {
            get
            {
                return stops;
            }
        } 
    }
}
