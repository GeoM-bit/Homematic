﻿using HomematicApp.Models;

namespace HomematicApp.Abstractions
{
    public interface IAuthenticationRepository
    {
        Task<bool> Register(User user);
        Task<string> Login(User user);
        Task<bool> Logout();
        Task<bool> ResetPassword(string deviceId, string newPassword);
    }
}
