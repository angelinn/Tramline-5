using TramlineFive.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.ViewModels
{
    public class VersionViewModel
    {
        private string version = VersionManager.Version;
        public string Version
        {
            get
            {
                return version;
            }
        }

        private string displayName = VersionManager.DisplayName.ToUpper();
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }
    }
}
