using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels.Wrappers
{
    public class DirectionViewModel
    {
        public DirectionViewModel(DirectionDO domain)
        {
            core = domain;
        }

        public async Task LoadDays()
        {
            await core.LoadDays();
        }

        public string Name
        {
            get
            {
                return core.Name;
            }
        }

        public IEnumerable<DayDO> Days
        {
            get
            {
                return core.Days;
            }
        }

        private DirectionDO core;
    }
}
