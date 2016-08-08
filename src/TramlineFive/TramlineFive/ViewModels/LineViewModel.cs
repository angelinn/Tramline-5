using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels
{
    public class LineViewModel : NotifyingViewModel
    {
        private List<LineDO> lines;
        public List<LineDO> Lines
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
