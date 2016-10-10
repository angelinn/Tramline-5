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
            AreFavouritesVisible = false;

            await FavouritesViewModel.LoadFavouritesAsync();

            IsLoading = false;
            AreFavouritesVisible = true;
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

        public bool AreFavouritesEmpty
        {
            get
            {
                return FavouritesViewModel.IsEmpty;
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
