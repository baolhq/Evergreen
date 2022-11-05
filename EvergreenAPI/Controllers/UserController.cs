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
    [Authorize (Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository UserRepository, IMapper mapper)
        {
            _UserRepository = UserRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUsers()
        {
            var Users = _mapper.Map<List<UserDTO>>(_UserRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Users);
        }

        [HttpGet("{UserId}")]
        [AllowAnonymous]
        public IActionResult GetUser(int UserId)
        {
            if (!_UserRepository.UserExist(UserId))
                return NotFound($"User Category '{UserId}' is not exists!!");

            var Users = _mapper.Map<UserDTO>(_UserRepository.GetUserById(UserId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Users);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO UserCreate)
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





        [HttpPut("{UserId}")]
        public IActionResult UpdateUser(int UserId, [FromBody] UserDTO updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (UserId != updatedUser.AccountId)
                return BadRequest(ModelState);

            if (!_UserRepository.UserExist(UserId))
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



        [HttpDelete("{UserId}")]
        public IActionResult DeleteUser(int UserId)
        {
            if (!_UserRepository.UserExist(UserId))
                return NotFound();

            var UserToDelete = _UserRepository.GetUserById(UserId);

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
