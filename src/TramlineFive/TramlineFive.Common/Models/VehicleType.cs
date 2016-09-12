using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common.Models
{
    public enum VehicleType
    {
        None = -1,
        Bus,
        Tram,
        Trolley
    }

    public static class VehicleTypeManager
    {
        public static string TypeToString(VehicleType type, bool plural = false)
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
    }
}
