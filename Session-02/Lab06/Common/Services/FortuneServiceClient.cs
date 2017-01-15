
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace Fortune_Teller_Service.Common.Services
{
    public class FortuneServiceClient : IFortuneService
    {
        ILogger<FortuneServiceClient> _logger;
        public FortuneServiceClient(ILogger<FortuneServiceClient> logger)
        {
            _logger = logger;
        }

        public async Task<List<Fortune>> AllFortunesAsync()
        {
            return await Task.Run(() => new List<Fortune>() { new Fortune() { Id = 1, Text = "I need to be wired up!" } });
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            var all = await AllFortunesAsync();
            return all[0];
        }


        private async Task<T> HandleRequest<T>(string url) where T : class
        {
            _logger?.LogDebug("FortuneService call: {0}", url);
            try
            {
                using (var client = GetClient())
                {
                    var stream = await client.GetStreamAsync(url);
                    var result = Deserialize<T>(stream);
                    _logger?.LogDebug("FortuneService returned: {0}", result);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("FortuneService exception: {0}", e);
            }
            return null;
        }


        private T Deserialize<T>(Stream stream) where T : class
        {
            try
            {
                using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (T)serializer.Deserialize(reader, typeof(T));
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("FortuneService serialization exception: {0}", e);
            }
            return (T)null;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            return client;
        }
    }
}

