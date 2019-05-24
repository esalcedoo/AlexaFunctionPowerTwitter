using AlexaPowerTwitter.Models;
using AlexaPowerTwitter.Models.TranslateModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlexaPowerTwitter.Services
{
    public class TranslateService
    {
        private readonly HttpClient _client;
        private readonly LanguageService _languageService;

        public TranslateService(HttpClient client, LanguageService languageService)
        {
            _client = client;
            _languageService = languageService;
        }

        internal async Task<string> DetectLanguage(string text)
        {
            string lang = string.Empty;

            string route = "/detect?api-version=3.0";

            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(route, httpContent);

            if (response.IsSuccessStatusCode)
            {
                lang = (await response.Content.ReadAsAsync<List<LanguageDetectionResultModel>>())
                    .FirstOrDefault()?.Language;
            }

            return _languageService.AdaptCulture(lang);
        }

        internal async Task<string> TranslateToLanguage(string message, string lang)
        {
            string translation = string.Empty;

            string route = $"/translate?api-version=3.0&to=de&to={lang}";

            object[] body = new object[] { new { Text = message } };
            var requestBody = JsonConvert.SerializeObject(body);

            HttpContent httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(route, httpContent);

            if (response.IsSuccessStatusCode)
            {
                translation = (await response.Content.ReadAsAsync<List<TranslationResultModel>>())
                    .FirstOrDefault().Translations
                    .FirstOrDefault(t => lang.Contains(t.To, StringComparison.InvariantCultureIgnoreCase))?.Text;
            }

            return translation;
        }
    }
}
