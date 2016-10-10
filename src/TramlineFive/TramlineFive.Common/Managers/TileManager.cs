using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Models;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TramlineFive.Common.Managers
{
    public static class TileManager
    {
        public static async Task QueryStopAndUpdate()
        {
            List<Arrival> arrivals = await SumcManager.GetByStopAsync(SettingsManager.ReadValue(SettingsKeys.FavouriteStopCode) as string, null);

            string type = SettingsManager.ReadValue(SettingsKeys.FavouriteType);
            string line = SettingsManager.ReadValue(SettingsKeys.FavouriteLine);

            foreach (Arrival arrival in arrivals)
            {
                if (arrival.Type == type && arrival.VehicleNumber.ToString() == line)
                {
                    UpdateTiles($"{arrival.Type} {arrival.VehicleNumber}\n{ParseManager.ParseStopTitle(arrival.StopTitle)}\n{String.Join(", ", arrival.Timings)}");
                    break;
                }
            }
        }

        private static void UpdateTiles(string message)
        {
            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            TileNotification wide = CreateWideNotification(message);
            TileNotification square = CreateSquareNotification(message);

            updater.Update(wide);
            updater.Update(square);
        }

        private static TileNotification CreateWideNotification(string message)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150PeekImage03);
            ((XmlElement)tileXml.GetElementsByTagName("image")[0]).SetAttribute("src", WIDE_LOGO_SRC);
            tileXml.GetElementsByTagName("text")[0].InnerText = message;

            TileNotification notification = new TileNotification(tileXml);

            notification.ExpirationTime = DateTime.Now.AddHours(1);
            return notification;
        }

        private static TileNotification CreateSquareNotification(string message)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText03);
            string[] split = message.Split('\n');

            ((XmlElement)tileXml.GetElementsByTagName("image")[0]).SetAttribute("src", SQUARE_LOGO_SRC);
            tileXml.GetElementsByTagName("text")[0].InnerText = split[0];
            tileXml.GetElementsByTagName("text")[1].InnerText = split.Length > 1 ? split[1] : String.Empty;
            tileXml.GetElementsByTagName("text")[2].InnerText = split.Length > 2 ? split[2] : String.Empty;

            TileNotification notification = new TileNotification(tileXml);

            notification.ExpirationTime = DateTime.Now.AddHours(1);
            return notification;
        }

        private const string WIDE_LOGO_SRC = "ms-appx:///Assets/Store/Wide310x150Logo.scale-400.png";
        private const string SQUARE_LOGO_SRC = "ms-appx:///Assets/Store/Square150x150Logo.scale-400.png";
    }
}
