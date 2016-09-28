﻿using System;
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
            IsLoadingFavourites = true;

            if (Favourites.Count == 0 || force)
            {
                foreach (FavouriteDO favourite in await FavouriteDO.AllAsync())
                    Favourites.Add(new FavouriteViewModel(favourite));
            }

            IsLoadingFavourites = false;
            OnPropertyChanged("IsEmpty");
        }

        public async Task AddAsync(string code)
        {
            FavouriteDO added = await FavouriteDO.Add(code);
            if (added != null)
                Favourites.Insert(0, new FavouriteViewModel(added));

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
                return (Favourites.Count == 0 && !IsLoadingFavourites);
            }
        }

        private bool isLoadingFavourites = true;
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
    }
}
