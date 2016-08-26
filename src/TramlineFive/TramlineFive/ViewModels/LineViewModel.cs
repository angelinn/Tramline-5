using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels
{
    public class LineViewModel : NotifyingViewModel
    {
        private IEnumerable<LineDO> lines;
        public IEnumerable<LineDO> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
                OnPropertyChanged("Lines");
            }
        }

        private IEnumerable<IGrouping<string, LineDO>> grouped;
        public IEnumerable<IGrouping<string, LineDO>> Grouped
        {
            get
            {
                return grouped;
            }
            set
            {
                grouped = value;
                OnPropertyChanged("Grouped");
            }
        }
    }
}
