using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace TramlineFive.Common.Managers
{
    public static class VersionManager
    {
        public static string Version
        {
            get
            {
                if (version == null)
                {
                    PackageVersion packageVersion = Package.Current.Id.Version;
                    version = $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
                }
                return version;
            }
        }
        
        public static string DisplayName
        {
            get
            {
                return Package.Current.DisplayName;
            }
        }

        private static string version;
    }
}
