using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public interface IAccountRepository
    {
        Account GetAccount(AccountDTO account);
        Account Login(LoginDTO account);
        Task<bool> Register(AccountDTO account);
        Task<Account> Verify(string token);
        Task<Account> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordDTO request);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
