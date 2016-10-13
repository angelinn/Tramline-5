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
        public void Update()
        {
            OnPropertyChanged("Favourite");
            OnPropertyChanged("FavouriteExists");
        }

        public async Task ClearHistoryAsync()
        {
            IsClearingHistory = true;

            await HistoryDO.ClearAllAsync();

            IsClearingHistory = false;
        }

        public async Task<string> ExportFavourites()
        {
            return JsonConvert.SerializeObject(await FavouriteDO.AllAsync());
        }

        public async Task ImportFavourites(string json)
        {
            IEnumerable<FavouriteDO> backuped = JsonConvert.DeserializeObject<IEnumerable<FavouriteDO>>(json);
            foreach (FavouriteDO favourite in backuped)
                await FavouriteDO.Add(favourite.Code);
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(stopCode);
        }

        public bool ArePushNotificationsEnabled
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

        private bool isLiveTileEnabled;
        public bool IsLiveTileEnabled
        {
            get
            {
                return isLiveTileEnabled;
            }
            set
            {
                isLiveTileEnabled = value;
                OnPropertyChanged();
            }
        }

        public string FavouriteName { get; set; }
        public string FavouriteNumber { get; set; }
        public VehicleType FavouriteType { get; set; }
        
        public string Favourite
        {
            get
            {
                return (FavouriteName == null) ? null : $"{VehicleTypeManager.Stringify(FavouriteType)} №{FavouriteNumber} - {FavouriteName}";
            }
        }

        public bool FavouriteExists
        {
            get
            {
                return !String.IsNullOrEmpty(Favourite);
            }
        }

        private string stopCode;
        public string StopCode
        {
            get
            {
                return stopCode;
            }
            set
            {
                stopCode = value;
                OnPropertyChanged();
            }
        }

        private bool isClearingHistory;
        public bool IsClearingHistory
        {
            get
            {
                return isClearingHistory;
            }
            set
            {
                isClearingHistory = value;
                OnPropertyChanged();
            }
        }

        private bool isSwitchable = true;
        public bool IsSwitchable
        {
            get
            {
                return isSwitchable;
            }
            set
            {
                isSwitchable = value;
                OnPropertyChanged();
            }
        }
    }
}
