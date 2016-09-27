using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Managers;
using TramlineFive.Common.Models;
using TramlineFive.DataAccess.DomainLogic;

namespace TramlineFive.ViewModels.Wrappers
{
    public class LineViewModel
    {
        public LineViewModel(LineDO domain)
        {
            core = domain;
        }

        public IEnumerable<DirectionViewModel> Directions
        {
            get
            {
                return core.Directions.Select(d => new DirectionViewModel(d));
            }
        }

        public string Route
        {
            get
            {
                return core.Directions.First().Name;
            }
        }

        public int Number
        {
            get
            {
                return core.Number;
            }
        }

        public string FriendlyName
        {
            get
            {
                return $"{VehicleTypeManager.Stringify(core.Type)} {core.NumberString}";
            }
        }

        public VehicleType Type
        {
            get
            {
                return core.Type;
            }
        }

        public string NumberString
        {
            get
            {
                return core.NumberString;
            }
        }

        public int SortID
        {
            get
            {
                switch (Type)
                {
                    case VehicleType.Bus:
                        return 3;
                    case VehicleType.Tram:
                        return 1;
                    case VehicleType.Trolley:
                        return 2;
                    default:
                        return 4;
                }
            }
        }

        public async Task LoadDirections()
        {
            await core.LoadDirections();
        }

        private LineDO core;
    }
}
