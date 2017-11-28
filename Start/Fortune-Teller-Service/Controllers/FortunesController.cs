
using Fortune_Teller_Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fortune_Teller_Service.Controllers
{
    [Route("api/[controller]")]
    public class FortunesController : Controller
    {
        ILogger<FortunesController> _logger;


        public FortunesController(ILogger<FortunesController> logger)
        {
            _logger = logger;
        }


        // GET: api/fortunes/all
        [HttpGet("all")]
        public async Task<List<Fortune>> AllFortunesAsync()
        {
            _logger?.LogDebug("AllFortunesAsync");
            return await Task.FromResult(
                new List<Fortune>() { new Fortune() { Id = 1, Text = "Hello from FortuneController Web API!" } });

        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public async Task<Fortune> RandomFortuneAsync()
        {
            _logger?.LogDebug("RandomFortuneAsync");
            return (await AllFortunesAsync())[0];
        }
    }
}
