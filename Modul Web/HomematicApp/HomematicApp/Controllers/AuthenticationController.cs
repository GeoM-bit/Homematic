using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace HomematicApp.Controllers
{
    public class AuthenticationController : Controller
    {
        
        private readonly IAuthenticationRepository authenticationRepository;
        private readonly IMapper mapper;
        public AuthenticationController(IAuthenticationRepository _authenticationRepository, IMapper _mapper)
        {
            authenticationRepository = _authenticationRepository;
            mapper = _mapper;
        }

        public IActionResult Register()
        {
            var RegisterFailed = Request.Query["RegisterFailed"].ToString();
            if (!string.IsNullOrEmpty(RegisterFailed))
            {
                ViewBag.RegisterFailed = bool.Parse(RegisterFailed);
            }
            return View();
        }

        public async Task<IActionResult> RegisterUser(UserModel userModel)
        {
            User user = mapper.Map<User>(userModel);
            bool result = await authenticationRepository.Register(user);
            if (result)
            {
                return RedirectToAction("Login", new {RegisterSuccess=true});
            }
            else
            {
                return RedirectToAction("Register", new { RegisterFailed = true });
            }
        }

        public IActionResult Login()
        {
            var RegisterSuccess = Request.Query["RegisterSuccess"].ToString();
            if(!string.IsNullOrEmpty(RegisterSuccess)) {
                ViewBag.RegisterSuccess = bool.Parse(RegisterSuccess);
            }
            var LoginFailed = Request.Query["LoginFailed"].ToString();
            if (!string.IsNullOrEmpty(LoginFailed))
            {
                ViewBag.LoginFailed = bool.Parse(LoginFailed);
            }
            return View();
        }
        public async Task<IActionResult> LoginUser(LoginModel loginModel)
        {
            LoginUser loginUser = mapper.Map<LoginUser>(loginModel);
            bool result = await authenticationRepository.Login(loginUser);

            if (result)
            {
                return RedirectToAction("Login");  //redirect to user/admin homepage when done
            }
            else
            {
                return RedirectToAction("Login", new {LoginFailed = true});
            }
        }
        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public async Task<IActionResult> ForgotPassword(ResetPasswordEmail resetPasswordEmail)
        {
            if (resetPasswordEmail.Email != null)
            {
                bool result = await authenticationRepository.ForgotPassword(resetPasswordEmail.Email);
            }
            return View();
        }

    }
}
