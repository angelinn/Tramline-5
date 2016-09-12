using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common;
using TramlineFive.Common.Converters;
using TramlineFive.Common.Models;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class FavouriteStopBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            List<Arrival> arrivals = await SumcManager.GetByStopAsync("2193", null);

            UpdateTiles(arrivals);
            deferral.Complete();
        }

        private void UpdateTiles(List<Arrival> arrivals)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            int itemCount = 0;

            foreach (Arrival arrival in arrivals)
            {
                string title = arrivals.First().StopTitle;
                string message = $"{arrival.Type} {arrival.VehicleNumber}\n{SumcParser.ParseStopTitle(title)}\n{String.Join(", ", arrival.Timings)}";

                TileNotification wide = CreateWideNotification(message);
                TileNotification square = CreateSquareNotification(message);

                updater.Update(wide);
                updater.Update(square);
                if (itemCount++ > 5)
                    break;
            }
        }

        private TileNotification CreateWideNotification(string message)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150PeekImage03);
            ((XmlElement)tileXml.GetElementsByTagName("image")[0]).SetAttribute("src", WIDE_LOGO_SRC);
            tileXml.GetElementsByTagName("text")[0].InnerText = message;

            TileNotification notification = new TileNotification(tileXml);

            notification.ExpirationTime = DateTime.Now.AddHours(1);
            return notification;
        }

        private TileNotification CreateSquareNotification(string message)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText03);
            string[] split = message.Split('\n');

            ((XmlElement)tileXml.GetElementsByTagName("image")[0]).SetAttribute("src", SQUARE_LOGO_SRC);
            tileXml.GetElementsByTagName("text")[0].InnerText = split[0];
            tileXml.GetElementsByTagName("text")[1].InnerText = split[1];
            tileXml.GetElementsByTagName("text")[2].InnerText = split[2];

            TileNotification notification = new TileNotification(tileXml);

            notification.ExpirationTime = DateTime.Now.AddHours(1);
            return notification;
        }

        private const string WIDE_LOGO_SRC = "ms-appx:///Assets/Store/Wide310x150Logo.scale-400.png";
        private const string SQUARE_LOGO_SRC = "ms-appx:///Assets/Store/Square150x150Logo.scale-400.png";
    }
}
