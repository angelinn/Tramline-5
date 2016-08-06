using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.Entities;

namespace TramlineFive.ViewModels
{
    public class LineViewModel : NotifyingViewModel
    {
        private List<Line> lines;
        public List<Line> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
                OnPropertyChanged("Lines");
            }
        }
    }
}
