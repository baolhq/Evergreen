using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool DeleteUser(Account user)
        {
            _context.Remove(user);
            return Save();
        }

        public Account GetUser(string email)
        {
            return _context.Accounts.Where(s => s.Email == email).FirstOrDefault();
        }

        public ICollection<Account> GetUsers()
        {
            return _context.Accounts.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateUser(UserDTO user)
        {
            _context.Add(user);
            return Save();
        }

        public bool UpdateUser(UserDTO user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExist(string email)
        {
            return _context.Accounts.Any(f => f.Email == email);
        }
    }
}
