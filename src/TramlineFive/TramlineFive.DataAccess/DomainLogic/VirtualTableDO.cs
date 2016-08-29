using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.DataAccess.DomainLogic
{
    public class VirtualTableDO : NotifyingModel
    {
        private string stopTitle;
        public string StopTitle
        {
            get
            {
                return stopTitle;
            }
            set
            {
                stopTitle = value;
                OnPropertyChanged("StopTitle");
            }
        }

        private string asOfTime;
        public string AsOfTime
        {
            get
            {
                return asOfTime;
            }
            set
            {
                asOfTime = value;
                OnPropertyChanged("AsOfTime");
            }
        }

        private bool isQueried;
        public bool IsQueried
        {
            get
            {
                return isQueried;
            }
            set
            {
                isQueried = value;
                OnPropertyChanged("IsQueried");
            }
        }
    }
}
