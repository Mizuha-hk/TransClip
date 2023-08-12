using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TransClip.Library
{
    public class Translator
    {
        private bool IsPro { get; set; }
        private string AuthKey { get; set; }

        private const string freeUri = "https://api-free.deepl.com/v2/translate";
        private const string proUri = "https://api.deepl.com/v2/translate";

        public struct Language
        {
            public const string EN = "EN";
            public const string JP = "JA";
        }
        
        public Translator(bool isPro ,string authKey)
        {
            this.IsPro = isPro;
            this.AuthKey = authKey;
        }

        public void ChangeDeepLSettings(bool isPro,  string authKey)
        {
            this.IsPro = isPro;
            this.AuthKey = authKey;
        }

        public async Task<string> TranslateAsync(string text, string lang)
        {
            var uri = IsPro ? proUri : freeUri;

            using (var http = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), uri))
                {
                    var contentList = new List<string>
                    {
                        "auth_key=" + AuthKey,
                        "text=" + text,
                        "target_lang=" + lang
                    };

                    request.Content = new StringContent(string.Join("&", contentList));
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = await http.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var jObject = JsonSerializer.Deserialize<translations>(responseString);
                    return jObject.text;
                }
            }
        }

        private class translations
        {
            public string detected_source_language { get; set; }
            public string text { get; set; }
        }
    }
}
