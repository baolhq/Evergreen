﻿using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserRepository(AppDbContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }






        public bool DeleteUser(int id)
        {
            var user = _context.Accounts.SingleOrDefault(u => u.AccountId == id);
            if (user == null)
            {
                return false;
            }
            user.Status = false;
            _context.Remove(user);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Something is wrong when trying to remove user!");
            }

            return true;
        }

        public Account GetUser(int Id)
        {
            var user = _context.Accounts.FirstOrDefault(u => u.AccountId == Id);
            if (user == null) return null;

            return user;
        }

        public ICollection<Account> GetUsers()
        {
            var users = _context.Accounts.ToList();
            return users;
        }

        public bool CreateUser(UserDTO user)
        {
            if (_context.Accounts.Any(u => u.Email == user.Email))
            {
                return false;
            }
            var password = user.Password;
            var confirmPassword = user.ConfirmPassword;
            if (password != confirmPassword)
            {
                return false;
            }
            if (password.Length < 6)
            {
                return false;
            }

            CreatePasswordHash(user.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var member = new Account
            {
                Username = user.Username,
                FullName = user.FullName,
                Password = password,
                ConfirmPassword = user.ConfirmPassword,
                Email = user.Email,
                Professions = user.Professions,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = user.Role != string.Empty ? user.Role : "User",
                Token = GenerateToken(user.Email, user.Role),
                VerifiedAt = DateTime.Now
            };

            _context.Accounts.Add(member);
            _context.SaveChanges();

            return true;
        }








        public bool UpdateUser(Account userDTO, int id)
        {
            var user = _context.Accounts.SingleOrDefault(f => f.AccountId == id);
            if (user == null)
            {
                return false;
            }

            user.Email = userDTO.Email;
            user.Username = userDTO.Username;
            user.Role = userDTO.Role;
            user.Professions = userDTO.Professions;
            user.PhoneNumber = userDTO.PhoneNumber;
            _context.Update(user);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }








        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }








        private string GenerateToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.Now;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        }),

                Expires = now.AddDays(Convert.ToInt32(1)),
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
