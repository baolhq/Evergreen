using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace EvergreenAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AccountRepository( ITokenService tokenService, IConfiguration config, AppDbContext context)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
        }
        public Account GetAccount(AccountDTO account)
        {
            return _context.Accounts.Where(x => x.Username.ToLower() == account.Username.ToLower()
                && x.Password == account.Password).FirstOrDefault();
        }

        public string Login(AccountDTO account)
        {
            if (string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password))
            {
                return null;
            }
            var validUser = GetAccount(account);

            if (validUser != null)
            {
                var generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), validUser);
                return generatedToken;
            }
            else
            {
                return null;
            }
        }

        public bool Register(Account account)
        {
            var found = _context.Accounts.Any(a => a.Email == account.Email);
            if (found) return false;

            _context.Accounts.Add(account);
            _context.SaveChanges();
            return true;
        }
    }
}
