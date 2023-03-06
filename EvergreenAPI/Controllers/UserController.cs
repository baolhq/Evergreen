using AutoMapper;
using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserController(IUserRepository UserRepository, IMapper mapper, AppDbContext context)
        {
            _UserRepository = UserRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public IActionResult GetUsers()
        {
            var Users = _mapper.Map<List<Account>>(_UserRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Users);
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult GetUser(string username)
        {
            if (!_UserRepository.UserExist(username))
                return NotFound($"User '{username}' is not exists!!");

            var Users = _mapper.Map<Account>(_UserRepository.GetUser(username));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Users);
        }

        [HttpPut("ManageRole")]
        [AllowAnonymous]
        public async Task<IActionResult> SetRole(RoleDTO roleDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = _context.Accounts.Where(a => a.AccountId == int.Parse(roleDTO.AccountId)).FirstOrDefault();
            if (account == null) return NotFound($"Account {roleDTO.AccountId} cannot be found");

            account.Role = roleDTO.Role;
            await _context.SaveChangesAsync();

            return Ok(account);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] Account UserCreate)
        {
            if (UserCreate == null)
                return BadRequest(ModelState);

            var User = _UserRepository.GetUsers()
                .Where(c => c.Username.Trim().ToUpper() == UserCreate.Username.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (User != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserMap = _mapper.Map<Account>(UserCreate);

            if (!_UserRepository.SaveUser(UserMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }


        [HttpPut("{username}")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult UpdateUser(string username, [FromBody] Account updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (username != updatedUser.Username)
                return BadRequest(ModelState);

            if (!_UserRepository.UserExist(username))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserMap = _mapper.Map<Account>(updatedUser);

            if (!_UserRepository.UpdateUser(UserMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }



        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string username)
        {
            if (!_UserRepository.UserExist(username))
                return NotFound();

            var UserToDelete = _UserRepository.GetUser(username);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_UserRepository.DeleteUser(UserToDelete))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }
            return Ok("Delete Success");
        }
    }
}
