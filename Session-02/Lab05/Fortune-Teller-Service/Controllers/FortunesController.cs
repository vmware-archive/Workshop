
using Fortune_Teller_Service.Common.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fortune_Teller_Service.Controllers
{
    [Route("api/[controller]")]
    public class FortunesController : Controller, IFortuneService
    {

        // GET: api/fortunes/all
        [HttpGet("all")]
        public async Task<List<Fortune>> AllFortunesAsync()
        {
            return await Task.Run( () => new List<Fortune>() { new Fortune() { Id = 1, Text = "Hello from FortuneController Web API!" } });
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public async Task<Fortune> RandomFortuneAsync()
        {
            var all = await AllFortunesAsync();
            return all[0];
        }
    }
}
