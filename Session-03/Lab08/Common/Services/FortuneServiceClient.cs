
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace Fortune_Teller_Service.Common.Services
{
    public class FortuneServiceClient : IFortuneService
    {
        ILogger<FortuneServiceClient> _logger;

        // Lab07 Start
        IOptionsSnapshot<FortuneServiceConfig> _config;
        public FortuneServiceClient(IOptionsSnapshot<FortuneServiceConfig> config, ILogger<FortuneServiceClient> logger)
        {
            _logger = logger;
            _config = config;
        }
        // Lab07 End

        public async Task<List<Fortune>> AllFortunesAsync()
        {
            // Lab07
            return await HandleRequest<List<Fortune>>(_config.Value.AllFortunesURL());
            // Lab07
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            // Lab07
            return await HandleRequest<Fortune>(_config.Value.RandomFortuneURL());
            // Lab07
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


