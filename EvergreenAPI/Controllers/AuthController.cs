using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using EvergreenAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

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
        public IActionResult Login([FromBody] LoginDTO account)
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

            if (_accountRepository.Register(account)) return Ok("Register Successfully.");
            else return BadRequest("An error occured, please contact admin.");
        }



        [Route("verify")]
        [HttpPost]
        public IActionResult Verify(string account)
        {
            var acc = _accountRepository.Verify(account);
            if (acc != null)
            {
                return Ok("User Verified.");
            }
            else
            {
                return BadRequest("An error occured, please contact admin.");
            }
        }






        [Route("forgot-password")]
        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var mail = _accountRepository.ForgotPassword(email);
            if(mail != null)
            {
                return Ok("You may now reset your password.");
            }
            else
            {
                return BadRequest("An error occured, please contact admin.");
            }
        }




       
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDTO request)
        {
            if (request == null) return BadRequest();

            if (_accountRepository.ResetPassword(request))
            {
                return Ok("Password Successfully Reset.");
            }
            else
            {
                return BadRequest("An error occured, please contact admin.");
            }
        }

    }
}