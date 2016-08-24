using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.Common.Models;
using TramlineFive.Views.Dialogs;

namespace TramlineFive.ViewModels
{
    public class ArrivalViewModel
    {
        public ObservableCollection<Arrival> Arrivals { get; set; }
        public StringViewModel StopTitle { get; set; }
        public StringViewModel AsOfTime { get; set; }

        public ArrivalViewModel()
        {
            Arrivals = new ObservableCollection<Arrival>();
            StopTitle = new StringViewModel();
            AsOfTime = new StringViewModel();
        }

        public async Task<bool> GetByStopCode(string stopCode)
        {
            Arrivals.Clear();
            StopTitle.Source = String.Empty;
            AsOfTime.Source = String.Empty;

            IEnumerable<Arrival> arrivals = await SumcManager.GetByStopAsync(stopCode, typeof(CaptchaDialog));

            if (arrivals?.Count() == 0)
                return false;

            foreach (Arrival arrival in arrivals ?? Enumerable.Empty<Arrival>())
                Arrivals.Add(arrival);

            StopTitle.Source = SumcParser.ParseStopTitle(Arrivals.FirstOrDefault()?.StopTitle);
            AsOfTime.Source = "Данни от " + DateTime.Now.ToString("HH:mm");

            return true;
        } 
    }
}
