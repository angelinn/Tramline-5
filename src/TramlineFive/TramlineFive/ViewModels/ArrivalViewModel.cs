using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.Common.Managers;
using TramlineFive.Common.Models;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.Views.Dialogs;

namespace TramlineFive.ViewModels
{
    public class ArrivalViewModel
    {
        public ObservableCollection<Arrival> Arrivals { get; set; }
        public VirtualTableDO VirtualTable { get; set; }

        public ArrivalViewModel()
        {
            Arrivals = new ObservableCollection<Arrival>();
            VirtualTable = new VirtualTableDO();
        }

        public async Task<bool> GetByStopCode(string stopCode)
        {
            Arrivals.Clear();
            VirtualTable.IsQueried = false;

            List<Arrival> arrivals = await SumcManager.GetByStopAsync(stopCode, typeof(CaptchaDialog));

            if (arrivals != null)
            {

                if (arrivals.Count == 0)
                    return false;

                foreach (Arrival arrival in arrivals)
                    Arrivals.Add(arrival);

                VirtualTable.StopTitle = SumcParser.ParseStopTitle(Arrivals.FirstOrDefault().StopTitle);
                VirtualTable.AsOfTime = "Данни от " + DateTime.Now.ToString("HH:mm");
                VirtualTable.IsQueried = true;
            }

            return true;
        } 
    }
}
