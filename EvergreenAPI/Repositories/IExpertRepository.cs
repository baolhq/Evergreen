using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IExpertRepository
    {
        bool SaveExpert(Account b);

        Account GetExpert(string username);

        bool DeleteExpert(Account b);

        bool UpdateExpert(Account b);

        ICollection<Account> GetExperts();
        bool ExpertExist(string username);
        bool Save();
    }
}
