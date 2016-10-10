using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Models;

namespace TramlineFive.Common.Managers
{
    public static class VehicleTypeManager
    {
        public static string Stringify(VehicleType type, bool plural = false)
        {
            switch (type)
            {
                case VehicleType.Bus:
                    return plural ? "Автобуси" : "Автобус";
                case VehicleType.Tram:
                    return plural ? "Трамваи" : "Трамвай";
                case VehicleType.Trolley:
                    return plural ? "Тролеи" : "Тролей";

                default:
                    return String.Empty;
            }
        }

        public static VehicleType Destringify(string stringified)
        {
            switch (stringified)
            {
                case "Автобус":
                    return VehicleType.Bus;
                case "Трамвай":
                    return VehicleType.Tram;
                case "Тролей":
                    return VehicleType.Trolley;

                default:
                    return VehicleType.None;
            }
        }

        public static string ToShort(VehicleType type)

        {
            switch (type)
            {
                case VehicleType.Bus:
                    return "А";
                case VehicleType.Tram:
                    return "ТМ";
                case VehicleType.Trolley:
                    return "ТБ";

                default:
                    return String.Empty;
            }
        }
    }
}
