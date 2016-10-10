using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels.Wrappers
{
    public class FavouriteViewModel
    {
        public FavouriteViewModel(FavouriteDO domain)
        {
            core = domain;
        }

        public static async Task Remove(FavouriteViewModel favourite)
        {
            await FavouriteDO.Remove(favourite.core);
        }

        public async Task<IEnumerable<LineViewModel>> GetLines()
        {
            return (await core.GetLines()).Select(l => new LineViewModel(l));
        }

        public string Name
        {
            get
            {
                return core.Name;
            }
        }

        public string Direction
        {
            get
            {
                return core.Direction;
            }
        }

        public string Code
        {
            get
            {
                return core.Code;
            }
        }

        private FavouriteDO core;
    }
}
