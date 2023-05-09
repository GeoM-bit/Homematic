using HomematicApp.Context.Context;
using HomematicApp.Context.DbModels;
using HomematicApp.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace HomematicApp.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly HomematicContext _context;
        private readonly IHashService _hashService;
        public AuthenticationRepository(HomematicContext context, IHashService hashService)
        {
            _context = context;
            _hashService = hashService;
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
