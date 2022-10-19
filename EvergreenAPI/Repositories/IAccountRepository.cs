using EvergreenAPI.DTO;
using EvergreenAPI.Models;

namespace EvergreenAPI.Repositories
{
    public interface IAccountRepository
    {
        AccountDTO GetAccount(AccountDTO account);
    }
}
