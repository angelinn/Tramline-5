using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels
{
    public class ScheduleViewModel
    {
        public ObservableCollection<DirectionDO> Directions { get; set; }
        public ObservableCollection<DayDO> Days { get; set; }

        public DirectionDO SelectedDirection { get; set; }
        public DayDO SelectedDay { get; set; }

        public ScheduleViewModel(LineDO line)
        {
            this.line = line;

            Directions = new ObservableCollection<DirectionDO>();
            Days = new ObservableCollection<DayDO>();
        }

        public async Task LoadChoosableData()
        {
            await line.LoadDirections();
            foreach (DirectionDO direction in line.Directions)
            {
                Directions.Add(direction);
                await direction.LoadDays();
            }

            foreach (DayDO day in Directions.First().Days)
                Days.Add(day);
        }

        public bool IsValid()
        {
            return (SelectedDirection != null && SelectedDay != null);
        }

        private LineDO line;
    }
}
