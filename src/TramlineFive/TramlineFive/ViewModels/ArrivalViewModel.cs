using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.Common.Models;
using TramlineFive.Views.Dialogs;
using Windows.UI.Popups;

namespace TramlineFive.ViewModels
{
    public class ArrivalViewModel
    {
        public ObservableCollection<Arrival> Arrivals { get; set; }
        public StringViewModel StopTitle { get; set; }

        public ArrivalViewModel()
        {
            Arrivals = new ObservableCollection<Arrival>();
            StopTitle = new StringViewModel();
        }

        public async Task<bool> GetByStopCode(string stopCode)
        {
            Arrivals.Clear();
            StopTitle.Source = String.Empty;

            VirtualTable table = await SumcManager.GetByStopAsync(stopCode, new CaptchaDialog());

            if (table?.Arrivals.Count() == 0)
                return false;

            foreach (Arrival arrival in table.Arrivals ?? Enumerable.Empty<Arrival>())
                Arrivals.Add(arrival);

            StopTitle.Source = SumcParser.ParseStopTitle(Arrivals.FirstOrDefault()?.StopTitle);

            return true;
        } 
    }
}
