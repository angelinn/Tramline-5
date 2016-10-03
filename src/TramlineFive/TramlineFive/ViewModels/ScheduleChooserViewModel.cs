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

        public LineViewModel SelectedLine { get; set; }

        public ScheduleChooserViewModel()
        {
            Directions = new ObservableCollection<DirectionViewModel>();
            Days = new ObservableCollection<DayViewModel>();
        }

        public async Task LoadChoosableData()
        {
            IsLoading = true;

            Directions.Clear();
            Days.Clear();

            await SelectedLine.LoadDirections();
            foreach (DirectionViewModel direction in SelectedLine.Directions)
            {
                Directions.Add(direction);
                await direction.LoadDays();
            }

            SelectedDirection = Directions.First();

            IsLoading = false;
        }

        public bool IsValid()
        {
            return (SelectedDirection != null && SelectedDay != null);
        }

        private void UpdateDays()
        {
            Days.Clear();
            foreach (DayViewModel day in SelectedDirection.Days)
                Days.Add(day);

            SelectedDay = Days.FirstOrDefault();
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

                UpdateDays();
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
