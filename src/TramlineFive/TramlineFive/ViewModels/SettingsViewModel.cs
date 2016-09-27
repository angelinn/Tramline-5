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
    }
}
