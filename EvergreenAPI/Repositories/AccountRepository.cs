using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace EvergreenAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AccountRepository(ITokenService tokenService, IConfiguration config, AppDbContext context)
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

        public Account Login(AccountDTO account)
        {
            if (string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password))
            {
                return null;
            }
            var validUser = GetAccount(account);

            if (validUser != null)
            {
                validUser.Token = GenerateToken(validUser.Username, validUser.Role);
                _context.Accounts.Update(validUser);
                _context.SaveChanges();
                return validUser;
            }
            else
            {
                return null;
            }
        }

        public bool Register(AccountDTO accountDto)
        {
            var account = new Account()
            {
                Username = accountDto.Username,
                Password = accountDto.Password,
                Role = "User",
                Token = GenerateToken(accountDto.Username, "User")
            };

            var found = _context.Accounts.Any(a => a.Username == account.Username);
            if (found) return false;

            _context.Accounts.Add(account);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        public string GenerateToken(string username, string role, int expireMinutes = 30)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
