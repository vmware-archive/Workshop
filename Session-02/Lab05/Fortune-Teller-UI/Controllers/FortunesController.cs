
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fortune_Teller_Service.Common.Services;

namespace Fortune_Teller_UI.Controllers
{
    public class FortunesController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RandomFortune()
        {
            var fortune = await Task.Run(() => new Fortune() { Id = 1, Text = "Hello from FortuneController UI!" } );
            return View(fortune);
        }

    }
}
