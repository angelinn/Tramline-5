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
        public HistoryDO(History entity)
        {
            code = entity.Stop.Code;
            name = entity.Stop.Name;
            id = entity.ID;
            timeStamp = entity.TimeStamp;
        }

        public static async Task Add(string code)
        {
            await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    int intCode = Int32.Parse(code);

                    History history = new History
                    {
                        StopID = uow.Stops.Where(s => s.Code == intCode.ToString()).First().ID,
                        TimeStamp = DateTime.Now
                    };

                    uow.HistoryEntries.Add(history);
                    uow.Save();
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

        private string code;
        public string Code
        {
            get
            {
                return code;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        private DateTime? timeStamp;
        public DateTime? TimeStamp
        {
            get
            {
                return timeStamp;
            }
        }
    }
}
