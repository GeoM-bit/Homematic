using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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

        [Route("ResetPassword/{userEmail}/{userToken}", Name = "ResetPassword/{userEmail}/{userToken}")]
        public async Task<IActionResult> ResetPassword(string? userEmail, string? userToken, ResetPasswordModel resetPasswordModel)
        {
            if (userEmail != null && userToken != null && resetPasswordModel.NewPassword==null)
            {
                string? cookieToken = null;

                if (HttpContext.Request.Cookies.TryGetValue("resetPasswordToken", out cookieToken))
                {
                    bool result = await authenticationRepository.ConfirmResetPasswordRequest(userEmail, userToken, cookieToken);
                    return View();
                }

                ViewBag.TokenExpired = true;
                return View();
            }
            else if(resetPasswordModel.NewPassword != null)
            {
                bool result = await authenticationRepository.ResetPassword(userEmail, resetPasswordModel.NewPassword);
                if (result)
                {
                    ViewBag.ResetSuccess = true;
                }
                else
                {
                    ViewBag.ResetSuccess = false;
                }
                return View();
            }
            return View();
        }

        public async Task<IActionResult> ForgotPassword(ResetPasswordModel resetPasswordmodel)
        {
            if (resetPasswordmodel.Email != null)
            {
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                HttpContext.Response.Cookies.Append("resetPasswordToken", token, new CookieOptions { Expires = DateTime.Now.AddMinutes(10) });
                var resetPasswordLink = Url.Action("ResetPassword", "Authentication", new { userEmail = HttpUtility.UrlEncode(resetPasswordmodel.Email), userToken = HttpUtility.UrlEncode(token)}, protocol: Request.Scheme);
                bool result = await authenticationRepository.ForgotPassword(resetPasswordmodel.Email, resetPasswordLink);
                if (result)
                {
                    ViewBag.RequestSuccess = true;
                }
                else
                {
                    ViewBag.RequestSuccess = false;
                }
                return View();
            }
            return View();
        }

    }
}
