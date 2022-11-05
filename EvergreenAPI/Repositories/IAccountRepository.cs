using EvergreenAPI.DTO;
using EvergreenAPI.Models;

namespace EvergreenAPI.Repositories
{
    public interface IAccountRepository
    {
        Account GetAccount(AccountDTO account);
        Account Login(AccountDTO account);
        bool Register(AccountDTO account);
    }
}
