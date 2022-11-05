using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using EvergreenAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AuthController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] AccountDTO account)
        {
            var token = _accountRepository.Login(account);

            if (token != null)
            {
                HttpContext.Session.SetString("Token", token);
                return new JsonResult(token);
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(Account account)
        {
            if (account == null) return BadRequest();

            if (_accountRepository.Register(account)) return Ok();
            else return BadRequest("An error occured, please contact admin");
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}
