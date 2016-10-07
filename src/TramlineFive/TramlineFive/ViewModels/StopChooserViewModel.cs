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
            await FavouritesViewModel.LoadFavouritesAsync();
        }

        public async Task LoadAvailableLinesAsync()
        {
            foreach (LineViewModel line in await SelectedFavourite.GetLines())
                Lines.Add(line);
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
        
        public bool IsEmpty
        {
            get
            {
                return FavouritesViewModel.IsEmpty;
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
