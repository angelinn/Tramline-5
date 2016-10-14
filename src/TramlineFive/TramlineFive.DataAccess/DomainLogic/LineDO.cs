using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Managers;
using TramlineFive.Common.Models;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Extensions;
using TramlineFive.DataAccess.Repositories;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class LineDO
    {
        public int Number { get; private set; }
        public string NumberString { get; private set; }
        public VehicleType Type { get; private set; }
        public IEnumerable<DirectionDO> Directions { get; private set; }

        public LineDO(Line entity)
        {
            id = entity.ID;
            NumberString = WebUtility.UrlDecode(entity.Number);
            Directions = entity.Directions?.Select(d => new DirectionDO(d)).ToList();
            Type = entity.Type;

            int tempNum;
            if (Int32.TryParse(NumberString, out tempNum))
                Number = tempNum;
            else
                Number = Int32.Parse(NumberString[0].ToString());
        }

        public static async Task<IEnumerable<LineDO>> AllAsync()
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IEnumerable<Line> lines = uow.Lines.All().IncludeMultiple(l => l.Directions).ToList();
                    return lines?.Select(l => new LineDO(l));
                }
            });
        }

        public async Task LoadDirections()
        {
            Directions = await DirectionDO.GetByLineId(id);
        }

        public static async Task<List<StopDO>> FetchByVehicleAsync(VehicleType type, string number)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Line chosen = uow.Lines.Where(l => l.Type == type && l.Number == number).FirstOrDefault();
                if (chosen == null)
                    return null;

                List<StopDO> result = new List<StopDO>();

                LineDO domain = new LineDO(chosen);

                await domain.LoadDirections();

                foreach (DirectionDO direction in domain.Directions)
                {
                    await direction.LoadDays();

                    DayDO first = direction.Days.First();
                    await first.LoadStops();
                    result.AddRange(first.Stops);
                }

                return result;
            }
        }


        public static async Task<bool> DoesStopAt(VehicleType type, string number, string stopCode)
        {
            List<StopDO> stops = await FetchByVehicleAsync(type, number);
            return stops.Any(s => s.Code == stopCode);
        }

        private int id;
    }
}
