using EasyBus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus.ViewModels
{
    public class SettingsViewModel : NotifyingViewModel
    {
        public bool PushNotifications
        {
            get
            {
                return SettingsManager.PushNotifications;
            }
            set
            {
                SettingsManager.PushNotifications = value;
                OnPropertyChanged("PushNotifications");
            }
        }

        public bool LiveTile
        {
            get
            {
                return SettingsManager.LiveTile;
            }
            set
            {
                SettingsManager.LiveTile = value;
                OnPropertyChanged("LiveTile");
            }
        }
    }
}
