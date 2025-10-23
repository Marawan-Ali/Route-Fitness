using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class HomeController : Controller
    {
        // BaseURL/Home/Index
        public IActionResult Index()
        {
            return View();
        }
    }
}
