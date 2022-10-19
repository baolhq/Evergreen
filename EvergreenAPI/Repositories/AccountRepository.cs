using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly List<AccountDTO> accounts = new List<AccountDTO>();

        public AccountRepository()
        {
            accounts.Add(new AccountDTO
            {
                Username = "joydipkanjilal",
                Password = "joydip123",
                Role = "user"
            });
            accounts.Add(new AccountDTO
            {
                Username = "michaelsanders",
                Password = "michael321",
                Role = "user"
            });
            accounts.Add(new AccountDTO
            {
                Username = "stephensmith",
                Password = "stephen123",
                Role = "admin"
            });
            accounts.Add(new AccountDTO
            {
                Username = "rodpaddock",
                Password = "rod123",
                Role = "professor"
            });
            accounts.Add(new AccountDTO
            {
                Username = "rexwills",
                Password = "rex321",
                Role = "professor"
            });
        }
        public AccountDTO GetAccount(AccountDTO account)
        {
            return accounts.Where(x => x.Username.ToLower() == account.Username.ToLower()
                && x.Password == account.Password).FirstOrDefault();
        }
    }
}
