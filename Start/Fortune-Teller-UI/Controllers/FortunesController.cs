
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

            var fortune = await Task.FromResult(new Fortune() { Id = 1, Text = "Hello from FortuneController UI!" });
            HttpContext.Session.SetString("MyFortune", fortune.Text); 
            return View(fortune);

        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            await HttpContext.Session.CommitAsync();
            return RedirectToAction(nameof(FortunesController.Index), "Fortunes");
        }

        [HttpGet]
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

        public IActionResult Error()
        {
            return View();
        }

    }
}
