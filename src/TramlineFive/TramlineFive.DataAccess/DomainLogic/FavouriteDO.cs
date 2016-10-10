using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Extensions;
using TramlineFive.DataAccess.Repositories;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class FavouriteDO
    {
        public string Code { get; private set; }

        public string Name { get; private set; }

        public string Direction { get; private set; }

        public FavouriteDO(Favourite entity)
        {
            Code = entity.Stop.Code;
            Name = entity.Stop.Name;
            id = entity.ID;
            Direction = entity.Stop.Day?.Direction.Name;
        }

        public static async Task<FavouriteDO> Add(string code)
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    int intCode = Int32.Parse(code);
                    Favourite existing = uow.Favourites.All()
                                                       .IncludeMultiple(f => f.Stop)
                                                       .Where(s => s.Stop.Code == intCode.ToString())
                                                       .FirstOrDefault();
                    if (existing != null)
                        return null;

                    Favourite favourite = new Favourite
                    {
                        StopID = uow.Stops.Where(s => s.Code == intCode.ToString()).First().ID
                    };

                    uow.Favourites.Add(favourite);
                    uow.Save();

                    return new FavouriteDO(favourite);
                };
            });
        }

        public static async Task Remove(FavouriteDO favourite)
        {
            await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.Favourites.Delete(favourite.id);
                    uow.Save();
                }
            });
        }

        public static async Task<IEnumerable<FavouriteDO>> AllAsync()
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    return uow.Favourites.All().IncludeMultiple(f => f.Stop, f => f.Stop.Day, f => f.Stop.Day.Direction).ToList().Select(f => new FavouriteDO(f));
                }
            });
        }

        public async Task<IEnumerable<LineDO>> GetLines()
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    return new StopDO(uow.Favourites.Where(f => f.ID == id).IncludeMultiple(f => f.Stop).First().Stop).FetchLinesAsync();
                }
            });
        }

        private int id;
    }
}
