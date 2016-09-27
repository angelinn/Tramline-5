using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class ScheduleChooserViewModel
    {
        public IList<DirectionViewModel> Directions { get; private set; }
        public IList<DayViewModel> Days { get; private set; }
        
        public DirectionViewModel SelectedDirection { get; set; }
        public DayViewModel SelectedDay { get; set; }
        public LineViewModel SelectedLine { get; set; }

        public ScheduleChooserViewModel()
        {
            Directions = new ObservableCollection<DirectionViewModel>();
            Days = new ObservableCollection<DayViewModel>();
        }

        public ScheduleChooserViewModel(LineViewModel line)
        {
            SelectedLine = line;

            Directions = new ObservableCollection<DirectionViewModel>();
            Days = new ObservableCollection<DayViewModel>();
        }

        public async Task LoadChoosableData()
        {
            await SelectedLine.LoadDirections();
            foreach (DirectionViewModel direction in SelectedLine.Directions)
            {
                Directions.Add(direction);
                await direction.LoadDays();
            }

            foreach (DayViewModel day in Directions.First().Days)
                Days.Add(day);
        }

        public bool IsValid()
        {
            return (SelectedDirection != null && SelectedDay != null);
        }

        public void UpdateFrom(ScheduleChooserViewModel other)
        {
            SelectedLine = other.SelectedLine;
        }
    }
}
