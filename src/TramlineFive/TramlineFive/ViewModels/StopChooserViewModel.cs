using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.ViewModels.Wrappers;

namespace TramlineFive.ViewModels
{
    public class StopChooserViewModel : BaseViewModel
    {

        public IList<LineViewModel> Lines { get; private set; }
        public FavouritesViewModel FavouritesViewModel { get; private set; }
        public FavouriteViewModel SelectedFavourite { get; set; }
        public string Code { get; set; }

        public StopChooserViewModel()
        {
            FavouritesViewModel = new FavouritesViewModel();
            Lines = new ObservableCollection<LineViewModel>();
            FavouritesViewModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        public async Task LoadFavouritesAsync()
        {
            IsLoading = true;

            await FavouritesViewModel.LoadFavouritesAsync();

            IsLoading = false;
        }

        public async Task LoadAvailableLinesAsync()
        {
            IsLoadingStops = true;

            Lines.Clear();
            Favourites.Clear();

            foreach (LineViewModel line in await SelectedFavourite.GetLines())
                Lines.Add(line);

            IsLoadingStops = false;

            OnPropertyChanged("AreLinesEmpty");
            OnPropertyChanged("IsEmpty");
        }        

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading || FavouritesViewModel.IsLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool AreFavouritesEmpty
        {
            get
            {
                return FavouritesViewModel.IsEmpty;
            }
        }
        
        public bool AreFavouritesVisible
        {
            get
            {
                return FavouritesViewModel.IsEmpty;
            }
        }

        public bool AreLinesVisible
        {
            get
            {

            }
        }

        public bool AreLinesEmpty
        {
            get
            {
                return Lines.Count == 0;
            }
        }

        private bool isLoadingStops;
        public bool IsLoadingStops
        {
            get
            {
                return isLoadingStops;
            }
            set
            {
                isLoadingStops = value;
                OnPropertyChanged();
            }
        }

        public IList<FavouriteViewModel> Favourites
        {
            get
            {
                return FavouritesViewModel.Favourites;
            }
        }
    }
}
