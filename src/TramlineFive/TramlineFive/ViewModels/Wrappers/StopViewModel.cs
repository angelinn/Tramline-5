using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels.Wrappers
{
    public class StopViewModel
    {
        public StopViewModel(StopDO domain)
        {
            core = domain;
        }

        public string Name
        {
            get
            {
                return core.Name;
            }
        }

        public string Code
        {
            get
            {
                return core.Code;
            }
        }

        public List<string> Timings
        {
            get
            {
                return core.Timings;
            }
        }

        private StopDO core;
    }
}
