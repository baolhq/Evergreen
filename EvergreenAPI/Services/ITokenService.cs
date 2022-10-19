using EvergreenAPI.DTO;

namespace EvergreenAPI.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, AccountDTO account);
        bool ValidateToken(string key, string issuer, string token);
    }
}
