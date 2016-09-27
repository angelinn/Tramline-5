using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.ViewModels
{
    public class HomeViewModel
    {
        public VirtualTableViewModel VirtualTableViewModel { get; private set; }
        public FavouritesViewModel FavouritesViewModel { get; private set; }
        public HistoryViewModel HistoryViewModel { get; private set; }
        public VersionViewModel VersionViewModel { get; private set; }

        public HomeViewModel()
        {
            VirtualTableViewModel = new VirtualTableViewModel();
            FavouritesViewModel = new FavouritesViewModel();
            HistoryViewModel = new HistoryViewModel();
            VersionViewModel = new VersionViewModel();
        }
    }
}
