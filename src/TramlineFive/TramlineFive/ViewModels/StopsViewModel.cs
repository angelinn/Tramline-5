﻿using System;
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

            foreach (StopViewModel stop in scheduleChooserViewModel.SelectedDay.Stops)
                LineStops.Add(stop);
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
