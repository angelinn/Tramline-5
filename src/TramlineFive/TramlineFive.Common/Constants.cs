using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common
{
    public static class Strings
    {
        public static string StatusBarText = "Софийски градски транспорт";
        public static string NoResults = "Няма резултати";
        public static string RequestOutlookRedirect = "Ще бъдете прехвърлени към Outlook.";
        public static string Yes = "Да";
        public static string No = "Не";
        public static string SumcRedirect = "Ще бъдете прехвърлени към сайта на Градска мобилност.";
        public static string PromptExit = "Изход?";
        public static string InvalidRequest = "Невалиден номер на спирка.";
        public static string DatabaseNotFound = "Базата от данни не беше открита. Приложението не може да продължи.";
    }

    public static class Urls
    {
        public static string Sumc = "http://m.sofiatraffic.bg/";
        public static string DeveloperEmail = "mailto:angelin.nedelchev@outlook.com";
    }
}
