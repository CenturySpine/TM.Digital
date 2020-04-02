using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace TM.Digital.Transport
{
    public class TmDigitalClientRequestHandler
    {
        private HttpClient _client;

        public static TmDigitalClientRequestHandler Instance { get; } = new TmDigitalClientRequestHandler();

        public TmDigitalClientRequestHandler()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:50154/api/") };
        }

        public async Task<T> Request<T>(string route)
        {
            var result = await _client.GetAsync(route);
            result.EnsureSuccessStatusCode();
            string json = await result.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public async Task Post<T>(string uri, T postParameter)
        {
            HttpContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postParameter));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await _client.PostAsync(uri, content);
            result.EnsureSuccessStatusCode();
        }
    }
}