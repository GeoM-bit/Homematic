
using HomematicApp.Context.DbModels;

namespace HomematicApp.Domain.Abstractions
{
    public interface IAuthenticationRepository
    {
        Task<bool> Register(User user);
        Task<string?> Login(LoginUser loginUser);
        Task<bool> Logout();
        Task<bool> ForgotPassword(string email, string link);
        Task<bool> ResetPassword(string email, string newPassword);
        Task<bool> ConfirmResetPasswordRequest(string email, string token, string cookieToken);
        Task<string> GetRole(string email);
    }
}
