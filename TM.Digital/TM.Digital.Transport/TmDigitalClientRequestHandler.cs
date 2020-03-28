using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TM.Digital.Transport
{
    public class TmDigitalClientRequestHandler
    {
        private HttpClient _client;

        public static TmDigitalClientRequestHandler Instance { get; } = new TmDigitalClientRequestHandler();

        public TmDigitalClientRequestHandler()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:50154/api/");
        }

        public async Task<T> Request<T>(string route)
        {
            var result = await _client.GetAsync(route);
            result.EnsureSuccessStatusCode();
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}