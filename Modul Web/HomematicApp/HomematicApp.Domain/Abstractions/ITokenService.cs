namespace HomematicApp.Domain.Abstractions
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, string email, string role);
        bool ValidateToken(string key, string issuer, string token);
    }
}
