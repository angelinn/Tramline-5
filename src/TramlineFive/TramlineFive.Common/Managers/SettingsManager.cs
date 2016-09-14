using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TramlineFive.Common.Managers
{
    public static class SettingsManager
    {
        public static bool UpdateValue(string key, object value)
        {
            if (ApplicationData.Current.LocalSettings.Values[key]?.ToString() == value?.ToString())
                return false;

            ApplicationData.Current.LocalSettings.Values[key] = value?.ToString();
            return true;
        }

        public static string ReadValue(string key)
        {
            return ApplicationData.Current.LocalSettings.Values[key] as string;
        }

        public static bool ClearValue(string key)
        {
            return UpdateValue(key, null);
        }
    }
}
