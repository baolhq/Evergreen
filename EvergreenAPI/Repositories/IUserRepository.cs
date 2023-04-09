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

        Account GetUser(int Id);

        bool DeleteUser(int id);

        bool UpdateUser(AccountDTO user, int id);

        ICollection<Account> GetUsers();
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        List<Account> Search(string search);

    }
}
