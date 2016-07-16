using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus.Common.Exceptions
{
    public class CaptchaLocationError : Exception
    {
        public CaptchaLocationError(string message) : base(message)
        {   }
    }
}
