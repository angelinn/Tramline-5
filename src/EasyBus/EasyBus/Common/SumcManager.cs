using EasyBus.ViewModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EasyBus.Common
{
    public static class SumcManager
    {
        public static async Task Load()
        {
            await ReadCookie();
        }

        public static async Task<IEnumerable<ArrivalViewModel>> GetByStopAsync(string query)
        {
            int queryNum;
            if (String.IsNullOrEmpty(query) || !Int32.TryParse(query, out queryNum))
                return null;

            Uri address = new Uri(VIRTUAL_TABLES_URL);

            CookieContainer cookies = new CookieContainer();
            cookies.Add(address, new Cookie(MAGIC_COOKIE_NAME, MAGIC_COOKIE_VALUE));

            using (HttpClientHandler handler = new HttpClientHandler())
            using (HttpClient client = new HttpClient(handler))
            {
                handler.CookieContainer = cookies;
                client.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);

                var getresult = await client.GetAsync(address);
                HtmlDocument doc = new HtmlDocument();
                doc.Load(await getresult.Content.ReadAsStreamAsync());

                List<KeyValuePair<string, string>> formQuery = GetHiddenFields(doc).ToList();
                formQuery.Add(new KeyValuePair<string, string>(STOP_CODE, query));
                formQuery.Add(new KeyValuePair<string, string>(SUBMIT, WebUtility.UrlEncode(SUBMIT_VALUE)));


                if (RequiresCaptcha(doc))
                {
                    Captcha captchaDialog = new Captcha(captchaUrl);
                    await captchaDialog.ShowAsync();

                    formQuery.Add(new KeyValuePair<string, string>(CAPTCHA_KEY, captchaDialog.CaptchaString));
                }

                FormUrlEncodedContent content = new FormUrlEncodedContent(formQuery);

                HttpResponseMessage response = await client.PostAsync(address, content);

                await SaveCookie(handler.CookieContainer.GetCookies(address));
                return GetArrivals(await response.Content.ReadAsStringAsync());
            }
        }

        private static IEnumerable<KeyValuePair<string, string>> GetHiddenFields(HtmlDocument root)
        {
            return root.DocumentNode.Descendants()
                    .Where(d => d.Name == "input" && d.GetAttributeValue("type", "") == "hidden" &&
                           d.Attributes.Contains("name") && d.Attributes.Contains("value"))
                    .Select(n => new KeyValuePair<string, string>(n.Attributes["name"].Value, n.Attributes["value"].Value));
        }

        private static IEnumerable<ArrivalViewModel> GetArrivals(string htmlString)
        {
            if (htmlString == null)
                return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            IEnumerable<HtmlNode> infos = doc.DocumentNode.Descendants()
                    .Where(d => d.GetAttributeValue("class", "").StartsWith("arr_info"));

            List<ArrivalViewModel> arrivals = new List<ArrivalViewModel>();
            foreach (var info in infos)
            {
                string title = info.Descendants().Where(d => d.OriginalName == "b").Select(n => n.InnerText).FirstOrDefault();
                string[] data = info.InnerText.Split('\n');

                int number;
                if (Int32.TryParse(title, out number) && data.Length >= 4)
                {
                    arrivals.Add(new ArrivalViewModel
                    {
                        VehicleNumber = Int32.Parse(title),
                        Timings = data[2].Trim().Split(','),
                        Direction = data[3].Trim()
                    });
                }
            }
            
            return arrivals;
        }

        private static bool RequiresCaptcha(HtmlDocument root)
        {
            bool requiresCaptcha = root.DocumentNode.Descendants()
                                    .Any(d => d.GetAttributeValue("class", "") == "formLblCaptcha");

            if (requiresCaptcha)
            {
                captchaUrl = BASE_URL + root.DocumentNode.Descendants()
                                        .Where(d => d.OriginalName == "img")
                                        .Last()
                                        .Attributes["src"]
                                        .Value;
            }

            return requiresCaptcha;
        }

        private static async Task<bool> ReadCookie()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile cookieFile = null;

            try
            {
                cookieFile = await folder.GetFileAsync("kewl.txt");
            }
            catch (FileNotFoundException)
            {
                // It's ok, that means first boot of app is done
                return false;
            }

            MAGIC_COOKIE_VALUE = await FileIO.ReadTextAsync(cookieFile);
            return true;
        }

        private static async Task<bool> SaveCookie(CookieCollection cookies)
        {
            foreach (Cookie cookie in cookies)
            {
                if (cookie.Name == MAGIC_COOKIE_NAME)
                {
                    if (MAGIC_COOKIE_VALUE != cookie.Value)
                    {
                        MAGIC_COOKIE_VALUE = cookie.Value;

                        StorageFolder folder = ApplicationData.Current.LocalFolder;
                        StorageFile cookieFile = await folder.CreateFileAsync("kewl.txt", CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteTextAsync(cookieFile, MAGIC_COOKIE_VALUE);
                    }

                    return true;
                }
            }
            return false;
        }

        public static void ResetCookie()
        {
            MAGIC_COOKIE_VALUE = String.Empty;
        }

        private static string captchaUrl;

        private const string BASE_URL = "http://m.sofiatraffic.bg";
        private const string VIRTUAL_TABLES_URL = "http://m.sofiatraffic.bg/vt";
        private const string STOP_CODE = "stopCode";
        private const string SUBMIT = "submit";
        private const string SUBMIT_VALUE = "Провери";
        private const string CAPTCHA_KEY = "sc";
        private const string MAGIC_COOKIE_NAME = "alpocjengi";
        private static string MAGIC_COOKIE_VALUE = "af64ddf454724184db77e2562c92a15a64d42171";
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
    }
}
