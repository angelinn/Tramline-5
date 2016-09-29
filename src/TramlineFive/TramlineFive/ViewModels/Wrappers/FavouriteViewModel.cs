﻿using System;
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

        public string Name
        {
            get
            {
                return core.Name;
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