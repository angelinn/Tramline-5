using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Managers;
using TramlineFive.Common.Models;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class AllLinesViewModel : BaseViewModel
    {
        public async Task LoadAndGroupLinesAsync()
        {
            IsLoading = true;

            Lines = (await LineDO.AllAsync()).Select(l => new LineViewModel(l))
                                                .Where(l => l.Type != VehicleType.None)
                                                .OrderBy(l => l.SortID)
                                                .ThenBy(l => l.Number);

            Grouped = Lines.GroupBy(l => VehicleTypeManager.Stringify(l.Type, true));

            IsLoading = false;
        }

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
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
