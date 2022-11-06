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

        public bool DeleteUser(Account b)
        {
            _context.Remove(b);
            return Save();
        }

        public Account GetUserByEmail(string email)
        {
            return _context.Accounts.Where(a => a.Email == email).FirstOrDefault();
        }

        public Account GetUser(string username)
        {
            return _context.Accounts.Where(s => s.Username == username).FirstOrDefault();
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

        public bool SaveUser(Account b)
        {
            _context.Add(b);
            return Save();
        }

        public bool UpdateUser(Account b)
        {
            _context.Update(b);
            return Save();
        }

        public bool UserExist(string username)
        {
            return _context.Accounts.Any(f => f.Username == username);
        }
    }
}
