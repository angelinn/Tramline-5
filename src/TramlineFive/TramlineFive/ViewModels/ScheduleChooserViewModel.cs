using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Managers;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class ScheduleChooserViewModel : BaseViewModel
    {
        public IList<DirectionViewModel> Directions { get; private set; }
        public IList<DayViewModel> Days { get; private set; }
        public IList<StopViewModel> Stops { get; private set; }

        public LineViewModel SelectedLine { get; set; }

        public ScheduleChooserViewModel(LineViewModel line = null)
        {
            SelectedLine = line;

            Directions = new ObservableCollection<DirectionViewModel>();
            Days = new ObservableCollection<DayViewModel>();
            Stops = new ObservableCollection<StopViewModel>();
        }

        public async Task LoadChoosableData()
        {
            IsLoading = true;

            await SelectedLine.LoadDirections();
            foreach (DirectionViewModel direction in SelectedLine.Directions)
            {
                Directions.Add(direction);
                await direction.LoadDays();
            }

            foreach (DayViewModel day in Directions.First().Days)
                Days.Add(day);

            SelectedDirection = Directions.First();
            SelectedDay = Days.First();

            IsLoading = false;
        }

        public bool IsValid()
        {
            return (SelectedDirection != null && SelectedDay != null);
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

        private DirectionViewModel selectedDirection;
        public DirectionViewModel SelectedDirection
        {
            get
            {
                return selectedDirection;
            }
            set
            {
                selectedDirection = value;
                OnPropertyChanged();
            }
        }

        private DayViewModel selectedDay;
        public DayViewModel SelectedDay
        {
            get
            {
                return selectedDay;
            }
            set
            {
                selectedDay = value;
                OnPropertyChanged();
            }
        }
                            
        public string Title
        {
            get
            {
                return $"{VehicleTypeManager.Stringify(SelectedLine.Type).ToUpper()} №{SelectedLine.Number} - МАРШРУТИ";
            }
        }
    }
}
