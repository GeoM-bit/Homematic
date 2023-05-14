using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;


namespace HomematicApp.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly HomematicContext _context;
        private readonly IHashService _hashService;
        private readonly IEmailSender _emailSender;
        private readonly ITemplateFillerService _templateFillerService;
        public AuthenticationRepository(HomematicContext context, IHashService hashService, IEmailSender emailSender, ITemplateFillerService templateFillerService)
        {
            _context = context;
            _hashService = hashService;
            _emailSender = emailSender;
            _templateFillerService = templateFillerService;
        }

        public async Task<bool> ForgotPassword(string email)
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
                    Link = "aha"
                };

                var body = await _templateFillerService.FillTemplate(pathToFile, model);
                await _emailSender.SendEmailAsync(email, "Reset Password", body);
                return true;
            }
            return false;
        }

        public async Task<bool> Login(LoginUser loginUser)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (dbUser != null)
            {
                if (_hashService.VerifyPassword(loginUser.Password, dbUser.Password))
                    return true;
                return false;
            }

            return false;
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(User user)
        {
            var existentDbuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.CNP==user.CNP || u.Device_Id==user.Device_Id);
            if(existentDbuser != null)
            {
                return false;
            }
            user.Password = _hashService.HashPassword(user.Password);
            user.Is_Admin = false;
            _context.Users.Add(user);
            
            return await _context.SaveChangesAsync() == 1;

        }

        public Task<bool> ResetPassword(string deviceId, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
