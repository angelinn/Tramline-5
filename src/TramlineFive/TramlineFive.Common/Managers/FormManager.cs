using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TramlineFive.Common.Managers
{
    public static class FormManager
    { 
        public static List<KeyValuePair<string, string>> GetFormFields(HtmlNode form)
        {
            List<KeyValuePair<string, string>> formData = GetHiddenFields(form).ToList();
            HtmlNode submit = form.Descendants().Where(d => d.Name == "input" && d.GetAttributeValue("type", "") == "submit").FirstOrDefault();
            if (submit != null)
                formData.Add(new KeyValuePair<string, string>(submit.Attributes["name"].Value, WebUtility.UrlEncode(submit.Attributes["value"].Value)));

            return formData;
        }

        private static IEnumerable<KeyValuePair<string, string>> GetHiddenFields(HtmlNode root)
        {
            return root.Descendants()
                    .Where(d => d.Name == "input" && d.GetAttributeValue("type", "") == "hidden" &&
                           d.Attributes.Contains("name") && d.Attributes.Contains("value"))
                    .Select(n => new KeyValuePair<string, string>(n.Attributes["name"].Value, n.Attributes["value"].Value));
        }
    }
}
