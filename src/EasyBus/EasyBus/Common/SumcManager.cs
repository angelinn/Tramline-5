using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyBus.Common
{
    public static class SumcManager
    {
        // 
        // If null is returned, CaptchaUrl is set. Captcha is required.
        // If empty IEnumerable is returned, there are no results in the query.
        //

        public static async Task<IEnumerable<ArrivalViewModel>> GetByStopAsync(string query)
        {
            Uri address = new Uri(BASE_URL);
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

                if (RequiresCaptcha(doc))
                    return null;

                List<KeyValuePair<string, string>> formQuery = GetHiddenFields(doc).ToList();
                formQuery.Add(new KeyValuePair<string, string>(STOP_CODE, query));
                formQuery.Add(new KeyValuePair<string, string>(SUBMIT, WebUtility.UrlEncode(SUBMIT_VALUE)));

                FormUrlEncodedContent content = new FormUrlEncodedContent(formQuery);

                HttpResponseMessage response = await client.PostAsync(address, content);
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
                        Timings = data[2].Trim(),
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
                CaptchaUrl = root.DocumentNode.Descendants()
                                        .Where(d => d.OriginalName == "img")
                                        .First()
                                        .InnerHtml;
            }

            return requiresCaptcha;
        }

        public static string CaptchaUrl { get; set; }

        private const string BASE_URL = "http://m.sofiatraffic.bg/vt";
        private const string STOP_CODE = "stopCode";
        private const string SUBMIT = "submit";
        private const string SUBMIT_VALUE = "Провери";
        private const string MAGIC_COOKIE_NAME = "INSERT_NAME_HERE";
        private const string MAGIC_COOKIE_VALUE = "INSERT_VALUE_HERE";
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
    }
}
