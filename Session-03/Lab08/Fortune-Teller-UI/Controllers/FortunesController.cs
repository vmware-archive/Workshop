
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

        // Lab06 Start
        private IFortuneService _fortunes;
        public FortunesController(ILogger<FortunesController> logger, IFortuneService fortunes)
        {
            _logger = logger;
            _fortunes = fortunes;
        }
        // Lab06 End

        public IActionResult Index()
        {
            _logger?.LogDebug("Index");
            ViewData["MyFortune"] = HttpContext.Session.GetString("MyFortune");
            return View();
        }

        public async Task<IActionResult> RandomFortune()
        {
            _logger?.LogDebug("RandomFortune");

            // Lab06 Start
            var fortune = await _fortunes.RandomFortuneAsync();
            // Lab06 End
            HttpContext.Session.SetString("MyFortune", fortune.Text);
            return View(fortune);
  
        }

    }
}
