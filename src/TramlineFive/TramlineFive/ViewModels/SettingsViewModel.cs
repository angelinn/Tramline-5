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
        public List<NameValueObject> VehicleTypes { get; private set; }
        public string VehicleTypeKey { get; private set; }
        public string VehicleTypeValue { get; private set; }

        public SettingsViewModel()
        {
            VehicleTypes = VehicleTypeManager.GetNameValuePair();
            VehicleTypeKey = "Name";
            VehicleTypeValue = "Value";
        }

        public async Task ClearHistoryAsync()
        {
            IsClearingHistory = true;

            await HistoryDO.ClearAllAsync();

            IsClearingHistory = false;
        }

        public async Task<string> GetSerializedFavourites()
        {
            return JsonConvert.SerializeObject(await FavouriteDO.AllAsync());
        }

        public bool IsValid()
        {
            return LiveTile || (!String.IsNullOrEmpty(StopCode) && !String.IsNullOrEmpty(LineNumber));
        }

        public async Task<bool> DoesStopExist()
        {
            IsSwitchable = false;
            IsCheckingStop = true;
            
            bool doesStop = await LineDO.DoesStopAt((VehicleType)SelectedType.Value, LineNumber, StopCode);
            if (!doesStop)
            {
                LiveTile = !LiveTile;
                IsSwitchable = true;
            }

            IsCheckingStop = false;

            return doesStop;
        }

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

        private bool liveTile;
        public bool LiveTile
        {
            get
            {
                return liveTile;
            }
            set
            {
                liveTile = value;
                OnPropertyChanged();
            }
        }

        private NameValueObject selectedType;
        public NameValueObject SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
                OnPropertyChanged();
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

        private string lineNumber;
        public string LineNumber
        {
            get
            {
                return lineNumber;
            }
            set
            {
                lineNumber = value;
                OnPropertyChanged();
            }
        }


        private bool isCheckingStop;
        public bool IsCheckingStop
        {
            get
            {
                return isCheckingStop;
            }
            set
            {
                isCheckingStop = value;
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
