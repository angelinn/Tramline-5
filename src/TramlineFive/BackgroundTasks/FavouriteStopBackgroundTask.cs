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

            UpdateTile(arrivals);
            deferral.Complete();
        }

        private void UpdateTile(List<Arrival> arrivals)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            XmlDocument xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Image);
            ((XmlElement)xml.GetElementsByTagName("image")[0]).SetAttribute("src", "ms-appx:///Assets/Store/Wide310x150Logo.scale-400.png");

            int itemCount = 1;

            foreach (Arrival arrival in arrivals)
            {
                XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text03);
                string title = arrivals.First().StopTitle;
                tileXml.GetElementsByTagName("text")[0].InnerText = SumcParser.ParseStopTitle(title) + "\n" + String.Join(", ", arrival.Timings);
                //tileXml.GetElementsByTagName("text")[1].InnerText = String.Join(", ", arrival.Timings);

                TileNotification timings = new TileNotification(tileXml);
                timings.ExpirationTime = DateTime.Now.AddHours(1);

                updater.Update(timings);
                if (itemCount++ > 5)
                    break;
            }
            TileNotification defaultTile = new TileNotification(xml);
            defaultTile.ExpirationTime = DateTime.Now.AddHours(1);
            updater.Update(defaultTile);
        }
    }
}
