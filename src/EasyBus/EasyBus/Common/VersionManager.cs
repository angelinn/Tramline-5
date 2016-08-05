using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace EasyBus.Common
{
    public static class VersionManager
    {
        public static string GetVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }
    }
}
