using EasyBus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus.ViewModels
{
    class VersionViewModel
    {
        private string version = VersionManager.GetVersion();
        public string Version
        {
            get
            {
                return version;
            }
        }
    }
}
