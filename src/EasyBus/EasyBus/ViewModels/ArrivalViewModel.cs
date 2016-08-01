using EasyBus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus.ViewModels
{
    public class ArrivalViewModel : NotifyingViewModel
    {
        private int vehicleNumber;
        public int VehicleNumber
        {
            get
            {
                return vehicleNumber;
            }

            set
            {
                vehicleNumber = value;
                OnPropertyChanged("VehicleNumber");
            }
        }

        private VehicleType vehicleType;
        public VehicleType VehicleType
        {
            get
            {
                return vehicleType;
            }

            set
            {
                vehicleType = value;
                OnPropertyChanged("VehicleType");
            }
        }

        private string timings;
        public string Timings
        {
            get
            {
                return timings;
            }
            set
            {
                if (value != null)
                {
                    timings = value;
                    OnPropertyChanged("Timings");
                }
            }
        }

        private string direction;
        public string Direction
        {
            get
            {
                return direction;
            }
            set
            {
                if (value != null)
                {
                    direction = value;
                    OnPropertyChanged("Direction");
                }
            }
        }
    }
}
