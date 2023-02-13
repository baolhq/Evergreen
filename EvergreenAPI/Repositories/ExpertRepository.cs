using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class ExpertRepository : IExpertRepository
    {
        private readonly AppDbContext _context;



        public ExpertRepository(AppDbContext context)
        {
            _context = context;
        }





        public bool DeleteExpert(Account b)
        {
            _context.Remove(b);
            return Save();
        }





        public Account GetExpertByEmail(string email)
        {
            return _context.Accounts.Where(a => a.Email == email).FirstOrDefault();
        }





        public Account GetExpert(string username)
        {
            return _context.Accounts.Where(s => s.Username == username).FirstOrDefault();
        }





        public ICollection<Account> GetExperts()
        {
            return _context.Accounts.ToList();
        }




        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }



        public bool SaveExpert(Account b)
        {
            _context.Add(b);
            return Save();
        }



        public bool UpdateExpert(Account b)
        {
            _context.Update(b);
            return Save();
        }




        public bool ExpertExist(string username)
        {
            return _context.Accounts.Any(f => f.Username == username);
        }
    }
}
