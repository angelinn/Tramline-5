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
    public class HistoryViewModel : BaseViewModel
    {
        public IList<HistoryEntryViewModel> History { get; set; }

        public HistoryViewModel()
        {
            History = new ObservableCollection<HistoryEntryViewModel>();
        }

        public async Task AddHistoryAsync(string code)
        {
            HistoryDO added = await HistoryDO.Add(code);
            History.Insert(0, new HistoryEntryViewModel(added));

            OnPropertyChanged("IsEmpty");
        }

        public async Task LoadHistoryAsync()
        {
            IsLoadingHistory = true;
            History.Clear();

            foreach (HistoryDO history in (await HistoryDO.AllAsync()).Reverse())
                History.Add(new HistoryEntryViewModel(history));

            IsLoadingHistory = false;

            OnPropertyChanged("IsEmpty");
        }

        public bool IsEmpty
        {
            get
            {
                return (History.Count == 0 && !IsLoadingHistory);
            }
        }

        private bool isLoadingHistory = true;
        public bool IsLoadingHistory
        {
            get
            {
                return isLoadingHistory;
            }
            set
            {
                isLoadingHistory = value;
                OnPropertyChanged();
            }
        }
    }
}
