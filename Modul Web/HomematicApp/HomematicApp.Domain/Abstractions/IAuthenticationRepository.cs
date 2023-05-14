
using HomematicApp.Context.DbModels;

namespace HomematicApp.Domain.Abstractions
{
    public interface IAuthenticationRepository
    {
        Task<bool> Register(User user);
        Task<bool> Login(LoginUser loginUser);
        Task<bool> Logout();
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(string deviceId, string newPassword);
    }
}
