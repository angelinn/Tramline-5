using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Models;

namespace TramlineFive.Common
{
    public static class SumcManager
    {
        public static async Task<IEnumerable<Arrival>> GetByStopAsync(string query, ICaptchaDialog captchaDialog)
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
                    captchaDialog.SetUrl(captchaUrl);
                    await captchaDialog.ShowAsync();

                    formQuery.Add(new KeyValuePair<string, string>(CAPTCHA_KEY, captchaDialog.CaptchaString));
                }

                FormUrlEncodedContent content = new FormUrlEncodedContent(formQuery);

                HttpResponseMessage response = await client.PostAsync(address, content);

                if (UpdateCookie(handler.CookieContainer.GetCookies(address)))
                    SettingsManager.UpdateValue("cookie", MAGIC_COOKIE_VALUE);

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

        private static IEnumerable<Arrival> GetArrivals(string htmlString)
        {
            if (htmlString == null)
                return null;

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlString);

            IEnumerable<HtmlNode> boldNodes = doc.DocumentNode.Descendants().Where(d => d.OriginalName == "b");
            string stopTitle = String.Empty;
            if (boldNodes.Count() >= 3)
                stopTitle = boldNodes.ToList()[2].InnerText;

            IEnumerable<HtmlNode> infos = doc.DocumentNode.Descendants()
                    .Where(d => d.GetAttributeValue("class", "").StartsWith("arr_info"));

            List<Arrival> arrivals = new List<Arrival>();
            foreach (var info in infos)
            {
                string title = info.Descendants().Where(d => d.OriginalName == "b").Select(n => n.InnerText).FirstOrDefault();
                string[] data = info.InnerText.Split('\n');

                int number;
                if (Int32.TryParse(title, out number) && data.Length >= 4)
                {
                    arrivals.Add(new Arrival
                    {
                        VehicleNumber = Int32.Parse(title),
                        Timings = data[2].Trim().Split(','),
                        Direction = data[3].Trim(),
                        StopTitle = stopTitle
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

        private static bool UpdateCookie(CookieCollection cookies)
        {
            foreach (Cookie cookie in cookies)
            {
                if (cookie.Name == MAGIC_COOKIE_NAME)
                {
                    if (MAGIC_COOKIE_VALUE != cookie.Value)
                    {
                        MAGIC_COOKIE_VALUE = cookie.Value;
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
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
        private static string MAGIC_COOKIE_VALUE = SettingsManager.ReadValue("cookie") as string;
    }
}
