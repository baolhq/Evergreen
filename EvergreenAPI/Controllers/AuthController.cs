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
            var acc = _accountRepository.Login(account);

            if (acc != null)
            {
                return new JsonResult(acc);
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromBody] AccountDTO account)
        {
            if (account == null) return BadRequest();

            if (_accountRepository.Register(account)) return Ok();
            else return BadRequest("An error occured, please contact admin.");
        }
    }
}