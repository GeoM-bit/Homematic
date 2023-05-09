namespace HomematicApp.Domain.Abstractions
{
    public interface IHashService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
