using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace TramlineFive.Common
{
    public static class VersionManager
    {
        private static string version;
        public static string Version
        {
            get
            {
                if (version == null)
                {
                    PackageVersion packageVersion = Package.Current.Id.Version;
                    version = String.Format("{0}.{1}.{2}", packageVersion.Major, packageVersion.Minor, packageVersion.Build);
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
    }
}
