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
    public class FavouritesViewModel : BaseViewModel
    {
        public IList<FavouriteViewModel> Favourites { get; set; }

        public FavouritesViewModel()
        {
            Favourites = new ObservableCollection<FavouriteViewModel>();
        }

        public async Task LoadFavouritesAsync(bool force = false)
        {
            IsLoading = true;

            if (Favourites.Count == 0 || force)
            {
                foreach (FavouriteDO favourite in await FavouriteDO.AllAsync())
                    Favourites.Add(new FavouriteViewModel(favourite));
            }

            IsLoading = false;
            OnPropertyChanged("IsEmpty");
        }

        public async Task AddAsync()
        {
            IsAdding = true;

            FavouriteDO added = await FavouriteDO.Add((App.Current as App).AppViewModel.StopCode);
            if (added != null)
                Favourites.Insert(0, new FavouriteViewModel(added));

            IsAdding = false;

            OnPropertyChanged("IsEmpty");
        }

        public async Task Remove(FavouriteViewModel favourite)
        {
            Favourites.Remove(Favourites.Where(f => f.Code == favourite.Code).First());
            await FavouriteViewModel.Remove(favourite);

            OnPropertyChanged("IsEmpty");
        }

        public bool IsEmpty
        {
            get
            {
                return (Favourites.Count == 0 && !IsLoading);
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
