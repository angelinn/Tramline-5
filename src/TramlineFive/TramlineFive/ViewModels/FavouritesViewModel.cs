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
    public class FavouritesViewModel
    {
        public IList<FavouriteViewModel> Favourites { get; set; }

        public FavouritesViewModel()
        {
            Favourites = new ObservableCollection<FavouriteViewModel>();
        }

        public async Task LoadFavouritesAsync(bool force = false)
        {
            if (Favourites.Count == 0 || force)
            {
                foreach (FavouriteDO favourite in await FavouriteDO.AllAsync())
                    Favourites.Add(new FavouriteViewModel(favourite));
            }
        }

        public async Task AddAsync(string code)
        {
            FavouriteDO added = await FavouriteDO.Add(code);
            if (added != null)
                Favourites.Insert(0, new FavouriteViewModel(added));
        }

        public async Task Remove(FavouriteViewModel favourite)
        {
            Favourites.Remove(Favourites.Where(f => f.Code == favourite.Code).First());
            await FavouriteViewModel.Remove(favourite);
        }
    }
}
