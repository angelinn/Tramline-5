using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TramlineFive.Common.Converters
{
    public class StopCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string code = value as string;
            return String.Format("{0:D4}", Int32.Parse(code));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return String.Empty;
        }
    }
}
