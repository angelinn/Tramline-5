using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Repositories;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class DirectionDO
    {
        public DirectionDO(Direction entity)
        {
            id = entity.ID;
            name = entity.Name;
            days = entity.Days?.Select(d => new DayDO(d));
        }

        public static async Task<IEnumerable<DirectionDO>> GetByLineId(int lineId)
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IEnumerable<Direction> directions = uow.Directions.Where(d => d.LineID == lineId).ToList();
                    return directions?.Select(d => new DirectionDO(d));
                }
            });
        }

        public async Task LoadDays()
        {
            days = await DayDO.GetByDirectionId(id);
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

        private IEnumerable<DayDO> days;
        public IEnumerable<DayDO> Days
        {
            get
            {
                return days;
            }
        }
    }
}
