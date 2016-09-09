using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels
{
    public class FavouritesViewModel
    {
        public ObservableCollection<FavouriteDO> Favourites { get; set; }

        public FavouritesViewModel()
        {
            Favourites = new ObservableCollection<FavouriteDO>();
        }

        public async Task LoadFavouritesAsync(bool force = false)
        {
            if (Favourites.Count == 0 || force)
            {
                foreach (FavouriteDO favourite in await FavouriteDO.AllAsync())
                    Favourites.Add(favourite);
            }
        }

        public async Task AddAsync(string code)
        {
            FavouriteDO added = await FavouriteDO.Add(code);
            if (added != null)
                Favourites.Insert(0, added);
        }

        public async Task Remove(FavouriteDO favourite)
        {
            Favourites.Remove(Favourites.Where(f => f.Code == favourite.Code).First());
            await FavouriteDO.Remove(favourite);
        }
    }
}
