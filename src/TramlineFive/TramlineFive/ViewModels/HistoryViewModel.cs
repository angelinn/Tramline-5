using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels
{
    public class HistoryViewModel
    {
        public ObservableCollection<HistoryDO> History { get; set; }

        public HistoryViewModel()
        {
            History = new ObservableCollection<HistoryDO>();
        }

        public async Task AddHistoryAsync(string code)
        {
            await HistoryDO.Add(code);
            await LoadHistoryAsync();
        }

        public async Task LoadHistoryAsync()
        {
            History.Clear();

            foreach (HistoryDO history in (await HistoryDO.AllAsync()).Reverse())
                History.Add(history);
        }
    }
}
