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
        public string Code { get; set; }

        public string Name { get; set; }

        public string Direction { get; set; }

        public FavouriteDO() { }

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
                    Favourite existing = uow.Favourites.All()
                                                       .IncludeMultiple(f => f.Stop)
                                                       .Where(s => s.Stop.Code == code)
                                                       .FirstOrDefault();
                    if (existing != null)
                        return null;

                    int intCode;
                    if (!Int32.TryParse(code, out intCode))
                        return null;

                    Favourite favourite = new Favourite
                    {
                        StopID = uow.Stops.Where(s => s.Code == code).First().ID
                    };

                    uow.Favourites.Add(favourite);
                    uow.Save();

                    return new FavouriteDO(uow.Favourites.Where(f => f.ID == favourite.ID).IncludeMultiple(f => f.Stop, f => f.Stop.Day, f => f.Stop.Day.Direction).First());
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
