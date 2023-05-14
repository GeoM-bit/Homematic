using Microsoft.AspNetCore.Mvc;

namespace HomematicApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
