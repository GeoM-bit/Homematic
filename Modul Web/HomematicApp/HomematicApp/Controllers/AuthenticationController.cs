﻿using AutoMapper;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.Domain.Common;
using HomematicApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
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
            var RegisterSuccess = Request.Query["RegisterSuccess"].ToString();
            if (!string.IsNullOrEmpty(RegisterSuccess))
            {
                ViewBag.RegisterSuccess = bool.Parse(RegisterSuccess);
            }
            if (!string.IsNullOrEmpty(RegisterSuccess))
            {
                ViewBag.RegisterFailed = bool.Parse(RegisterSuccess);
            }
            return View();
        }

        [Authorize(Roles="ADMIN")]
        public async Task<IActionResult> RegisterUser(UserModel userModel)
        {
            User user = mapper.Map<User>(userModel);
            bool result = await authenticationRepository.Register(user);
            if (result)
            {
                return RedirectToAction("Register", new {RegisterSuccess=true});
            }
            else
            {
                return RedirectToAction("Register", new { RegisterSuccess = false });
            }
        }

        public IActionResult Login()
        {           
            var LoginFailed = Request.Query["LoginFailed"].ToString();
            if (!string.IsNullOrEmpty(LoginFailed))
            {
                ViewBag.LoginFailed = bool.Parse(LoginFailed);
            }
            if (HttpContext.Session.GetString("Token")!=null)
            {
                if (User.IsInRole(Roles.USER.ToString()))
                {
                    return RedirectToAction("ViewActions", "User");
                }
                else if (User.IsInRole(Roles.ADMIN.ToString()))
                {
                    return RedirectToAction("ViewActions", "User");
                }
                else
                {
                    HttpContext.Session.Remove("Token");
                    return RedirectToAction("Login", new { LoginFailed = true });
                }
            }
            return View();
        }
        public async Task<IActionResult> LoginUser(LoginModel loginModel)
        {
            LoginUser loginUser = mapper.Map<LoginUser>(loginModel);
            string? result = await authenticationRepository.Login(loginUser);

            if (result!=null)
            {
                HttpContext.Session.SetString("Token", result);
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Login", new {LoginFailed = true});
            }
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login");        
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

        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 401 || statusCode == 403 || statusCode == 404 || statusCode == 500)
                {
                    var viewName = statusCode.ToString();
                    return View(viewName);
                }
            }
            return View();
        }
    }
}
