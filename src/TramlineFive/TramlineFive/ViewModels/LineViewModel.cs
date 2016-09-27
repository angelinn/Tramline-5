using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class LineViewModel : BaseViewModel
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }
    }
}
