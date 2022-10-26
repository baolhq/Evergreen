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
        void SaveUser(Account user);

        Account GetUserById(int id);

        void DeleteUser(Account user);

        void UpdateUser(Account user);

        List<Account> GetUsers();

        Account GetUserByEmail(string email);

    }
}
