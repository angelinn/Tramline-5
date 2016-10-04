using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common
{
    public static class Strings
    {
        public static string AppName = "Tramline-5";
        public static string StatusBarText = "Софийски градски транспорт";
        public static string NoResults = "Няма резултати";
        public static string RequestOutlookRedirect = "Ще бъдете прехвърлени към Outlook.";
        public static string Yes = "Да";
        public static string No = "Не";
        public static string SumcRedirect = "Ще бъдете прехвърлени към сайта на Градска мобилност.";
        public static string PromptExit = "Изход?";
        public static string InvalidRequest = "Невалиден номер на спирка.";
        public static string DatabaseNotFound = "Базата от данни не беше открита. Приложението не може да продължи.";
        public static string HistoryCleared = "Историята е изчистена успешно.";
        public static string BackupExtension = "t5d";
        public static string NoInternetConnection = "Няма налична интернет връзка.";
    }

    public static class Urls
    {
        public static string Sumc = "http://m.sofiatraffic.bg/";
        public static string DeveloperEmail = "mailto:angelin.nedelchev@outlook.com";
        public static string FeedbackEmail = "https://wantoo.io/no5/ideas/";
    }

    public static class Formats
    {
        public static string Timestamp = "yyyyMMddHHmmss";
        public static string ExportSuccess = "Успешно запазване във файл";
        public static string ConfirmDeleteFavourite = "Премахване на '{0}' от любими?";
        public static string DataFromTime = "Данни от";
        public static string DoesNotStopAt = "{0} №{1} не спира на спирка с код {2}";
    }

    public static class SettingsKeys
    {
        public static string LiveTile = "LiveTile";
        public static string PushNotifications = "PushNotifications";
        public static string FavouriteStopCode = "Favourite";
        public static string FavouriteType = "FavouriteType";
        public static string FavouriteLine = "FavouriteLine";
    }
}
