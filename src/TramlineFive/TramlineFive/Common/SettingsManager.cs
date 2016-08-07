using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TramlineFive.Common
{
    public static class SettingsManager
    {
        public static void UpdateValue(string key, object value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        public static object ReadValue(string key)
        {
            return ApplicationData.Current.LocalSettings.Values[key];
        }
    }
}
