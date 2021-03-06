﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TramlineFive.Common.Models;

namespace TramlineFive.Common.Managers
{
    public static class SumcManager
    {
        static SumcManager()
        {
            HtmlNode.ElementsFlags.Remove("form");
            SessionCookieValue = SettingsManager.ReadValue("cookie") as string;

            CookieContainer cookies = new CookieContainer();
            cookies.Add(VT_URI, new Cookie(SESSION_COOKIE_NAME, SessionCookieValue));

            httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = cookies;

            httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);
        }

        public static async Task<List<Arrival>> GetByStopAsync(string query, Type captchaType, string submitIndex = "")
        {
            int queryNum;
            if (String.IsNullOrEmpty(query) || !Int32.TryParse(query, out queryNum))
                return null;

            await CheckIfObsoleteAsync();

            HttpResponseMessage getResult = await httpClient.GetAsync(VT_URI);
            HtmlDocument doc = new HtmlDocument();
            doc.Load(await getResult.Content.ReadAsStreamAsync());

            List<KeyValuePair<string, string>> formQuery = FormManager.GetFormFields(doc.DocumentNode);
            formQuery.Add(new KeyValuePair<string, string>(STOP_CODE, query));

            if (RequiresCaptcha(doc))
            {
                ICaptchaDialog captchaDialog = Activator.CreateInstance(captchaType) as ICaptchaDialog;
                captchaDialog.SetUrl(captchaUrl);
                await captchaDialog.ShowAsync();

                formQuery.Add(new KeyValuePair<string, string>(CAPTCHA_KEY, captchaDialog.CaptchaString));
            }

            FormUrlEncodedContent content = new FormUrlEncodedContent(formQuery);
            HttpResponseMessage response = await httpClient.PostAsync(VT_URI, content);

            UpdateCookie();
            
            HtmlDocument responseHtml = new HtmlDocument();
            responseHtml.LoadHtml(await response.Content.ReadAsStringAsync());

            List<Arrival> arrivals = ParseArrivals(responseHtml.DocumentNode);
            IEnumerable<FormUrlEncodedContent> formsData = ParseOtherTransportTypes(responseHtml.DocumentNode);
            if (formsData.Count() > 0)
                arrivals.AddRange(await GetOtherTransportTypes(formsData));

            return arrivals;
        }

        private static async Task<List<Arrival>> GetOtherTransportTypes(IEnumerable<FormUrlEncodedContent> formsData)
        {
            List<Arrival> more = new List<Arrival>();
            HttpResponseMessage response = null;
            HtmlDocument document = new HtmlDocument();

            foreach (FormUrlEncodedContent formData in formsData)
            {
                response = await httpClient.PostAsync(VT_URI, formData);
                document.LoadHtml(await response.Content.ReadAsStringAsync());
                more.AddRange(ParseArrivals(document.DocumentNode));
            }
            return more;
        }

        private static IEnumerable<FormUrlEncodedContent> ParseOtherTransportTypes(HtmlNode node)
        {
            IEnumerable<HtmlNode> forms = node.Descendants().Where(d => d.Name == "form" && d.GetAttributeValue("name", "") != "").Skip(1);
            foreach (HtmlNode form in forms)
                yield return new FormUrlEncodedContent(FormManager.GetFormFields(form));
        }

        private static List<Arrival> ParseArrivals(HtmlNode node)
        {
            IEnumerable<HtmlNode> boldNodes = node.Descendants().Where(d => d.OriginalName == "b");
            string stopTitle = String.Empty;
            if (boldNodes.Count() >= 3)
                stopTitle = boldNodes.ToList()[2].InnerText;

            IEnumerable<HtmlNode> infos = node.Descendants().Where(d => d.GetAttributeValue("class", "").StartsWith("arr_info"));

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
                        Type = ParseManager.ParseSumcVehicleType(info.Attributes["class"].Value.Last()),
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

        private static bool UpdateCookie()
        {
            foreach (Cookie cookie in httpClientHandler.CookieContainer.GetCookies(VT_URI))
            {
                if (cookie.Name == SESSION_COOKIE_NAME)
                {
                    if (SessionCookieValue != cookie.Value)
                    {
                        SessionCookieValue = cookie.Value;
                        SettingsManager.UpdateValue("cookie", SessionCookieValue);
                    }

                    return true;
                }
            }
            return false;
        }

        public static void ResetCookie()
        {
            SessionCookieValue = String.Empty;
        }

        private static async Task CheckIfObsoleteAsync()
        {
            if (String.IsNullOrEmpty(azureVersion))
            {
                HttpResponseMessage res = await httpClient.GetAsync(AZURE_API_URL);
                azureVersion = await res.Content.ReadAsStringAsync();
            }

            if (azureVersion != $"\"{VersionManager.Version}\"")
                throw new VersionException();
        }

        private const string BASE_URL = "http://m.sofiatraffic.bg";
        private const string VIRTUAL_TABLES_URL = "http://m.sofiatraffic.bg/vt";
        private const string AZURE_API_URL = "http://tramlinefive.azurewebsites.net/api/values";
        private const string STOP_CODE = "stopCode";
        private const string CAPTCHA_KEY = "sc";
        private const string SESSION_COOKIE_NAME = "alpocjengi";
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";

        private static Uri VT_URI = new Uri(VIRTUAL_TABLES_URL);
        private static string captchaUrl;
        private static string SessionCookieValue;

        private static string azureVersion;

        private static HttpClient httpClient;
        private static HttpClientHandler httpClientHandler;
    }
}
