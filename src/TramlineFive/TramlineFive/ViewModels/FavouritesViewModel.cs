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

        public async Task LoadFavourites()
        {
            foreach (FavouriteDO favourite in await FavouriteDO.AllAsync())
                Favourites.Add(favourite);
        }

        public async Task Add(string code)
        {
            await FavouriteDO.Add(code);
        }

        public async Task Remove(FavouriteDO favourite)
        {
            Favourites.Remove(Favourites.Where(f => f.Code == favourite.Code).First());
            await FavouriteDO.Remove(favourite);
        }
    }
}
