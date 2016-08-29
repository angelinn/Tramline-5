using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace TramlineFive.Common.Converters
{
    public class UrlToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string imageSource = String.Empty;
            switch(value as string)
            {
                case "Трамвай":
                    imageSource = "ms-appx:///Assets/Tram-64.png";
                    break;
                case "Тролей":
                    imageSource = "ms-appx:///Assets/Trolleybus-64.png";
                    break;
                case "Автобус":
                    imageSource = "ms-appx:///Assets/Bus-64.png";
                    break;
            }
            return new BitmapImage(new Uri(imageSource));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
