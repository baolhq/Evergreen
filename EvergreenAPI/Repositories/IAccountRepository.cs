using EvergreenAPI.DTO;
using EvergreenAPI.Models;

namespace EvergreenAPI.Repositories
{
    public interface IAccountRepository
    {
        Account GetAccount(AccountDTO account);
        Account Login(LoginDTO account);
        bool Register(AccountDTO account);
        Account Verify(string token);
        Account ForgotPassword(string email);
        bool ResetPassword(ResetPasswordDTO request);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
