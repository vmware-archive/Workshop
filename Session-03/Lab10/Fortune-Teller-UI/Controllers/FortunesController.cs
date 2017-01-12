
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fortune_Teller_Service.Common.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Fortune_Teller_UI.Controllers
{
    public class FortunesController : Controller
    {
        // Lab06 Start
        private IFortuneService _fortunes;
        public FortunesController(IFortuneService fortunes)
        {
            _fortunes = fortunes;
        }
        // Lab06 End

        public IActionResult Index()
        {
            ViewData["MyFortune"] = HttpContext.Session.GetString("MyFortune");// Lab10
            return View();
        }

        public async Task<IActionResult> RandomFortune()
        {
            // Lab06 Start
            var fortune = await _fortunes.RandomFortuneAsync();
            HttpContext.Session.SetString("MyFortune", fortune.Text); // Lab10
            return View(fortune);
            // Lab06 End
        }

    }
}
