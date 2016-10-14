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
        public IList<HistoryEntryViewModel> History { get; private set; }

        public HistoryViewModel()
        {
            History = new ObservableCollection<HistoryEntryViewModel>();
        }

        public async Task AddHistoryAsync()
        {
            IsAdding = true;

            string code = (App.Current as App).AppViewModel.StopCode;
            int intCode;

            if (code.Length == 4 && Int32.TryParse(code, out intCode))
            {
                HistoryDO added = await HistoryDO.Add(code);
                History.Insert(0, new HistoryEntryViewModel(added));
            }

            IsAdding = false;

            OnPropertyChanged("IsEmpty");
        }

        public async Task LoadHistoryAsync()
        {
            IsLoading = true;
            History.Clear();

            foreach (HistoryDO history in (await HistoryDO.AllAsync()).Reverse())
                History.Add(new HistoryEntryViewModel(history));

            IsLoading = false;

            OnPropertyChanged("IsEmpty");
        }

        public bool IsEmpty
        {
            get
            {
                return (History.Count == 0 && !IsLoading);
            }
        }

        private bool isLoading = true;
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

        private bool isAdding;
        public bool IsAdding
        {
            get
            {
                return isAdding;
            }
            set
            {
                isAdding = value;
                OnPropertyChanged();
            }
        }
    }
}
