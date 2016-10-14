using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class StopsViewModel
    {
        public IList<StopViewModel> LineStops { get; private set; }

        public StopsViewModel(ScheduleChooserViewModel scheduleChooser)
        {
            scheduleChooserViewModel = scheduleChooser;
            LineStops = new ObservableCollection<StopViewModel>();
        }

        public async Task LoadStops()
        {
            await scheduleChooserViewModel.SelectedDay.LoadStops();

            LineStops = scheduleChooserViewModel.SelectedDay.Stops.OrderBy(s => s.Index).ToList();
        }

        public string Title
        {
            get
            {
                return $"{scheduleChooserViewModel.SelectedLine.ShortName} - {scheduleChooserViewModel.SelectedDirection.Name.ToUpper()}";
            }
        }

        private ScheduleChooserViewModel scheduleChooserViewModel;
    }
}
