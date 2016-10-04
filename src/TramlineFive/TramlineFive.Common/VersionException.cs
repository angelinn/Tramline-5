using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common
{
    public class VersionException : Exception
    {
        public VersionException(string message = "") : base(message)
        {   }
    }
}
