using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class AllLinesViewModel : BaseViewModel
    {
        public async Task<IEnumerable<LineViewModel>> AllAsync()
        {
            return (await LineDO.AllAsync()).Select(l => new LineViewModel(l));
        }

        private IEnumerable<LineViewModel> lines;
        public IEnumerable<LineViewModel> Lines
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

        private IEnumerable<IGrouping<string, LineViewModel>> grouped;
        public IEnumerable<IGrouping<string, LineViewModel>> Grouped
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
