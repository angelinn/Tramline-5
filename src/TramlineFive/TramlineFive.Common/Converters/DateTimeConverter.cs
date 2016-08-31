using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TramlineFive.Common.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime? date = value as DateTime?;
            if (date == null)
                return String.Empty;

            return date.Value.ToString("dd MMMM HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
