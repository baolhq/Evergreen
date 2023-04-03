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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public UserController(IUserRepository userRepository, IMapper mapper, AppDbContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(_mapper.Map<List<Account>>(users));
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUser(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(_mapper.Map<Account>(user));
        }

        [HttpPut("ManageRole")]
        public async Task<IActionResult> SetRole(RoleDTO roleDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == int.Parse(roleDto.AccountId));
            if (account == null) return NotFound($"Account {roleDto.AccountId} cannot be found");

            account.Role = roleDto.Role;
            await _context.SaveChangesAsync();

            return Ok(account);
        }


        [HttpPut("ManageStatus")]
        public async Task<IActionResult> SetBlocked(BlockedDTO blockedDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == int.Parse(blockedDto.AccountId));
            if (account == null) return NotFound($"Account {blockedDto.AccountId} cannot be found");

            account.Status = blockedDto.Status;
            await _context.SaveChangesAsync();


            return Ok(account);
        }


        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO user)

        {
            if (user == null)
                return BadRequest(ModelState);

            var userToCreate = _userRepository
                .GetUsers()
                .FirstOrDefault(c => c.Email.Trim().ToUpper() == user.Email.TrimEnd().ToUpper());

            if (userToCreate != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<UserDTO>(user);

            if (_userRepository.CreateUser(userMap))
            {
                return Ok("Create User Successfully");
            }

            ModelState.AddModelError("", "Something was wrong while saving");
            return StatusCode(500, ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] AccountUpdateDTO updatedUser)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                return BadRequest(ModelState);

            if (user.AccountId != updatedUser.AccountId)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.UpdateUser(updatedUser, id))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{email}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(id))
            {
                ModelState.AddModelError("", "Something was wrong when delete");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpGet("Search")]
        public ActionResult<List<Account>> Search(string search)
        {
            var list = _userRepository.Search(search);

            return Ok(list);
        }
    }
}