
using System.Threading.Tasks;
using System.Collections.Generic;
using Pivotal.Discovery.Client;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Fortune_Teller_Service.Common.Services
{
    public class FortuneServiceClient : IFortuneService
    {

        ILogger<FortuneServiceClient> _logger;
        IOptionsSnapshot<FortuneServiceConfig> _config;

        // Lab09 Start
        DiscoveryHttpClientHandler _handler;
        // Lab09 End
        public FortuneServiceClient(IDiscoveryClient client, IOptionsSnapshot<FortuneServiceConfig> config, ILogger<FortuneServiceClient> logger)
        {
            // Lab09 Start
            _handler = new DiscoveryHttpClientHandler(client);
            // Lab09 End
            _logger = logger;
            _config = config;
        }



        public async Task<List<Fortune>> AllFortunesAsync()
        {
            return await HandleRequest<List<Fortune>>(_config.Value.AllFortunesURL());
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            return await HandleRequest<Fortune>(_config.Value.RandomFortuneURL());
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
            // Lab09 Start
            var client = new HttpClient(_handler, false);
            // Lab09 End
            return client;
        }
    }
}