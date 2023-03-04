using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Services;
using EvergreenAPI.Services.EmailService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using static EvergreenAPI.Services.EmailService.EmailService;

namespace EvergreenAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;

        public AccountRepository(ITokenService tokenService, IConfiguration config, AppDbContext context)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
        }
        public Account GetAccount(AccountDTO account)
        {
            return _context.Accounts.Where(x => x.Password == account.Password && x.Email == account.Email).FirstOrDefault();
        }

        public Account Login(LoginDTO account)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Password == account.Password && x.Email == account.Email);
            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(account.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            if (user.VerifiedAt == null)
            {
                return null;
            }


            var validUser = _context.Accounts
                .Where(x => x.Password == account.Password && x.Email == account.Email)
                .FirstOrDefault();

            if (validUser != null)
            {
                validUser.Token = GenerateToken(validUser.Email, validUser.Role);
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
            string userRole = "User";
            var found = _context.Accounts.Any(a => a.Email == accountDto.Email);
            if (found)
            {
                return false;
            }
            var password = accountDto.Password;
            var confirmpassword = accountDto.ConfirmPassword;
            if (password != confirmpassword)
            {
                return false;
            }
            if (password.Length < 6)
            {
                return false;
            }


            CreatePasswordHash(accountDto.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);


            var account = new Account()
            {
                Username = accountDto.Username,
                Email = accountDto.Email,
                Password = accountDto.Password,
                ConfirmPassword = accountDto.ConfirmPassword,
                FullName = accountDto.FullName,
                PhoneNumber = accountDto.PhoneNumber,
                Professions = accountDto.Professions,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User",
                Token = GenerateToken(accountDto.Email.ToString(), userRole.ToString())

            };


            #region Add Email Template
            try
            {
                var builder = new BodyBuilder();
                //Giao thuc IO Truyen file
                using (StreamReader SourceReader = System.IO.File.OpenText($"{_webHostEnvironment.WebRootPath}Templates/VerifyAccountTemplate.html"))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                // replace chữ trong indexs
                string htmlBody = builder.HtmlBody.Replace("Welcome!", $"Welcome {account.Email}!")
                .Replace("#Token", account.Token)
                .Replace("#memberEmail", account.Email);
                string messagebody = string.Format("{0}", htmlBody);

                #region Send Verification Mail To User
                try
                {
                    var mailContent1 = new MailContent();
                    mailContent1.To = "art.gulgowski@ethereal.email"; //temp email
                    mailContent1.Subject = "Welcome To Evergreen!";
                    mailContent1.Body = messagebody;
                    _emailService.SendMail(mailContent1);
                }
                catch (System.ArgumentNullException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion
            }
            catch (Exception e)
            {
                return false;
            }

            #endregion

            _context.Accounts.Add(account);
            _context.SaveChanges();
            return true;
        }



        public Account Verify(string token)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Token == token);
            if(user == null)
            {
                return null;
            }
            if(user.VerifiedAt != null)
            {
                return null;
            }
            user.VerifiedAt = DateTime.Now;

            _context.SaveChanges();
            return user;

        }


        public Account ForgotPassword(string email)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            user.PasswordResetToken = GenerateToken(email, "User");
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _context.SaveChanges();

            #region Add Email Template
            try
            {
                var builder = new BodyBuilder();
                //Giao thuc IO Truyen file
                using (StreamReader SourceReader = System.IO.File.OpenText($"{_webHostEnvironment.WebRootPath}Templates/ResetPasswordTemplate.html"))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }

                // replace chữ trong indexs
                string htmlBody = builder.HtmlBody.Replace("Welcome!", $"Welcome {user.Email}!")
                .Replace("We're excited to have you get started. First, you need to confirm your account. Just press the button below.", "RESET PASSWORD REQUEST!")
                .Replace("Confirm Account", "Reset Password")
                .Replace("#Token", user.PasswordResetToken);
                string messagebody = string.Format("{0}", htmlBody);

                #region Send Verification Mail To User
                try
                {
                    var mailContent1 = new MailContent();
                    mailContent1.To = "art.gulgowski@ethereal.email"; //temp email
                    mailContent1.Subject = "Reset Password!";
                    mailContent1.Body = messagebody;
                    _emailService.SendMail(mailContent1);
                }
                catch (System.ArgumentNullException)
                {
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
                #endregion
            }
            catch (Exception e)
            {
                return null;
            }
            #endregion

            return user;

        }


        public bool ResetPassword(ResetPasswordDTO request)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.PasswordResetToken == request.Token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return false;
            }
            var password = request.Password;
            var confirmpassword = request.ConfirmPassword;
            if(password != confirmpassword)
            {
                return false;
            }

            if(password.Length < 6)
            {
                return false;
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            _context.SaveChanges();
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


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
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
