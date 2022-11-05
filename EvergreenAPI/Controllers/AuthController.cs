using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using EvergreenAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IAccountRepository _accountRepository;
        private string generatedToken = null;

        public AuthController(AppDbContext context, IConfiguration config, ITokenService tokenService, IAccountRepository accountRepository)
        {
            _context = context;
            _config = config;
            _tokenService = tokenService;
            _accountRepository = accountRepository;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IActionResult Login(AccountDTO account)
        {
            if (string.IsNullOrEmpty(account.Username) || string.IsNullOrEmpty(account.Password))
            {
                return BadRequest();
            }
            IActionResult response = Unauthorized();
            var validUser = GetAccount(account);

            if (validUser != null)
            {
                generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), validUser);
                if (generatedToken != null)
                {
                    HttpContext.Session.SetString("Token", generatedToken);
                    HttpContext.Session.SetString("AccountId", account.AccountId.ToString());
                    return new JsonResult(generatedToken);

                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private AccountDTO GetAccount(AccountDTO account)
        {
            // Write your code here to authenticate the user     
            return _accountRepository.GetAccount(account);
        }
    }
}
