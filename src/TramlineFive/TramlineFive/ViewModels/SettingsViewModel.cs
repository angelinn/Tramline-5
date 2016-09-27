using TramlineFive.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;
using TramlineFive.DataAccess.Repositories;
using TramlineFive.Common.Managers;
using TramlineFive.ViewModels.Wrappers;
using Newtonsoft.Json;
using TramlineFive.Common.Models;

namespace TramlineFive.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public bool PushNotifications
        {
            get
            {
                string value = SettingsManager.ReadValue(SettingsKeys.PushNotifications);
                if (value == null)
                    return false;

                return Boolean.Parse(value);
            }
            set
            {
                SettingsManager.UpdateValue(SettingsKeys.PushNotifications, value);
                OnPropertyChanged();
            }
        }

        public bool LiveTile
        {
            get
            {
                string value = SettingsManager.ReadValue(SettingsKeys.LiveTile);
                if (value == null)
                    return false;

                return Boolean.Parse(value);
            }
            set
            {
                SettingsManager.UpdateValue(SettingsKeys.LiveTile, value);
                OnPropertyChanged();
            }
        }

        public async Task ClearHistoryAsync()
        {
            await HistoryDO.ClearAllAsync();
        }

        public async Task<string> GetSerializedFavourites()
        {
            return JsonConvert.SerializeObject(await FavouriteDO.AllAsync());
        }

        public async Task<bool> DoesStopExist(VehicleType type, string line, string code)
        {
            return await LineDO.DoesStopAt(type, line, code);
        }
    }
}
