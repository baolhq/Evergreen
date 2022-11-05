using EvergreenAPI.DTO;
using EvergreenAPI.Models;

namespace EvergreenAPI.Services
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, Account account);
        bool ValidateToken(string key, string issuer, string token);
    }
}
