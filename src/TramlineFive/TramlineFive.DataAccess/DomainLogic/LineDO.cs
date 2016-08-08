using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;
using TramlineFive.DataAccess.Repositories;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class LineDO
    {
        public LineDO(Line entity)
        {
            name = entity.Name;
            directions = entity.Directions;
        }

        public static async Task<IEnumerable<LineDO>> AllAsync()
        {
            return await Task.Run(() =>
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    IEnumerable<Line> lines = uow.Lines.All().ToList();
                    return lines == null ? null : lines.Select(l => new LineDO(l));
                }
            });
        }

        private string name;
        public string Name { get; }

        private List<Direction> directions;
        public List<Direction> Directions { get; set; }
    }
}
