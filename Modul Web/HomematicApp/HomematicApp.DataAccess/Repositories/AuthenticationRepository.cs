using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using HomematicApp.Domain.Common;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace HomematicApp.DataAccess.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly HomematicContext _context;
        private readonly IHashService _hashService;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateFillerService _templateFillerService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthenticationRepository(HomematicContext context, IHashService hashService, IEmailSender emailSender, ITemplateFillerService templateFillerService, ITokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _hashService = hashService;
            _emailSender = emailSender;
            _templateFillerService = templateFillerService;
            _tokenService = tokenService;   
            _configuration = configuration;
        }

        public async Task<bool> ConfirmResetPasswordRequest(string email, string token, string cookieToken)
        {
            email = HttpUtility.UrlDecode(email);
            token = HttpUtility.UrlDecode(token);
           
            var userFromDb = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFromDb != null && token != null && cookieToken!=null)
            {
                if (token == cookieToken)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> ForgotPassword(string email, string link)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (dbUser != null)
            {
                var pathToFile = Path.GetDirectoryName(Directory.GetCurrentDirectory())
            + Path.DirectorySeparatorChar.ToString()
            + "HomematicApp.Service"
            + Path.DirectorySeparatorChar.ToString()
            + "Services"
            + Path.DirectorySeparatorChar.ToString()
            + "EmailService"
            + Path.DirectorySeparatorChar.ToString()
            + "ResetPasswordTemplate.cshtml";

                object model = new
                {
                    FirstName = dbUser.First_Name,
                    LastName = dbUser.Last_Name,
                    Link = link
            };
                var body = await _templateFillerService.FillTemplate(pathToFile, model);
                await _emailSender.SendEmailAsync(email, "Reset Password", body);
                return true;
            }
            return false;
        }

        public async Task<string?> Login(LoginUser loginUser)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);
            if (dbUser != null)
            {
                if (dbUser.Device_Id.Length==16)
                {
                    if (_hashService.VerifyPassword(loginUser.Password, dbUser.Passwrd))
                    {
                        string role = GetRole(loginUser.Email).Result;
                        if (role == null)
                        {
                            return null;
                        }
                        string generatedToken = _tokenService.BuildToken(_configuration["Jwt:Key"].ToString(), _configuration["Jwt:Issuer"].ToString(), loginUser.Email, role);
                        if (generatedToken != null)
                        {
                            return generatedToken;
                        }
                        else
                            return null;
                    }
                }
                return "IMEI";
            }
            return null;
        }

        public async Task<bool> Register(User user)
        {
            var existentDbuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.CNP == user.CNP || u.Device_Id == user.Device_Id);
            if (existentDbuser != null)
            {
                return false;
            }
            user.Passwrd = _hashService.HashPassword(user.Passwrd);
            user.Is_Admin = false;
            _context.Users.Add(user);
            
            return await _context.SaveChangesAsync() == 1;

        }

        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            email = HttpUtility.UrlDecode(email);
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (dbUser != null)
            {
                dbUser.Passwrd = _hashService.HashPassword(newPassword);
                _context.Users.Update(dbUser);

                return await _context.SaveChangesAsync() == 1;
            }

            return false;
        }

        public async Task<string> GetRole(string email)
        {
            string result = "";
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (dbUser != null)
            {
                if (dbUser.Is_Admin)
                {
                    return Roles.ADMIN.ToString();
                }
                return Roles.USER.ToString();

            }
            else
                return null;
        }       
    }
}
