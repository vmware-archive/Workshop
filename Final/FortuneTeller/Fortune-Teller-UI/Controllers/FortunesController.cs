
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Fortune_Teller_UI.Controllers
{
    public class FortunesController : Controller
    {
        ILogger<FortunesController> _logger;

        // Lab11 Start
        private FortuneServiceCommand _fortunes;
        public FortunesController(ILogger<FortunesController> logger, FortuneServiceCommand fortunes)
        {
            _logger = logger;
            _fortunes = fortunes;
        }
        // Lab11 End

        // Lab06 Start
        //private IFortuneService _fortunes;
        //public FortunesController(ILogger<FortunesController> logger, IFortuneService fortunes)
        //{
        //    _logger = logger;
        //    _fortunes = fortunes;
        //}
        // Lab06 End

        public IActionResult Index()
        {
            _logger?.LogDebug("Index");
            ViewData["MyFortune"] = HttpContext.Session.GetString("MyFortune");// Lab09
            return View();
        }

        // Lab10 Start
        [Authorize(Policy = "testgroup")] 
        // Lab10 End
        public async Task<IActionResult> RandomFortune()
        {
            _logger?.LogDebug("RandomFortune");

            // Lab06 Start
            var fortune = await _fortunes.RandomFortuneAsync();
            // Lab06 End

            HttpContext.Session.SetString("MyFortune", fortune.Text); // Lab09
            return View(fortune);

        }

        // Lab10 Start
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            await HttpContext.Session.CommitAsync();
            return RedirectToAction(nameof(FortunesController.Index), "Fortunes");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction(nameof(FortunesController.Index), "Fortunes");
        }

        public IActionResult Manage()
        {
            ViewData["Message"] = "Manage accounts using UAA or CF command line.";
            return View();
        }

        public IActionResult AccessDenied()
        {
            ViewData["Message"] = "Insufficient permissions.";
            return View();
        }
        // Lab10 End

        public IActionResult Error()
        {
            return View();
        }

    }
}
