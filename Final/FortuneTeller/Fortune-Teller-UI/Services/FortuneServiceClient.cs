
using System.Threading.Tasks;
using System.Collections.Generic;
using Pivotal.Discovery.Client;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Discovery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Fortune_Teller_UI.Services
{
    public class FortuneServiceClient : IFortuneService
    {

        ILogger<FortuneServiceClient> _logger;
        IOptionsSnapshot<FortuneServiceConfig> _config;

        private FortuneServiceConfig Config
        {
            get
            {
                return _config.Value;
            }
        }

        // Lab08 Start
        DiscoveryHttpClientHandler _handler;
        // Lab08 End

        // Lab10 Start
        IHttpContextAccessor _reqContext;
        // Lab10 End

        public FortuneServiceClient(IDiscoveryClient client,
            IOptionsSnapshot<FortuneServiceConfig> config, 
            ILogger<FortuneServiceClient> logger,
            // Lab10 Start
            IHttpContextAccessor context = null)
            // Lab10 End
        {
            // Lab08 Start
            _handler = new DiscoveryHttpClientHandler(client);
            // Lab08 End

            // Lab10 Start
            _reqContext = context;
            // Lab10 End

            _logger = logger;
            _config = config;
  
        }



        public async Task<List<Fortune>> AllFortunesAsync()
        {
            return await HandleRequest<List<Fortune>>(Config.AllFortunesURL);
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            return await HandleRequest<Fortune>(Config.RandomFortuneURL);
        }

        private async Task<T> HandleRequest<T>(string url) where T : class
        {
            _logger?.LogDebug("FortuneService call: {url}", url);
            try
            {
                using (var client = await GetClientAsync())
                {
                    var stream = await client.GetStreamAsync(url);
                    var result = Deserialize<T>(stream);
                    _logger?.LogDebug("FortuneService returned: {result}", result);
                    return result;
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("FortuneService exception: {0}", e);
                throw;
            }
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

        private async Task<HttpClient> GetClientAsync()
        {
            // Lab08 Start
            var client = new HttpClient(_handler, false);
            // Lab08 End

            // Lab10 Start
            if (_reqContext != null)
            {
                var token = await _reqContext.HttpContext.GetTokenAsync("access_token");

                _logger?.LogDebug("GetClientAsync access token: {token}", token);

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            // Lab10 End

            return client;
        }
    }
}