
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fortune_Teller_Service.Common.Services;

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
            return View();
        }

        public async Task<IActionResult> RandomFortune()
        {
            // Lab06 Start
            var fortune = await _fortunes.RandomFortuneAsync();
            return View(fortune);
            // Lab06 End
        }

    }
}
