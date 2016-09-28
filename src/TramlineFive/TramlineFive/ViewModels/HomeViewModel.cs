using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public VirtualTableViewModel VirtualTableViewModel { get; private set; }
        public FavouritesViewModel FavouritesViewModel { get; private set; }
        public HistoryViewModel HistoryViewModel { get; private set; }

        public HomeViewModel()
        {
            VirtualTableViewModel = new VirtualTableViewModel();
            FavouritesViewModel = new FavouritesViewModel();
            HistoryViewModel = new HistoryViewModel();

            IsLoadingFavourites = true;
            IsLoadingHistory = true;
        }

        private bool isLoadingFavourites;
        public bool IsLoadingFavourites
        {
            get
            {
                return isLoadingFavourites;
            }
            set
            {
                isLoadingFavourites = value;
                OnPropertyChanged();
            }
        }

        private bool isLoadingHistory;
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
