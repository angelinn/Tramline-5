using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.Common.Managers;
using TramlineFive.Common.Models;
using TramlineFive.ViewModels.Wrappers;
using TramlineFive.Views.Dialogs;

namespace TramlineFive.ViewModels
{
    public class VirtualTableViewModel : BaseViewModel
    {
        public IList<Arrival> Arrivals { get; set; }

        public VirtualTableViewModel()
        {
            Arrivals = new ObservableCollection<Arrival>();
        }
        public async Task<bool> GetByStopCode(string stopCode)
        {
            IsLoading = true;
            Arrivals.Clear();
            IsQueried = false;

            List<Arrival> arrivals = await SumcManager.GetByStopAsync(stopCode, typeof(CaptchaDialog));

            if (arrivals != null)
            {

                if (arrivals.Count == 0)
                {
                    IsLoading = false;
                    return false;
                }

                foreach (Arrival arrival in arrivals)
                    Arrivals.Add(arrival);

                StopTitle = ParseManager.ParseStopTitle(Arrivals.FirstOrDefault().StopTitle);
                AsOfTime = $"{Formats.DataFromTime} {DateTime.Now.ToString("HH:mm")}";
                IsQueried = true;
            }

            IsLoading = false;
            return true;
        }

        private string stopTitle;
        public string StopTitle
        {
            get
            {
                return stopTitle;
            }
            set
            {
                stopTitle = value;
                OnPropertyChanged();
            }
        }

        private string asOfTime;
        public string AsOfTime
        {
            get
            {
                return asOfTime;
            }
            set
            {
                asOfTime = value;
                OnPropertyChanged();
            }
        }

        private bool isQueried;
        public bool IsQueried
        {
            get
            {
                return isQueried;
            }
            set
            {
                isQueried = value;
                OnPropertyChanged();
            }
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
    }
}
