using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Laputa.Localization
{
    public static class LocalizationManager
    {
        public static LanguageName currentLanguageName = LanguageName.English;
        private static readonly HttpClient Client = new HttpClient();

        public static void OnChangeLanguage(LanguageName languageName)
        {
            currentLanguageName = languageName;
            LocalizationObserver.onLanguageChanged?.Invoke(languageName);
            PlayerPrefs.SetString("localization",languageName.ToString());
        }

        public static async Task<string> TranslateAsync(string text, string targetLanguage, string sourceLanguage = "auto")
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLanguage}&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";

            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            Debug.LogWarning(response.EnsureSuccessStatusCode());
        
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseJson = JArray.Parse(responseBody);
        

            return (string) responseJson[0][0]?[0];
        }
    }

    public static class LocalizationObserver
    {
        public static Action<LanguageName> onLanguageChanged;
    }
}