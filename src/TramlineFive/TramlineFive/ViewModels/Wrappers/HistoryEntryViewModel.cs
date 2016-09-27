using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels.Wrappers
{
    public class HistoryEntryViewModel
    {
        public HistoryEntryViewModel(HistoryDO domain)
        {
            core = domain;
        }

        public string Code
        {
            get
            {
                return core.Code;
            }
        }

        public string Name
        {
            get
            {
                return core.Name;
            }
        }

        public DateTime? TimeStamp
        {
            get
            {
                return core.TimeStamp;
            }
        }

        private HistoryDO core;
    }
}
