using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Services;
using EvergreenAPI.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static EvergreenAPI.Services.EmailService.EmailService;

namespace EvergreenAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AccountRepository(ITokenService tokenService, IConfiguration config, AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
            _emailService = emailService;
        }
        public Account GetAccount(AccountDTO account)
        {
            return _context.Accounts.FirstOrDefault(x => x.Password == account.Password && x.Email == account.Email);
        }

        public Account Login(LoginDTO account)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Password == account.Password && x.Email == account.Email && !x.IsBlocked);

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
                .FirstOrDefault(x => x.Password == account.Password && x.Email == account.Email);

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





        public async Task<bool> Register(AccountDTO accountDto)
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
                Token = GenerateToken(accountDto.Email.ToString(), userRole.ToString()),
                Chat = "AI: Hello, how can I help you today?",
            };


            #region Add Email Template
            try
            {

                #region Send Verification Mail To User
                try
                {
                    var mailContent1 = new MailContent();
                    mailContent1.To = account.Email; //temp email
                    mailContent1.Subject = "Welcome To Evergreen!";
                    mailContent1.Body = account.Token.ToString();
                    await _emailService.SendMail(mailContent1);
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
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<Account> Verify(string token)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Token == token);
            if (user == null)
            {
                return null;
            }
            if (user.VerifiedAt != null)
            {
                return null;
            }
            user.VerifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return user;

        }


        public async Task<Account> ForgotPassword(string email)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            
            user.PasswordResetToken = GenerateToken(email, "User");
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            #region Add Email Template
            try
            {


                #region Send Verification Mail To User
                try
                {
                    var mailContent1 = new MailContent();
                    mailContent1.To = user.Email; //temp email
                    mailContent1.Subject = "Reset Password!";
                    mailContent1.Body = user.PasswordResetToken.ToString();
                    await _emailService.SendMail(mailContent1);
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


        public async Task<bool> ResetPassword(ResetPasswordDTO request)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.PasswordResetToken == request.Token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return false;
            }
            var password = request.Password;
            var confirmpassword = request.ConfirmPassword;
            if (password != confirmpassword)
            {
                return false;
            }

            if (password.Length < 7)
            {
                return false;
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
            user.Password = password;
            await _context.SaveChangesAsync();
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
