using AutoMapper;
using HomematicApp.Abstractions;
using HomematicApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomematicApp.Controllers
{
    public class AuthenticationController : Controller
    {
        
        private IAuthenticationRepository authenticationRepository;
        private IMapper mapper;
        public AuthenticationController(IAuthenticationRepository _authenticationRepository, IMapper _mapper)
        {
            authenticationRepository = _authenticationRepository;
            mapper = _mapper;
        }
        public IActionResult Register()
        {           
            return (IActionResult)View();
        }
        public async Task<IActionResult> RegisterUser(UserModel userModel)
        {
            User user = mapper.Map<User>(userModel);
            bool result = await authenticationRepository.Register(user);

            if (result)
            {
                return RedirectToAction("Login");
            }
            else
                return RedirectToAction("Register");

        }

        public IActionResult Login()
        {
            return (IActionResult)View();
        }

        public IActionResult Logout()
        {
            return (IActionResult)View();
        }

        public IActionResult ResetPassword()
        {
            return (IActionResult)View();
        }
    }
}
