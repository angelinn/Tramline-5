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
        public LineDO(Line entity)
        {
            id = entity.ID;
            numberString = WebUtility.UrlDecode(entity.Number);
            directions = entity.Directions?.Select(d => new DirectionDO(d)).ToList();
            type = entity.Type;

            int tempNum;
            if (Int32.TryParse(numberString, out tempNum))
                number = tempNum;
            else
                number = Int32.Parse(numberString[0].ToString());
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
            directions = await DirectionDO.GetByLineId(id);
        }

        public static async Task<IEnumerable<StopDO>> FetchByVehicleAsync(VehicleType type, string number)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Line chosen = uow.Lines.Where(l => l.Type == type && l.Number == number).FirstOrDefault();
                if (chosen == null)
                    return null;

                LineDO domain = new LineDO(chosen);

                await domain.LoadDirections();
                DirectionDO direction = domain.directions.First();
                await direction.LoadDays();
                DayDO day = direction.Days.First();
                await day.LoadStops();

                return day.Stops;
            }
        }

        private int id;

        private VehicleType type;
        public VehicleType Type
        {
            get
            {
                return type;
            }
        }

        private int number;
        public int Number
        {
            get
            {
                return number;
            }
        }

        private string numberString;
        public string NumberString
        {
            get
            {
                return numberString;
            }
        }

        private IEnumerable<DirectionDO> directions;
        public IEnumerable<DirectionDO> Directions
        {
            get
            {
                return directions;
            }
        }

        public string TypeToString(bool plural = false)
        {
            return VehicleTypeManager.TypeToString(type, plural);
        }

        public override string ToString()
        {
            string stringType = TypeToString();

            return $"{stringType} {numberString}";
        }

        public int SortID
        {
            get
            {
                switch (type)
                {
                    case VehicleType.Bus:
                        return 3;
                    case VehicleType.Tram:
                        return 1;
                    case VehicleType.Trolley:
                        return 2;
                    default:
                        return 4;
                }
            } 
        }
    }
}
