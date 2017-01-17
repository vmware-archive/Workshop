
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fortune_Teller_Service.Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Fortune_Teller_UI.Controllers
{
    public class FortunesController : Controller
    {
        ILogger<FortunesController> _logger;

        public FortunesController(ILogger<FortunesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger?.LogDebug("Index");
            ViewData["MyFortune"] = HttpContext.Session.GetString("MyFortune");
            return View();
        }

        public async Task<IActionResult> RandomFortune()
        {
            _logger?.LogDebug("RandomFortune");

            var fortune = await Task.Run(() => new Fortune() { Id = 1, Text = "Hello from FortuneController UI!" });
            HttpContext.Session.SetString("MyFortune", fortune.Text);
            return View(fortune);
    
        }

    }
}
