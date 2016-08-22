using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.ViewModels
{
    public class StringViewModel : NotifyingViewModel
    {
        private string source;
        public string Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }
    }
}
