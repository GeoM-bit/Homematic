using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomematicApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult ViewParameters()
        {
            return View();
        }
    }
}
