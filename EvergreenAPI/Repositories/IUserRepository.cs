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
        bool CreateUser(UserDTO user);

        Account GetUser(string email);

        bool DeleteUser(Account user);

        bool UpdateUser(UserDTO user);

        ICollection<Account> GetUsers();
        bool UserExist(string email);
        bool Save();

        

    }
}
