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
        public LineViewModel SelectedLine { get; set; }

        public StopChooserViewModel()
        {
            FavouritesViewModel = new FavouritesViewModel();
            Lines = new ObservableCollection<LineViewModel>();
        }

        public async Task LoadFavouritesAsync()
        {
            IsLoading = true;
            AreFavouritesVisible = false;

            await FavouritesViewModel.LoadFavouritesAsync();

            AreFavouritesEmpty = FavouritesViewModel.Favourites.Count == 0;
            AreFavouritesVisible = !AreFavouritesEmpty;
            IsLoading = false;
        }

        public async Task LoadAvailableLinesAsync()
        {
            IsLoading = true;

            Lines.Clear();
            OnPropertyChanged("AreLinesVisible");

            
            foreach (LineViewModel line in await SelectedFavourite.GetLines())
                Lines.Add(line);
            
            IsLoading = false;

            OnPropertyChanged("AreLinesVisible");
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

        private bool areFavouritesEmpty;
        public bool AreFavouritesEmpty
        {
            get
            {
                return areFavouritesEmpty;
            }
            set
            {
                areFavouritesEmpty = value;
                OnPropertyChanged();
            }
        }

        private bool areFavouritesVisible;
        public bool AreFavouritesVisible
        {
            get
            {
                return areFavouritesVisible;
            }
            set
            {
                areFavouritesVisible = value;
                OnPropertyChanged();
            }
        }
        

        public bool AreLinesVisible
        {
            get
            {
                return Lines.Count > 0;
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
