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

        public UserController(IUserRepository UserRepository, IMapper mapper)
        {
            _UserRepository = UserRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUsers()
        {
            var Users = _mapper.Map<List<Account>>(_UserRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Users);
        }

        [HttpGet("{email}")]
        [Authorize (Roles = "User,Admin")]
        public IActionResult GetUser(string email)
        {
            
            if (!_UserRepository.UserExist(email))
                return BadRequest($"User '{email}' is not exists!!");


            var Users = _mapper.Map<Account>(_UserRepository.GetUser(email));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser([FromBody] UserDTO UserCreate)
        {
            if (UserCreate == null)
                return BadRequest(ModelState);

            var User = _UserRepository.GetUsers()
                .Where(c => c.Email.Trim().ToUpper() == UserCreate.Email.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (User != null)
            {
                ModelState.AddModelError("", "It is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserMap = _mapper.Map<UserDTO>(UserCreate);

            if (!_UserRepository.CreateUser(UserMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create Success");
        }


        [HttpPut("{email}")]
        [Authorize(Roles = "User")]
        public IActionResult UpdateUser(string email, [FromBody] UserDTO updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);

            if (email != updatedUser.Email)
                return BadRequest(ModelState);

            if (!_UserRepository.UserExist(email))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var UserMap = _mapper.Map<UserDTO>(updatedUser);

            if (!_UserRepository.UpdateUser(UserMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Updated Success");
        }



        [HttpDelete("{email}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(string email)
        {
            if (!_UserRepository.UserExist(email))
                return NotFound();

            var UserToDelete = _UserRepository.GetUser(email);

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
