using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TramlineFive.Common.Converters
{
    public class TimingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] timings = (string[])value;
            StringBuilder builder = new StringBuilder();
            foreach (string singleTiming in timings)
            {
                TimeSpan timing;
                if (TimeSpan.TryParse((string)singleTiming, out timing))
                {
                    TimeSpan timeLeft = timing - DateTime.Now.TimeOfDay;
                    builder.AppendFormat("{0} ({1} мин), ", singleTiming, timeLeft.Minutes);
                }
            }

            builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return String.Empty;
        }
    }
}
