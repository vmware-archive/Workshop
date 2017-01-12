
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
        // Lab09 Start
        DiscoveryHttpClientHandler _handler;
        ILogger<FortuneServiceClient> _logger;
        IOptionsSnapshot<FortuneServiceConfig> _config;

        public FortuneServiceClient(IDiscoveryClient client, IOptionsSnapshot<FortuneServiceConfig> config, ILogger<FortuneServiceClient> logger)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _logger = logger;
            _config = config;
        }
        // Lab09 End


        public async Task<List<Fortune>> AllFortunesAsync()
        {
            // Lab09
            return await HandleRequest<List<Fortune>>(_config.Value.AllFortunesUrl);
            // Lab09
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            // Lab09
            return await HandleRequest<Fortune>(_config.Value.RandomFortuneUrl);
            // Lab09
        }


        // Lab09 Start
        private async Task<T> HandleRequest<T>(string url) where T: class
        {
            try
            {
                using (var client = GetClient())
                {
                    var stream = await client.GetStreamAsync(url);
                    return Deserialize<T>(stream);
                }
            } catch (Exception e)
            {
                _logger?.LogError("FortuneService exception: {0}", e);
            }
            return null;
        }


        private T Deserialize<T>(Stream stream) where T: class
        {
            try
            {
                using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (T) serializer.Deserialize(reader, typeof(T));
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
            var client = new HttpClient(_handler, false);
            return client;
        }
        // Lab09 End
    }
}
