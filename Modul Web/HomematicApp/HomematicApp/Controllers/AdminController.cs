using Microsoft.AspNetCore.Mvc;

namespace HomematicApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
