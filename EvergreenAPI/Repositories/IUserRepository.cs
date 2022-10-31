using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public interface IUserRepository
    {
        bool SaveUser(Account b);

        Account GetUserById(int id);

        bool DeleteUser(Account b);

        bool UpdateUser(Account b);

        ICollection<Account> GetUsers();
        bool UserExist(int id);
        bool Save();

        

    }
}
