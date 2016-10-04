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
    public class HistoryDO
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public DateTime? TimeStamp { get; private set; }

        public HistoryDO(History entity)
        {
            id = entity.ID;
            Code = entity.Stop.Code;
            Name = entity.Stop.Name;
            TimeStamp = entity.TimeStamp;
        }

        public static async Task<HistoryDO> Add(int intCode)
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    History history = new History
                    {
                        StopID = uow.Stops.Where(s => s.Code == intCode.ToString()).First().ID,
                        TimeStamp = DateTime.Now
                    };

                    uow.HistoryEntries.Add(history);
                    uow.Save();

                    return new HistoryDO(history);
                };

            });
        }

        public static async Task Remove(HistoryDO history)
        {
            await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.HistoryEntries.Delete(history.id);
                    uow.Save();
                }
            });
        }

        public static async Task<IEnumerable<HistoryDO>> AllAsync()
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    return uow.HistoryEntries.All().IncludeMultiple(h => h.Stop).ToList().Select(h => new HistoryDO(h));
                }
            });
        }

        public static async Task ClearAllAsync()
        {
            await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.HistoryEntries.Clear();
                    uow.Save();
                }
            });
        }

        private int id;
    }
}
